using System.ComponentModel.DataAnnotations;
using School.Models;



namespace School.Models
{

    // View Model
    public class PartialClass
    {
  
        public partial class Student
        {
            public int StudentId { get; set; }
            public string FullName { get; set; }
        }

        public partial class Course
        {
          
        }
        public partial class Enrollment
        {
           
        }
    }
}
