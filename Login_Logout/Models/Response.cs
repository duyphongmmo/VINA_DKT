using System.Collections.Generic;

namespace Login_Logout.Models
{
    public class Response
    {
        public List<DayClass> StatusList { get; set; }
        public List<Salarystatus> TotalList { get; set; }
    }
}