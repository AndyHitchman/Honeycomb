using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Honeycomb.Aggregate
{
    public abstract class Entity<T> where T : AggregateRoot
    {
        private T aggregateRoot;
        private int id;

        /// <summary>
        /// The aggregate root is responsible for assigning ids to entities within its aggregate
        /// </summary>
        /// <param name="aggregateRoot"></param>
        /// <param name="id"></param>
        protected Entity(T aggregateRoot, int id)
        {
            this.aggregateRoot = aggregateRoot;
            this.id = id;
        }

        public bool IsEntityForRoot(T aggregateRoot)
        {
            return aggregateRoot == this.aggregateRoot;
        }
    }
}
