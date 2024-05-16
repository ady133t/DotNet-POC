using System;
using System.Collections.Generic;

namespace My_Dashboard.Models.DB
{
    public partial class Machine
    {
        public int Id { get; set; }
        public string MachineName { get; set; } = null!;
        public string MachineId { get; set; } = null!;
        public DateTime DateTime { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
