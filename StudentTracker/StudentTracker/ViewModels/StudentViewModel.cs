﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using StudentTracker.Core.Entities;

namespace StudentTracker.ViewModels
{
    public class StudentViewModel : Student
    {
        public Profile Profile { get; set; }
    }
}