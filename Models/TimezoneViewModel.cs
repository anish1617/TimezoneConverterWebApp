namespace TimezoneConverterWebApp.Models
{
    public class TimezoneViewModel
    {
        public string? DeadlineTimezone { get; set; }
        public string? FirmAdminTimezone { get; set; }
        public string? OutlookCalendarTimezone { get; set; }
        public string? UserTimezone { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
    }
}
