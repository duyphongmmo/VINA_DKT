using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Login_Logout.Models
{
    public class DutyModel
    {
        public string START_TIME { get; set; }
        public string END_TIME { get; set; }
        public string PERSON_NUM { get; set; }
        public string PERSON_NAME { get; set; }
        public string DUTY_NAME { get; set; }
        public int PERSON_ID { get; set; }
        public string DEPT_NAME { get; set; }
        public int DUTY_PERIOD_ID { get; set; }
        public DateTime START_DATE { get; set; }
        public DateTime END_DATE { get; set;}
        public DateTime? WORK_START_DATE { get; set; }
        public DateTime? REAL_START_DATE { get;set; }
        public DateTime? WORK_END_DATE { get;set; }
        public DateTime? REAL_END_DATE { get;set; }
        public string APPROVED_YN { get; set; }
        public string CONFIRMED_YN { get; set; }
        public bool Confirmed => CONFIRMED_YN == "Y" && CONFIRMED_YN != null;
        public string APPROVE_STATUS { get; set; }
        public string CALENDAR_TRAN_YN { get; set; }
        public string DESCRIPTION { get; set; }
        public string EMAIL_STATUS { get; set; }
        public string REJECT_YN { get; set; }
        public string REJECT_REMARK { get; set; }
        public string TEAM_APPROVED_YN { get; set; }
        public string APPROVED2_YN { get; set; }
        public int REQ_PERSON_ID { get; set; }
    }


    public class DutyCount
    {
        public int Total { get; set; }
        public int CONFIRMED { get; set; }
        public int NOT_CONFIRM { get; set; }
    }
}