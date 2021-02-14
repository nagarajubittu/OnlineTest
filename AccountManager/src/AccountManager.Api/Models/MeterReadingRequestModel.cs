namespace AccountManager.Api.Models
{
    public class MeterReadingRequestModel
    {
        public string AccountId { get; set; }
        public string MeterReadingDateTime { get; set; }
        public string MeterReadValue { get; set; }
    }
}