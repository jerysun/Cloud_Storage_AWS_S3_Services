using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using ImageS3WebApi.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace ImageS3WebApi.Infrastructures
{
    public abstract class S3SqliteAbstract : IS3SqliteLib
    {
        private IAmazonS3 GetObjectFromS3Bucket(string bucketName, string referenceId, ref GetObjectRequest downloadRequest)
        {
            string accessKey = "your_accessKey";
            string secretKey = "your_secretKey";
            string regionEndpoint = "your_vm_region";
            bool useHttp = false;
            IAmazonS3 s3Client;
            AmazonS3Config s3Config;

            if (string.IsNullOrEmpty(referenceId))
            {
                return null;
            }


            try
            {
                s3Config = new AmazonS3Config()
                {
                    RegionEndpoint = RegionEndpoint.GetBySystemName(regionEndpoint),
                    UseHttp = useHttp
                };

                s3Client = new AmazonS3Client(accessKey, secretKey, s3Config);
            }
            catch (AmazonS3Exception)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new BlobStoreException($"Technical error initializing AmazonS3 client.", e);
            }

            s3Client = new AmazonS3Client(
                    accessKey,
                    secretKey,
                    s3Config
                    );


            //Download a s3 object to a file
            downloadRequest.BucketName = bucketName;
            downloadRequest.Key = referenceId;

            return s3Client;
        }

        public async Task<string> GetFileFromS3Bucket(string bucketName, string referenceId, string outputFile = "")
        {
            GetObjectRequest downloadRequest = new GetObjectRequest();
            IAmazonS3 s3Client = GetObjectFromS3Bucket(bucketName, referenceId, ref downloadRequest);
            GetObjectResponse downloadResponse = null;
            string s3File = string.Empty;

            s3File = string.IsNullOrEmpty(outputFile) ? Path.GetTempFileName() : outputFile;

            try
            {
                await Task.Run(() =>
                {
                    downloadResponse = s3Client.GetObject(downloadRequest);
                });

                downloadResponse.WriteResponseStreamToFile(s3File);
            }
            catch (Exception)
            {
                return s3File;
            }
            finally
            {
                if (downloadResponse != null)
                {
                    downloadResponse.Dispose();
                }
            }

            return s3File;
        }

        public async Task<string> GetFileBase64FromS3Bucket(string bucketName, string referenceId)
        {
            GetObjectRequest downloadRequest = new GetObjectRequest();
            IAmazonS3 s3Client = GetObjectFromS3Bucket(bucketName, referenceId, ref downloadRequest);
            GetObjectResponse downloadResponse = null;
            string fileBase64 = string.Empty;

            try
            {
                await Task.Run(() =>
                {
                    downloadResponse = s3Client.GetObject(downloadRequest);
                });

                Stream fileStream = downloadResponse.ResponseStream;
                var bytes = ReadFully(fileStream);
                fileBase64 = Convert.ToBase64String(bytes);
            }
            catch (Exception)
            {
                return fileBase64;
            }
            finally
            {
                if (downloadResponse != null)
                {
                    downloadResponse.Dispose();
                }
            }

            return fileBase64;
        }

        private byte[] ReadFully(Stream input)
        {
            byte[] buffer = new byte[16 * 1024];

            using (MemoryStream ms = new MemoryStream())
            {
                int read;

                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }

                return ms.ToArray();
            }
        }
    }
}