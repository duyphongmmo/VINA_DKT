
using Helper.DBMlib;
using Login_Logout.Helper;
using Login_Logout.Models;
using Login_Logout.Service;
using Newtonsoft.Json;
using OfficeOpenXml;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.Mvc;

namespace Login_Logout.Controllers
{
    [CustomAuthenticationFilter]
    public class AccessManageController : Controller
    {
        private AccessManageService _accessManage;
        public AccessManageController()
        {
            _accessManage = new AccessManageService();
        }


        // GET: AccessManage
        public ActionResult Index(string date, string s, int? uid)
        {
            try
            {
                date = date ?? DateTime.Now.ToString("yyyy-MM-dd"); 
                var dta = _accessManage.getList(date, s);

                ViewBag.listData = dta.Item1;
                ViewBag.total = dta.Item2;

                if (uid != null)
                {
                    ViewBag.userif = _accessManage.GetUserIf(uid);
                }
            }
            catch (Exception)
            {
                TempData["msg"] = "SYSTEM_ERROR";
            }

            return View();
        }

        [HttpPost]
        public ActionResult Report(string type, string s)
        {
            List<HrmPerson> list = new List<HrmPerson>();

            return Content(list.toJson(), "application/json");
        }

        [HttpPost]
        public ActionResult ConfirmDuty(int dutyID)
        {
            //DBM
            try
            {
                _accessManage.ConfirmDuty(dutyID);
                return Content(new
                {
                    isOK = true,
                }.toJson(), "application/json");
            }
            catch (Exception e)
            {
                return Content(new
                {
                    msg = e.ToString()
                }.toJson(), "application/json");
            }
        }
        public ActionResult ExportToExcel()
        {

            var excelData = _accessManage.ExportExcel();
            var contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            var fileName = $"Employees-{DateTime.Now.ToString("yyyy-MM-dd")}.xlsx";

            return File(excelData, contentType, fileName);

        }

        public ActionResult IndexOut(string type, string s)
        {
            return View();
        }

        public ActionResult GuestIn(string s, string type, DateTime? date)
        {
            Console.WriteLine();
            DateTime time = new DateTime(2024, 04, 23);
            List<HrmPerson> lst = new List<HrmPerson>();
            using (var con = DBM.createConnection())
            {
                string sql = "SELECT PERSON_ID, PERSON_NUM, NAME, BIRTHDAY, LAST_UPDATE_DATE " +
                    "FROM HRM_PERSON_MASTER " +
                    "WHERE LAST_UPDATE_DATE >= :startDate AND LAST_UPDATE_DATE < :startDate + 1 order by LAST_UPDATE_DATE";
                //
                OracleCommand command = new OracleCommand(sql, con);
                command.Parameters.Add(":startDate", OracleDbType.Date).Value = time;
                using (var rd = command.ExecuteReader())
                {
                    lst = rd.ToList<HrmPerson>();
                }
            }

            var ck = Commons.GenCookie("user", JsonConvert.SerializeObject(new
            {
                user_no = "42034234"
            }), DateTime.Now.AddMinutes(1));
            Response.Cookies.Add(ck);
            Session["msg"] = "Hello World";
            return View();
        }

        
        public ActionResult Dashboard()
        {
            var d = DateTime.Today;
            OracleParameter[] cmds =
            {
                   new OracleParameter("P_YEAR", OracleDbType.Varchar2) { Value = "2024"},
                   new OracleParameter("P_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output)
            };
            List<reportm> reportms = DBM.ExecuteReader<reportm>("HRM_WEB_G2.Report", CommandType.StoredProcedure, cmds);

            return View();
        }
    }
}