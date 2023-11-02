using Contracts.Money;
using MassTransit;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        //services.AddDbContext<SagasDbContext>(options =>
        // options.UseSqlite(hostContext.Configuration.GetConnectionString("default")));

        services.AddMassTransit(cfg =>
        {
            cfg.SetKebabCaseEndpointNameFormatter();
            cfg.AddDelayedMessageScheduler();

            cfg.AddConsumer<GetMoneyConsumer>();

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
                rbfc.ReceiveEndpoint(ec =>
                {
                    ec.DiscardSkippedMessages();
                });
            });
        });
    }).Build();

host.Run();
