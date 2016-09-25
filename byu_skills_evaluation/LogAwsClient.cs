using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Transfer;
using System.IO;

namespace byu_skills_evaluation
{
    internal class LogAwsClient
    {
        private readonly TransferUtility utility;

        internal LogAwsClient(string accessKey, string secretKey)
        {
            AWSCredentials c = new BasicAWSCredentials(accessKey, secretKey);
            AmazonS3Client awsS3client = new AmazonS3Client(c, RegionEndpoint.USEast1);
            utility = new TransferUtility(awsS3client);
            awsS3client.Dispose();
        }

        internal void Log(string bucket, string objectName)
        {
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.Write("Hello!");
            writer.Flush();
            stream.Position = 0;
            utility.Upload(stream, bucket, objectName);
        }
    }
}