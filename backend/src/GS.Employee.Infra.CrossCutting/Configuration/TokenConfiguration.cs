using System;
using System.Collections.Generic;
using System.Text;

namespace GS.Employee.Infra.CrossCutting.Configuration
{    
    public class TokenConfiguration
    {
        public string Audience { get; set; }
        public string Issuer { get; set; }
        public string Secret { get; set; }
        public int Minutes { get; set; }
        public int DaysToExpiry { get; set; }
    }
}
