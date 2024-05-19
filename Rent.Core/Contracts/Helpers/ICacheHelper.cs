using Rent.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rent.Core.Contracts.Helpers
{
    public interface ICacheHelper
    {
        Guid SetApproveModel(RegisterUserModelsExpanded model);
        RegisterUserModelsExpanded GetApproveModel(Guid modelId);
    }
}
