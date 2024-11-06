using System;
using System.Collections.Generic;

namespace web_db.Models
{
    public partial class EmployeeAudit
    {
        public int AuditId { get; set; }
        public int EmployeeId { get; set; }
        public string Action { get; set; } = null!;
        public string? OldName { get; set; }
        public string? OldGender { get; set; }
        public int? OldDeptId { get; set; }
        public DateTime? ChangeDate { get; set; }
    }
}
