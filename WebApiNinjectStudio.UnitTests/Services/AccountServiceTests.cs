using Moq;
using WebApiNinjectStudio.Domain.Abstract;
using Xunit;
using Microsoft.EntityFrameworkCore;
using WebApiNinjectStudio.Domain.Concrete;
using Microsoft.Extensions.Configuration;
using WebApiNinjectStudio.Services;

namespace WebApiNinjectStudio.UnitTests.Services
{
    public class AccountServiceTests
    {
        private readonly IConfiguration _Configuration;
        private readonly IUserRepository _EFUserRepository;
        private readonly IRoleRepository _EFRoleRepository;
        private readonly AuthPolicyRequirement _AuthPolicyRequirement;


        public AccountServiceTests()
        {
            //IConfiguration
            var mockConfSection = new Mock<IConfigurationSection>();
            mockConfSection.SetupGet(m => m[It.Is<string>(s => s == "TokenSettings:SecretKey")]).Returns("SecureKeySecureKeySecureKeySecureKeySecureKeySecureKey");
            mockConfSection.SetupGet(m => m[It.Is<string>(s => s == "TokenSettings:HoursExpires")]).Returns("1");
            this._Configuration = mockConfSection.Object;

            //Repository
            var dbOptions = new DbContextOptionsBuilder<EFDbContext>()
                    .UseInMemoryDatabase(databaseName: "WebApiNinjectStudioDbInMemory")
                    .Options;
            var context = new EFDbContext(dbOptions);
            context.Database.EnsureCreated();
            this._EFUserRepository = new EFUserRepository(context);
            this._EFRoleRepository = new EFRoleRepository(context);

            //AuthPolicyRequirement
            this._AuthPolicyRequirement = new AuthPolicyRequirement();
        }

        [Fact]
        public void GetToken()
        {
            var target = new AccountService(this._EFUserRepository, this._Configuration, this._EFRoleRepository, this._AuthPolicyRequirement);
            var returnToken = target.GetToken("one@gmail.com", "M4jZrsPV2wNAeOH1YooKUdALx6Ek0DJaMf8yoiYI0Mc=");
            Assert.NotNull(returnToken);

            returnToken = target.GetToken("two@gmail.com", "M4jZrsPV2wNAeOH1YooKUdALx6Ek0DJaMf8yoiYI0Mc=");
            Assert.Null(returnToken);
        }
    }
}
