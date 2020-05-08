﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace EmployeeManagement.Controllers
{
    public class ErrorController : Controller
    {
        private readonly ILogger<ErrorController> logger;

        //group Logg at ErrorController
        public ErrorController(ILogger<ErrorController> logger)
        {
            this.logger = logger;
        }

        [Route("Error/{statusCode}")]
        public IActionResult HttpStatusCodeHandler(int statusCode)
        {
            var statusCodeResult = HttpContext.Features.Get<IStatusCodeReExecuteFeature>();

            switch (statusCode)
            {
                case 404:
                    ViewBag.ErrorMessage = "Sorry, the resource you required could not be found";
                    //ViewBag.Path = statusCodeResult.OriginalPath;
                    //ViewBag.QS = statusCodeResult.OriginalQueryString;
                    logger.LogWarning($"404 Error Occured. Path = {statusCodeResult.OriginalPath}" +
                        $"and QueryString = {statusCodeResult.OriginalQueryString}");
                    break;
            }

            return View("NotFound");
        }

        [Route("Error")]
        [AllowAnonymous]
        public IActionResult Error()
        {
            var exceptionDetails = HttpContext.Features.Get<IExceptionHandlerPathFeature>();

            //ViewBag.ExceptionPath = exceptionDetails.Path;
            //ViewBag.ExceptionMessage = exceptionDetails.Error.Message;
            //ViewBag.ExceptionStacktrace = exceptionDetails.Error.StackTrace;
            logger.LogError($"The path {exceptionDetails.Path} threw an exception " +
                $"{exceptionDetails.Error}");

            return View("Error");
        }
    }
}