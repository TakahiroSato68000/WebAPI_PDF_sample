using Microsoft.Playwright;

namespace net_pdf_sample.Models
{
    public class ReportPdf
    {
        public string Content { get; set; } = string.Empty;
        public string? HtmlFilePath { get; set; }
        public string? PdfFilePath { get; set; }
        private string _tempDirectory { get; set; }

        // コンストラクタでテンポラリディレクトリのパスを受け取る
        public ReportPdf(string tempDirectory)
        {
            _tempDirectory = tempDirectory;
        }

        // HTML文字列を受け取り、テンポラリディレクトリにHTMLファイルを作成する
        public async Task<string> CreateHtmlFile(string htmlContent)
        {
            return await Task<string>.Run(() =>
            {
                var tempPath = Path.Combine(_tempDirectory, $"{Path.GetRandomFileName()}.html");
                File.WriteAllText(tempPath, htmlContent);
                HtmlFilePath = tempPath;
                return tempPath;
            });
        }

        // HTMLファイルをPDFに変換する
        public async Task<string> ConvertToPdfAsync(string htmlFile)
        {
            using var playwright = await Playwright.CreateAsync();
            var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
            {
                Headless = true,
            });
            var pdfPath = Path.ChangeExtension(htmlFile, ".pdf");
            var page = await browser.NewPageAsync();
            await page.GotoAsync("file://" + htmlFile);
            await page.PdfAsync(new PagePdfOptions
            {
                Path = pdfPath
            });
            PdfFilePath= pdfPath;
            await browser.CloseAsync();
            return pdfPath;
        }
    }
}
