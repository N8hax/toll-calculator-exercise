using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TollCalculatorExercise.Domain.Enums;
using TollCalculatorExercise.Domain.Settings;
using TollCalculatorExercise.Services.Features.TollFee.Queries;
using TollCalculatorExercise.Services.Interfaces.Repositories;

namespace TollCalculatorExercise.UnitTest
{
    [TestFixture]
    public class GetTotalTollFeesPerDayQueryTests
    {
        private Mock<ITollFeeRepository> _tollFeeRepository;
        private Mock<IOptions<ApplicationSettings>>_config;
        private GetTotalTollFeesPerDayQueryHandler _getTotalTollFeesPerDayQueryHandler;
        public static IEnumerable<TestCaseData> WeekendsTestData
        {
            get
            {
                //Saturday
                yield return new TestCaseData(new DateTime[] { new DateTime(2022, 1, 2) });
                //Sunday
                yield return new TestCaseData(new DateTime[] { new DateTime(2022, 1, 3) });
            }
        }
        public static IEnumerable<TestCaseData> HolidaysTestData
        {
            get
            {
                yield return new TestCaseData(new DateTime[] { new DateTime(2013, 1, 1) });
                yield return new TestCaseData(new DateTime[] { new DateTime(2013, 3, 28) });

            }
        }

        public static IEnumerable<TestCaseData> FeeIsEightTestData
        {
            get
            {
                yield return new TestCaseData(new DateTime[] { new DateTime(2021, 10, 18, 6, 29, 59) });
                yield return new TestCaseData(new DateTime[] { new DateTime(2021, 10, 18, 8, 30, 0) });
                yield return new TestCaseData(new DateTime[] { new DateTime(2021, 10, 18, 10, 0, 0) });
                yield return new TestCaseData(new DateTime[] { new DateTime(2021, 10, 18, 14, 59, 59) });
            }
        }

        [SetUp]
        public void Setup()
        {
            ApplicationSettings app = new ApplicationSettings() {MAX_FEES_PER_DAY=60, CHARGE_INTERVAL_IN_SECONDS=3600};
            _config = new Mock<IOptions<ApplicationSettings>>();
            _config.Setup(ap => ap.Value).Returns(app);
            _tollFeeRepository = new Mock<ITollFeeRepository>();
            _getTotalTollFeesPerDayQueryHandler = new GetTotalTollFeesPerDayQueryHandler(_tollFeeRepository.Object, _config.Object);
        }

        [TestCase(VehicleTypeEnum.Motorbike)]
        [TestCase(VehicleTypeEnum.Tractor)]
        [TestCase(VehicleTypeEnum.Emergency)]
        [TestCase(VehicleTypeEnum.Diplomat)]
        [TestCase(VehicleTypeEnum.Foreign)]
        [TestCase(VehicleTypeEnum.Military)]
        public async Task GetTotalTollFeesPerDayQuery_FeeFreeVehicle_ShouldReturnZero(VehicleTypeEnum vehicleType)
        {
            // Arrange
            var dates = new DateTime[] { DateTime.Now };
            _tollFeeRepository.Setup(x => x.IsTollFreeAsync(vehicleType)).Returns(true);
            var query = new GetTotalTollFeesPerDayQuery { VehicleType = vehicleType, Dates = dates };

            // Act
            var task = await _getTotalTollFeesPerDayQueryHandler.Handle(query, new CancellationToken());

            // Assert
            Assert.AreEqual(0, task.Data);
        }

        [TestCaseSource(typeof(GetTotalTollFeesPerDayQueryTests), "WeekendsTestData")]
        public async Task GetTotalTollFeesPerDayQuery_Weekend_ShouldReturnZero(DateTime[] dates)
        {
            // Arrange
            var vehicleType = VehicleTypeEnum.Car;
            _tollFeeRepository.Setup(x => x.IsTollFreeAsync(dates[0])).Returns(true);
            var query = new GetTotalTollFeesPerDayQuery { VehicleType = vehicleType, Dates = dates};

            // Act
            var task = await _getTotalTollFeesPerDayQueryHandler.Handle(query, new CancellationToken());

            // Assert
            Assert.AreEqual(0, task.Data);
        }

        [TestCaseSource(typeof(GetTotalTollFeesPerDayQueryTests), "HolidaysTestData")]
        public async Task GetTotalTollFeesPerDayQuery_Holiday_ShouldReturnZero(DateTime[] dates)
        {
            // Arrange
            var vehicleType = VehicleTypeEnum.Car;
            _tollFeeRepository.Setup(x => x.IsTollFreeAsync(dates[0])).Returns(true);
            var query = new GetTotalTollFeesPerDayQuery { VehicleType = vehicleType, Dates = dates };

            // Act
            var task = await _getTotalTollFeesPerDayQueryHandler.Handle(query, new CancellationToken());

            // Assert
            Assert.AreEqual(0, task.Data);
        }

        [TestCaseSource(typeof(GetTotalTollFeesPerDayQueryTests), "FeeIsEightTestData")]
        public async Task GetTotalTollFeesPerDayQuery_FeeIsEight_ShouldReturnEight(DateTime[] dates)
        {
            // Arrange
            var vehicleType = VehicleTypeEnum.Car;
            _tollFeeRepository.Setup(x => x.GetMaxFeeByTimeSpanAsync(dates[0].TimeOfDay)).Returns(8M);
            var query = new GetTotalTollFeesPerDayQuery { VehicleType = vehicleType, Dates = dates };

            // Act
            var task = await _getTotalTollFeesPerDayQueryHandler.Handle(query);

            // Assert
            Assert.AreEqual(8, task.Data);
        }
    }
}