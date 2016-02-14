using System;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

using Sunlight.Model;

namespace Sunlight.UnitTests
{
    [TestClass]
    public class PagingTests
    {
        [TestMethod]
        public async Task GetTwoPages()
        {
            var Keys = new Keys();
            string key = Keys.Data.Sunlight;
            var congress = new Congress(key);

            var page1 = await congress.GetHearings();

            var page2 = await congress.GetNextPage(page1);
            Assert.IsNotNull(page2);
            Assert.AreEqual(2, (int)page2.page.page);
        }

        [TestMethod]
        public async Task GetTwoPagesWithArgs()
        {
            var Keys = new Keys();
            string key = Keys.Data.Sunlight;
            var congress = new Congress(key);

            var page1 = await congress.GetHearings("senate");

            var page2 = await congress.GetNextPage(page1);
            Assert.IsNotNull(page2);
            Assert.AreEqual(2, (int)page2.page.page);
        }
    }
}
