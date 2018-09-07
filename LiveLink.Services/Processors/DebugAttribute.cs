using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiveLink.Services.Processors
{
    public class DebugAttribute : InjectableProcessorAttribute
    {
        public override object ProcessValue()
        {
            throw new NotImplementedException();
        }
    }
}