using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using YoutubeGuesserApi.Library.Youtube;
using YoutubeGuesserApi.Model;

namespace YoutubeGuesserApi.Tests
{
    [TestClass]
    public class YoutubeApiTests
    {
        private static YoutubeApi youtubeApi;

        [ClassInitialize]
        public static void TestInitialize(TestContext context)
        {
            youtubeApi = new YoutubeApi();
        }

        [TestMethod]
        public async Task Assert_GetRequest_is_correct()
        {
            string url = "https://google.com";
            YoutubeResponse response = await youtubeApi.GetRequest(url);
            Assert.IsNotNull(response.Content);
        }
    }
}