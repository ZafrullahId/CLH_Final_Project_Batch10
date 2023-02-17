using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dansnom.Dtos;

namespace Dansnom.Auth
{
    public interface IJWTAuthenticationManager
    {
        string GenerateToken(string key,string issuer,UserDto user);
        bool IsTokenValid(string key, string issuer, string token);
    }
}
