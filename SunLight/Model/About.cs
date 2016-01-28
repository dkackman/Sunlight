using System;
using System.Collections.Generic;

using Windows.ApplicationModel;

namespace Sunlight.Model
{
    sealed class About : IAbout
    {
        public About()
        {
            Links = new List<Link>()
            {
                new Link() { Text = "Help", Target = new Uri("https://github.com/dkackman", UriKind.Absolute) }
            };

            Credits = new List<Credit>()
            {
                new Credit()
                {
                    Subject = "Legislative data provided by",
                    ImageUri = new Uri("ms-appx:///Assets/sflogo.png")
                }
            };
        }

        public string Name => Package.Current.DisplayName;

        public string Publisher => Package.Current.PublisherDisplayName;

        public Uri PrivacyStatement => new Uri("https://github.com/dkackman/Sunlight.Net/blob/master/Privacy.md");

        public Uri TermsOfUse => null;

        public string Version
        {
            get
            {
                var id = Package.Current.Id;
                return $"Version {id.Version.Major}.{id.Version.Minor}.{id.Version.Build}.{id.Version.Revision}";
            }
        }

        public string PackageFamilyName => Package.Current.Id.FamilyName;

        public string ProductId => Package.Current.Id.ProductId;

        public IEnumerable<Link> Links { get; private set; }

        public IEnumerable<Credit> Credits { get; private set; }
    }
}