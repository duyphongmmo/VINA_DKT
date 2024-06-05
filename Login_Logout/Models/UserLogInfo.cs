using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Login_Logout.Models
{
    public class UserLogInfo
    {
        public string Name { get; set; }
        public string USER_CODE { get; set; }
        public string PERSON_ID { get; set; }
        public int DEPT_ID { get; set; }
    }

    public class HrmPerson
    {
        public int PERSON_ID { get; set; }
        public string PERSON_NUM { get; set; }
        public string NAME { get; set; }
        public int DEPT_ID { get; set; }
        public string DEPT_NAME { get; set; }
        public string DUTY_NAME { get; set; }
        public string DESCRIPTION { get; set; }
        public string CONFIRMED_YN { get; set; }
        public int DUTY_PERIOD_ID { get; set; }
        public bool Confirmed => CONFIRMED_YN == "Y" && CONFIRMED_YN != null;
        public string HP_PHONE_NO { get; set; }
        public string TELEPHON_NO { get; set; }
        public DateTime? START_DATE { get; set; }
        public DateTime? END_DATE { get; set; }
        public DateTime? JOIN_DATE { get; set; }
        public DateTime? BIRTHDAY { get; set; }
        public DateTime? LAST_UPDATE_DATE { get; set; }
        public string dob => BIRTHDAY?.ToString("dd/MM/yyyy");
        //////
        public string StartDateStr => $"{START_DATE?.ToString("HH:mm")} {START_DATE?.Day}/{START_DATE?.Month}/{START_DATE?.Year}";
        public string EndDateStr => $"{END_DATE?.ToString("HH:mm")} {END_DATE?.Day}/{END_DATE?.Month}/{END_DATE?.Year}";
    }

    public class reportm
    {
        public string MONTH_NUM { get; set; }
        public int count_per_month { get; set; }
    }
}