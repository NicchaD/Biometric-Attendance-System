//****************************************************
//Modification History
//****************************************************
//Modified by          Date          Description
//****************************************************
//Manthan Rajguru      2015.09.16    YRS-AT-2550: YRS data cleanup: copyright comments and namespace (Changed the Namespace reference from 'Infotech' to 'YMCARET')
//*****************************************************
using System;
//using System.Collections.Generic;
using System.Text;

namespace YMCARET.YmcaBusinessObject
{
	public enum DateIntervalNew
	{
		Day,
		DayOfYear,
		Hour,
		Minute,
		Month,
		Quarter,
		Second,
		Weekday,
		WeekOfYear,
		Year
	}

	public class DateAndTime
	{
		public static int DateDiffNew(DateIntervalNew interval, DateTime dt1, DateTime dt2)
		{
			return DateDiffNew(interval, dt1, dt2, System.Globalization.DateTimeFormatInfo.CurrentInfo.FirstDayOfWeek);
		}

		private static int GetQuarter(int nMonth)
		{
			if (nMonth <= 3)
				return 1;
			if (nMonth <= 6)
				return 2;
			if (nMonth <= 9)
				return 3;
			return 4;
		}

		public static int DateDiffNew(DateIntervalNew interval, DateTime dt1, DateTime dt2, DayOfWeek eFirstDayOfWeek)
		{
			if (interval == DateIntervalNew.Year)
				return dt2.Year - dt1.Year;

			if (interval == DateIntervalNew.Month)
				return (dt2.Month - dt1.Month) + (12 * (dt2.Year - dt1.Year));

			TimeSpan ts = dt2 - dt1;
            
			if (interval == DateIntervalNew.Day || interval == DateIntervalNew.DayOfYear)
				return Round(ts.TotalDays);
            
			if (interval == DateIntervalNew.Hour)
				return Round(ts.TotalHours);

			if (interval == DateIntervalNew.Minute)
				return Round(ts.TotalMinutes);

			if (interval == DateIntervalNew.Second)
				return Round(ts.TotalSeconds);

			if (interval == DateIntervalNew.Weekday )
			{
				return Round(ts.TotalDays / 7.0);
			}

			if (interval == DateIntervalNew.WeekOfYear)
			{
				while (dt2.DayOfWeek != eFirstDayOfWeek)
					dt2 = dt2.AddDays(-1);
				while (dt1.DayOfWeek != eFirstDayOfWeek)
					dt1 = dt1.AddDays(-1);
				ts = dt2 - dt1;
				return Round(ts.TotalDays / 7.0);
			}

			if (interval == DateIntervalNew.Quarter)
			{
				double d1Quarter = GetQuarter(dt1.Month);
				double d2Quarter = GetQuarter(dt2.Month);
				double d1 = d2Quarter - d1Quarter;
				double d2 = (4 * (dt2.Year - dt1.Year));
				return Round(d1 + d2);
			}

			return 0;

		}

		private static int Round(double dVal)
		{
			if (dVal >= 0)
				return (int)Math.Floor(dVal);
			return (int)Math.Ceiling(dVal);
		}
	}
}