﻿namespace TimezoneConverterWebApp.Models
{
    public class TimezoneModel
    {
        public string? Id { get; set; }
        public bool HasIanaId { get; set; }
        public string? DisplayName { get; set; }
        public string? StandardName { get; set; }
        public string? DaylightName { get; set; }
        public bool? SupportsDaylightSavingTime { get; set; }

    }
}
