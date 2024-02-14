using EvatVerificationApp.model;

using Microsoft.AspNetCore.Identity;
using System.Globalization;

namespace EvatVerificationApp.services
{
    public class Verification : IVerification
    {

        public async Task<bool> GetVerifiedAsync(QueryDto encrypted, CreateVerificationDto data)
        {
            {
                var arrResult = new List<bool>
            {
                encrypted.InvoiceNumber.Equals(data.Invoicenumber),
                encrypted.ItemCount.Equals(data.Numberofitems.ToString()),
                GetFormatedAmount(double.Parse(encrypted.TotalAmount), data.Totalamount).Result,
                CompareDate(data.Date,encrypted.VsdcTime).Result,
                 CompareTime(data.Time,encrypted.VsdcTime.Split(" ")[1]).Result,
               // encrypted.VsdcTime.Equals(data.Date + " "+ data.Time),
             
                //encrypted.VsdcTime.Split(" ")[0].Equals(data.Date),
                 //encrypted.VsdcTime.Split(" ")[1]    .Equals(data.Time),
                encrypted.CompanyTin.Equals(data.Suppliertin),
                 GetFormatedAmount(double.Parse(encrypted.VatAmount), data.Vatamount).Result,
            };
                return (!arrResult.Contains(item: false))
                    ? await Task.FromResult(true) : await Task.FromResult(result: false);
            }

        }
        public async Task<bool> GetFormatedAmount(double amount, double encrypAmount)
        {
            double tolerance = 0.05;
            double difference = Math.Abs(amount - encrypAmount);
            // return difference <= tolerance;
            return await Task.FromResult(difference <= tolerance);

        }

        public async Task<bool> CompareDate(string userInputDate, string encrypDate)
        {

            var date_encrypt = DateTime.Parse(encrypDate);
            var date = date_encrypt.Date.ToString("yyyy/MM/dd");
            return await Task.FromResult(userInputDate.Equals(date));

        }

        public async Task<bool> CompareTime(string userInputTime, string encrypDate)
        {
            var date_encrypt = DateTime.Parse(encrypDate);
            var time = date_encrypt.ToString("HH:mm"); // $"{date_encrypt.Hour}:{date_encrypt.Minute}";
            return await Task.FromResult(userInputTime.Equals(time));

        }


        public async Task<bool> GetVerifiedAsync1_1(QueryDto_1_1 encrypted, CreateVerificationDto data)
        {
            string timestampString = encrypted.VsdcTime;
            long timestamp = long.Parse(timestampString);
            DateTime dateTime = DateTimeOffset.FromUnixTimeMilliseconds(timestamp).DateTime;
            string formattedDate = dateTime.ToString("yyyy/MM/dd HH:mm");

            var arrResult = new List<bool>
            {
                encrypted.InvoiceNumber.Equals(data.Invoicenumber),
                encrypted.ItemsCount.Equals(data.Numberofitems.ToString()),
                GetFormatedAmount(double.Parse(encrypted.TotalAmount), data.Totalamount).Result,
                formattedDate.Split(" ")[0].Equals(data.Date),
                formattedDate.Split(" ")[1].Equals(data.Time),
                encrypted.CompanyTin.Equals(data.Suppliertin),
                 GetFormatedAmount(double.Parse(encrypted.TotalVatAmount), data.Vatamount).Result,
            };

            return (!arrResult.Contains(item: false));
            //? await Task.FromResult(true) : await Task.FromResult(result: false);

        }

        public async Task<bool> GetNGVerifiedAsync1_1(NG_QueryDto_1_1 encrypted, WHTVerificationDto data)
        {
            string timestampString = encrypted.CreatedDate;
            long timestamp = long.Parse(timestampString);
            DateTime dateTime = DateTimeOffset.FromUnixTimeMilliseconds(timestamp).DateTime;
            string formattedDate = dateTime.ToString("yyyy/MM/dd HH:mm");

            var arrResult = new List<bool>
            {
                 encrypted.PayeeName.Equals(data.PayeeName),
                 encrypted.PayorName.Equals(data.PayorName),
                 encrypted.ReceiptNumber.Equals(data.ReceiptNumber),
                 encrypted.TransactionNumber.Equals(data.TransactionNumber),
                 formattedDate.Split(" ")[0].Equals(data.CreatedDate),
                 //GetFormatedAmount(double.Parse(encrypted.PayableAmount), data.PayableAmount).Result,
                 GetFormatedAmount(double.Parse(encrypted.WithholdingAmount), data.WithholdingAmount).Result,
                 //encrypted.TermsInMonths.Equals(data.TermsInMonths.ToString())

            };

            return (!arrResult.Contains(item: false));
            //? await Task.FromResult(true) : await Task.FromResult(result: false);

        }


       
    }
}
