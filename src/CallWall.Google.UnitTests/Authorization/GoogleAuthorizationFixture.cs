using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

// ReSharper disable InconsistentNaming
namespace CallWall.Google.UnitTests.Authorization
{
    public abstract class Given_a_newly_constructed_GoogleAuthorization
    {
        private Given_a_newly_constructed_GoogleAuthorization()
        { }

        [TestFixture]
        public sealed class When_requesting_AccessToken : Given_a_newly_constructed_GoogleAuthorization
        {
            //Should call the registereed call back
            //Should validate that there are some resources enabled.
            //  when multiple concurrent requests for access token are made
        }

        
    }

    //Given an Authorized_GoogleAuthoization
    //  When requesting access token
    //  when requesting access token for modified resources
    //  when multiple concurrent requests for access token are made
    //Given a lapsed GoogleAuthoization
    
}
// ReSharper restore InconsistentNaming