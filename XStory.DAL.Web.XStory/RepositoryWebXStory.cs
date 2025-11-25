using HtmlAgilityPack;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using XStory.DAL.Web.XStory.Contracts;

namespace XStory.DAL.Web.XStory
{
    public class RepositoryWebXStory : IRepositoryWebXStory
    {
        public const string BASE_URL = @"https://www.xstory-fr.com/";

        private static HttpClient _httpClient;
        public static HttpClient HttpClient
        {
            get
            {
                if (_httpClient == null)
                {
                    _httpClient = new HttpClient(new HttpClientHandler()
                    {
                        ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) =>
                        {
                            //bypass
                            return true;
                        },
                    }, false)
                    { BaseAddress = new Uri(BASE_URL) };
                    _httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:141.0) Gecko/20100101 Firefox/141.0");
                    _httpClient.DefaultRequestHeaders.Add("Accept", "text/html,application/xhtml+xml");
                }
                return _httpClient;
            }
        }

        public HttpClient GetHttpClient()
        {
            return HttpClient;
        }

        public async Task<string> GetHtmlPage(string url)
        {
            string htmlPage = string.Empty;
            try
            {
                Uri requestUri = new Uri(url);
                HttpResponseMessage response = await HttpClient.GetAsync(requestUri);

                if (!response.IsSuccessStatusCode)
                {
                    string error = response.StatusCode + " " + response.ReasonPhrase;
                    throw new Exception(error);
                }

                // Start Encoding utf-8
                System.Text.Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

                Encoding sourceEncoding = Encoding.GetEncoding("windows-1252");
                Encoding targetEncoding = Encoding.UTF8;

                byte[] sourceBytes = await response.Content.ReadAsByteArrayAsync();
                byte[] utf8Bytes = Encoding.Convert(sourceEncoding, targetEncoding, sourceBytes);
                string html = Encoding.UTF8.GetString(utf8Bytes);

                htmlPage = HttpUtility.HtmlDecode(html);

                // End Encoding utf-8
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return htmlPage;
        }
    }
}
