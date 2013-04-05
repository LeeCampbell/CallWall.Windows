using System;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.Reactive.Testing;
using NUnit.Framework;

// ReSharper disable InconsistentNaming
namespace CallWall.Core.UnitTests
{
    public abstract class Given_an_instance_of_INotifyPropertyChanged
    {
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
                var expected = new[]{1,2,3,4,5};

                var observer = new TestScheduler().CreateObserver<int>();
                _sut.PropertyChanges(s => s.Age).Subscribe(observer);

                foreach (var i in expected)
                {
                    _sut.Age = i;    
                }
                
                CollectionAssert.AreEqual(expected, observer.Messages.Select(rn=>rn.Value.Value));
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
                var ex = Assert.Throws<ArgumentNullException>(()=> _sut.PropertyChanges(expression));
                Assert.AreEqual("property", ex.ParamName);
            }
        }

        [TestFixture]
        public sealed class When_observing_PropertyChanges_for_a_method : Given_an_instance_of_INotifyPropertyChanged
        {
            [Test]
            public void Should_Throw_ArgException()
            {
                var ex = Assert.Throws<ArgumentException>(() => _sut.PropertyChanges(s=>s.DummyMethod()));
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
                var ex = Assert.Throws<ArgumentNullException>(()=> _sut.AnyPropertyChanges());
                Assert.AreEqual("source", ex.ParamName);
            }
        }
    }

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