namespace Contracts.Money
{
    public interface IGetMoneyResponse
    {
        public Guid OrderId { get; set; }
        public string Message { get; set; }
        public Uri Address { get; set; }
    }

    public class GetMoneyResponse
    {
        public Guid OrderId { get; set; }
        public string? Message { get; set; }
        public Uri? Address { get; set; }
    }
}
