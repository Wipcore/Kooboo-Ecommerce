﻿using Kooboo.Commerce.ShoppingCarts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Events.ShoppingCarts
{
    [Event(Order = 200)]
    public class CartItemAdded : DomainEvent, IShoppingCartEvent
    {
        public int CartId { get; set; }

        public int ItemId { get; set; }

        public int ProductId { get; set; }

        public int ProductPriceId { get; set; }

        public int Quantity { get; set; }

        protected CartItemAdded() { }

        public CartItemAdded(ShoppingCart cart, ShoppingCartItem item)
        {
            CartId = cart.Id;
            ItemId = item.Id;
            ProductId = item.ProductPrice.ProductId;
            ProductPriceId = item.ProductPrice.Id;
            Quantity = item.Quantity;
        }
    }
}
