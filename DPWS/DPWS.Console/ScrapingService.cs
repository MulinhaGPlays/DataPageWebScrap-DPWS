using AngleSharp.Html.Parser;
using HtmlAgilityPack;
using System.Text;

namespace DPWS.Console
{
    public static class ScrapingJobService
    {
        private static readonly HttpClient _httpClient = new();
        private static readonly StringBuilder _strBuilder = new();
        private static readonly HtmlParser _htmlParser = new();
        private static readonly HtmlDocument _documentHtml = new();
        private static readonly HtmlDocument _documentJob = new();

        public class Job
        {
            public string Name { get; set; }
            public string Company { get; set; }
            public string Detail { get; set; }
            public string Description { get; set; }
        }

        /// <summary>
        ///     Método que irá fazer a requisição do html do site
        /// </summary>
        /// <param name="url">Url do site</param>
        /// <returns>html da página web</returns>
        public static async Task<string> RequestHtml(string url)
        {
            try
            {
                _strBuilder.Append(await _httpClient.GetStringAsync(url));
                var content = _htmlParser.ParseDocument(_strBuilder.ToString());
                var html = content.DocumentElement.OuterHtml;

                return html;
            }
            finally
            {
                _httpClient.Dispose();
            }
        }

        /// <summary>
        ///     Método onde haverá a busca dos trabalhos
        /// </summary>
        /// <param name="html">html retornado pelo método "RequestHtml"</param>
        /// <param name="filters">Filtros para encontrar os dados baseado no modelo "Job". O Primeiro parâmetro é sempre o principal que irá conter os dados para o modelo</param>
        /// <returns>Lista de trabalhos</returns>
        public static List<Job> ReturnJobList(this string html, params string[] filters)
        {
            _documentHtml.LoadHtml(html);

            var jobsTags = _documentHtml.DocumentNode.SelectNodes(filters.First()).ToList();
            var jobs = jobsTags.Select(x =>
            {
                _documentJob.LoadHtml(x.InnerHtml);

                string getString(string str) => _documentJob.DocumentNode.SelectSingleNode(str).InnerText;

                return new Job
                {
                    Name = getString(filters[1]),
                    Company = getString(filters[2]),
                    Detail = getString(filters[3]),
                    Description = getString(filters[4]),
                };
            });

            return jobs.ToList();
        }
    }
}
