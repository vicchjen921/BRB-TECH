using Application.DTO;
using Domain.Entities;
using MediatR;

namespace Application.Features.GetMonthlyBalance
{
    public class GetMonthlyBalanceRequest : IRequest<MonthlyBalance>
    {
        public GetMonthlyBalanceDTO _getMonthlyBalance { get; set; }

        public GetMonthlyBalanceRequest(GetMonthlyBalanceDTO getMonthlyBalance)
        {
            _getMonthlyBalance = getMonthlyBalance;
        }
    }
}
