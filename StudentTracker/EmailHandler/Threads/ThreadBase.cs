// -----------------------------------------------------------------------
// <copyright file="ThreadBase.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace EmailHandler.Threads
{
    using System;
    using System.Threading;

    /// <summary>
    /// Thread base class
    /// </summary>
    public abstract class ThreadBase
    {
        /// <summary>
        /// Thread that is running
        /// </summary>
        private Thread thread = null;

       
        /// <summary>
        /// Value to indicate whether the service should stop
        /// </summary>
        private bool shouldStop = false;

        /// <summary>
        /// Initializes a new instance of the ThreadBase class
        /// </summary>
        public ThreadBase()
        {
        }

        
        /// <summary>
        /// Gets a value indicating whether the service is running
        /// </summary>
        public bool IsRunning
        {
            get { return this.thread != null && this.thread.ThreadState == ThreadState.Running; }
        }

        /// <summary>
        /// Gets a value indicating whether the thread should stop
        /// </summary>
        protected bool ShouldStop
        {
            get { return this.shouldStop; }
        }

        /// <summary>
        /// Starts the thread
        /// </summary>
        public virtual void Start()
        {
            if (this.thread == null)
            {
                this.thread = new Thread(this.Run);

                // The threads seem to be causing the SubscriptionService to continue running 
                // for 30 seconds when the Windows Service is stopped through the Services list
                // in MMC.
                //
                // According to http://msdn.microsoft.com/en-us/library/h339syd0.aspx setting 
                // the thread to a background thread does not have any differences except 
                // that the thread does not keep the managed execution environment running.
                this.thread.IsBackground = true;
            }

            this.thread.Start();
        }

        /// <summary>
        /// Stops the thread
        /// </summary>
        public virtual void Stop()
        {
            this.shouldStop = true;
            this.thread.Join();
        }

        /// <summary>
        /// Thread main loop
        /// </summary>
        protected abstract void Run();
    }
}

