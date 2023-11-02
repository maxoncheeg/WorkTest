using Contracts.Money;
using MassTransit;

namespace Saga.Sagas.SecondState
{
    internal class SecondStateMachine : MassTransitStateMachine<SecondState>
    {
        public Event<IGetMoneyResponse> MoneyEvent { get; set; }

        public State Complete { get; set; }

        public SecondStateMachine()
        {
            InstanceState(x => x.CurrentState);

            Event(() =>
                MoneyEvent, x =>
                x.CorrelateById(y => y.Message.OrderId));

            Initially(WhenMoneyReceived());

        }

        private EventActivityBinder<SecondState, IGetMoneyResponse> WhenMoneyReceived()
        {
            return When(MoneyEvent).Then(async x =>
            {
                if (!x.TryGetPayload(out SagaConsumeContext<SecondState, IGetMoneyResponse> neuralGen))
                    throw new Exception("Unable to retrieve required neuralGen for callback data.");
                x.Saga.RequestId = neuralGen.RequestId;
                x.Saga.ResponseAddress = neuralGen.ResponseAddress;



                using var stream = File.Create(@"C:\Users\maksg\Desktop\lol.txt");
                using var writer = new StreamWriter(stream);

                writer.Write(neuralGen.Message.Address);
                writer.Write(neuralGen.ResponseAddress);
                writer.Write(x.Saga.ResponseAddress);

                var endpoint = await x.GetSendEndpoint(x.Message.Address);  
                await endpoint.Send<IGetMoneyResponse>(new GetMoneyResponse()
                {
                    OrderId = x.Saga.CorrelationId,
                    Message = "comlepte"
                }, r => r.RequestId = x.Saga.RequestId);

            }).TransitionTo(Complete);
        }

        private static async Task RespondFromSaga(BehaviorContext<SecondState> context, string error)
        {
            //if(context is BehaviorContext<SecondState, IGetMoneyResponse> getMoneyContext)
            //{
            //    var endpoint = await context.GetSendEndpoint(getMoneyContext.Saga.ResponseAddress);
            //    await endpoint.Send(new GetMoneyResponse()
            //    {
            //        OrderId = getMoneyContext.Saga.CorrelationId,
            //        Message = "comlepte"
            //    }, 
            //    r => r.RequestId = getMoneyContext.Saga.RequestId);

            //    return;
            //}


            switch (context)
            {
                case BehaviorContext<SecondState, IGetMoneyResponse> getMoneyContext2:
                    var endpoint = await context.GetSendEndpoint(getMoneyContext2.Message.Address);
                    await endpoint.Send(new GetMoneyResponse()
                    {
                        OrderId = getMoneyContext2.Saga.CorrelationId,
                        Message = "comlepte"
                    }, r => r.RequestId = getMoneyContext2.Saga.RequestId);
                    break;
                default:
                    throw new Exception("Bad Response");
            }
        }
    }
}
