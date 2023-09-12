using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TyranoCupUwpApp.Shared
{
    public class Counter
    {
        public int Count { get; private set; }

        public Counter()
        {
            Count = 0;
        }

        public void Increment()
        {
            Count++;
        }
    }
}
