using System.ComponentModel.DataAnnotations;

namespace NetCoreIdentity_1.Models.Admins.AppRoles.RequestModels
{
    public class CreateRoleRequestModel
    {
        [Required(ErrorMessage ="Rol ismi gereklidir")]
        public string RoleName { get; set; }

    }
}
