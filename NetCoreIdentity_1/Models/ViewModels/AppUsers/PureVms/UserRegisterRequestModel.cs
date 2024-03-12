using System.ComponentModel.DataAnnotations;

namespace NetCoreIdentity_1.Models.ViewModels.AppUsers.PureVms
{
    public class UserRegisterRequestModel
    {
        [Required(ErrorMessage ="{0}  girilmesi zorunludur")]
        [Display(Name ="Kullanıcı ismi")]
        public string UserName { get; set; }

        [EmailAddress(ErrorMessage ="Email formatında giriş yapınız")]
        public string Email { get; set; }

        [Required(ErrorMessage ="{0} zorunludur")]
        [MinLength(3,ErrorMessage ="Minimum {0} karakter girilmesi gereklidir")]
        public string Password { get; set; }

        [Compare("Password",ErrorMessage ="Parolalar uyusmuyor")]
        public string ConfirmPassword { get; set; }



    }
}
