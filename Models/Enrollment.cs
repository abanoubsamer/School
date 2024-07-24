using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace School.Models;

public partial class Enrollment
{
    public int EnrollmentId { get; set; }
    [Required(ErrorMessage = "Course is required")]
    public int CourseId { get; set; }
    [Required(ErrorMessage = "Student is required")]
    public int StudentId { get; set; }

    public decimal? Grade { get; set; }

    public virtual Course? Course { get; set; } 

    public virtual Student? Student { get; set; } 
}
