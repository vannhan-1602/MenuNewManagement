using MediatR;
using MenuNews.Application.Features.News.Commands.CreateNews;
using MenuNews.Application.Features.News.Commands.DeleteNews;
using MenuNews.Application.Features.News.Commands.UpdateNews;
using MenuNews.Application.Features.News.Queries.GetAllNews;
using MenuNews.Application.Features.News.Queries.GetNewsById;
using MenuNews.Domain.Common;
using Microsoft.AspNetCore.Mvc;

namespace MenuNews.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class NewsController : ControllerBase
{
    private readonly IMediator _mediator;

    public NewsController(IMediator mediator) => _mediator = mediator;

    [HttpPost]
    public async Task<ActionResult<ApiResponse<object>>> Create([FromBody] CreateNewsCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(ApiResponse<object>.Ok(result, "Tạo News thành công - message đã gửi RabbitMQ."));
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<object>>> GetAll([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        var result = await _mediator.Send(new GetAllNewsQuery(pageNumber, pageSize));
        return Ok(ApiResponse<object>.Ok(result));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ApiResponse<object>>> GetById(int id)
    {
        var result = await _mediator.Send(new GetNewsByIdQuery(id));
        return Ok(ApiResponse<object>.Ok(result));
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<ApiResponse<object>>> Update(int id, [FromBody] UpdateNewsRequest request)
    {
        var result = await _mediator.Send(new UpdateNewsCommand(id, request.Title, request.Content, request.Author));
        return Ok(ApiResponse<object>.Ok(result, "Cập nhật News thành công."));
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult<ApiResponse<object>>> Delete(int id)
    {
        await _mediator.Send(new DeleteNewsCommand(id));
        return Ok(ApiResponse<object>.Ok(null!, "Xóa News thành công (soft delete)."));
    }
}

public class UpdateNewsRequest
{
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
}
