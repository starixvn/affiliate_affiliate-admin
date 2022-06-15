using Fanex.AffiliateAdmin.Controller.UnitTests.Helpers;
using Fanex.AffiliateAdmin.Core.Configurations;
using Fanex.AffiliateAdmin.Core.Services.Interface;
using Fanex.AffiliateAdmin.Criterias.Configuration.AffType;
using Fanex.AffiliateAdmin.Criterias.Configuration.Country;
using Fanex.AffiliateAdmin.Criterias.Configuration.Language;
using Fanex.AffiliateAdmin.Criterias.Configuration.Product;
using Fanex.AffiliateAdmin.Criterias.Configuration.SystemSetting;
using Fanex.AffiliateAdmin.DTO;
using Fanex.AffiliateAdmin.Models;
using Fanex.AffiliateAdmin.Models.Configurations;
using NSubstitute;
using System.Collections.Generic;
using System.Web.Mvc;
using Xunit;

namespace Fanex.AffiliateAdmin.Controller.UnitTests
{
    public class ConfigurationControllerTest
    {
        private readonly IAppSetting _appSetting;
        private readonly ISystemSettingService _systemSettingService;

        public ConfigurationControllerTest()
        {
            _appSetting = Substitute.For<IAppSetting>();
            _systemSettingService = Substitute.For<ISystemSettingService>();
        }

        [Fact]
        public void Index_WhenCall_ReturnView()
        {
            //Arrange
            //MockingHelpers.SetUpEmptyHttpContextSession();
            _systemSettingService.GetSystemSetting(Arg.Any<SystemSettingsSelByNameCriteria>()).Returns(new SystemSetting());

            //Action
            var controller = CreateController();
            var result = controller.Index() as ViewResult;
            var model = result.Model as ConfigurationViewModel;

            //Assert
            Assert.Equal("", result.ViewName);
            Assert.NotNull(model.TypeList);
        }

        [Fact]
        public void GetSystemSettingList_WhenCall_ReturnCorrectData()
        {
            //Arrange
            _systemSettingService.GetSystemSettingList(Arg.Any<SystemSettingSelectCriteria>()).Returns(new List<SystemSetting> { new SystemSetting() });

            //Action
            var controller = CreateController();
            var result = controller.GetSystemSettingList(new SystemSettingSelectCriteria()) as JsonResult;

            //Assert
            Assert.NotNull(result.Data);
        }

        [Fact]
        public void ConfigurationDetail_WhenCall_ReturnView()
        {
            //Arrange
            _systemSettingService.GetSystemSetting(Arg.Any<SystemSettingsSelByIdCriteria>()).Returns(new SystemSetting());
            _systemSettingService.GetSystemSettingList(Arg.Any<SystemSettingsSelByTypeCriteria>()).Returns(new List<SystemSetting> { new SystemSetting() });

            //Action
            var controller = CreateController();
            var result = controller.ConfigurationDetail(new SystemSettingsSelByIdCriteria()) as ViewResult;
            var model = result.Model as SystemSettingUpdateViewModel;

            //Assert
            Assert.Equal("ConfigurationDetail", result.ViewName);
            Assert.Single(model.EnvironmentList);
        }

        [Theory]
        [InlineData("")]
        [InlineData("value")]
        public void UpdateConfiguration_WhenCall_ReturnView(string value)
        {
            //Arrange
            var httpRequest = MockingHelpers.GetHttpRequest();
            MockingHelpers.SetUpEmptyHttpContextSession();

            _systemSettingService.GetSystemSetting(Arg.Any<SystemSettingsSelByIdCriteria>()).Returns(new SystemSetting());
            _systemSettingService.GetSystemSettingList(Arg.Any<SystemSettingsSelByTypeCriteria>()).Returns(new List<SystemSetting> { new SystemSetting() });

            //Action
            var controller = CreateController();
            if (string.IsNullOrEmpty(value))
            {
                controller.ModelState.AddModelError("Error01", "error");
            }
            controller.ControllerContext = MockingHelpers.GetControllerContext(httpRequest);
            var result = controller.UpdateConfiguration(new SystemSettingUpdateCriteria { Value = value }) as ViewResult;
            var model = result.Model as SystemSettingUpdateViewModel;

            //Assert
            Assert.Equal("ConfigurationDetail", result.ViewName);
            Assert.Single(model.EnvironmentList);
        }

        [Theory]
        [InlineData("", 1003)]
        [InlineData("1,2,3", 0)]
        public void DeleteConfiguration_WhenCall_ReturnView(string idList, int errCode)
        {
            //Arrange
            var httpRequest = MockingHelpers.GetHttpRequest();
            MockingHelpers.SetUpEmptyHttpContextSession();

            _systemSettingService.GetSystemSetting(Arg.Any<SystemSettingsSelByIdCriteria>()).Returns(new SystemSetting());
            _systemSettingService.GetSystemSettingList(Arg.Any<SystemSettingsSelByTypeCriteria>()).Returns(new List<SystemSetting> { new SystemSetting() });

            //Action
            var controller = CreateController();
            if (string.IsNullOrEmpty(idList))
            {
                controller.ModelState.AddModelError("Error01", "error");
            }
            controller.ControllerContext = MockingHelpers.GetControllerContext(httpRequest);
            var result = controller.DeleteConfiguration(new SystemSettingDeleteCriteria { IDList = idList }) as JsonResult;
            var data = result.Data as ResultViewModel;

            //Assert
            Assert.Equal(errCode, data.ErrorCode);
        }

        [Fact]
        public void AddConfiguration_GET_WhenCall_ReturnView()
        {
            //Arrange
            _systemSettingService.GetSystemSetting(Arg.Any<SystemSettingsSelByNameCriteria>()).Returns(new SystemSetting());
            _systemSettingService.GetSystemSettingList(Arg.Any<SystemSettingsSelByTypeCriteria>()).Returns(new List<SystemSetting> { new SystemSetting() });

            //Action
            var controller = CreateController();
            var result = controller.AddConfiguration() as ViewResult;
            var model = result.Model as AddConfigurationViewModel;

            //Assert
            Assert.Equal("", result.ViewName);
            Assert.NotNull(model.TypeList);
            Assert.Single(model.EnvironmentList);
        }

        [Theory]
        [InlineData("", "")]
        [InlineData("abc", "abc")]
        public void AddConfiguration_POST_WhenCall_ReturnView(string key, string value)
        {
            //Arrange
            var httpRequest = MockingHelpers.GetHttpRequest();
            MockingHelpers.SetUpEmptyHttpContextSession();

            _systemSettingService.GetSystemSetting(Arg.Any<SystemSettingsSelByNameCriteria>()).Returns(new SystemSetting { SValue = "1"} );
            _systemSettingService.GetSystemSettingList(Arg.Any<SystemSettingsSelByTypeCriteria>()).Returns(new List<SystemSetting> { new SystemSetting() });

            //Action
            var controller = CreateController();
            if (string.IsNullOrEmpty(key))
            {
                controller.ModelState.AddModelError("Error01", "error");
            }
            controller.ControllerContext = MockingHelpers.GetControllerContext(httpRequest);
            var result = controller.AddConfiguration(new SystemSettingAddCriteria { Key = key, Value = value }) as ViewResult;

            //Assert
            Assert.Equal("", result.ViewName);
        }

        [Fact]
        public void Country_WhenCall_ReturnView()
        {
            //Arrange

            //Action
            var controller = CreateController();
            var result = controller.Country() as ViewResult;

            //Assert
            Assert.Equal("Country", result.ViewName);
        }

        [Fact]
        public void GetCountryList_WhenCall_ReturnCorrectData()
        {
            //Arrange
            _systemSettingService.GetCountryList(Arg.Any<CountrySelectCriteria>()).Returns(new List<Country> { new Country() });

            //Action
            var controller = CreateController();
            var result = controller.GetCountryList(new CountrySelectCriteria()) as JsonResult;

            //Assert
            Assert.NotNull(result.Data);
        }

        [Fact]
        public void CountryDetail_WhenCall_ReturnCorrectView()
        {
            //Arrange
            _systemSettingService.GetCountry(Arg.Any<CountryDetailCriteria>()).Returns(new Country());

            //Action
            var controller = CreateController();
            var result = controller.CountryDetail(new CountryDetailCriteria()) as ViewResult;

            //Assert
            Assert.Equal("CountryDetail", result.ViewName);
        }

        [Theory]
        [InlineData("", "")]
        [InlineData("123", "abc")]
        public void UpdateCountry_WhenCall_ReturnView(string countryId, string countryName)
        {
            //Arrange
            var httpRequest = MockingHelpers.GetHttpRequest();
            MockingHelpers.SetUpEmptyHttpContextSession();

            //Action
            var controller = CreateController();
            if (string.IsNullOrEmpty(countryId))
            {
                controller.ModelState.AddModelError("Error01", "error");
            }
            controller.ControllerContext = MockingHelpers.GetControllerContext(httpRequest);
            var result = controller.UpdateCountry(new CountryUpdateCriteria { CountryID = countryId, CountryName = countryName }) as ViewResult;

            //Assert
            Assert.Equal("CountryDetail", result.ViewName);
        }

        [Fact]
        public void AddCountry_GET_WhenCall_ReturnCorrectView()
        {
            //Arrange

            //Action
            var controller = CreateController();
            var result = controller.AddCountry() as ViewResult;

            //Assert
            Assert.Equal("", result.ViewName);
        }

        [Theory]
        [InlineData("", "")]
        [InlineData("123", "abc")]
        public void AddCountry_POST_WhenCall_ReturnView(string countryId, string countryName)
        {
            //Arrange
            var httpRequest = MockingHelpers.GetHttpRequest();
            MockingHelpers.SetUpEmptyHttpContextSession();

            //Action
            var controller = CreateController();
            if (string.IsNullOrEmpty(countryId))
            {
                controller.ModelState.AddModelError("Error01", "error");
            }
            controller.ControllerContext = MockingHelpers.GetControllerContext(httpRequest);
            var result = controller.AddCountry(new CountryInsertCriteria { CountryID = countryId, CountryName = countryName }) as ViewResult;

            //Assert
            Assert.Equal("", result.ViewName);
        }

        [Theory]
        [InlineData("", 1003)]
        [InlineData("1,2,3", 0)]
        public void DeleteCountry_WhenCall_ReturnCorrectData(string idList, int errCode)
        {
            //Arrange
            var httpRequest = MockingHelpers.GetHttpRequest();
            MockingHelpers.SetUpEmptyHttpContextSession();

            //Action
            var controller = CreateController();
            if (string.IsNullOrEmpty(idList))
            {
                controller.ModelState.AddModelError("Error01", "error");
            }
            controller.ControllerContext = MockingHelpers.GetControllerContext(httpRequest);
            var result = controller.DeleteCountry(new CountryDeleteCriteria { IDList = idList }) as JsonResult;
            var data = result.Data as ResultViewModel;

            //Assert
            Assert.Equal(errCode, data.ErrorCode);
        }

        [Fact]
        public void Product_WhenCall_ReturnView()
        {
            //Arrange

            //Action
            var controller = CreateController();
            var result = controller.Product() as ViewResult;

            //Assert
            Assert.Equal("Product", result.ViewName);
        }

        [Fact]
        public void GetProductList_WhenCall_ReturnCorrectData()
        {
            //Arrange
            _systemSettingService.GetProductList(Arg.Any<ProductSelectCriteria>()).Returns(new List<Product> { new Product() });

            //Action
            var controller = CreateController();
            var result = controller.GetProductList(new ProductSelectCriteria()) as JsonResult;

            //Assert
            Assert.NotNull(result.Data);
        }

        [Fact]
        public void ProductDetail_WhenCall_ReturnCorrectView()
        {
            //Arrange
            _systemSettingService.GetProduct(Arg.Any<ProductDetailCriteria>()).Returns(new Product());

            //Action
            var controller = CreateController();
            var result = controller.ProductDetail(new ProductDetailCriteria()) as ViewResult;

            //Assert
            Assert.Equal("ProductDetail", result.ViewName);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(123)]
        public void UpdateProduct_WhenCall_ReturnView(byte productID)
        {
            //Arrange
            var httpRequest = MockingHelpers.GetHttpRequest();
            MockingHelpers.SetUpEmptyHttpContextSession();

            //Action
            var controller = CreateController();
            if (productID == 0)
            {
                controller.ModelState.AddModelError("Error01", "error");
            }
            controller.ControllerContext = MockingHelpers.GetControllerContext(httpRequest);
            var result = controller.UpdateProduct(new ProductUpdateCriteria { ProductID = productID }) as ViewResult;

            //Assert
            Assert.Equal("ProductDetail", result.ViewName);
        }

        [Fact]
        public void AddProduct_GET_WhenCall_ReturnCorrectView()
        {
            //Arrange

            //Action
            var controller = CreateController();
            var result = controller.AddProduct() as ViewResult;

            //Assert
            Assert.Equal("", result.ViewName);
        }

        [Theory]
        [InlineData("")]
        [InlineData("abc")]
        public void AddProduct_POST_WhenCall_ReturnView(string productName)
        {
            //Arrange
            var httpRequest = MockingHelpers.GetHttpRequest();
            MockingHelpers.SetUpEmptyHttpContextSession();

            //Action
            var controller = CreateController();
            if (string.IsNullOrEmpty(productName))
            {
                controller.ModelState.AddModelError("Error01", "error");
            }
            controller.ControllerContext = MockingHelpers.GetControllerContext(httpRequest);
            var result = controller.AddProduct(new ProductAddCriteria { ProductName = productName }) as ViewResult;

            //Assert
            Assert.Equal("", result.ViewName);
        }

        [Theory]
        [InlineData("", 1003)]
        [InlineData("1,2,3", 0)]
        public void DeleteProduct_WhenCall_ReturnCorrectData(string idList, int errCode)
        {
            //Arrange
            var httpRequest = MockingHelpers.GetHttpRequest();
            MockingHelpers.SetUpEmptyHttpContextSession();

            //Action
            var controller = CreateController();
            if (string.IsNullOrEmpty(idList))
            {
                controller.ModelState.AddModelError("Error01", "error");
            }
            controller.ControllerContext = MockingHelpers.GetControllerContext(httpRequest);
            var result = controller.DeleteProduct(new ProductDeleteCriteria { IDList = idList }) as JsonResult;
            var data = result.Data as ResultViewModel;

            //Assert
            Assert.Equal(errCode, data.ErrorCode);
        }

        [Fact]
        public void Language_WhenCall_ReturnView()
        {
            //Arrange

            //Action
            var controller = CreateController();
            var result = controller.Language() as ViewResult;

            //Assert
            Assert.Equal("Language", result.ViewName);
        }

        [Fact]
        public void GetLanguageList_WhenCall_ReturnCorrectData()
        {
            //Arrange
            _systemSettingService.GetLanguageList(Arg.Any<LanguageSelectCriteria>()).Returns(new List<Language> { new Language() });

            //Action
            var controller = CreateController();
            var result = controller.GetLanguageList(new LanguageSelectCriteria()) as JsonResult;

            //Assert
            Assert.NotNull(result.Data);
        }

        [Fact]
        public void LanguageDetail_WhenCall_ReturnCorrectView()
        {
            //Arrange
            _systemSettingService.GetLanguage(Arg.Any<LanguageDetailCriteria>()).Returns(new Language());

            //Action
            var controller = CreateController();
            var result = controller.LanguageDetail(new LanguageDetailCriteria()) as ViewResult;

            //Assert
            Assert.Equal("LanguageDetail", result.ViewName);
        }

        [Theory]
        [InlineData("", "", "")]
        [InlineData("123", "en-US", "English")]
        public void UpdateLanguage_WhenCall_ReturnView(string countryId, string languageID, string languageName)
        {
            //Arrange
            var httpRequest = MockingHelpers.GetHttpRequest();
            MockingHelpers.SetUpEmptyHttpContextSession();
            _systemSettingService.GetLanguage(Arg.Any<LanguageDetailCriteria>()).Returns(new Language());

            //Action
            var controller = CreateController();
            if (string.IsNullOrEmpty(countryId))
            {
                controller.ModelState.AddModelError("Error01", "error");
            }
            controller.ControllerContext = MockingHelpers.GetControllerContext(httpRequest);
            var result = controller.UpdateLanguage(new LanguageUpdateCriteria { CountryID = countryId, LanguageID = languageID, LanguageName = languageName }) as ViewResult;

            //Assert
            Assert.Equal("LanguageDetail", result.ViewName);
        }

        [Fact]
        public void AddLanguage_GET_WhenCall_ReturnCorrectView()
        {
            //Arrange

            //Action
            var controller = CreateController();
            var result = controller.AddLanguage() as ViewResult;

            //Assert
            Assert.Equal("", result.ViewName);
        }

        [Theory]
        [InlineData("")]
        [InlineData("en-US")]
        public void AddLanguage_POST_WhenCall_ReturnView(string languageID)
        {
            //Arrange
            var httpRequest = MockingHelpers.GetHttpRequest();
            MockingHelpers.SetUpEmptyHttpContextSession();

            //Action
            var controller = CreateController();
            if (string.IsNullOrEmpty(languageID))
            {
                controller.ModelState.AddModelError("Error01", "error");
            }
            controller.ControllerContext = MockingHelpers.GetControllerContext(httpRequest);
            var result = controller.AddLanguage(new LanguageAddCriteria { LanguageID = languageID }) as ViewResult;

            //Assert
            Assert.Equal("", result.ViewName);
        }

        [Theory]
        [InlineData("", 1003)]
        [InlineData("1,2,3", 0)]
        public void DeleteLanguage_WhenCall_ReturnCorrectData(string idList, int errCode)
        {
            //Arrange
            var httpRequest = MockingHelpers.GetHttpRequest();
            MockingHelpers.SetUpEmptyHttpContextSession();

            //Action
            var controller = CreateController();
            if (string.IsNullOrEmpty(idList))
            {
                controller.ModelState.AddModelError("Error01", "error");
            }
            controller.ControllerContext = MockingHelpers.GetControllerContext(httpRequest);
            var result = controller.DeleteLanguage(new LanguageDeleteCriteria { IDList = idList }) as JsonResult;
            var data = result.Data as ResultViewModel;

            //Assert
            Assert.Equal(errCode, data.ErrorCode);
        }

        [Fact]
        public void AffType_WhenCall_ReturnView()
        {
            //Arrange

            //Action
            var controller = CreateController();
            var result = controller.AffType() as ViewResult;

            //Assert
            Assert.Equal("AffType", result.ViewName);
        }

        [Fact]
        public void GetAffTypeList_WhenCall_ReturnCorrectData()
        {
            //Arrange
            _systemSettingService.GetAffTypeList(Arg.Any<AffTypeSelectCriteria>()).Returns(new List<AffType> { new AffType() });

            //Action
            var controller = CreateController();
            var result = controller.GetAffTypeList(new AffTypeSelectCriteria()) as JsonResult;

            //Assert
            Assert.NotNull(result.Data);
        }

        [Fact]
        public void AffTypeDetail_WhenCall_ReturnCorrectView()
        {
            //Arrange
            _systemSettingService.GetAffType(Arg.Any<AffTypeDetailCriteria>()).Returns(new AffType());

            //Action
            var controller = CreateController();
            var result = controller.AffTypeDetail(new AffTypeDetailCriteria()) as ViewResult;

            //Assert
            Assert.Equal("AffTypeDetail", result.ViewName);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(123)]
        public void UpdateAffType_WhenCall_ReturnView(byte id)
        {
            //Arrange
            var httpRequest = MockingHelpers.GetHttpRequest();
            MockingHelpers.SetUpEmptyHttpContextSession();

            //Action
            var controller = CreateController();
            if (id == 0)
            {
                controller.ModelState.AddModelError("Error01", "error");
            }
            controller.ControllerContext = MockingHelpers.GetControllerContext(httpRequest);
            var result = controller.UpdateAffType(new AffTypeUpdateCriteria { ID = id }) as ViewResult;

            //Assert
            Assert.Equal("AffTypeDetail", result.ViewName);
        }

        [Fact]
        public void AddAffType_GET_WhenCall_ReturnCorrectView()
        {
            //Arrange

            //Action
            var controller = CreateController();
            var result = controller.AddAffType() as ViewResult;

            //Assert
            Assert.Equal("", result.ViewName);
        }

        [Theory]
        [InlineData("")]
        [InlineData("name")]
        public void AddAffType_POST_WhenCall_ReturnView(string name)
        {
            //Arrange
            var httpRequest = MockingHelpers.GetHttpRequest();
            MockingHelpers.SetUpEmptyHttpContextSession();

            //Action
            var controller = CreateController();
            if (string.IsNullOrEmpty(name))
            {
                controller.ModelState.AddModelError("Error01", "error");
            }
            controller.ControllerContext = MockingHelpers.GetControllerContext(httpRequest);
            var result = controller.AddAffType(new AffTypeAddCriteria { Name = name }) as ViewResult;

            //Assert
            Assert.Equal("", result.ViewName);
        }

        [Theory]
        [InlineData("", 1003)]
        [InlineData("1,2,3", 0)]
        public void DeleteAffType_WhenCall_ReturnCorrectData(string idList, int errCode)
        {
            //Arrange
            var httpRequest = MockingHelpers.GetHttpRequest();
            MockingHelpers.SetUpEmptyHttpContextSession();

            //Action
            var controller = CreateController();
            if(string.IsNullOrEmpty(idList))
            {
                controller.ModelState.AddModelError("Error01", "error");
            }
            controller.ControllerContext = MockingHelpers.GetControllerContext(httpRequest);
            var result = controller.DeleteAffType(new AffTypeDeleteCriteria { IDList = idList }) as JsonResult;
            var data = result.Data as ResultViewModel;

            //Assert
            Assert.Equal(errCode, data.ErrorCode);
        }

        private ConfigurationController CreateController() =>
            new ConfigurationController(_appSetting, _systemSettingService);
    }
}
