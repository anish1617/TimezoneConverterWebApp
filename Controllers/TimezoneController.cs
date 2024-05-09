using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Text.Json;
using TimezoneConverterWebApp.Models;

namespace TimezoneConverterWebApp.Controllers
{
    public class TimezoneController : Controller
    {
        public IActionResult Index()
        {
            string jsonFilePath = "timezones.json";
            string jsonContent = System.IO.File.ReadAllText(jsonFilePath);
            List<TimezoneModel> timezones = JsonSerializer.Deserialize<List<TimezoneModel>>(jsonContent);
            return View(timezones);
        }

        [HttpPost]
        public IActionResult ConvertToTimezone([FromForm] TimezoneViewModel timezoneViewModel)
        {
            //DateTime.TryParseExact(timezoneViewModel.StartDateTime, "MM/dd/yyyy hh:mm tt",CultureInfo.InvariantCulture,DateTimeStyles.None, out DateTime parsedStartDateTime);
            //DateTime.TryParseExact(timezoneViewModel.EndDateTime, "MM/dd/yyyy hh:mm tt",CultureInfo.InvariantCulture,DateTimeStyles.None, out DateTime parsedEndDateTime);
            var timezoneInfo = ConvertUserTimeZone(timezoneViewModel.StartDateTime, timezoneViewModel.EndDateTime, timezoneViewModel?.DeadlineTimezone, timezoneViewModel?.UserTimezone, timezoneViewModel?.FirmAdminTimezone, timezoneViewModel?.OutlookCalendarTimezone);

            return PartialView(timezoneInfo);
        }



        private static ConvertedTimezoneDetailViewModel ConvertUserTimeZone(DateTime startDateTime, DateTime endDateTime, string deadlineTimeZone, string userTimeZone, string firmAdminTimeZone, string outlookTimezone)
        {
            
            DateTime deadlineStartDateTime = startDateTime;
            DateTime deadlineEndDateTime = endDateTime;

            // get TimeZone info for user, appointment and firm admin
            TimeZoneInfo deadlineTimeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(deadlineTimeZone);
            TimeZoneInfo userTimeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(userTimeZone);
            TimeZoneInfo firmAdminTimeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(firmAdminTimeZone);
            TimeZoneInfo outlookCalendarTimeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(outlookTimezone);

            #region step 1: convert deadlineTimezone to adminTimeZone and save in db
            DateTime convertedAdminStartDateTime = TimeZoneInfo.ConvertTime(deadlineStartDateTime, deadlineTimeZoneInfo, firmAdminTimeZoneInfo);
            DateTime convertedAdminEndDateTime = TimeZoneInfo.ConvertTime(deadlineEndDateTime, deadlineTimeZoneInfo, firmAdminTimeZoneInfo);
            #endregion

            #region Step 2: convert the adminTimeZone back to deadlineTimezone while adding to calendar event
            DateTime convertedDeadlineStartDateTime = TimeZoneInfo.ConvertTime(convertedAdminStartDateTime, firmAdminTimeZoneInfo, deadlineTimeZoneInfo);
            DateTime convertedDeadlinEndDateTime = TimeZoneInfo.ConvertTime(convertedAdminEndDateTime, firmAdminTimeZoneInfo, deadlineTimeZoneInfo);
            #endregion

            #region Step 3: convert firm admin timezone to logged in user timezone when fetching for deadline chart
            DateTime convertedUserStartDateTime = TimeZoneInfo.ConvertTime(convertedAdminStartDateTime, firmAdminTimeZoneInfo, userTimeZoneInfo);
            DateTime convertedUserEndDateTime = TimeZoneInfo.ConvertTime(convertedAdminEndDateTime, firmAdminTimeZoneInfo, userTimeZoneInfo);
            #endregion

            #region Step 4: convert the deadlineTimezone to user timezone in calendar
            DateTime convertedOutlookStartDateTime = TimeZoneInfo.ConvertTime(convertedDeadlineStartDateTime, deadlineTimeZoneInfo, outlookCalendarTimeZoneInfo);
            DateTime convertedOutlookEndDateTime = TimeZoneInfo.ConvertTime(convertedDeadlinEndDateTime, deadlineTimeZoneInfo, outlookCalendarTimeZoneInfo);
            #endregion

            ConvertedTimezoneDetailViewModel convertedTimezoneDetail = new() 
            { 
                OriginalStartDateTime = deadlineStartDateTime,
                OriginalEndDateTime = deadlineEndDateTime,
                ConvertedAdminStartDateTime = convertedAdminStartDateTime,
                ConvertedAdminEndDateTime = convertedAdminEndDateTime,
                ConvertedDeadlineStartDateTime = convertedDeadlineStartDateTime,
                ConvertedDeadlineEndDateTime = convertedDeadlinEndDateTime,
                ConvertedOutlookStartDateTime = convertedOutlookStartDateTime,
                ConvertedOutlookEndDateTime = convertedOutlookEndDateTime,
                ConvertedUserStartDateTime = convertedUserStartDateTime,
                ConvertedUserEndDateTime = convertedUserEndDateTime,
                SelectedTimezones = new ()
                {
                    FirmAdminTimezone = firmAdminTimeZone,
                    DeadlineTimezone = deadlineTimeZone,
                    OutlookCalendarTimezone = outlookTimezone,
                    UserTimezone = userTimeZone
                }
            };

            return convertedTimezoneDetail;

        }
    }
}
