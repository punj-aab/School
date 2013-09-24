// -----------------------------------------------------------------------
// <copyright file="QueueListener.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace EmailHandler.Threads
{
    using EmailHandler.MQ;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class QueueListener : ThreadBase
    {
        /// <summary>
        /// Queue path to pick up the connection related event for
        /// </summary>
        private string queueName = string.Empty;

        /// <summary>
        /// Message queue receiver to actually pick up the messages
        /// </summary>
        private MessageQueueReceiver receiver = null;

        /// <summary>
        /// Initializes a new instance of the QueueListener Thread class
        /// </summary>
        /// <param name="log">Current log</param>
        public QueueListener(string queueName)
            : base()
        {
            this.queueName = queueName;
            this.receiver = new MessageQueueReceiver(queueName);
            this.receiver.MessageReceived += new MessageReceivedHandler(this.Receiver_MessageReceived);
        }

        /// <summary>
        /// Stops the thread
        /// </summary>
        public override void Stop()
        {
            base.Stop();
        }

        protected override void Run()
        {
            while (!this.ShouldStop)
            {
                Thread.Sleep(1000);
            }
        }

        private void Receiver_MessageReceived(object sender, MessageEventArgs e)
        {
            try
            {
                if (e.MessageBody == null)
                {
                    Console.WriteLine("Received an empty message");
                    return;
                }

                if (e.MessageBody is Email)
                {
                    var email = e.MessageBody as Email;
                    if (email == null)
                    {
                        return;
                    }

                    if (Utilities.SendMailThruGmail(email))
                    {
                    }
                }
                else
                {
                    Console.WriteLine("Message is not of Email type");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
