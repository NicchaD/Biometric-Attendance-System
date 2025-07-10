using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YMCAObjects
{
    public abstract class YMCAEventArgs
    {
        public bool IsSucceeded { get; set; }

        public abstract override string ToString();
    }
}
