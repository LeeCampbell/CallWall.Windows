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
using System;
using System.ComponentModel;
using System.Windows;

namespace CallWall.PrismExtensions
{
    /// <summary>
    /// Declares the Attached Properties and Behaviours for implementing Popup regions.
    /// </summary>
    /// <remarks>
    /// Although the fastest way is to create a RegionAdapter for a Window and register it with the RegionAdapterMappings,
    /// this would be conceptually incorrect because we want to create a new popup window every time a view is added 
    /// (instead of having a Window as a host control and replacing its contents every time Views are added, as other adapters do).
    /// This is why we have a different class for this behaviour, instead of reusing the <see cref="RegionManager.RegionNameProperty"/> attached property.
    /// </remarks>
    public static class RegionPopupBehaviors
    {
        /// <summary>
        /// The name of the Popup <see cref="IRegion"/>.
        /// </summary>
        public static readonly DependencyProperty CreatePopupRegionWithNameProperty =
            DependencyProperty.RegisterAttached("CreatePopupRegionWithName", typeof(string), typeof(RegionPopupBehaviors), new PropertyMetadata(CreatePopupRegionWithNamePropertyChanged));

        /// <summary>
        /// The <see cref="Style"/> to set to the Popup.
        /// </summary>
        public static readonly DependencyProperty ContainerWindowStyleProperty =
          DependencyProperty.RegisterAttached("ContainerWindowStyle", typeof(Style), typeof(RegionPopupBehaviors), null);

        /// <summary>
        /// Gets the name of the Popup <see cref="IRegion"/>.
        /// </summary>
        /// <param name="owner">Owner of the Popup.</param>
        /// <returns>The name of the Popup <see cref="IRegion"/>.</returns>
        public static string GetCreatePopupRegionWithName(DependencyObject owner)
        {
            if (owner == null)
            {
                throw new ArgumentNullException("owner");
            }

            return owner.GetValue(CreatePopupRegionWithNameProperty) as string;
        }

        /// <summary>
        /// Sets the name of the Popup <see cref="IRegion"/>.
        /// </summary>
        /// <param name="owner">Owner of the Popup.</param>
        /// <param name="value">Name of the Popup <see cref="IRegion"/>.</param>
        public static void SetCreatePopupRegionWithName(DependencyObject owner, string value)
        {
            if (owner == null)
            {
                throw new ArgumentNullException("owner");
            }

            owner.SetValue(CreatePopupRegionWithNameProperty, value);
        }

        /// <summary>
        /// Gets the <see cref="Style"/> for the Popup.
        /// </summary>
        /// <param name="owner">Owner of the Popup.</param>
        /// <returns>The <see cref="Style"/> for the Popup.</returns>
        public static Style GetContainerWindowStyle(DependencyObject owner)
        {
            if (owner == null)
            {
                throw new ArgumentNullException("owner");
            }

            return owner.GetValue(ContainerWindowStyleProperty) as Style;
        }

        /// <summary>
        /// Sets the <see cref="Style"/> for the Popup.
        /// </summary>
        /// <param name="owner">Owner of the Popup.</param>
        /// <param name="style"><see cref="Style"/> for the Popup.</param>
        public static void SetContainerWindowStyle(DependencyObject owner, Style style)
        {
            if (owner == null)
            {
                throw new ArgumentNullException("owner");
            }

            owner.SetValue(ContainerWindowStyleProperty, style);
        }

        /// <summary>
        /// Creates a new <see cref="IRegion"/> and registers it in the default <see cref="IRegionManager"/>
        /// attaching to it a <see cref="DialogActivationBehavior"/> behaviour.
        /// </summary>
        /// <param name="owner">The owner of the Popup.</param>
        /// <param name="regionName">The name of the <see cref="IRegion"/>.</param>
        /// <remarks>
        /// This method would typically not be called directly, instead the behaviour 
        /// should be set through the Attached Property <see cref="CreatePopupRegionWithNameProperty"/>.
        /// </remarks>
        public static void RegisterNewPopupRegion(DependencyObject owner, string regionName)
        {
            // Creates a new region and registers it in the default region manager.
            // Another option if you need the complete infrastructure with the default region behaviours
            // is to extend DelayedRegionCreationBehavior overriding the CreateRegion method and create an 
            // instance of it that will be in charge of registering the Region once a RegionManager is
            // set as an attached property in the Visual Tree.
            var regionManager = ServiceLocator.Current.GetInstance<IRegionManager>();
            if (regionManager != null)
            {
                IRegion region = new SingleActiveRegion();
                DialogActivationBehavior behavior;
#if SILVERLIGHT
                behavior = new PopupDialogActivationBehavior();
#else
                behavior = new WindowDialogActivationBehavior();
#endif
                behavior.HostControl = owner;

                region.Behaviors.Add(DialogActivationBehavior.BehaviorKey, behavior);
                region.Behaviors.Add(RegionActiveAwareBehavior.BehaviorKey, new RegionActiveAwareBehavior());
                regionManager.Regions.Add(regionName, region);
            }
        }

        private static void CreatePopupRegionWithNamePropertyChanged(DependencyObject hostControl, DependencyPropertyChangedEventArgs e)
        {
            if (IsInDesignMode(hostControl))
            {
                return;
            }

            RegisterNewPopupRegion(hostControl, e.NewValue as string);
        }

        private static bool IsInDesignMode(DependencyObject element)
        {
            // Due to a known issue in Cider, GetIsInDesignMode attached property value is not enough to know if it's in design mode.
            return DesignerProperties.GetIsInDesignMode(element) || Application.Current == null
                   || Application.Current.GetType() == typeof(Application);
        }
    }
}
