namespace WebViewAjaxInterceptor
{
    public sealed partial class WebHost
    {
        public WebHost()
        {
            InitializeComponent();

            var uri = WebView.BuildLocalStreamUri("Ajax", "/index.html");
            WebView.NavigateToLocalStreamUri(uri, new AjaxResolver());
        }
    }
}
