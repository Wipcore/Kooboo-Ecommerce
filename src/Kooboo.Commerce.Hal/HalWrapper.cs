﻿using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.API;
using Kooboo.Commerce.HAL.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.HAL
{
    [Dependency(typeof(IHalWrapper))]
    public class HalWrapper : IHalWrapper
    {
        private IUriResolver _uriResolver;
        private IResourceLinkPersistence _resourceLinkPersistence;
        private IResourceDescriptorProvider _resourceDescriptorProvider;

        public HalWrapper(
            IResourceDescriptorProvider resourceDescriptorProvider,
            IResourceLinkPersistence resourceLinkPersistence,
            IUriResolver uriResolver)
        {
            _resourceDescriptorProvider = resourceDescriptorProvider;
            _resourceLinkPersistence = resourceLinkPersistence;
            _uriResolver = uriResolver;
        }

        public void AddLinks(string resourceName, IItemResource resource, IDictionary<string, object> parameterValues)
        {
            if (String.IsNullOrEmpty(resourceName))
                throw new ArgumentException("Resource name is required.", "resourceName");

            if (resource == null)
                throw new ArgumentNullException("resource");

            var descriptor = _resourceDescriptorProvider.GetDescriptor(resourceName);
            AssertDescriptorNotNull(descriptor, resourceName);
            FillLinksNoRecursive(descriptor, resource, parameterValues);
        }

        public void AddLinks<T>(string resourceName, IListResource<T> resource, IDictionary<string, object> parameterValues, Func<T, IDictionary<string, object>> itemParameterValuesResolver)
            where T : IItemResource
        {
            if (String.IsNullOrEmpty(resourceName))
                throw new ArgumentException("Resource name is required.", "resourceName");

            if (resource == null)
                throw new ArgumentNullException("resource");

            var descriptor = _resourceDescriptorProvider.GetDescriptor(resourceName);

            AssertDescriptorNotNull(descriptor, resourceName);
            FillLinksNoRecursive(descriptor, resource, parameterValues);

            var itemDescriptor = _resourceDescriptorProvider.GetDescriptor(descriptor.ItemResourceName);
            AssertDescriptorNotNull(itemDescriptor, descriptor.ItemResourceName);

            foreach (var item in resource)
            {
                var itemParamValues = itemParameterValuesResolver == null ? null : itemParameterValuesResolver(item);
                FillLinksNoRecursive(itemDescriptor, item, itemParamValues);
            }
        }

        private void FillLinksNoRecursive(ResourceDescriptor descriptor, IResource resource, IDictionary<string, object> parameterValues)
        {
            // Add self
            resource.Links.Add(new Link
            {
                Rel = "self",
                Href = _uriResolver.Resovle(descriptor.ResourceUri, parameterValues)
            });

            var savedLinks = _resourceLinkPersistence.GetLinks(descriptor.ResourceName);

            foreach (var savedLink in savedLinks)
            {
                var targetResourceDescriptor = _resourceDescriptorProvider.GetDescriptor(savedLink.DestinationResourceName);

                AssertDescriptorNotNull(targetResourceDescriptor, savedLink.DestinationResourceName);

                var link = new Link
                {
                    Rel = savedLink.Relation,
                    Href = _uriResolver.Resovle(targetResourceDescriptor.ResourceUri, parameterValues)
                };

                resource.Links.Add(link);
            }
        }

        private void AssertDescriptorNotNull(ResourceDescriptor descriptor, string resourceName)
        {
            if (descriptor == null)
                throw new InvalidOperationException("Cannot find resource descriptor for resource: " + resourceName + ", ensure the resource name is correct.");
        }
    }
}
