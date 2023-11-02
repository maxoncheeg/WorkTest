using Contracts.Money;
using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saga.Sagas.FirstState
{
    internal class FirstStateMachine : MassTransitStateMachine<FirstState>
    {
        public Request<FirstState, IGetMoneyRequest, IGetMoneyResponse> GetMoney { get; set; }
        public Event<IGetMoneyRequest> GetMoneyEvent { get; set; }

        public State Complete { get; set; }

        public FirstStateMachine()
        {

            InstanceState(x => x.CurrentState);

            Request(() => GetMoney);

            Event(() =>
                GetMoneyEvent, x =>
                x.CorrelateById(y => y.Message.OrderId));      

            Initially(WhenMoneyReceived());


            During(GetMoney.Pending, When(GetMoney.Completed).TransitionTo(Complete));
        }

        private EventActivityBinder<FirstState, IGetMoneyRequest> WhenMoneyReceived()
        {
            Uri? address = null; 
            return When(GetMoneyEvent).Then(x =>
            {
                if (!x.TryGetPayload(out SagaConsumeContext<FirstState, IGetMoneyRequest> neuralGen))
                    throw new Exception("Unable to retrieve required neuralGen for callback data.");
                x.Saga.RequestId = neuralGen.RequestId;
                x.Saga.ResponseAddress = neuralGen.ResponseAddress;
                address = x.Saga.ResponseAddress;
            }).Request(GetMoney, x => x.Init<IGetMoneyRequest>(new GetMoneyRequest()
            {
                OrderId = x.Message.OrderId,
                Address = address.ToString()
            })).TransitionTo(GetMoney.Pending);
        }
    }
}
