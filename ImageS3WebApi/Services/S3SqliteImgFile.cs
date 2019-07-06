using ImageS3WebApi.Infrastructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ImageS3WebApi.Services
{
    public sealed class S3SqliteImgFile : S3SqliteAbstract
    {
        private static object mLock = new object();
        private static S3SqliteImgFile mInstance;
        public string BucketName { get; }

        private S3SqliteImgFile()
        {
            throw new Exception("It should never be called!");
        }

        private S3SqliteImgFile(string bucketName)
        {
            BucketName = bucketName;
        }

        public static S3SqliteImgFile Instance
        {
            get
            {
                if (mInstance == null)
                {
                    throw new Exception("Object not created yet!");
                }

                return mInstance;
            }
        }

        public static void Create(string bucketName)
        {
            lock (mLock)
            {
                if (mInstance != null)
                {
                    return;
                }

                mInstance = new S3SqliteImgFile(bucketName);
            }
        }
    }
}