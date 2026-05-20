using MediatR;
using MenuNews.Application.Features.Menus.Commands.CreateMenu;
using MenuNews.Application.Features.Menus.Commands.DeleteMenu;
using MenuNews.Application.Features.Menus.Commands.UpdateMenu;
using MenuNews.Application.Features.Menus.Queries.GetAllMenus;
using MenuNews.Application.Features.Menus.Queries.GetMenuById;
using MenuNews.Domain.Common;
using Microsoft.AspNetCore.Mvc;

namespace MenuNews.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MenusController : ControllerBase
{
    private readonly IMediator _mediator;

    public MenusController(IMediator mediator) => _mediator = mediator;

    [HttpPost]
    public async Task<ActionResult<ApiResponse<object>>> Create([FromBody] CreateMenuCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(ApiResponse<object>.Ok(result, "Tạo Menu thành công."));
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<object>>> GetAll([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        var result = await _mediator.Send(new GetAllMenusQuery(pageNumber, pageSize));
        return Ok(ApiResponse<object>.Ok(result));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ApiResponse<object>>> GetById(int id)
    {
        var result = await _mediator.Send(new GetMenuByIdQuery(id));
        return Ok(ApiResponse<object>.Ok(result));
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<ApiResponse<object>>> Update(int id, [FromBody] UpdateMenuRequest request)
    {
        var result = await _mediator.Send(new UpdateMenuCommand(id, request.Name, request.Description));
        return Ok(ApiResponse<object>.Ok(result, "Cập nhật Menu thành công."));
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult<ApiResponse<object>>> Delete(int id)
    {
        await _mediator.Send(new DeleteMenuCommand(id));
        return Ok(ApiResponse<object>.Ok(null!, "Xóa Menu thành công (soft delete)."));
    }
}

public class UpdateMenuRequest
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
}
