using System.Drawing.Text;
using System.Windows.Forms;
using System.Xml;

namespace TestXMLRead
{
	using System;
	using System.Diagnostics;
	using System.Text;
	using System.Collections.Generic;

	/* basic class structure
	 * 
	 * "master class"
	 * UserProjects - holds the collection of UserProjectsUser
	 * 
	 * UserProjectsUser
	 *      holds field: user name
	 *      holds collection of UserProjecstUserProject
	 * 
	 * UserProjecstUserProject
	 *      holds the individual fields
	 *      for a single project
	 * 
	 * uProject
	 * 
	 */

	

	/// <summary>
	/// Methods for working with a UserProject XML file
	/// </summary>
	/// <remarks>
	/// Method Summary:<br/>
	/// <b>UserExists:</b> Determines if a user exists<br/>
	/// <b>FindUserProjects:</b> Find all projects for a user<br/>
	/// <b>FindUserProjects(ListType):</b> Finds all projects for a user of ListType (All, Active, Inactive)<br/>
	/// </remarks>
	public partial class UserProjects
	{
		public bool UserExists(uProject upx)
		{
			return User.Exists(x => x.Name.Equals(upx.UserName));
		}

		// find a user and all of their projects
		private UserProjectsUser FindUserProjects(uProject upx)
		{
			return User.Find(x => x.Name.Equals(upx.UserName));
		}

		/// <summary>
		/// Find a user and their projects by type - all, active, or inactive
		/// </summary>
		/// <param name="upx">User Project Data</param>
		/// <param name="listProjects">Project Listing Type</param>
		/// <returns></returns>
		public UserProjectsUser FindUserProjects(uProject upx,  ListingTypeStruct listProjects)
		{

			if (listProjects == ListingType.All)
			{
				return FindUserProjects(upx);
			}

			bool active = (listProjects == ListingType.Active);

			UserProjectsUser userProjectsAll = this.FindUserProjects(upx);
			UserProjectsUser userProjectsActive = null;

			if (userProjectsAll != null)
			{
				userProjectsActive = new UserProjectsUser();

				foreach (UserProjectsUserProject p in userProjectsAll.Project)
				{
					if (p.Active == active)
					{
						userProjectsActive.Project.Add(p);
					}
				}
			}
			return userProjectsActive; 
		}

		public bool AddUser(uProject upx)
		{
			if (this.UserExists(upx))
				return false;

			UserProjectsUser user = new UserProjectsUser(upx);

			User.Add(user);

			return true;
		}


		// delete a user if:
		// user exists (and name provided is not empty)
		// user has no projects
		public bool AddProjectToUser(uProject upx)
		{
			if (!this.UserExists(upx))
			{
				// user does not already exist
				// add this user
				if (!this.AddUser(upx))
				{
					return false;
				}
			}

			// get a reference to the user's data
			UserProjectsUser upu = this.FindUserProjects(upx);

			// does the project already exist
			if (upu.FindProject(upx) != null)
			{
				return false;
			}

			// got a UserProjectUser class to hold the new project
			// and the project does not already exist
			// add a project
			upu.AddProject(upx);

			return true;
		}

		public bool DeleteUser(uProject upx)
		{
			if (String.IsNullOrWhiteSpace(upx.UserName) || 
				!this.UserExists(upx))
				return false;

			UserProjectsUser upu = this.userField.Find(x => x.Name.Equals(upx.UserName));

//			if (upu.Project.Count != 0)
//				return false;

			return this.userField.Remove(upu);

		}

		public bool DeleteProjectFromUser(uProject upx)
		{
			// get a reference to the users list of projects
			UserProjectsUser upu = this.FindUserProjects(upx);

			// found nothing, no such user
			if (upu == null || !upu.DeleteProject(upx))
				return false;

			if (upu.Project.Count == 0)
				DeleteUser(upx);

			return true;
		}

		public bool DeleteAllProjectsFromUser(uProject upx)
		{
			// find a user and their projects
			UserProjectsUser upu = FindUserProjects(upx);

			// found nothing, no such user
			if (upu == null)
				return false;

			upu.DeleteAllProjects();

			return true;
		}

		// delete from all users any reference to the project number
		// provided regardless of the task, phase, or building
		public void DeleteProjectFromAllUsersByNumber(uProject upx)
		{
			if (!String.IsNullOrWhiteSpace(upx.ProjNumber.Number))
			{
				List<uProject> usersToDelete = new List<uProject>(0);

				foreach (UserProjectsUser upu in this.userField)
				{
					upu.Project.RemoveAll(x => x.Number.Equals(upx.ProjNumber));

					if (upu.Project.Count == 0)
					{
						usersToDelete.Add(new uProject(upu.Name));
					}
				}

				// now delete any users that ended up with zero projects
				if (usersToDelete.Count > 0)
				{
					// we 1 or more users to delete
					foreach (uProject u in usersToDelete)
					{
						DeleteUser(u);
					}
				}
			}
		}

		// delete from all users any reference to the project number
		// provided regardless of the task, phase, or building
		public void DeleteProjectFromAllUsersByNumberAndTask(uProject upx)
		{
			if (!String.IsNullOrWhiteSpace(upx.ProjNumber.Number))
			{
				List<uProject> usersToDelete = new List<uProject>(0);

				foreach (UserProjectsUser upu in this.userField)
				{
					upu.Project.RemoveAll(x => x.Number.Equals(upx.ProjNumber) && x.Task.Equals(upx.Task));

					if (upu.Project.Count == 0)
					{
						usersToDelete.Add(new uProject(upu.Name));
					}
				}

				// now delete any users that ended up with zero projects
				if (usersToDelete.Count > 0)
				{
					// we 1 or more users to delete
					foreach (uProject u in usersToDelete)
					{
						DeleteUser(u);
					}
				}
			}
		}

		public bool SetUserProjectToCurrent(uProject upx)
		{
			// set the user / project per upx to current
			// find the projects for this user
			UserProjectsUser upu = this.FindUserProjects(upx);

			if (upu == null)
			{
				// no such user
				return false;
			}

			return upu.SetProjectCurrent(upx, true);
		}

		public bool SetUserProjectToNotCurrent(uProject upx)
		{
			// set the user / project per upx to current
			// find the projects for this user
			UserProjectsUser upu = this.FindUserProjects(upx);

			if (upu == null)
			{
				// no such user
				return false;
			}

			return upu.SetProjectCurrent(upx, false);
		}

		public bool SetUserProjectToActive(uProject upx)
		{
			// set the user / project per upx to Active
			UserProjectsUser upu = this.FindUserProjects(upx);

			if (upu == null)
			{
				// no such user
				return false;
			}

			return upu.SetProjectActiveInactive(upx, true);
		}

		public bool SetUserProjectToInactive(uProject upx)
		{
			// set the user / project per the upx to inactive
			UserProjectsUser upu = this.FindUserProjects(upx);

			if (upu == null)
			{
				// user not found
				return false;
			}

			return upu.SetProjectActiveInactive(upx, false);
		}

		public int ProjectCountByUser(uProject upx)
		{
			return this.FindUserProjects(upx).Project.Count;
		}

	}

	public partial class UserProjectsUser
	{
		// construct empty UserProjectsUser
		public UserProjectsUser()
		{
			Name = "";
			Project = new List<UserProjectsUserProject>();
		}

		// construct UserProjectsUser with just a name
		public UserProjectsUser(uProject upx)
		{
			Name = upx.UserName;
			Project = new List<UserProjectsUserProject>();
		}


		// add a project (for a user)
		public void AddProject(uProject upx)
		{
			Project.Add(UserProjectsUserProject.CreateProject(upx));
		}

		// delete a project (from a user)
		public bool DeleteProject(uProject upx)
		{
			UserProjectsUserProject upup = Project.Find(x => (x.Active == upx.Active &&
				x.Number.Equals(upx.ProjNumber) &&
				x.Task.Equals(upx.Task) &&
				x.Phase.Equals(upx.Phase) &&
				x.Building.Equals(upx.Building)));

			return projectField.Remove(upup);
		}

		public void DeleteAllProjects()
		{
			if (this.projectField.Count > 0)
				this.projectField.RemoveRange(0, projectField.Count);
		}

		public UserProjectsUserProject FindProject(uProject upx)
		{
			if (upx.Equals(default(uProject)) || Name != upx.UserName)
				return null;

			foreach (UserProjectsUserProject p in Project)
			{
				if (p.Match(upx))
					return p;
			}
			return null;
		}

		public bool SetProjectActiveInactive(uProject upx, bool active)
		{
			if (projectField == null || projectField.Count == 0)
				return false;

			foreach (UserProjectsUserProject p in projectField)
			{
				if (p.Match(upx))
				{
					p.Active = active;
					return true;
				}
			}

			return true;
		}

		// set a project to current or not current - if it exists
		// insures that only 1 project can be current and allows
		// that all projects can be not-current
		// it is OK to have all projects not-current
		public bool SetProjectCurrent(uProject upx, bool current)
		{
			if (projectField == null || 
				projectField.Count == 0)
				return false;

			bool result = false;

			foreach (UserProjectsUserProject p in projectField)
			{

				result = result || p.Match(upx);

				p.Current = p.Match(upx) && current;
			}

			return result;
		}

		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();

			sb.Append("user: " + this.nameField);
			sb.Append(Util.nl + "projects:");
			

			foreach (UserProjectsUserProject p in this.Project)
			{
				sb.Append(Util.nl);
				sb.Append(p.ToStringWithActive());
			}


			return sb.ToString();
		}
	}


	public partial class UserProjectsUserProject
	{
		public static UserProjectsUserProject CreateProject(uProject upx)
		{
			UserProjectsUserProject upup = new UserProjectsUserProject();

			upup.Current = upx.Current;
			upup.CurrentSpecified = true;
			upup.Active = upx.Active;
			upup.ActiveSpecified = true;
			upup.Number = upx.ProjNumber.Number;
			upup.Task = upx.Task.Number;
			upup.Phase = upx.Phase.Number;
			upup.Building = upx.Building.Number;

			return upup;
		}

		public bool Match(uProject upx)
		{
			return this.Number.Equals(upx.ProjNumber.Number) &&
				this.Task.Equals(upx.Task.Number) &&
				this.Phase.Equals(upx.Phase.Number) &&
				this.Building.Equals(upx.Building.Number);
		}

		public override string ToString()
		{
			string phase = phaseField ?? "";
			string building = buildingField ?? "";

			phase = Util.diamonds.Substring(phase.Length) + phase;
			building = Util.diamonds.Substring(building.Length) + building;

			return String.Format("{0}-{1}-{2}-{3}",
					numberField ?? "", taskField?? "", phase, building);
		}

		public string ToStringWithActive()
		{
//			string active = activeField.ToString();

			string project = this.ToString();

			return String.Format("active? {0} project: {1}", activeField, project);


		}
	}
}
