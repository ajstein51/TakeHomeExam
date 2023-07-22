using System;
using System.Collections.Generic;
using System.Linq;

namespace DTExtension
{
    static class Program
    {
        static void Main(string[] args)
        {
            var weekend = new DateTime(2023, 07, 20, 11, 10, 0);
            var holiday = new HashSet<DateTime> { new DateTime(2023, 07, 15), new DateTime(2023, 07, 14), new DateTime(2023, 07, 11), new DateTime(2023, 07, 12), new DateTime(2023, 07, 20) };

            // output simple
            Console.WriteLine("Basic:");
            Console.WriteLine("Next DT Now: " + DateTime.Now.GetNextBusinessDay() + "\tDay of the week is: " + DateTime.Now.GetNextBusinessDay().DayOfWeek);
            Console.WriteLine("Next DT Weekend: " + weekend.GetNextBusinessDay() + "\tDay of the week is: " + weekend.GetNextBusinessDay().DayOfWeek);

            // ouput: Bonus #1
            Console.WriteLine("\nBonus #1: Allow the caller to specify the number of business days instead of simply the next business day.");
            Console.WriteLine("Next DT Now: " + DateTime.Now.GetNextBusinessDay(2) + "\tDay of the week is: " + DateTime.Now.GetNextBusinessDay(2).DayOfWeek);
            Console.WriteLine("Next DT Weekend: " + weekend.GetNextBusinessDay(2) + " \tDay of the week is: " + weekend.GetNextBusinessDay(2).DayOfWeek);

            // ouput: Bonus #2
            Console.WriteLine("\nBonus #2: Allow the caller to specify holidays");
            Console.WriteLine("Next DT Now: " + DateTime.Now.GetNextBusinessDay(holiday, 2) + "\tDay of the week is: " + DateTime.Now.GetNextBusinessDay(holiday, 2).DayOfWeek);
            Console.WriteLine("Next DT Weekend: " + weekend.GetNextBusinessDay(holiday, 1) + " \tDay of the week is: " + weekend.GetNextBusinessDay(holiday, 1).DayOfWeek);

            // ouput: Bonus #3
            Console.WriteLine("\nBonus #3: Account for time zone differences.");
            Console.WriteLine("Next DT Now: " + DateTime.Now.GetNextBusinessDay(holiday, "Eastern Standard Time") + "\tDay of the week is: " + DateTime.Now.GetNextBusinessDay(holiday, "Eastern Standard Time").DayOfWeek);
            Console.WriteLine("Next DT Weekend Hawaiian Time: " + weekend.GetNextBusinessDay(holiday, "Hawaiian Standard Time") + " \tDay of the week is: " + weekend.GetNextBusinessDay(holiday, "Hawaiian Standard Time").DayOfWeek); // -5 hours

            // ouput: Bonus #4
            Console.WriteLine("\nBonus #4:   Allow the caller to specify the work week schedule to account for the business week not being the same in every country, for example Israel’s workweek is Sunday to Thursday.");
            Console.WriteLine("Next DT Now: " + DateTime.Now.GetNextBusinessDay(holiday, new[] { DayOfWeek.Monday, DayOfWeek.Tuesday }) + "\tDay of the week is: " + DateTime.Now.GetNextBusinessDay(holiday, new[] { DayOfWeek.Monday, DayOfWeek.Tuesday }).DayOfWeek);
            Console.WriteLine("Next DT Weekend: " + weekend.GetNextBusinessDay(holiday, new[] { DayOfWeek.Thursday, DayOfWeek.Saturday }) + " \tDay of the week is: " + weekend.GetNextBusinessDay(holiday, new[] { DayOfWeek.Thursday, DayOfWeek.Saturday }).DayOfWeek);
        }

        /*
         *  Arguments: this DateTime: dT variable in use, 
         *             optionalDays: number of days to add for the next business day will default to 0 if not given a value
         * Desc: extension method for datetime to give the next day of the week that is a business day. 
         *       Given a number of Days that is greater than 0 it will add that to the current date to get the business day.
         *       If that day ends up on a weekend itll go to the next business day
         *       This assumes it is a monday - friday work week and doesnt skip holidays
         * Return: Next business day datetime
         */
        public static DateTime GetNextBusinessDay(this DateTime dT, int optionalDays = 0)
        {
            dT = optionalDays <= 0 ? dT.AddDays(1) : dT.AddDays(optionalDays);

            if (dT.DayOfWeek == DayOfWeek.Saturday || dT.DayOfWeek == DayOfWeek.Sunday)
            {
                while (dT.DayOfWeek == DayOfWeek.Saturday || dT.DayOfWeek == DayOfWeek.Sunday)
                    dT = dT.AddDays(1);
                return dT;
            }

            return dT;
        }

        // note that im using a hashset instead of list due to unknown size that the list could be thus hashset overall is more stable (list is better with lower count)
        /*
       *  Arguments: this DateTime: dT variable in use, 
       *             Holidays: ashSet of dates that are marked as holidays, 
       *             optionalDays: number of days to add for the next business day (OPTIONAL)
       *  Desc: extension method for datetime to give the next day of the week that is a business day. 
       *       Given a number of Days that is greater than 0 it will add that to the current date to get the business day or use 1 for getting the next business day.
       *       If that day ends up on a weekend/holiday itll go to the next business day
       *       This assumes it is a monday - friday work
       *  Return: Next business day datetime
       */
        public static DateTime GetNextBusinessDay(this DateTime dT, HashSet<DateTime> holidays, int optionalDays = 0)
        {
            dT = optionalDays <= 0 ? dT.AddDays(1) : dT.AddDays(optionalDays);

            if (dT.DayOfWeek == DayOfWeek.Saturday || dT.DayOfWeek == DayOfWeek.Sunday || holidays.Contains(dT.Date))
            {
                while (dT.DayOfWeek == DayOfWeek.Saturday || dT.DayOfWeek == DayOfWeek.Sunday || holidays.Contains(dT.Date))
                    dT = dT.AddDays(1);
                return dT;
            }

            return dT;
        }

        // documentation: https://learn.microsoft.com/en-us/dotnet/standard/datetime/converting-between-time-zones
        /*
          *  Arguments: this DateTime: dT variable in use, 
          *             Holidays: HashSet of dates that are marked as holidays, 
          *             givenTimeZone: given a timezone it will take it into account or wont check timezones, 
          *             optionalDays: number of days to add for the next business day (OPTIONAL)
          *  Desc: extension method for datetime to give the next day of the week that is a business day. 
          *       Given a number of Days that is greater than 0 it will add that to the current date to get the business day or use 1 for getting the next business day.
          *       If that day ends up on a weekend/holiday itll go to the next business day
          *         This assumes it is a monday - friday work
          *       Given a tiemzone it will see if it actually exists and if so itll take it into account. Otherwise itll return the default datetime of January 1, 0001 00:00:00 (aka datetime.minvalue)
          *  Return: Next business day datetime, or default datetime
          */
        public static DateTime GetNextBusinessDay(this DateTime dT, HashSet<DateTime> holidays, string givenTimeZone = null, int optionalDays = 0)
        {
            if(givenTimeZone != null)
            {
                dT = GetChangeTime(dT, givenTimeZone);
                // default check it
                if (dT == DateTime.MinValue)
                    return dT;
            }
            return dT.GetNextBusinessDay(holidays, optionalDays);
        }

        /*
        *  Arguments: this DateTime: dT variable in use, 
        *             Holidays: HashSet of dates that are marked as holidays, 
        *             workWeek: given day(s) it will use that as non-worked days (weekends)
        *             givenTimeZone: given a timezone it will take it into account or wont check timezones, 
        *             optionalDays: number of days to add for the next business day (OPTIONAL)
        *  Desc: extension method for datetime to give the next day of the week that is a business day. 
          *       Given a number of Days that is greater than 0 it will add that to the current date to get the business day or use 1 for getting the next business day.
          *       If that day ends up on a weekend/holiday itll go to the next business day
          *         This assumes it is a monday - friday work
          *       Given a tiemzone it will see if it actually exists and if so itll take it into account. Otherwise itll return the default datetime of January 1, 0001 00:00:00 (aka datetime.minvalue)
          *       If given a workWeek it will use that as the weekend/non-work days
          *  Return: Next business day datetime, or default datetime
        */
        public static DateTime GetNextBusinessDay(this DateTime dT, HashSet<DateTime> holidays, IEnumerable<DayOfWeek> workWeek = null, string givenTimeZone = null, int optionalDays = 0)
        {
            if(givenTimeZone != null)
            {
                dT = GetChangeTime(dT, givenTimeZone);
                // default check it
                if (dT == DateTime.MinValue)
                    return dT;
            }

            // true represents a weekend AKA non-worked day, false represents a non-weekend AKA work day
            // assume people work every day
            var Week = new Dictionary<DayOfWeek, bool>
            {
                { DayOfWeek.Sunday, false },
                { DayOfWeek.Monday, false },
                { DayOfWeek.Tuesday, false },
                { DayOfWeek.Wednesday, false },
                { DayOfWeek.Thursday, false },
                { DayOfWeek.Friday, false },
                { DayOfWeek.Saturday, false }
            };

            if (workWeek.Any())
                foreach (var day in workWeek)
                    Week[day] = true;
            else
            {
                Week[DayOfWeek.Saturday] = true;
                Week[DayOfWeek.Sunday] = true;
            }

            dT = optionalDays <= 0 ? dT.AddDays(1) : dT.AddDays(optionalDays);

            if (Week.First(day => day.Key == dT.DayOfWeek).Value || holidays.Contains(dT.Date))
            {
                while (Week.First(day => day.Key == dT.DayOfWeek).Value || holidays.Contains(dT.Date))
                    dT = dT.AddDays(1);
                return dT;
            }

            return dT;
        }

        /*
        *  Arguments: specified timezone: string for a timezone
        *  Desc: checks if the given timezone actually exists and if so it will convert the given datetime to that timezone otherwise it will return default datetime
        *  Return: Default Datetime or converted datetime to specified timezone
        */
        public static DateTime GetChangeTime(DateTime dT, string timeZone)
        {
            try
            {
                // convert to UTC (Coordinated Universal Time)
                var utcDt = dT.ToUniversalTime();

                // see if the timezone exists
                var timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(timeZone);

                return TimeZoneInfo.ConvertTimeFromUtc(utcDt, timeZoneInfo);
            }
            catch
            {
                // returns default datetime so users can default check for failed time conversion
                return new DateTime();
            }
        }
    }
}