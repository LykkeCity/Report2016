using System;
namespace Report2016
{
    public class SettingsModel
    {
        public class ReportsModel
        {
            public string VotesConnectionString { get; set; }
            public string LogsConnectionString { get; set; }
        }


        public ReportsModel Report2016 { get; set; }

    }



}
