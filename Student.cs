using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library_system
{
    internal class Student
    {
        public static string? email;
        public static string? firstName;
        public static string? lastName;
        public static string? branch;
        public static string? type;
        public static string? studentId;
        public static string? targetBranch;

        public static void clear()
        {
            email = null;
            firstName = null;
            lastName = null;
            branch = null;
            type = null;
            studentId = null;
        }
    }
}
