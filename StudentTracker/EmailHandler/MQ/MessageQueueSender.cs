// -----------------------------------------------------------------------
// <copyright file="MessageQueueSender.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace EmailHandler.MQ
{
    using System;

    /// <summary>
    /// Sends a message in the message queue
    /// </summary>
    public class MessageQueueSender : MessageQueueBase
    {
        /// <summary>
        /// Initializes a new instance of the MessageQueueSender class
        /// </summary>
        /// <param name="log">Log to write log messages to</param>
        /// <param name="queuepath">Message Queue path to use</param>
        public MessageQueueSender(string queuepath)
            : base(queuepath)
        {
        }

        /// <summary>
        /// Puts a message into the message queue
        /// </summary>
        /// <param name="o">Object to send</param>
        public void SendMessage(object o)
        {
            try
            {
                MQ.Send(o);
            }
            catch (Exception e)
            {
               
            }
        }
    }
}