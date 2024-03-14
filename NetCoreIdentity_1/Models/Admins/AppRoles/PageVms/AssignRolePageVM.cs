using NetCoreIdentity_1.Models.Admins.AppRoles.ResponseModels;
using NetCoreIdentity_1.Models.Entities;

namespace NetCoreIdentity_1.Models.Admins.AppRoles.PageVms
{
    public class AssignRolePageVM
    {
        public List<AppRoleResponseModel> Roles { get; set; }
        public int UserID { get; set; }
    }
}
