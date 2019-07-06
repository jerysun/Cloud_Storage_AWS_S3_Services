using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageS3WebApi.Interfaces
{
    public interface IS3SqliteLib
    {
        Task<string> GetFileFromS3Bucket(string bucketName, string bucketKey, string outputFile = "");
    }
}