namespace net_pdf_sample.Models
{
    public class Report
    {
        public string? Title { get; set; }
        public string[]? Args { get; internal set; }
        public string Content { get; set; } = string.Empty;
        public string? HtmlFilePath { get; set; }

        private string _tempDirectory { get; set; }

        // コンストラクタでテンポラリディレクトリのパスを受け取る
        public Report(string tempDirectory)
        {
            _tempDirectory = tempDirectory;
        }

        public string CreateHtmlFile(string htmlContent)
        {
            var tempPath = Path.Combine(_tempDirectory, $"{Path.GetRandomFileName()}.html");
            File.WriteAllText(tempPath, htmlContent);
            HtmlFilePath = tempPath;
            return tempPath;
        }
    }
}
