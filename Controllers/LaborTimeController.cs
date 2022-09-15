using Credential_Mvc_sample.Models;
using CoreconWebApi.ExternalApiDto;
using Newtonsoft.Json;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Credential_Mvc_sample.Controllers
{
    public class LaborTimeController : Controller
    {
        // GET: LaborTime
        public ActionResult Index(Token token)
        {
            TempData["Token"] = token;
            return View(new LaborTimeCard());            
        }

        [HttpPost]
        public async Task<ActionResult> ImportExcel(FormCollection formCollection)
        {
            ViewBag.Response = "fileinput";
            Token token = TempData.Peek("Token") as Token;
            try
            {
                HttpClientHandler handler = new HttpClientHandler();
                HttpClient client = new HttpClient(handler);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.AccessToken);
                client.DefaultRequestHeaders.Add("Access-Control-Allow-Origin", "*");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                string uri = ConstantValue.BaseAddress
                    + "api/LaborTime/Add?accountId=" + ConstantValue.account_id + "&integrationKey=" + ConstantValue.client_id + "&userTimeZone=India Standard Time";
                HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Post, uri);

                //File data
                List<GenericAddDto> list = new List<GenericAddDto>();
                HttpPostedFileBase file = Request.Files["UploadedFile"];
                if ((file != null) && (file.ContentLength > 0) && !string.IsNullOrEmpty(file.FileName))
                {
                    string fileName = file.FileName;
                    string fileContentType = file.ContentType;
                    byte[] fileBytes = new byte[file.ContentLength];
                    var filedata = file.InputStream.Read(fileBytes, 0, Convert.ToInt32(file.ContentLength));
                    GenericAddDto importExcelDto = null;
                    Dictionary<string, string> dicObj = null;
                    using (ExcelPackage package = new ExcelPackage(file.InputStream))
                    {
                        var currentSheet = package.Workbook.Worksheets;
                        var workSheet = currentSheet.First();
                        var noOfCol = workSheet.Dimension.End.Column;
                        var noOfRow = workSheet.Dimension.End.Row;
                        for (int row = 2; row <= noOfRow; row++)
                        {
                            importExcelDto = new GenericAddDto();
                            dicObj = new Dictionary<string, string>();
                            importExcelDto.UserName = "A Sharma";
                            importExcelDto.UserCompanyName = "Company name-Test";
                            for (int col = 1; col <= noOfCol; col++)
                            {

                                if (workSheet.Cells[row, col].Value != null && workSheet.Cells[row, col].Value.GetType() != typeof(string))
                                {
                                    var date = DateTime.FromOADate(double.Parse(Convert.ToString(workSheet.Cells[row, col].Value))).ToString("MM/dd/yyyy");
                                    if (DateTime.ParseExact(date, "MM/dd/yyyy", null) > DateTime.ParseExact("01-01-1901", "MM/dd/yyyy", null))
                                    {
                                        dicObj.Add(Convert.ToString(workSheet.Cells[1, col].Value), date);
                                    }
                                    else
                                    {
                                        dicObj.Add(Convert.ToString(workSheet.Cells[1, col].Value), Convert.ToString(workSheet.Cells[row, col].Value));
                                    }
                                }
                                else
                                {
                                    dicObj.Add(Convert.ToString(workSheet.Cells[1, col].Value), Convert.ToString(workSheet.Cells[row, col].Value));
                                }
                            }
                            importExcelDto.Collection = dicObj;
                            list.Add(importExcelDto);
                        }
                    }
                }

                requestMessage.Content = new StringContent(JsonConvert.SerializeObject(list), System.Text.Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.SendAsync(requestMessage);
                if (response.IsSuccessStatusCode)
                {
                    var JsonContent = response.Content.ReadAsStringAsync().Result;
                    string Message = JsonContent.ToString();
                    ViewBag.Message = Message;
                    return View("Index", new LaborTimeCard());
                }
                else
                {
                    ViewBag.Message = "Error : " + response.ReasonPhrase;
                    return View("Index", new LaborTimeCard());
                }
            }
            catch (Exception ex)
            {
                ViewBag.Message = "Error : " + ex.Message;
                return View("Index", new LaborTimeCard());
            }
        }

        [HttpPost]
        public ActionResult JsonTimeData(LaborTimeCard laborTimeCard)
        {
            ViewBag.Response = "forminput";
            if (ModelState.IsValid)
            {
                try
                {
                    List<GenericAddDto> list = new List<GenericAddDto>();
                    GenericAddDto importExcelDto = new GenericAddDto();
                    Dictionary<string, string> dicObj = new Dictionary<string, string>();
                    importExcelDto.UserName = "A Sharma";
                    importExcelDto.UserCompanyName = "Company name-Test";

                    PropertyInfo[] infos = laborTimeCard.GetType().GetProperties();
                    foreach (PropertyInfo info in infos)
                    {
                        dicObj.Add(info.Name, Convert.ToString(info.GetValue(laborTimeCard, null)));
                    }
                    importExcelDto.Collection = dicObj;
                    list.Add(importExcelDto);
                    TempData["JsonData"] = Convert.ToString(JsonConvert.SerializeObject(list));
                    TempData["TimeCard"] = laborTimeCard;
                    ViewBag.DataGenerate = "yes";
                    return View("Index", laborTimeCard);
                }
                catch (Exception ex)
                {
                    ViewBag.Message = "Error : " + ex.Message;
                    return View("Index", laborTimeCard);
                }
            }
            else
            {
                return View("Index", laborTimeCard);
            }
        }


        public async Task<ActionResult> PostTimeCard()
        {
            ViewBag.Response = "forminput";
            Token token = TempData.Peek("Token") as Token;
            LaborTimeCard laborTimeCard = TempData.Peek("TimeCard") as LaborTimeCard;
            try
            {
                string jsonData = TempData.Peek("JsonData") as string;
                HttpClientHandler handler = new HttpClientHandler();
                HttpClient client = new HttpClient(handler);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.AccessToken);
                client.DefaultRequestHeaders.Add("Access-Control-Allow-Origin", "*");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                string uri = ConstantValue.BaseAddress
                    + "api/LaborTime/Add?accountId=" + ConstantValue.account_id + "&integrationKey=" + ConstantValue.client_id + "&userTimeZone=India Standard Time";
                HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Post, uri);

                requestMessage.Content = new StringContent(jsonData, System.Text.Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.SendAsync(requestMessage);
                if (response.IsSuccessStatusCode)
                {
                    var JsonContent = response.Content.ReadAsStringAsync().Result;
                    string Message = JsonContent.ToString();
                    ViewBag.Message = Message;
                    return View("Index", laborTimeCard);
                }
                else
                {
                    ViewBag.Message = "Error : " + response.ReasonPhrase;
                    return View("Index", laborTimeCard);
                }
            }
            catch (Exception ex)
            {
                ViewBag.Message = "Error : " + ex.Message;
                return View("Index", laborTimeCard);
            }

        }

        public ActionResult FormInput()
        {
            ViewBag.Response = "forminput";
            return View("Index", new LaborTimeCard());
        }

        public ActionResult FileInput()
        {
            ViewBag.Response = "fileinput";
            return View("Index", new LaborTimeCard());
        }
    }
}
