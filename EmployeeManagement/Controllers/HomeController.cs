﻿using System;
using EmployeeManagement.Models;
using EmployeeManagement.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.Controllers
{
    public class HomeController : Controller
    {
        private readonly IEmployeeRepository _employeeRepository;

        public HomeController(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        public ViewResult Index()
        {
            var model = _employeeRepository.GetAllemployee();

            return View(model);
        }

        public ViewResult Details()
        {
            HomeDetailsViewModel homeDetailsViewModel = new HomeDetailsViewModel()
            {
                Employee = _employeeRepository.GetEmployee(1),
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
