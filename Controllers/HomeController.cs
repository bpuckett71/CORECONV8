
using Credential_Mvc_sample.Models;
using System.Web.Mvc;

namespace Credential_Mvc_sample.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            CredModel obj = new CredModel();
            obj.UserName = "XXXXX";
            obj.Password = "XXXXX";
            obj.ClientID = ConstantValue.client_id;
            obj.SecretCode = ConstantValue.secretKey;
            obj.Scopes = "add,read,edit";
            return View(obj);
        }

        public ActionResult GetToken(CredModel authModel)
        {
            string authorizeUrl = ConstantValue.BaseAddress;

            if (ModelState.IsValid)
            {
                TempData["ClientId"] = authModel.ClientID;
                TempData["SecretCode"] = authModel.SecretCode;
                return RedirectToAction("Index", "callback", authModel);
            }
            else
            {
                return (View("Index"));
            }
        }

    }
}