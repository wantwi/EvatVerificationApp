import React, { useState } from "react";
import { Input, FormFeedback } from "reactstrap";
import * as yup from "yup";
import { yupResolver } from "@hookform/resolvers/yup";
import { useForm, Controller } from "react-hook-form";
import axios from "axios";

import LOGO from "../assets/h-logo.png";
import VALID from "../assets/stamp.png"
import CurrencyInput from "react-currency-input-field"

// import { NumericTextBoxComponent } from '@syncfusion/ej2-react-inputs';

const defaultValues = {
    invoiceNumber: "",
    payorName: "",
    payeeName: "",
    invoiceDate: "",
    withholdingAmount: "",
    payableAmount: "",
    termsInMonths: "",
    transactionNumber: ""
};

function formatDate(date) {
    const d = new Date(date);
    let month = '' + (d.getMonth() + 1);
    let day = '' + d.getDate();
    const year = d.getFullYear();

    if (month.length < 2) {
        month = '0' + month;
    }
    if (day.length < 2) {
        day = '0' + day;
    }

    return [year, month, day].join('/');
}

const moneyInTxt = (value, standard, dec = 0) => {
    const nf = new Intl.NumberFormat(standard, {
        minimumFractionDigits: dec,
        maximumFractionDigits: 2
    })
    return nf.format(Number(value) ? value : 0.0)
}


const isObjEmpty = (obj) => Object.keys(obj).length === 0;
// const  formattedValue = (inputValue) => parseFloat(inputValue).toLocaleString('en-US');
export const Home = () => {
    const [isValid, setIsValid] = useState(false)
    const [isLoading, setIsLoading] = useState(false)

    const queryParameters = new URLSearchParams(window.location.search)
    const typeData = queryParameters.get("data") || queryParameters.get("Data")
    const version = queryParameters.get("v")
    const [urlSearch] = useState({ data: typeData || "", version })
    const [errTex, setErrText] = useState(false)
    const [vatAmount, setVatAmount] = useState("0.00")
    const [totalAmount, setTotalAmount] = useState("0.00")


    const SignupSchema = yup.object().shape({
        invoiceNumber: yup.string().required("Invoice number is required"),
        transactionNumber: yup.string().required("Transaction number is required"),
        payorName: yup.string().required("Owner name is required"),
        payeeName: yup.string().required("Paid By is required"),
        invoiceDate: yup.string().required("Invoice Date is required"),
        // invoiceTime: yup.string().required("Invoice time is required"),
        withholdingAmount: yup
            .number()
            .required("Vat amount is required")
            .positive("invalid ammount")
            .min(0, "VAT Amount is invalid."),
        payableAmount: yup
            .number()
            .required("Total amont is required")
            .positive()
            .min(0, "Total amount is invalid."),
        termsInMonths: yup
            .number()
            .required("Terms in months is required")
            .positive("invalid ammount")
            .min(1, "Number of terms must be grater than zero."),
    });

    const {
        control,
        handleSubmit,
        formState: { errors },
        setValue
    } = useForm({
        defaultValues,
        resolver: yupResolver(SignupSchema),
    });



    const handleVATAmountInputChange = (e) => {
        const { value } = e.target;

        const formattedValue = value.replace(/[^0-9.]/g, ''); // Remove non-numeric characters
        setVatAmount(formattedValue);
        setValue("withholdingAmount", Number(formattedValue))
    };
    const handleVATAmountInputBlur = () => {
        setVatAmount(moneyInTxt(vatAmount, "en", 2))
    };
    const handleVATAmountInputFocus = () => {
        setVatAmount(vatAmount.replace(/,/g, ""))
    }


    const handleTotalAmountInputChange = (e) => {
        const { value } = e.target;

        const formattedValue = value.replace(/[^0-9.]/g, ''); // Remove non-numeric characters
        setTotalAmount(formattedValue);
        setValue("payableAmount", Number(formattedValue))
    };
    const handleTotalAmountInputBlur = () => {
        //const formattedValue = parseFloat(inputValue).toLocaleString('en-US');
        setTotalAmount(moneyInTxt(totalAmount, "en", 2));
    };
    const handleTotalAmountInputFocus = () => {
        setTotalAmount(totalAmount.replace(/,/g, ""))
    }



    const onSubmit = async (data) => {
        if (isObjEmpty(errors)) {
        }

        setIsLoading(true)
        let postObj = {
            //receiptNumber : data?.invoiceNumber,
            transactionNumber: data?.transactionNumber,
            payorName: data?.payorName,
            payeeName: data?.payeeName,
            CreatedDate: formatDate(data?.invoiceDate),
            // time: data?.invoiceTime,
            // payableAmount: data?.payableAmount,
            withholdingAmount: data?.withholdingAmount,
            //termsInMonths: data?.termsInMonths,
            New: urlSearch?.version ? true : false,
            Encrypteddata: urlSearch?.data.replace(/\s/g, "+"),
            Version: urlSearch?.version,
            Signature: ""
        }

        try {
            const request = await axios.post(`${process.env.REACT_APP_BASENAME}/verify`, postObj)

            if (request?.data?.response === "TRUE") {
                setIsValid(true)
            }
            else if (request?.data?.response === "FALSE") {
                setErrText(true)
            }

            setIsLoading(false)

        } catch (error) {
            setErrText(true)
            setIsLoading(false)
        }
    };


    return (
        <div className="flex-1 flex flex-col p-4 justify-between">
            {!isValid ? <div
                className="w-full md:w-[60%] place-content-center"
                style={{ margin: "0 auto" }}
            >
                <div className="grid place-content-center mt-4 mb-5 ">
                    <img src={LOGO} width={200} alt="logo" className="shadow-sm p-2" />
                </div>

                {errTex ? <div className="grid place-content-center bg-red-400 p-4 rounded-md opacity-60">
                    <h1 className="text-yellow-50 ">Sorry, your invoice is invalid.</h1>
                </div> : null
                }

                {
                    urlSearch?.data ?
                        <form onSubmit={handleSubmit(onSubmit)}>
                            <div
                                className="grid gap-3"
                                style={{
                                    display: "grid",
                                    gridTemplateColumns: "repeat( auto-fit, minmax(250px, 1fr) )",
                                }}
                            >
                                <div className="flex flex-col gap-1">
                                    <label>Invoice Number</label>
                                    <Controller
                                        id="invoiceNumber"
                                        name="invoiceNumber"
                                        control={control}
                                        render={({ field }) => (
                                            <Input
                                                className="border border-gray-300 bg-[#ECEEF0] text-gray-900 sm:text-sm rounded-lg focus:ring-blue-500 focus:border-blue-500 block w-full p-2.5"
                                                placeholder="Invoice Number"
                                                invalid={errors.invoiceNumber && true}
                                                {...field}
                                            />
                                        )}
                                    />
                                    {errors.invoiceNumber && (
                                        <FormFeedback>{errors.invoiceNumber.message}</FormFeedback>
                                    )}
                                </div>
                                <div className="flex flex-col gap-1">
                                    <label>Transaction Number</label>
                                    <Controller
                                        id="transactionNumber"
                                        name="transactionNumber"
                                        control={control}
                                        render={({ field }) => (
                                            <Input
                                                className="border border-gray-300 bg-[#ECEEF0] text-gray-900 sm:text-sm rounded-lg focus:ring-blue-500 focus:border-blue-500 block w-full p-2.5"
                                                placeholder="Transaction Number"
                                                invalid={errors.transactionNumber && true}
                                                {...field}
                                            />
                                        )}
                                    />
                                    {errors.transactionNumber && (
                                        <FormFeedback>{errors.transactionNumber.message}</FormFeedback>
                                    )}
                                </div>


                            </div>
                            <div
                                className="grid gap-3 mt-2"
                                style={{
                                    display: "grid",
                                    gridTemplateColumns: "repeat( auto-fit, minmax(250px, 1fr) )",
                                }}
                            >
                                <div className="flex flex-col gap-1">
                                    <label>Owner</label>
                                    <Controller
                                        id="payorName"
                                        name="payorName"
                                        control={control}
                                        render={({ field }) => (
                                            <Input
                                                className="border border-gray-300 bg-[#ECEEF0] text-gray-900 sm:text-sm rounded-lg focus:ring-blue-500 focus:border-blue-500 block w-full p-2.5"
                                                placeholder="Owner Name"
                                                invalid={errors.payorName && true}
                                                {...field}
                                            />
                                        )}
                                    />
                                    {errors.payorName && <FormFeedback>{errors.payorName.message}</FormFeedback>}
                                </div>

                                <div className="flex flex-col gap-1">
                                    <label>Paid By</label>
                                    <Controller
                                        id="payeeName"
                                        name="payeeName"
                                        control={control}
                                        render={({ field }) => (
                                            <Input
                                                className="border border-gray-300 bg-[#ECEEF0] text-gray-900 sm:text-sm rounded-lg focus:ring-blue-500 focus:border-blue-500 block w-full p-2.5"
                                                placeholder="Paid by"
                                                invalid={errors.payeeName && true}
                                                {...field}
                                            />
                                        )}
                                    />
                                    {errors.payeeName && <FormFeedback>{errors.payeeName.message}</FormFeedback>}
                                </div>

                            </div>

                            <div
                                className="grid gap-3 mt-2"
                                style={{
                                    display: "grid",
                                    gridTemplateColumns: "repeat( auto-fit, minmax(250px, 1fr) )",
                                }}
                            >
                                <div className="flex flex-col gap-1">
                                    <label>Invoice Date</label>
                                    <Controller
                                        id="invoiceDate"
                                        name="invoiceDate"
                                        control={control}
                                        render={({ field }) => (
                                            <Input
                                                type="date"
                                                className="border border-gray-300 bg-[#ECEEF0] text-gray-900 sm:text-sm rounded-lg focus:ring-blue-500 focus:border-blue-500 block w-full p-2.5"
                                                placeholder="Invoce Date"
                                                invalid={errors.invoiceDate && true}
                                                {...field}
                                            />
                                        )}
                                    />
                                    {errors.invoiceDate && (
                                        <FormFeedback>{errors.invoiceDate.message}</FormFeedback>
                                    )}
                                </div>
                                <div className="flex flex-col gap-1">
                                    <label>WHT Amount</label>
                                    <Controller
                                        name={`withholdingAmount`}
                                        control={control}
                                        rules={{ required: true }} // Add your validation rules here
                                        render={({ field, fieldState }) => (
                                            <CurrencyInput
                                                id="withholdingAmount"
                                                name={field.name}
                                                value={field.value}
                                                className={`border border-gray-300 bg-[#ECEEF0] text-gray-900 sm:text-sm rounded-lg focus:ring-blue-500 focus:border-blue-500 block w-full p-2.5 text-r ${errors.withholdingAmount ? 'is-invalid' : 'text-r'}`}
                                                placeholder="0.00"
                                                decimalsLimit={2}
                                                onValueChange={(value, name) => field.onChange(value)}
                                                style={{ textAlign: "right" }}

                                            />
                                        )}
                                    />
                                    {/* <input value={vatAmount} onChange={handleVATAmountInputChange} onBlur={handleVATAmountInputBlur} onFocus={handleVATAmountInputFocus} className="border border-gray-300 text-right bg-[#ECEEF0] text-gray-900 sm:text-sm rounded-lg focus:ring-blue-500 focus:border-blue-500 block w-full p-2.5" />

                  <Controller
                    id="withholdingAmount"
                    name="withholdingAmount"

                    control={control}
                    render={({ field }) => (
                      <Input
                        type="number"
                        hidden
                        className="border border-gray-300 text-right bg-[#ECEEF0] text-gray-900 sm:text-sm rounded-lg focus:ring-blue-500 focus:border-blue-500 block w-full p-2.5"
                        placeholder="Total Amount"
                        invalid={errors.withholdingAmount && true}
                        {...field}

                      />
                    )}
                  /> */}


                                    {errors.withholdingAmount && (
                                        <FormFeedback>{errors.withholdingAmount.message}</FormFeedback>
                                    )}
                                </div>
                                {/*<div className="flex flex-col gap-1">*/}
                                {/*  <label>Terms (Months)</label>*/}
                                {/*  <Controller*/}
                                {/*    name={`termsInMonths`}*/}
                                {/*    control={control}*/}
                                {/*    rules={{ required: true }} // Add your validation rules here*/}
                                {/*    render={({ field, fieldState }) => (*/}
                                {/*      <CurrencyInput*/}
                                {/*        id="termsInMonths"*/}
                                {/*        name={field.name}*/}
                                {/*        value={field.value}*/}
                                {/*        className={`border border-gray-300 bg-[#ECEEF0] text-gray-900 sm:text-sm rounded-lg focus:ring-blue-500 focus:border-blue-500 block w-full p-2.5 text-r ${errors.termsInMonths ? 'is-invalid' : 'text-r'}`}*/}
                                {/*        placeholder="0.00"*/}
                                {/*        decimalsLimit={0}*/}
                                {/*        onValueChange={(value, name) => field.onChange(value)}*/}
                                {/*        style={{textAlign:"right"}}*/}

                                {/*      />*/}
                                {/*    )}*/}
                                {/*  />*/}
                                {/* <Controller
                    id="termsInMonths"
                    name="termsInMonths"
                    control={control}
                    render={({ field }) => (
                      <Input
                        type="number"
                        min={1}
                        className="border border-gray-300 bg-[#ECEEF0] text-gray-900 sm:text-sm rounded-lg focus:ring-blue-500 focus:border-blue-500 block w-full p-2.5"
                        placeholder="Number of Item(s)"
                        invalid={errors.termsInMonths && true}
                        {...field}
                      />
                    )}
                  /> */}
                                {/*{errors.termsInMonths && (*/}
                                {/*  <FormFeedback>{errors.termsInMonths.message}</FormFeedback>*/}
                                {/*)}*/}
                                {/*</div>*/}

                                {/* <div className="flex flex-col gap-1" hidden>
                  <label>Invoice Time</label>
                  <Controller
                    id="invoiceTime"
                    name="invoiceTime"
                    control={control}
                    render={({ field }) => (
                      <Input

                        type="time"
                        className="border border-gray-300 bg-[#ECEEF0] text-gray-900 sm:text-sm rounded-lg focus:ring-blue-500 focus:border-blue-500 block w-full p-2.5"
                        placeholder="invoiceTime"
                        invalid={errors.invoiceTime && true}
                        {...field}

                      />
                    )}
                  />
                  {errors.invoiceTime && (
                    <FormFeedback>{errors.invoiceTime.message}</FormFeedback>
                  )}
                </div> */}
                            </div>

                            <div
                                className="grid gap-3 mt-2"
                                style={{
                                    display: "grid",
                                    gridTemplateColumns: "repeat( auto-fit, minmax(250px, 1fr) )",
                                }}
                            >
                                {/*<div className="flex flex-col gap-1">*/}
                                {/*  <label>Payable Amount</label>*/}
                                {/*  */}{/* <input value={totalAmount} onChange={handleTotalAmountInputChange} onBlur={handleTotalAmountInputBlur} onFocus={handleTotalAmountInputFocus} className="border border-gray-300 text-right bg-[#ECEEF0] text-gray-900 sm:text-sm rounded-lg focus:ring-blue-500 focus:border-blue-500 block w-full p-2.5" /> */}
                                {/*  <Controller*/}
                                {/*    name={`payableAmount`}*/}
                                {/*    control={control}*/}
                                {/*    rules={{ required: true }} // Add your validation rules here*/}
                                {/*    render={({ field, fieldState }) => (*/}
                                {/*      <CurrencyInput*/}
                                {/*        id="input-example"*/}
                                {/*        name={field.name}*/}
                                {/*        value={field.value}*/}
                                {/*        className={`border border-gray-300 bg-[#ECEEF0] text-gray-900 sm:text-sm rounded-lg focus:ring-blue-500 focus:border-blue-500 block w-full p-2.5 text-r ${errors.payableAmount ? 'is-invalid' : 'text-r'}`}*/}
                                {/*        placeholder="0.00"*/}
                                {/*        decimalsLimit={2}*/}
                                {/*        onValueChange={(value, name) => field.onChange(value)}*/}
                                {/*        style={{textAlign:"right"}}*/}

                                {/*      />*/}
                                {/*    )}*/}
                                {/*  />*/}

                                {/*  {errors.payableAmount && (*/}
                                {/*    <FormFeedback>{errors.payableAmount.message}</FormFeedback>*/}
                                {/*  )}*/}
                                {/*</div>*/}


                            </div>
                            <div
                                hidden
                                className="grid gap-3 mt-2"
                                style={{
                                    display: "grid",
                                    gridTemplateColumns: "repeat( auto-fit, minmax(250px, 1fr))",
                                }}
                            >

                                <div> </div>


                            </div>
                            <button disabled={isLoading} className={`w-full  md:w-[200px] bg-[#00a4d7] rounded-md py-2.5 text-white font-bold focus:ring-blue-500 focus:border-blue-500 mt-3 ${isLoading ?? 'animate-pulse'}`}>
                                {isLoading ? "Processing ..." : "Verify"}
                            </button>


                        </form> :
                        <div className="grid place-content-center bg-red-400 p-4 rounded-md opacity-60">
                            <h1 className="text-yellow-50 text-2xl ">Please scan the QR Code on your invoice</h1>

                        </div>
                }

            </div> :
                <div className="grid place-content-center p-4 rounded-md h-[90%]">
                    <img src={VALID} width={200} alt="" />
                    <div className="grid place-content-center bg-green-400 p-2 rounded-md opacity-60 mt-3">
                        <h1 className="text-green-800 ">Your invoice is valid.</h1>
                    </div>

                    {/* <button onClick={() => setIsValid(false)} className="w-full  md:w-[200px] bg-[#00a4d7] rounded-md py-2.5 text-white font-bold focus:ring-blue-500 focus:border-blue-500 mt-3">Back</button> */}


                </div>
            }

        </div>
    );
};
