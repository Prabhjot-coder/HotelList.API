using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApplication4.Models;

[Table("COURSES")]
public partial class Course
{
    [Key]
    public int CourseId { get; set; }

    [StringLength(100)]
    public string CourseName { get; set; } = null!;

    public int? TeacherId { get; set; }

    public bool? IsActive { get; set; }
    // Navigational Properties 
    [InverseProperty("Course")]
    public virtual ICollection<Student> Students { get; set; } = new List<Student>();

    [ForeignKey("TeacherId")]
    [InverseProperty("Courses")]
    public virtual Teacher? Teacher { get; set; }
}
