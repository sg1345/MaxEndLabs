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

        public async Task<IActionResult> Index(string searchTerm = "", int page = 1)
        {
            try
            {
                var productDto = await _newsService.GetNewsArticleSummariesAsync();

                var model = new NewsArticlePaginationViewModel
                {
                    SearchTerm = searchTerm,
                    CurrentPage = 3,
                    TotalPages = 10,
                    HasNextPage = true,
                    HasPreviousPage = true,
                    Articles = productDto.Select(a => new NewsArticleSummaryViewModel
                    {
                        Id = a.Id,
                        CoverImageUrl = a.CoverImageUrl,
                        TeaserTitle = a.TeaserTitle,
                        Summary = a.Summary
                    })
                };

                ViewBag.CurrentPage = page;
                return View(model);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
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

                //add TempData for the Output Message
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
