using Helper.DBMlib;
using Login_Logout.Helper;
using Login_Logout.Models;
using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Web.Mvc;

namespace Login_Logout.Controllers
{
    public class UserLoginController : Controller
    {
        // GET: UserLogin
        public ActionResult Index()
        {
            if (Session["PERSON_ID"] != null)
            {
                return RedirectToAction("Index", "HomeVN");
            }
            return View();
        }

        public ActionResult Logout()
        {
            Session.Clear();

            return RedirectToAction("Index", "UserLogin");
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult Index(LoginModelClass lc)
        {


            string maincon = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
            using (OracleConnection con = new OracleConnection(maincon))
            {

                try
                {
                    con.Open();
                    OracleCommand cmd = new OracleCommand("HRM_WEB_G.HRM_WEB_LOGIN_CHECK", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Input parameters
                    cmd.Parameters.Add("P_PERSON_NUM", OracleDbType.Varchar2).Value = lc.UserCode;
                    cmd.Parameters.Add("P_PASS_WORD", OracleDbType.Varchar2).Value = lc.PassWord;

                    // Output parameters

                    cmd.Parameters.Add("P_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

                    cmd.ExecuteNonQuery();
                    List<LoginModelClass> login = new List<LoginModelClass>();
                    OracleDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        string username = lc.UserCode.ToString();
                        string password = lc.PassWord.ToString();

                        Session["Name"] = reader["NAME"].ToString();
                        Session["USER_CODE"] = lc.UserCode.ToString();
                        Session["PASS_WORD"] = lc.PassWord.ToString();
                        Session["PERSON_ID"] = reader["PERSON_ID"].ToString();
                        Session["DEPT_ID"] = reader["DEPT_ID"].ToString();

                        // save cookies
                        Response.Cookies.Add(Commons.GenCookie("uif", new
                        {
                            Name = reader["NAME"].ToString(),
                            USER_CODE = lc.UserCode.ToString(),
                            PERSON_ID = reader["PERSON_ID"].ToString(),
                            DEPT_ID = reader["DEPT_ID"].ToString(),
                        }.toJson(), DateTime.Now.AddDays(1)));

                        reader.Close();
                        if (String.Compare(username, password, true) == 0 || password == "1111")
                        {
                            return RedirectToAction("Changepassword", "UserLogin");
                        }
                        else
                        {
                            return RedirectToAction("Index", "HomeVN");
                        }
                    }
                    else
                    {
                        TempData["msg"] = "Account or password wrong!";

                    }
                    con.Close();
                }
                catch (Exception ex)
                {
                    TempData["msg"] = "Account or password wrong!!";
                }

                return View();
            }

        }

        public ActionResult Changepassword(string retype_password)
        {

            if (retype_password != null)
            {
                string maincon = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
                using (OracleConnection con = new OracleConnection(maincon))
                {
                    try
                    {
                        con.Open();
                        OracleCommand cmd = new OracleCommand("HRM_WEB_G.HRM_WEB_CHANGE_PASSWORD", con);
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Input parameter
                        cmd.Parameters.Add("P_PERSON_NUM", OracleDbType.Varchar2).Value = Session["USER_CODE"].ToString();
                        cmd.Parameters.Add("P_PASS_WORD", OracleDbType.Varchar2).Value = retype_password.Trim();
                        cmd.ExecuteNonQuery();
                        con.Clone();

                        return RedirectToAction("Index", "HomeVN");
                    }
                    catch (Exception ex)
                    {
                        TempData["msg"] = "Account or password wrong!";
                    }
                }
            }
            return View();
        }
    }
}