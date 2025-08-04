using MediatR;

namespace InnoClinic.Authorization.Application.Features.Auth.Queries.CheckEmailExists
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
