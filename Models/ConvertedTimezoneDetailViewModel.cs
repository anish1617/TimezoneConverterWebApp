namespace TimezoneConverterWebApp.Models
{
    public class ConvertedTimezoneDetailViewModel
    {
        public DateTime? OriginalStartDateTime { get; set; }
        public DateTime? OriginalEndDateTime { get; set; }
        public DateTime? ConvertedAdminStartDateTime { get; set; }
        public DateTime? ConvertedAdminEndDateTime { get; set; }
        public DateTime? ConvertedDeadlineStartDateTime { get; set; }
        public DateTime? ConvertedDeadlineEndDateTime { get; set; }
        public DateTime? ConvertedOutlookStartDateTime { get; set; }
        public DateTime? ConvertedOutlookEndDateTime { get; set; }
        public DateTime? ConvertedUserStartDateTime { get; set; }
        public DateTime? ConvertedUserEndDateTime { get; set; }
        public TimezoneViewModel? SelectedTimezones { get; set; }
    }
}
