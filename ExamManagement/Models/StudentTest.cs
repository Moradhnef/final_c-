using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyTestMaker.Models
{
    public class StudentTest
    {
        public string StudentName { get; set; }
        public List<int> SelectedAnswers { get; set; }
        public StudentTest()
        {
            SelectedAnswers = new List<int>();
        }

       
    }
}
