using Credential_Mvc_sample.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Credential_Mvc_sample.Controllers
{
    public class ProjectFinancialController : Controller
    {
        // GET: ProjectFinancial
        public ActionResult Index(Token token)
        {
            TempData["Token"] = token;
            ViewBag.Message = "";
            return View(token);
        }

        public async Task<ActionResult> GetProjectFinancials(string accessToken, string apiURI)
        {
            Token token = TempData.Peek("Token") as Token;
            try
            {
                HttpClientHandler handler = new HttpClientHandler();
                HttpClient client = new HttpClient(handler);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                client.DefaultRequestHeaders.Add("Access-Control-Allow-Origin", "*");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                string uri = ConstantValue.BaseAddress
                    + "api/ProjectFinancial/" + apiURI + "?accountId=" + ConstantValue.account_id + "&integrationKey=" + ConstantValue.client_id + "&page=1&itemPerPage=2";
                HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Post, uri);
                Dictionary<string, object> data = new Dictionary<string, object>();
                var projects = new[]
                {
                new { ProjectNumber="GC-001", PrimeContractNumber="001,002,003"},
                new { ProjectNumber="GC-002", PrimeContractNumber="001,002,003"},
            };
                data.Add("Project", projects);

                var date = new { FromDate = "01-Jan-2022", ToDate = "04-Mar-2022" };
                data.Add("Date", date);
                data.Add("Timezone", "India Standard Time");
                requestMessage.Content = new StringContent(JsonConvert.SerializeObject(data), System.Text.Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.SendAsync(requestMessage);
                if (response.IsSuccessStatusCode)
                {
                    var JsonContent = response.Content.ReadAsStringAsync().Result;
                    string Message = JsonContent.ToString();
                    ViewBag.Message = Message;
                    ViewBag.JsonData = JValue.Parse(Message).ToString(Formatting.Indented);

                    return View("Index", token);
                }
                else
                {
                    ViewBag.Message = "Error : " + response.ReasonPhrase;
                    return View("Index", token);
                }
            }
            catch (Exception ex)
            {
                ViewBag.Message = "Error : " + ex.Message;
                return View("Index", token);
            }
        }
    }
}