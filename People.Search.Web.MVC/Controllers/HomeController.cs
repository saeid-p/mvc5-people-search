using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Mvc;
using HealthCatalyst.LogicLayer;
using HealthCatalyst.Models;
using HealthCatalyst.Utilities;
using Newtonsoft.Json;

namespace HealthCatalyst.Controllers
{
    public class HomeController : BaseController
    {
        private IPeopleLogic Logic { get; }

        /// <summary>
        /// Controller constructor called from the MVC pipeline.
        /// </summary>
        public HomeController()
        {
            using (DependencyContainer.BeginScope())
            {
                Logic = DependencyContainer.GetInstance<IPeopleLogic>();
            }
        }

        /// <summary>
        /// Custom controller to be used for unit testing.
        /// </summary>
        /// <param name="logic">Mocked business logic instance.</param>
        public HomeController(IPeopleLogic logic)
        {
            Logic = logic;
        }

        [HttpGet]
        public async Task<ActionResult> Index() =>
            View(model: JsonConvert.SerializeObject(await Logic.GetPeopleAsync()));

        [Route("Search/{query}")]
        [HttpPost]
        public async Task<JsonStringResult> Search(string query)
        {
            var model = await Logic.GetPeopleAsync(query);
            if (model.Any()) Thread.Sleep(2000);
            return JsonStringResult.Build(model);
        }

        [HttpGet]
        [OutputCache(CacheProfile = "ImagesCachingProfile")]
        public async Task<FileContentResult> GetPersonPicture(int id)
        {
            var avatarBytes = await Logic.GetPersonPictureAsync(id);
            if (avatarBytes == null || avatarBytes.Length == 0)
                avatarBytes = System.IO.File.ReadAllBytes(Server.MapPath("~/App_Data/person-avatar.png"));
            return new FileContentResult(avatarBytes, "image/png");
        }

        [HttpGet]
        public ActionResult AddPersonModal() => PartialView();

        [HttpPost, ValidateModelState]
        public async Task<JsonStringResult> AddPerson(PersonModel model)
        {
            var jsonResult = new LogicValidationResult
            {
                NewId = (await Logic.AddPersonAsync(model)).ToString()
            };
            return JsonStringResult.Build(jsonResult.AjaxResult);
        }

        [HttpPost]
        public async Task<JsonStringResult> EditPersonPicture(int id)
        {
            if (Request.Files != null && Request.Files.AllKeys.Contains("Picture"))
            {
                var avatarFile = Request.Files["Picture"];
                if (avatarFile != null && avatarFile.ContentLength > 0 && avatarFile.HasValidImageFormat())
                    await Logic.EditPersonPicture(id, avatarFile.GetBytes());
            }

            return JsonStringResult.Build(new LogicValidationResult().AjaxResult);
        }
    }
}