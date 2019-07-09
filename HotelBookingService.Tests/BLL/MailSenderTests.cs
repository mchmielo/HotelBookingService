using AutoFixture;
using HotelBookingService.BLL;
using HotelBookingService.BLL.Model;
using NUnit.Framework;
using System;

namespace HotelBookingService.Tests.BLL
{
    class MailSenderTests
    {
        private Fixture _fixture;
        private IMailSender _sut;


        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            _fixture = new Fixture();
            _fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        }

        [SetUp]
        public void Setup()
        {
            _sut = new MailSender();
        }

        [Test]
        public void SendMail_NotCorrectEmailAddressPassed_ThrowsFormatException()
        {
            //Arrange
            UserMailDto dto = _fixture.Create<UserMailDto>();

            //Act & assert
            Assert.Throws<FormatException>(() => _sut.SendMail(dto));
        }

        [Test]
        public void SendMail_CorrectDataPassed_DoesNotThrow()
        {
            //Arrange
            UserMailDto dto = new UserMailDto()
            {
                MailAddress = "bookingservicefake@gmail.com",
                Password = "BookingService123!",
                Username = "bookingservicefake@gmail.com"
            };

            //Act & assert
            Assert.DoesNotThrow(() => _sut.SendMail(dto));
        }
    }
}
