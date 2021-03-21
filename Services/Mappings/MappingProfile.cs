using AutoMapper;
using TradeReportsConverter.Extensions;
using TradeStats.Extensions;
using TradeStats.Models.Domain;
using TradeStats.ViewModel.DTO;

namespace TradeStats.Services.Mappings
{
    class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<OpenTrade, TradeMergeItemDto>()
                .ForMember(dto => dto.Date, dom => dom.MapFrom(src => src.Datetime))
                .ForMember(dto => dto.Pair, dom => dom.MapFrom(src => src.FirstCurrency + "/" + src.SecondCurrency))
                .ForMember(dto => dto.Price, dom => dom.MapFrom(src => src.Price.ToTableViewString()))
                .ForMember(dto => dto.Sum, dom => dom.MapFrom(src => src.Sum.ToTableViewString()))
                .ForMember(dto => dto.Amount, dom => dom.MapFrom(src => src.Amount.ToTableViewString()));
        }
    }
}
