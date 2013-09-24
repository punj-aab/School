// -----------------------------------------------------------------------
// <copyright file="MessageEventArgs.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace EmailHandler.MQ
{
    using System;

    /// <summary>
    /// Internal message event arguments
    /// </summary>
    public class MessageEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the MessageEventArgs class
        /// </summary>
        /// <param name="body">Message body</param>
        public MessageEventArgs(object body)
        {
            this.MessageBody = body;
        }

        /// <summary>
        /// Gets or sets the body of the message
        /// </summary>
        public object MessageBody
        {
            get;
            set;
        }
    }
}
