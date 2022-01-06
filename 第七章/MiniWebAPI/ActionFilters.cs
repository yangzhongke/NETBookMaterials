using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniWebAPI
{
    public class ActionFilters
    {
        public static List<IMyActionFilter> Filters = new List<IMyActionFilter>();
    }

    public interface IMyActionFilter
    {
        void Execute();
    }
}
