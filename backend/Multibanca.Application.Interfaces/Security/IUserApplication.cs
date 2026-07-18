using Common.Application.Interfaces;
using Multibanca.Domain.Models.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multibanca.Application.Interfaces.Security
{
    public interface IUserApplication : IMultibancaGenericApplication<users>
    {
    }
}
