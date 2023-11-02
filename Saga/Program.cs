using Saga;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Saga.Sagas.FirstState;
using Saga.Sagas.SecondState;
using Contracts.Money;
using MassTransit.EntityFrameworkCoreIntegration;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
{
    services.AddDbContext<SagasDbContext>(options =>
       options.UseSqlite("DataSource=app.db;Cache=Shared"));

    services.AddMassTransit(cfg =>
    {
        cfg.SetKebabCaseEndpointNameFormatter();
        cfg.AddDelayedMessageScheduler();

        cfg.AddSagaStateMachine<FirstStateMachine, FirstState>()
                .EntityFrameworkRepository(r =>
                {
                    r.ExistingDbContext<SagasDbContext>();
                    r.LockStatementProvider = new SqliteLockStatementProvider();
                });

        cfg.AddSagaStateMachine<SecondStateMachine, SecondState>()
                .EntityFrameworkRepository(r =>
                {
                    r.ExistingDbContext<SagasDbContext>();
                    r.LockStatementProvider = new SqliteLockStatementProvider();
                });

        cfg.UsingRabbitMq((brc, rbfc) =>
        {
            rbfc.UseInMemoryOutbox();
            //rbfc.UseMessageRetry(r => { r.Incremental(3, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1)); });
            rbfc.UseDelayedMessageScheduler();
            rbfc.Host("localhost", "/", h =>
            {
                h.Username("rmuser");
                h.Password("rmpassword");
            });

            rbfc.ConfigureEndpoints(brc);
        });
    });

})
    .Build();

host.Run();
