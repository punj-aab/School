// -----------------------------------------------------------------------
// <copyright file="MessageQueueBase.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace EmailHandler.MQ
{
    /// <summary>
    /// Base class for Message Queue interface
    /// </summary>
    public abstract class MessageQueueBase
    {
        /// <summary>
        /// Windows message queue
        /// </summary>
        private System.Messaging.MessageQueue mq = null;

        

        /// <summary>
        /// Initializes a new instance of the MessageQueueBase class
        /// </summary>
        /// <param name="log">Log to write messages to</param>
        /// <param name="queuepath">The path of the queue to use</param>
        public MessageQueueBase(string queuepath)
        {
            this.InitializeQueue(queuepath);
        }

        /// <summary>
        /// Gets the Windows message queue
        /// </summary>
        protected System.Messaging.MessageQueue MQ
        {
            get { return this.mq; }
        }

    
        /// <summary>
        /// Initializes the Windows message queue
        /// </summary>
        /// <param name="queuepath">Path of the queue to initialize</param>
        private void InitializeQueue(string queuepath)
        {
            if (System.Messaging.MessageQueue.Exists(queuepath))
            {
                this.mq = new System.Messaging.MessageQueue(queuepath);
            }
            else
            {
                this.mq = System.Messaging.MessageQueue.Create(queuepath);
            }
        }
    }
}
