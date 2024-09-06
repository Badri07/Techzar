using System;
using System.Collections.Generic;

namespace Techzar.Models;

public partial class Student
{
    public int Studentid { get; set; }

    public string Studentname { get; set; } = null!;

    public int? Departmentid { get; set; }

    public int? Courseid { get; set; }

    public virtual Course? Course { get; set; }

    public virtual Department? Department { get; set; }
}
