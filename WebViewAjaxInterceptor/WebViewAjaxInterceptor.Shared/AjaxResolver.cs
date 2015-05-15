using System;
using System.IO;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.Web;
using Newtonsoft.Json;

namespace WebViewAjaxInterceptor
{
    public sealed class AjaxResolver : IUriToStreamResolver
    {
        private readonly JsonSerializer _jsonSerializer = new JsonSerializer();

        public IAsyncOperation<IInputStream> UriToStreamAsync(Uri uri)
        {
            string path = uri.AbsolutePath;

            if (path.StartsWith("/ajax", StringComparison.OrdinalIgnoreCase))
            {
                return GetObject(new
                {
                    a = 1,
                    b = "b"
                }).AsAsyncOperation();
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
                return await f.OpenReadAsync();
            }
            catch (Exception e)
            {
                throw new FileNotFoundException("File not found in '/Html'.", path, e);
            }
        }

        private Task<IInputStream> GetObject<T>(T item)
        {
            IRandomAccessStream inMemoryStream = new InMemoryRandomAccessStream();
            var stream = inMemoryStream.AsStream();
            inMemoryStream.Seek(0);
            var writer = new StreamWriter(stream);
            _jsonSerializer.Serialize(writer, item);
            writer.Flush();
            stream.Seek(0L, SeekOrigin.Begin);
            return Task.FromResult<IInputStream>(new InputStreamWithContentType(inMemoryStream, "application/javascript"));
        }

        private class InputStreamWithContentType : IInputStream, IContentTypeProvider
        {
            private readonly IInputStream _inputStream;
            private readonly string _contentType;

            public InputStreamWithContentType(IInputStream inputStream, string contentType)
            {
                _inputStream = inputStream;
                _contentType = contentType;
            }

            public void Dispose()
            {
                _inputStream.Dispose();
            }

            public IAsyncOperationWithProgress<IBuffer, uint> ReadAsync(IBuffer buffer, uint count, InputStreamOptions options)
            {
                return _inputStream.ReadAsync(buffer, count, options);
            }

            public string ContentType
            {
                get { return _contentType; }
            }
        }
    }
}
