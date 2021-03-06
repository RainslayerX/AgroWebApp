﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AWA.Models
{
    public class Timesheet
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int TimesheetId { get; set; }
        public bool IsVerified { get; set; } = false;

        [Required]
        public int UserId { get; set; }
        public User User { get; set; }
        
        [Required]
        public int AssignmentId { get; set; }
        public Assignment Assignment { get; set; }

        public ICollection<TimesheetRecord> Records { get; set; }
    }
}
