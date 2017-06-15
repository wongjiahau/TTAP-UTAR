using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using Time_Table_Arranging_Program.Class.Converter;


namespace Time_Table_Arranging_Program.Class.GoogleCalendarApi {
    public class GoogleCalendarApi {
        private static DateTime _dateOfMondayOfWeekOne;

        private static CalendarService GetRequestService() {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "Time_Table_Arranging_Program.client_secret.json";

            UserCredential credential;
            using (Stream streamReader = assembly.GetManifestResourceStream(resourceName)) {
                string credPath = Environment.GetFolderPath(
                    Environment.SpecialFolder.Personal);
                credPath = Path.Combine(credPath, ".credentials/calendar-dotnet-quickstart.json");

                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(streamReader).Secrets,
                    new List<string> {CalendarService.Scope.Calendar},
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
                Console.WriteLine(@"Credential file saved to: " + credPath);
            }
            var service = new CalendarService(new BaseClientService.Initializer
            {
                HttpClientInitializer = credential,
                ApplicationName = "Time Table Arranging Program (UTAR)"
            });
            return service;
        }

        public static void AddTimetableToCalendar(ITimetable timetable, DateTime dateOfMondayOfWeekOne) {
            _dateOfMondayOfWeekOne = dateOfMondayOfWeekOne;
            List<Event> events = ConvertTimetableToEvents(timetable);
            AddEventsToCalendar(events);
        }

        private static List<Event> ConvertTimetableToEvents(ITimetable timetable) {
            List<Slot> slotList = timetable.ToList();
            var events = new List<Event>();
            foreach (var slot in slotList) {
                events.Add(ConvertSlotToEvent(slot));
            }
            return events;
        }

        private static void AddEventsToCalendar(List<Event> events) {
            string calendarId = "primary";
            var service = GetRequestService();
            foreach (var e in events) {
                EventsResource.InsertRequest request = service.Events.Insert(e, calendarId);
                request.Execute();
            }
        }

        private static Event ConvertSlotToEvent(Slot slot) {
            return new Event
            {
                Summary = $"{slot.SubjectName} ({slot.Type}-{slot.Number})",
                Location = slot.Venue,
                Description = $"Subject code : {slot.Code}, Week : {slot.WeekNumber}",
                Start = new EventDateTime
                {
                    DateTime = GetDateTime(slot.Day, slot.StartTime, _dateOfMondayOfWeekOne),
                    // TimeZone = "Asia/Kuala_Lumpur" ,
                    TimeZone = "UTC+08:00"
                },
                End = new EventDateTime
                {
                    DateTime = GetDateTime(slot.Day, slot.EndTime, _dateOfMondayOfWeekOne),
                    //TimeZone = "Asia/Kuala_Lumpur" ,
                    TimeZone = "UTC+08:00"
                },
                Recurrence = GetRecurrence(slot.WeekNumber)
            };
        }

        private static IList<string> GetRecurrence(WeekNumber weekNumber) {
            return new[] {$"RRULE:FREQ=WEEKLY;COUNT={weekNumber.Max()}"};
        }

        private static DateTime? GetDateTime(Day day, ITime startTime, DateTime dateOfMondayOfWeekOne) {
            int interval = day.IntValue - Day.Parse(dateOfMondayOfWeekOne.DayOfWeek).IntValue;
            var d = dateOfMondayOfWeekOne + new TimeSpan(interval, 0, 0, 0);
            return new DateTime(d.Year, d.Month, d.Day, startTime.Hour, startTime.Minute, 0);
        }
    }
}