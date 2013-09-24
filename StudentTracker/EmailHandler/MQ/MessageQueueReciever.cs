// -----------------------------------------------------------------------
// <copyright file="MessageQueueReciever.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace EmailHandler.MQ
{
    using System;
    using System.Messaging;

    /// <summary>
    /// Delegate that is used to define the callback when a message is picked up
    /// </summary>
    /// <param name="sender">Object that calls the callback</param>
    /// <param name="e">Event arguments</param>
    public delegate void MessageReceivedHandler(object sender, MessageEventArgs e);

    /// <summary>
    /// Receiver for messages from the message queue
    /// </summary>
    public class MessageQueueReceiver : MessageQueueBase
    {
        /// <summary>
        /// Message types to expect
        /// </summary>
        private Type[] messageTypes = 
                    {
                        typeof(Email) 
                    };

        /// <summary>
        /// Initializes a new instance of the MessageQueueReceiver class
        /// </summary>
        /// <param name="log">Log to write messages to</param>
        /// <param name="queuepath">Path to the message queue</param>
        public MessageQueueReceiver(string queuepath)
            : base(queuepath)
        {
            this.MQ.Formatter = new XmlMessageFormatter(this.messageTypes);
            this.MQ.ReceiveCompleted += new ReceiveCompletedEventHandler(this.ReceiveCompleted);
            this.MQ.BeginReceive();
        }

        /// <summary>
        /// Callback that is called when a message is picked up
        /// </summary>
        public event MessageReceivedHandler MessageReceived;

        /// <summary>
        /// Callback function that is called when a message is picked up
        /// </summary>
        /// <param name="sender">Object that calls the callback</param>
        /// <param name="e">Event arguments</param>
        private void ReceiveCompleted(object sender, ReceiveCompletedEventArgs e)
        {
            try
            {
                Message m = this.MQ.EndReceive(e.AsyncResult);
                this.RaiseMessageReceived(m.Body);
            }
            catch (Exception ex)
            {
            }
            finally
            {
                this.MQ.BeginReceive();
            }
        }

        /// <summary>
        /// Raises the event
        /// </summary>
        /// <param name="body">Message body</param>
        private void RaiseMessageReceived(object body)
        {
            if (this.MessageReceived != null)
            {
                this.MessageReceived(this, new MessageEventArgs(body));
            }
        }
    }
}
