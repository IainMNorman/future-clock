namespace fc_core
{
	public class NormDateTime
	{
		const ulong LEAPOCH = (946684800 + 86400*(31+29));
		const ulong DAYS_PER_400Y = (365*400 + 97);
		const ulong DAYS_PER_100Y = (365*100 + 24);
		const ulong DAYS_PER_4Y = (365*4   + 1);


		private ulong _ticks;

		public ulong Ticks { 
			get {
				return _ticks;
			}
			set { 
				_ticks = value;
				ConvertTimestamp (_ticks);
			}
		}

		public ulong Year { get; set; }
		public ulong MonthNumber { get; set;}
		public ulong Day { get; set;}
		public ulong DayOfWeekNumber { get; set;}
		public ulong DayOfYear { get; set;}
		public ulong Hours { get; set;}
		public ulong Minutes { get; set;}
		public ulong Seconds { get; set;}

		public NormDateTime (ulong timestamp)
		{
			this.Ticks = timestamp;
		}

		public override string ToString ()
		{
			return $"{Year}-{MonthNumber+1:d2}-{Day:d2} {Hours:d2}:{Minutes:d2}:{Seconds:d2}";
		}

		private void ConvertTimestamp (ulong t)
		{
			ulong secs, days;
			ulong remdays, remsecs, remyears;
			ulong qc_cycles, c_cycles, q_cycles;
			ulong years, months;
			ulong wday, yday, leap;
			ulong[] days_in_month = {31,30,31,30,31,31,30,31,30,31,31,29};

			secs = t - LEAPOCH;
			days = (secs / 86400);
			remsecs = (secs % 86400);
			if (remsecs < 0) {
				remsecs += 86400;
				days--;
			}

			wday = ((3+days)%7);
			if (wday < 0) wday += 7;

			qc_cycles = (days / DAYS_PER_400Y);
			remdays = (days % DAYS_PER_400Y);
			if (remdays < 0) {
				remdays += DAYS_PER_400Y;
				qc_cycles--;
			}

			c_cycles = remdays / DAYS_PER_100Y;
			if (c_cycles == 4) c_cycles--;
			remdays -= c_cycles * DAYS_PER_100Y;

			q_cycles = remdays / DAYS_PER_4Y;
			if (q_cycles == 25) q_cycles--;
			remdays -= q_cycles * DAYS_PER_4Y;

			remyears = remdays / 365;
			if (remyears == 4) remyears--;
			remdays -= remyears * 365;

			leap = (ulong)(((remyears != 0) && (q_cycles == 0 || c_cycles != 0)) ? 1 : 0);
			yday = remdays + 31 + 28 + leap;
			if (yday >= 365+leap) yday -= 365+leap;

			years = remyears + 4*q_cycles + 100*c_cycles + 400*qc_cycles;

			for (months=0; days_in_month[months] <= remdays; months++)
				remdays -= days_in_month[months];

			this.Year = years + 2000;
			this.MonthNumber = months + 2;
			if (this.MonthNumber >= 12) {
				this.MonthNumber -= 12;
				this.Year++;
			}
			this.Day = remdays + 1;
			this.DayOfWeekNumber = wday;
			this.DayOfYear = yday;
			this.Hours = remsecs / 3600;
			this.Minutes = remsecs / 60 % 60;
			this.Seconds = remsecs % 60;
		}

	}
}

