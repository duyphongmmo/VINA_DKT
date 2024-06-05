using System.ComponentModel.DataAnnotations;

namespace Login_Logout.Models
{
    public class LoginModelClass
    {

        [Required(ErrorMessage = "Please Enter the Employee code")]
        [Display(Name = "Enter Employee code:")]
        public string UserCode { get; set; }
        [Required(ErrorMessage = "Please Enter the Password")]
        [Display(Name = "Enter Password:")]
        public string PassWord { get; set; }
        public string Name { get; set; }


    }
    public class ForgotPassword
    {

        [Required(ErrorMessage = "Please Enter the Employee code")]
        [Display(Name = "Enter Employee code:")]
        public string UserCode { get; set; }
    }

}