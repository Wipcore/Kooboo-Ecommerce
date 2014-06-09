﻿using Kooboo.Commerce.Orders;
using Kooboo.Commerce.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Events.Orders
{
    [Event(Order = 200)]
    public class OrderStatusChanged : DomainEvent, IOrderEvent
    {
        [Reference(typeof(Order), Prefix = "")]
        public int OrderId { get; set; }

        [Param]
        public OrderStatus OldStatus { get; set; }

        [Param]
        public OrderStatus NewStatus { get; set; }

        private OrderStatusChanged() { }

        public OrderStatusChanged(Order order, OrderStatus oldStatus, OrderStatus newStatus)
        {
            OrderId = order.Id;
            OldStatus = oldStatus;
            NewStatus = newStatus;
        }
    }
}
