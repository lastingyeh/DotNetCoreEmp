using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.ViewModels
{
    public class UserClaimsViewModel
    {
        public List<UserClaim> Claims { get; set; }
        public string UserId { get; set; }

        public UserClaimsViewModel()
        {
            Claims = new List<UserClaim>();
        }
    }
}
