using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EmailHandler
{
    public class Email
    {
        public string mailTo { get; set; }
        public string mailFrom { get; set; }
        public string mailSubject { get; set; }
        public string mailBody { get; set; }
    }
}
