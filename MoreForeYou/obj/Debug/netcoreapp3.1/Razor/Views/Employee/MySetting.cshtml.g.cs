#pragma checksum "D:\Work\MoreForYou\Development\MoreForeYou\MoreForeYou\Views\Employee\MySetting.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "61db6870226b4991aff0c967ef50af52afa3197d"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Employee_MySetting), @"mvc.1.0.view", @"/Views/Employee/MySetting.cshtml")]
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
#line 1 "D:\Work\MoreForYou\Development\MoreForeYou\MoreForeYou\Views\_ViewImports.cshtml"
using MoreForeYou;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "D:\Work\MoreForYou\Development\MoreForeYou\MoreForeYou\Views\_ViewImports.cshtml"
using MoreForeYou.Models;

#line default
#line hidden
#nullable disable
#nullable restore
#line 3 "D:\Work\MoreForYou\Development\MoreForeYou\MoreForeYou\Views\_ViewImports.cshtml"
using Microsoft.AspNetCore.Identity;

#line default
#line hidden
#nullable disable
#nullable restore
#line 4 "D:\Work\MoreForYou\Development\MoreForeYou\MoreForeYou\Views\_ViewImports.cshtml"
using MoreForYou.Models.Models;

#line default
#line hidden
#nullable disable
#nullable restore
#line 5 "D:\Work\MoreForYou\Development\MoreForeYou\MoreForeYou\Views\_ViewImports.cshtml"
using MoreForYou.Models.Auth;

#line default
#line hidden
#nullable disable
#nullable restore
#line 6 "D:\Work\MoreForYou\Development\MoreForeYou\MoreForeYou\Views\_ViewImports.cshtml"
using System.Text.Json;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"61db6870226b4991aff0c967ef50af52afa3197d", @"/Views/Employee/MySetting.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"6e7782a3013a356e352b3c5090dc5d1c090ec7da", @"/Views/_ViewImports.cshtml")]
    public class Views_Employee_MySetting : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<MoreForYou.Services.Models.API.UserSetting>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("action", new global::Microsoft.AspNetCore.Html.HtmlString("#"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        #line hidden
        #pragma warning disable 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperExecutionContext __tagHelperExecutionContext;
        #pragma warning restore 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner __tagHelperRunner = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner();
        #pragma warning disable 0169
        private string __tagHelperStringValueBuffer;
        #pragma warning restore 0169
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __backed__tagHelperScopeManager = null;
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __tagHelperScopeManager
        {
            get
            {
                if (__backed__tagHelperScopeManager == null)
                {
                    __backed__tagHelperScopeManager = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager(StartTagHelperWritingScope, EndTagHelperWritingScope);
                }
                return __backed__tagHelperScopeManager;
            }
        }
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.FormTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper;
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.RenderAtEndOfFormTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n");
#nullable restore
#line 3 "D:\Work\MoreForYou\Development\MoreForeYou\MoreForeYou\Views\Employee\MySetting.cshtml"
  
    ViewData["Title"] = "MySetting";
    Layout = "~/Views/Shared/_More4ULayout2.cshtml";


#line default
#line hidden
#nullable disable
            WriteLiteral(@"<!--==================================
=            User Profile            =
===================================-->

<section class=""user-profile section"">
    <div class=""container"">
        <div class=""row"">
            <div class=""col-md-10 offset-md-1 col-lg-3 offset-lg-0"">
                <div class=""sidebar"">
                    <!-- User Widget -->
                    <div class=""widget user"">
                        <!-- User Image -->
                        <div class=""image d-flex justify-content-center"">
                            <img src=""images/user/user-thumb.jpg""");
            BeginWriteAttribute("alt", " alt=\"", 755, "\"", 761, 0);
            EndWriteAttribute();
            BeginWriteAttribute("class", " class=\"", 762, "\"", 770, 0);
            EndWriteAttribute();
            WriteLiteral(">\r\n                        </div>\r\n                        <!-- User Name -->\r\n                        <h5 class=\"text-center\">");
#nullable restore
#line 24 "D:\Work\MoreForYou\Development\MoreForeYou\MoreForeYou\Views\Employee\MySetting.cshtml"
                                           Write(Model.employeeName);

#line default
#line hidden
#nullable disable
            WriteLiteral(@"</h5>
                    </div>
                </div>
            </div>
            <div class=""col-md-10 offset-md-1 col-lg-9 offset-lg-0"">
                <!-- Edit Profile Welcome Text -->
                <div class=""widget welcome-message"">
                    <h2>Edit Profile</h2>
                    <p></p>
                </div>
                <!-- Edit Personal Info -->
                <div class=""row"">
                    <div class=""col-lg-6 col-md-6"">
                        <div class=""widget personal-info"">
                            <h3 class=""widget-header user"">Edit Profile Picture</h3>
                            ");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("form", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "61db6870226b4991aff0c967ef50af52afa3197d6639", async() => {
                WriteLiteral("\r\n                                <!-- File chooser -->\r\n                                <div class=\"form-group choose-file d-inline-flex\">\r\n                                    <i class=\"fa fa-user text-center px-3\"></i>\r\n");
                WriteLiteral("                                </div>\r\n                                <!-- Submit button -->\r\n                                <button class=\"btn btn-transparent\">Save My Changes</button>\r\n                            ");
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.FormTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.RenderAtEndOfFormTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_0);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral(@"
                        </div>
                    </div>
                    <div class=""col-lg-6 col-md-6"">
                        <!-- Change Password -->
                        <div class=""widget change-password"">
                            <h3 class=""widget-header user"">Edit Password</h3>
                            ");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("form", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "61db6870226b4991aff0c967ef50af52afa3197d8781", async() => {
                WriteLiteral(@"
                                <!-- Current Password -->
                                <div class=""form-group"">
                                    <label for=""current-password"">Current Password</label>
                                    <input type=""password"" class=""form-control"" id=""current-password"">
                                </div>
                                <!-- New Password -->
                                <div class=""form-group"">
                                    <label for=""new-password"">New Password</label>
                                    <input type=""password"" class=""form-control"" id=""new-password"">
                                </div>
                                <!-- Confirm New Password -->
                                <div class=""form-group"">
                                    <label for=""confirm-password"">Confirm New Password</label>
                                    <input type=""password"" class=""form-control"" id=""confirm-password"">
           ");
                WriteLiteral("                     </div>\r\n                                <!-- Submit Button -->\r\n                                <button class=\"btn btn-transparent\">Change Password</button>\r\n                            ");
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.FormTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.RenderAtEndOfFormTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_0);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\r\n                        </div>\r\n                    </div>\r\n                </div>\r\n            </div>\r\n        </div>\r\n    </div>\r\n</section>");
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<MoreForYou.Services.Models.API.UserSetting> Html { get; private set; }
    }
}
#pragma warning restore 1591