using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ImageS3WebApi.Infrastructures
{
    public class BlobStoreException : Exception
    {
        public BlobStoreException(string message) : base(message)
        {

        }

        public BlobStoreException(string message, Exception exception) : base(message, exception)
        {

        }
    }
}