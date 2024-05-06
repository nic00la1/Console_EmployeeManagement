using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Console_EmployeeManagement.Models
{
    public class Worker
    {
        public int Id { get;set; }
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string? Login { get; set; }
        public string? Password { get; set; }
        public int IdRole { get; set; }
        public int Age { get; set; }
        public DateTime HireDate { get; set; }
        public int IsWorking { get; set; }

    }
}
