using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InnoClinic.Authorization.Application.Features.Auth.Queries
{
    public class CheckEmailExistsQuery : IRequest<bool>
    {
        public string Email { get; }

        public CheckEmailExistsQuery(string email)
        {
            Email = email;
        }
    }
}
