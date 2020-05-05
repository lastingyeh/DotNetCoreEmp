using System;
using EmployeeManagement.Models;
using EmployeeManagement.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.Controllers
{
    [Route("[controller]/[action]")]
    public class HomeController : Controller
    {
        private readonly IEmployeeRepository _employeeRepository;

        public HomeController(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        [Route("")]
        [Route("~/Home")]
        [Route("~/")]  //index
        public ViewResult Index()
        {
            var model = _employeeRepository.GetAllemployee();

            return View(model);
        }

        [Route("{id?}")]
        public ViewResult Details(int? id)
        {
            HomeDetailsViewModel homeDetailsViewModel = new HomeDetailsViewModel()
            {
                Employee = _employeeRepository.GetEmployee(id ?? 1),
                PageTitle = "Employee Details"
            };
            //ViewData["employee"] = employee;
            //ViewData["PageTitle"] = "Employee Details";
            //ViewBag.Employee = employee;
            //ViewBag.PageTitle = "Employee Details";
            return View(homeDetailsViewModel);
        }
    }
}
