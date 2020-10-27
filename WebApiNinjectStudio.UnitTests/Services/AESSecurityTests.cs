using Moq;
using Xunit;
using Microsoft.Extensions.Configuration;
using WebApiNinjectStudio.Services;

namespace WebApiNinjectStudio.UnitTests.Services
{
    public class AESSecurityTests
    {
        private readonly IConfiguration _Configuration;
        public AESSecurityTests()
        {
            //IConfiguration
            var mockConfSection = new Mock<IConfigurationSection>();
            mockConfSection.SetupGet(m => m[It.Is<string>(s => s == "AppSettings:SecretKeyOfAes")]).Returns("I15TMSLO0KXUWTHO");            
            this._Configuration = mockConfSection.Object;
        }

        [Fact]
        public void DecyptAndEncrypt()
        {
            var target = new AESSecurity(this._Configuration);
            var clearTxt = "HelloWorld";
            var encryptedTxt = target.AesEncrypt(clearTxt);
            Assert.Equal(clearTxt, target.AesDecypt(encryptedTxt));
        }


    }
}
