using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Console_EmployeeManagement.Models
{
    public class Note
    {
        public int IdNote { get; set; }
        public int IdWorker { get; set; }
        public string Content { get; set; }
        public DateTime AddedAt { get; set; }
    }
}
