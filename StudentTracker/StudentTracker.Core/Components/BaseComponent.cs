// -----------------------------------------------------------------------
// <copyright file="BaseComponent.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace StudentTracker.Core.Components
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using StudentTracker.Core.Components.Interfaces;
    using StudentTracker.Core.DAL;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class BaseComponent : IBusinessComponent
    {
        protected readonly StudentContext context;

        public BaseComponent(StudentContext _context = null)
        {
            if (context == null && _context == null)
            {
                context = new StudentContext();
            }
            else if(context==null)
            {
                context = _context;
            }
        }

        public void Dispose()
        {
            context.Dispose();
        }
    }
}
