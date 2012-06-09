using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Honeycomb
{
    public class IdentityGenerator
    {
        public Guid NewId()
        {
            return Guid.NewGuid();
        }
    }
}
