// Author: Dakota Methvin
using NUnit.Framework;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using DSharpPlus;
using TheGodfather.Modules.R6Siege;

namespace TheGodfatherTests.Modules.R6Siege
{
    [TestFixture]
    public class R6SiegeStatsModuleTest
    {
        private static R6SiegeStatsModule testObject = new R6SiegeStatsModule();

        
        [TestMethod]
        public async Task GeneralStatsTest()
        {
            // Set up test object
            CommandContext ctx = new CommandContext();
            var result = await testObject.GeneralStats(ctx, "heyDK");
            // Check for a value
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task RankedStatsTest()
        {
            // Set up test object
            CommandContext ctx = new CommandContext();
            var result = await testObject.RankedStats(ctx, "heyDK", "whitenoise");
            // Check for a value
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void GeneralStatsToDiscordEmbedTest()
        {
            // Set up test object
            DiscordEmbed testEmbed = testObject.GeneralStatsToDiscordEmbed(6, 7, 8, 9, 0, 1, "some string", "https://bit.ly/2AhQIbV", 40, 41, 43);
            // Check the parameters
            Assert.AreEqual("https://bit.ly/2AhQIbV", testObject.ThumbnailUrl);
            // Check the generation
            Assert.AreEqual(DiscordColor.HotPink, testObject.Color);
        }

        [TestMethod]
        public void RankedStatsToDiscordEmbedTest()
        {
            // Set up test object
            DiscordEmbed testEmbed = testObject.RankedStatsToDiscordEmbed(4.3, 3.5, 6, 7, 8, "string1", "test", "last string", "https://bit.ly/2AhQIbV");
            // Check the parameters
            Assert.AreEqual("last string stats", testEmbed.Description);
            // Check the generation
            Assert.AreEqual(DiscordColor.HotPink, testObject.Color);
        }

        [TestMethod]
        public void GetSeasonIDTest()
        {
            // Check valid string
            Assert.AreEqual(8, testObject.GetSeasonID("whitenoise"));
            // Check invalid string
            Assert.AreEqual(-1, testObject.GetSeasonID("badstring"));
            
        }
    }
}
