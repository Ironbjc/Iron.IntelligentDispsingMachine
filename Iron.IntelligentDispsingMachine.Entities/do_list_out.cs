namespace Iron.IntelligentDispsingMachine.Entities
{
    /// <summary>
    /// 对应数据库中的表
    /// </summary>
    public class do_list_out
    {
        public string PresNo { get; set; }
        public string MedOnlyCode { get; set; }
        public DateTime MedValidTime { get; set; }
        public string MedName { get; set; }
        public string MedUnit { get; set; }
        public string MedPack { get; set; }
        public string MedPos { get; set; }
        public float MedOutAMT { get; set; }
        public string PName { get; set; }
        public string MedFactory { get; set; }
        public string OutFlag { get; set; }

    }
}