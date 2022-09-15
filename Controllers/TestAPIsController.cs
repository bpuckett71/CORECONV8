using Credential_Mvc_sample.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Credential_Mvc_sample.Controllers
{
    public class TestAPIsController : Controller
    {
        // GET: TestAPIs
        public ActionResult Index(Token token)
        {
            return View(token);
        }

        public ActionResult ProjectFinancial(Token token)
        {
            return RedirectToAction("Index", "ProjectFinancial", token);
        }

        public ActionResult LaborTime(Token token)
        {
            return RedirectToAction("Index", "LaborTime", token);
        }
        public ActionResult EquipmentTime(Token token)
        {
            return RedirectToAction("Index", "EquipmentTime", token);
        }
        public ActionResult EmployeeMiscExpenses(Token token)
        {
            return RedirectToAction("Index", "EmployeeMiscExpenses", token);
        }
    }
}