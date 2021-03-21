using System.ComponentModel.DataAnnotations;


namespace TodoFlutter.core.Models
{
    //  His: Web.Api.Models.Request
    //  Input model for a user refresh token request.
    //  Unlike ExchangeRefreshTokenRequest
    public class ExchangeRefreshTokenRequestInput
    {
        [Required]
        public string AccessToken { get; set; }
        [Required]
        public string RefreshToken { get; set; }
    }
}
