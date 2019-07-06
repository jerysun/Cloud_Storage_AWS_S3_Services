using ImageS3WebApi.Services;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace ImageS3WebApi.Handlers
{
    public class S3Sqlite3Handler
    {
        public async Task<string> S3Sqlite3GetImageBase64(string referenceId)
        {
            const string bucketKey = "your_bucket_name";//a S3 bucket storing images;

            S3SqliteImgFile.Create(bucketKey);
            S3SqliteImgFile s3SqliteImgFile = S3SqliteImgFile.Instance;

            string fileBase64 = await s3SqliteImgFile.GetFileBase64FromS3Bucket(bucketKey, referenceId);
            return fileBase64;
        }

        public async Task S3Sqlite3GetImage(string referenceId, string outputFile)
        {
            const string bucketKey = "your_bucket_name";//a S3 bucket storing images;

            S3SqliteImgFile.Create(bucketKey);
            S3SqliteImgFile s3SqliteImgFile = S3SqliteImgFile.Instance;

            await s3SqliteImgFile.GetFileFromS3Bucket(bucketKey, referenceId, outputFile);
        }

        public async Task<string> S3Sqlite3CodingRequestImageId(string referenceId, string entryId)
        {
            const string bucketKey = "your_bucket_name_sqlite3_files";
            string imageId = string.Empty;

            S3SqliteDBFile.Create(bucketKey);
            S3SqliteDBFile s3SqliteDbFile = S3SqliteDBFile.Instance;

            string dbFile = await s3SqliteDbFile.GetFileFromS3Bucket(s3SqliteDbFile.BucketName, referenceId);

            using (SQLiteConnection conn = s3SqliteDbFile.CreateConnection(dbFile))
            {
                if (conn != null)
                {
                    imageId = s3SqliteDbFile.GetImageID(conn, entryId);
                }
            }

            return imageId;
        }
    }
}