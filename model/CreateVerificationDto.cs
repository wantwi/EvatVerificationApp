using Newtonsoft.Json;

namespace EvatVerificationApp.model
{
    public class CreateVerificationDto
    {
        [JsonProperty("invoicenumber")]
        public string Invoicenumber { get; set; }
        [JsonProperty("suppliertin")]
        public string Suppliertin { get; set; }
        [JsonProperty("date")]
        public string Date { get; set; }
        [JsonProperty("time")]
        public string Time { get; set; }

        [JsonProperty("totalamount")]
        public double Totalamount { get; set; }
        [JsonProperty("vatamount")]
        public double Vatamount { get; set; }
        [JsonProperty("numberofitems")]
        public int Numberofitems { get; set; }
        [JsonProperty("encrypteddata")]
        public string Encrypteddata { get; set; }
        [JsonProperty("new")]
        public bool New { get; set; }
        [JsonProperty("version")]
        public string Version { get; set; }
    }

    public class WHTVerificationDto
    {
       
        //public double PayableAmount { get; set; }
        public double WithholdingAmount { get; set; }
        //public int TermsInMonths { get; set; }
        public string CreatedDate { get; set; }
        public string PayeeName { get; set; }
        public string PayorName { get; set; }
        public string TransactionNumber { get; set; }
        public string ReceiptNumber { get; set; }
        public string Version { get; set; }
        //public string Signature { get; set; }
        public string Encrypteddata { get; set; }
    }
}
