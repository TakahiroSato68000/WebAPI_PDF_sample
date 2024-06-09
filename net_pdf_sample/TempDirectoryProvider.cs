namespace net_pdf_sample
{
    public class TempDirectoryProvider: IDisposable
    {
        private bool disposedValue;

        public string DirectoryName { get; }

        public TempDirectoryProvider(IHostApplicationLifetime appLifetime)
        {
            // テンポラリディレクトリの作成
            var tempDirectory = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            Directory.CreateDirectory(tempDirectory);
            DirectoryName = tempDirectory;
            appLifetime.ApplicationStopping.Register(OnStopping);
        }

        private void OnStopping()
        {
            Dispose();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // アプリケーション終了時にテンポラリディレクトリを削除
                    System.IO.Directory.Delete(DirectoryName, true);
                }
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
    }
}
