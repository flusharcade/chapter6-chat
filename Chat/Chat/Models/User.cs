
namespace Chat.Models 
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Web;

    public class SigRUser
    {
        public string Name { get; set; }
        public HashSet<string> ConnectionIds { get; set; }
    }
}