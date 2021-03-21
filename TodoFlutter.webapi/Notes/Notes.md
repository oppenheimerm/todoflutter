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


https://damienbod.com/2019/10/25/securing-a-web-api-using-multiple-token-servers/

https://github.com/damienbod/ApiJwtWithTwoSts/blob/main/WebApi/Startup.cs

