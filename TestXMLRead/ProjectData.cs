using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Net.Configuration;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Messaging;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace TestXMLRead
{
	/*
	 * Organization:
	 * root class:
	 * ProjectData
	 *   +-> ProjectInformation ->
	 *   +-> CDPackages = list<ProjectDataCDPackageTask>
	 *   |
	 *   v
	 * ProjectDataCDPackageTask
	 *    +-> Task
	 *    +-> Description
	 *    +-> Phase = list<ProjectDataCDPackageTaskPhase>
	 *    | 
	 *    v
	 * ProjectDataCDPackageTaskPhase
	 *    +-> Phase
	 *    +-> Description
	 *    +-> Building = list<ProjectDataCDPackageTaskPhaseBuilding>
	 *    |
	 *    v
	 * ProjectDataCDPackageTaskPhaseBuilding
	 *    +-> Building
	 *    +-> Description
	 *    +-> CDFolder -> PathInfo -> Path
	 *    +-> SheetNumberFormat
	 *    +-> LocationAutoCAD ->
	 *    +-> LocationRevit ->
	 * 
	 * ProjectDataProjectInformation
	 *    +-> ProjectInfo ->
	 *    +-> RootFolder
	 *    
	 * ProjectInfo
	 *    +-> Number
	 *    +-> Description
	 *    
	 * ProjectDataCDPackageTaskPhaseBuildingLocationAutoCAD
	 *    +-> SheetFileFolder -> PathInfo -> Path
	 *    +-> XrefFolder -> PathInfo -> Path
	 *    +-> DetailFolder -> PathInfo -> Path
	 *    +-> BorderFile -> PathInfo -> Path
	 * 
	 * ProjectDataCDPackageTaskPhaseBuildingRevit
	 *    +-> CDModelFile -> PathInfo -> Path
	 *    +-> LibraryModelFile -> PathInfo -> Path
	 *    +-> KeynoteFile -> PathInfo -> Path
	 *    +-> LinkedFolder -> PathInfo -> Path
	 *    +-> XrefFolder -> PathInfo -> Path
	 *		
	 * 
	 *  
	 * needed methods
	 * @ ProjectData
	 * ✔ create project data file => bool (T or F)
	 * 
	 * ✔ Exists(uProject)
	 * 
	 * ✔ Find(uProject) => List<uProject>
	 * 
	 * ✔ GetProjectInfo(uProject) => ProjectDataProjectInformation
	 * ✔ GetShtNumFmt(uProject [specific] ) => string or null 
	 * 
	 * ✔ GetAcadLocationInfo(uProject [specific] ) => ProjectDataCDPackagesTaskPhaseBuildingLocationAutoCAD
	 * ✔ GetRevitLicationInfo(uProject [specific] ) => ProjectDataCDPackagesTaskPhaseBuildingLocationRevit
	 * 
	 * ✔ AddProject(uProjectData) => bool
	 * 
	 * ✔ DeleteProject(uProject) => bool
	 * 
	 * ✔ MakeChangeList(list<uProject> existing info, List<uProject> new info) => ChangeList
	 * 
	 *		change the task / phase / building - number and/ or descriptions
	 * ✔ ChangeProject(ChangeList) => out int[]
	 * 
	 * ✔ ChangeAcadLocation(uProjectData) => bool (adds if does not already exist)
	 * 
	 * ✔ ChangeRevitLocation(uProjectData) => bool (adds if does not already exist)
	 * 
	 * ✔ Sort the data file => void
	 * 
	 * 
	 *         
	 * setCDFolder(task, phase, building) => bool
	 * setShtNumFmt(task, phase, building) = bool
	 * setAcadLocation(uTask, uPhase, uBuilding, 
	 *         ProjectDataCDPackagesTaskPhaseBuildingLocationAutoCAD) => bool
	 * setRevitLocation(uTask, uPhase, uBuilding, 
	 *         ProjectDataCDPackagesTaskPhaseBuildingLocationRevit) => bool
	 * 
	 * deleteTask(task) => bool
	 * deletePhase(task, phase) => bool
	 * deleteBuilding(task, phase, building) => bool
	 * 
	 * hasChildren(task, phase) => bool
	 * 
	 * 
	 * project data
	 *   itemNumber
	 *   itemDescription
	 *   itemList
	 * 
	 * 
	 * 
	 * 
	 */

	public struct FindItem
	{
		public ProjNumInfo Item;
		public List<FindItem> FoundItems;

		public FindItem(ProjNumInfo item, List<FindItem> founditems)
		{
			Item = item;
			FoundItems = founditems;
		}
	}

	public struct ChangeItem : IComparable<ChangeItem> //, IEquatable<ChangeItem>
	{
		public string TskPhBldg;
		public uProject upxExisting;
		public uProject upxNew;

		public ChangeItem(uProject upxexisting, uProject upxnew)
		{
			upxExisting = upxexisting;
			upxNew = upxnew;
			TskPhBldg = Util.FormatTaskPhaseBuilding(upxexisting);
		}

		public int CompareTo(ChangeItem compareItem)
		{
			return TskPhBldg.CompareTo(compareItem.TskPhBldg);
		}

	}

	partial class ProjectData : IPDInfo
	{

		private string XMLFileAndPath;
		protected bool configured = false;

		public string XmlFile
		{
			get { return XMLFileAndPath; }
		}

		public bool Configured
		{
			get { return configured; }
		}

		private void SetXMLFileAndPath(string FileAndPath)
		{
			XMLFileAndPath = FileAndPath;
			configured = true;
		}

		[XmlIgnore]
		public  string itemNumber { get { return this.ProjectInformation.Project.Number; } }

		[XmlIgnore]
		public  string itemDescription { get { return this.ProjectInformation.Project.Description; } }

		[XmlIgnore]
		public override List<ProjectDataCDPackagesTask> itemList
		{
			get { return CDPackages; }
		}

		internal string junk()
		{
			string st = ">" + FindChild("00").itemDescription;

			st += "< :: >" + FindChild("00").FindChild("1").itemDescription + "<";

			return st;
		}
//
//		public List<TestXMLRead.ProjectDataCDPackagesTask> GetItems<ProjectDataCDPackagesTask>()
//		{
//			return CDPackages;
//		}

		// this knows about ProjectDataCDPackages
		public static ProjectData CreateFile(string FileAndPath, uProjectData updx)
		{
			ProjectData pd = new ProjectData();

			pd.ProjectInformation = new ProjectDataProjectInformation(updx);

			pd.SetXMLFileAndPath(FileAndPath);

			pd.Save();

			return pd;
		}

		public static ProjectData LoadFile(string FileAndPath)
		{
			if (!File.Exists(FileAndPath))
			{
				return null;
			}

			ProjectData pd = LoadFromFile(FileAndPath);

			pd.SetXMLFileAndPath(FileAndPath);

			return pd;
		}

		// create a basic, empty file

		// add a new "project" which could mean add a 
		// new task - phase - building
		// existing task / new phase - building
		// existing task / existing phase / new building
		internal bool AddProject(uProject upx)
		{
			if (Exists(upx)) return false;

			ProjectDataCDPackagesTask foundTask = FindTask(upx);
			ProjectDataCDPackagesTaskPhase foundPhase;


			if (foundTask == null)
			{
				cDPackagesField.Add(new ProjectDataCDPackagesTask(upx));
			}
			else
			{
				foundPhase = foundTask.FindPhase(upx);

				if (foundPhase == null)
				{
					foundTask.Phase.Add(new ProjectDataCDPackagesTaskPhase(upx));
				}
				else
				{
					foundPhase.Building.Add(new ProjectDataCDPackagesTaskPhaseBuilding(upx));
				}
			}

			return true;
		}

		internal bool AddProject(uProjectData updx)
		{
			if (Exists(updx.Project)) return false;

			uProject upx = updx.Project;

			ProjectDataCDPackagesTask foundTask = FindTask(upx);
			ProjectDataCDPackagesTaskPhase foundPhase;


			if (foundTask == null)
			{
				cDPackagesField.Add(new ProjectDataCDPackagesTask(updx));
			}
			else
			{
				foundPhase = foundTask.FindPhase(upx);

				if (foundPhase == null)
				{
					foundTask.Phase.Add(new ProjectDataCDPackagesTaskPhase(updx));
				}
				else
				{
					foundPhase.Building.Add(new ProjectDataCDPackagesTaskPhaseBuilding(updx));
				}
			}
			return true;
		}

		internal bool DeleteProject(uProject upx)
		{
			bool result;

			ProjectDataCDPackagesTask foundTask = (FindTask(upx));

			if (foundTask != null)
			{
				result = foundTask.Delete(upx);

				if (foundTask.Phase.Count == 0)
				{
					CDPackages.Remove(foundTask);
				}
			}
			else
			{
				result = false;
			}

			return result;
		}

		internal bool Exists(uProject upx)
		{
			return FindProject(upx) != null;
		}

		public List<uProject> Find(uProject upx, int level = 0)
		{
			List<FindItem> FoundList = FindItems(upx, level);
			List<uProject> uProjects = new List<uProject>();

			foreach (FindItem oneTask in FoundList)
			{
				foreach (FindItem onePhase in oneTask.FoundItems)
				{
					foreach (FindItem oneBldg in onePhase.FoundItems)
					{
						uProjects.Add(new uProject(null, oneTask.Item,
							onePhase.Item, oneBldg.Item));
					}
				}
			}
			return uProjects;
		}

		private ProjectDataCDPackagesTaskPhaseBuilding FindProject(uProject upx)
		{
			ProjectDataCDPackagesTask foundTask = FindChild(upx.Task.Number);
			ProjectDataCDPackagesTaskPhase foundPhase;
			ProjectDataCDPackagesTaskPhaseBuilding foundBldg = null;

			if (foundTask != null)
			{
				foundPhase = foundTask.FindChild(upx.Phase.Number);
				if (foundPhase != null)
				{
					foundBldg = foundPhase.FindChild(upx.Building.Number);
				}
			}
			return foundBldg;
		}

		internal ProjectDataCDPackagesTask FindTask(uProject upx)
		{
			return CDPackages.Find(x => x.Task.Equals(upx.Task.Number));
		}

		internal ProjectDataCDPackagesTask FindTask()
		{
			return CDPackages.Find(x => x.Task.Equals(itemNumber));
		}


		internal bool ChangeProject(List<ChangeItem> changeList, out int[] failed)
		{

			if (changeList == null || changeList.Count == 0)
			{
				failed = new int[1];
				return false;
			}

			failed = new int[changeList.Count];

			int result = 0;

			for (int i = 0; i < changeList.Count; i++)
			{
				failed[i] = ChangeProject(changeList[i].upxExisting, changeList[i].upxNew);

				result += failed[i];
			}

			return result == 0;

		}

		private int ChangeProject(uProject upxExisting, uProject upxNew)
		{
			// existing must exist
			if (!Exists(upxExisting)) return 1;

			// there are (2) types of change 
			// 1: just change the description field(s)
			// 2: change the task / phase / building number

			if (upxExisting.Task.Number.Equals(upxNew.Task.Number) &&
				upxExisting.Phase.Number.Equals(upxNew.Phase.Number) &&
				upxExisting.Building.Number.Equals(upxNew.Building.Number))
			{
				// change type 1
				return ChangeTypeOne(upxExisting, upxNew);
			}
			else
			{
				// change type 2
				return ChangeTypeTwo(upxExisting, upxNew);
			}

			return 0;
		}

		// change project descriptions only
		private int ChangeTypeTwo(uProject upxExisting, uProject upxNew)
		{
			// this means, create a new project using the new information
			// and using the old information
			
			// new cannot already exist
			if (Exists(upxNew)) return 2;

			// verified that the new and old projects either do not or do exist
			// as appropriate
			// make a copy of the existing project's data
			uProjectData updx = Copy(upxExisting);

			// update the information
			updx.Update(upxNew);

			// add a new project
			if (!AddProject(updx)) return -2;

			// delete the old project
			if (!DeleteProject(upxExisting)) return -3;

			return 0;
		}

		private int ChangeTypeOne(uProject upxExisting, uProject upxNew)
		{
			ProjectDataCDPackagesTask foundTask;
			ProjectDataCDPackagesTaskPhase foundPhase;
			ProjectDataCDPackagesTaskPhaseBuilding foundBldg;

			if (GetTaskPhaseBuilding(upxExisting, out foundTask, out foundPhase, out foundBldg))
			{
				if (!ProjNumInfo.DescriptionIsNullOrEmpty(upxNew.Task)) foundTask.Description = upxNew.Task.Description;

				if (!ProjNumInfo.DescriptionIsNullOrEmpty(upxNew.Phase)) foundPhase.Description = upxNew.Phase.Description;

				if (!ProjNumInfo.DescriptionIsNullOrEmpty(upxNew.Building)) foundBldg.Description = upxNew.Building.Description;

				return 0;
			}
			return -1;
		}

		// change project data
		internal bool ChangeProjectInfo(uProject upxExisting, uProjectData updxNew)
		{

			ProjectDataCDPackagesTask foundTask;
			ProjectDataCDPackagesTaskPhase foundPhase;
			ProjectDataCDPackagesTaskPhaseBuilding foundBldg;

			if (GetTaskPhaseBuilding(upxExisting, out foundTask, out foundPhase, out foundBldg))
			{
				// got a specific project to change - determine what data to change
				// choices are:
				// project numbers (this is done last as this will invalidate the found objects)
				// project number descriptions
				// root folder
				// cd folder
				// sheet number format
				// acad location info
				// revit location info

				Debug.Print("has number: " + updxNew.Project.HasNumber());
				Debug.Print("has Description: " + updxNew.Project.HasDescription());





				return true;
			}
			return false;
		}



		internal bool ChangeAcadLocation(uProjectData updx)
		{
			uProject upxExisting = updx.Project;

			if (!Exists(upxExisting)) return false;

			ProjectDataCDPackagesTask foundTask;
			ProjectDataCDPackagesTaskPhase foundPhase;
			ProjectDataCDPackagesTaskPhaseBuilding foundBldg;



			if (GetTaskPhaseBuilding(upxExisting, out foundTask, out foundPhase, out foundBldg))
			{
				if (foundBldg.LocationAutoCAD == null)
				{
					foundBldg.LocationAutoCAD = new ProjectDataCDPackagesTaskPhaseBuildingLocationAutoCAD();
				}

				return foundBldg.LocationAutoCAD.Update(updx);
			}
			return false;
		}

		internal bool ChangeRevitLocation(uProjectData updx)
		{
			uProject upxExisting = updx.Project;

			if (!Exists(upxExisting)) return false;

			ProjectDataCDPackagesTask foundTask;
			ProjectDataCDPackagesTaskPhase foundPhase;
			ProjectDataCDPackagesTaskPhaseBuilding foundBldg;

			if (GetTaskPhaseBuilding(upxExisting, out foundTask, out foundPhase, out foundBldg))
			{
				if (foundBldg.LocationRevit == null)
				{
					foundBldg.LocationRevit = new ProjectDataCDPackagesTaskPhaseBuildingLocationRevit();
				}

				return foundBldg.LocationRevit.Update(updx);
			}
			return false;
		}

		internal uProjectData Copy(uProject upx)
		{
			ProjectDataCDPackagesTask foundTask;
			ProjectDataCDPackagesTaskPhase foundPhase;
			ProjectDataCDPackagesTaskPhaseBuilding foundBldg;

			uProjectData updx = new uProjectData();

			if (GetTaskPhaseBuilding(upx, out foundTask, out foundPhase, out foundBldg))
			{
				
				updx.Project = new uProject();
				updx.Project.ProjNumber = ProjectInformation.Copy();

				updx.RootFolder = ProjectInformation.RootFolder;

				updx.Project.Task = foundTask.Copy();
				updx.Project.Phase = foundPhase.Copy();
				updx.Project.Building = foundBldg.Copy();
				updx.CDFolder = foundBldg.CDFolder;

				ShtNumFmt sht = ShtNumFmt.Find(foundBldg.SheetNumberFormat);

				updx.SheetNumberFormat = sht ?? new ShtNumFmt();
				if (foundBldg.LocationAutoCAD != null)
					updx.AutoCAD = foundBldg.LocationAutoCAD.Copy();

				if (foundBldg.LocationRevit != null)
					updx.Revit = foundBldg.LocationRevit.Copy();
			}

			return updx;
		}


		internal ProjectDataProjectInformation GetProjectInfo()
		{
			return this.ProjectInformation;
		}

		private bool GetTaskPhaseBuilding(uProject upx,
			out ProjectDataCDPackagesTask foundTask,
			out ProjectDataCDPackagesTaskPhase foundPhase,
			out ProjectDataCDPackagesTaskPhaseBuilding foundBldg)
		{

			foundTask = null;
			foundPhase = null;
			foundBldg = null;

			foundTask = FindTask(upx);

			if (foundTask != null)
			{
				foundPhase = foundTask.FindPhase(upx);

				if (foundPhase != null)
				{
					foundBldg = foundPhase.FindBuilding(upx);

					if (foundBldg != null)
					{
						return true;
					}
				}
			}

			return false;
		}

		internal string GetSheetNumberFormat(uProject upx)
		{
			ProjectDataCDPackagesTaskPhaseBuilding result = FindProject(upx);

			return (result != null) ? result.SheetNumberFormat : null;
		}

		internal ProjectDataCDPackagesTaskPhaseBuildingLocationAutoCAD
			GetACADLocationInfo(uProject upx)
		{
			ProjectDataCDPackagesTaskPhaseBuilding result = FindProject(upx);
			return (result != null) ? result.LocationAutoCAD : null;
		}

		internal ProjectDataCDPackagesTaskPhaseBuildingLocationRevit
			GetRevitLocationInfo(uProject upx)
		{
			ProjectDataCDPackagesTaskPhaseBuilding result = FindProject(upx);
			return (result != null) ? result.LocationRevit : null;
		}

		public List<ChangeItem> MakeChangeList(List<uProject> listExisting, List<uProject> listNew)
		{
			if (listExisting.Count != listNew.Count)
				return null;

			List<ChangeItem> ChangeList = new List<ChangeItem>(listExisting.Count);

			for (int j = 0; j < listExisting.Count; j++)
			{
				ChangeList.Add(new ChangeItem(listExisting[j], listNew[j]));
			}

			ChangeList.Sort();

			return ChangeList;
		}

		public void Save()
		{
			this.SaveToFile(XmlFile);
		}

		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();

			sb.Append(this.ProjectInformation.ToString());

			return sb.ToString();
		}
	}





// *****************************************************************
	// a single task - holds the list of phases
	partial class ProjectDataCDPackagesTask : IPDInfo//, IPDInfo2
	{
		[XmlIgnore]
		public  string itemNumber { get { return Task;} }
		
		[XmlIgnore]
		public  string itemDescription { get { return Description;} }

		[XmlIgnore]
		public override List<ProjectDataCDPackagesTaskPhase> itemList
		{
			get { return Phase; }
		}

		// parameterless constructor is required
		internal ProjectDataCDPackagesTask()
		{
		}

		internal ProjectDataCDPackagesTask(string taskName, string taskDesc)
		{
			Task = taskName ?? "";
			Description = taskDesc ?? "";
		}

		internal ProjectDataCDPackagesTask(uProject upx) : this(upx.Task.Number, upx.Task.Description)
		{
			Phase = ProjectDataCDPackagesTaskPhase.Create(upx);
		}

		internal ProjectDataCDPackagesTask(uProjectData updx)
			: this(updx.Project.Task.Number, updx.Project.Task.Description)
		{
			Phase = ProjectDataCDPackagesTaskPhase.Create(updx);
		}

		internal static List<ProjectDataCDPackagesTask> Create(uProject upx)
		{
			List<ProjectDataCDPackagesTask> t =
				new List<ProjectDataCDPackagesTask>();

			t.Add(new ProjectDataCDPackagesTask(upx));

			return t;
		}

		internal ProjNumInfo Copy()
		{
			return new ProjNumInfo(Task, Description);
		}

		internal bool Delete(uProject upx)
		{
			bool result = true;

			ProjectDataCDPackagesTaskPhase foundPhase = FindPhase(upx);

			if (foundPhase != null)
			{
				result = foundPhase.Delete(upx);

				if (foundPhase.Building.Count == 0)
				{
					Phase.Remove(foundPhase);
				}
			}
			else
			{
				result = false;
			}

			return result;
		}

		public ProjectDataCDPackagesTaskPhase FindPhase(uProject upx)
		{
			return this.Phase.Find(x => x.Phase.Equals(upx.Phase.Number));
		}

		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();

			sb.Append(Util.FormatItemN(Util.column, "Task", this.Task));
			sb.Append(Util.FormatItemN(Util.column, "Description", this.Description));

			return sb.ToString();
		}
	}




// *****************************************************************
	// a single phase - holds the list of buildings
	public partial class ProjectDataCDPackagesTaskPhase : IPDInfo//, IPDInfo2
	{
		[XmlIgnore]
		public  string itemNumber { get { return Phase; } }
		
		[XmlIgnore]
		public  string itemDescription { get { return Description; } }

		[XmlIgnore]
		public override List<ProjectDataCDPackagesTaskPhaseBuilding> itemList
		{
			get { return Building;}
		}

		// parameterless constructor is required
		internal ProjectDataCDPackagesTaskPhase()
		{
		}

		internal ProjectDataCDPackagesTaskPhase(string phase, string phasedesc)
		{
			Phase = phase ?? "";
			Description = phasedesc ?? "";
		}

		internal ProjectDataCDPackagesTaskPhase(uProject upx)
			: this(upx.Phase.Number, upx.Phase.Description)
		{
			Building = ProjectDataCDPackagesTaskPhaseBuilding.Create(upx);
		}

		internal ProjectDataCDPackagesTaskPhase(uProjectData updx)
			: this(updx.Project.Phase.Number, updx.Project.Phase.Description)
		{
			Building = ProjectDataCDPackagesTaskPhaseBuilding.Create(updx);
		}

		internal static List<ProjectDataCDPackagesTaskPhase> Create(uProject upx)
		{
			List<ProjectDataCDPackagesTaskPhase> p =
				new List<ProjectDataCDPackagesTaskPhase>();

			p.Add(new ProjectDataCDPackagesTaskPhase(upx));

			return p;
		}

		internal static List<ProjectDataCDPackagesTaskPhase> Create(uProjectData updx)
		{
			List<ProjectDataCDPackagesTaskPhase> p =
				new List<ProjectDataCDPackagesTaskPhase>();

			p.Add(new ProjectDataCDPackagesTaskPhase(updx));

			return p;
		}

		internal ProjNumInfo Copy()
		{
			return new ProjNumInfo(Phase, Description);
		}

		internal bool Delete(uProject upx)
		{
			bool result;

			ProjectDataCDPackagesTaskPhaseBuilding foundBuilding = FindBuilding(upx);

			if (foundBuilding != null)
			{
				result = Building.Remove(foundBuilding);
			}
			else
			{
				result = false;
			}

			return result;
		}

		internal ProjectDataCDPackagesTaskPhaseBuilding FindBuilding(uProject upx)
		{
			return Building.Find(x => x.Building.Equals(upx.Building.Number));
		}

		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();

			sb.Append(Util.FormatItemN(Util.column, "Phase", this.Phase));
			sb.Append(Util.FormatItemN(Util.column, "Description", this.Description));

			return sb.ToString();
		}
	}






// *****************************************************************
	// a single building - holds information about the programs
	public partial class ProjectDataCDPackagesTaskPhaseBuilding : IPDInfo//, IPDInfo2
	{
		[XmlIgnore]
		public string itemNumber { get { return Building; } }

		[XmlIgnore]
		public string itemDescription { get { return Description; } }

		// parameterless constructor is required
		internal ProjectDataCDPackagesTaskPhaseBuilding()
		{
		}

		internal ProjectDataCDPackagesTaskPhaseBuilding(string bldg, string bldgdesc)
		{
			Building = bldg ?? "";
			Description = bldgdesc ?? "";
		}

		internal ProjectDataCDPackagesTaskPhaseBuilding(uProject upx)
			: this(upx.Building.Number, upx.Building.Description)
		{
			CDFolder = "";
			SheetNumberFormat = "";
			LocationAutoCAD = new ProjectDataCDPackagesTaskPhaseBuildingLocationAutoCAD();
			LocationRevit = new ProjectDataCDPackagesTaskPhaseBuildingLocationRevit();
		}

		internal ProjectDataCDPackagesTaskPhaseBuilding(uProjectData updx)
			: this(updx.Project.Building.Number, updx.Project.Building.Description)
		{
			CDFolder = updx.CDFolder;
			SheetNumberFormat = updx.SheetNumberFormat.Format;
			LocationAutoCAD = new ProjectDataCDPackagesTaskPhaseBuildingLocationAutoCAD(updx);
			LocationRevit = new ProjectDataCDPackagesTaskPhaseBuildingLocationRevit(updx);
		}

		internal static List<ProjectDataCDPackagesTaskPhaseBuilding> Create(uProject upx)
		{
			List<ProjectDataCDPackagesTaskPhaseBuilding> b =
				new List<ProjectDataCDPackagesTaskPhaseBuilding>();

			b.Add(new ProjectDataCDPackagesTaskPhaseBuilding(upx));

			return b;
		}

		internal static List<ProjectDataCDPackagesTaskPhaseBuilding> Create(uProjectData updx)
		{
			List<ProjectDataCDPackagesTaskPhaseBuilding> b =
				new List<ProjectDataCDPackagesTaskPhaseBuilding>();

			b.Add(new ProjectDataCDPackagesTaskPhaseBuilding(updx));

			return b;
		}

		internal ProjNumInfo Copy()
		{
			return new ProjNumInfo(Building, Description);
		}

		public List<FindItem> FindItems(uProject upx, int level)
		{
			return null;
		}

		public void Sort()
		{
			return;
		}

		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();

			sb.Append(Util.FormatItemN(Util.column, "Building", this.Building));
			sb.Append(Util.FormatItemN(Util.column, "Description", this.Description));

			return sb.ToString();
		}
	}




// *****************************************************************
	// holds data about one autocad location
	public partial class ProjectDataCDPackagesTaskPhaseBuildingLocationAutoCAD
	{
		private string nl = Util.nl;

		internal ProjectDataCDPackagesTaskPhaseBuildingLocationAutoCAD()
		{
			SheetFileFolder = new PathInfo();
			XrefFolder = new PathInfo();
			DetailFolder = new PathInfo();
			BorderFile = new PathInfo();
		}

		internal ProjectDataCDPackagesTaskPhaseBuildingLocationAutoCAD(uProjectData updx)
		{
			uProjectDataAutoCAD ax = updx.AutoCAD;

			SheetFileFolder = new PathInfo(ax.SheetFileFolder);
			XrefFolder = new PathInfo(ax.XrefFolder);
			DetailFolder = new PathInfo(ax.DetailFolder);
			BorderFile = new PathInfo(ax.BorderFile);
		}

		internal uProjectDataAutoCAD Copy()
		{
			uProjectDataAutoCAD ax = new uProjectDataAutoCAD();

			ax.SheetFileFolder = SheetFileFolder.Path;
			ax.XrefFolder = XrefFolder.Path;
			ax.DetailFolder = DetailFolder.Path;
			ax.BorderFile = BorderFile.Path;

			return ax;

		}

		public bool Update(uProjectData updx)
		{
			if (!updx.Valid) return false;

			bool result = false;

			if (!String.IsNullOrWhiteSpace(updx.AutoCAD.SheetFileFolder))
			{
				SheetFileFolder.Path = updx.AutoCAD.SheetFileFolder;
				result = true;
			}

			if (!String.IsNullOrWhiteSpace(updx.AutoCAD.XrefFolder))
			{
				XrefFolder.Path = updx.AutoCAD.XrefFolder;
				result = true;
			}

			if (!String.IsNullOrWhiteSpace(updx.AutoCAD.DetailFolder))
			{
				DetailFolder.Path = updx.AutoCAD.DetailFolder;
				result = true;
			}

			if (!String.IsNullOrWhiteSpace(updx.AutoCAD.BorderFile))
			{
				BorderFile.Path = updx.AutoCAD.BorderFile;
				result = true;
			}

			return result;
		}

		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();

			sb.Append(Util.FormatItemDividerN());
			sb.Append(Util.FormatItemN(Util.column, "AutoCAD Location Information"));

			Util.column += Util.columnAdjust;

			sb.Append(Util.FormatItemN(Util.column, "SheetFileFolder", this.SheetFileFolder.Path));
			sb.Append(Util.FormatItemN(Util.column, "XrefFolder", this.XrefFolder.Path));
			sb.Append(Util.FormatItemN(Util.column, "DetailFolder", this.DetailFolder.Path));
			sb.Append(Util.FormatItemN(Util.column, "BorderFile", this.BorderFile.Path));

			Util.column -= Util.columnAdjust;

			return sb.ToString();
		}
	}




// *****************************************************************
	// holds data about one revit location
	public partial class ProjectDataCDPackagesTaskPhaseBuildingLocationRevit
	{
		private string nl = Util.nl;


		internal ProjectDataCDPackagesTaskPhaseBuildingLocationRevit()
		{
			CDModelFile = new PathInfo();
			LibraryModelFile = new PathInfo();
			KeynoteFile = new PathInfo();
			LinkedFolder = new PathInfo();
			XrefFolder = new PathInfo();
		}

		internal ProjectDataCDPackagesTaskPhaseBuildingLocationRevit(uProjectData updx)
		{
			uProjectDataRevit rx = updx.Revit;

			CDModelFile = new PathInfo(rx.CDModelFile);
			LibraryModelFile = new PathInfo(rx.LibraryModelFile);
			KeynoteFile = new PathInfo(rx.KeynoteFile);
			LinkedFolder = new PathInfo(rx.LinkedFolder);
			XrefFolder = new PathInfo(rx.XrefFolder);
		}

		internal uProjectDataRevit Copy()
		{
			uProjectDataRevit rx = new uProjectDataRevit();

			rx.CDModelFile = CDModelFile.Path;
			rx.LibraryModelFile = LibraryModelFile.Path;
			rx.KeynoteFile = KeynoteFile.Path;
			rx.LinkedFolder = LinkedFolder.Path;
			rx.XrefFolder = XrefFolder.Path;

			return rx;
		}

		public bool Update(uProjectData updx)
		{
			if (!updx.Valid) return false;

			bool result = false;

			if (!String.IsNullOrWhiteSpace(updx.Revit.CDModelFile))
			{
				CDModelFile.Path = updx.Revit.CDModelFile;
				result = true;
			}

			if (!String.IsNullOrWhiteSpace(updx.Revit.LibraryModelFile))
			{
				LibraryModelFile.Path = updx.Revit.LibraryModelFile;
				result = true;
			}

			if (!String.IsNullOrWhiteSpace(updx.Revit.KeynoteFile))
			{
				KeynoteFile.Path = updx.Revit.KeynoteFile;
				result = true;
			}

			if (!String.IsNullOrWhiteSpace(updx.Revit.LinkedFolder))
			{
				LinkedFolder.Path = updx.Revit.LinkedFolder;
				result = true;
			}

			if (!String.IsNullOrWhiteSpace(updx.Revit.XrefFolder))
			{
				XrefFolder.Path = updx.Revit.XrefFolder;
				result = true;
			}

			return result;
		}


		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();

			int column = Util.column;

			sb.Append(Util.FormatItemN(Util.column, "Revit Location Information"));

			Util.column += Util.columnAdjust;

			sb.Append(Util.FormatItemN(Util.column, "CDModelFile", this.CDModelFile.Path));
			sb.Append(Util.FormatItemN(Util.column, "LibraryModelFile", this.LibraryModelFile.Path));
			sb.Append(Util.FormatItemN(Util.column, "KeynoteFile", this.KeynoteFile.Path));
			sb.Append(Util.FormatItemN(Util.column, "LinkedFolder", this.LinkedFolder.Path));
			sb.Append(Util.FormatItemN(Util.column, "XrefFolder", this.XrefFolder.Path));

			Util.column -= Util.columnAdjust;

			return sb.ToString();
		}
	}




// *****************************************************************
	// holds the project information data
	partial class ProjectDataProjectInformation
	{
		private string nl = Util.nl;

		internal ProjectDataProjectInformation()
		{
		}

		internal ProjectDataProjectInformation(uProjectData updx)
		{
			Project = new ProjectInfo(updx);
			RootFolder = updx.RootFolder;
		}

		internal ProjNumInfo Copy()
		{
			return new ProjNumInfo(Project.Number, Project.Description);
		}

		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();

			sb.Append(this.Project.ToString());
			sb.Append(Util.FormatItemN("Root folder", this.RootFolder));

			return sb.ToString();
		}
	}


// *****************************************************************
	// holds project information
	partial class ProjectInfo
	{
		private string nl = Util.nl;

		internal ProjectInfo()
		{
		}

		internal ProjectInfo(uProjectData updx)
		{
			Number = updx.Project.ProjNumber.Number;
			Description = updx.Project.ProjNumber.Description;
		}

		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();

			sb.Append(Util.FormatItemN("Project number", this.Number));
			sb.Append(Util.FormatItemN("Description", this.Description));

			return sb.ToString();
		}
	}

// *****************************************************************
	// holds project information
	partial class PathInfo
	{
		internal PathInfo()
		{
			Path = "";
		}

		internal PathInfo(string path)
		{
			Path = path;
		}

	}



	public class EntityBase1<T> : EntityBase<T>
	{
		internal string nl = Util.nl;
	}

	public abstract class EntityBase2<T1, T2> : EntityBase1<T1> where T2 : class, IPDInfo
	{
		[XmlIgnore]
		public abstract List<T2> itemList { get; }

		public T2 FindChild(string number)
		{
			if (number == null) { return null; }

			return itemList.Find(x => x.itemNumber.Equals(number));
		}

		public List<FindItem> FindItems(uProject upx, int level)
		{
			List<FindItem> FoundList = new List<FindItem>();

			level++;

			if (ProjNumInfo.NumberIsAll(upx[level]))
			{
				foreach (T2 oneItem in itemList)
				{
					List<FindItem> foundItems = oneItem.FindItems(upx, level);

					FoundList.Add(new FindItem(
						new ProjNumInfo(oneItem.itemNumber, oneItem.itemDescription), foundItems));
				}
			}
			else
			{
				if (upx[level] != null)
				{
					T2 oneItem = FindChild(upx[level].Number);

					if (oneItem != null)
					{
						List<FindItem> foundItems = oneItem.FindItems(upx, level);
						FoundList.Add(new FindItem(
							new ProjNumInfo(oneItem.itemNumber, oneItem.itemDescription), foundItems));
					}
				}
			}
			return FoundList;
		}

		public void Sort()
		{
			if (itemList == null
				|| itemList.Count == 0) return;

			foreach (T2 oneItem in itemList)
			{
				oneItem.Sort();
			}

			itemList.Sort((x,y) => x.itemNumber.CompareTo(y.itemNumber));

		}

	}

}
