using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PagingWPFDataGrid
{
    /// <summary>
    /// Generic Class Model Adopted from https://www.codeproject.com/Articles/1092189/WPF-Pagination-for-DataGrid
    /// </summary>
    class StudentModel
    {
        public class Student
        {
            public string FirstName { get; set; }
            public string MiddleName { get; set; }
            public string LastName { get; set; }
            public int Age { get; set; }
        }


        public IList<Student> GetData()
        {
            List<Student> genericList = new List<Student>();
            Student studentObj;
            Random randomObj = new Random();
            for (int i = 0; i < 12345; i++) //You can make this number anything you can think of (and your processor can handle).
            {
                studentObj = new Student
                {
                    FirstName = "first " + i,
                    MiddleName = "Middle " + i,
                    LastName = "Last " + i,
                    Age = (int)randomObj.Next(1, 100)
                };

                genericList.Add(studentObj);

            }
            return genericList;
        }
    }
}
