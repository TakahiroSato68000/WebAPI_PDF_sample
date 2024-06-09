namespace net_pdf_sample.Models
{
    public class ReportPdf
    {
        public string Content { get; set; } = string.Empty;
        public string? HtmlFilePath { get; set; }
        private string _tempDirectory { get; set; }

        // コンストラクタでテンポラリディレクトリのパスを受け取る
        public ReportPdf(string tempDirectory)
        {
            _tempDirectory = tempDirectory;
        }

        // HTML文字列を受け取り、テンポラリディレクトリにHTMLファイルを作成する
        public string CreateHtmlFile(string htmlContent)
        {
            var tempPath = Path.Combine(_tempDirectory, $"{Path.GetRandomFileName()}.html");
            File.WriteAllText(tempPath, htmlContent);
            HtmlFilePath = tempPath;
            return tempPath;
        }
    }
}
