using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MultiLayerSignalRSample.Models {
    
    public class LoginModel {

        [Required]
        public string Name { get; set; }
    }
}