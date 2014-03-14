﻿using Kooboo.CMS.Common;
using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Web.Mvc;
using Kooboo.Commerce.Web.Mvc.Controllers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kooboo.Commerce.Activities.OrderInvoiceMailing.Controllers
{
    public class HomeController : CommerceControllerBase
    {
        private IActivityRuleService _ruleService;

        public HomeController(IActivityRuleService ruleService)
        {
            _ruleService = ruleService;
        }

        public ActionResult Settings(int ruleId, int attachedActivityId)
        {
            var rule = _ruleService.GetById(ruleId);
            var attachedActivity = rule.FindAttachedActivity(attachedActivityId);
            var settings = new ActivityData();

            if (!String.IsNullOrEmpty(attachedActivity.ActivityData))
            {
                settings = JsonConvert.DeserializeObject<ActivityData>(attachedActivity.ActivityData);
            }

            return View(settings);
        }

        [HttpPost, HandleAjaxFormError, Transactional]
        public ActionResult Settings(int ruleId, int attachedActivityId, ActivityData settings)
        {
            var rule = _ruleService.GetById(ruleId);
            var attachedActivity = rule.FindAttachedActivity(attachedActivityId);
            attachedActivity.ActivityData = JsonConvert.SerializeObject(settings);

            return AjaxForm();
        }
    }
}
