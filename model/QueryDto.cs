using Newtonsoft.Json;

namespace EvatVerificationApp.model
{
    public class QueryDto
    {
        [JsonProperty("total_amount")]
        public string TotalAmount { get; set; }

        [JsonProperty("item_count")]
        public string ItemCount { get; set; }



        [JsonProperty("vsdc_time")]
        public string VsdcTime { get; set; }



        [JsonProperty("vat_amount")]
        public string VatAmount { get; set; }



        [JsonProperty("company_tin")]
        public string CompanyTin { get; set; }



        [JsonProperty("invoice_number")]
        public string InvoiceNumber { get; set; }



        [JsonProperty("receipt_number")]
        public string ReceiptNumber { get; set; }
    }

    public class QueryDto_1_1
    {
       
        public string TotalAmount { get; set; }
        public string ItemsCount { get; set; }
        public string VsdcTime { get; set; }
        public string TotalVatAmount { get; set; }
        public string CompanyTin { get; set; }
        public string InvoiceNumber { get; set; }
        public string ReceiptNumber { get; set; }
    }

    public class NG_QueryDto_1_1
    {

        public string PayableAmount { get; set; }
        public string WithholdingAmount { get; set; }
        public string TermsInMonths { get; set; }
        public string CreatedDate { get; set; }
        public string PayeeName { get; set; }
        public string PayorName { get; set; }
        public string ReceiptNumber { get; set; }
        public string TransactionNumber { get; set; }
        public string Signature { get; set; }
       
    }
}
