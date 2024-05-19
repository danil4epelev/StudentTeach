using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rent.Core.Contracts.Helpers
{
    public interface IAccountHelper
    {
        public Task RegisterAccount(string email, string password);

    }
}
