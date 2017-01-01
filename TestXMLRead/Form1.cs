using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Text;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;



namespace TestXMLRead
{
	public partial class Form1 : Form
	{

		enum TestType 
		{
			UserProjects,
			ProjectData
		}

		public Form1()
		{
			InitializeComponent();

			TestType t;

//			t = TestType.UserProjects;
			t = TestType.ProjectData;



			switch (t)
			{
				case TestType.UserProjects:
					TestUserProjects tup = new TestUserProjects();
					textBox1.Text = tup.UserProjectsTests().ToString();
					break;

				case TestType.ProjectData:
					TestProjectData tpd = new TestProjectData();
					textBox1.Text = tpd.ProjectDataTests().ToString();
					break;

			}
			
		}

		private void button1_Click(object sender, EventArgs e)
		{
			this.Close();
		}


	}

}
