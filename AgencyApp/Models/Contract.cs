namespace AgencyApp.Models
{
    public class Contract
    {
        public int ContractId { get; set; }
        public int ClientId { get; set; }
        public int AgentId { get; set; }
        public int DictionaryId { get; set; }
        public DateTime Date { get; set; }
        public virtual Dictionary? Dictionary { get; set; }
        public virtual Client? Client { get; set; }
        public virtual Agent? Agent { get; set; }
    }
}
