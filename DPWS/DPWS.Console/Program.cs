using DPWS.Console;

const string jobTag = "//div [@class='jg__job']";
const string jobName = "//h2 [@class='job__name']";
const string jobCompany = "//h3 [@class='job__company']";
const string jobDetail = "//h3 [@class='job__detail']";
const string jobDescription = "//p [@class='job__description']";

var html = await ScrapingJobService.RequestHtml("https://www.trabalhabrasil.com.br/");
var lista = html.ReturnJobList(jobTag, jobName, jobCompany, jobDetail, jobDescription);

lista.ForEach(x => Console.WriteLine(x.Name));