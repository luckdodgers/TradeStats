using AutoMapper;
using TradeStats.Models.Domain;
using TradeStats.Views.Main;

namespace TradeStats.Services.Mappings
{
    class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Trade, TradeMergeItemDto>()
                .ForMember(dto => dto.Date, dom => dom.MapFrom(src => src.Datetime))
                .ForMember(dto => dto.Pair, dom => dom.MapFrom(src => src.FirstCurrency + "/" + src.SecondCurrency))
                .ForMember(dto => dto.Sum, dom => dom.MapFrom(src => (src.Price * src.Amount).ToString()))
                .ForMember(dto => dto.IsChecked, dom => dom.Ignore());
        }
    }
}
