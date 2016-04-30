using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DogDays.Models
{
    public class SignupViewModel
    {
        [Required(ErrorMessage = "You know who you are")]


        public string Name { get; set; }
    }
}