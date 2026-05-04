using MaxEndLabs.GCommon.Exceptions;
using MaxEndLabs.Service.Models.NewsArticle;
using MaxEndLabs.Services.Core.Contracts;
using MaxEndLabs.ViewModels.NewsArticles;
using Microsoft.AspNetCore.Mvc;

namespace MaxEndLabs.Web.Areas.Admin.Controllers
{
    public class NewsManagementController : BaseAdminController
    {
        private readonly INewsArticleService _newsService;

        public NewsManagementController(INewsArticleService newsService)
        {
            _newsService = newsService;
        }

        public IActionResult Index()
        {
            return Ok("entered");
        }

        [HttpGet]
        public IActionResult Create()
        {
            try
            {
                return View();
            }
            catch (EntityNotFoundException e)
            {
                return NotFound();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create(NewsArticleFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                View(model);
            }

            try
            {
                var newsCreateDto = new NewsArticleDto()
                {
                    Content = model.Content,
                    ContentTitle = model.ContentTitle,
                    TeaserTitle = model.TeaserTitle,
                    CoverImageUrl = model.CoverImageUrl,
                    ArticleImageUrl = model.ArticleImageUrl,
                    Summary = model.Summary
                };

                await _newsService.AddNewsArticle(newsCreateDto);

                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
