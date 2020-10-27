using Moq;
using Xunit;
using Microsoft.Extensions.Configuration;
using WebApiNinjectStudio.Services;

namespace WebApiNinjectStudio.UnitTests.Services
{
    public class Pbkdf2SecurityTests
    {
        private readonly IConfiguration _Configuration;

        public Pbkdf2SecurityTests()
        {
            //IConfiguration
            var mockConfSection = new Mock<IConfigurationSection>();
            mockConfSection.SetupGet(m => m[It.Is<string>(s => s == "AppSettings:SaltKeyOfPbkdf2")]).Returns("JKL28AUP2XYWQOPA");
            this._Configuration = mockConfSection.Object;
        }

        [Fact]
        public void HashPassword()
        {
            var target = new Pbkdf2Security(this._Configuration);
            // this._NumberOfRounds = 1000;
            //HelloWorld
            //M4jZrsPV2wNAeOH1YooKUdALx6Ek0DJaMf8yoiYI0Mc=
            Assert.Equal("M4jZrsPV2wNAeOH1YooKUdALx6Ek0DJaMf8yoiYI0Mc=", target.HashPassword("HelloWorld"));
        }
    }
}
