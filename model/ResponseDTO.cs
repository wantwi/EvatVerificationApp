using System.Runtime.InteropServices;
using System.Text.Json.Serialization;

namespace EvatVerificationApp.model
{

    public class PostResponse : BaseResponse
    {
        public string token { get; set; }
        public string vsdc { get; set; }
    }

    public class BaseResponse
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.Always)]
        public bool success { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string code { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string response { get; set; }

    }
}
