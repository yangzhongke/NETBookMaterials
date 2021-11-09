using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI1.Models
{
    public class Person
    {
        public string Name { get; set; }
        public bool IsVIP { get; set; }
        public DateTime CreatedTime { get; set; }
    }
}
