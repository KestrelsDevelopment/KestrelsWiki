using System.Net.Mime;
using kestrelswiki.logging.logFormat;
using kestrelswiki.logging.loggerFactory;
using kestrelswiki.models;
using Microsoft.AspNetCore.Mvc;

namespace kestrelswiki.api.controller;

[ApiController]
[Route("search")]
public class SearchController(ILoggerFactory loggerFactory)
    : KestrelsController(loggerFactory, LogDomain.SearchController)
{
    [HttpPost]
    public ActionResult PostSearch([FromBody] SearchRequest searchRequest)
    {
        switch (searchRequest.SearchString.ToLowerInvariant())
        {
            case "one":
                Response.Headers.ContentType = MediaTypeNames.Text.Html;

                return new ObjectResult("<li href=\"./\">One link item</li>");
            case "multiple":
                Response.Headers.ContentType = MediaTypeNames.Text.Html;

                return new ObjectResult(
                    "<li href=\"./\">One link item</li><li href=\"./\">Two link items</li><li href=\"./\">Three link items</li>");
            default:
                return NotFound();
        }
    }
}