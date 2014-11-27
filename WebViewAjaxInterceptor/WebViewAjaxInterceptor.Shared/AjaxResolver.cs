using System;
using System.IO;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.Web;

namespace WebViewAjaxInterceptor
{
    public sealed class AjaxResolver : IUriToStreamResolver
    {
        public IAsyncOperation<IInputStream> UriToStreamAsync(Uri uri)
        {
            string path = uri.AbsolutePath;

            if (path.StartsWith("/ajax", StringComparison.OrdinalIgnoreCase))
            {
                return GetContent(new Uri("any:///ajax/data.txt")).AsAsyncOperation();
            }

            return GetContent(uri).AsAsyncOperation();
        }

        private async Task<IInputStream> GetContent(Uri uri)
        {
            string path = uri.AbsolutePath;
            try
            {
                Uri localUri = new Uri("ms-appx:///Html" + path);
                StorageFile f = await StorageFile.GetFileFromApplicationUriAsync(localUri);
                IRandomAccessStream stream = await f.OpenAsync(FileAccessMode.Read);
                return stream.GetInputStreamAt(0);
            }
            catch (Exception e)
            {
                throw new FileNotFoundException("File not found in '/Html'.", path, e);
            }
        }
    }
}
