using Microsoft.AspNetCore.Identity;

namespace WebApiProject.Api.Reponsitories
{
    public interface ITokenRepository
    {
        string CreateJWTToken(IdentityUser identityUser, List<string> userRoles);
    }
}
