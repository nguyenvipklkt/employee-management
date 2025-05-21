using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Common
{
    public static class CommonFunc
    {
        public static string GenerateOTP()
        {
            Random random = new Random();
            return string.Concat(Enumerable.Range(0, 6).Select(_ => random.Next(0, 10)));
        }
    }
}
