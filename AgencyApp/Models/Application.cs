namespace AgencyApp.Models
{
    public class Application
    {
        public int ApplicationId { get; set; }
        public int DictionaryId { get; set; }
        public int ClientId { get; set; }
        public int Telephone { get; set; }
        public string Status { get; set; }
        public virtual Client? Client { get; set; }
        public virtual Dictionary? Dictionary { get; set; }
    }
}
