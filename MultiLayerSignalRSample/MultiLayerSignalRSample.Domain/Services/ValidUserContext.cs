using System.Security.Principal;

namespace MultiLayerSignalRSample.Domain.Services
{
    public class ValidUserContext
    {
        public IPrincipal Principal { get; set; }
        public UserWithRoles User { get; set; }

        public bool IsValid()
        {
            return Principal != null;
        }
    }
}
