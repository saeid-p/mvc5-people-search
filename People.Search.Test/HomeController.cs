using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using HealthCatalyst.Controllers;
using HealthCatalyst.LogicLayer;
using HealthCatalyst.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoMoq;

namespace People.Search.Test
{
    [TestClass]
    public class ProductControllerTest
    {
        private readonly IFixture _fixture = new Fixture().Customize(new AutoMoqCustomization());

        [TestMethod]
        public async Task IndexActionTest()
        {
            // Arrange
            var mockModel = GetMockPeopleList();

            var mockLogic = new Mock<IPeopleLogic>();
            mockLogic.Setup(logic => logic.GetPeopleAsync(null))
                .Returns(Task.FromResult(mockModel));

            var controller = new HomeController(mockLogic.Object);

            // Act
            var actionView = await controller.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(actionView);
            Assert.IsNotNull(actionView.Model);
            Assert.AreEqual(JsonConvert.SerializeObject(mockModel), actionView.Model);
            Assert.IsInstanceOfType(actionView.Model, typeof(string));
        }

        [TestMethod]
        public async Task SearchActionTest()
        {
            // Arrange
            var mockSearchQuery = "Mock Search Query";
            var mockModel = GetMockPeopleList();
            
            var mockLogic = new Mock<IPeopleLogic>();
            mockLogic.Setup(logic => logic.GetPeopleAsync(mockSearchQuery))
                .Returns(Task.FromResult(mockModel));

            var controller = new HomeController(mockLogic.Object);

            // Act
            var actionView = await controller.Search(mockSearchQuery);

            // Assert
            Assert.IsNotNull(actionView);
            Assert.IsNotNull(actionView.Data);
            Assert.AreEqual(mockModel, actionView.Data);
            Assert.IsInstanceOfType(actionView.Data, typeof(List<PersonModel>));
        }

        private List<PersonModel> GetMockPeopleList() => 
            _fixture.CreateMany<PersonModel>().ToList();

        [TestMethod]
        public async Task GetPersonPictureActionTest()
        {
            // Arrange
            const int mockPersonId = 1;
            var mockModel = GetMockPersonPicture();

            var mockLogic = new Mock<IPeopleLogic>();
            mockLogic.Setup(logic => logic.GetPersonPictureAsync(mockPersonId))
                .Returns(Task.FromResult(mockModel));

            var controller = new HomeController(mockLogic.Object);

            // Act
            var actionView = await controller.GetPersonPicture(mockPersonId);

            // Assert
            Assert.IsNotNull(actionView);
            Assert.IsNotNull(actionView.FileContents);
            Assert.AreEqual(mockModel, actionView.FileContents);
        }

        private byte[] GetMockPersonPicture() => 
            _fixture.Create<byte[]>();

        [TestMethod]
        public void AddPersonModalActionTest()
        {
            // Arrange
            var mockLogic = new Mock<IPeopleLogic>();
            var controller = new HomeController(mockLogic.Object);

            // Act
            var actionView = controller.AddPersonModal() as PartialViewResult;

            // Assert
            Assert.IsNotNull(actionView);
            Assert.IsNull(actionView.Model);
        }

        [TestMethod]
        public async Task EditPersonPicture_NoPictureSelected_Test()
        {
            // Arrange
            const int mockPersonId = 1;
            var mockPicture = GetMockPersonPicture();

            var request = new Mock<HttpRequestBase>();
            request.SetupGet(x => x.Files).Returns(() => null);

            var context = new Mock<HttpContextBase>();
            context.SetupGet(x => x.Request).Returns(request.Object);

            var mockLogic = new Mock<IPeopleLogic>();
            mockLogic.Setup(logic => logic.EditPersonPicture(mockPersonId, mockPicture))
                .Returns(Task.CompletedTask);

            var controller = new HomeController(mockLogic.Object);
            controller.ControllerContext = 
                new ControllerContext(context.Object, new RouteData(), controller);
            
            // Act
            var actionView = await controller.EditPersonPicture(mockPersonId);

            // Assert
            Assert.IsNotNull(actionView);
        }

        [TestMethod]
        public async Task EditPersonPicture_PictureSelected_Test()
        {
            // Arrange
            const int mockPersonId = 1;
            var mockPicture = GetMockPersonPicture();

            var mockPictureFile = new Mock<HttpPostedFileBase>();
            mockPictureFile.Setup(x => x.ContentLength).Returns(mockPicture.Length);
            mockPictureFile.Setup(x => x.ContentType).Returns("image/jpg");
            mockPictureFile.Setup(x => x.InputStream).Returns(new MemoryStream(mockPicture));

            var requestFiles = new Mock<HttpFileCollectionBase>();
            requestFiles.Setup(x => x.Count).Returns(1);
            requestFiles.Setup(x => x["Picture"]).Returns(mockPictureFile.Object);

            var request = new Mock<HttpRequestBase>();
            request.SetupGet(x => x.Files).Returns(requestFiles.Object);

            var context = new Mock<HttpContextBase>();
            context.SetupGet(x => x.Request).Returns(request.Object);

            var mockLogic = new Mock<IPeopleLogic>();
            mockLogic.Setup(logic => logic.EditPersonPicture(mockPersonId, mockPicture))
                .Returns(Task.CompletedTask);

            var controller = new HomeController(mockLogic.Object);
            controller.ControllerContext =
                new ControllerContext(context.Object, new RouteData(), controller);

            // Act
            var actionView = await controller.EditPersonPicture(mockPersonId);

            // Assert
            Assert.IsNotNull(actionView);
        }
    }
}