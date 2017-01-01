using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace TestXMLRead
{
	public class TestUserProjects
	{
		private UserProjects up;

		private  string xmlUserFile = @"D:\Users\Jeff\Documents\Programming\_Office Standards\ProjectUserData\UserProjects.xml";

		public StringBuilder UserProjectsTests()
		{
			string nl = Util.nl;

			uProject upx;

			StringBuilder sb = new StringBuilder();

			up = UserProjects.LoadFromFile(xmlUserFile);

			string name = "jeffs";
			// default 
			upx = new uProject(name, false, false,
				new ProjNumInfo("2099-999"), new ProjNumInfo("00"), 
				new ProjNumInfo("1"), new ProjNumInfo("A"));


//			int[] testNumber = new[] { 1, 22, 1, 21, 1 };	// test flip current

//			int[] testNumber = new[] { 1, 13, 1, 12, 1 };	// test flip active

			int[] testNumber = new[] { 899900, 1, 21, 1, 22, 1 };	// test flip current for non-current

			foreach (int test in testNumber)
			{
				switch (test)
				{
					case 0:		// list all projects
						sb.Append(ListAllUsersAndProjects());
						break;
					case 1:		// list all projects for a user by type: all
						sb.Append(ListProjectsForUserByType(upx, ListingType.All));
						break;
					case 2:		// list all project for a user by type: Active
						sb.Append(ListProjectsForUserByType(upx, ListingType.Active));
						break;
					case 3:		// list all project for a user by type: Inactive
						sb.Append(ListProjectsForUserByType(upx, ListingType.Inactive));
						break;
					case 11:	// add a project
						sb.Append(nl + AddProject(upx));
						break;
					case 12:	// make project active
						sb.Append(nl + ActiveProject(upx));
						break;
					case 13:	// make project inactive
						sb.Append(nl + InactiveProject(upx));
						break;
					case 14:	// delete a project
						sb.Append(nl + DeleteProject(upx));
						break;
					case 21:	// make a project current
						sb.Append(nl + CurrentProject(upx));
						break;
					case 22:	// make a project not current
						sb.Append(nl + NotCurrentProject(upx));
						break;
					case 31:	// delete a project from all users
						sb.Append(nl + DeleteProjectFromAllUsers(upx));
						break;
					case 32:	// delete a project / task from all users
						sb.Append(nl + DeleteProjectAndTaskFromAllUsers(upx));
						break;
//					case 41:	// add a user - this is done via addProject
//						sb.Append(nl + AddUser(upx));
//						break;
					case 42:	// delete a user
						sb.Append(nl + DeleteUser(upx));
						break;
					case 99:	// save the file
						savefile();
						break;
					case 999900:	// set upx: 2099-999 / 00 / 1 / A
						upx = new uProject(name, false, false,
							new ProjNumInfo("2099-999"), new ProjNumInfo("00"),
							new ProjNumInfo("1"), new ProjNumInfo("A"));
						break;
					case 899900:	// set upx: 2099-999 / 00 / 1 / A
						upx = new uProject(name, false, false,
							new ProjNumInfo("2098-999"), new ProjNumInfo("00"),
							new ProjNumInfo("1"), new ProjNumInfo("A"));
						break;
					default:
						sb.Append(nl).Append("No tests selected");
						break;
				}
			}
			

			//			sb.Append(ListProjectsForUserByType(upx, ListingType.All));
			//
			//			sb.Append(nl + AddProject(upx));
			//
			//			sb.Append(ListProjectsForUserByType(upx, ListingType.All));
			//
			//			sb.Append(nl + ActiveProject(upx));
			//
			//			sb.Append(ListProjectsForUserByType(upx, ListingType.All));
			//
			//			sb.Append(nl + InactiveProject(upx));
			//
			//			sb.Append(ListProjectsForUserByType(upx, ListingType.All));
			//
			//			sb.Append(nl + DeleteProject(upx));
			//
			//			sb.Append(ListProjectsForUserByType(upx, ListingType.All));
			//
			//			sb.Append(Util.nl + NotCurrentProject(upx));
			//
			//			sb.Append(ListProjectsForUserByType(upx, ListingType.All));
			//
			//			sb.Append(Util.nl + CurrentProject(upx));
			//
			//			sb.Append(ListProjectsForUserByType(upx, ListingType.All));
			//
			//			upx = new uProject("alyx", false, false, "2099-999", "00", "1", "A");
			//
			//			sb.Append(Util.nl + AddProject(upx));
			//
			//			sb.Append(ListProjectsForUserByType(upx, ListingType.All));
			//
			//			sb.Append(ListProjectsForUserByType(upx, ListingType.All));
			//			sb.Append(ListProjectsForUserByType(upx, ListingType.Active));
			//			sb.Append(ListProjectsForUserByType(upx, ListingType.Inactive));


			//			upx = new uProject("jeff", false, false, "2099-999", "00", "1", "A");
			//
			//			sb.Append(ListAllUsersAndProjects());
			//			sb.Append(DeleteProjectFromAllUsers(upx));

			//			sb.Append(DeleteProjectAndTaskFromAllUsers(upx));

			//			

			//			sb.Append(DeleteUser(upx));

			//			up.DeleteAllProjectsFromUser(upx);

			//			sb.Append(ListAllUsersAndProjects());

//			savefile();

			return sb;
		}

		public void savefile()
		{
			up.SaveToFile(xmlUserFile);
		}

		public StringBuilder DescribeProcedure(uProject upx, string message)
		{
			StringBuilder sb = new StringBuilder(message);

			sb.Append(Util.nl + FormatProjectNumber(upx));
			sb.Append(Util.nl + "for user: ");
			sb.Append(upx.UserName);

			return sb;
		}

		public StringBuilder DeleteUser(uProject upx)
		{
			StringBuilder sb = new StringBuilder(Util.nl);

			sb.Append(DescribeProcedure(upx, "Delete user"));

			if (up.DeleteUser(upx))
				sb.Append(Util.nl + "\uD83D\uDC4D user deleted" + Util.nl);
			else
				sb.Append(Util.nl + "\uD83D\uDD93 user NOT deleted" + Util.nl);

			return sb;
		}

		public StringBuilder DeleteProjectAndTaskFromAllUsers(uProject upx)
		{
			StringBuilder sb = new StringBuilder(Util.nl);

			sb.Append(DescribeProcedure(upx, "Delete project/task from all users"));

			up.DeleteProjectFromAllUsersByNumberAndTask(upx);

			return sb;
		}

		public StringBuilder DeleteProjectFromAllUsers(uProject upx)
		{
			StringBuilder sb = new StringBuilder(Util.nl);

			sb.Append(DescribeProcedure(upx, "Delete project from all users"));

			up.DeleteProjectFromAllUsersByNumber(upx);

			return sb;
		}

		public StringBuilder NotCurrentProject(uProject upx)
		{
			StringBuilder sb = new StringBuilder(Util.nl);

			sb.Append(DescribeProcedure(upx, "Setting project not-current"));

			if (up.SetUserProjectToNotCurrent(upx))
				sb.Append(Util.nl + "\uD83D\uDC4D project not-current" + Util.nl);
			else
				sb.Append(Util.nl + "\uD83D\uDD93 project NOT not-current" + Util.nl);

			return sb;
		}

		public StringBuilder CurrentProject(uProject upx)
		{
			StringBuilder sb = new StringBuilder(Util.nl);

			sb.Append(DescribeProcedure(upx, "Setting project current"));

			if (up.SetUserProjectToCurrent(upx))
				sb.Append(Util.nl + "\uD83D\uDC4D project current" + Util.nl);
			else
				sb.Append(Util.nl + "\uD83D\uDD93 project NOT current" + Util.nl);

			return sb;
		}


		public StringBuilder ActiveProject(uProject upx)
		{
			StringBuilder sb = new StringBuilder(Util.nl);

			sb.Append(DescribeProcedure(upx, "Setting project active"));

			if (up.SetUserProjectToActive(upx))
				sb.Append(Util.nl + "\uD83D\uDC4D project active" + Util.nl);
			else
				sb.Append(Util.nl + "\uD83D\uDD93 project NOT active" + Util.nl);

			return sb;
		}

		public StringBuilder InactiveProject(uProject upx)
		{
			StringBuilder sb = new StringBuilder(Util.nl);

			sb.Append(DescribeProcedure(upx, "Setting project inactive"));

			if (up.SetUserProjectToInactive(upx))
				sb.Append(Util.nl + "\uD83D\uDC4D project inactive" + Util.nl);
			else
				sb.Append(Util.nl + "\uD83D\uDD93 project NOT inactive" + Util.nl);

			return sb;
		}

		public StringBuilder DeleteProject(uProject upx)
		{
			StringBuilder sb = new StringBuilder(Util.nl);

			sb.Append(DescribeProcedure(upx, "Deleting project"));

			if (up.DeleteProjectFromUser(upx))
				sb.Append(Util.nl + "\uD83D\uDC4D project deleted" + Util.nl);
			else
				sb.Append(Util.nl + "\uD83D\uDD93 project NOT deleted" + Util.nl);

			return sb;
		}

		public StringBuilder AddProject(uProject upx)
		{

			StringBuilder sb = new StringBuilder(Util.nl);

			sb.Append(DescribeProcedure(upx, "Adding project"));

			if (up.AddProjectToUser(upx))
			{
				sb.Append(Util.nl + "\uD83D\uDC4D project added" + Util.nl);
			}
			else
			{
				sb.Append(Util.nl + "\uD83D\uDD93project NOT added" + Util.nl);
			}

			return sb;
		}


		public static StringBuilder FormatProjectNumber(uProject upx)
		{

			string phase = upx.Phase.Number ?? "";
			string building = upx.Building.Number ?? "";

			phase = Util.diamonds.Substring(phase.Length) + phase;
			building = Util.diamonds.Substring(building.Length) + building;

			return new StringBuilder(String.Format("{0}-{1}-{2}-{3}",
					upx.ProjNumber.Number ?? "", upx.Task.Number ?? "", phase, building));
		}



		private StringBuilder FormatProjectNumber(UserProjectsUserProject p)
		{
			if (p == null)
				return new StringBuilder("");

			return FormatProjectNumber(new uProject("", false, true, 
				new ProjNumInfo(p.Number), new ProjNumInfo(p.Task), 
				new ProjNumInfo(p.Phase), new ProjNumInfo(p.Building)));
		}



		private StringBuilder FormatProject(UserProjectsUserProject p)
		{
			return new StringBuilder(String.Format("\tproject: {0}   is Active? {1}  is current? {2}{3}",
				FormatProjectNumber(p), p.Active, p.Current, Util.nl));
		}


		private StringBuilder ListProjects(UserProjectsUser upu)
		{
			StringBuilder sb = new StringBuilder("");


			if (upu != null && upu.Project.Count > 0)
			{
				foreach (UserProjectsUserProject p in upu.Project)
				{
					sb.Append(FormatProject(p));
				}
			}
			else
			{
				sb.Append("no projects" + Util.nl);
			}

			return sb;
		}

		private StringBuilder ListAllUsersAndProjects()
		{
			StringBuilder sb = new StringBuilder();

			List<UserProjectsUser> upUsers = up.User;

			List<UserProjectsUserProject> projects;

			sb.Append("count: " + upUsers.Count + Util.nl);

			for (int i = 0; i < upUsers.Count; i++)
			{
				sb.Append("user " + i + ":  is: " + upUsers[i].Name);

				projects = upUsers[i].Project;

				sb.Append(" and has " + projects.Count + " projects" + Util.nl);

				for (int j = 0; j < projects.Count; j++)
				{
					UserProjectsUserProject p = projects[j];
					sb.Append(FormatProject(p));
				}
			}

			return sb;
		}

		private StringBuilder ListProjectsForUserByType(uProject upx, ListingTypeStruct listingType)
		{
			StringBuilder sb = new StringBuilder();

			string listingTypeName = listingType.Name;

			UserProjectsUser upu = up.FindUserProjects(upx, listingType);

			if (upu != null && upu.Project != null
				&& upu.Project.Count > 0)
			{
				sb.Append(Util.nl + listingTypeName + " projects for " + upx.UserName + ":");
				sb.Append(Util.nl + "project count: " + upu.Project.Count);
				sb.Append(Util.nl);
				sb.Append(ListProjects(upu));
			}
			else
			{
				sb.Append("user not found");
			}
			return sb;
		}

	}
}
