//===================================================================================
// Microsoft patterns & practices
// Composite Application Guidance for Windows Presentation Foundation and Silverlight
//===================================================================================
// Copyright (c) Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===================================================================================
// The example companies, organizations, products, domain names,
// e-mail addresses, logos, people, places, and events depicted
// herein are fictitious.  No association with any real company,
// organization, product, domain name, email address, logo, person,
// places, or events is intended or should be inferred.
//===================================================================================

using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Prism.Regions.Behaviors;
using Microsoft.Practices.ServiceLocation;
using System.Windows;

namespace CallWall.PrismExtensions
{
    /// <summary>
    /// Declares the Attached Properties and Behaviors for implementing Popup regions.
    /// </summary>
    /// <remarks>
    /// Although the fastest way is to create a RegionAdapter for a Window and register it with the RegionAdapterMappings,
    /// this would be conceptually incorrect because we want to create a new popup window every time a view is added 
    /// (instead of having a Window as a host control and replacing its contents every time Views are added, as other adapters do).
    /// This is why we have a different class for this behavior, instead of reusing the <see cref="RegionManager.RegionNameProperty"/> attached property.
    /// </remarks>
    public static class RegionPopupBehaviors
    {

        /// <summary>
        /// Creates a new <see cref="IRegion"/> and registers it in the default <see cref="IRegionManager"/>
        /// attaching to it a <see cref="DialogActivationBehavior"/> behavior.
        /// </summary>
        /// <param name="regionName">The name of the <see cref="IRegion"/>.</param>
        /// <param name="windowStyle">The style to apply to the window region.</param>
        public static void RegisterNewWindowRegion(string regionName, Style windowStyle)
        {
            // Creates a new region and registers it in the default region manager.
            // Another option if you need the complete infrastructure with the default region behaviors
            // is to extend DelayedRegionCreationBehavior overriding the CreateRegion method and create an 
            // instance of it that will be in charge of registering the Region once a RegionManager is
            // set as an attached property in the Visual Tree.
            var regionManager = ServiceLocator.Current.GetInstance<IRegionManager>();
            if (regionManager != null)
            {
                IRegion region = new Region();
                var behavior = new WindowDialogActivationBehavior {WindowStyle = windowStyle};
                region.Behaviors.Add(DialogActivationBehavior.BehaviorKey, behavior);
                region.Behaviors.Add(RegionActiveAwareBehavior.BehaviorKey, new RegionActiveAwareBehavior());
                regionManager.Regions.Add(regionName, region);
            }
        }
    }
}
