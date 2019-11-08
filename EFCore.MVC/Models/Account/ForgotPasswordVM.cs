using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EFCore.MVC.Models
{
    public class ForgotPasswordVM
    {
        [Required]
        public string Email { get; set; }
    }
}
