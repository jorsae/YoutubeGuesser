using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using YoutubeGuesserApi.Controllers;
using YoutubeGuesserApi.DataAccess;
using YoutubeGuesserApi.Library.Utility;
using YoutubeGuesserApi.Model;

namespace YoutubeGuesserApi.Tests
{
    [TestClass]
    public class HarvestsControllerTests
    {
        private static DatabaseContext _context;
        private static HarvestsController harvestsController;
        private static UserData userData;
        private static Dictionary<string, Microsoft.Extensions.Primitives.StringValues> collectionValues;
        private static FormCollection collection;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            _context = new DatabaseContext("Server=(localdb)\\mssqllocaldb;Database=YoutubeGuesserApiDB_Tests;Trusted_Connection=True;MultipleActiveResultSets=true");
            harvestsController = new HarvestsController(_context);
            userData = new UserData("browser", "OS", "123123.23.2", "location", "language", "referrer", true);
            collectionValues = new Dictionary<string, Microsoft.Extensions.Primitives.StringValues>
            {
                { "password", Constants.ADMIN_API_PASSWORD }
            };
            collection = new FormCollection(collectionValues);
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _context.UserData.Add(new UserData("Chrome", "Windows 10", "1239123.", "Australia", "language", "referrer", true));
            _context.UserData.Add(new UserData("Firefox", "Linux", "1239123.", "Australia", "language", "referrer", false));
            _context.UserData.Add(new UserData("Chrome", "Windows 10", "1239123.", "Switzerland", "language", "referrer", true));
            _context.UserData.Add(new UserData("Chrome", "OS X", "1239123.", "Australia", "language", "referrer", true));
            _context.UserData.Add(new UserData("Chrome", "Windows 10", "1239123.", "Switzerland", "language", "referrer", false));
            _context.UserData.Add(new UserData(null, null, null, null, null, null, false));
            _context.SaveChanges();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            collectionValues.Remove("type");
            _context.UserData.RemoveRange(_context.UserData);
            _context.SaveChanges();
        }

        [TestMethod]
        public async Task Assert_PostUserData_is_correct()
        {
            StatusCodeResult result = (StatusCodeResult) await harvestsController.PostUserDataAsync(userData);
            Assert.AreEqual(200, result.StatusCode);
        }

        [TestMethod]
        public async Task Assert_PostUserData_userdata_is_null()
        {
            StatusCodeResult result = (StatusCodeResult) await harvestsController.PostUserDataAsync(null);
            Assert.AreEqual(400, result.StatusCode);
        }

        [TestMethod]
        public async Task Assert_PostUserData_userdata_manually_set_id()
        {
            UserData userData = new UserData("browser", "OS", "123123.23.2", "location", "language", "referrer", true)
            {
                Id = 9001
            };
            StatusCodeResult result = (StatusCodeResult) await harvestsController.PostUserDataAsync(userData);
            Assert.AreEqual(400, result.StatusCode);
        }

        [TestMethod]
        public void Assert_GetDevices_is_correct()
        {
            ActionResult actionResult = harvestsController.GetDevices(collection);
            var okResult = actionResult as OkObjectResult;
            Dictionary<string, int> devices = okResult.Value as Dictionary<string, int>;
            Assert.AreEqual(3, devices["pc"]);
            Assert.AreEqual(3, devices["mobile"]);
            Assert.AreEqual(6, devices["total"]);
        }

        [TestMethod]
        public void Assert_GetDevices_is_0()
        {
            _context.UserData.RemoveRange(_context.UserData);
            _context.SaveChanges();

            ActionResult actionResult = harvestsController.GetDevices(collection);
            var okResult = actionResult as OkObjectResult;
            Dictionary<string, int> devices = okResult.Value as Dictionary<string, int>;
            Assert.AreEqual(0, devices["pc"]);
            Assert.AreEqual(0, devices["mobile"]);
            Assert.AreEqual(0, devices["total"]);
        }

        [TestMethod]
        public void Assert_GetDevices_wrong_password()
        {
            Dictionary<string, Microsoft.Extensions.Primitives.StringValues> collectionValues =
                        new Dictionary<string, Microsoft.Extensions.Primitives.StringValues>
            {
                { "password", "notthepassword" }
            };
            var collection = new FormCollection(collectionValues);
            StatusCodeResult result = (StatusCodeResult)harvestsController.GetDevices(collection);
            Assert.AreEqual(401, result.StatusCode);
        }

        [TestMethod]
        public void Assert_GetDevices_null_password()
        {
            var collection = new FormCollection(null);
            StatusCodeResult result = (StatusCodeResult)harvestsController.GetDevices(collection);
            Assert.AreEqual(401, result.StatusCode);
        }

        [TestMethod]
        public void Assert_GetDevices_null_FormCollection()
        {
            StatusCodeResult result = (StatusCodeResult)harvestsController.GetDevices(null);
            Assert.AreEqual(400, result.StatusCode);
        }

        [TestMethod]
        public void Assert_GetDistinctType_browser_is_correct()
        {
            collectionValues.Add("type", "browser");
            var collection = new FormCollection(collectionValues);

            ActionResult actionResult = harvestsController.GetDistinctType(collection);
            var okResult = actionResult as OkObjectResult;
            Dictionary<string, int> distinctBrowser = okResult.Value as Dictionary<string, int>;

            Assert.AreEqual(1, distinctBrowser["Firefox"]);
            Assert.AreEqual(4, distinctBrowser["Chrome"]);
            Assert.AreEqual(1, distinctBrowser["Unknown"]);
        }

        [TestMethod]
        public void Assert_GetDistinctType_os_is_correct()
        {
            collectionValues.Add("type", "os");
            var collection = new FormCollection(collectionValues);

            ActionResult actionResult = harvestsController.GetDistinctType(collection);
            var okResult = actionResult as OkObjectResult;
            Dictionary<string, int> distinctOS = okResult.Value as Dictionary<string, int>;

            Assert.AreEqual(3, distinctOS["Windows 10"]);
            Assert.AreEqual(1, distinctOS["Linux"]);
            Assert.AreEqual(1, distinctOS["OS X"]);
            Assert.AreEqual(1, distinctOS["Unknown"]);
        }

        [TestMethod]
        public void Assert_GetDistinctType_location_is_correct()
        {
            collectionValues.Add("type", "location");
            var collection = new FormCollection(collectionValues);

            ActionResult actionResult = harvestsController.GetDistinctType(collection);
            var okResult = actionResult as OkObjectResult;
            Dictionary<string, int> distinctLocation= okResult.Value as Dictionary<string, int>;

            Assert.AreEqual(3, distinctLocation["Australia"]);
            Assert.AreEqual(2, distinctLocation["Switzerland"]);
            Assert.AreEqual(1, distinctLocation["Unknown"]);
        }

        [TestMethod]
        public void Assert_GetDistinctType_invalid_password()
        {
            Dictionary<string, Microsoft.Extensions.Primitives.StringValues> collectionValues =
                        new Dictionary<string, Microsoft.Extensions.Primitives.StringValues>
            {
                { "password", "notthepassword" }
            };
            var collection = new FormCollection(collectionValues);
            StatusCodeResult result = (StatusCodeResult)harvestsController.GetDistinctType(collection);
            Assert.AreEqual(401, result.StatusCode);
        }

        [TestMethod]
        public void Assert_GetDistinctType_invalid_type()
        {
            collectionValues.Add("type", "invalid type;:_-,-.");
            var collection = new FormCollection(collectionValues);
            StatusCodeResult result = (StatusCodeResult)harvestsController.GetDistinctType(collection);
            Assert.AreEqual(400, result.StatusCode);
        }

        [TestMethod]
        public void Assert_GetDistinctType_null_FormCollection()
        {
            StatusCodeResult result = (StatusCodeResult)harvestsController.GetDistinctType(null);
            Assert.AreEqual(400, result.StatusCode);
        }
    }
}