using ImageS3WebApi.Infrastructures;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Web;

namespace ImageS3WebApi.Services
{
    public sealed class S3SqliteDBFile : S3SqliteAbstract
    {
        private static object mLock = new object();
        private static S3SqliteDBFile mInstance;
        public string BucketName { get; }

        private S3SqliteDBFile()
        {
            throw new Exception("It should never be called!");
        }

        private S3SqliteDBFile(string bucketName)
        {
            BucketName = bucketName;
        }

        public static S3SqliteDBFile Instance
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

                mInstance = new S3SqliteDBFile(bucketName);
            }
        }

        public SQLiteConnection CreateConnection(string dbFile)
        {
            if (string.IsNullOrEmpty(dbFile))
            {
                return null;
            }

            SQLiteConnection sqlite_conn;
            string conStr = "Data Source = " + dbFile + "; Version = 3; New = True; Compress = True;";

            sqlite_conn = new SQLiteConnection(conStr);
            try
            {
                sqlite_conn.Open();
                return sqlite_conn;
            }
            catch (Exception)
            {
                return sqlite_conn;
            }
        }

        public string GetImageID(SQLiteConnection conn, string entryId)
        {
            SQLiteDataReader sqlite_datareader;
            SQLiteCommand sqlite_cmd = null;

            sqlite_cmd = conn?.CreateCommand();
            if (sqlite_cmd == null)
            {
                return "";
            }

            sqlite_cmd.CommandText = "SELECT * FROM YourTable";
            string content;
            string contentReference = "";

            sqlite_datareader = sqlite_cmd.ExecuteReader();
            while (sqlite_datareader.Read())
            {
                string myreader = sqlite_datareader.GetString(0);
                if (string.Equals(myreader, entryId))
                {
                    content = sqlite_datareader.GetString(1);
                    var details = JObject.Parse(content);
                    contentReference = (string)details["Images"][0]["ContentReference"];
                    break;
                }
            }

            conn.Close();
            return contentReference;
        }
    }
}