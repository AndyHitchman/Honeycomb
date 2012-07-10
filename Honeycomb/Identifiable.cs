using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Honeycomb
{
    public interface Identifiable
    {
        Guid Identity { get; set; }
    }
}
