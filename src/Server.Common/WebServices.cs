using System.Threading;
using System.Threading.Tasks;
using ServiceModel;
using ServiceStack;
using ServiceStack.Auth;

namespace Server
{
    public static class SharedAppHost
    {
        public static void Configure(IAppHost appHost)
        {
            appHost.Config.DefaultRedirectPath = "/metadata";

            appHost.Plugins.Add(new CorsFeature());

            appHost.Plugins.Add(new AuthFeature(() => new AuthUserSession(),
                new IAuthProvider[] {
                    new CustomCredentialsAuthProvider(),
                    new JwtAuthProvider
                    {
                        AuthKeyBase64 = Config.JwtAuthKeyBase64,
                        RequireSecureConnection = false,
                    },
                }));

            appHost.Plugins.Add(new EncryptedMessagesFeature
            {
                PrivateKeyXml = Config.PrivateKeyXml
            });
        }

        public class CustomCredentialsAuthProvider : CredentialsAuthProvider
        {
            public override async Task<bool> TryAuthenticateAsync(IServiceBase authService, string userName, string password, CancellationToken token=default)
            {
                return userName == "user" && password == "pass";
            }
        }
    }

    public class WebServices : Service
    {
        public object Any(Hello request) => new HelloResponse { Result = "Hello, " + request.Name };
    }

    [Authenticate]
    public class AuthServices : Service
    {
        public object Any(HelloAuth request)
        {
            return new HelloResponse { Result = "Hello, " + request.Name };
        }
    }

    public class FileServices : Service
    {
        public object Any(SendFile request)
        {
            var response = new SendFileResponse
            {
                Name = request.Name,
            };

            if (base.Request.Files.Length > 0)
            {
                var file = base.Request.Files[0];
                response.Result = $"Files: {Request.Files.Length}, name: {file.FileName}, size: {file.ContentLength} bytes";
            }

            return response;
        }
    }
}