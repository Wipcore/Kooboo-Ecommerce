﻿using Kooboo.Commerce.Settings.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Payments.PayPal
{
    public class PayPalSettings
    {
        [Display(Name = "API username")]
        public string ApiUserName { get; set; }

        [Display(Name = "API password")]
        public string ApiPassword { get; set; }

        [Display(Name = "API signature")]
        public string ApiSignature { get; set; }

        [Display(Name = "Application ID")]
        public string ApplicationId { get; set; }

        [Display(Name = "Merchant account")]
        public string MerchantAccount { get; set; }

        [Display(Name = "Sandbox mode")]
        public bool SandboxMode { get; set; }

        public PayPalSettings()
        {
            ApiUserName = "jb-us-seller_api1.paypal.com";
            ApiPassword = "WX4WTU3S8MY44S7F";
            ApiSignature = "AFcWxV21C7fd0v3bYYYRCpSSRl31A7yDhhsPUU2XhtMoZXsWHFxu-RWy";
            ApplicationId = "APP-80W284485P519543T";
        }

        public string Serialize()
        {
            return JsonConvert.SerializeObject(this);
        }

        public static PayPalSettings Deserialize(string data)
        {
            if (String.IsNullOrEmpty(data))
            {
                return null;
            }

            return JsonConvert.DeserializeObject<PayPalSettings>(data);
        }

        public void SaveTo(IKeyValueService service)
        {
            service.Set("Kooboo.Commerce.Payments.PayPal", Serialize());
        }

        public static PayPalSettings FetchFrom(IKeyValueService service)
        {
            return Deserialize(service.Get("Kooboo.Commerce.Payments.PayPal"));
        }
    }
}