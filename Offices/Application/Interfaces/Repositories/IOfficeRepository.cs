using InnoClinic.Offices.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InnoClinic.Offices.Application.Interfaces.Repositories
{
    public interface IOfficeRepository
    {
        Task InsertAsync(Office office, CancellationToken cancellationToken);
    }
}
