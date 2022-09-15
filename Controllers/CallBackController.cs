using Credential_Mvc_sample.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Credential_Mvc_sample.Controllers
{
    public class CallBackController : Controller
    {
        public async Task<ActionResult> Index(CredModel credModel)
        {
            Token token = new Token();
            try
            {

                byte[] plainTextBytes = Encoding.UTF8.GetBytes(credModel.ClientID + ":" + credModel.SecretCode);
                string key = System.Convert.ToBase64String(plainTextBytes);

                HttpClient _client = new HttpClient
                {
                    BaseAddress = new Uri(ConstantValue.BaseAddress + "token")
                };

                _client.DefaultRequestHeaders.Accept.Clear();
                _client.DefaultRequestHeaders.Add("authorization", "Basic " + key);
                _client.DefaultRequestHeaders.Add("Access-Control-Allow-Origin", "*");
                _client.DefaultRequestHeaders.Add("No-Auth", "true");
                _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));

                HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Post, ConstantValue.BaseAddress + "token")
                {
                    Content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                { "grant_type", "password" },
                { "username", credModel.UserName },
                { "password", credModel.Password },
                { "scope", credModel.Scopes }

                        //{ "grant_type", "client_credentials" },
                        //{ "scope", credModel.Scopes },
                        //{ "client_id", credModel.ClientID },
                        //{ "secret_key", credModel.SecretCode }
            })
                };

                HttpResponseMessage response = await _client.SendAsync(requestMessage);
                if (response.IsSuccessStatusCode)
                {
                    HttpContent responseContent = response.Content;
                    string responseString = responseContent.ReadAsStringAsync().Result;
                    dynamic tokenInfo = JObject.Parse(responseString);
                    token.AccessToken = tokenInfo.access_token;
                    token.ExpiresIn = tokenInfo.expires_in;
                    token.RefreshToken = tokenInfo.refresh_token;
                    token.TokenType = tokenInfo.token_type;
                    TempData["Token"] = token;                    
                    return RedirectToAction("Index", "TestAPIs", token);
                }
                else
                {
                    ViewBag.Error = "Error : " + response.ReasonPhrase;
                    return View(token);
                }
            }
            catch (Exception ex)
            {
                ViewBag.Message = "Error : " + ex.Message;
                return View(token);
            }
        }

        [HttpGet]
        public async Task<ActionResult> GetRefreshToken(string refresh_token)
        {
            Token token = new Token();
            try
            {

                string client_id = string.Empty;
                string secret_key = string.Empty;
                if (TempData.ContainsKey("ClientId"))
                    client_id = TempData.Peek("ClientId").ToString();

                if (TempData.ContainsKey("SecretCode"))
                    secret_key = TempData.Peek("SecretCode").ToString();
                HttpClientHandler handler = new HttpClientHandler();
                HttpClient client = new HttpClient(handler);
                string ClientIDandSecret = client_id + ":" + secret_key;
                string authorizationHeader = Convert.ToBase64String(Encoding.UTF8.GetBytes(ClientIDandSecret));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authorizationHeader);
                Dictionary<string, string> RequestBody = new Dictionary<string, string>
                {
                    {"grant_type", "refresh_token"},
                    {"refresh_token", refresh_token}
                };
                HttpResponseMessage tokenResponse = client.PostAsync(ConstantValue.BaseAddress + "token", new FormUrlEncodedContent(RequestBody)).Result;
                if (tokenResponse.IsSuccessStatusCode)
                {
                    HttpContent responseContent = tokenResponse.Content;
                    string responseString = responseContent.ReadAsStringAsync().Result;
                    dynamic tokenInfo = JObject.Parse(responseString);

                    token.AccessToken = tokenInfo.access_token;
                    token.ExpiresIn = tokenInfo.expires_in;
                    token.RefreshToken = tokenInfo.refresh_token;
                    token.TokenType = tokenInfo.token_type;
                    TempData["Token"] = token;
                    return View("Index", token);

                }
                else
                {
                    ViewBag.Error = "Error : " + tokenResponse.ReasonPhrase;
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