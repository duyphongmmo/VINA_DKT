using Login_Logout.Models;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Web.Mvc;

namespace Login_Logout.Controllers
{
    public class HomeVNController : Controller
    {
        // GET: HomeVN
        public ActionResult Index()
        {
            return View();
        }

        /// MONTH TOTAL
        public List<BaseSalary> monthTotal(String Month)
        {
            List<BaseSalary> basesalaryList = new List<BaseSalary>();
            string maincon = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
            using (OracleConnection orclconn = new OracleConnection(maincon))
            {
                try
                {
                    orclconn.Open();
                    OracleCommand cmd = new OracleCommand("HRM_WEB_G.SELECT_PAYMENT_SPREAD_VN2", orclconn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Output parameters
                    OracleParameter output = cmd.Parameters.Add("P_CURSOR", OracleDbType.RefCursor);
                    output.Direction = ParameterDirection.Output;

                    // Input parameters
                    cmd.Parameters.Add("W_CORP_ID", OracleDbType.Int64).Value = 25;
                    cmd.Parameters.Add("W_PAY_YYYYMM", OracleDbType.Varchar2).Value = Month.ToString();
                    cmd.Parameters.Add("W_WAGE_TYPE", OracleDbType.Varchar2).Value = "P1";
                    cmd.Parameters.Add("W_PERSON_ID", OracleDbType.Int64).Value = Session["PERSON_ID"].ToString(); ;
                    cmd.Parameters.Add("W_PAY_TYPE", OracleDbType.Int64).Value = 1;
                    cmd.Parameters.Add("P_CONNECT_PERSON_ID", OracleDbType.Int64).Value = 0;
                    cmd.Parameters.Add("W_SOB_ID", OracleDbType.Int64).Value = 70;
                    cmd.Parameters.Add("W_ORG_ID", OracleDbType.Int64).Value = 701;

                    OracleDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        BaseSalary basesalary = new BaseSalary();
                        {
                            basesalary.number = reader["PERSON_NUM"].ToString();   // Mã Nhân Viên
                            basesalary.name = reader["NAME"].ToString();           // Tên Nhân Viên
                            basesalary.P02 = (reader["P02"] != DBNull.Value) ? reader["P02"].ToString() : "0"; //  Lương Cơ bản
                            basesalary.P37 = (reader["P37"] != DBNull.Value) ? reader["P37"].ToString() : "0"; //  PC Chức Vụ
                            basesalary.P44 = (reader["P44"] != DBNull.Value) ? reader["P44"].ToString() : "0"; //  PC Đời Sống
                            basesalary.P43 = (reader["P43"] != DBNull.Value) ? reader["P43"].ToString() : "0"; //  PC Chuyên Cần
                            basesalary.P47 = (reader["P47"] != DBNull.Value) ? reader["P47"].ToString() : "0"; //  PC Kiểm Tra
                            basesalary.A37 = (reader["A37"] != DBNull.Value) ? reader["A37"].ToString() : "0"; //  Thưởng Kiểm Tra
                            basesalary.P45 = (reader["P45"] != DBNull.Value) ? reader["P45"].ToString() : "0"; //  PC Quần Áo
                            basesalary.P41 = (reader["P41"] != DBNull.Value) ? reader["P41"].ToString() : "0"; //  PC Thâm Niên
                            basesalary.P38 = (reader["P38"] != DBNull.Value) ? reader["P38"].ToString() : "0"; //  PC AN toàn PCCC
                            basesalary.P40 = (reader["P40"] != DBNull.Value) ? reader["P40"].ToString() : "0"; //  Thưởng
                            basesalary.P39 = (reader["P39"] != DBNull.Value) ? reader["P39"].ToString() : "0"; //  Trợ cấp điện thoại
                            basesalary.P46 = (reader["P46"] != DBNull.Value) ? reader["P46"].ToString() : "0"; //  P/C Sửa hàng
                            basesalary.P42 = (reader["P42"] != DBNull.Value) ? reader["P42"].ToString() : "0"; //  P/C thâm niên kỹ thuật

                        }
                        basesalaryList.Add(basesalary);
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    TempData["msg"] = "Server Error!";
                }
            }
            return basesalaryList;
        }

        public ActionResult IndexVN(DateTime? Month, int? page)
        {
            string maincon = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
            using (OracleConnection orclconn = new OracleConnection(maincon))
            {
                List<Salarystandards> salarystandards = new List<Salarystandards>();
                AllSalary ALLSALARY = new AllSalary();

                try
                {
                    if (Month != null)
                    {
                        DateTime now = DateTime.Now;
                        string SalaryMonth = Month.Value.ToString("yyyy-MM-dd");

                        if (now.Day < 10 && now.Month - 1 == Month.Value.Month)
                        {
                            ViewBag.Message = "Phòng nhân sự chưa chốt công!";
                        }
                        else
                        {
                            orclconn.Open();
                            OracleCommand cmd = new OracleCommand("HRM_WEB_G.SELECT_PAYMENT_SPREAD_VN", orclconn);
                            cmd.CommandType = CommandType.StoredProcedure;

                            // Output parameters
                            OracleParameter output = cmd.Parameters.Add("P_CURSOR", OracleDbType.RefCursor);
                            output.Direction = ParameterDirection.Output;

                            // Input parameters
                            cmd.Parameters.Add("W_CORP_ID", OracleDbType.Int64).Value = 25;
                            cmd.Parameters.Add("W_PAY_YYYYMM", OracleDbType.Varchar2).Value = SalaryMonth.Substring(0, 7);
                            cmd.Parameters.Add("W_WAGE_TYPE", OracleDbType.Varchar2).Value = "P1";
                            cmd.Parameters.Add("W_PERSON_ID", OracleDbType.Int64).Value = Session["PERSON_ID"].ToString();
                            cmd.Parameters.Add("P_PERSON_NUM", OracleDbType.Varchar2).Value = Session["USER_CODE"].ToString();
                            cmd.Parameters.Add("W_PAY_TYPE", OracleDbType.Int64).Value = 1;
                            cmd.Parameters.Add("P_CONNECT_PERSON_ID", OracleDbType.Int64).Value = 0;
                            cmd.Parameters.Add("W_SOB_ID", OracleDbType.Int64).Value = 70;
                            cmd.Parameters.Add("W_ORG_ID", OracleDbType.Int64).Value = 701;

                            OracleDataReader reader = cmd.ExecuteReader();
                            if (reader.Read())
                            {
                                Salarystandards balary = new Salarystandards();
                                {
                                    balary.Month = reader["PAY_YYYYMM"].ToString();   // tháng
                                    balary.Mumber = reader["PERSON_NUM"].ToString();   // Mã Nhân Viên
                                    balary.name = reader["NAME"].ToString();         // Tên Nhân Viên
                                    balary.basesalary = (reader["P02"] != DBNull.Value) ? reader["P02"].ToString() : "0";   //  Lương Cơ bản
                                    balary.position = (reader["P37"] != DBNull.Value) ? reader["P37"].ToString() : "0";   //  PC Chức Vụ
                                    balary.libing = (reader["P44"] != DBNull.Value) ? reader["P44"].ToString() : "0";   //  PC Đời Sống
                                    balary.punctuality = (reader["P43"] != DBNull.Value) ? reader["P43"].ToString() : "0";   //  PC Chuyên Cần
                                    balary.checksub = (reader["P47"] != DBNull.Value) ? reader["P47"].ToString() : "0";   //  PC Kiểm Tra
                                    balary.compensate = (reader["A37"] != DBNull.Value) ? reader["A37"].ToString() : "0";   //  Thưởng Kiểm Tra
                                    balary.clothes = (reader["P45"] != DBNull.Value) ? reader["P45"].ToString() : "0";   //  PC Quần Áo
                                    balary.seniority = (reader["P41"] != DBNull.Value) ? reader["P41"].ToString() : "0";   //  PC Thâm Niên
                                    balary.fire = (reader["P38"] != DBNull.Value) ? reader["P38"].ToString() : "0";   //  PC AN toàn PCCC
                                    balary.Bouns = (reader["P40"] != DBNull.Value) ? reader["P40"].ToString() : "0";   //  Thưởng

                                    /* /////////////// /////////  tabel 2 /////////////////////////////////*/

                                    balary.P39 = (reader["P39"] != DBNull.Value) ? reader["P39"].ToString() : "0"; //  trợ cấp điện thoại
                                    balary.P46 = (reader["P46"] != DBNull.Value) ? reader["P46"].ToString() : "0"; //  P/C Sửa hàng
                                    balary.P42 = (reader["P42"] != DBNull.Value) ? reader["P42"].ToString() : "0"; //  P/C thâm niên kỹ thuật
                                    balary.P10 = (reader["P10"] != DBNull.Value) ? reader["P10"].ToString() : "0"; //  OT ca ngày (150%)
                                    balary.P12 = (reader["P12"] != DBNull.Value) ? reader["P12"].ToString() : "0"; //  OT ca ngày(210%)
                                    balary.P24 = (reader["P24"] != DBNull.Value) ? reader["P24"].ToString() : "0"; //  OT ca đêm (200%)
                                    balary.P26 = (reader["P26"] != DBNull.Value) ? reader["P26"].ToString() : "0"; //  OT ca đêm (215%)
                                    balary.P28 = (reader["P28"] != DBNull.Value) ? reader["P28"].ToString() : "0"; //  TC ca đêm(30%)
                                    balary.P14 = (reader["P14"] != DBNull.Value) ? reader["P14"].ToString() : "0"; //  OT ca ngày CN(200%)
                                    balary.P16 = (reader["P16"] != DBNull.Value) ? reader["P16"].ToString() : "0"; //  OT ca đêm CN(240%)
                                    balary.P18 = (reader["P18"] != DBNull.Value) ? reader["P18"].ToString() : "0"; //  OT ca đêm CN(270%)
                                    balary.P20 = (reader["P20"] != DBNull.Value) ? reader["P20"].ToString() : "0"; //  OT ca ngày Lễ(300%)
                                    balary.P22 = (reader["P22"] != DBNull.Value) ? reader["P22"].ToString() : "0"; //  OT ca đêm Lễ(390%)

                                    /* /////////////// /////////  tabel 3 /////////////////////////////////*/

                                    balary.P30 = (reader["P30"] != DBNull.Value) ? reader["P30"].ToString() : "0"; //  Trợ cấp 45p
                                    balary.P32 = (reader["P32"] != DBNull.Value) ? reader["P32"].ToString() : "0"; //  Hỗ trợ làm đêm
                                    /* balary.P42 = (reader["P42"] != DBNull.Value) ? reader["P42"].ToString() : "0"; //  Giờ trừ*/
                                    balary.P48 = (reader["P48"] != DBNull.Value) ? reader["P48"].ToString() : "0"; //  Hỗ trợ Nuôi con nhỏ
                                    balary.P49 = (reader["P49"] != DBNull.Value) ? reader["P49"].ToString() : "0"; //  Trợ cấp Sinh lý(Phụ Nữ)
                                    balary.P93 = (reader["P93"] != DBNull.Value) ? reader["P93"].ToString() : "0"; //  Nghỉ lý do công ty
                                    balary.P94 = (reader["P94"] != DBNull.Value) ? reader["P94"].ToString() : "0"; //  Thưởng giới thiệu
                                    balary.P95 = (reader["P95"] != DBNull.Value) ? reader["P95"].ToString() : "0"; //  Điều chỉnh lương

                                    /* /////////////// /////////  Khấu Trừ /////////////////////////////////*/

                                    balary.P76 = (reader["P76"] != DBNull.Value) ? reader["P76"].ToString() : "0"; //  BHXH
                                    balary.P77 = (reader["P77"] != DBNull.Value) ? reader["P77"].ToString() : "0"; //  BHTN
                                    balary.P78 = (reader["P78"] != DBNull.Value) ? reader["P78"].ToString() : "0"; //  BHYT 
                                    balary.P79 = (reader["P79"] != DBNull.Value) ? reader["P79"].ToString() : "0"; //  Bồi Thường 
                                    balary.P87 = (reader["P87"] != DBNull.Value) ? reader["P87"].ToString() : "0"; //  Thuế thu nhập 
                                    balary.P75 = (reader["P75"] != DBNull.Value) ? reader["P75"].ToString() : "0"; //  Phí Công đoàn 
                                    balary.P63 = (reader["P63"] != DBNull.Value) ? reader["P63"].ToString() : "0"; //  Mua thêm quần áo

                                    /* /////////////// /////////  Tổng  /////////////////////////////////*/

                                    balary.P56 = (reader["P56"] != DBNull.Value) ? reader["P56"].ToString() : "0"; //  Tổng thu nhập
                                    balary.P90 = (reader["P90"] != DBNull.Value) ? reader["P90"].ToString() : "0"; //  Tổng khấu trừ
                                    balary.P91 = (reader["P91"] != DBNull.Value) ? reader["P91"].ToString() : "0"; //   quyết toán thuế cuối năm
                                    balary.P92 = (reader["P92"] != DBNull.Value) ? reader["P92"].ToString() : "0"; //  Lương thực lĩnh

                                    int balaryP90 = int.Parse(balary.P90);
                                    int balaryP91 = int.Parse(balary.P91);
                                    balary.P90 = (balaryP90 - balaryP91).ToString(); //  Tổng khấu trừ
                                }
                                salarystandards.Add(balary);
                            }
                            reader.Close();
                            ALLSALARY.ListSalary = salarystandards;
                            List<BaseSalary> baseSalaryList = monthTotal(SalaryMonth.Substring(0, 7));
                            ALLSALARY.ListBase = baseSalaryList;
                        };
                    };
                }
                catch (Exception ex)
                {
                    TempData["msg"] = "Server Error!";
                }
                return View(ALLSALARY);
            }
        }


        // From search betwwen day and Month
        public ActionResult IndexDayMonthVN(DateTime? Month, int? page)
        {
            string maincon = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
            List<DayClass> ec = new List<DayClass>();
            Response response = new Models.Response();

            if (Month != null)
            {
                DateTime firstDayOfMonth = new DateTime(Month.Value.Year, Month.Value.Month, 1);
                DateTime lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

                string Fromdate = firstDayOfMonth.ToString("yyyy-MM-dd");
                string Todate = lastDayOfMonth.ToString("yyyy-MM-dd");
                using (OracleConnection orclconn = new OracleConnection(maincon))
                {
                    orclconn.Open();
                    OracleCommand cmd = new OracleCommand("HRM_WEB_G.DAY_DUTY", orclconn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Output parameters
                    OracleParameter output = cmd.Parameters.Add("P_CURSOR", OracleDbType.RefCursor);
                    output.Direction = ParameterDirection.Output;

                    // Input parameters
                    cmd.Parameters.Add("W_CORP_ID", OracleDbType.Int64).Value = 25;
                    cmd.Parameters.Add("W_START_DATE", OracleDbType.Date).Value = DateTime.ParseExact(Fromdate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                    cmd.Parameters.Add("W_END_DATE", OracleDbType.Date).Value = DateTime.ParseExact(Todate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                    cmd.Parameters.Add("W_SOB_ID", OracleDbType.Int64).Value = 70;
                    cmd.Parameters.Add("W_ORG_ID", OracleDbType.Int64).Value = 701;
                    cmd.Parameters.Add("W_PERSON_ID", OracleDbType.Int64).Value = 0; /*Session["PERSON_ID"].ToString(); */
                    cmd.Parameters.Add("W_CONNECT_PERSON_ID", OracleDbType.Int64).Value = Session["PERSON_ID"].ToString();
                    cmd.Parameters.Add("P_PERSON_NUM", OracleDbType.Varchar2).Value = Session["USER_CODE"].ToString();

                    cmd.ExecuteNonQuery();
                    OracleDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        DayClass search = new DayClass();
                        {
                            search.Workdate = ((DateTime)reader["WORK_DATE"]).ToString("yyyy/MM/dd");
                            search.Week = reader["WORK_WEEK"].ToString();
                            search.Personbumber = reader["PERSON_NUM"].ToString();
                            search.Username = reader["NAME"].ToString();
                            search.Workcenter = reader["DEPT_NAME"].ToString();
                            search.Dutyname = reader["DUTY_NAME"].ToString();
                            search.Holyname = reader["HOLY_TYPE_NAME"].ToString();
                            search.Opentime = (reader["OPEN_TIME"] != DBNull.Value) ? DateTime.Parse(reader["OPEN_TIME"].ToString()).ToString("yyyy-MM-dd HH:mm:ss") : "";
                            search.Closetime = (reader["CLOSE_TIME"] != DBNull.Value) ? DateTime.Parse(reader["CLOSE_TIME"].ToString()).ToString("yyyy-MM-dd HH:mm:ss") : "";
                            search.BeforeStart = (reader["BEFORE_OT_START"] != DBNull.Value) ? DateTime.Parse(reader["BEFORE_OT_START"].ToString()).ToString("yyyy-MM-dd HH:mm:ss") : "";
                            search.BeforeEnd = (reader["BEFORE_OT_END"] != DBNull.Value) ? DateTime.Parse(reader["BEFORE_OT_END"].ToString()).ToString("yyyy-MM-dd HH:mm:ss") : "";
                            /*     search.BeforeStart = reader["PL_BEFORE_OT_START"].ToString();
                                 search.BeforeEnd = reader["PL_BEFORE_OT_END"].ToString();*/
                            search.AfterStart = (reader["AFTER_OT_START"] != DBNull.Value) ? DateTime.Parse(reader["AFTER_OT_START"].ToString()).ToString("yyyy-MM-dd HH:mm:ss") : "";
                            search.AfterEnd = (reader["AFTER_OT_START"] != DBNull.Value) ? DateTime.Parse(reader["AFTER_OT_END"].ToString()).ToString("yyyy-MM-dd HH:mm:ss") : "";
                            search.Next = reader["DANGJIK_YN"].ToString();
                            search.AllNight = reader["ALL_NIGHT_YN"].ToString();
                            search.Restot = reader["REST_OT_1"].ToString();
                            search.leaveTime = (reader["LEAVE_TIME"] != DBNull.Value) ? Convert.ToDecimal(reader["LEAVE_TIME"].ToString()).ToString("#,##0.00") : "";

                            double lunchTime;
                            double breakFastTime;
                            double dinnerTime;
                            double midNightTime;
                            double TotalTime;

                            string lunchTimeString;
                            string breakFastTimeString;
                            string dinnerTimeString;
                            string midNightTimeString;

                            if (reader["BREAKFAST_TIME"] != DBNull.Value)
                            {
                                breakFastTimeString = reader["BREAKFAST_TIME"].ToString();
                                breakFastTimeString = breakFastTimeString.TrimEnd('h');
                                breakFastTime = Convert.ToDouble(breakFastTimeString);
                            }
                            else
                            {
                                breakFastTime = 0;
                            }


                            if (reader["LUNCH_TIME"] != DBNull.Value)
                            {
                                lunchTimeString = reader["LUNCH_TIME"].ToString();
                                lunchTimeString = lunchTimeString.TrimEnd('h');
                                lunchTime = Convert.ToDouble(lunchTimeString);
                            }
                            else
                            {
                                lunchTime = 0;
                            }

                            if (reader["DINNER_TIME"] != DBNull.Value)
                            {
                                dinnerTimeString = reader["DINNER_TIME"].ToString();
                                dinnerTimeString = dinnerTimeString.TrimEnd('h');
                                dinnerTime = Convert.ToDouble(dinnerTimeString);
                            }
                            else
                            {
                                dinnerTime = 0;
                            }

                            if (reader["MIDNIGHT_TIME"] != DBNull.Value)
                            {
                                midNightTimeString = reader["MIDNIGHT_TIME"].ToString();
                                midNightTimeString = midNightTimeString.TrimEnd('h');
                                midNightTime = Convert.ToDouble(midNightTimeString);
                            }
                            else
                            {
                                midNightTime = 0;
                            }

                            TotalTime = breakFastTime + lunchTime + dinnerTime + midNightTime;

                            if (TotalTime != 0)
                            {
                                search.Breakfast = TotalTime.ToString();
                            }
                            else
                            {
                                search.Breakfast = null;
                            }
                        }
                        ec.Add(search);
                    }
                    reader.Close();
                    response.StatusList = ec;
                    List<Salarystatus> salarystatusList = monthTotalWorking(Fromdate.Substring(0, 7));
                    response.TotalList = salarystatusList;
                }

            }

            return View(response);
        }


        /// MONTH TOTAL WORKING 
        public List<Salarystatus> monthTotalWorking(String Month)
        {
            List<Salarystatus> salarystatusList = new List<Salarystatus>();
            string maincon = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
            using (OracleConnection orclconn = new OracleConnection(maincon))
            {
                try
                {
                    orclconn.Open();
                    OracleCommand cmd = new OracleCommand("HRM_WEB_G.MONTH_DUTY", orclconn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Output parameters
                    OracleParameter output = cmd.Parameters.Add("P_CURSOR3", OracleDbType.RefCursor);
                    output.Direction = ParameterDirection.Output;

                    // Input parameters
                    cmd.Parameters.Add("W_CORP_ID", OracleDbType.Int64).Value = 25;
                    cmd.Parameters.Add("W_PAY_YYYYMM", OracleDbType.Varchar2).Value = Month.ToString();
                    cmd.Parameters.Add("W_PERSON_NUM", OracleDbType.Varchar2).Value = Session["USER_CODE"].ToString();
                    cmd.Parameters.Add("W_SOB_ID", OracleDbType.Int64).Value = 70;

                    OracleDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        Salarystatus salarystatus = new Salarystatus();
                        {
                            salarystatus.person_number = reader["PERSON_NUM"].ToString();   // Mã Nhân Viên
                            salarystatus.name = reader["NAME"].ToString();         // Tên Nhân Viên
                            salarystatus.monthSearch = reader["DUTY_YYYYMM"].ToString();
                            salarystatus.TOTAL_ATT_DAY = (reader["TOTAL_ATT_DAY"] != DBNull.Value) ? reader["TOTAL_ATT_DAY"].ToString() : "0";   // Số ngày công
                            salarystatus.TOTAL_DED_DAY = (reader["TOTAL_DED_DAY"] != DBNull.Value) ? reader["TOTAL_DED_DAY"].ToString() : "0";   // Số ngày nghỉ
                            salarystatus.OT_150 = (reader["OT_150"] != DBNull.Value) ? reader["OT_150"].ToString() : "0";                 // OT ca ngày (150%)
                            salarystatus.OT_210 = (reader["OT_210"] != DBNull.Value) ? reader["OT_210"].ToString() : "0";                 // OT ca ngày(210%)
                            salarystatus.HOLY_0_200 = (reader["HOLY_0_200"] != DBNull.Value) ? reader["HOLY_0_200"].ToString() : "0";         //  OT ca đêm (200%)
                            salarystatus.NGITH_215 = (reader["NGITH_215"] != DBNull.Value) ? reader["NGITH_215"].ToString() : "0";           // OT ca đêm (215%)
                            salarystatus.NIGHT_BONUS_30 = (reader["NIGHT_BONUS_30"] != DBNull.Value) ? reader["NIGHT_BONUS_30"].ToString() : "0"; //  TC ca đêm(30%)
                            salarystatus.HOLY_D_200 = (reader["HOLY_D_200"] != DBNull.Value) ? reader["HOLY_D_200"].ToString() : "0";         //  OT ca ngày CN(200%)
                            salarystatus.HOLY_0_240 = (reader["HOLY_0_240"] != DBNull.Value) ? reader["HOLY_0_240"].ToString() : "0";         //  OT ca đêm CN(240%)
                            salarystatus.HOLY_0_270 = (reader["HOLY_0_270"] != DBNull.Value) ? reader["HOLY_0_270"].ToString() : "0";         //  OT ca đêm CN(270%)
                            salarystatus.N_HOLY_300 = (reader["N_HOLY_300"] != DBNull.Value) ? reader["N_HOLY_300"].ToString() : "0";         //  OT ca ngày Lễ(300%)
                            salarystatus.N_HOLY_390 = (reader["N_HOLY_390"] != DBNull.Value) ? reader["N_HOLY_390"].ToString() : "0";         //  OT ca đêm Lễ(390%)

                        }
                        salarystatusList.Add(salarystatus);
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    TempData["msg"] = "Server Error!";
                }
            }
            return salarystatusList;
        }

        public ActionResult IndexFile()
        {
            return View();
        }

        public ActionResult Reset_Password()
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
                    cmd.Parameters.Add("P_PASS_WORD", OracleDbType.Varchar2).Value = Session["USER_CODE"].ToString();
                    cmd.ExecuteNonQuery();
                    con.Clone();

                    return RedirectToAction("Index", "UserLogin");
                }
                catch (Exception ex)
                {
                    TempData["msg"] = "Account or password wrong!";
                }
            }

            return View();
        }
    }
}
