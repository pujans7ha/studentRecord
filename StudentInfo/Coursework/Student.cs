using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursework
{
    class Student
    {
        private string FilePath = "student.json";
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public string Gender { get; set; }
        public string Address { get; set; }
        public string Contact { get; set; }
        public string Email { get; set; }
        public string Course { get; set; }
        public DateTime RegistrationDate { get; set; }
        public void Add(Student info)
        {
            Random r = new Random();
            info.Id = r.Next(101, 1000);
            string data = JsonConvert.SerializeObject(info, Formatting.None);
            Utility.WriteToTextFile(FilePath, data);
        }
        public void Edit(Student info)
        {
            //invoking list method of the student class to get student list
            List<Student> list = List();
            //using linq to select student having the specified id
            Student s = list.Where(x => x.Id == info.Id).FirstOrDefault();
            //removing  student object that is to be updated from the list
            list.Remove(s);
            //adding the updated student object to the list
            list.Add(info);
            //converting list of student to string
            string data = JsonConvert.SerializeObject(list, Formatting.None);
            //invoking method of utility class 
            Utility.WriteToTextFile(FilePath, data, false);
        }
        public void Delete(int id)
        {
            //invoking list method of the student class to get student list
            List<Student> list = List();
            //using linq to select student having the specified id
            Student s = list.Where(x => x.Id == id).FirstOrDefault();
            //removing  student object that is to be updated from the list
            list.Remove(s);
            //converting list of student to string
            int count = list.Count;
            string data = JsonConvert.SerializeObject(list, Formatting.None);
            //invoking method of utility class 
            Utility.WriteToTextFile(FilePath, data, false, count);

        }
        public List<Student> List()
        {
            string d = Utility.ReadFromTextFile(FilePath);
            if (d != null)
            {
                List<Student> list = JsonConvert.DeserializeObject<List<Student>>(d);
                return list;
            }
            return null;
        }
        public List<Student> sortByName(List<Student> list)
        {
            Student temp;
            for (int i = 0; i < list.Count - 1; i++)
            {
                for (int j = i + 1; j < list.Count; j++)
                {
                    if (list[i].Name.CompareTo(list[j].Name) > 0)
                    {
                        temp = list[i];
                        list[i] = list[j];
                        list[j] = temp;
                    }
                }
            }
            return list;
        }
        public List<Student> sortByRegDate(List<Student> list)
        {
            Student temp;
            for (int i = 0; i < list.Count - 1; i++)
            {
                for (int j = i + 1; j < list.Count; j++)
                {
                    if (list[i].RegistrationDate.CompareTo(list[j].RegistrationDate) > 0)
                    {
                        temp = list[i];
                        list[i] = list[j];
                        list[j] = temp;
                    }
                }
            }
            return list;

        }
    }
}
