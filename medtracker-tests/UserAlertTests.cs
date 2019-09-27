using NUnit.Framework;
using Moq;
using medtracker.SQL;
using medtracker.Infrastructure;

namespace Tests
{
    public class Tests
    {
        private UserAlertService userAlertService;
        private readonly string test_userId = "test_userId";
        private readonly string test_teamId = "test_teamId";

        [OneTimeSetUp]
        public void Setup()
        {
            var mockUserTimeRepository = new Mock<IUserTimesRepository>();
            mockUserTimeRepository.Setup(x => x.SetUserTime(test_userId, test_teamId, 1567580400));
            var mockSubscriberRepository = new Mock<ISubscriberRepository>();
            userAlertService = new UserAlertService(mockUserTimeRepository.Object, mockSubscriberRepository.Object);
        }

        [Test]
        public void TestValidHMMTTCommand()
        {
            var result = userAlertService.SetUserAlert("ping me at 8:30pm", test_userId, test_teamId);
            Assert.AreEqual(false, result.Error);
            Assert.AreEqual("08:30 PM", result.ResultMessage);
        }

        [Test]
        public void TestValidHHMMTTCommand()
        {
            var result = userAlertService.SetUserAlert("ping me at 08:30pm", test_userId, test_teamId);
            Assert.AreEqual(false, result.Error);
            Assert.AreEqual("08:30 PM", result.ResultMessage);
        }

        [Test]
        public void TestValidHTTCommand()
        {
            var result = userAlertService.SetUserAlert("ping me at 8pm", test_userId, test_teamId);
            Assert.AreEqual(false, result.Error);
            Assert.AreEqual("08:00 PM", result.ResultMessage);
        }

        [Test]
        public void TestValidHHMMCommand()
        {
            var result = userAlertService.SetUserAlert("ping me at 10:22", test_userId, test_teamId);
            Assert.AreEqual(false, result.Error);
            Assert.AreEqual("10:22 AM", result.ResultMessage);
        }

        [Test]
        public void TestValidHMMCommand()
        {
            var result = userAlertService.SetUserAlert("ping me at 8:22", test_userId, test_teamId);
            Assert.AreEqual(false, result.Error);
            Assert.AreEqual("08:22 AM", result.ResultMessage);
        }

        [Test]
        public void TestInvalidTimeFormat()
        {
            var result = userAlertService.SetUserAlert("ping me at 2230", test_userId, test_teamId);
            Assert.AreEqual(true, result.Error);
            Assert.AreEqual(" 2230", result.ResultMessage);
        }

        [Test]
        public void TestInvalidTime()
        {
            var result = userAlertService.SetUserAlert("ping me at 25:50", test_userId, test_teamId);
            Assert.AreEqual(true, result.Error);
            Assert.AreEqual(" 25:50", result.ResultMessage);
        }
    }
}