using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Honeycomb.Projections
{
    public class VersionedProjection<T> where T : Projection
    {
        public readonly Guid identity;
        public readonly Projection projection;

        public VersionedProjection(Guid identity, T projection)
        {
            this.identity = identity;
            this.projection = projection;
        }
    }
}
