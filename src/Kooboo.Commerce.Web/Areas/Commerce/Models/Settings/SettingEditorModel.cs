﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Kooboo.Commerce.Settings;

namespace Kooboo.Commerce.Web.Areas.Commerce.Models.Settings
{

    public class SettingEditorModel
    {

        public SettingEditorModel()
        {
        }

        public StoreSettingEditorModel StoreSetting
        {
            get;
            set;
        }

        public ImageSettingEditorModel ImageSetting
        {
            get;
            set;
        }

        public ProductSettingEditorModel ProductSetting
        {
            get;
            set;
        }
    }
}