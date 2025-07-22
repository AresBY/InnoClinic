using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InnoClinic.Application.JWT
{
    public interface IJwtTokenGenerator
    {
        string GenerateAccessToken(Guid userId, string email);
        string GenerateRefreshToken();
    }
}
