using System;
using System.Diagnostics;
using System.Net;
using System.Threading;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.Owin.Hosting;

namespace Core.Api
{
    //public class WorkerRole : RoleEntryPoint
    //{
    //    // The name of your queue
    //    const string QueueName = "ProcessingQueue";

    //    // QueueClient is thread-safe. Recommended that you cache 
    //    // rather than recreating it on every request
    //    QueueClient Client;
    //    ManualResetEvent CompletedEvent = new ManualResetEvent(false);

    //    public override void Run()
    //    {
    //        Trace.WriteLine("Starting processing of messages");

    //        // Initiates the message pump and callback is invoked for each message that is received, calling close on the client will stop the pump.
    //        Client.OnMessage((receivedMessage) =>
    //            {
    //                try
    //                {
    //                    // Process the message
    //                    Trace.WriteLine("Processing Service Bus message: " + receivedMessage.SequenceNumber.ToString());
    //                }
    //                catch
    //                {
    //                    // Handle any message processing specific exceptions here
    //                }
    //            });

    //        CompletedEvent.WaitOne();
    //    }

    //    public override bool OnStart()
    //    {
    //        // Set the maximum number of concurrent connections 
    //        ServicePointManager.DefaultConnectionLimit = 12;

    //        // Create the queue if it does not exist already
    //        string connectionString = CloudConfigurationManager.GetSetting("Microsoft.ServiceBus.ConnectionString");
    //        var namespaceManager = NamespaceManager.CreateFromConnectionString(connectionString);
    //        if (!namespaceManager.QueueExists(QueueName))
    //        {
    //            namespaceManager.CreateQueue(QueueName);
    //        }

    //        // Initialize the connection to Service Bus Queue
    //        Client = QueueClient.CreateFromConnectionString(connectionString, QueueName);
    //        return base.OnStart();
    //    }

    //    public override void OnStop()
    //    {
    //        // Close the connection to Service Bus Queue
    //        Client.Close();
    //        CompletedEvent.Set();
    //        base.OnStop();
    //    }
    //}

    public class WorkerRole : RoleEntryPoint
    {
        private IDisposable _app = null;

        public override void Run()
        {
            Trace.TraceInformation("WebApiRole entry point called", "Information");

            while (true)
            {
                Thread.Sleep(10000);
                Trace.TraceInformation("Working", "Information");
            }
        }

        public override bool OnStart()
        {
            ServicePointManager.DefaultConnectionLimit = 12;

            var endpoint = RoleEnvironment.CurrentRoleInstance.InstanceEndpoints["Endpoint1"];
            string baseUri = String.Format("{0}://{1}",
                endpoint.Protocol, endpoint.IPEndpoint);

            Trace.TraceInformation(String.Format("Starting OWIN at {0}", baseUri),
                "Information");

            _app = WebApp.Start<Startup>(new StartOptions(url: baseUri));
            return base.OnStart();
        }

        public override void OnStop()
        {
            if (_app != null)
            {
                _app.Dispose();
            }
            base.OnStop();
        }
    }
}
