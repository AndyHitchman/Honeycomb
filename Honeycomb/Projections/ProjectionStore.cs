using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Honeycomb.Projections
{
    public interface ProjectionStore
    {
        /// <summary>
        /// Store a projection
        /// </summary>
        /// <param name="projection">The projection to store</param>
        /// <param name="identity">The identity of what the projection represents</param>        
        /// <returns>The version of the projection that has just been stored</returns>
        Guid StoreProjection(Projection projection);

        /// <summary>
        /// Retrieve a projection
        /// </summary>
        /// <param name="projectionType">The type of the projection to get</param>
        /// <param name="identity">The identity of what the projection represents</param>
        /// <param name="version">Optionally will try and retrieve a specific version of the projection</param>
        Projection RetrieveProjection(string identity, Guid version = new Guid());

        T RetrieveProjection<T>(string identity, Guid version = new Guid()) where T : Projection;
    }
}
