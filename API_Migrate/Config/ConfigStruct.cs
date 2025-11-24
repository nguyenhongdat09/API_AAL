using API_Migrate.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace API_Migrate.Config
{
    public class ValueDescription
    {
        public string Value { get; set; }
        public string Description { get; set; }
    }

    public class Configuration
    {
        public ValueDescription TimeSleep { get; set; }
        public ValueDescription TokenTimeOut { get; set; }
        public ValueDescription TimeInterval { get; set; }
        public ValueDescription TimeIntervalSQL { get; set; }
        public ValueDescription UserName { get; set; }
        public ValueDescription UserPass { get; set; }
        public ValueDescription ApiKey { get; set; }
        public ValueDescription SysDatabaseName { get; set; }
        public ValueDescription Organization { get; set; }
        public ValueDescription IsDebug { get; set; }
        public ValueDescription UserDebug { get; set; }
        public ValueDescription TitleTools { get; set; } 
        public ValueDescription ApiURL { get; set; }
        public ValueDescription Token { get; set; }
        public ValueDescription AppConnectionString { get; set; }
    }

    public class ConfigStruct
    {
        public Configuration Configuration { get; set; }
    }


}
