using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApplication4.Models;

[Table("STUDENTS")]
[Index("Email", Name = "UQ__STUDENTS__161CF724FF11CB4D", IsUnique = true)]
public partial class Student
{
    [Key]
    public int StudentId { get; set; }

    [StringLength(50)]
    public string FirstName { get; set; } = null!;

    [StringLength(50)]
    public string LastName { get; set; } = null!;

    [Column("EMAIL")]
    [StringLength(50)]
    public string Email { get; set; } = null!;

    public DateOnly DateOfBirth { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? PhoneNumber { get; set; }

    public DateOnly EnrollDate { get; set; }

    public bool? IsActive { get; set; }

    public int? CourseId { get; set; }

    [ForeignKey("CourseId")]
    [InverseProperty("Students")]
    public virtual Course? Course { get; set; }
}
