using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApplication4.Models;

[Index("Email", Name = "UQ__Teachers__161CF72423DA2485", IsUnique = true)]
public partial class Teacher
{
    [Key]
    public int TeacherId { get; set; }

    [StringLength(50)]
    public string FirstName { get; set; } = null!;

    [StringLength(50)]
    public string LastName { get; set; } = null!;

    [Column("EMAIL")]
    [StringLength(50)]
    public string Email { get; set; } = null!;

    public DateOnly HireDate { get; set; }

    [Column("DepartmentID")]
    public int? DepartmentId { get; set; }

    public bool? IsActive { get; set; }

    [InverseProperty("Teacher")]
    public virtual ICollection<Course> Courses { get; set; } = new List<Course>();

    [ForeignKey("DepartmentId")]
    [InverseProperty("Teachers")]
    public virtual Department? Department { get; set; }
}
