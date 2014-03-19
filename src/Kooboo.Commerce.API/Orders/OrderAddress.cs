﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.Commerce.API.Locations;

namespace Kooboo.Commerce.API.Orders
{
    public class OrderAddress
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }

        public int CountryId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Address1 { get; set; }

        /// <summary>
        /// optional
        /// </summary>
        public string Address2 { get; set; }

        public string Postcode { get; set; }

        public string City { get; set; }

        /// <summary>
        /// optional
        /// </summary>
        public string State { get; set; }

        public Country Country { get; set; }

        /// <summary>
        /// optional
        /// </summary>
        public string Phone { get; set; }

        public void FromAddress(Address address)
        {
            this.CustomerId = address.CustomerId;
            this.CountryId = address.CountryId;
            this.FirstName = address.FirstName;
            this.LastName = address.LastName;
            this.Address1 = address.Address1;
            this.Address2 = address.Address2;
            this.Postcode = address.Postcode;
            this.City = address.City;
            this.State = address.State;
            this.Phone = address.Phone;
        }
    }
}
