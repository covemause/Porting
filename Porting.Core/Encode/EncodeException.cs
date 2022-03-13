using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Porting.Core.Encode
{
    public class EncodeException : Exception
    {
        public string EncodeDescription { get; set; } = string.Empty;

        public EncodeException(string message)
        {
            this.EncodeDescription = message;
        }
    }
}
