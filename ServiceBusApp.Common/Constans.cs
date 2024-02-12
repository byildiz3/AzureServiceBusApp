using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceBusApp.Common
{
    public static  class Constans
    {
        public const string ConnectionString = "Endpoint=sb://starbuddy.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=Z45QDCSZFPk0+YEZREvojkZdQ05Es+mJN+ASbPiBWuI=";

        public const string OrderCreatedQueueName = "OrderCreatedQueue";
        public const string OrderDeletedQueueName = "OrderDeletedQueue";  
        public const string OrderTopic = "OrderTopic"; 
        public const string OrderCreatedSubscription = "OrderCreatedSubscription"; 
        public const string OrderDeletedSubscription = "OrderDeletedSubscription"; 
    }
}
