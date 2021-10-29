using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TollCalculatorExercise.Domain.Enums;
using TollCalculatorExercise.Domain.Settings;
using TollCalculatorExercise.Services;
using TollCalculatorExercise.Services.Features.TollFee.Queries;
using TollCalculatorExercise.Services.Interfaces.Repositories;
using TollCalculatorExercise.Services.Validators;

namespace TollCalculatorExercise.UnitTest
{
    [TestFixture]
    public class GetTotalTollFeesPerDayQueryTests
    {
        private Mock<IDateTollFeeRepository> _dateTollFeeRepository;
        private Mock<IDayOfWeekTollFeeRepository> _dayOfWeekTollFeeRepository;
        private Mock<ITimeSpanTollFeeRepository> _timeSpanTollFeeRepository;
        private Mock<IVehicleTypeTollFeeRepository> _vehicleTypeTollFeeRepository;
        private Mock<IOptions<ApplicationSettings>> _config;
        private GetTotalTollFeesPerDayQueryHandler _getTotalTollFeesPerDayQueryHandler;

        public static IEnumerable<TestCaseData> NotSameDayTestData
        {
            get
            {
                yield return new TestCaseData(new DateTime[] { new DateTime(2021, 1, 1), new DateTime(2021, 1, 2) });
            }
        }

        public static IEnumerable<TestCaseData> FutureDatesTestData
        {
            get
            {
                yield return new TestCaseData(new DateTime[] { new DateTime(2022, 1, 1) });
                yield return new TestCaseData(new DateTime[] { new DateTime(2021, 1, 1), new DateTime(2022, 1, 2) });
            }
        }
        public static IEnumerable<TestCaseData> WeekendsTestData
        {
            get
            {
                //Saturday
                yield return new TestCaseData(new DateTime[] { new DateTime(2021, 10, 16) });
                //Sunday
                yield return new TestCaseData(new DateTime[] { new DateTime(2021, 10, 17) });
            }
        }

        public static IEnumerable<TestCaseData> HolidaysTestData
        {
            get
            {
                yield return new TestCaseData(new DateTime[] { new DateTime(2021, 1, 1) });
                yield return new TestCaseData(new DateTime[] { new DateTime(2021, 3, 28) });

            }
        }

        public static IEnumerable<TestCaseData> FeeIsEightTestData
        {
            get
            {
                yield return new TestCaseData(new DateTime[] { new DateTime(2021, 10, 18, 6, 10, 0) });
                yield return new TestCaseData(new DateTime[] { new DateTime(2021, 10, 18, 8, 59, 59) });
                yield return new TestCaseData(new DateTime[] { new DateTime(2021, 10, 18, 12, 30, 0) });
                yield return new TestCaseData(new DateTime[] { new DateTime(2021, 10, 18, 18, 0, 0) });
            }
        }

        public static IEnumerable<TestCaseData> FeeIsFortySevenTestData
        {
            get
            {
                yield return new TestCaseData(new DateTime[] { 
                    new DateTime(2021, 10, 18, 6, 30, 0), new DateTime(2021, 10, 18, 6, 20, 0), 
                    new DateTime(2021, 10, 18, 11, 30, 0),
                    new DateTime(2021, 10, 18, 12, 31, 0),
                    new DateTime(2021, 10, 18, 14, 59, 0), new DateTime(2021, 10, 18, 15, 29, 0), new DateTime(2021, 10, 18, 15, 31, 0) });
            }
        }

        [SetUp]
        public void Setup()
        {
            ApplicationSettings app = new ApplicationSettings() { MAX_FEES_PER_DAY = 60, CHARGE_INTERVAL_IN_SECONDS = 3600 };
            _config = new Mock<IOptions<ApplicationSettings>>();
            _config.Setup(ap => ap.Value).Returns(app);

            _dateTollFeeRepository = new Mock<IDateTollFeeRepository>();
            _dateTollFeeRepository.Setup(x => x.IsTollFree(It.Is<DateTime>(y => y.Date == new DateTime(2021, 1, 1).Date))).Returns(true);
            _dateTollFeeRepository.Setup(x => x.IsTollFree(It.Is<DateTime>(y => y.Date == new DateTime(2021, 3, 28).Date))).Returns(true);

            _dayOfWeekTollFeeRepository = new Mock<IDayOfWeekTollFeeRepository>();
            _dayOfWeekTollFeeRepository.Setup(x => x.IsTollFree(It.Is<DateTime>(y => y.DayOfWeek ==  DayOfWeek.Saturday))).Returns(true);
            _dayOfWeekTollFeeRepository.Setup(x => x.IsTollFree(It.Is<DateTime>(y => y.DayOfWeek == DayOfWeek.Sunday))).Returns(true);

            _vehicleTypeTollFeeRepository = new Mock<IVehicleTypeTollFeeRepository>();
            _vehicleTypeTollFeeRepository.Setup(x => x.IsTollFree(VehicleTypeEnum.Motorbike)).Returns(true);
            _vehicleTypeTollFeeRepository.Setup(x => x.IsTollFree(VehicleTypeEnum.Tractor)).Returns(true);
            _vehicleTypeTollFeeRepository.Setup(x => x.IsTollFree(VehicleTypeEnum.Emergency)).Returns(true);
            _vehicleTypeTollFeeRepository.Setup(x => x.IsTollFree(VehicleTypeEnum.Diplomat)).Returns(true);
            _vehicleTypeTollFeeRepository.Setup(x => x.IsTollFree(VehicleTypeEnum.Foreign)).Returns(true);
            _vehicleTypeTollFeeRepository.Setup(x => x.IsTollFree(VehicleTypeEnum.Military)).Returns(true);
            _vehicleTypeTollFeeRepository.Setup(x => x.IsTollFree(VehicleTypeEnum.Car)).Returns(false);

            _timeSpanTollFeeRepository = new Mock<ITimeSpanTollFeeRepository>();
            _timeSpanTollFeeRepository.Setup(x => x.GetFeeByTimeSpan(It.IsInRange(new TimeSpan(6, 0, 0), new TimeSpan(6, 29, 59), Moq.Range.Inclusive))).Returns(8M);
            _timeSpanTollFeeRepository.Setup(x => x.GetFeeByTimeSpan(It.IsInRange(new TimeSpan(6, 30, 0), new TimeSpan(6, 59, 59), Moq.Range.Inclusive))).Returns(13M);
            _timeSpanTollFeeRepository.Setup(x => x.GetFeeByTimeSpan(It.IsInRange(new TimeSpan(7, 0, 0), new TimeSpan(7, 59, 59), Moq.Range.Inclusive))).Returns(18M);
            _timeSpanTollFeeRepository.Setup(x => x.GetFeeByTimeSpan(It.IsInRange(new TimeSpan(8, 0, 0), new TimeSpan(8, 29, 59), Moq.Range.Inclusive))).Returns(13M);
            _timeSpanTollFeeRepository.Setup(x => x.GetFeeByTimeSpan(It.IsInRange(new TimeSpan(8, 30, 0), new TimeSpan(8, 59, 59), Moq.Range.Inclusive))).Returns(8M);
            _timeSpanTollFeeRepository.Setup(x => x.GetFeeByTimeSpan(It.IsInRange(new TimeSpan(9, 30, 0), new TimeSpan(9, 59, 59), Moq.Range.Inclusive))).Returns(8M);
            _timeSpanTollFeeRepository.Setup(x => x.GetFeeByTimeSpan(It.IsInRange(new TimeSpan(10, 30, 0), new TimeSpan(10, 59, 59), Moq.Range.Inclusive))).Returns(8M);
            _timeSpanTollFeeRepository.Setup(x => x.GetFeeByTimeSpan(It.IsInRange(new TimeSpan(11, 30, 0), new TimeSpan(11, 59, 59), Moq.Range.Inclusive))).Returns(8M);
            _timeSpanTollFeeRepository.Setup(x => x.GetFeeByTimeSpan(It.IsInRange(new TimeSpan(12, 30, 0), new TimeSpan(12, 59, 59), Moq.Range.Inclusive))).Returns(8M);
            _timeSpanTollFeeRepository.Setup(x => x.GetFeeByTimeSpan(It.IsInRange(new TimeSpan(13, 30, 0), new TimeSpan(13, 59, 59), Moq.Range.Inclusive))).Returns(8M);
            _timeSpanTollFeeRepository.Setup(x => x.GetFeeByTimeSpan(It.IsInRange(new TimeSpan(14, 30, 0), new TimeSpan(14, 59, 59), Moq.Range.Inclusive))).Returns(8M);
            _timeSpanTollFeeRepository.Setup(x => x.GetFeeByTimeSpan(It.IsInRange(new TimeSpan(15, 0, 0), new TimeSpan(15, 29, 59), Moq.Range.Inclusive))).Returns(13M);
            _timeSpanTollFeeRepository.Setup(x => x.GetFeeByTimeSpan(It.IsInRange(new TimeSpan(15, 30, 0), new TimeSpan(16, 59, 59), Moq.Range.Inclusive))).Returns(18M);
            _timeSpanTollFeeRepository.Setup(x => x.GetFeeByTimeSpan(It.IsInRange(new TimeSpan(17, 0, 0), new TimeSpan(17, 59, 59), Moq.Range.Inclusive))).Returns(13M);
            _timeSpanTollFeeRepository.Setup(x => x.GetFeeByTimeSpan(It.IsInRange(new TimeSpan(18, 0, 0), new TimeSpan(18, 29, 59), Moq.Range.Inclusive))).Returns(8M);

            _getTotalTollFeesPerDayQueryHandler = new GetTotalTollFeesPerDayQueryHandler(_dateTollFeeRepository.Object,
                _dayOfWeekTollFeeRepository.Object, _timeSpanTollFeeRepository.Object, 
                _vehicleTypeTollFeeRepository.Object, _config.Object);
        }

        [TestCase(null)]
        public async Task GetTotalTollFeesPerDayQueryValidator_VehicleTypeIsNull_ShouldHaveError(VehicleTypeEnum vehicleType)
        {
            // Arrange
            var dates = new DateTime[] { DateTime.Now };
            var query = new GetTotalTollFeesPerDayQuery { VehicleType = vehicleType, Dates = dates };
            var validator = new GetTotalTollFeesPerDayQueryValidator();

            // Act
            var result = await validator.ValidateAsync(query);

            // Assert
            Assert.That(result.Errors.Any(o => o.PropertyName == "VehicleType"));
        }

        [TestCase((VehicleTypeEnum)100)]
        public async Task GetTotalTollFeesPerDayQueryValidator_InvalidVehicleType_ShouldHaveError(VehicleTypeEnum vehicleType)
        {
            // Arrange
            var dates = new DateTime[] { DateTime.Now };
            var query = new GetTotalTollFeesPerDayQuery { VehicleType = vehicleType, Dates = dates };
            var validator = new GetTotalTollFeesPerDayQueryValidator();

            // Act
            var result = await validator.ValidateAsync(query);

            // Assert
            Assert.That(result.Errors.Any(o => o.PropertyName == "VehicleType"));
        }

        [TestCase(VehicleTypeEnum.Motorbike)]
        [TestCase(VehicleTypeEnum.Car)]
        public async Task GetTotalTollFeesPerDayQueryValidator_DatesArrayIsNull_ShouldHaveError(VehicleTypeEnum vehicleType)
        {
            // Arrange
            DateTime[] dates = null;
            var query = new GetTotalTollFeesPerDayQuery { VehicleType = vehicleType, Dates = dates };
            var validator = new GetTotalTollFeesPerDayQueryValidator();

            // Act
            var result = await validator.ValidateAsync(query);

            // Assert
            Assert.That(result.Errors.Any(o => o.PropertyName == "Dates"));
        }

        [TestCase(VehicleTypeEnum.Motorbike)]
        [TestCase(VehicleTypeEnum.Car)]
        public async Task GetTotalTollFeesPerDayQueryValidator_DatesArrayIsEmpty_ShouldHaveError(VehicleTypeEnum vehicleType)
        {
            // Arrange
            DateTime[] dates = new DateTime[0];
            var query = new GetTotalTollFeesPerDayQuery { VehicleType = vehicleType, Dates = dates };
            var validator = new GetTotalTollFeesPerDayQueryValidator();

            // Act
            var result = await validator.ValidateAsync(query);

            // Assert
            Assert.That(result.Errors.Any(o => o.PropertyName == "Dates"));
        }

        [TestCaseSource(typeof(GetTotalTollFeesPerDayQueryTests), "NotSameDayTestData")]
        public async Task GetTotalTollFeesPerDayQueryValidator_DatesArrayAreNotInSameDay_ShouldHaveError(DateTime[] dates)
        {
            // Arrange
            var vehicleType = VehicleTypeEnum.Car;
            var query = new GetTotalTollFeesPerDayQuery { VehicleType = vehicleType, Dates = dates };
            var validator = new GetTotalTollFeesPerDayQueryValidator();

            // Act
            var result = await validator.ValidateAsync(query);

            // Assert
            Assert.That(result.Errors.Any(o => o.PropertyName == "Dates"));
        }

        [TestCaseSource(typeof(GetTotalTollFeesPerDayQueryTests), "FutureDatesTestData")]
        public async Task GetTotalTollFeesPerDayQueryValidator_DatesArrayAreInTheFuture_ShouldHaveError(DateTime[] dates)
        {
            // Arrange
            var vehicleType = VehicleTypeEnum.Car;
            var query = new GetTotalTollFeesPerDayQuery { VehicleType = vehicleType, Dates = dates };
            var validator = new GetTotalTollFeesPerDayQueryValidator();

            // Act
            var result = await validator.ValidateAsync(query);

            // Assert
            Assert.That(result.Errors.Any(o => o.PropertyName == "Dates"));
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
            var query = new GetTotalTollFeesPerDayQuery { VehicleType = vehicleType, Dates = dates };

            // Act
            var task = await _getTotalTollFeesPerDayQueryHandler.Handle(query, new CancellationToken());

            // Assert
            Assert.AreEqual(0, task.Data);
        }

        [TestCaseSource(typeof(GetTotalTollFeesPerDayQueryTests), "WeekendsTestData")]
        public async Task GetTotalTollFeesPerDayQuery_WeekendAndPaidVehicleType_ShouldReturnZero(DateTime[] dates)
        {
            // Arrange
            var vehicleType = VehicleTypeEnum.Car;
            var query = new GetTotalTollFeesPerDayQuery { VehicleType = vehicleType, Dates = dates };

            // Act
            var task = await _getTotalTollFeesPerDayQueryHandler.Handle(query, new CancellationToken());

            // Assert
            Assert.AreEqual(0, task.Data);
        }

        [TestCaseSource(typeof(GetTotalTollFeesPerDayQueryTests), "WeekendsTestData")]
        public async Task GetTotalTollFeesPerDayQuery_WeekendAndFreeVehicleType_ShouldReturnZero(DateTime[] dates)
        {
            // Arrange
            var vehicleType = VehicleTypeEnum.Motorbike;
            var query = new GetTotalTollFeesPerDayQuery { VehicleType = vehicleType, Dates = dates };

            // Act
            var task = await _getTotalTollFeesPerDayQueryHandler.Handle(query, new CancellationToken());

            // Assert
            Assert.AreEqual(0, task.Data);
        }

        [TestCaseSource(typeof(GetTotalTollFeesPerDayQueryTests), "HolidaysTestData")]
        public async Task GetTotalTollFeesPerDayQuery_HolidayAndPaidVehicleType_ShouldReturnZero(DateTime[] dates)
        {
            // Arrange
            var vehicleType = VehicleTypeEnum.Car;
            var query = new GetTotalTollFeesPerDayQuery { VehicleType = vehicleType, Dates = dates };

            // Act
            var task = await _getTotalTollFeesPerDayQueryHandler.Handle(query, new CancellationToken());

            // Assert
            Assert.AreEqual(0, task.Data);
        }

        [TestCaseSource(typeof(GetTotalTollFeesPerDayQueryTests), "HolidaysTestData")]
        public async Task GetTotalTollFeesPerDayQuery_HolidayAndFreeVehicleType_ShouldReturnZero(DateTime[] dates)
        {
            // Arrange
            var vehicleType = VehicleTypeEnum.Motorbike;
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
            var query = new GetTotalTollFeesPerDayQuery { VehicleType = vehicleType, Dates = dates };

            // Act
            var task = await _getTotalTollFeesPerDayQueryHandler.Handle(query);

            // Assert
            Assert.AreEqual(8, task.Data);
        }

        [TestCaseSource(typeof(GetTotalTollFeesPerDayQueryTests), "FeeIsFortySevenTestData")]
        public async Task GetTotalTollFeesPerDayQuery_FeeIsFortySeven_ShouldReturnFortySeven(DateTime[] dates)
        {
            // Arrange
            var vehicleType = VehicleTypeEnum.Car;
            var query = new GetTotalTollFeesPerDayQuery { VehicleType = vehicleType, Dates = dates };

            // Act
            var task = await _getTotalTollFeesPerDayQueryHandler.Handle(query);

            // Assert
            Assert.AreEqual(47, task.Data);
        }
    }
}