using NUnit.Framework;
using ServiceModel;
using ServiceStack;
using ServiceStack.Auth;
using ServiceStack.Testing;
using ServiceStack.Text;

namespace Tests.Client
{
    [TestFixture]
    public class JwtTasks
    {
        [Test]
        public void Create_JWT()
        {
            using (new BasicAppHost().Init())
            {
                var jwtProvider = new JwtAuthProvider
                {
                    AuthKeyBase64 = Config.JwtAuthKeyBase64,
                    ExpireTokensInDays = 3650
                };

                var header = JwtAuthProvider.CreateJwtHeader(jwtProvider.HashAlgorithm);
                var body = JwtAuthProvider.CreateJwtPayload(new AuthUserSession
                    {
                        UserAuthId = "1",
                        DisplayName = "test",
                        UserName = "test",
                        IsAuthenticated = true,
                    },
                    issuer: jwtProvider.Issuer,
                    expireIn: jwtProvider.ExpireTokensIn,
                    audiences: new []{ jwtProvider.Audience },
                    roles: new[] { "TheRole" },
                    permissions: new[] { "ThePermission" });

                var jwtToken = JwtAuthProvider.CreateJwt(header, body, jwtProvider.GetHashAlgorithm());
                jwtToken.Print();
            }
        }
    }
}