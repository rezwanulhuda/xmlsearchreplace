using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XmlSearchReplaceLib
{
    public class SaveFailedException : BaseException
    {
        public SaveFailedException(string msg, Exception innerException)
            : base(msg, innerException)
        {

        }

        
    }
}
