using System;

namespace medtracker.Models
{
    public class UserRecord
    {
        public bool Error { get; private set; }
        public bool? HaPresent { get; private set; }
        public int? NumMaxalt { get; private set; }
        public int? NumAleve { get; private set; }
        public DateTime? Date { get; private set; }

        public static UserRecord CreateRecord(string [] entries)
        {
            if(!ValidateRecord(entries))
                return new UserRecord
                {
                    Error = true,
                    HaPresent = null,
                    NumMaxalt = null,
                    NumAleve = null,
                    Date = null
                };

            if (entries.Length == 4)
                return new UserRecord
                {
                    Error = false,
                    HaPresent = entries[1] == "yes" ? true : false,
                    NumMaxalt = int.Parse(entries[2]),
                    NumAleve = int.Parse(entries[3]),
                    Date = null
                };
            else return new UserRecord
            {
                Error = false,
                Date = DateTime.Parse(entries[1]),
                HaPresent = entries[2] == "yes" ? true : false,
                NumMaxalt = int.Parse(entries[3]),
                NumAleve = int.Parse(entries[4]),
            };
        }

        private static bool ValidateRecord(string [] entries)
        {
            if (entries.Length != 4 && entries.Length != 5) return false;

            string maxalt = "";
            string aleve = "";
            string ha_present = "";

            if (entries.Length == 4)
            {
                ha_present = entries[1];
                maxalt = entries[2];
                aleve = entries[3];
            }
            else if (entries.Length == 5)
            {
                ha_present = entries[2];
                maxalt = entries[3];
                aleve = entries[4];
                if (!DateTime.TryParse(entries[1], out DateTime date)) return false;
            }

            if (int.TryParse(maxalt, out int num_maxalt) && int.TryParse(aleve, out int num_aleve))
            {
                return (ha_present == "yes" || ha_present == "no") && num_maxalt >= 0 && num_aleve >= 0;
            }

            return false;
        }
    }
}
