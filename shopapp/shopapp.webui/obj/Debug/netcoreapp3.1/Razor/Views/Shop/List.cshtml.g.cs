#pragma checksum "D:\My Projects\Web Projects\Asp.Net Core Mvc 3.1 Sadık TURAN\shopapp\shopapp.webui\Views\Shop\List.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "25374b4e705fc74c89a71e83ac9795e5c83a839e"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Shop_List), @"mvc.1.0.view", @"/Views/Shop/List.cshtml")]
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
#nullable restore
#line 1 "D:\My Projects\Web Projects\Asp.Net Core Mvc 3.1 Sadık TURAN\shopapp\shopapp.webui\Views\_ViewImports.cshtml"
using shopapp.entity;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "D:\My Projects\Web Projects\Asp.Net Core Mvc 3.1 Sadık TURAN\shopapp\shopapp.webui\Views\_ViewImports.cshtml"
using shopapp.webui.Models;

#line default
#line hidden
#nullable disable
#nullable restore
#line 3 "D:\My Projects\Web Projects\Asp.Net Core Mvc 3.1 Sadık TURAN\shopapp\shopapp.webui\Views\_ViewImports.cshtml"
using Newtonsoft.Json;

#line default
#line hidden
#nullable disable
#nullable restore
#line 4 "D:\My Projects\Web Projects\Asp.Net Core Mvc 3.1 Sadık TURAN\shopapp\shopapp.webui\Views\_ViewImports.cshtml"
using shopapp.webui.Extensions;

#line default
#line hidden
#nullable disable
#nullable restore
#line 5 "D:\My Projects\Web Projects\Asp.Net Core Mvc 3.1 Sadık TURAN\shopapp\shopapp.webui\Views\_ViewImports.cshtml"
using Microsoft.AspNetCore.Identity;

#line default
#line hidden
#nullable disable
#nullable restore
#line 6 "D:\My Projects\Web Projects\Asp.Net Core Mvc 3.1 Sadık TURAN\shopapp\shopapp.webui\Views\_ViewImports.cshtml"
using shopapp.webui.Identity;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"25374b4e705fc74c89a71e83ac9795e5c83a839e", @"/Views/Shop/List.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"0f82bc0666dc4ed3155d495226781eb62eb9c56e", @"/Views/_ViewImports.cshtml")]
    public class Views_Shop_List : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<ProductListViewModel>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n<div class=\"row\">\r\n    <div class=\"col-md-3\">\r\n        ");
#nullable restore
#line 5 "D:\My Projects\Web Projects\Asp.Net Core Mvc 3.1 Sadık TURAN\shopapp\shopapp.webui\Views\Shop\List.cshtml"
   Write(await Component.InvokeAsync("Categories"));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n    </div>\r\n    <div class=\"col-md-9\">\r\n        <div class=\"row\">\r\n");
#nullable restore
#line 9 "D:\My Projects\Web Projects\Asp.Net Core Mvc 3.1 Sadık TURAN\shopapp\shopapp.webui\Views\Shop\List.cshtml"
             foreach (var product in Model.Products)
            {

#line default
#line hidden
#nullable disable
            WriteLiteral("                <div class=\"col-md-4\">\r\n                    ");
#nullable restore
#line 12 "D:\My Projects\Web Projects\Asp.Net Core Mvc 3.1 Sadık TURAN\shopapp\shopapp.webui\Views\Shop\List.cshtml"
               Write(await Html.PartialAsync("_product",product));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                </div>\r\n");
#nullable restore
#line 14 "D:\My Projects\Web Projects\Asp.Net Core Mvc 3.1 Sadık TURAN\shopapp\shopapp.webui\Views\Shop\List.cshtml"
            }

#line default
#line hidden
#nullable disable
            WriteLiteral("        </div>\r\n\r\n        <div class=\"row\">\r\n            <div class=\"col\">\r\n                <nav aria-label=\"Page navigation example\">\r\n                    <ul class=\"pagination\">\r\n");
#nullable restore
#line 21 "D:\My Projects\Web Projects\Asp.Net Core Mvc 3.1 Sadık TURAN\shopapp\shopapp.webui\Views\Shop\List.cshtml"
                         for (int i=0; i < Model.PageInfo.TotalPages(); i++)
                        {
                            

#line default
#line hidden
#nullable disable
#nullable restore
#line 23 "D:\My Projects\Web Projects\Asp.Net Core Mvc 3.1 Sadık TURAN\shopapp\shopapp.webui\Views\Shop\List.cshtml"
                             if(String.IsNullOrEmpty(Model.PageInfo.CurrentCategory)){

#line default
#line hidden
#nullable disable
            WriteLiteral("                                <li");
            BeginWriteAttribute("class", " class=\"", 820, "\"", 886, 2);
            WriteAttributeValue("", 828, "page-item", 828, 9, true);
#nullable restore
#line 24 "D:\My Projects\Web Projects\Asp.Net Core Mvc 3.1 Sadık TURAN\shopapp\shopapp.webui\Views\Shop\List.cshtml"
WriteAttributeValue(" ", 837, Model.PageInfo.CurrentPage==i+1 ? "active":"", 838, 48, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral("><a class=\"page-link\"");
            BeginWriteAttribute("href", " href=\"", 908, "\"", 936, 2);
            WriteAttributeValue("", 915, "/products?page=", 915, 15, true);
#nullable restore
#line 24 "D:\My Projects\Web Projects\Asp.Net Core Mvc 3.1 Sadık TURAN\shopapp\shopapp.webui\Views\Shop\List.cshtml"
WriteAttributeValue("", 930, i+1, 930, 6, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(">");
#nullable restore
#line 24 "D:\My Projects\Web Projects\Asp.Net Core Mvc 3.1 Sadık TURAN\shopapp\shopapp.webui\Views\Shop\List.cshtml"
                                                                                                                                                     Write(i+1);

#line default
#line hidden
#nullable disable
            WriteLiteral("</a></li>\r\n");
#nullable restore
#line 25 "D:\My Projects\Web Projects\Asp.Net Core Mvc 3.1 Sadık TURAN\shopapp\shopapp.webui\Views\Shop\List.cshtml"
                            }
                            else
                            {

#line default
#line hidden
#nullable disable
            WriteLiteral("                                <li");
            BeginWriteAttribute("class", " class=\"", 1086, "\"", 1152, 2);
            WriteAttributeValue("", 1094, "page-item", 1094, 9, true);
#nullable restore
#line 28 "D:\My Projects\Web Projects\Asp.Net Core Mvc 3.1 Sadık TURAN\shopapp\shopapp.webui\Views\Shop\List.cshtml"
WriteAttributeValue(" ", 1103, Model.PageInfo.CurrentPage==i+1 ? "active":"", 1104, 48, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral("><a class=\"page-link\"");
            BeginWriteAttribute("href", " href=\"", 1174, "\"", 1234, 4);
            WriteAttributeValue("", 1181, "/products/", 1181, 10, true);
#nullable restore
#line 28 "D:\My Projects\Web Projects\Asp.Net Core Mvc 3.1 Sadık TURAN\shopapp\shopapp.webui\Views\Shop\List.cshtml"
WriteAttributeValue("", 1191, Model.PageInfo.CurrentCategory, 1191, 31, false);

#line default
#line hidden
#nullable disable
            WriteAttributeValue("", 1222, "?page=", 1222, 6, true);
#nullable restore
#line 28 "D:\My Projects\Web Projects\Asp.Net Core Mvc 3.1 Sadık TURAN\shopapp\shopapp.webui\Views\Shop\List.cshtml"
WriteAttributeValue("", 1228, i+1, 1228, 6, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(">");
#nullable restore
#line 28 "D:\My Projects\Web Projects\Asp.Net Core Mvc 3.1 Sadık TURAN\shopapp\shopapp.webui\Views\Shop\List.cshtml"
                                                                                                                                                                                     Write(i+1);

#line default
#line hidden
#nullable disable
            WriteLiteral("</a></li>\r\n");
#nullable restore
#line 29 "D:\My Projects\Web Projects\Asp.Net Core Mvc 3.1 Sadık TURAN\shopapp\shopapp.webui\Views\Shop\List.cshtml"
                            }

#line default
#line hidden
#nullable disable
#nullable restore
#line 29 "D:\My Projects\Web Projects\Asp.Net Core Mvc 3.1 Sadık TURAN\shopapp\shopapp.webui\Views\Shop\List.cshtml"
                             
                        }

#line default
#line hidden
#nullable disable
            WriteLiteral("                    </ul>\r\n                </nav>\r\n            </div>\r\n        </div>\r\n\r\n    </div>\r\n</div>\r\n\r\n");
            DefineSection("Scripts", async() => {
                WriteLiteral(@"
    <script src=""https://cdn.jsdelivr.net/npm/popper.js@1.16.0/dist/umd/popper.min.js"" integrity=""sha384-Q6E9RHvbIyZFJoft+2mJbHaEWldlvI9IOYy5n3zV9zzTtmI3UksdQRVvoxMfooAo"" crossorigin=""anonymous""></script>
    <script src=""https://stackpath.bootstrapcdn.com/bootstrap/4.4.1/js/bootstrap.min.js"" integrity=""sha384-wfSDF2E50Y2D1uUdj0O3uMBJnjuUD4Ih7YwaYd1iqfktj0Uod8GCExl3Og8ifwB6"" crossorigin=""anonymous""></script>
");
            }
            );
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<ProductListViewModel> Html { get; private set; }
    }
}
#pragma warning restore 1591
