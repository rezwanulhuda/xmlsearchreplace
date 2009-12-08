using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XmlSearchReplaceLib
{
    public class BackupFailedException : BaseException
    {
        public BackupFailedException(string msg, Exception innerException)
            : base(msg, innerException)
        {
        }       
    }
}
