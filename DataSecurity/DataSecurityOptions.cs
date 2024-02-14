using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvatVerificationApp.DataSecurity
{
    internal class DataSecurityOptions
    {
        public string SecurityKey { get; set; }
        public string SecurityIv { get; set; }
        public string SecuritySalt { get; set; }
        public string VSDCId { get; set; }
        public string VSDCUrl { get; set; }
        public string AuthorityUrl { get; set; }
        public bool RequireHttps { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string Scope { get; set; }
    }
}
