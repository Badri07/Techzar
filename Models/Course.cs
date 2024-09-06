using System;
using System.Collections.Generic;

namespace Techzar.Models;

public partial class Course
{
    public int Courseid { get; set; }

    public string Coursename { get; set; } = null!;

    public int? Departmentid { get; set; }

    public virtual Department? Department { get; set; }

    public virtual ICollection<Student> Students { get; set; } = new List<Student>();
}
