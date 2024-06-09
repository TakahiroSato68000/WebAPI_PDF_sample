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
            // アプリケーション終了時に呼び出す
            appLifetime.ApplicationStopping.Register(OnStopping);
        }

        // アプリケーション終了時に呼び出される
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

                // TODO: アンマネージド リソース (アンマネージド オブジェクト) を解放し、ファイナライザーをオーバーライドします
                // TODO: 大きなフィールドを null に設定します
                disposedValue = true;
            }
        }

        // // TODO: 'Dispose(bool disposing)' にアンマネージド リソースを解放するコードが含まれる場合にのみ、ファイナライザーをオーバーライドします
        // ~TempDirectoryProvider()
        // {
        //     // このコードを変更しないでください。クリーンアップ コードを 'Dispose(bool disposing)' メソッドに記述します
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // このコードを変更しないでください。クリーンアップ コードを 'Dispose(bool disposing)' メソッドに記述します
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
