using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace School.Models;

public partial class Course
{
    public int CourseId { get; set; }

    public string Tital { get; set; } = null!;

    [Range(1,4)]
    public int Credits { get; set; }
    [ForeignKey("CourseLevel")]
    [Required(ErrorMessage = "Please select a course level.")]
    [Display(Name="Level")]
    public int? LevelId { get; set; } // Foreign key to CourseLevelId
    [Display(Name = "Descreption")]
    public string? CourseDesc { get; set; }
    public decimal? Price { get; set; }
    [Display(Name = "Active")]
    public bool IsCourseActive { get; set; }

    public virtual CourseLevel? CourseLevel { get; set; } // Navigation property for CourseLevel
    public virtual ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();



}
