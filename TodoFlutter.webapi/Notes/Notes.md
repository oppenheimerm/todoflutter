# UserManager.CheckPasswordAsync vs SignInManager.PasswordSignInAsync

###  UserManager.CheckPasswordAsync
This method hashes the provided password and compares it against the existing password hash (stored in the database, for example).

### SignInManager.PasswordSignInAsync

This method **does a lot more.** Here's a rough breakdown:

- Checks whether sign-in is allowed. For example, if the user must have a confirmed email before being allowed to sign-in, the method returns SignInResult.Failed.

- Calls UserManager.CheckPasswordAsync to check that the password is correct (as detailed above).

  - If the password is not correct and lockout is supported, the method tracks the failed sign-in attempt. If the configured amount of failed sign-in attempts is exceeded, the method locks the user out.

- If two-factor authentication is enabled for the user, the method sets up the relevant cookie and returns SignInResult.TwoFactorRequired.

- Finally, performs the sign-in process, which ends up creating a ClaimsPrincipal and persisting it via a cookie.

-  If you are not interested in requiring confirmed emails, lockout, etc, then using **UserManager.CheckPasswordAsync** as in your question will suffice.

JWT Authentication with ASP.NET Core 2 Web API, Angular 5, .NET Core Identity and Facebook Login - 
https://fullstackmark.com/post/13/jwt-authentication-with-aspnet-core-2-web-api-angular-5-net-core-identity-and-facebook-login

JWT Authentication Flow with Refresh Tokens in ASP.NET Core Web API - 
https://fullstackmark.com/post/19/jwt-authentication-flow-with-refresh-tokens-in-aspnet-core-web-api


ASP.NET Core 3.1 - Create and Validate JWT Tokens + Use Custom JWT Middleware -
https://jasonwatmore.com/post/2020/07/21/aspnet-core-3-create-and-validate-jwt-tokens-use-custom-jwt-middleware


https://damienbod.com/2019/10/25/securing-a-web-api-using-multiple-token-servers/

https://github.com/damienbod/ApiJwtWithTwoSts/blob/main/WebApi/Startup.cs

