using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Honeycomb;

namespace DDD
{
    public abstract class AggregateRoot : Identifiable
    {
        protected AggregateRoot()
        {
            Identity = Guid.NewGuid();
        }

        public virtual Guid Identity { get; set; }
    }
}
