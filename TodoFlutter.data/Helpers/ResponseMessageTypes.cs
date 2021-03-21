using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TodoFlutter.data.Helpers
{
    public static class ResponseMessageTypes
    {
        public static string USER_CREATED_SUCCESS = "Success creating user";
        public static string USER_CREATED_FAILURE = "Failed to create user";
        public static string USER_LOGIN_SUCCESS = "Successful login";
        public static string USER_LOGIN_FAILURE = "Unsuccessful login";
        public static string REFRESH_TOKEN_SUCCESS = "Success generating refresh token";
        public static string REFRESH_TOKEN_FAILURE = "Faied generating refresh token";

    }
}
