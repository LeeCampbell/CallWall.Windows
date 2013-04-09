using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reactive;
using Microsoft.Reactive.Testing;
using NUnit.Framework;

// ReSharper disable InconsistentNaming
namespace CallWall.Core.UnitTests
{
    public abstract class Given_an_instance_of_INotifyPropertyChanged
    {
        private Given_an_instance_of_INotifyPropertyChanged()
        {}

        private SampleDto _sut;

        [SetUp]
        public virtual void SetUp()
        {
            _sut = new SampleDto();
        }

        [TestFixture]
        public sealed class When_observing_PropertyChanges_for_property : Given_an_instance_of_INotifyPropertyChanged
        {
            [Test]
            public void Should_push_property_value_when_it_changes()
            {
                var expected = 27;

                var observer = new TestScheduler().CreateObserver<int>();
                _sut.PropertyChanges(s => s.Age).Subscribe(observer);

                _sut.Age = expected;

                Assert.AreEqual(1, observer.Messages.Count);
                Assert.AreEqual(expected, observer.Messages[0].Value.Value);
            }

            [Test]
            public void Should_push_property_value_for_multiple_changes()
            {
                var expected = new[] { 1, 2, 3, 4, 5 };

                var observer = new TestScheduler().CreateObserver<int>();
                _sut.PropertyChanges(s => s.Age).Subscribe(observer);

                foreach (var i in expected)
                {
                    _sut.Age = i;
                }

                CollectionAssert.AreEqual(expected, observer.Messages.Select(rn => rn.Value.Value));
            }

            [Test]
            public void Should_not_push_property_after_unsubscription()
            {
                var expected = 27;

                var observer = new TestScheduler().CreateObserver<int>();
                using (_sut.PropertyChanges(s => s.Age).Subscribe(observer))
                {
                    _sut.Age = expected;
                }

                //Push another value after the subscription is disposed (which is ignored)
                _sut.Age++;

                Assert.AreEqual(1, observer.Messages.Count);
                Assert.AreEqual(expected, observer.Messages[0].Value.Value);
            }

            [Test]
            public void Should_only_push_value_when_specified_property_changes()
            {
                var expected = 27;

                var observer = new TestScheduler().CreateObserver<int>();
                using (_sut.PropertyChanges(s => s.Age).Subscribe(observer))
                {
                    _sut.Age = expected;
                    _sut.FriendsCount = -1;
                }

                Assert.AreEqual(1, observer.Messages.Count);
                Assert.AreEqual(expected, observer.Messages[0].Value.Value);
            }

            [Test]
            public void Should_throw_if_instance_is_null()
            {
                _sut = null;
                var ex = Assert.Throws<ArgumentNullException>(() => _sut.PropertyChanges(s => s.Age));
                Assert.AreEqual("source", ex.ParamName);
            }
        }

        [TestFixture]
        public sealed class When_observing_PropertyChanges_for_null_property : Given_an_instance_of_INotifyPropertyChanged
        {
            [Test]
            public void Should_Throw_ArgNullException()
            {
                Expression<Func<SampleDto, int>> expression = null;
                var ex = Assert.Throws<ArgumentNullException>(() => _sut.PropertyChanges(expression));
                Assert.AreEqual("property", ex.ParamName);
            }
        }

        [TestFixture]
        public sealed class When_observing_PropertyChanges_for_a_method : Given_an_instance_of_INotifyPropertyChanged
        {
            [Test]
            public void Should_Throw_ArgException()
            {
                var ex = Assert.Throws<ArgumentException>(() => _sut.PropertyChanges(s => s.DummyMethod()));
                Assert.AreEqual("property", ex.ParamName);
                StringAssert.StartsWith("Expression is not a property", ex.Message);
            }
        }

        [TestFixture]
        public sealed class When_observing_PropertyChanges_for_a_field : Given_an_instance_of_INotifyPropertyChanged
        {
            [Test]
            public void Should_Throw_ArgException()
            {
                var ex = Assert.Throws<ArgumentException>(() => _sut.PropertyChanges(s => s.DummyField));
                Assert.AreEqual("property", ex.ParamName);
                StringAssert.StartsWith("Expression is not a property", ex.Message);
            }
        }

        [TestFixture]
        public sealed class When_observing_any_ProperyChanges : Given_an_instance_of_INotifyPropertyChanged
        {
            [Test]
            public void Should_push_source_when_any_property_changes()
            {
                var observer = new TestScheduler().CreateObserver<SampleDto>();
                _sut.AnyPropertyChanges().Subscribe(observer);

                _sut.Age = 1;
                Assert.AreEqual(1, observer.Messages[0].Value.Value.Age);
                Assert.AreEqual(null, observer.Messages[0].Value.Value.Name);
                Assert.AreEqual(0, observer.Messages[0].Value.Value.FriendsCount);

                _sut.Name = "John";
                Assert.AreEqual(1, observer.Messages[1].Value.Value.Age);
                Assert.AreEqual("John", observer.Messages[1].Value.Value.Name);
                Assert.AreEqual(0, observer.Messages[1].Value.Value.FriendsCount);

                _sut.FriendsCount = 2;
                Assert.AreEqual(1, observer.Messages[2].Value.Value.Age);
                Assert.AreEqual("John", observer.Messages[2].Value.Value.Name);
                Assert.AreEqual(2, observer.Messages[2].Value.Value.FriendsCount);
            }

            [Test]
            public void Should_not_push_property_after_unsubscription()
            {
                var observer = new TestScheduler().CreateObserver<SampleDto>();
                using (_sut.AnyPropertyChanges().Subscribe(observer))
                {
                    _sut.Age = 27;
                }

                //Push another value after the subscription is disposed (which is ignored)
                _sut.Age++;

                Assert.AreEqual(1, observer.Messages.Count);
            }

            [Test]
            public void Should_throw_if_instance_is_null()
            {
                _sut = null;
                var ex = Assert.Throws<ArgumentNullException>(() => _sut.AnyPropertyChanges());
                Assert.AreEqual("source", ex.ParamName);
            }
        }
    }



    public abstract class Given_an_ObservableCollection
    {
        private Given_an_ObservableCollection()
        {}

        private ObservableCollection<int> _sut;

        [SetUp]
        public virtual void SetUp()
        {
            _sut = new ObservableCollection<int>();
        }

        public abstract class When_observing_collectionChanges : Given_an_ObservableCollection
        {
            private IObservable<CollectionChangedData<int>> _changes;

            public override void SetUp()
            {
                base.SetUp();
                _changes = _sut.CollectionChanges();
            }

            [TestFixture]
            public sealed class When_Item_added : When_observing_collectionChanges
            {
                private int _expected;
                private ITestableObserver<CollectionChangedData<int>> _observer;
                private Recorded<Notification<CollectionChangedData<int>>> _message;
                private CollectionChangedData<int> _data;

                public override void SetUp()
                {
                    base.SetUp();
                    _observer = new TestScheduler().CreateObserver<CollectionChangedData<int>>();
                    _changes.Subscribe(_observer);
                    _expected = 27;
                    _sut.Add(_expected);
                    _message = _observer.Messages.Single();
                    _data = _message.Value.Value;
                }

                [Test]
                public void Should_push_a_notification_with_the_new_Item()
                {
                    Assert.AreEqual(_expected, _data.NewItems.Single());
                }

                [Test]
                public void Should_push_a_notification_with_no_OldItems()
                {
                    Assert.IsFalse(_data.OldItems.Any());
                }

                [Test]
                public void Should_push_a_notification_with_Add_as_the_Action()
                {
                    Assert.AreEqual(System.Collections.Specialized.NotifyCollectionChangedAction.Add, _data.Action);
                }
            }

            [TestFixture]
            public sealed class When_Item_removed : When_observing_collectionChanges
            {
                private int _expected;
                private ITestableObserver<CollectionChangedData<int>> _observer;
                private Recorded<Notification<CollectionChangedData<int>>> _message;
                private CollectionChangedData<int> _data;

                public override void SetUp()
                {
                    base.SetUp();
                    _expected = 27;
                    _sut.Add(_expected);

                    _observer = new TestScheduler().CreateObserver<CollectionChangedData<int>>();
                    _changes.Subscribe(_observer);
                    _sut.RemoveAt(0);
                    _message = _observer.Messages.Single();
                    _data = _message.Value.Value;
                }

                [Test]
                public void Should_push_a_notification_with_the_oldItem()
                {
                    Assert.AreEqual(_expected, _data.OldItems.Single());
                }

                [Test]
                public void Should_push_a_notification_with_no_NewItems()
                {
                    Assert.IsFalse(_data.NewItems.Any());
                }

                [Test]
                public void Should_push_a_notification_with_Remove_as_the_Action()
                {
                    Assert.AreEqual(System.Collections.Specialized.NotifyCollectionChangedAction.Remove, _data.Action);
                }
            }

            [TestFixture]
            public sealed class When_Item_replaced : When_observing_collectionChanges
            {
                private int _expectedOld;
                private int _expectedNew;
                private ITestableObserver<CollectionChangedData<int>> _observer;
                private Recorded<Notification<CollectionChangedData<int>>> _message;
                private CollectionChangedData<int> _data;

                public override void SetUp()
                {
                    base.SetUp();
                    _expectedOld = 27;
                    _expectedNew = 79;
                    _sut.Add(_expectedOld);

                    _observer = new TestScheduler().CreateObserver<CollectionChangedData<int>>();
                    _changes.Subscribe(_observer);
                    _sut[0] = _expectedNew;
                    _message = _observer.Messages.Single();
                    _data = _message.Value.Value;
                }

                [Test]
                public void Should_push_a_notification_with_the_oldItem()
                {
                    Assert.AreEqual(_expectedOld, _data.OldItems.Single());
                }

                [Test]
                public void Should_push_a_notification_with_the_newItem()
                {
                    Assert.AreEqual(_expectedNew, _data.NewItems.Single());
                }

                [Test]
                public void Should_push_a_notification_with_Remove_as_the_Action()
                {
                    Assert.AreEqual(System.Collections.Specialized.NotifyCollectionChangedAction.Replace, _data.Action);
                }
            }

            [TestFixture]
            public sealed class When_Collection_cleared : When_observing_collectionChanges
            {
                private IList<int> _expected;
                private ITestableObserver<CollectionChangedData<int>> _observer;
                private Recorded<Notification<CollectionChangedData<int>>> _message;
                private CollectionChangedData<int> _data;

                public override void SetUp()
                {
                    base.SetUp();
                    _expected = new[] { 27, 12, 79 };
                    foreach (var i in _expected)
                    {
                        _sut.Add(i);
                    }

                    _observer = new TestScheduler().CreateObserver<CollectionChangedData<int>>();
                    _changes.Subscribe(_observer);
                    _sut.Clear();
                    _message = _observer.Messages.Single();
                    _data = _message.Value.Value;
                }


                //No current requirement for this 
                //TODO: Potentially push the old items on a collection reset -LC
                //[Test]
                //public void Should_push_a_notification_with_the_OldItems()
                //{
                //    CollectionAssert.AreEquivalent(_expected, _data.OldItems);
                //}

                [Test]
                public void Should_push_a_notification_with_no_NewItems()
                {
                    Assert.IsFalse(_data.NewItems.Any());
                }

                [Test]
                public void Should_push_a_notification_with_Reset_as_the_Action()
                {
                    Assert.AreEqual(System.Collections.Specialized.NotifyCollectionChangedAction.Reset, _data.Action);
                }
            }

            [TestFixture]
            public sealed class When_subscription_disposed : When_observing_collectionChanges
            {
                [Test]
                public void Should_not_push_value_on_item_add()
                {
                    var observer = new TestScheduler().CreateObserver<CollectionChangedData<int>>();
                    using(_changes.Subscribe(observer))
                    {
                        _sut.Add(1);
                        Assume.That(observer.Messages.Count == 1);
                    }
                    _sut.Add(2);
                    Assert.AreEqual(1, observer.Messages.Count);
                }

                [Test]
                public void Should_not_push_value_on_item_remove()
                {
                    var observer = new TestScheduler().CreateObserver<CollectionChangedData<int>>();
                    using (_changes.Subscribe(observer))
                    {
                        _sut.Add(1);
                        Assume.That(observer.Messages.Count == 1);
                    }
                    _sut.RemoveAt(0);
                    Assert.AreEqual(1, observer.Messages.Count);
                }

                [Test]
                public void Should_not_push_value_on_clear()
                {
                    var observer = new TestScheduler().CreateObserver<CollectionChangedData<int>>();
                    using (_changes.Subscribe(observer))
                    {
                        _sut.Add(1);
                        Assume.That(observer.Messages.Count == 1);
                    }
                    _sut.Clear();
                    Assert.AreEqual(1, observer.Messages.Count);
                }

                [Test]
                public void Should_not_push_value_on_replace()
                {
                    var observer = new TestScheduler().CreateObserver<CollectionChangedData<int>>();
                    using (_changes.Subscribe(observer))
                    {
                        _sut.Add(1);
                        Assume.That(observer.Messages.Count == 1);
                    }
                    _sut[0] = 2;
                    Assert.AreEqual(1, observer.Messages.Count);
                }
            }
        }
    }

    public abstract class Given_an_ObservableCollection_of_INotifyPropertyChanged_items
    {
        private Given_an_ObservableCollection_of_INotifyPropertyChanged_items()
        {}

        private ObservableCollection<SampleDto> _sut;

        [SetUp]
        public virtual void SetUp()
        {
            _sut = new ObservableCollection<SampleDto>();
        }
        public abstract class When_observing_CollectionItemsChange : Given_an_ObservableCollection_of_INotifyPropertyChanged_items
        {
            private IObservable<CollectionChangedData<SampleDto>> _changes;

            public override void SetUp()
            {
                base.SetUp();
                _changes = _sut.CollectionItemsChange();
            }

            private static SampleDto CreateJohn()
            {
                return new SampleDto { Name = "John", Age = 27, FriendsCount = 5 };
            }
            private static SampleDto CreateJack()
            {
                return new SampleDto { Name = "Jack", Age = 21, FriendsCount = 30 };
            }

            [TestFixture]
            public sealed class When_item_added : When_observing_CollectionItemsChange
            {
                private SampleDto _expected;
                private ITestableObserver<CollectionChangedData<SampleDto>> _observer;
                private Recorded<Notification<CollectionChangedData<SampleDto>>> _message;
                private CollectionChangedData<SampleDto> _data;

                public override void SetUp()
                {
                    base.SetUp();
                    _observer = new TestScheduler().CreateObserver<CollectionChangedData<SampleDto>>();
                    _changes.Subscribe(_observer);
                    _expected = CreateJohn();
                    _sut.Add(_expected);
                    _message = _observer.Messages.Single();
                    _data = _message.Value.Value;
                }

                

                [Test]
                public void Should_push_a_notification_with_the_new_Item()
                {
                    Assert.AreEqual(_expected, _data.NewItems.Single());
                }

                [Test]
                public void Should_push_a_notification_with_no_OldItems()
                {
                    Assert.IsFalse(_data.OldItems.Any());
                }

                [Test]
                public void Should_push_a_notification_with_Add_as_the_Action()
                {
                    Assert.AreEqual(System.Collections.Specialized.NotifyCollectionChangedAction.Add, _data.Action);
                }
            }

            [TestFixture]
            public sealed class When_Item_removed : When_observing_CollectionItemsChange
            {
                private SampleDto _expected;
                private ITestableObserver<CollectionChangedData<SampleDto>> _observer;
                private Recorded<Notification<CollectionChangedData<SampleDto>>> _message;
                private CollectionChangedData<SampleDto> _data;

                public override void SetUp()
                {
                    base.SetUp();
                    _expected = CreateJohn();
                    _sut.Add(_expected);

                    _observer = new TestScheduler().CreateObserver<CollectionChangedData<SampleDto>>();
                    _changes.Subscribe(_observer);
                    _sut.RemoveAt(0);
                    _message = _observer.Messages.Single();
                    _data = _message.Value.Value;
                }

                [Test]
                public void Should_push_a_notification_with_the_oldItem()
                {
                    Assert.AreEqual(_expected, _data.OldItems.Single());
                }

                [Test]
                public void Should_push_a_notification_with_no_NewItems()
                {
                    Assert.IsFalse(_data.NewItems.Any());
                }

                [Test]
                public void Should_push_a_notification_with_Remove_as_the_Action()
                {
                    Assert.AreEqual(System.Collections.Specialized.NotifyCollectionChangedAction.Remove, _data.Action);
                }
            }

            [TestFixture]
            public sealed class When_Item_replaced : When_observing_CollectionItemsChange
            {
                private SampleDto _expectedOld;
                private SampleDto _expectedNew;
                private ITestableObserver<CollectionChangedData<SampleDto>> _observer;
                private Recorded<Notification<CollectionChangedData<SampleDto>>> _message;
                private CollectionChangedData<SampleDto> _data;

                public override void SetUp()
                {
                    base.SetUp();
                    _expectedOld = CreateJohn();
                    _expectedNew = CreateJack();
                    _sut.Add(_expectedOld);

                    _observer = new TestScheduler().CreateObserver<CollectionChangedData<SampleDto>>();
                    _changes.Subscribe(_observer);
                    _sut[0] = _expectedNew;
                    _message = _observer.Messages.Single();
                    _data = _message.Value.Value;
                }

                [Test]
                public void Should_push_a_notification_with_the_oldItem()
                {
                    Assert.AreEqual(_expectedOld, _data.OldItems.Single());
                }

                [Test]
                public void Should_push_a_notification_with_the_newItem()
                {
                    Assert.AreEqual(_expectedNew, _data.NewItems.Single());
                }

                [Test]
                public void Should_push_a_notification_with_Remove_as_the_Action()
                {
                    Assert.AreEqual(System.Collections.Specialized.NotifyCollectionChangedAction.Replace, _data.Action);
                }
            }

            [TestFixture]
            public sealed class When_Collection_cleared : When_observing_CollectionItemsChange
            {
                private IList<SampleDto> _expected;
                private ITestableObserver<CollectionChangedData<SampleDto>> _observer;
                private Recorded<Notification<CollectionChangedData<SampleDto>>> _message;
                private CollectionChangedData<SampleDto> _data;

                public override void SetUp()
                {
                    base.SetUp();
                    _expected = new[] { CreateJohn(), CreateJack(), new SampleDto(){Name="Jill", Age=19, FriendsCount = 30} };
                    foreach (var i in _expected)
                    {
                        _sut.Add(i);
                    }

                    _observer = new TestScheduler().CreateObserver<CollectionChangedData<SampleDto>>();
                    _changes.Subscribe(_observer);
                    _sut.Clear();
                    _message = _observer.Messages.Single();
                    _data = _message.Value.Value;
                }


                //No current requirement for this 
                //TODO: Potentially push the old items on a collection reset -LC
                //[Test]
                //public void Should_push_a_notification_with_the_OldItems()
                //{
                //    CollectionAssert.AreEquivalent(_expected, _data.OldItems);
                //}

                [Test]
                public void Should_push_a_notification_with_no_NewItems()
                {
                    Assert.IsFalse(_data.NewItems.Any());
                }

                [Test]
                public void Should_push_a_notification_with_Reset_as_the_Action()
                {
                    Assert.AreEqual(System.Collections.Specialized.NotifyCollectionChangedAction.Reset, _data.Action);
                }
            }


            [TestFixture]
            public sealed class When_item_value_changes : When_observing_CollectionItemsChange
            {
                private SampleDto _expected;
                private const int _expectedAge = 49;
                private ITestableObserver<CollectionChangedData<SampleDto>> _observer;
                private Recorded<Notification<CollectionChangedData<SampleDto>>> _message;
                private CollectionChangedData<SampleDto> _data;
                private IDisposable _subscription;

                public override void SetUp()
                {
                    base.SetUp();
                    _expected = CreateJohn();
                    _sut.Add(_expected);

                    _observer = new TestScheduler().CreateObserver<CollectionChangedData<SampleDto>>();
                    _subscription = _changes.Subscribe(_observer);
                    Assume.That(_sut[0].Age != _expectedAge);
                    _sut[0].Age = _expectedAge;
                    _message = _observer.Messages.Single();
                    _data = _message.Value.Value;
                }

                [Test]
                public void Should_push_a_notification_with_the_changed_item_as_a_NewItem()
                {
                    var actual = _data.NewItems.Single();
                    Assert.AreEqual(_expected, actual);
                    Assert.AreEqual(_expectedAge, actual.Age);
                }


                [Test]
                public void Should_push_a_notification_with_no_OldItems()
                {
                    Assert.IsFalse(_data.OldItems.Any());
                }

                [Test]
                public void Should_push_a_notification_with_Replace_as_the_Action()
                {
                    Assert.AreEqual(System.Collections.Specialized.NotifyCollectionChangedAction.Replace, _data.Action);
                }
                
                [Test]
                public void Should_not_notify_when_subscription_is_disposed()
                {
                    Assume.That(_observer.Messages.Count == 1);
                    Assume.That(_sut[0].Age != -1);
                    _subscription.Dispose();
                    _sut[0].Age = -1;

                    Assert.AreEqual(1, _observer.Messages.Count);
                }

                [Test]
                public void Should_not_notify_when_item_was_removed()
                {
                    var removedItem = _sut[0];
                    _sut.Remove(removedItem);
                    Assume.That(_observer.Messages.Count == 2); //updated, then removed.
                    Assume.That(removedItem.Age != -1);

                    removedItem.Age = -1;

                    Assert.AreEqual(2, _observer.Messages.Count);
                }

            }

        }

        //public abstract class When_observing_ItemsPropertyChange : Given_an_ObservableCollection_of_INotifyPropertyChanged_items
        //{
        //    private IObservable<CollectionChangedData<SampleDto>> _changes;

        //    public override void SetUp()
        //    {
        //        base.SetUp();
        //        _sut.ItemsPropertyChange(item=>item.Age)
        //    }
        //}

    }

    /*
    
    When Item replaced
      Should push a notification with old value in Old items
                                      new value in New items
                                      Replace as the Action
        }
    }
    */
    public class SampleDto : INotifyPropertyChanged
    {
        private string _name;
        private int _age;
        private int _friendsCount;

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged("Name");
            }
        }

        public int Age
        {
            get { return _age; }
            set
            {
                _age = value;
                OnPropertyChanged("Age");
            }
        }

        public int FriendsCount
        {
            get { return _friendsCount; }
            set
            {
                _friendsCount = value;
                OnPropertyChanged("FriendsCount");
            }
        }

        public int DummyField;
        public int DummyMethod()
        {
            return 0;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
// ReSharper restore InconsistentNaming