using MassTransit;

namespace Contracts.Money
{
    public interface IGetMoneyRequest
    {
        public Guid OrderId { get; set; }
        public string Address { get; set; }
    }

    public class GetMoneyRequest : IGetMoneyRequest
    {
        public Guid OrderId { get; set; }
        public string? Address { get; set; }
    }

}
