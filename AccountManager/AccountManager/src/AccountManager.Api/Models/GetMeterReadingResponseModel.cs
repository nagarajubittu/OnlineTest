namespace AccountManager.Api.Models
{
    public class GetMeterReadingResponseModel
    {
        public int MeterId { get; set; }
        public int AccountId { get; set; }
        public System.DateTime ReadingDatetime { get; set; }
        public int ReadingValue { get; set; }
    }
}