using MassTransit;

namespace Contracts.Money
{
    public class GetMoneyConsumer : IConsumer<IGetMoneyRequest>
    {
        public async Task Consume(ConsumeContext<IGetMoneyRequest> context)
        {
            using var stream = File.Create(@"C:\Users\maksg\Desktop\111.txt");
            using var writer = new StreamWriter(stream);

            writer.Write(context.Message.Address);
            await context.Publish<IGetMoneyResponse>(new GetMoneyResponse() { OrderId = context.Message.OrderId, Message = "123", Address = new Uri(context.Message.Address) });
        }

    }
}
