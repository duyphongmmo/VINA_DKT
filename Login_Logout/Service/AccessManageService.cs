using Login_Logout.Helper;
using Login_Logout.Models;
using OfficeOpenXml;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Windows.Documents;

namespace Login_Logout.Service
{
    public class AccessManageService
    {
        public byte[] ExportExcel()
        {
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Employees");

                // Add header
                worksheet.Cells[1, 1].Value = "PERSON_ID";
                worksheet.Cells[1, 2].Value = "PERSON_NUM";
                worksheet.Cells[1, 3].Value = "NAME";
                worksheet.Cells[1, 4].Value = "BIRTHDAY";
                worksheet.Cells[1, 5].Value = "LAST_UPDATE_DATE";

                // Add data rows
                for (int i = 0; i < 5; i++)
                {

                    HrmPerson user = new HrmPerson()
                    {
                        PERSON_ID = i,
                        NAME = "Ten " + i,
                        BIRTHDAY = DateTime.Now.AddYears(-i),
                        LAST_UPDATE_DATE = DateTime.Now.AddDays(i),
                    };

                    worksheet.Cells[i + 2, 1].Value = user.PERSON_ID;
                    worksheet.Cells[i + 2, 2].Value = user.PERSON_NUM;
                    worksheet.Cells[i + 2, 3].Value = user.NAME;
                    worksheet.Cells[i + 2, 4].Value = user.BIRTHDAY?.ToString("yyyy/MM/dd");
                    worksheet.Cells[i + 2, 5].Value = user.LAST_UPDATE_DATE.ToString();
                }

                return package.GetAsByteArray();
            }
        }



        public Tuple<List<DutyModel>, DutyCount> getList(string date, string s)
        {
            List<DutyModel> lst = new List<DutyModel>();
            DateTime time = new DateTime(2024, 6, 1);
            //string date = time.ToString("yyyy-MM-dd");

            OracleParameter[] parm =
            {
                 new OracleParameter("P_DATE_S", OracleDbType.Varchar2){ Value = date },
                 new OracleParameter("P_SEARCH", OracleDbType.Varchar2){ Value = s },
                 new OracleParameter("P_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output)
            };
            lst = DBM.ExecuteReader<DutyModel>("HRM_WEB_G2.GET_LIST", CommandType.StoredProcedure, parm);


            OracleParameter[] parms =
            {
                 new OracleParameter("P_DATE_S", OracleDbType.Varchar2){ Value = date },
                 new OracleParameter("P_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output)
            };
            List<DutyCount> lsts = DBM.ExecuteReader<DutyCount>("HRM_WEB_G2.TOTAL_GET_LIST", CommandType.StoredProcedure, parms);
            
            return Tuple.Create(lst, lsts.FirstOrDefault());
        }
        

        public void ConfirmDuty(int dutyID)
        {
            OracleParameter[] parm =
            {
                 new OracleParameter("P_DUTY_ID", OracleDbType.Int32){ Value = dutyID },
                 new OracleParameter("P_CONFIRMED_YN", OracleDbType.Varchar2){ Value = "Y" },
                 new OracleParameter("P_DESCRIPTION", OracleDbType.NVarchar2){ Value = DBNull.Value },
                 new OracleParameter("P_PERSON_ID", OracleDbType.Int32){ Value = 1 },
                 new OracleParameter("P_CREATION_DATE", OracleDbType.Date){ Value = DateTime.Now },
            };
            int effect = DBM.ExecuteNonQuery("HRM_WEB_G2.DUTY_CONFIRM", commandType: CommandType.StoredProcedure, parameter: parm);
        }

        public HrmPerson GetUserIf(int? id)
        {

            OracleParameter[] parm =
            {
                 new OracleParameter("P_DUTYID", OracleDbType.Int32){ Value = id },
                 new OracleParameter("P_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output)
            };

            var lst = DBM.ExecuteReader<HrmPerson>("HRM_WEB_G2.GET_USERIF", CommandType.StoredProcedure, parm);

            return lst.FirstOrDefault();
        }


    }
}