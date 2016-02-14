﻿namespace Mollie.Api.Models.PaymentMethod {
    public class PaymentMethodResponse {
        /// <summary>
        /// The unique identifier of the payment method. When used during payment creation, the payment method selection screen will be skipped.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// The full name of the payment method.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The minimum and maximum allowed payment amount will differ between payment methods.
        /// </summary>
        public PaymentMethodResponseAmount ResponseAmount { get; set; }

        /// <summary>
        /// URLs of images representing the payment method.
        /// </summary>
        public PaymentMethodResponseImage Image { get; set; }
    }
}
