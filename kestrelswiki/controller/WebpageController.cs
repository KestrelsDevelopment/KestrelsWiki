using System.Collections.Generic;
using kestrelswiki.environment;
using kestrelswiki.logging.logFormat;
using kestrelswiki.logging.loggerFactory;
using kestrelswiki.service.article;
using kestrelswiki.util;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;

namespace kestrelswiki.controller;

[ApiController]
public class WebpageController(
    ILoggerFactory loggerFactory,
    IContentTypeProvider contentTypeProvider,
    IArticleService articleService)
    : KestrelsController(loggerFactory, LogDomain.WebpageController)
{
    protected Dictionary<string, WebpageInfo> pages = new DictionaryBuilder<string, WebpageInfo>()
        .Add("", new(Variables.Webpage.HomeDirectory))
        .Add("wiki", new(Variables.Webpage.FrontpageDirectory))
        .Add("wiki/*path", new(Variables.Webpage.FrontpageDirectory))
        .Add("not-found", new(Variables.Webpage.FrontpageDirectory))
        .Build();

    /*
     * Get webpage html at /, /wiki, /wiki/path
     * Get file at path
     *      File path starts after web path
     *      e.g.
     *          /js/scroll.js -> home/js/scroll.js
     *          /wiki/css/style.css -> frontpage/css/style.css
     *          /wiki/*article/css/style.css -> article/css/style.css
     * Get not found as fallback
     *
     */

    [HttpGet("")]
    public ActionResult GetHomepage()
    {
        return File(pages[""].HtmlPath, "text/html");
    }

    [HttpGet("wiki")]
    public ActionResult GetWikiFrontpage()
    {
        return File(pages["wiki"].HtmlPath, "text/html");
    }

    [HttpGet("wiki/*path")]
    public ActionResult GetWikiArticlePage4()
    {
        return File(pages["wiki/*path"].HtmlPath, "text/html");
    }

    protected ActionResult GetFile(string path)
    {
        contentTypeProvider.TryGetContentType(path, out string? contentType);
        return File(path, contentType ?? "application/octet-stream");
    }

    protected class WebpageInfo(string dirPath)
    {
        public string DirPath => Path.Combine(Variables.WebRootPath, dirPath);
        public string HtmlPath => Path.Combine(DirPath, "index.html");
    }
}