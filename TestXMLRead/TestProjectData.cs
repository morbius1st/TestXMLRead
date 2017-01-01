using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;
using System.Xml;
using System.Xml.XPath;


namespace TestXMLRead
{
	public class TestProjectData
	{
		private string nl = Util.nl;
		private static string ProjectFile = @"C:\2099-999 Sample Project\CD\.config";
		private static string xmlFileName = "8999 ProjectData.xml";
		private static string xmlProjectFile = string.Concat(ProjectFile, "\\", xmlFileName);

		private static string xmlTestFileName = "xxxx ProjectData.xml";
		private static string xmlTestProjectFile = string.Concat(ProjectFile, "\\", xmlTestFileName);

		List<ChangeItem> changeList = new List<ChangeItem>();
			

		public StringBuilder ProjectDataTests()
		{
			StringBuilder sb = new StringBuilder();

			uProjectData updxNew = AssignProjDataNew();


			ProjectData pd = ProjectData.LoadFile(xmlProjectFile);

			List<uProject> upList;


			// existing @both
			uProject upAll = new uProject(null, new ProjNumInfo(Util.all),
				new ProjNumInfo(Util.all), new ProjNumInfo(Util.all));

			uProject up_00_all_all = new uProject(null, new ProjNumInfo("00"),
				new ProjNumInfo(Util.all), new ProjNumInfo(Util.all));

			uProject up_all_1_all = new uProject(null, new ProjNumInfo(Util.all),
				new ProjNumInfo("1"), new ProjNumInfo(Util.all));

			uProject up_mt_mt_mt = new uProject(null, new ProjNumInfo(""),
				new ProjNumInfo(""), new ProjNumInfo(""));

			uProject up_null_null_null = new uProject();



			// existing @8999
			uProject up_02_1_all = new uProject(null, new ProjNumInfo("02"),
				new ProjNumInfo("1"), new ProjNumInfo(Util.all));

			uProject up_02_1_C = new uProject(null, new ProjNumInfo("02"),
				new ProjNumInfo("1"), new ProjNumInfo("C"));
			uProject up_02_1_B = new uProject(null, new ProjNumInfo("02"),
				new ProjNumInfo("1"), new ProjNumInfo("B"));


			ProjNumInfo px = new ProjNumInfo("2099-999", "TestX");
			// existing @xxxx
			uProject up_00_all_B = new uProject(px, new ProjNumInfo("00"),
				new ProjNumInfo(Util.all), new ProjNumInfo("B"));

			uProject up_02_5_G = new uProject(px, new ProjNumInfo("02", "Task 02"),
				new ProjNumInfo("5", "Phase 05"), new ProjNumInfo("G", "Building G"));

			uProject up_02_5_H = new uProject(px, new ProjNumInfo("02", "Task 02"),
				new ProjNumInfo("5", "Phase 05"), new ProjNumInfo("H", "Building H"));

			uProject up_00_1_F = new uProject(px, new ProjNumInfo("00", "Task 00"),
				new ProjNumInfo("1", "Phase 01"), new ProjNumInfo("F", "Building F"));

			uProject up_00_6_F = new uProject(px, new ProjNumInfo("00", "Task 00"),
				new ProjNumInfo("6", "Phase F"), new ProjNumInfo("F", "Building F"));

			uProject up_00_mt_A = new uProject(px, new ProjNumInfo("00", "Task 00"),
				new ProjNumInfo("", "Phase none"), new ProjNumInfo("A", "Building A"));

			uProject up_00_mt_B = new uProject(px, new ProjNumInfo("00", "Task 00"),
				new ProjNumInfo("", "Phase none"), new ProjNumInfo("B", "Building B"));

			uProject up_01_mt_mt = new uProject(px, new ProjNumInfo("01", "Task 01"),
				new ProjNumInfo("", "No Phase"), new ProjNumInfo("", "No Building"));


			// new
			uProject up_00_mt_C = new uProject(px, new ProjNumInfo("00", "Task 00"),
				new ProjNumInfo("", "Phase none"), new ProjNumInfo("C", "Building C"));

			uProject up_01_1_mt = new uProject(px, new ProjNumInfo("01"),
				new ProjNumInfo("1"), new ProjNumInfo(""));

			uProject up_00_2_C = new uProject(px, new ProjNumInfo("00", "Task 00"),
				new ProjNumInfo("2", "Phase 2"), new ProjNumInfo("C", "Building C"));

			uProject up_00_2_D = new uProject(px, new ProjNumInfo("00", "Task 00"),
				new ProjNumInfo("2", "Phase 2"), new ProjNumInfo("D", "Building D"));

			uProject up_03_0_C = new uProject(px, new ProjNumInfo("03", "Task 03"),
				new ProjNumInfo("0", "Phase 0"), new ProjNumInfo("C", "Building C"));

			uProject up_00_mt_A_desc = new uProject(px, new ProjNumInfo("00", "Task 00"),
				new ProjNumInfo("", "Phase does not apply"), new ProjNumInfo("A", "Building A"));

			uProject up_00_mt_B_desc = new uProject(px, new ProjNumInfo("00", "Task 00"),
				new ProjNumInfo("", "Phase does not apply"), new ProjNumInfo("B", "Building B"));


			uProjectData upd01;
			uProjectData upd02;

			List<uProject> upListExist01;
			List<uProject> upListNew01;

			
			
			string[] tests = new[] { "101" };	// default


			bool[] programsBoth = {true, true};
			bool[] programsAcad = { true, false };
			bool[] programsRevit = { false, true };

			bool[] programs = programsBoth;


			

//			tests = new[] { "0", "X", "41" };						// test creating a file

//			tests = new[] { "D", "2", "3", "D" };		// xml file information (must be loaded first)

//			tests = new[] { "41"};					// display whole file

//			tests = new[] { "51", "D" };			// list specific task
//			tests = new[] { "52", "D" };			// list specific task & phase

//			tests = new[] { "61", "D" };			// list specific phase

//			tests = new[] { "101" };				// find all (standard)

//			tests = new[] { "102" };				// find specific task

//			tests = new[] { "103" };				// alt find all projects

//			tests = new[] { "104" };				// alt find

//			tests = new[] { "105" };				// alt find

//			tests = new[] { "106" };				// alt find all mt

//			tests = new[] { "107" };				// alt find all null

//			tests = new[] { "111", "101" };			// find specific task / phase

//			tests = new[] { "121", "D", "41" };		// find specific task / Bldg

//			tests = new[] { "122", "101" };			// find with empty value

//			tests = new[] { "X", "122", "101" };	// find with empty value

//			tests = new[] { "131", "D", "41" };		// find specific task / phase / bldg

//			tests = new[] { "132", "D", "41" };		// find specific task / phase / bldg

//			tests = new[] {"101", "102", "111", "121", "131", "132"};

//			tests = new[] { "151"};					// exists: all - false

//			tests = new[] { "152"};					// exists: specific task - false

//			tests = new[] { "161" };				// exists: specific task / phase / bldg

//			tests = new[] { "151", "152", "161" };	// 

//			tests = new[] { "201" };				// get project information

//			tests = new[] {"211"};					// get sheet number format: non-specific

//			tests = new[] { "212" };				// get sheet number format: specific task / phase / bldg

//			tests = new[] { "221", "222" };			// get ACAD Location: non-specific & specific task / phase / bldg

//			tests = new[] { "231", "232" };			// get Revit Location: non-specific & specific task / phase / bldg

//			tests = new[] { "222", "232" };			// get Location: specific: ACAD & Revit

//			tests = new[] { "301", "41" };			// add a new project (task, phase, & building)

//			tests = new[] { "X", "101", "320", "101", 
//				"E", "101" };	// load test file and delete one project

//			tests = new[] { "X", "101", "321", "101", "322", "101" };	// load test file and delete one project

//			tests = new[] { "X", "101", "323", "101" };		// delete one

//			tests = new[] { "X", "101", "307", "101", "323", "101" };	// change test - add then delete

//			tests = new[] { "X", "101", "351", "101" };		// create a change list

//			tests = new[] { "X", "101", "351", "352", "101" };

			// test: create whole new project
//			tests = new[] { "0", "X", "301", "302", "303", "304", "305", "306", "S", "41" };

			// copy project
//			tests = new[] {"X", "41", "330", "41"};

			// copy project data
//			tests = new[] { "X", "331"};

			// change some projects number(s)
//			tests = new[] { "X", "101", "351", "361", "101" };

			// change some project desciption(s)
//			tests = new[] { "X", "101", "352", "361", "101" };

			// change some projects, save, load, display
//			tests = new[] { "X", "101", "351", "361", "S", "E", "101", "X", "101"};

			// change acad location info
//			tests = new[] {"371"};

			// change revit location info
//			tests = new[] { "372" };

			// test the generic method
//			tests = new[] { "junk" };

//			// sort the data using the x-file and save
//			tests = new[] { "X", "101", "380", "101", "S" };

			// sort the data using the primary file
//			tests = new[] { "101", "380", "101" };

			// add location information in the x-file
//			tests = new[] { "X", "390" };

			// general change project information
			tests = new[] { "X", "400" };




			// begin testing
			sb.Append(Util.FormatItemDivider());
			sb.Append(Util.FormatItemN("running tests:", ""));

			int i = 1;

			foreach (string test in tests)
			{
				sb.Append(test);

				if (i++ != tests.Length)
					sb.Append(" :: ");
			}

			sb.Append(Util.FormatItemDivider());

			StringBuilder sbTemp = new StringBuilder();
			uProject up01;
			uProject up02;

			foreach (string test in tests)
			{
				switch (test)
				{
					case "E":	// load the base example file
						pd = ProjectData.LoadFile(xmlProjectFile);
						break;
					
					case "D":	// divider
						sb.Append(Util.FormatItemDividerN());
						break;

					case "S":
						pd.Save();
						sb.Append(Util.FormatItemN("saving file to", pd.XmlFile));
						break;

					case "junk":
						sb.Append(nl).Append(pd.junk());
						break;

					case "X":	// load the alternate example file
						sb.Append(Util.FormatItemN("loading file", xmlTestProjectFile));
						pd = ProjectData.LoadFile(xmlTestProjectFile);
						break;

					case "0":	// create new file
						FileInfo F = new FileInfo(xmlTestProjectFile);
						ProjectData pdNew = ProjectData.CreateFile(xmlTestProjectFile, updxNew);
						sb.Append(Util.FormatItemN("File Created?", (F.Length != 0).ToString()));
						break;

					case "2":	// show loaded file xml file name
						sb.Append(Util.FormatItem("xmlFile", pd.XmlFile));
						break;
					case "3":	// show if project data is configured
						sb.Append(Util.FormatItem("configured", pd.Configured.ToString()));
						break;

					case "11":	// list the project information
						ProjectDataProjectInformation pi = pd.ProjectInformation;
						sb.Append(pi.ToString());
						break;
					case "21":	// list progm
						sb.Append(ListProgm());
						break;
					case "22":	// list the sheet number formats
						sb.Append(ListShtNumFmt());
						break;
//					case "31":	// list an autocad project data
//						sb.Append(uPDAcad.ToString());
//						break;
//					case "32":	// list a revit project data
//						sb.Append(uPDRevit.ToString());
//						break;
					case "41":	// list all project information 
						sb.Append(ListProjectData(upAll, pd));
						break;
					case "51":	// list project information for a task
						sb.Append(ListProjectData(up_00_all_all, pd));
						break;
//					case "52":	// list project information for a task
//						sb.Append(pd.ListProjectData(up_02_1_all, programs));
//						break;
//					case "61":	// list project information for a task
//						sb.Append(pd.ListProjectData(up_all_1_all, programs));
//						break;
//					case "91":	// list project information for a task
//						sb.Append(pd.ListProjectData(up_02_1_C, programs));
//						break;
					case "101": // find projects: all
						up01 = upAll;
						sb.Append(Util.FormatItemN("data file", pd.XmlFile));
						sb.Append(Util.FormatItemN("finding", FormatProject(up01).ToString()));
						upList = pd.Find(up01);
						sb.Append(Util.FormatItemN("found items", upList.Count.ToString()));
						sb.Append(FormatProjectList(upList));
						break;
					case "102": // find projects: per task
						upList = pd.Find(up_00_all_all);
						sb.Append(Util.FormatItemN("found items", upList.Count.ToString()));
						sb.Append(FormatProjectList(upList));
						break;
					case "103": // alt find projects: all
						up01 = upAll;
						sb.Append(Util.FormatItemN("data file", pd.XmlFile));
						sb.Append(Util.FormatItemN("finding", FormatProject(up01).ToString()));
						upList = pd.Find(up01);
						sb.Append(Util.FormatItemN("found items", upList.Count.ToString()));
						sb.Append(FormatProjectList(upList));
						break;
					case "104": // find projects: specific task / phase / bldg
						upList = pd.Find(up_02_1_C);
						sb.Append(Util.FormatItemN("found items", upList.Count.ToString()));
						sb.Append(FormatProjectList(upList));
						break;
					case "105": // find projects: specific task
						upList = pd.Find(up_00_all_all);
						sb.Append(Util.FormatItemN("found items", upList.Count.ToString()));
						sb.Append(FormatProjectList(upList));
						break;
					case "106": // find projects: specific task
						upList = pd.Find(up_mt_mt_mt);
						sb.Append(Util.FormatItemN("found items", upList.Count.ToString()));
						sb.Append(FormatProjectList(upList));
						break;
					case "107": // find projects: specific task
						upList = pd.Find(up_null_null_null);
						sb.Append(Util.FormatItemN("found items", upList.Count.ToString()));
						sb.Append(FormatProjectList(upList));
						break;

					case "111":
						up01 = up_02_1_all;
						sb.Append(Util.FormatItemN("finding", FormatProject(up01).ToString()));
						upList = pd.Find(up01);
						sb.Append(Util.FormatItemN("found items", upList.Count.ToString()));
						sb.Append(FormatProjectList(upList));
						break;
					case "121":
						upList = pd.Find(up_00_all_B);
						sb.Append(Util.FormatItemN("found items", upList.Count.ToString()));
						sb.Append(FormatProjectList(upList));
						break;
					case "122":
						up01 = up_00_mt_A;
						upList = pd.Find(up01);
						sb.Append(Util.FormatItemN("finding", FormatProject(up01).ToString()));
						sb.Append(Util.FormatItemN("found items", upList.Count.ToString()));
						sb.Append(FormatProjectList(upList));
						break;
					case "131":
						upList = pd.Find(up_02_1_C);
						sb.Append(Util.FormatItemN("found items", upList.Count.ToString()));
						sb.Append(FormatProjectList(upList));
						break;
					case "132":
						upList = pd.Find(up_02_1_B);
						sb.Append(Util.FormatItemN("found items", upList.Count.ToString()));
						sb.Append(FormatProjectList(upList));
						break;
					case "151":
						sb.Append(Util.FormatItemN("this project",""));
						sb.Append(FormatProject(upAll));
						sb.Append(Util.FormatItemN("found?", pd.Exists(upAll).ToString()));
						break;
					case "152":
						sb.Append(Util.FormatItemN("this project", ""));
						sb.Append(FormatProject(up_00_all_all));
						sb.Append(Util.FormatItemN("found?", pd.Exists(up_00_all_all).ToString()));
						break;
					case "161":
						sb.Append(Util.FormatItemN("this project", ""));
						sb.Append(FormatProject(up_02_1_C));
						sb.Append(Util.FormatItemN("found?", pd.Exists(up_02_1_C).ToString()));
						break;
					case "201":
						sb.Append(FormatProjectInfo(pd.GetProjectInfo()));
						break;
					case "211":
						sbTemp.Append("sht num fmt for: ").Append(FormatProject(upAll));

						sb.Append(Util.FormatItemN(sbTemp.ToString(),
						pd.GetSheetNumberFormat(upAll) ?? "is null"));
						break;
					case "212":
						sbTemp.Append("sht num fmt for: ").Append(FormatProject(up_02_1_C));

						sb.Append(Util.FormatItemN(sbTemp.ToString(),
						pd.GetSheetNumberFormat(up_02_1_C) ?? "is null"));
						break;

					case "221":
						sbTemp.Append("ACAD Loc for: ").Append(FormatProject(upAll));
						sb.Append(Util.FormatItemN(sbTemp.ToString(), ""));

						sb.Append(FormatACADLocationInfo(pd.GetACADLocationInfo(upAll)));
						break;

					case "222":
						sbTemp.Append("ACAD Loc for: ").Append(FormatProject(up_02_1_B));
						sb.Append(Util.FormatItemN(sbTemp.ToString(), ""));

						sb.Append(FormatACADLocationInfo(pd.GetACADLocationInfo(up_02_1_B)));
						break;

					case "231":
						sbTemp.Append("Revit Loc for: ").Append(FormatProject(upAll));
						sb.Append(Util.FormatItemN(sbTemp.ToString(), ""));

						sb.Append(FormatRevitLocationInfo(pd.GetRevitLocationInfo(upAll)));
						break;

					case "232":
						sbTemp.Append("Revit Loc for: ").Append(FormatProject(up_02_1_B));
						sb.Append(Util.FormatItemN(sbTemp.ToString(), ""));

						sb.Append(FormatRevitLocationInfo(pd.GetRevitLocationInfo(up_02_1_B)));
						break;

					case "301":
						up01 = up_02_5_G;
						sb.Append(CreateNewProject(pd, up01));
						break;

					case "302":
						up01 = up_02_5_H;
						sb.Append(CreateNewProject(pd, up01));
						break;

					case "303":
						up01 = up_00_1_F;
						sb.Append(CreateNewProject(pd, up01));
						break;

					case "304":
						up01 = up_00_mt_A;
						sb.Append(CreateNewProject(pd, up01));
						break;

					case "305":
						up01 = up_00_mt_B;
						sb.Append(CreateNewProject(pd, up01));
						break;
						
					case "306":
						up01 = up_01_mt_mt;
						sb.Append(CreateNewProject(pd, up01));
						break;

					case "307":
						up01 = up_00_mt_C;
						sb.Append(CreateNewProject(pd, up01));
						break;

					case "320":
						up01 = up_02_5_H;
						sb.Append(Util.FormatItemN("Deleting project", FormatProject(up01).ToString()));
						sb.Append(Util.FormatItemN("deleted?", pd.DeleteProject(up01).ToString()));
						break;

					case "321":
						up01 = up_01_1_mt;
						sb.Append(Util.FormatItemN("Deleting project", FormatProject(up01).ToString()));
						sb.Append(Util.FormatItemN("deleted?", pd.DeleteProject(up01).ToString()));
						break;

					case "322":
						up01 = up_01_mt_mt;
						sb.Append(Util.FormatItemN("Deleting project", FormatProject(up01).ToString()));
						sb.Append(Util.FormatItemN("deleted?", pd.DeleteProject(up01).ToString()));
						break;

					case "323":
						up01 = up_00_mt_A;
						sb.Append(Util.FormatItemN("Deleting project", FormatProject(up01).ToString()));
						sb.Append(Util.FormatItemN("deleted?", pd.DeleteProject(up01).ToString()));
						break;

					case "330":		// copy project information & change
						up01 = up_02_5_G;
						up02 = up_03_0_C;
						upd02 = pd.Copy(up01);

						// got copy - change names
						upd02.Project.Task = up02.Task.Clone();
						upd02.Project.Phase = up02.Phase.Clone();
						upd02.Project.Building = up02.Building.Clone();

						// add the revision as a new project
						pd.AddProject(upd02);

						// delete the old project
						pd.DeleteProject(up01);

						break;

					case "331": // copy test
						up01 = up_02_5_G;
						upd02 = pd.Copy(up01);

						sb.Append(Util.FormatItemN("original project", FormatProject(up01).ToString()));
						sb.Append(Util.FormatItemN("copy project", FormatProject(upd02).ToString()));

						break;


					// create a change list
					case "351":
						upListExist01 = new List<uProject> {up_00_mt_A, up_00_mt_B};
						upListNew01 = new List<uProject> {up_00_2_C, up_00_2_D};

						sb.Append(FormatChangeList(upListExist01, upListNew01, pd));

						break;

					// create a change list
					case "352":
						upListExist01 = new List<uProject> { up_00_mt_A, up_00_mt_B };
						upListNew01 = new List<uProject> { up_00_mt_A_desc, up_00_mt_B_desc };

						sb.Append(FormatChangeList(upListExist01, upListNew01, pd));

						break;

					// change projects based on the change lists
					case "361":
						int[] Failed = new int[changeList.Count];
						
						sb.Append(Util.FormatItemN("change worked?", pd.ChangeProject(changeList, out Failed).ToString()));

						sb.Append(Util.FormatItemN("Success / fails",""));

						foreach (int fail in Failed)
						{
							sb.Append(fail.ToString()).Append("  ");
						}


						break;

					// change AutoCAD location Information
					case "371":
						up01 = up_00_6_F;

						upd01 = MakeProjectEx(up01);

						sb.Append(nl).Append("project data input");
						sb.Append(FormatProject(upd01));

						sb.Append(Util.FormatItemDividerN());

						sb.Append(Util.FormatItemN("change worked?",
							pd.ChangeAcadLocation(upd01).ToString()));

						sb.Append(nl).Append("project data changed");
						sb.Append(ListProjectData(up01, pd));
						break;

					// change Revit location Information
					case "372":
						up01 = up_00_6_F;

						upd01 = MakeProjectEx(up01);

						sb.Append(nl).Append("project data input");
						sb.Append(FormatProject(upd01));

						sb.Append(Util.FormatItemDividerN());

						sb.Append(Util.FormatItemN("change worked?",
							pd.ChangeRevitLocation(upd01).ToString()));

						sb.Append(nl).Append("project data changed");
						sb.Append(ListProjectData(up01, pd));
						break;

					case "380":
						sb.Append(nl).Append("sorting the data");
						pd.Sort();
						break;

					// add autocad location via change autocad location information
					case "390":
						up01 = up_00_mt_A;

						sb.Append(ListProjectData(up01, pd));

						sb.Append(Util.FormatItemDividerN());

						upd01 = MakeProjectEx(up01);

						sb.Append(nl).Append("project data input");
						sb.Append(FormatProject(upd01));

						sb.Append(Util.FormatItemDividerN());

						sb.Append(Util.FormatItemN("change worked?",
							pd.ChangeAcadLocation(upd01).ToString()));

						sb.Append(nl).Append("project data changed");
						sb.Append(ListProjectData(up01, pd));

						break;

					// general change project data
					case "400":
						up01 = up_00_mt_A;
						up02 = new uProject();
						up02.Task = new ProjNumInfo("99");

						updxNew = MakeProjectEx(up02);
						
						sb.Append(Util.FormatItemN("changing project data", ""));

						sb.Append(ListProjectData(up01, pd));

						sb.Append(Util.FormatItemN("changing project data result",
							pd.ChangeProjectInfo(up01, updxNew).ToString()));

						break;
					
					default:
						sb.Append("no tests selected");
						break;
				}

				sb.Append(Util.FormatItemDividerN());
				sbTemp.Clear();
			}

			return sb;
		}

		internal StringBuilder FormatChangeList(List<ChangeItem> changeList)
		{
			StringBuilder sb = new StringBuilder();

			foreach (ChangeItem item in changeList)
			{
				sb.Append(Util.FormatItemN("tskphbld", item.TskPhBldg));
				sb.Append(Util.FormatItemN(4, "existing", FormatProject(item.upxExisting).ToString()));
				sb.Append(Util.FormatItemN(4, "new", FormatProject(item.upxNew).ToString()));
			}
			return sb;
		}

		internal StringBuilder FormatProjectList(List<uProject> upList)
		{
			StringBuilder sb = new StringBuilder();

			int i = 1;

			foreach (uProject upx in upList)
			{
				sb.Append(Util.FormatItemN("project number", (i++.ToString("00: ") + FormatProjectEx(upx).ToString())));
			}
			return sb;
		}


		internal StringBuilder FormatProject(uProjectData updx)
		{
			StringBuilder sb = new StringBuilder();

			sb.Append(Util.FormatItemN("Project number", FormatProjectExx(updx.Project).ToString()));

			sb.Append(Util.FormatItemN(2, "roof folder", updx.RootFolder));
			sb.Append(Util.FormatItemN(2, "cd folder", updx.CDFolder));
			sb.Append(Util.FormatItemN(2, "sht number format", updx.SheetNumberFormat.Name +
				" : " + updx.SheetNumberFormat.Format));

			sb.Append(Util.FormatItemN(2, "AutoCAD info"));

			sb.Append(Util.FormatItemN(4, "sht folder", updx.AutoCAD.SheetFileFolder));
			sb.Append(Util.FormatItemN(4, "xref folder", updx.AutoCAD.XrefFolder));
			sb.Append(Util.FormatItemN(4, "detail folder", updx.AutoCAD.DetailFolder));
			sb.Append(Util.FormatItemN(4, "border file", updx.AutoCAD.BorderFile));

			sb.Append(Util.FormatItemN(2, "Revit info"));
			sb.Append(Util.FormatItemN(4, "model file", updx.Revit.CDModelFile));
			sb.Append(Util.FormatItemN(4, "library file", updx.Revit.LibraryModelFile));
			sb.Append(Util.FormatItemN(4, "keynote file", updx.Revit.KeynoteFile));
			sb.Append(Util.FormatItemN(4, "linked folder", updx.Revit.LinkedFolder));
			sb.Append(Util.FormatItemN(4, "xref folder", updx.Revit.XrefFolder));

			return sb;
		}


		internal StringBuilder FormatProject(uProject upx)
		{
			StringBuilder projNumber = new StringBuilder();

			string tsk = (upx.Task.Number ?? "null").Equals("") ? "<mt>" : upx.Task.Number;
			string ph  = (upx.Phase.Number ?? "null").Equals("") ? "<mt>" : upx.Phase.Number;
			string bld = (upx.Building.Number ?? "null").Equals("") ? "<mt>" : upx.Building.Number;

			projNumber.Append("tsk: ").Append(tsk);
			projNumber.Append(" ph: ").Append(ph);
			projNumber.Append(" bld: ").Append(bld);

			return projNumber;
		}

		internal StringBuilder FormatProjectEx(uProject upx)
		{
			StringBuilder projNumber = new StringBuilder();

			string tsk = (upx.Task.Number ?? "null").Equals("") ? "<mt>" : upx.Task.Number;
			string ph  = (upx.Phase.Number ?? "null").Equals("") ? "<mt>" : upx.Phase.Number;
			string bld = (upx.Building.Number ?? "null").Equals("") ? "<mt>" : upx.Building.Number;

			string tsk_desc = (upx.Task.Description ?? "null").Equals("") ? "<mt>" : upx.Task.Description;
			string ph_desc  = (upx.Phase.Description ?? "null").Equals("") ? "<mt>" : upx.Phase.Description;
			string bld_desc = (upx.Building.Description ?? "null").Equals("") ? "<mt>" : upx.Building.Description;

			projNumber.Append(tsk).Append(" - ").Append(tsk_desc).Append(" | ");
			projNumber.Append(ph).Append(" - ").Append(ph_desc).Append(" | ");
			projNumber.Append(bld).Append(" - ").Append(bld_desc);

			return projNumber;
		}

		internal StringBuilder FormatProjectExx(uProject upx)
		{
			StringBuilder projNumber = new StringBuilder();

			string proj = (upx.ProjNumber.Number ?? "null").Equals("") ? 
				"<mt>" : upx.ProjNumber.Number;
			string proj_desc = (upx.ProjNumber.Description ?? "null").Equals("") ? 
				"<mt>" : upx.ProjNumber.Description;

			projNumber.Append(proj).Append(" - ").Append(proj_desc).Append(" | ");

			projNumber.Append(FormatProjectEx(upx));

			return projNumber;
		}
		
		internal StringBuilder 
			FormatProjectInfo(ProjectDataProjectInformation pdInfo)
		{
			StringBuilder sb = new StringBuilder();

			if (pdInfo == null)
				return sb.Append(Util.FormatItemN("pdInfo", "is null"));

			sb.Append(Util.FormatItemN("project name", pdInfo.Project.Description));
			sb.Append(Util.FormatItemN("project number", pdInfo.Project.Number));
			sb.Append(Util.FormatItemN("root folder", pdInfo.RootFolder));

			return sb;
		}

		internal StringBuilder
			FormatACADLocationInfo(ProjectDataCDPackagesTaskPhaseBuildingLocationAutoCAD acadLoc)
		{
			StringBuilder sb = new StringBuilder();

			if (acadLoc == null)
				return sb.Append(Util.FormatItemN("revitLoc", "is null"));

			sb.Append(Util.FormatItemN("SheetFileFolder", acadLoc.SheetFileFolder.Path));
			sb.Append(Util.FormatItemN("XrefFolder", acadLoc.XrefFolder.Path));
			sb.Append(Util.FormatItemN("DetailFolder", acadLoc.DetailFolder.Path));
			sb.Append(Util.FormatItemN("BorderFile", acadLoc.BorderFile.Path));

			return sb;
		}

		internal StringBuilder
			FormatRevitLocationInfo(ProjectDataCDPackagesTaskPhaseBuildingLocationRevit revitLoc)
		{
			StringBuilder sb = new StringBuilder();

			if (revitLoc == null)
				return sb.Append(Util.FormatItemN("revitLoc", "is null"));

			sb.Append(Util.FormatItemN("CDModelFile", revitLoc.CDModelFile.Path));
			sb.Append(Util.FormatItemN("LibraryModelFile", revitLoc.LibraryModelFile.Path));
			sb.Append(Util.FormatItemN("LinkedFolder", revitLoc.LinkedFolder.Path));
			sb.Append(Util.FormatItemN("XrefFolder", revitLoc.XrefFolder.Path));
			sb.Append(Util.FormatItemN("KeynoteFile", revitLoc.KeynoteFile.Path));

			return sb;
		}

		internal StringBuilder FormatChangeList(List<uProject> upListExist, List<uProject> upListNew, ProjectData pd)
		{
			StringBuilder sb = new StringBuilder();

			sb.Append(nl).Append("renaming projects");
			sb.Append(nl).Append(nl).Append("existing project info");

			sb.Append(FormatProjectList(upListExist));

			sb.Append(nl).Append(nl).Append("new project info");
			sb.Append(FormatProjectList(upListNew));

			changeList = pd.MakeChangeList(upListExist, upListNew);
			sb.Append(nl).Append(nl).Append("change list");
			sb.Append(FormatChangeList(changeList));
			return sb;
			
		}

		private uProjectData AssignProjDataNew()
		{
			uProject upx = new uProject();

			upx.ProjNumber = new ProjNumInfo("2099-999", "TextX");

			return new uProjectData(upx, @"p:\Root Folder");
		}


		private StringBuilder CreateNewProject(ProjectData pd, uProject upx)
		{
			StringBuilder sb = new StringBuilder();

			uProjectData newProject = MakeProject(upx);
			sb.Append(Util.FormatItemN("Adding project", FormatProject(upx).ToString()));
			sb.Append(FormatProject(newProject));
			sb.Append(Util.FormatItemN("added?", pd.AddProject(newProject).ToString()));

			return sb;
		}

		// create a revised project information data struct
		private uProjectData MakeProjectEx(uProject upx)
		{
			uProjectData updx = new uProjectData();

			updx.Project = upx.Clone();

			string rootfolder = @"this is the root folder!";
			string cdfolder = rootfolder + @"\some sub-folder";

			updx.Valid = true;

			updx.Project = upx.Clone();
			updx.SheetNumberFormat = ShtNumFmt.VOIDsnf;
			updx.RootFolder = rootfolder;
			updx.CDFolder = cdfolder;

			updx.AutoCAD = MakeAutoCADEx();
			updx.Revit = MakeRevitEx();

			return updx;
		}


		// create sample project information
		private uProjectData MakeProject(uProject upx)
		{
			uProjectData updx = new uProjectData();

			string rootfolder = @"p:\" + upx.ProjNumber.Number + "-root";
			string cdfolder = rootfolder + @"\cd";

			updx.Valid = true;

			updx.Project = upx.Clone();
			updx.SheetNumberFormat = ShtNumFmt.LEVEL3;
			updx.RootFolder = rootfolder;
			updx.CDFolder = cdfolder;

			updx.AutoCAD = MakeAutoCAD(cdfolder, upx);
			updx.Revit = MakeRevit(cdfolder, upx);

			return updx;
		}

		private uProjectDataAutoCAD MakeAutoCAD(string cdfolder, uProject upx)
		{
			uProjectDataAutoCAD acad = new uProjectDataAutoCAD();

			string tpb = upx.Task.Number + "-" + upx.Phase.Number + "-" + upx.Building.Number;

			string shtfolder = cdfolder + @"\" + tpb + "-acad";
			string detail = shtfolder + @"\detail";

			acad.SheetFileFolder = shtfolder;
			acad.XrefFolder = shtfolder + @"\x-ref";
			acad.DetailFolder = detail;
			acad.BorderFile = detail + @"\border.dwg";

			return acad;
		}

		private uProjectDataAutoCAD MakeAutoCADEx()
		{
			uProjectDataAutoCAD acad = new uProjectDataAutoCAD();

			acad.SheetFileFolder = "this is the sheet folder";
			acad.XrefFolder = "this is the xref folder";
			acad.DetailFolder = "this is the detail folder";
			acad.BorderFile = "this is the border file";

			return acad;
		}

		private uProjectDataRevit MakeRevit(string cdfolder, uProject upx)
		{
			uProjectDataRevit revit = new uProjectDataRevit();

			string tpb = upx.Task.Number + "-" + upx.Phase.Number + "-" + upx.Building.Number;

			string modelfolder = cdfolder + @"\" + tpb + "-revit";

			revit.CDModelFile = modelfolder + @"\model.rvt";
			revit.LibraryModelFile = modelfolder + @"\library.rvt";
			revit.KeynoteFile = modelfolder + @"\keynote.txt";
			revit.LinkedFolder = modelfolder + @"\Linked";
			revit.XrefFolder = modelfolder + @"\x-ref";

			return revit;
		}
		
		private uProjectDataRevit MakeRevitEx()
		{
			uProjectDataRevit revit = new uProjectDataRevit();

			revit.CDModelFile = "this is the CD Model file";
			revit.LibraryModelFile = "this is the library file";
			revit.KeynoteFile = "this is the keynote file";
			revit.LinkedFolder = "this is the Linked folder";
			revit.XrefFolder = "this is the xref folder";

			return revit;
		}

		private uProjectDataRevit MakeRevitExx()
		{
			uProjectDataRevit revit = new uProjectDataRevit();

			revit.CDModelFile = "this is the CD Model file";
			revit.LibraryModelFile = "";
			revit.KeynoteFile = "";
			revit.LinkedFolder = "";
			revit.XrefFolder = "";

			return revit;
		}
		private StringBuilder ListProjectData(uProject upx, ProjectData pd)
		{
			StringBuilder sb = new StringBuilder();

			int column = Util.column = 0;

			sb.Append(Util.FormatItemDividerN());
			sb.Append(Util.FormatItemN(column, "Project Data"));
			sb.Append(Util.FormatItemDividerN());

			sb.Append(pd.ToString());

			Util.column += Util.columnAdjust;

			if (ProjNumInfo.NumberIsAll(upx.Task))
			{
				// listing all tasks
				sb.Append(Util.FormatItemDividerN());
				sb.Append(Util.FormatItemN(column, "CD Packages"));

				foreach (ProjectDataCDPackagesTask oneTask in pd.CDPackages)
				{
					sb.Append(ListTask(upx, oneTask));
					Util.column = column + Util.columnAdjust;
				}
			}
			else
			{
				// listing one task
				ProjectDataCDPackagesTask oneTask = pd.FindTask(upx);

				sb.Append(Util.FormatItemDividerN());
				sb.Append(Util.FormatItemN(column, "CD Package"));

				if (oneTask != null)
				{
					sb.Append(ListTask(upx, oneTask));
				}
				else
				{
					sb.Append(Util.FormatItemN(column, "Task: " + upx.Task.Number, "** not found **"));
				}
			}
			return sb;
		}

		private StringBuilder ListTask(uProject upx, ProjectDataCDPackagesTask oneTask)
		{
			StringBuilder sb = new StringBuilder();

			int column = Util.column;

			sb.Append(Util.FormatItemDividerN());
			sb.Append(oneTask.ToString());

			Util.column += Util.columnAdjust;

			if (ProjNumInfo.NumberIsAll(upx.Phase))
			{
				foreach (ProjectDataCDPackagesTaskPhase onePhase in oneTask.Phase)
				{
					sb.Append(ListPhase(upx, onePhase));

					Util.column = column + Util.columnAdjust;
				}
			}
			else
			{
				ProjectDataCDPackagesTaskPhase onePhase = oneTask.FindPhase(upx);

				if (onePhase != null)
				{
					sb.Append(ListPhase(upx, onePhase));
				}
				else
				{
					sb.Append(Util.FormatItemN(column, "Phase: " + upx.Phase.Number, "** not found **"));
				}
			}
			return sb;
		}

		private StringBuilder ListPhase(uProject upx, ProjectDataCDPackagesTaskPhase onePhase)
		{
			StringBuilder sb = new StringBuilder();

			int column = Util.column;

			sb.Append(Util.FormatItemDividerN());
			sb.Append(onePhase.ToString());

			Util.column += Util.columnAdjust;

			if (ProjNumInfo.NumberIsAll(upx.Building))
			{
				//				Debug.Print("listing all buildings");

				foreach (ProjectDataCDPackagesTaskPhaseBuilding oneBldg in onePhase.Building)
				{
					sb.Append(ListBuilding(oneBldg));

					Util.column = column + Util.columnAdjust;
				}
			}
			else
			{
				ProjectDataCDPackagesTaskPhaseBuilding oneBldg = onePhase.FindBuilding(upx);

				if (oneBldg != null)
				{
					sb.Append(ListBuilding(oneBldg));
				}
				else
				{
					sb.Append(Util.FormatItemN(column, "Building: " + upx.Building.Number, "** not found **"));
				}
			}

			return sb;
		}

		private StringBuilder ListBuilding(ProjectDataCDPackagesTaskPhaseBuilding oneBuilding)
		{
			StringBuilder sb = new StringBuilder();
			int column = Util.column;


			sb.Append(Util.FormatItemDividerN());
			sb.Append(oneBuilding.ToString());

			Util.column += Util.columnAdjust;

			if (oneBuilding.LocationAutoCAD != null)
			{
				sb.Append(oneBuilding.LocationAutoCAD.ToString());
			}
			else
			{
				sb.Append(Util.FormatItemDividerN());
				sb.Append(Util.FormatItemN(column + Util.columnAdjust, "no autocad location information", ""));
			}

			if (oneBuilding.LocationRevit != null)
			{
				sb.Append(oneBuilding.LocationRevit.ToString());
			}
			else
			{
				sb.Append(Util.FormatItemDividerN());
				sb.Append(Util.FormatItemN(column + Util.columnAdjust, "no revit location information", ""));
			}

			return sb;
		}


		private StringBuilder ListShtNumFmt()
		{
			StringBuilder sb = new StringBuilder();

			ShtNumFmt snf;

			sb.Append("void: " + ShtNumFmt.VOIDsnf.Ordinal + " = " + ShtNumFmt.VOIDsnf);

			snf = ShtNumFmt.LEVEL1;
			sb.Append(nl).Append("Level1: ").Append(snf.Ordinal).Append(" = ").Append(snf).Append(" = ").Append(snf.Format);
			sb.Append(nl).Append("Formatted sheet number: ").Append(snf.FormatSheetFileName("9999", "T", "2", "0", "1", "name"));

			snf = ShtNumFmt.LEVEL2;
			sb.Append(nl).Append("Level2: ").Append(snf.Ordinal).Append(" = ").Append(snf).Append(" = ").Append(snf.Format);
			sb.Append(nl).Append("Formatted sheet number: ").Append(snf.FormatSheetFileName("9999", "T", "2", "0", "1", "name"));

			snf = ShtNumFmt.LEVEL3;
			sb.Append(nl).Append("Level3: ").Append(snf.Ordinal).Append(" = ").Append(snf).Append(" = ").Append(snf.Format);

			sb.Append(nl).Append("Formatted sheet number: ").Append(snf.FormatSheetFileName("9999", "T", "2", "0", "1", "name"));

			return sb;
		}


		private StringBuilder ListProgm()
		{
			StringBuilder sb = new StringBuilder();

			sb.Append("void: " + Software.VOIDsw.Ordinal + " = " + Software.VOIDsw);
			sb.Append(Util.nl + "acad: " + Software.AutoCAD.Ordinal + " = " + Software.AutoCAD);
			sb.Append(Util.nl + "revt: " + Software.Revit.Ordinal + " = " + Software.Revit);

			sb.Append(Util.nl);

			sb.Append(Util.nl + "access using index:");

			Software p = new Software();

			Software z = Software.AutoCAD;
			Software y = Software.AutoCAD;

			for(int i = 0;i < 3; i++)
			{
				sb.Append(Util.nl + "index " + i + ": " + p[i]);
			}

			sb.Append(Util.nl);
			sb.Append(Util.nl + "access using foreach & name:");

			foreach (Software x in p)
			{
				sb.Append(Util.nl + "index " + x.Ordinal + ": " + x.Name);
			}

			sb.Append(Util.nl + Util.nl + "finding Revit (2): " + Software.Find("REVIT").Ordinal);

			sb.Append(Util.nl + Util.nl + "finding test (not found): " + Software.Find("TEST").Ordinal);

			sb.Append(Util.nl + Util.nl + "to string: " + Software.AutoCAD);

			sb.Append(Util.nl + Util.nl + "is member (false): " + Software.IsMember("test"));

			sb.Append(Util.nl + Util.nl + "is member (true): " + Software.IsMember("VOIDsnf"));

			sb.Append(Util.nl + Util.nl + "count: " + p.Count);

			sb.Append(Util.nl + Util.nl + "size: " + Software.Size);

			sb.Append(Util.nl + Util.nl + "equal 1: " + z.Equals(y));
			sb.Append(Util.nl + "equal 2: " + (z == y));


			sb.Append(Util.nl + Util.nl + "hash code: " + z.GetHashCode() + "  " + y.GetHashCode());

			sb.Append(Util.nl + Util.nl + "switch?");

			

			sb.Append(Util.nl + Util.nl + "array:");

			Software[] xp = p.ToArray();

			

			for (int i = 0; i < xp.Count(); i++)
			{
				sb.Append(Util.nl + "index " + i + " ordinal: " + xp[i].Ordinal + ": " + xp[i].Name);
			}

			return sb;
		}

	}
}
