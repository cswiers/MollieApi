﻿using System;
using Mollie.Api.Models.List;
using Mollie.Api.Models.Payment;
using Mollie.Api.Models.Payment.Request;
using Mollie.Api.Models.Payment.Response;
using Mollie.Tests.Integration.Framework;
using NUnit.Framework;

namespace Mollie.Tests.Integration.Api {
    [TestFixture]
    public class PaymentTests : BaseMollieApiTestClass {
        [Test]
        public void CanRetrievePaymentList() {
            // When: Retrieve payment list with default settings
            ListResponse<PaymentResponse> response = this._mollieClient.GetPaymentList().Result;

            // Then
            Assert.IsNotNull(response);
        }

        [Test]
        public void ListPaymentsNeverReturnsMorePaymentsThenTheNumberOfRequestedPayments() {
            // If: Number of payments requested is 5
            int numberOfPayments = 5;

            // When: Retrieve 5 payments
            ListResponse<PaymentResponse> response = this._mollieClient.GetPaymentList(0, numberOfPayments).Result;

            // Then
            Assert.IsTrue(response.Count <= numberOfPayments);
        }

        [Test]
        public void CanCreateDefaultPaymentWithOnlyRequiredFields() {
            // If: we create a payment request with only the required parameters
            PaymentRequest paymentRequest = new PaymentRequest() {
                Amount = 100,
                Description = "Description",
                RedirectUrl = this.DefaultRedirectUrl
            };

            // When: We send the payment request to Mollie
            PaymentResponse result = this._mollieClient.CreatePayment(paymentRequest).Result;

            // Then: Make sure we get a valid response
            Assert.IsNotNull(result);
            Assert.AreEqual(paymentRequest.Amount, result.Amount);
            Assert.AreEqual(paymentRequest.Description, result.Description);
            Assert.AreEqual(paymentRequest.RedirectUrl, result.Links.RedirectUrl);
        }

        [Test]
        public void CanCreateDefaultPaymentWithAllFields() {
            // If: we create a payment request where all parameters have a value
            PaymentRequest paymentRequest = new PaymentRequest() {
                Amount = 100,
                Description = "Description",
                RedirectUrl = this.DefaultRedirectUrl,
                Locale = "nl",
                Metadata = "Our metadata",
                Method = PaymentMethod.BankTransfer,
                WebhookUrl = this.DefaultWebhookUrl
            };

            // When: We send the payment request to Mollie
            PaymentResponse result = this._mollieClient.CreatePayment(paymentRequest).Result;

            // Then: Make sure all requested parameters match the response parameter values
            Assert.IsNotNull(result);
            Assert.AreEqual(paymentRequest.Amount, result.Amount);
            Assert.AreEqual(paymentRequest.Description, result.Description);
            Assert.AreEqual(paymentRequest.RedirectUrl, result.Links.RedirectUrl);
            Assert.AreEqual(paymentRequest.Locale, result.Locale);
            Assert.AreEqual(paymentRequest.Metadata, result.Metadata);
            Assert.AreEqual(paymentRequest.WebhookUrl, result.Links.WebhookUrl);
        }

        [TestCase(typeof(IDealPaymentRequest), PaymentMethod.Ideal)]
        [TestCase(typeof(CreditCardPaymentRequest), PaymentMethod.CreditCard)]
        [TestCase(typeof(PaymentRequest), PaymentMethod.MisterCash)]
        [TestCase(typeof(PaymentRequest), PaymentMethod.Sofort)]
        [TestCase(typeof(BankTransferPaymentRequest), PaymentMethod.BankTransfer)]
        [TestCase(typeof(PaymentRequest), PaymentMethod.DirectDebit)]
        [TestCase(typeof(PaymentRequest), PaymentMethod.Belfius)]
        [TestCase(typeof(PayPalPaymentRequest), PaymentMethod.PayPal)]
        [TestCase(typeof(PaymentRequest), PaymentMethod.Bitcoin)]
        [TestCase(typeof(PaymentRequest), PaymentMethod.PodiumCadeaukaart)]
        public void CanCreateSpecificPaymentType(Type paymentType, PaymentMethod paymentMethod ) {
            // If: we create a specific payment type with some bank transfer specific values
            PaymentRequest paymentRequest = (PaymentRequest) Activator.CreateInstance(paymentType);
            paymentRequest.Amount = 100;
            paymentRequest.Description = "Description";
            paymentRequest.RedirectUrl = this.DefaultRedirectUrl;
            paymentRequest.Method = paymentMethod;

            // When: We send the payment request to Mollie
            PaymentResponse result = this._mollieClient.CreatePayment(paymentRequest).Result;

            // Then: Make sure all requested parameters match the response parameter values
            Assert.IsNotNull(result);
            Assert.AreEqual(paymentRequest.Amount, result.Amount);
            Assert.AreEqual(paymentRequest.Description, result.Description);
            Assert.AreEqual(paymentRequest.RedirectUrl, result.Links.RedirectUrl);
        }

        public void CanCreatePaymentAndRetrieveIt() {
            // If: we create a new payment request
            PaymentRequest paymentRequest = new PaymentRequest() {
                Amount = 100,
                Description = "Description",
                RedirectUrl = this.DefaultRedirectUrl
            };

            // When: We send the payment request to Mollie and attempt to retrieve it
            PaymentResponse paymentResponse = this._mollieClient.CreatePayment(paymentRequest).Result;
            PaymentResponse result = this._mollieClient.GetPayment(paymentResponse.Id).Result;

            Assert.IsNotNull(result);
            Assert.AreEqual(paymentResponse.Id, result.Id);
            Assert.AreEqual(paymentResponse.Amount, result.Amount);
            Assert.AreEqual(paymentResponse.Description, result.Description);
            Assert.AreEqual(paymentResponse.Links.RedirectUrl, result.Links.RedirectUrl);
        }
    }
}
