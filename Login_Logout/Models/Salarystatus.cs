namespace Login_Logout.Models
{
    public class Salarystatus
    {
        public string person_number { get; set; }       // Mã Nhân Viên
        public string name { get; set; }                // Tên Nhân Viên
        public string monthSearch { get; set; }         // tháng
        public string TOTAL_ATT_DAY { get; set; }       // Số ngày công
        public string TOTAL_DED_DAY { get; set; }       // Số ngày công
        public string OT_150 { get; set; }              // OT ca ngày (150%)
        public string OT_210 { get; set; }              // OT ca ngày(210%)
        public string HOLY_0_200 { get; set; }          // HOLY_0_200
        public string NGITH_215 { get; set; }           // OT ca đêm (215%)
        public string NIGHT_BONUS_30 { get; set; }      // TC ca đêm(30%)
        public string HOLY_0_240 { get; set; }          // OT ca ngày CN(200%)
        public string HOLY_D_200 { get; set; }          // OT ca đêm CN(240%)
        public string HOLY_0_270 { get; set; }          // OT ca đêm CN(270%)
        public string N_HOLY_300 { get; set; }          // OT ca ngày Lễ(300%)
        public string N_HOLY_390 { get; set; }          // OT ca đêm Lễ(390%)

    }
}