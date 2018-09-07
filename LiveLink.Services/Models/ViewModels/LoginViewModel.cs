using System.ComponentModel.DataAnnotations;
using System.Globalization;
using Our.Umbraco.Ditto;
using umbraco.editorControls.SettingControls;
using Umbraco.Core.Models;
using Umbraco.Web.Models;

namespace LiveLink.Services.Models.ViewModels
{
    public class LoginViewModel : RenderModel
    {
        public LoginViewModel(IPublishedContent content, CultureInfo culture)
            : base(content, culture)
        {
        }

        [DittoIgnore] public LoginForm Form { get; set; }

        [UmbracoProperty("url")] public string Url { get; set; }
    }

    public class LoginForm
    {
        [Required]
        [Display(Name = "Username")]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "RememberMe")] public bool RememberMe { get; set; }
    }
}