using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace JinderAPI.Providers
{
    public class SimpleAuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {

            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });
            int userId = -1;
            using (JinderDBEntities1 _db = new JinderDBEntities1())
            {
                JinderUser user = new JinderUser();
                foreach (JinderUser jinderUser in _db.JinderUsers) {
                    if (jinderUser.Username == context.UserName && jinderUser.Password == context.Password)
                        userId = jinderUser.JinderUserId;
                        user = jinderUser;
                }

                if (user == null)
                {
                    context.SetError("invalid_grant", "The user name or password is incorrect.");
                    return;
                }
            }
            

            var identity = new ClaimsIdentity(context.Options.AuthenticationType);
            identity.AddClaim(new Claim("id", userId.ToString()));
            
            context.Validated(identity);

        }
    }
}