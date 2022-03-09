using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Porting.Core.Decode
{
    public sealed class DecodeException : Exception
    {
        public string DecodeDescription { get; set; } = string.Empty;

        public DecodeException(string decodeDescription)
        {
            DecodeDescription = decodeDescription;
        }
    }
}
