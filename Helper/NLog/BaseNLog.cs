using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;

namespace Helper.NLog
{
    public static class BaseNLog
    {
        public static Logger logger = LogManager.GetCurrentClassLogger();
    }
}
