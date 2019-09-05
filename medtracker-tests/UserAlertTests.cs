using NUnit.Framework;
using Moq;
using medtracker.SQL;
using medtracker.Infrastructure;

namespace Tests
{
    public class Tests
    {
        [Test]
        public void TestValidHMMTTCommand()
        {
            var mockUserTimeRepository = new Mock<IUserTimesRepository>();
            mockUserTimeRepository.Setup(x => x.SetUserTime("test_userID", "test_teamID", 1567580400));

            var userAlert = new UserAlertService(mockUserTimeRepository.Object);
            var result = userAlert.SetUserAlert("ping me at 8:30pm", "test_userID", "test_teamID");
            Assert.AreEqual(false, result.Error);
            Assert.AreEqual("08:30 PM", result.ResultMessage);
        }

        [Test]
        public void TestValidHHMMTTCommand()
        {
            var mockUserTimeRepository = new Mock<IUserTimesRepository>();
            mockUserTimeRepository.Setup(x => x.SetUserTime("test_userID", "test_teamID", 1567580400));

            var userAlert = new UserAlertService(mockUserTimeRepository.Object);
            var result = userAlert.SetUserAlert("ping me at 08:30pm", "test_userID", "test_teamID");
            Assert.AreEqual(false, result.Error);
            Assert.AreEqual("08:30 PM", result.ResultMessage);
        }

        [Test]
        public void TestValidHTTCommand()
        {
            var mockUserTimeRepository = new Mock<IUserTimesRepository>();
            mockUserTimeRepository.Setup(x => x.SetUserTime("test_userID", "test_teamID", 1567580400));

            var userAlert = new UserAlertService(mockUserTimeRepository.Object);
            var result = userAlert.SetUserAlert("ping me at 8pm", "test_userID", "test_teamID");
            Assert.AreEqual(false, result.Error);
            Assert.AreEqual("08:00 PM", result.ResultMessage);
        }

        [Test]
        public void TestValidHHMMCommand()
        {
            var mockUserTimeRepository = new Mock<IUserTimesRepository>();
            mockUserTimeRepository.Setup(x => x.SetUserTime("test_userID", "test_teamID", 1567580400));

            var userAlert = new UserAlertService(mockUserTimeRepository.Object);
            var result = userAlert.SetUserAlert("ping me at 10:22", "test_userID", "test_teamID");
            Assert.AreEqual(false, result.Error);
            Assert.AreEqual("10:22 AM", result.ResultMessage);
        }

        [Test]
        public void TestValidHMMCommand()
        {
            var mockUserTimeRepository = new Mock<IUserTimesRepository>();
            mockUserTimeRepository.Setup(x => x.SetUserTime("test_userID", "test_teamID", 1567580400));

            var userAlert = new UserAlertService(mockUserTimeRepository.Object);
            var result = userAlert.SetUserAlert("ping me at 8:22", "test_userID", "test_teamID");
            Assert.AreEqual(false, result.Error);
            Assert.AreEqual("08:22 AM", result.ResultMessage);
        }

        [Test]
        public void TestInvalidTimeFormat()
        {
            var mockUserTimeRepository = new Mock<IUserTimesRepository>();
            mockUserTimeRepository.Setup(x => x.SetUserTime("test_userID", "test_teamID", 1567580400));

            var userAlert = new UserAlertService(mockUserTimeRepository.Object);
            var result = userAlert.SetUserAlert("ping me at 2230", "test_userID", "test_teamID");
            Assert.AreEqual(true, result.Error);
            Assert.AreEqual(" 2230", result.ResultMessage);
        }

        [Test]
        public void TestInvalidTime()
        {
            var mockUserTimeRepository = new Mock<IUserTimesRepository>();
            mockUserTimeRepository.Setup(x => x.SetUserTime("test_userID", "test_teamID", 1567580400));

            var userAlert = new UserAlertService(mockUserTimeRepository.Object);
            var result = userAlert.SetUserAlert("ping me at 25:50", "test_userID", "test_teamID");
            Assert.AreEqual(true, result.Error);
            Assert.AreEqual(" 25:50", result.ResultMessage);
        }
    }
}