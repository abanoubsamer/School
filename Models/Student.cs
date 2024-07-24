using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace School.Models;

public partial class Student
{


    public int StudentId { get; set; }
    [Required(ErrorMessage = "Please Enter The First Name")]
    public string FirstName { get; set; } = null!;
    [Required(ErrorMessage = "Please Enter The Last Name")]
    public string LastName { get; set; } = null!;
    [RegesStudrnt]
    public DateTime? StartDate { get; set; }
    [Display(Name ="Image")]
    public string? ImagePath { get; set; } = null;
    public string PhoneNumber { get; set; }=null!;


    public virtual ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
}
