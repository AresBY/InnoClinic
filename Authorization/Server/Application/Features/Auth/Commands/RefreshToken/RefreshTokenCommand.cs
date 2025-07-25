using InnoClinic.Authorization.Application.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InnoClinic.Authorization.Application.Features.Auth.Commands
{
    public class RefreshTokenCommand : IRequest<RefreshTokenResultDto>
    {
        public string RefreshToken { get; set; } = default!;
    }
}
