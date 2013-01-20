using System;
using CallWall.Contract;
using CallWall.Contract.Contact;
using CallWall.ProfileDashboard.Communication;

namespace CallWall.ProfileDashboard
{

    //When Loaded/Connected, it will listen for Activation of a (single?) profile.
    //  when activated, will query for data for the profile and update it's state
    //  should be able to either close (if only deals with single profile) or clear out if it does multiple.

    //Leaning towards single profile instance (take(1)).

    public interface IProfileDashboard : IDisposable
    {
        void Load(IProfile profile);

        //Status Status { get; }
        IObservable<IContactProfile> Contact { get; }

        IObservable<MessageViewModel> Messages { get; }
    }
}