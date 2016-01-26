using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunlight.Model
{
    public interface IAbout
    {
        string Name { get; }
        string Publisher { get; }
        string PackageFamilyName { get; }
        string ProductId { get; }
        string Version { get; }
        Uri PrivacyStatement { get; }
        Uri TermsOfUse { get; }
        IEnumerable<Link> Links { get; }
    }
}
