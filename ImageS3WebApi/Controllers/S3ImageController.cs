using ImageS3WebApi.Handlers;
using System.Threading.Tasks;
using System.Web.Http;

namespace ImageS3WebApi.Controllers
{
    public class S3ImageController : ApiController
    {
        [HttpGet]
		//http://localhost:portnumber/api/s3image/getimageid?referenceId=your_rid_from_guid&entryId=your_eid_from_guid
        public async Task<string> GetImageBase64(string referenceId, string entryId)
        {
            if (string.IsNullOrEmpty(referenceId) || string.IsNullOrEmpty(entryId))
            {
                return string.Empty;
            }

            var handle = new S3Sqlite3Handler();

            string imageId = await handle.S3Sqlite3CodingRequestImageId(referenceId, entryId);

            string fileBase64Code = await handle.S3Sqlite3GetImageBase64(imageId);
            
            return fileBase64Code;
        }
    }
}