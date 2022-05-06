using eShopSolution.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.ViewModels.System.Roles
{
    public class GetRolePagingRequest : PagingRequestBase
    {
        public string Keyword { get; set; }
    }
}
