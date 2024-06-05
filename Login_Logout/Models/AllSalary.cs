using System.Collections.Generic;

namespace Login_Logout.Models
{
    public class AllSalary
    {
        public List<BaseSalary> ListBase { get; set; }
        public List<Salarystandards> ListSalary { get; set; }
    }
}