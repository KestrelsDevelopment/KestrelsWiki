using kestrelswiki.environment;
using kestrelswiki.logging.logFormat;
using kestrelswiki.logging.loggerFactory;
using kestrelswiki.service.article;
using kestrelswiki.service.webpage;
using Microsoft.AspNetCore.Mvc;

namespace kestrelswiki.api.controller;

[ApiController]
public class WebpageController(
    ILoggerFactory loggerFactory,
    IArticleService articleService,
    IWebpageService webpageService)
    : KestrelsController(loggerFactory, LogDomain.WebpageController)
{
    protected WebpageInfo ArticlePage = new(Variables.Webpage.ArticleDirectory);
    protected WebpageInfo FrontPage = new(Variables.Webpage.FrontpageDirectory);
    protected WebpageInfo HomePage = new(Variables.Webpage.HomeDirectory);
    protected WebpageInfo NotFoundPage = new(Variables.Webpage.NotFoundDirectory);

    /// <summary>
    ///     The homepage at /
    /// </summary>
    [HttpGet("")]
    public ActionResult GetHomepage()
    {
        return File(HomePage.HtmlPath) ?? GetNotFoundPage();
    }

    /// <summary>
    ///     A file relative to /
    /// </summary>
    [HttpGet("{*path}")]
    public ActionResult GetHomepageFile(string path)
    {
        return File(Path.Combine(HomePage.DirPath, path)) ?? GetNotFoundPage();
    }

    /// <summary>
    ///     The wiki frontpage at /wiki/
    /// </summary>
    [HttpGet("wiki")]
    public ActionResult GetWikiFrontpage()
    {
        return File(FrontPage.HtmlPath) ?? GetNotFoundPage();
    }

    /// <summary>
    ///     A file relative to /wiki/
    ///     Or a file relative to /wiki/*article/ if that does not exist
    ///     Or the article page at /wiki/article/ if that does not exist
    ///     Or the not found page if no article exists at this path
    /// </summary>
    [HttpGet("wiki/{*path}")]
    public ActionResult GetWikiArticlePage(string path)
    {
        return File(Path.Combine(FrontPage.DirPath, path))
               ?? File(Path.Combine(ArticlePage.DirPath, path))
               ?? (articleService.Exists(path) ? File(ArticlePage.HtmlPath) : GetNotFoundPage())
               ?? GetNotFoundPage();
    }


    /// <summary>
    ///     A global file relative to /global
    /// </summary>
    [HttpGet("global/{*path}")]
    public ActionResult GetGlobalFile(string path)
    {
        return File(Path.Combine(
            Directory.GetCurrentDirectory(),
            Variables.WebRootPath,
            Variables.Webpage.GlobalFileDirectory,
            path)
        ) ?? GetNotFoundPage();
    }

    protected ActionResult? File(string physicalPath)
    {
        return webpageService.TryGetFile(physicalPath).Result;
    }

    protected ActionResult GetNotFoundPage()
    {
        return File(NotFoundPage.HtmlPath) ?? NotFound();
    }

    protected class WebpageInfo(string dirPath)
    {
        public string DirPath { get; } = Path.Combine(Directory.GetCurrentDirectory(), Variables.WebRootPath, dirPath);
        public string HtmlPath => Path.Combine(DirPath, "index.html");
    }
}