using System.Net;
using Domain.Dtos;
using Domain.Filters;
using Domain.Response;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[Route("[controller]")]
[Authorize]
public class QuoteController  :ControllerBase
{
    private readonly IQuoteService _quoteService;

    public QuoteController(IQuoteService quoteService)
    {
        _quoteService = quoteService;
    }

    [HttpGet("get-quotes")]
    public async Task<PagedResponse<List<GetQuoteDto>>> GetQuotes(GetQuoteFilter filter)
    {
        return await _quoteService.GetQuotes(filter);
    }
    
    
    [HttpPost("add-quote")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> AddQuote([FromBody]AddQuoteDto quoteDto)
    {
        if (ModelState.IsValid)
        {
            var response = await _quoteService.AddQuote(quoteDto);
            return StatusCode(response.StatusCode, response);
        }
        else
        {
            var errors = ModelState.SelectMany(e => e.Value.Errors.Select(er=>er.ErrorMessage)).ToList();
            var response  =  new Response<GetQuoteDto>(HttpStatusCode.BadRequest, errors);
            return StatusCode(response.StatusCode, response);
        }
        
    }
    
    
    [HttpPut("update-quote")]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<Response<GetQuoteDto>> UpdateQuote(AddQuoteDto quoteDto)
    {
        return await _quoteService.UpdateQuote(quoteDto);
    }
    
    [HttpDelete("delete-quote")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> AddQuote(int  quoteId)
    {
      var response = await _quoteService.DeleteQuote(quoteId);
      return StatusCode(response.StatusCode, response);
    }
}