using System.Net;
using kestrelswiki.extensions;
using kestrelswiki.logging.logFormat;
using kestrelswiki.logging.loggerFactory;
using kestrelswiki.models;
using kestrelswiki.service.article;
using Microsoft.AspNetCore.Mvc;

namespace kestrelswiki.api.controller;

[ApiController]
[Route("article")]
public class ArticleController(ILoggerFactory loggerFactory, IArticleStore articleStore)
    : KestrelsController(loggerFactory, LogDomain.ArticleController)
{
    [HttpGet("{*path}")]
    public ActionResult GetArticle(string path)
    {
        LogIncomingRequest();

        Article? article = articleStore.Get(path);

        if (article is null) return NotFound();
        if (article.Meta.MirrorOf.IsNullOrWhiteSpace()) return Ok(article);

        Response.Headers.Location = article.Meta.MirrorOf;
        return StatusCode(HttpStatusCode.PartialContent);
    }
}