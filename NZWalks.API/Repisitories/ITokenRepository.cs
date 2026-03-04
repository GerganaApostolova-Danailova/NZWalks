using Microsoft.AspNetCore.Identity;

namespace NZWalks.API.Repisitories;

public interface ITokenRepository
{
    string CreateJWTToken(IdentityUser user, List<string> roles);
}
