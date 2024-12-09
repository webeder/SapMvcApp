 
using SapMvcApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text;
using System.Net;
using System.Threading.Tasks;

namespace SapMvcApp.Controllers
{
    [Route("sap")]
    public class SapController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly CookieContainer _cookieContainer;

        private const string SapClient = "110";
        private const string BaseUrl = "https://vhudsds4ci.sap.unidasul.com.br:44300/sap/opu/odata/sap/z_gw_authorization_poc_srv";
        private const string AuthHeader = "Basic ***************";

        public SapController(IHttpClientFactory httpClientFactory)
        {
            _cookieContainer = new CookieContainer();
            var handler = new HttpClientHandler()
            {
                CookieContainer = _cookieContainer
            };
            _httpClient = new HttpClient(handler);
        }

        private async Task<(string csrfToken, string sapSessionId, string sapUserContext, string sapMysapSso2)> FetchCsrfTokenAsync()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, BaseUrl)
            {
                Headers =
                {
                    { "x-csrf-token", "fetch" },
                    { "sap-client", SapClient },
                    { "Authorization", AuthHeader }
                }
            };

            var response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"Erro ao obter o CSRF token: {response.StatusCode}. Detalhes: {responseContent}");
            }

            var csrfToken = response.Headers.TryGetValues("x-csrf-token", out var tokenValues)
                ? tokenValues.FirstOrDefault()
                : string.Empty;

            string sapSessionId = string.Empty;
            string sapUserContext = string.Empty;
            string sapMysapSso2 = string.Empty;

            Uri uri = new Uri(BaseUrl);
            foreach (Cookie cookie in _cookieContainer.GetCookies(uri))
            {
                if (cookie.Name == "MYSAPSSO2")
                    sapMysapSso2 = cookie.Value;
                else if (cookie.Name == "SAP_SESSIONID_DS4_110")
                    sapSessionId = cookie.Value;
                else if (cookie.Name == "sap-usercontext")
                    sapUserContext = cookie.Value;
            }

            return (csrfToken ?? "", sapSessionId ?? "", sapUserContext ?? "", sapMysapSso2 ?? "");
        }

        private async Task<string> CallPostWithCsrfAsync()
        {
            var (csrfToken, sapSessionId, sapUserContext, sapMysapSso2) = await FetchCsrfTokenAsync();

            var postData = new
            {
                Uname = "ZEUS_02",
                ObjectName = "",
                StartPurchorg = "",
                StartPurchgroup = "",
                EndPurchorg = "",
                EndPurchgroup = "",
                Status = false,
                OrgLevel = new string[] { },
                Objects = new[]
                {
                    new
                    {
                        Uname = "",
                        ObjectName = "",
                        Activities = new string[] { }
                    }
                }
            };

            var jsonContent = Newtonsoft.Json.JsonConvert.SerializeObject(postData);

            var request = new HttpRequestMessage(HttpMethod.Post, $"{BaseUrl}/AuthorizationSet")
            {
                Content = new StringContent(jsonContent, Encoding.UTF8, "application/json"),
                Headers =
                {
                     { "x-csrf-token", csrfToken },
                     { "sap-client", SapClient },
                     { "Authorization", AuthHeader },
                 //   { "MYSAPSSO2", sapMysapSso2 },
                //    { "SAP_SESSIONID_DS4_110", sapSessionId },
                 //   { "sap-usercontext", sapUserContext }
                }
            };

            var response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"Erro ao fazer a requisição POST: {response.StatusCode}. Detalhes: {responseContent}");
            }

            var responseContentString = await response.Content.ReadAsStringAsync();
            var contentType = response.Content.Headers.ContentType?.MediaType;
            Console.WriteLine($"Content-Type da resposta: {contentType}");

            // Exibir corpo da resposta
            Console.WriteLine("Corpo da resposta:");
            Console.WriteLine(responseContentString);

            // Retornar a resposta XML sem qualquer manipulação
            return responseContentString;
        }

        [HttpPost("serv")]
        public async Task<IActionResult> CallPost()
        {
            try
            {
                var xmlResponse = await CallPostWithCsrfAsync();
                return Content(xmlResponse, "application/xml"); // Retorna a resposta como XML
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
          // Adicionando a nova rota aqui
        [HttpPost("printnome")]
        public IActionResult PrintNome([FromBody] NomeRequest nomeRequest, [FromHeader] string gxtoken) // ESSE É A FORMA DE RECEBER O CORPO DA REQUISIÇÃO E DO HEADER 
        {
            try
            {
                // Imprime o nome que foi passado no corpo da requisição
                Console.WriteLine($"Nome recebido: {nomeRequest.Nome}");
                Console.WriteLine($"Token recebido: {gxtoken}");

                // Retorna o nome na resposta
                return Content($"Nome recebido: {nomeRequest.Nome}", "text/plain");
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}
