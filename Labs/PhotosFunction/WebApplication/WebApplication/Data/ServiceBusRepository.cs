using Microsoft.Azure.ServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.Data
{
    public class ServiceBusRepository
    {
        public async static void SendMesage(int photoID)
        {
            QueueClient queueClient = new QueueClient("Endpoint=sb://stsaftservicebus.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=seO+ntp4tbeFVtgoWeCN/KT2csU8HgO1h+DYM6JuJi4=", "NewPhoto");
            Message newMessage = new Message();
            newMessage.UserProperties["PhotoID"] = photoID;
            await queueClient.SendAsync(newMessage);
        }
    }
}
