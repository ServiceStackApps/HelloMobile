﻿using ServiceModel;
using ServiceStack;
using ServiceStack.Auth;

namespace Server
{
    public static class SharedAppHost
    {
        public static void Configure(IAppHost appHost)
        {
            //appHost.Routes.AddFromAssembly(typeof(WebServices).Assembly);
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
    }

    [Authenticate]
    public class AdminServices : Service
    {
        public object Any(HelloAuth request)
        {
            var response = new HelloResponse { Result = "Hello, " + request.Name };
            return response;
        }
    }

    public class CustomCredentialsAuthProvider : CredentialsAuthProvider
    {
        public override bool TryAuthenticate(IServiceBase authService, string userName, string password)
        {
            return userName == "user" && password == "pass";
        }
    }

    public class WebServices : Service
    {
        public object Any(Hello request)
        {
            var response = new HelloResponse { Result = "Hello, " + request.Name };

            return response;
        }

        public object Any(SendFile request)
        {
            var response = new SendFileResponse {
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