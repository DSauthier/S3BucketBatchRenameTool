using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using System.Collections;

namespace AmazonUpperCase
{
    class Program
    {
        private static string awsSecret = "";
        private static string awsAccess = "";
        private static string bucket = "";
        private static string folder = "";


        static void Main(string[] args)
        {
          

            var counter = 0;


            void RecursiveLoop()
            {
                var client = new AmazonS3Client(awsAccess, awsSecret, RegionEndpoint.USEast1);

                ListObjectsRequest request = new ListObjectsRequest();
                request.BucketName = bucket;
                request.Prefix = folder;
                ListObjectsResponse response = client.ListObjects(request);

                foreach (S3Object o in response.S3Objects)
                    {
                       var newKey = o.Key.ToUpper();
                    //       if (newKey == o.Key)
                    //{
                    
                    //    continue;
                    //}
                       
                    /*
                     To copy new Folders/Objects into S3 , the Source folder and the Destination folder must have diferent names. ex: Access Folder != ACCESS Folder.

                    IF the folders have the exact same name, a 400 request error will happen, since s3 bucket will not replace the file/folder in itself.

                    To make easier to rename such folders and files, you can change the newKey to newKey = o.Key.toLower(); and run the program, changing everything to lowerCase
                    and after that, change it back to toUpper(), and running it again.
                     
                     */ 


                    var copy = new CopyObjectRequest();
                        copy.SourceBucket = bucket;
                        copy.SourceKey = o.Key;
                        copy.DestinationBucket = bucket;
                        copy.DestinationKey = newKey;

                    if (copy.SourceKey != copy.DestinationKey)
                    {
                        client.CopyObject(copy);


                        //if (o.Key.EndsWith("/"))
                        //    continue;

                        var delete = new DeleteObjectRequest();
                        delete.Key = o.Key;
                        delete.BucketName = bucket;
                        client.DeleteObject(delete);
                        counter++;
                    }
                    else
                    {
                        counter++;
                    }
                    //client.CopyObject(copy);


                    //        //if (o.Key.EndsWith("/"))
                    //        //    continue;

                    //        var delete = new DeleteObjectRequest();
                    //        delete.Key = o.Key;
                    //        delete.BucketName = bucket;
                    //        client.DeleteObject(delete);
                    //        counter++;
                     }
                var listToArray = response.S3Objects.ToArray();

                if (counter > 1000000 || listToArray.Length == 0)
                    {
                        return;
                    }
                else
                    {
                        RecursiveLoop();
                    }

            }

            RecursiveLoop();
        }
      
    }
}
