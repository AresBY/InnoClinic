using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InnoClinic.Server.Application.Features.Auth.Commands
{
    public sealed class ConfirmEmailCommand : IRequest<Unit>
    {
        public Guid UserId { get; set; }
    }
}
