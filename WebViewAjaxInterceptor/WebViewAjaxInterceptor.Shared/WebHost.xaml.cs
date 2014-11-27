using Windows.UI.Xaml;

namespace WebViewAjaxInterceptor
{
    public sealed partial class WebHost
    {
        public WebHost()
        {
            InitializeComponent();

            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            var uri = WebView.BuildLocalStreamUri("Ajax", "/index.html");
            WebView.NavigateToLocalStreamUri(uri, new AjaxResolver());
        }
    }
}
