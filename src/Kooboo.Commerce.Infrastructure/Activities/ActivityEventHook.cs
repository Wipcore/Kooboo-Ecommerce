﻿using Kooboo.Commerce.Events;
using Kooboo.Commerce.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Kooboo.CMS.Common.Runtime;
using Kooboo.Commerce.Events.Dispatching;

namespace Kooboo.Commerce.Activities
{
    // Activities can only bind to domain events,
    // because listing infrastructure events in the UI is weird.
    // This can also benifit the performance.
    abstract class ActivityEventHookBase : IHandles<IEvent>
    {
        public CommerceInstanceContext CommerceInstanceContext { get; protected set; }

        protected ActivityEventHookBase(CommerceInstanceContext commerceInstanceContext)
        {
            Require.NotNull(commerceInstanceContext, "commerceInstanceContext");
            CommerceInstanceContext = commerceInstanceContext;
        }

        public void Handle(IEvent @event, EventDispatchingContext context)
        {
            var commerceInstance = CommerceInstanceContext.CurrentInstance;

            // Activities must be executed within commerce instance context
            if (commerceInstance == null)
            {
                return;
            }

            DoHandle(@event, commerceInstance, context);
        }

        protected abstract void DoHandle(IEvent @event, CommerceInstance commerceInstance, EventDispatchingContext context);
    }

    class ActivityEventHook_OnEventRaised : ActivityEventHookBase
    {
        private IActivityFactory _activityFactory;

        public ActivityEventHook_OnEventRaised(IActivityFactory activityFactory, CommerceInstanceContext context)
            : base(context)
        {
            Require.NotNull(activityFactory, "activityFactory");
            _activityFactory = activityFactory;
        }

        protected override void DoHandle(IEvent @event, CommerceInstance commerceInstance, EventDispatchingContext context)
        {
            var executor = new ActivityExecutor(_activityFactory, commerceInstance.Database.GetRepository<ActivityRule>());
            executor.ExecuteActivities(@event, context);
        }
    }

    [AwaitTransactionComplete]
    class ActivityEventHook_OnTransactionCommitted : ActivityEventHookBase
    {
        private IActivityFactory _activityFactory;

        public ActivityEventHook_OnTransactionCommitted(IActivityFactory activityFactory, CommerceInstanceContext context)
            : base(context)
        {
            _activityFactory = activityFactory;
        }

        protected override void DoHandle(IEvent @event, CommerceInstance commerceInstance, EventDispatchingContext context)
        {
            var executor = new ActivityExecutor(_activityFactory, commerceInstance.Database.GetRepository<ActivityRule>());
            executor.ExecuteActivities(@event, context);
        }
    }
}
