#pragma checksum "C:\LEARNING\C#\EmployeeManagement\EmployeeManagement\Views\Home\Details.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "7a2865630f836a6504282814fb240a5caa52f561"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Home_Details), @"mvc.1.0.view", @"/Views/Home/Details.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.Razor.Compilation.RazorViewAttribute(@"/Views/Home/Details.cshtml", typeof(AspNetCore.Views_Home_Details))]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"7a2865630f836a6504282814fb240a5caa52f561", @"/Views/Home/Details.cshtml")]
    public class Views_Home_Details : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<EmployeeManagement.ViewModels.HomeDetailsViewModel>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            BeginContext(59, 2, true);
            WriteLiteral("\r\n");
            EndContext();
#line 3 "C:\LEARNING\C#\EmployeeManagement\EmployeeManagement\Views\Home\Details.cshtml"
  
    Layout = "~/Views/Shared/_Layout.cshtml";
    ViewBag.Title = "Employee Details";

#line default
#line hidden
            BeginContext(156, 6, true);
            WriteLiteral("\r\n<h3>");
            EndContext();
            BeginContext(163, 15, false);
#line 8 "C:\LEARNING\C#\EmployeeManagement\EmployeeManagement\Views\Home\Details.cshtml"
Write(Model.PageTitle);

#line default
#line hidden
            EndContext();
            BeginContext(178, 26, true);
            WriteLiteral("</h3>\r\n\r\n<div>\r\n    Name: ");
            EndContext();
            BeginContext(205, 19, false);
#line 11 "C:\LEARNING\C#\EmployeeManagement\EmployeeManagement\Views\Home\Details.cshtml"
     Write(Model.Employee.Name);

#line default
#line hidden
            EndContext();
            BeginContext(224, 28, true);
            WriteLiteral("\r\n</div>\r\n<div>\r\n    Email: ");
            EndContext();
            BeginContext(253, 20, false);
#line 14 "C:\LEARNING\C#\EmployeeManagement\EmployeeManagement\Views\Home\Details.cshtml"
      Write(Model.Employee.Email);

#line default
#line hidden
            EndContext();
            BeginContext(273, 33, true);
            WriteLiteral("\r\n</div>\r\n<div>\r\n    Department: ");
            EndContext();
            BeginContext(307, 25, false);
#line 17 "C:\LEARNING\C#\EmployeeManagement\EmployeeManagement\Views\Home\Details.cshtml"
           Write(Model.Employee.Department);

#line default
#line hidden
            EndContext();
            BeginContext(332, 10, true);
            WriteLiteral("\r\n</div>\r\n");
            EndContext();
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<EmployeeManagement.ViewModels.HomeDetailsViewModel> Html { get; private set; }
    }
}
#pragma warning restore 1591
