using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.IO;
using System.Threading.Tasks;

namespace net_pdf_sample.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ReportController : Controller
    {
        private readonly TempDirectoryProvider _tempDirectoryProvider;
        private readonly ILogger<ReportController> _logger;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly ICompositeViewEngine _viewEngine;
        private readonly ITempDataProvider _tempDataProvider;

        public ReportController(TempDirectoryProvider tempDirectoryProvider, ILogger<ReportController> logger, IWebHostEnvironment hostingEnvironment, ICompositeViewEngine viewEngine, ITempDataProvider tempDataProvider)
        {
            _tempDirectoryProvider = tempDirectoryProvider;
            _logger = logger;
            _hostingEnvironment = hostingEnvironment;
            _viewEngine = viewEngine;
            _tempDataProvider = tempDataProvider;
        }

        [HttpPost(Name = "Report")]
        public async Task<ActionResult<string>> PostReport(string title = "Title", string[]? args = null)
        {
            var report = new Models.Report()
            {
                Title = title,
                Args = args
            };
            // テンプレートをViewでレンダリングしてHTML文字列を作成する
            var html = await RenderViewToStringAsync("ReportTemplate", report);
            var reportPdf = new Models.ReportPdf(_tempDirectoryProvider.DirectoryName);
            reportPdf.CreateHtmlFile(html);
            return Ok(reportPdf);
        }

        private async Task<string> RenderViewToStringAsync<TModel>(string viewName, TModel model)
        {
            var viewResult = _viewEngine.FindView(ControllerContext, viewName, false);
            if (viewResult.View == null)
            {
                throw new ArgumentNullException($"{viewName} does not match any available view");
            }

            using (var sw = new StringWriter())
            {
                var viewContext = new ViewContext(
                    ControllerContext,
                    viewResult.View,
                    new ViewDataDictionary<TModel>(new EmptyModelMetadataProvider(), new ModelStateDictionary())
                    {
                        Model = model
                    },
                    new TempDataDictionary(ControllerContext.HttpContext, _tempDataProvider),
                    sw,
                    new HtmlHelperOptions()
                );

                await viewResult.View.RenderAsync(viewContext);
                return sw.GetStringBuilder().ToString();
            }
        }
    }
}