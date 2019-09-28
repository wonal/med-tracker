using NUnit.Framework;
using Moq;
using medtracker.SQL;
using medtracker.Infrastructure;
using medtracker.DTOs;

namespace Tests
{
    public class UserRecordTests
    {
        [Test]
        public void TestValidData()
        {
            var mockDataRepository = new Mock<IUserDataRepository>();
            mockDataRepository.Setup(x => x.SetData(new DataDTO { userID = "testUser", teamID = "testTeam", date = 1567580400, ha_present = false, num_maxalt = 0, num_aleve = 0 }));

            var userRecordService = new UserRecordService(mockDataRepository.Object);
            var result = userRecordService.StoreRecord("record no 0 0", "testUser", "testTeam");
            Assert.AreEqual(false, result.Error);
        }

        [Test]
        public void TestValidDataCaseSensitive()
        {
            var mockDataRepository = new Mock<IUserDataRepository>();
            mockDataRepository.Setup(x => x.SetData(new DataDTO { userID = "testUser", teamID = "testTeam", date = 1567580400, ha_present = false, num_maxalt = 0, num_aleve = 0 }));

            var userRecordService = new UserRecordService(mockDataRepository.Object);
            var result = userRecordService.StoreRecord("Record No 0 0", "testUser", "testTeam");
            Assert.AreEqual(true, result.Error);
        }

        [Test]
        public void TestInvalidCommand()
        {
            var mockDataRepository = new Mock<IUserDataRepository>();
            mockDataRepository.Setup(x => x.SetData(new DataDTO { userID = "testUser", teamID = "testTeam", date = 1567580400, ha_present = false, num_maxalt = 0, num_aleve = 0 }));

            var userRecordService = new UserRecordService(mockDataRepository.Object);
            var result = userRecordService.StoreRecord("record", "testUser", "testTeam");
            Assert.AreEqual(true, result.Error);
            Assert.AreEqual("record", result.ResultMessage);
        }

        [Test]
        public void TestInvalidNumberMaxalt()
        {
            var mockDataRepository = new Mock<IUserDataRepository>();
            mockDataRepository.Setup(x => x.SetData(new DataDTO { userID = "testUser", teamID = "testTeam", date = 1567580400, ha_present = false, num_maxalt = -1, num_aleve = 0 }));

            var userRecordService = new UserRecordService(mockDataRepository.Object);
            var result = userRecordService.StoreRecord("record no -1 0", "testUser", "testTeam");
            Assert.AreEqual(true, result.Error);
            Assert.AreEqual("record no -1 0", result.ResultMessage);
        }

        [Test]
        public void TestInvalidNumberAleve()
        {
            var mockDataRepository = new Mock<IUserDataRepository>();
            mockDataRepository.Setup(x => x.SetData(new DataDTO { userID = "testUser", teamID = "testTeam", date = 1567580400, ha_present = false, num_maxalt = 1, num_aleve = -1 }));

            var userRecordService = new UserRecordService(mockDataRepository.Object);
            var result = userRecordService.StoreRecord("record no 1 -1", "testUser", "testTeam");
            Assert.AreEqual(true, result.Error);
            Assert.AreEqual("record no 1 -1", result.ResultMessage);
        }

        [Test]
        public void TestInvalidHAEntry()
        {
            var mockDataRepository = new Mock<IUserDataRepository>();
            var userRecordService = new UserRecordService(mockDataRepository.Object);
            var result = userRecordService.StoreRecord("record n 1 1", "testUser", "testTeam");
            Assert.AreEqual(true, result.Error);
            Assert.AreEqual("record n 1 1", result.ResultMessage);
        }

        [Test]
        public void TestExtraneousInfo()
        {

            var mockDataRepository = new Mock<IUserDataRepository>();
            var userRecordService = new UserRecordService(mockDataRepository.Object);
            var result = userRecordService.StoreRecord("record no 1 1 some other stuff", "testUser", "testTeam");
            Assert.AreEqual(true, result.Error);
            Assert.AreEqual("record no 1 1 some other stuff", result.ResultMessage);
        }

        [Test]
        public void TestMissingInfo()
        {

            var mockDataRepository = new Mock<IUserDataRepository>();
            var userRecordService = new UserRecordService(mockDataRepository.Object);
            var result = userRecordService.StoreRecord("record no 1", "testUser", "testTeam");
            Assert.AreEqual(true, result.Error);
            Assert.AreEqual("record no 1", result.ResultMessage);
        }
    }
}
