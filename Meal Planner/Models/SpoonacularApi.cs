using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Meal_Planner.Models       // model for
{
    public class SpoonacularApi
    {
        public KeySettings Key { get; set; }
    }
    public class KeySettings
    {
        public string KeyHeader { get; set; }
        public string ApiKey { get; set; }
        public string HostHeader { get; set; }
        public string Host { get; set; }
        public int RequestLimit { get; set; }
    }
}
