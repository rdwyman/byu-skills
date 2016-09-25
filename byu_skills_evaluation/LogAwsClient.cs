using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Transfer;
using System.Collections.Generic;
using System.IO;

namespace byu_skills_evaluation
{
    internal class LogAwsClient
    {
        private readonly TransferUtility utility;

        /// <summary>
        /// Builds an AWS logging client
        /// </summary>
        /// <param name="accessKey">Access key from AWS credential system (see http://docs.aws.amazon.com/general/latest/gr/managing-aws-access-keys.html)</param>
        /// <param name="secretKey">Secret key from AWS credential system (see http://docs.aws.amazon.com/general/latest/gr/managing-aws-access-keys.html)</param>
        /// <param name="endPoint">A string used to martial an endpoint via RegionEndpoint.GetBySystemName</param>
        internal LogAwsClient(string accessKey, string secretKey, string endPoint)
        {
            AWSCredentials c = new BasicAWSCredentials(accessKey, secretKey);
            AmazonS3Client awsS3client = new AmazonS3Client(c, RegionEndpoint.GetBySystemName(endPoint));
            utility = new TransferUtility(awsS3client);
        }

        /// <summary>
        /// Uploads (or overwrites) a file to S3
        /// </summary>
        /// <param name="bucket">The AWS bucket to write to</param>
        /// <param name="objectName">The AWS object (like a file name) to use. If an object with the same name already exists, it will be overwritten</param>
        /// <param name="entries">A list of strings, each of which will be in the log file</param>
        internal void Log(string bucket, string objectName, IList<string> entries)
        {
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            foreach (string entry in entries)
            {
                writer.Write(entry + "\n");
            }
            writer.Flush();
            stream.Position = 0;
            utility.Upload(stream, bucket, objectName);
        }
    }
}