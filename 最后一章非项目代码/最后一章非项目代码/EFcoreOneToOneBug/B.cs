using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFcoreOneToOneBug
{
    class B
    {
        public Guid Id { get; set; }
        public int Age { get; set; }
        public A A { get; set; }
    }
}
