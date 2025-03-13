using kestrelswiki.logging.logFormat;
using Microsoft.AspNetCore.Mvc;
using ILoggerFactory = kestrelswiki.logging.loggerFactory.ILoggerFactory;

namespace kestrelswiki.api.controller;

[ApiController]
public class NotFoundController(ILoggerFactory loggerFactory) : KestrelsController(loggerFactory, LogDomain.Logging)
{
    [HttpGet("")]
    public ActionResult Get()
    {
        LogIncomingRequest();
        return NotFound();
    }
}