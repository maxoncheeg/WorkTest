using MassTransit.EntityFrameworkCoreIntegration;
using Microsoft.EntityFrameworkCore;
using Saga.Sagas.FirstState;
using Saga.Sagas.SecondState;

namespace Saga
{
    internal class SagasDbContext : SagaDbContext
    {
        public SagasDbContext(DbContextOptions options) : base(options)
        {
            Database.Migrate();
        }

        protected override IEnumerable<ISagaClassMap> Configurations => new ISagaClassMap[]
        {
        new FirstStateMap(),
        new SecondStateMap(),
        };


    }
}
