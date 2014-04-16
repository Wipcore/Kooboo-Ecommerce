﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.HAL
{
    public class ResourceLink
    {
        public string Id { get; set; }

        public string SourceResourceName { get; set; }

        public string DestinationResourceName { get; set; }

        public string Relation { get; set; }

        //public IDictionary<string, string> ParameterMappings { get; set; }

        public ResourceLink()
        {
            Id = Guid.NewGuid().ToString();
        }

        public ResourceLink Clone()
        {
            return (ResourceLink)MemberwiseClone();
        }
    }
}
