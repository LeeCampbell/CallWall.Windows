using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using Microsoft.Practices.Prism.Regions;
using Moq;

namespace CallWall.Windows.Testing
{
    public sealed class RegionManagerStub : IRegionManager
    {
        #region Implementation of IRegionManager

        private IRegionCollection _regions = new RegionCollectionStub();

        public IRegionManager CreateRegionManager()
        {
            return new RegionManagerStub();
        }

        public IRegionCollection Regions
        {
            get { return _regions; }
        }

        #endregion

        public Mock<IRegion> CreateAndAddMock(string regionName)
        {
            var region = new Mock<IRegion>();
            region.SetupGet(r => r.Name).Returns(regionName);
            Regions.Add(region.Object);
            return region;
        }

        private sealed class RegionCollectionStub : IRegionCollection
        {
            private readonly ObservableCollection<IRegion> _regions = new ObservableCollection<IRegion>();

            #region Implementation of IEnumerable

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

            #endregion

            #region Implementation of IEnumerable<out IRegion>

            public IEnumerator<IRegion> GetEnumerator()
            {
                return _regions.GetEnumerator();
            }

            #endregion

            #region Implementation of INotifyCollectionChanged

            public event NotifyCollectionChangedEventHandler CollectionChanged
            {
                add { _regions.CollectionChanged += value; }
                remove { _regions.CollectionChanged -= value; }
            }

            #endregion

            #region Implementation of IRegionCollection

            public void Add(IRegion region)
            {
                _regions.Add(region);
            }

            public bool Remove(string regionName)
            {
                var region = _regions.FirstOrDefault(r => r.Name == regionName);
                return region != null && _regions.Remove(region);
            }

            public bool ContainsRegionWithName(string regionName)
            {
                return _regions.Any(r => r.Name == regionName);
            }

            public IRegion this[string regionName]
            {
                get { return _regions.FirstOrDefault(r => r.Name == regionName); }
            }

            #endregion
        }
    }
}
