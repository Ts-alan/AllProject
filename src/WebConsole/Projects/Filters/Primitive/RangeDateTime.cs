using System;
using System.Collections.Generic;
using System.Text;

namespace Filters.Primitive
{
    public class RangeDateTime
    {
        private bool _isRange = true;
        private DateTime _start = DateTime.MinValue;
        private DateTime _stop = DateTime.MaxValue;
        private bool _isGreater = true;
        private Int32 _intervalVaue = 1;
        private DateTimeInterval _dateTimeIntervalValue = DateTimeInterval.Minutes;

        public bool IsRange
        {
            get { return _isRange; }
            set { _isRange = value; }
        }
        public DateTime Start
        {
            get { return _start; }
            set { _start = value; }
        }
        public DateTime Stop
        {
            get { return _stop; }
            set { _stop = value; }
        }
        public bool IsGreater
        {
            get { return _isGreater; }
            set { _isGreater = value; }
        }
        public Int32 IntervalValue
        {
            get { return _intervalVaue; }
            set { _intervalVaue = value; }
        }
        public DateTimeInterval DateTimeIntervalValue
        {
            get { return _dateTimeIntervalValue; }
            set { _dateTimeIntervalValue = value; }
        }

        public RangeDateTime(DateTime start, DateTime stop)
        {
            _isRange = true;
            _start = start;
            _stop = stop;
        }

        public RangeDateTime(bool isGreater, Int32 intervalValue, DateTimeInterval dateTimeInterval)
        {
            _isRange = false;
            _isGreater = isGreater;
            _intervalVaue = intervalValue;
            _dateTimeIntervalValue = dateTimeInterval;
        }

        public RangeDateTime(string rangeDateTime)
        {
            if (string.IsNullOrEmpty(rangeDateTime)) return;
            string[] r = rangeDateTime.Split(new char[] { '_' });
            if (r.Length != 2)
            {
                throw new ArgumentException("RangeDateTime: range is in incorrect format");
            }

            char[] begin = new char[] { '[' };
            char[] end = new char[] { ']' };
            r[0] = r[0].TrimStart(begin).TrimEnd(end);
            r[1] = r[1].TrimStart(begin).TrimEnd(end);

            if (!String.IsNullOrEmpty(r[0]))
            {
                this._isRange = true;
                DateTime start = DateTime.MinValue;
                DateTime stop = DateTime.MaxValue;
                string[] tmpRange = r[0].Split(new char[] { '-' });
                if (tmpRange.Length != 2)
                {
                    throw new ArgumentException("RangeDateTime: range is in incorrect format");
                }

                DateTime.TryParse(tmpRange[0], out start);
                DateTime.TryParse(tmpRange[1], out stop);

                this._start = start;
                this._stop = stop;
            }
            else
            {
                if (!String.IsNullOrEmpty(r[1]))
                {
                    this._isRange = false;
                    string[] tmp = r[1].Split(new char[] { '-' });
                    if (tmp.Length != 3)
                    {
                        throw new ArgumentException("RangeDateTime: range is in incorrect format");
                    }
                    Int32 result = 0;
                    Int32.TryParse(tmp[0], out result);
                    this._isGreater = (result == 0);

                    result = 1;
                    Int32.TryParse(tmp[1], out result);
                    this._intervalVaue = result;

                    result = 0;
                    Int32.TryParse(tmp[2], out result);
                    this._dateTimeIntervalValue = (DateTimeInterval)result;
                }
            }
        }

        public override string ToString()
        {
            String result = @"[{0}]_[{1}]";
            String part1 = String.Empty;
            String part2 = String.Empty;

            if (_isRange)
            {
                part1 = String.Format(@"{0}-{1}", _start, _stop);
            }
            else
            {
                part2 = String.Format(@"{0}-{1}-{2}", _isGreater ? 0 : 1, _intervalVaue, (Int32)_dateTimeIntervalValue);
            }

            return String.Format(result, part1, part2);
        }

        public DateTime GetStart()
        {
            if (IsRange)
            {
                return Start;
            }
            else
            {
                if (IsGreater)
                {
                    return new DateTime(1900, 1, 1, 0, 0, 0);
                }
                else
                {
                    return GetDateTimeByNotRange();
                }
            }
        }

        public DateTime GetStop()
        {
            if (IsRange)
            {
                return Stop;
            }
            else
            {
                if (IsGreater)
                {
                    return GetDateTimeByNotRange();
                }
                else
                {
                    return DateTime.Now;
                }
            }
        }

        private DateTime GetDateTimeByNotRange()
        {
            switch (DateTimeIntervalValue)
            {
                case DateTimeInterval.Minutes:
                    return DateTime.Now.AddMinutes(-(Double)IntervalValue);
                case DateTimeInterval.Hours:
                    return DateTime.Now.AddHours(-(Double)IntervalValue);
                case DateTimeInterval.Days:
                    return DateTime.Now.AddDays(-(Double)IntervalValue);
                case DateTimeInterval.Weeks:
                    return DateTime.Now.AddDays((-7) * (Double)IntervalValue);
                case DateTimeInterval.Months:
                    return DateTime.Now.AddMonths(-IntervalValue);
                case DateTimeInterval.Years:
                    return DateTime.Now.AddYears(-IntervalValue);
                default:
                    throw new Exception("No correct DateTimeIntervalType.");
            }
        }
    }

    public enum DateTimeInterval
    {
        Minutes = 0,
        Hours = 1,
        Days = 2,
        Weeks = 3,
        Months = 4,
        Years = 5
    }
}
