using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.Controllers
{
    public class DepartmentsController : Controller
    {
        public string Index()
        {
            return "Departments Index";
        }

        public string List()
        {
            return "Departments List";
        }
    }
}