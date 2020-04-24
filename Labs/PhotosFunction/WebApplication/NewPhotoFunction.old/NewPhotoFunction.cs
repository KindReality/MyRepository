using System;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace NewPhotoFunction
{
    public class Functions
    {

        [FunctionName("NewPhotoFunction")]
        public static void Run([ServiceBusTrigger("newphoto", Connection = "ServiceBusConnectionString")]Message message)
        {
            using (var context = new ApplicationDbContext())
            {
                var photoID = (int)message.UserProperties["PhotoID"];
                var photo = context.Photos.Find(photoID);
                photo.Processed = true;
                context.SaveChanges();
            }            
        }
    }
}
