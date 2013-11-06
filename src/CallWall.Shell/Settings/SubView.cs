using System;
using Microsoft.Practices.Prism.Commands;

namespace CallWall.Settings
{
    public sealed class SubView
    {
        private readonly string _name;
        private readonly Uri _image;
        private readonly DelegateCommand _open;

        public SubView(string name, Uri image, Action openView)
        {
            _name = name;
            _image = image;
            _open = new DelegateCommand(openView);
        }

        public string Name
        {
            get { return _name; }
        }

        public Uri Image
        {
            get { return _image; }
        }

        public DelegateCommand Open
        {
            get { return _open; }
        }
    }
}