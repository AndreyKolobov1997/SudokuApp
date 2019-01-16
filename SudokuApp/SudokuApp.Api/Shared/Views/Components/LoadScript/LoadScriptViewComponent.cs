using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.Options;
using SudokuApp.Common.Configuration.Options;

namespace SudokuApp.Api.Shared.Views.Components.LoadScript
{
    public class LoadScriptViewComponent : ViewComponent
    {
        private const string _BASE_PATH = "/Controllers/Mvc/";
        private const string _BASE_PATH_SHARED = "/Shared/";
        private const string _BASE_PATH_SHARED_SLASH = "Shared/";
        private const string _BASE_PATH_VIEWS = "Views/";
        private const string _BASE_PATH_COMPONENTS = "Components/";
        private const string _JS_PATH = "js/";

        private readonly ServiceNameOptions _options;

        public LoadScriptViewComponent(IOptions<ServiceNameOptions> options)
        {
            _options = options.Value;
        }

        public async Task<IViewComponentResult> InvokeAsync(RazorPage page, object val, bool includeScript = true)
        {
            var model = new LoadScriptModel()
            {
                Model = val,
                ScriptSrc = GetScriptPath(page, _options.ServiceName),
                IncludeScript = includeScript
            };

            return View("LoadScript", model);
        }

        private string GetScriptPath(RazorPage page, string serviceNamePrefix)
        {
            var serviceNameAndJs = $"~/{serviceNamePrefix}/static/{_JS_PATH}";

            var scriptPath = new StringBuilder(page.Path.Replace(_BASE_PATH, serviceNameAndJs))
                .Replace(_BASE_PATH_SHARED, serviceNameAndJs + _BASE_PATH_SHARED_SLASH)
                .Replace(_BASE_PATH_VIEWS, string.Empty)
                .Replace(_BASE_PATH_COMPONENTS, string.Empty)
                .Replace(".cshtml", ".js");

            return scriptPath.ToString();
        }
    }
}
