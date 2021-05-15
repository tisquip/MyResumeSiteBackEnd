using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyResumeSiteBackEnd.Exceptions
{
    public class BackgroundWorkerStoppedException: Exception
    {
        public BackgroundWorkerStoppedException(Type type) : base($"Background worker stopped: {type.ToString()}")
        {
        }
    }
}
