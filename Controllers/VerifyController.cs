
using EvatVerificationApp.helpers;
using EvatVerificationApp.model;
using EvatVerificationApp.services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;

namespace EvatVerificationApp.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class VerifyController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IVerification _job;
        //private readonly DataSecurityOptions _config;



        public VerifyController(IConfiguration configuration, IVerification job/*, DataSecurityOptions config*/)
        {
            _configuration = configuration;
            _job = job;
            //_config = config;
        }
        [HttpPost]
        public async Task<IActionResult> VerifyInvoice(WHTVerificationDto create)
        {
            try
            {

                if (create == null)
                    return StatusCode(StatusCodes.Status400BadRequest,
                        new PostResponse { success = false, code = "VRF400", response = "INVALID POST REQUEST" });

                if (!ModelState.IsValid)
                    return StatusCode(StatusCodes.Status422UnprocessableEntity,
                        new PostResponse { success = false, code = "VRF422", response = "INVALID POST REQUEST DATA" });



                if (string.IsNullOrEmpty(create.Encrypteddata))
                {
                    return StatusCode(StatusCodes.Status422UnprocessableEntity,
                            new PostResponse { success = true, code = "VRF_SUC_200", response = "INVALID ENCRYPTED TOKEN" });
                }

                var data = EncryptionHelper.DecryptStringFromBytes(create.Encrypteddata, create.Version);

                var queryData = data.AsJsonNGData();

                //if (!data.Contains("&"))
                //{
                //    return StatusCode(StatusCodes.Status422UnprocessableEntity,
                //            new PostResponse { success = true, code = "VRF_422", response = "INVALID ENCRYPTED TOKEN" });
                //}


                // validate token
                //if (!double.TryParse(queryData.PayableAmount, out var payableAmount))
                //{
                //    return StatusCode(StatusCodes.Status422UnprocessableEntity,
                //            new PostResponse { success = true, code = "VRF422", response = "ENCRYPTED TOKEN OF TYPE [PayableAmount] nust be a type of DECIMAL,DOUBLE" });
                //}

                if (!double.TryParse(queryData.WithholdingAmount, out var withholdingAmount))
                {
                    return StatusCode(StatusCodes.Status422UnprocessableEntity,
                            new PostResponse { success = true, code = "VRF422", response = "ENCRYPTED TOKEN OF TYPE [WithholdingAmount] nust be a type of DECIMAL,DOUBLE" });
                }

                //if (!int.TryParse(queryData.TermsInMonths, out var termsInMonths))
                //{
                //    return StatusCode(StatusCodes.Status422UnprocessableEntity,
                //            new PostResponse { success = true, code = "VRF422", response = "ENCRYPTED TOKEN OF TYPE [TermsInMonths] nust be a type of INT" });
                //}

                if (string.IsNullOrEmpty(queryData.PayeeName))
                {
                    return StatusCode(StatusCodes.Status422UnprocessableEntity,
                            new PostResponse { success = true, code = "VRF422", response = "INVALID POST REQUEST" });
                }

                if (string.IsNullOrEmpty(queryData.PayorName))
                {
                    return StatusCode(StatusCodes.Status422UnprocessableEntity,
                            new PostResponse { success = true, code = "VRF422", response = "INVALID POST REQUEST" });
                }
                if (string.IsNullOrEmpty(queryData.TransactionNumber))
                {
                    return StatusCode(StatusCodes.Status422UnprocessableEntity,
                            new PostResponse { success = true, code = "VRF422", response = "INVALID POST REQUEST" });
                }
                if (string.IsNullOrEmpty(queryData.ReceiptNumber))
                {
                    return StatusCode(StatusCodes.Status422UnprocessableEntity,
                            new PostResponse { success = true, code = "VRF422", response = "INVALID POST REQUEST" });
                }
                if (string.IsNullOrEmpty(queryData.Signature))
                {
                    return StatusCode(StatusCodes.Status422UnprocessableEntity,
                            new PostResponse { success = true, code = "VRF422", response = "INVALID POST REQUEST" });
                }


                var loadVerification = await _job.GetNGVerifiedAsync1_1(queryData, create);



                return loadVerification switch
                {
                    true => StatusCode(StatusCodes.Status200OK,
                            new PostResponse { success = true, code = "VRF_SUC_200", response = "TRUE" }),

                    _ => StatusCode(StatusCodes.Status422UnprocessableEntity,
                        new PostResponse { success = false, code = "VRF_SUC_404", response = "FALSE" })
                };










            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
    //    public async Task<IActionResult> VerifyInvoice(CreateVerificationDto create)
    //    {
    //        try
    //        {
    //            if (create.Version == "1.0")
    //            {
    //                if (create == null)
    //                    return StatusCode(StatusCodes.Status400BadRequest,
    //                        new PostResponse { success = false, code = "VRF400", response = "INVALID POST REQUEST" });

    //                if (!ModelState.IsValid)
    //                    return StatusCode(StatusCodes.Status422UnprocessableEntity,
    //                        new PostResponse { success = false, code = "VRF422", response = "INVALID POST REQUEST DATA" });



    //                if (string.IsNullOrEmpty(create.Encrypteddata))
    //                {
    //                    return StatusCode(StatusCodes.Status422UnprocessableEntity,
    //                            new PostResponse { success = true, code = "VRF_SUC_200", response = "INVALID ENCRYPTED TOKEN" });
    //                }

    //                var data = EncryptionHelper.DecryptStringFromBytes(create.Encrypteddata, create.Version);

    //                var queryData = data.AsJsonCast();

    //                if (!data.Contains("&"))
    //                {
    //                    return StatusCode(StatusCodes.Status422UnprocessableEntity,
    //                            new PostResponse { success = true, code = "VRF_422", response = "INVALID ENCRYPTED TOKEN" });
    //                }


    //                // validate token
    //                if (!double.TryParse(queryData.TotalAmount, out var totalAmount))
    //                {
    //                    return StatusCode(StatusCodes.Status422UnprocessableEntity,
    //                            new PostResponse { success = true, code = "VRF422", response = "ENCRYPTED TOKEN OF TYPE [TotalAmount] nust be a type of DECIMAL,DOUBLE" });
    //                }

    //                if (!double.TryParse(queryData.VatAmount, out var vatAmount))
    //                {
    //                    return StatusCode(StatusCodes.Status422UnprocessableEntity,
    //                            new PostResponse { success = true, code = "VRF422", response = "ENCRYPTED TOKEN OF TYPE [VatAmount] nust be a type of DECIMAL,DOUBLE" });
    //                }

    //                if (!int.TryParse(queryData.ItemCount, out var itermCount))
    //                {
    //                    return StatusCode(StatusCodes.Status422UnprocessableEntity,
    //                            new PostResponse { success = true, code = "VRF422", response = "ENCRYPTED TOKEN OF TYPE [ItermCount] nust be a type of INT" });
    //                }

    //                if (!DateTime.TryParse(queryData.VsdcTime, out var vsdcTime))
    //                {
    //                    return StatusCode(StatusCodes.Status422UnprocessableEntity,
    //                            new PostResponse { success = true, code = "VRF422", response = "ENCRYPTED TOKEN OF TYPE [VSDC_TIME] nust be a type of DATETIME" });
    //                }

    //                var loadVerification = await _job.GetVerifiedAsync(queryData, create);



    //                return loadVerification switch
    //                {
    //                    true => StatusCode(StatusCodes.Status200OK,
    //                            new PostResponse { success = true, code = "VRF_SUC_200", response = "TRUE" }),

    //                    _ => StatusCode(StatusCodes.Status422UnprocessableEntity,
    //                        new PostResponse { success = false, code = "VRF_SUC_404", response = "FALSE" })
    //                };




    //            }
    //            else
    //            {
    //                if (create == null)
    //                    return StatusCode(StatusCodes.Status400BadRequest,
    //                        new PostResponse { success = false, code = "VRF400", response = "INVALID POST REQUEST" });

    //                if (!ModelState.IsValid)
    //                    return StatusCode(StatusCodes.Status422UnprocessableEntity,
    //                        new PostResponse { success = false, code = "VRF422", response = "INVALID POST REQUEST DATA" });



    //                if (string.IsNullOrEmpty(create.Encrypteddata))
    //                {
    //                    return StatusCode(StatusCodes.Status422UnprocessableEntity,
    //                            new PostResponse { success = true, code = "VRF_SUC_200", response = "INVALID ENCRYPTED TOKEN" });
    //                }

    //                var data = EncryptionHelper.DecryptStringFromBytes(create.Encrypteddata, create.Version);

    //                var queryData = JsonConvert.DeserializeObject<QueryDto_1_1>(data);



    //                // validate token
    //                if (!double.TryParse(queryData.TotalAmount, out var totalAmount))
    //                {
    //                    return StatusCode(StatusCodes.Status422UnprocessableEntity,
    //                            new PostResponse { success = true, code = "VRF422", response = "ENCRYPTED TOKEN OF TYPE [TotalAmount] nust be a type of DECIMAL,DOUBLE" });
    //                }

    //                if (!double.TryParse(queryData.TotalAmount, out var TotalAmount))
    //                {
    //                    return StatusCode(StatusCodes.Status422UnprocessableEntity,
    //                            new PostResponse { success = true, code = "VRF422", response = "ENCRYPTED TOKEN OF TYPE [VatAmount] nust be a type of DECIMAL,DOUBLE" });
    //                }

    //                if (!int.TryParse(queryData.ItemsCount, out var itermsCount))
    //                {
    //                    return StatusCode(StatusCodes.Status422UnprocessableEntity,
    //                            new PostResponse { success = true, code = "VRF422", response = "ENCRYPTED TOKEN OF TYPE [ItermCount] nust be a type of INT" });
    //                }

    //                //if (!DateTime.TryParse(queryData.VsdcTime, out var vsdcTime))
    //                //{
    //                //    return StatusCode(StatusCodes.Status422UnprocessableEntity,
    //                //            new PostResponse { success = true, code = "VRF422", response = "ENCRYPTED TOKEN OF TYPE [VSDC_TIME] nust be a type of DATETIME" });
    //                //}



    //                var loadVerification = await _job.GetVerifiedAsync1_1(queryData, create);



    //                return loadVerification switch
    //                {
    //                    true => StatusCode(StatusCodes.Status200OK,
    //                            new PostResponse { success = true, code = "VRF_SUC_200", response = "TRUE" }),

    //                    _ => StatusCode(StatusCodes.Status422UnprocessableEntity,
    //                        new PostResponse { success = false, code = "VRF_SUC_404", response = "FALSE" })
    //                };

    //            }


    //        }
    //        catch (Exception ex)
    //        {
    //            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
    //        }
    //    }
    //}
}
