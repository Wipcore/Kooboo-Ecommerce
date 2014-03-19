﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Kooboo.CMS.Membership.Models;
using Kooboo.Commerce.API.ShoppingCarts;

namespace Kooboo.Commerce.WebAPI.Controllers
{
    public class ShoppingCartController : CommerceAPIControllerBase
    {
        // GET api/cart/5
        public ShoppingCart Get(string sessionId)
        {
            return Commerce().ShoppingCart.BySessionId(sessionId).FirstOrDefault();
        }

        // POST api/cart
        public bool Post([FromBody]Dictionary<string, object> form)
        {
            string sessionId = form["sessionId"].ToString();
            string accountId = form["accountId"].ToString();
            int productPriceId = Convert.ToInt32(form["productPriceId"]);
            int quantity = Convert.ToInt32(form["quantity"]);

            var cart = Commerce().ShoppingCart.BySessionId(sessionId).FirstOrDefault();
            if(cart == null)
            {
                cart = Commerce().ShoppingCart.ByAccountId(accountId).FirstOrDefault();
            }
            var cartItem = new ShoppingCartItem();

            return Commerce().ShoppingCart.AddToCart(sessionId, accountId, productPriceId, quantity);
        }

        // PUT api/cart/5
        public bool Put([FromBody]Dictionary<string, object> form)
        {
            string sessionId = form["sessionId"].ToString();
            string accountId = form["accountId"].ToString();
            int productPriceId = Convert.ToInt32(form["productPriceId"]);
            int quantity = Convert.ToInt32(form["quantity"]);

            return Commerce().ShoppingCart.UpdateCart(sessionId, accountId, productPriceId, quantity);
        }

        // DELETE api/cart/5
        public bool Delete(string sessionId, string accountId)
        {
            var cart = Commerce().ShoppingCart.BySessionId(sessionId).FirstOrDefault();
            if (cart == null)
            {
                cart = Commerce().ShoppingCart.ByAccountId(accountId).FirstOrDefault();
            }
            if (cart != null)
            {
                try
                {
                    Commerce().ShoppingCart.Delete(cart);
                    return true;
                }
                catch
                {
                    return false;
                }
            }
            return false;
        }
    }
}