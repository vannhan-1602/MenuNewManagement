using AutoMapper;
using MenuNews.Application.Features.Menus.Dtos;
using MenuNews.Application.Features.News.Dtos;
using MenuNews.Domain.Entities;

namespace MenuNews.Application.Common.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Menu, MenuDto>();
        CreateMap<NewsItem, NewsDto>();
    }
}
