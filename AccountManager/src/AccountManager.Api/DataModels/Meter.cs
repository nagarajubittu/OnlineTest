namespace AccountManager.Api.DataModels
{
    public class MeterReading
    {
        public int ReadingId { get; set; }
        public int AccountId { get; set; }
        public System.DateTime ReadingDatetime { get; set; }
        public int ReadingValue { get; set; }
    }
}