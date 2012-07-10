using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DDD.Aggregate
{
    public interface AggregateReference<AggregateRootType> where AggregateRootType : AggregateRoot
    {
        AggregateRootType GetAggregateRoot();
    }
}
