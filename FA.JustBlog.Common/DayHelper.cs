using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FA.JustBlog.Common
{
    public static class DayHelper
    {
        public static string GetTimeFromCreated(DateTime createDate)
        {
            int dayDiff = (DateTime.Now - createDate).Days;
            int hoursDiff = (DateTime.Now - createDate).Hours;
            int minsDff = (DateTime.Now - createDate).Minutes;
            int yearDiff = (DateTime.Now.Year - createDate.Year);
            switch (dayDiff)
            {
                case 1: return $"Yesterday";
                case 0:
                    if (hoursDiff == 0 && minsDff != 0)
                    {
                        return $"{minsDff} minutes ago";
                    }
                    if (hoursDiff == 0 && minsDff < 1) return $"Just now";
                    return $"{hoursDiff} hours";
                default:
                    if (yearDiff >= 1) return $"{yearDiff} years ago";
                    else return $"{dayDiff} days ago";
            }
        }
        }
    }
