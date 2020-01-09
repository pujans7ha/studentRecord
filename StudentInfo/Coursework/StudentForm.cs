using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Coursework
{
    public partial class StudentForm : Form
    {
        public bool ViewReport { get; set; }
        public StudentForm()
        {
            InitializeComponent();
            BindGrid();
            btnUpdate.Visible = false;
            this.ViewReport = false;
        }
        private void btnSubmit_Click(object sender, EventArgs e)
        {
            if (Validation())
            {
                Student obj = new Student();
                string firstName = txtFirstName.Text;
                string lastName = txtLastName.Text;
                obj.Name = firstName + " " + lastName;
                obj.Address = txtAddress.Text;
                obj.Age = int.Parse(txtAge.Text);
                obj.Gender = cmbGender.SelectedItem.ToString();
                obj.Contact = txtContact.Text;
                obj.Email = txtEmail.Text;
                obj.Course = cmbCourse.SelectedItem.ToString();
                obj.RegistrationDate = dpRegistration.Value;
                obj.Add(obj);
                BindGrid();
                Clear();
            }
        }
        private void BindGrid()
        {
            String sort;
            try
            {
                sort = cmbSort.SelectedItem.ToString();
            }
            catch
            {
                sort = "";
            }
            
            Student obj = new Student();
            List<Student> studentList = obj.List();
            
            if (sort == "Name")
            {
                List<Student> list = obj.sortByName(studentList);
            }
            if (sort == "Registration Date")
            {
                List<Student> list = obj.sortByRegDate(studentList);
            }

            DataTable dt = Utility.ConvertToDataTable(studentList);
            dataGridView.DataSource = dt;
            BindChart(studentList);
        }
        private void Clear()
        {
            txtFirstName.Text = "";
            txtLastName.Text = "";
            txtAge.Text = ""; ;
            cmbGender.SelectedItem = null;
            txtAddress.Text = "";
            txtContact.Text = "";
            txtEmail.Text = "";
            cmbCourse.SelectedItem = null;
            dpRegistration.Value = DateTime.Today;

        }
        private void BindChart(List<Student> lst)
        {
            if (lst != null)
            {
                var result = lst
                    .GroupBy(l => l.Course)
                    .Select(cl => new
                    {
                        Course = cl.First().Course,
                        Count = cl.Count().ToString()
                    }).ToList();
                DataTable dt = Utility.ConvertToDataTable(result);
                chart1.DataSource = dt;
                chart1.Name = "Course";
                chart1.Series["Series1"].XValueMember = "Course";
                chart1.Series["Series1"].YValueMembers = "Count";
                this.chart1.Titles.Remove(this.chart1.Titles.FirstOrDefault());
                this.chart1.Titles.Add("Student Enrollment Chart");
                chart1.Series["Series1"].IsValueShownAsLabel = true;
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (Validation())
            { Student obj = new Student();
                obj.Id = int.Parse(txtId.Text);
                string firstName = txtFirstName.Text;
                string lastName = txtLastName.Text;
                obj.Name = firstName + " " + lastName;
                obj.Address = txtAddress.Text;
                obj.Email = txtEmail.Text;
                obj.Age = int.Parse(txtAge.Text);
                obj.Contact = txtContact.Text;
                obj.Gender = cmbGender.SelectedItem.ToString();
                obj.Course = cmbCourse.SelectedItem.ToString();
                obj.RegistrationDate = dpRegistration.Value;
                obj.Edit(obj);
                BindGrid();
                Clear();
                btnUpdate.Visible = false;
                btnSubmit.Visible = true;
            }
        }

        private void dataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            Student obj = new Student();
            if (e.ColumnIndex == 0)
            {
                //get the value of the clicked rows id column
                string value = dataGridView[2, e.RowIndex].Value.ToString();
                int id = 0;
                if (String.IsNullOrEmpty(value))
                {
                    MessageBox.Show("Invalid Data");
                }
                else
                {
                    id = int.Parse(value);
                    Student s = obj.List().Where(x => x.Id == id).FirstOrDefault();
                    txtId.Text = s.Id.ToString();
                    txtFirstName.Text = s.Name.Split(' ')[0];
                    txtLastName.Text = s.Name.Split(' ')[1];
                    txtAge.Text = s.Age.ToString();
                    cmbGender.SelectedItem = s.Gender;
                    txtAddress.Text = s.Address;
                    txtContact.Text = s.Contact;
                    txtEmail.Text = s.Email;
                    cmbCourse.SelectedItem = s.Course;
                    dpRegistration.Value = s.RegistrationDate;
                    btnSubmit.Visible = false;
                    btnUpdate.Visible = true;
                }
            }
            else if (e.ColumnIndex == 1)
            {
                string message = "Do you want to Delete this Record?";
                string title = "Delete Confirmation";
                MessageBoxButtons buttons = MessageBoxButtons.OKCancel;
                DialogResult result = MessageBox.Show(message, title, buttons);
                if (result == DialogResult.OK)
                {
                    //get the value of the clicked rows id column
                    string value = dataGridView[2, e.RowIndex].Value.ToString();
                    obj.Delete(int.Parse(value));
                    BindGrid();
                    MessageBox.Show("Record Successfully Deleted");
                }
            }
        }
        private bool checkComboBox(ComboBox cmb)
        {
            try
            {
                cmb.SelectedItem.ToString();
               } 
            catch { 
                return false; 
            }
            return true;
        }
        public bool Validation()
        {
            List<String> error = new List<String>();
            bool genderValidation = checkComboBox(cmbGender);
            bool courseValidation = checkComboBox(cmbCourse);
            bool validation_result = true;

            if (txtFirstName.Text.Length == 0 || txtFirstName.Text.Length > 50)
            {
                error.Add("Enter First Name");
            }
            if (txtLastName.Text.Length == 0 || txtLastName.Text.Length > 50)
            {
                error.Add("Enter Last Name");
            }
            if (txtAge.Text.Length == 0)
            {
                error.Add("Enter Age");
            }
            if (txtAge.Text.Length != 0)
            {
                try
                {
                    int.Parse(txtAge.Text);
                }
                catch
                {
                    error.Add("Age should be in Number");
                }
            }
            if (!genderValidation)
            {
                error.Add("Choose Gender");
            }
            if (txtAddress.Text.Length == 0)
            {
                error.Add("Enter Address");
            }
            if (txtContact.Text.Length == 0)
            {
                error.Add("Enter Contact");
            }
            if (txtEmail.Text.Length == 0)
            {
                error.Add("Enter Email Address");
            }
            if (txtEmail.Text.Length != 0)
            {
                try
                {
                    new System.Net.Mail.MailAddress(txtEmail.Text);
                }
                catch
                {
                    error.Add("Not Valid Email Address");
                }
            }
            if (!courseValidation)
            {
                error.Add("Choose Course");
            }
            if (error.Count != 0)
            {
                MessageBox.Show(error[0]);
                return false;
            }
            return validation_result;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Clear();
            btnUpdate.Visible = false;
            btnSubmit.Visible = true;
        }

        private void btnSort_Click(object sender, EventArgs e)
        {
            BindGrid();
        }

        private void btnReport_Click(object sender, EventArgs e)
        {
            if (!ViewReport)
            {
                ReportForm obj = new ReportForm() { Report = this };
                obj.Show(this);
                ViewReport = true;
            }
        }
    }
}
