using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Coursework
{
    public partial class ReportForm : Form
    {
        public StudentForm Report { get; set; }
        public ReportForm()
        {
            InitializeComponent();
            BindGrid();
        }
        public void BindGrid()
        {
            Student obj = new Student();
            List<Student> listdatas = obj.List();
            WeeklyReport(listdatas);
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Report.ViewReport = false;
            this.Close();
        }
        private void WeeklyReport(List<Student> lst)
        {
            if (lst != null)
            {
                var result = lst
                    .GroupBy(d => new { V = CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(d.RegistrationDate, CalendarWeekRule.FirstDay, DayOfWeek.Sunday), d.Course })
                    .Select(c1 => new
                    {
                        Week = "Week" + c1.Key.V.ToString(),
                        Faculty = c1.First().Course,
                        Total_Student = c1.Count().ToString(),
                        
                    }).ToList();

                DataTable dt = Utility.ConvertToDataTable(result);
                reportData.DataSource = dt;

            }
        }

        private void ReportForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Report.ViewReport = false;
        }
    }
}
