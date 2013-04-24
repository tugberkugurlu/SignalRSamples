using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using MultiLayerSignalRSample.Domain.Entities;
using MultiLayerSignalRSample.Domain.Entities.Core;

namespace MultiLayerSignalRSample.Domain.Services
{
    public class MembershipService : IMembershipService 
    {
        private readonly IEntityRepository<User> _userRepository;
        private readonly IEntityRepository<Role> _roleRepository;
        private readonly IEntityRepository<UserInRole> _userInRoleRepository;

        public MembershipService(
            IEntityRepository<User> userRepository,
            IEntityRepository<Role> roleRepository,
            IEntityRepository<UserInRole> userInRoleRepository)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _userInRoleRepository = userInRoleRepository;
        }

        public ValidUserContext ValidateUser(string username, string password)
        {
            ValidUserContext userCtx = new ValidUserContext();
            User user = _userRepository.GetSingleByUsername(username);
            if (user != null && isUserValid(user, password))
            {
                IEnumerable<Role> userRoles = GetUserRoles(user.Id);
                userCtx.User = new UserWithRoles()
                {
                    User = user,
                    Roles = userRoles
                };

                GenericIdentity identity = new GenericIdentity(user.Name);
                userCtx.Principal = new GenericPrincipal(identity, userRoles.Select(x => x.Name).ToArray());
            }

            return userCtx;
        }

        public OperationResult<UserWithRoles> CreateUser(string username, string email, string password)
        {
            return CreateUser(username, email, password, roles: null);
        }

        public OperationResult<UserWithRoles> CreateUser(string username, string email, string password, string role)
        {
            return CreateUser(username, email, password, roles: new[] { role });
        }

        public OperationResult<UserWithRoles> CreateUser(string username, string email, string password, string[] roles)
        {
            bool existingUser = _userRepository.GetAll().Any(userEntity => userEntity.Name == username);
            if (existingUser)
            {
                return new OperationResult<UserWithRoles>(false);
            }

            string passwordSalt = GenerateSalt();
            User user = new User()
            {
                Name = username,
                Salt = passwordSalt,
                Email = email,
                IsLocked = false,
                HashedPassword = EncryptPassword(password, passwordSalt),
                CreatedOn = DateTimeOffset.Now
            };

            _userRepository.Add(user);
            _userRepository.Save();

            if (roles != null && roles.Any())
            {
                foreach (string roleName in roles)
                {
                    AddUserToRole(user, roleName);
                }
            }

            return new OperationResult<UserWithRoles>(true)
            {
                Entity = GetUserWithRoles(user)
            };
        }

        public UserWithRoles UpdateUser(User user, string username, string email)
        {
            user.Name = username;
            user.Email = email;
            user.LastUpdatedOn = DateTimeOffset.Now;

            _userRepository.Edit(user);
            _userRepository.Save();

            return GetUserWithRoles(user);
        }

        public bool ChangePassword(string username, string oldPassword, string newPassword)
        {
            User user = _userRepository.GetSingleByUsername(username);
            if (user != null && isPasswordValid(user, oldPassword))
            {
                user.HashedPassword = EncryptPassword(newPassword, user.Salt);
                _userRepository.Edit(user);
                _userRepository.Save();

                return true;
            }

            return false;
        }

        public bool AddToRole(string username, string role)
        {
            User user = _userRepository.GetSingleByUsername(username);
            if (user != null)
            {
                AddUserToRole(user, role);
                return true;
            }

            return false;
        }

        public bool AddToRole(int userId, string role)
        {
            User user = _userRepository.GetSingle(userId);
            if (user != null)
            {
                AddUserToRole(user, role);
                return true;
            }

            return false;
        }

        public bool RemoveFromRole(string username, string role)
        {
            User user = _userRepository.GetSingleByUsername(username);
            Role roleEntity = _roleRepository.GetSingleByRoleName(role);

            if (user != null && roleEntity != null)
            {
                UserInRole userInRole = _userInRoleRepository.GetAll()
                    .FirstOrDefault(x => x.RoleId == roleEntity.Id && x.UserId == user.Id);

                if (userInRole != null)
                {
                    _userInRoleRepository.Delete(userInRole);
                    _userInRoleRepository.Save();
                }
            }

            return false;
        }

        public IEnumerable<Role> GetRoles()
        {
            return _roleRepository.GetAll();
        }

        public Role GetRole(int id)
        {
            return _roleRepository.GetSingle(id);
        }

        public Role GetRole(string name)
        {
            return _roleRepository.GetSingleByRoleName(name);
        }

        public PaginatedList<UserWithRoles> GetUsers(int pageIndex, int pageSize)
        {
            PaginatedList<User> users = _userRepository.Paginate<int>(pageIndex, pageSize, x => x.Id);
            IQueryable<UserWithRoles> userWithRolesCollection = users.Select(user => new UserWithRoles()
            {
                User = user,
                Roles = GetUserRoles(user.Id)
            }).AsQueryable();

            return new PaginatedList<UserWithRoles>(
                userWithRolesCollection, users.PageIndex, users.PageSize, users.TotalCount);
        }

        public UserWithRoles GetUser(int id)
        {
            User user = _userRepository.GetSingle(id);
            return GetUserWithRoles(user);
        }

        public UserWithRoles GetUser(string name)
        {
            User user = _userRepository.GetSingleByUsername(name);
            return GetUserWithRoles(user);
        }

        // Private helpers

        private IEnumerable<Role> GetUserRoles(int userId)
        {
            UserInRole[] userInRoles = _userInRoleRepository.FindBy(x => x.UserId == userId).ToArray();
            if (userInRoles.Any())
            {
                int[] userRoleKeys = userInRoles.Select(x => x.RoleId).ToArray();
                Role[] userRoles = _roleRepository.FindBy(x => userRoleKeys.Contains(x.Id)).ToArray();

                return userRoles;
            }

            return Enumerable.Empty<Role>();
        }

        private UserWithRoles GetUserWithRoles(User user)
        {
            if (user != null)
            {
                IEnumerable<Role> userRoles = GetUserRoles(user.Id);
                return new UserWithRoles()
                {
                    User = user,
                    Roles = userRoles
                };
            }

            return null;
        }

        private void AddUserToRole(User user, string roleName)
        {
            Role role = _roleRepository.GetAll().FirstOrDefault(roleEntity => roleEntity.Name == roleName);
            if (role == null)
            {
                Role tempRole = new Role()
                {
                    Name = roleName
                };

                _roleRepository.Add(tempRole);
                _roleRepository.Save();
                role = tempRole;
            }

            UserInRole userInRole = new UserInRole()
            {
                RoleId = role.Id,
                UserId = user.Id
            };

            _userInRoleRepository.Add(userInRole);
            _userInRoleRepository.Save();
        }

        private bool isUserValid(User user, string password)
        {
            if (isPasswordValid(user, password))
            {
                return !user.IsLocked;
            }

            return false;
        }

        private bool isPasswordValid(User user, string password)
        {
            return string.Equals(EncryptPassword(password, user.Salt), user.HashedPassword);
        }

        private static string GenerateSalt()
        {
            byte[] data = new byte[0x10];
            using (RNGCryptoServiceProvider cryptoServiceProvider = new RNGCryptoServiceProvider())
            {
                cryptoServiceProvider.GetBytes(data);
                return Convert.ToBase64String(data);
            }
        }

        public string EncryptPassword(string password, string salt)
        {
            if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentNullException("password");
            }

            if (string.IsNullOrEmpty(salt))
            {
                throw new ArgumentNullException("salt");
            }

            using (SHA256 sha256 = SHA256.Create())
            {
                string saltedPassword = string.Format("{0}{1}", salt, password);
                byte[] saltedPasswordAsBytes = Encoding.UTF8.GetBytes(saltedPassword);

                return Convert.ToBase64String(sha256.ComputeHash(saltedPasswordAsBytes));
            }
        }
    }
}