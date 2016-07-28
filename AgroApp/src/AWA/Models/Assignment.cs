﻿using System.Collections.Generic;

namespace AWA.Models
{
    public class Assignment
    {
        public int AssignmentId { set; get; }
        public string Location { set; get; }
        public string Description { set; get; }
        public long Date { set; get; }

        public int CustomerId { get; set; }
        public Customer Customer { set; get; }

        public List<EmployeeAssignment> EmployeeAssignments { set; get; }
    }
}