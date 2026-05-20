using MediatR;
using MenuNews.Application.Features.MenuNews.Commands.AssignNewsToMenu;
using MenuNews.Application.Features.MenuNews.Queries.GetMenusByNews;
using MenuNews.Application.Features.MenuNews.Queries.GetNewsByMenu;
using MenuNews.Domain.Common;
using Microsoft.AspNetCore.Mvc;

namespace MenuNews.Api.Controllers;

[ApiController]
[Route("api/menu-news")]
public class MenuNewsController : ControllerBase
{
    private readonly IMediator _mediator;

    public MenuNewsController(IMediator mediator) => _mediator = mediator;

    //Gán News vào Menu (Many-to-Many)
    [HttpPost("assign")]
    public async Task<ActionResult<ApiResponse<object>>> Assign([FromBody] AssignNewsToMenuCommand command)
    {
        await _mediator.Send(command);
        return Ok(ApiResponse<object>.Ok(null!, "Gán News vào Menu thành công."));
    }

    //Lấy danh sách News theo MenuId
    [HttpGet("menu/{menuId:int}/news")]
    public async Task<ActionResult<ApiResponse<object>>> GetNewsByMenu(int menuId)
    {
        var result = await _mediator.Send(new GetNewsByMenuQuery(menuId));
        return Ok(ApiResponse<object>.Ok(result));
    }

    //Lấy danh sách Menu theo NewsId
    [HttpGet("news/{newsId:int}/menus")]
    public async Task<ActionResult<ApiResponse<object>>> GetMenusByNews(int newsId)
    {
        var result = await _mediator.Send(new GetMenusByNewsQuery(newsId));
        return Ok(ApiResponse<object>.Ok(result));
    }
}
