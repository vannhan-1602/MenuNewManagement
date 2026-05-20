using MenuNews.Application.Features.News.Commands.CreateNews;
using Xunit;

namespace MenuNews.UnitTests.Validators;

public class CreateNewsCommandValidatorTests
{
    private readonly CreateNewsCommandValidator _validator = new();

    [Fact]
    public void Should_Fail_When_Title_Too_Short()
    {
        var command = new CreateNewsCommand("abc", "Content đủ hai mươi ký tự cho validation.", "admin");
        var result = _validator.Validate(command);
        Assert.False(result.IsValid);
    }

    [Fact]
    public void Should_Pass_When_Valid()
    {
        var command = new CreateNewsCommand(
            "Tiêu đề hợp lệ",
            "Nội dung bài viết phải có ít nhất 20 ký tự để pass validation.",
            "admin");
        var result = _validator.Validate(command);
        Assert.True(result.IsValid);
    }
}
