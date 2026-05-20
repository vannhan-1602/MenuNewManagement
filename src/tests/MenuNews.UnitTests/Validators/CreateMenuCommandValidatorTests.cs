using MenuNews.Application.Features.Menus.Commands.CreateMenu;
using Xunit;

namespace MenuNews.UnitTests.Validators;

public class CreateMenuCommandValidatorTests
{
    private readonly CreateMenuCommandValidator _validator = new();

    [Fact]
    public void Should_Fail_When_Name_Empty()
    {
        var result = _validator.Validate(new CreateMenuCommand("", "desc"));
        Assert.False(result.IsValid);
    }

    [Fact]
    public void Should_Pass_When_Name_Provided()
    {
        var result = _validator.Validate(new CreateMenuCommand("Menu Demo", "Mô tả"));
        Assert.True(result.IsValid);
    }
}
