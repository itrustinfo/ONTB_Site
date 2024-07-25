using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjectManagementTool.Models
{
    public class tClass2
    {
        public string ProjectId { get; set; }
        public string Project { get; set; }
        public int Total { get; set; }
        public int Pending { get; set; }
        public int ActionTaken { get; set; }
    }
}