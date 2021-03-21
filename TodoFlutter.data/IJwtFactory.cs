using System;
using System.Threading.Tasks;
using TodoFlutter.core.Models.DTO;

namespace TodoFlutter.data
{
    public interface IJwtFactory
    {
        Task<AccessToken> GenerateEncodedToken(string userId, string userName, string email);        
    }
}
