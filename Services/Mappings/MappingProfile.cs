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
                .ForMember(dto => dto.FirstCurrency, dom => dom.MapFrom(src => src.FirstCurrency))
                .ForMember(dto => dto.SecondCurrency, dom => dom.MapFrom(src => src.SecondCurrency))
                .ForMember(dto => dto.Pair, dom => dom.MapFrom(src => src.FirstCurrency + "/" + src.SecondCurrency))
                .ForMember(dto => dto.Price, dom => dom.MapFrom(src => src.Price.ToTableViewPriceString()))
                .ForMember(dto => dto.Sum, dom => dom.MapFrom(src => src.Sum.ToTableViewPriceString()))
                .ForMember(dto => dto.Amount, dom => dom.MapFrom(src => src.Amount.ToTableViewPriceString()));

            CreateMap<ClosedTrade, ClosedTradeItemDto>()
                .ForMember(dto => dto.Date, dom => dom.MapFrom(src => src.Datetime))
                .ForMember(dto => dto.FirstCurrency, dom => dom.MapFrom(src => src.FirstCurrency))
                .ForMember(dto => dto.SecondCurrency, dom => dom.MapFrom(src => src.SecondCurrency))
                .ForMember(dto => dto.Pair, dom => dom.MapFrom(src => src.FirstCurrency + "/" + src.SecondCurrency))
                .ForMember(dto => dto.OpenPrice, dom => dom.MapFrom(src => src.OpenPrice.ToTableViewPriceString()))
                .ForMember(dto => dto.Sum, dom => dom.MapFrom(src => src.GetOpenSum().TwoDigitsAfterDotRoundUp()))
                .ForMember(dto => dto.ClosePrice, dom => dom.MapFrom(src => src.ClosePrice.ToTableViewPriceString()))
                .ForMember(dto => dto.ProfitPerTrade, dom => dom.MapFrom(src => src.GetPercentageProfit().TwoDigitsAfterDotRoundUp()))
                .ForMember(dto => dto.AbsProfit, dom => dom.MapFrom(src => src.GetAbsProfit().TwoDigitsAfterDotRoundUp()))
                .ForMember(dto => dto.TraderProfit, dom => dom.MapFrom(src => src.GetTraderProfit().TwoDigitsAfterDotRoundUp()))
                .ForMember(dto => dto.PureAbsProfit, dom => dom.MapFrom(src => src.GetPureAbsProfit().TwoDigitsAfterDotRoundUp()));
        }
    }
}
