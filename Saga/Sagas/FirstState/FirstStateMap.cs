using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MassTransit;

namespace Saga.Sagas.FirstState
{
    internal class FirstStateMap : SagaClassMap<FirstState>
    {
        protected override void Configure(EntityTypeBuilder<FirstState> entity, ModelBuilder model)
        {
            base.Configure(entity, model);
            entity.Property(x => x.CurrentState).HasMaxLength(255);
        }
    }
}
