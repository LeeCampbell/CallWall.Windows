using Microsoft.Practices.Prism.Regions;
using System;
using System.Windows.Controls;


//Ported from the SelectorRegionAdapter.
//Accordion is not CLS compliant which raised Static Analysis errors. Making this 'internal' resolves the errors.
namespace CallWall.PrismExtensions
{
    /// <summary>
    /// Adapter that creates a new <see cref="T:Microsoft.Practices.Prism.Regions.Region"/> and binds all
    ///             the views to the adapted <see cref="T:System.Windows.Controls.Accordion"/>.
    ///             It also keeps the <see cref="P:Microsoft.Practices.Prism.Regions.IRegion.ActiveViews"/> and the selected items
    ///             of the <see cref="T:System.Windows.Controls.Accordion"/> in sync.
    /// 
    /// </summary>
    internal class AccordionRegionAdapter : RegionAdapterBase<Accordion>
    {
        /// <summary>
        /// Initializes a new instance of <see cref="T:CallWall.PrismExtensions.AccordionRegionAdapter"/>.
        /// </summary>
        /// <param name="regionBehaviorFactory">The factory used to create the region behaviours to attach to the created regions.</param>
        public AccordionRegionAdapter(IRegionBehaviorFactory regionBehaviorFactory)
            : base(regionBehaviorFactory)
        {
        }

        /// <summary>
        /// Adapts an <see cref="T:System.Windows.Controls.Accordion"/> to an <see cref="T:Microsoft.Practices.Prism.Regions.IRegion"/>.
        /// </summary>
        /// <param name="region">The new region being used.</param>
        /// <param name="regionTarget">The object to adapt.</param>
        protected override void Adapt(IRegion region, Accordion regionTarget)
        {
        }

        /// <summary>
        /// Attach new behaviours.
        /// </summary>
        /// <param name="region">The region being used.</param>
        /// <param name="regionTarget">The object to adapt.</param>
        /// <remarks>
        /// This class attaches the base behaviours and also listens for changes in the
        /// activity of the region or the control selection and keeps the in sync.
        /// </remarks>
        protected override void AttachBehaviors(IRegion region, Accordion regionTarget)
        {
            if (region == null)
                throw new ArgumentNullException("region");
            var accordionItemsSourceSyncBehavior = new AccordionItemsSourceSyncBehavior { HostControl = regionTarget };
            region.Behaviors.Add(AccordionItemsSourceSyncBehavior.BehaviorKey, accordionItemsSourceSyncBehavior);
            base.AttachBehaviors(region, regionTarget);
        }

        /// <summary>
        /// Creates a new instance of <see cref="T:Microsoft.Practices.Prism.Regions.Region"/>.
        /// </summary>
        /// <returns>
        /// A new instance of <see cref="T:Microsoft.Practices.Prism.Regions.Region"/>.
        /// </returns>
        protected override IRegion CreateRegion()
        {
            return new Region();
        }
    }
}