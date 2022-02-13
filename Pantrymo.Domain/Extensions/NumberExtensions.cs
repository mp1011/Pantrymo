using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pantrymo.Domain.Extensions
{
    public static class NumberExtensions
    {
        public static int Wrap(this int number, int maxExclusive)
        {
            var result = number % maxExclusive;
            if (result < 0)
                return result + maxExclusive;
            else
                return result;
        }
    }
}
