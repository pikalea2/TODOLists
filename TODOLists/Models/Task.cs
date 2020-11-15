using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TODOLists.Models
{
    public class Task
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string TaskName { get; set; }
        public string Description { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public System.DateTime ModifiedDate { get; set; }
        public bool IsCompleted { get; set; }
    }
}