using System;


namespace TodoFlutter.data
{
    public interface ITokenFactory
    {
        string GenerateToken(int size = 32);
    }
}
