﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Events
{
    public enum WhenNotInTransaction
    {
        ExecuteImmediately = 0,
        DoNotExecute = 1
    }
}