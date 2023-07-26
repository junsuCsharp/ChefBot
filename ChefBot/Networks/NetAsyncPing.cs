using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using System.Net;
using System.Net.NetworkInformation;
using System.ComponentModel;
using System.Threading;
using System.Diagnostics;


namespace Networks
{
    public static class NetAsyncPing
    {
        public static Dictionary<string, bool> dicReConnectIps = new Dictionary<string, bool>();
        public static void PingCheckAsync(string who)
        {
            try
            {
                AutoResetEvent waiter = new AutoResetEvent(false);

                Ping pingSender = new Ping();

                // When the PingCompleted event is raised,
                // the PingCompletedCallback method is called.
                pingSender.PingCompleted += new PingCompletedEventHandler(PingCompletedCallback);

                // Create a buffer of 32 bytes of data to be transmitted.
                string data = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
                byte[] buffer = Encoding.ASCII.GetBytes(data);

                // Wait 12 seconds for a reply.
                int timeout = 120;

                // Set options for transmission:
                // The data can go through 64 gateways or routers
                // before it is destroyed, and the data packet
                // cannot be fragmented.
                PingOptions options = new PingOptions(64, true);

                //Console.WriteLine("Time to live: {0}", options.Ttl);
                //Console.WriteLine("Don't fragment: {0}", options.DontFragment);

                // Send the ping asynchronously.
                // Use the waiter as the user token.
                // When the callback completes, it can wake up this thread.
                pingSender.SendAsync(who, timeout, buffer, options, waiter);

                // Prevent this example application from ending.
                // A real application should do something useful
                // when possible.
                waiter.WaitOne();
                //Console.WriteLine($"{DateTime.Now} >>> {who} ::: Ping Sender completed.");
            }
            catch
            { }
        
        }

        private static void PingCompletedCallback(object sender, PingCompletedEventArgs e)
        {
            try
            {
                // If the operation was canceled, display a message to the user.
                if (e.Cancelled)
                {
                    Console.WriteLine("Ping canceled.");

                    // Let the main thread resume.
                    // UserToken is the AutoResetEvent object that the main thread
                    // is waiting for.
                    ((AutoResetEvent)e.UserState).Set();
                }

                // If an error occurred, display the exception to the user.
                if (e.Error != null)
                {
                    Console.WriteLine("Ping failed:");
                    Console.WriteLine(e.Error.ToString());

                    // Let the main thread resume.
                    ((AutoResetEvent)e.UserState).Set();
                }

                PingReply reply = e.Reply;

                DisplayReply(reply);

                // Let the main thread resume.
                ((AutoResetEvent)e.UserState).Set();
            }
            catch
            { }
            
        }

        public static void DisplayReply(PingReply reply)
        {
            if (reply == null)
                return;

            bool IsReplySucess = false;
            string ip = reply.Address.ToString();

            //Console.WriteLine($"{DateTime.Now} >>> {reply.Address} <<< ping status ::: {reply.Status}");
            if (reply.Status == IPStatus.Success)
            {
                //Console.WriteLine($"{DateTime.Now} >>> DisplayReply ::: {reply.Address}");

                //Console.WriteLine("RoundTrip time: {0}", reply.RoundtripTime);
                //Console.WriteLine("Time to live: {0}", reply.Options.Ttl);
                //Console.WriteLine("Don't fragment: {0}", reply.Options.DontFragment);
                //Console.WriteLine("Buffer size: {0}", reply.Buffer.Length);

                IsReplySucess = true;

            }
            else
            {
                IsReplySucess = false;
            }

            if (dicReConnectIps.TryGetValue(ip, out bool IsPingSucess))
            {
                dicReConnectIps.Remove(ip);
            }
            dicReConnectIps.Add(ip, IsReplySucess);
        }
    }
}
