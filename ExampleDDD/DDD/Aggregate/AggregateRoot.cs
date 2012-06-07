using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DDD
{
    public abstract class AggregateRoot
    {
        protected AggregateRoot()
        {
            Id = Guid.NewGuid();
        }

        public virtual Guid Id { get; private set; }
    }
}
