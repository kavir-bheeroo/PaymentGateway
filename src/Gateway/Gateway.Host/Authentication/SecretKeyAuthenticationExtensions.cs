using Microsoft.AspNetCore.Authentication;
using System;

namespace Gateway.Host.Authentication
{
    public static class SecretKeyAuthenticationExtensions
    {
        public static AuthenticationBuilder AddSecretKey(this AuthenticationBuilder builder)
        {
            return AddSecretKey(builder, SecretKeyAuthenticationDefaults.AuthenticationScheme, _ => { });
        }

        public static AuthenticationBuilder AddSecretKey(this AuthenticationBuilder builder, string authenticationScheme)
        {
            return AddSecretKey(builder, authenticationScheme, _ => { });
        }

        public static AuthenticationBuilder AddSecretKey(this AuthenticationBuilder builder, Action<SecretKeyAuthenticationOptions> configureOptions)
        {
            return AddSecretKey(builder, SecretKeyAuthenticationDefaults.AuthenticationScheme, configureOptions);
        }

        public static AuthenticationBuilder AddSecretKey(this AuthenticationBuilder builder, string authenticationScheme, Action<SecretKeyAuthenticationOptions> configureOptions)
        {
            return builder.AddScheme<SecretKeyAuthenticationOptions, SecretKeyAuthenticationHandler>(authenticationScheme, configureOptions);
        }
    }
}