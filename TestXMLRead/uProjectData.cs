using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace TestXMLRead
{
	public struct uProjectData
	{
		public bool Valid;
		public uProject Project;
		public string RootFolder;
		public string CDFolder;
		public ShtNumFmt SheetNumberFormat;
		public uProjectDataAutoCAD AutoCAD;
		public uProjectDataRevit Revit;

		public uProjectData(uProject upx, string rootFolder) :
			this(upx, rootFolder, null, null, new uProjectDataAutoCAD(), new uProjectDataRevit()) { }

		public uProjectData(uProject upx, string rootFolder, string cdFolder, 
			ShtNumFmt shtnumfmt, uProjectDataAutoCAD autoCad, uProjectDataRevit revit)
		{
			Valid = true;
			Project = upx;
			SheetNumberFormat = shtnumfmt;
			RootFolder = rootFolder;
			CDFolder = cdFolder;
			AutoCAD = autoCad;
			Revit = revit;

		}

		public uProjectData Clone()
		{
			return new uProjectData(Project.Clone(), RootFolder,
				CDFolder, SheetNumberFormat, AutoCAD.Clone(), Revit.Clone());
		}

		// update project number information
		public void Update(uProject upx)
		{
			if (!ProjNumInfo.NumberIsNullOrEmpty(upx.Task)) Project.Task.Number = upx.Task.Number;
			if (!ProjNumInfo.DescriptionIsNullOrEmpty(upx.Task)) Project.Task.Description = upx.Task.Description;

			if (!ProjNumInfo.NumberIsNullOrEmpty(upx.Phase)) Project.Phase.Number = upx.Phase.Number;
			if (!ProjNumInfo.DescriptionIsNullOrEmpty(upx.Phase)) Project.Phase.Description = upx.Phase.Description;

			if (!ProjNumInfo.NumberIsNullOrEmpty(upx.Building)) Project.Building.Number = upx.Building.Number;
			if (!ProjNumInfo.DescriptionIsNullOrEmpty(upx.Building)) Project.Building.Description = upx.Building.Description;
		}

		public void UpdateDescriptions(uProject upx)
		{
			if (!ProjNumInfo.DescriptionIsNullOrEmpty(upx.Task)) Project.Task.Description = upx.Task.Description;

			if (!ProjNumInfo.DescriptionIsNullOrEmpty(upx.Phase)) Project.Phase.Description = upx.Phase.Description;

			if (!ProjNumInfo.DescriptionIsNullOrEmpty(upx.Building)) Project.Building.Description = upx.Building.Description;
		}
	}

	public struct uProjectDataAutoCAD
	{
		public string SheetFileFolder;
		public string XrefFolder;
		public string DetailFolder;
		public string BorderFile;

//		public uProjectDataAutoCAD() { }

		public uProjectDataAutoCAD(string sFolder,
			string xFolder, string dFolder, string bFolder)
		{
			SheetFileFolder = sFolder;
			XrefFolder      = xFolder;
			DetailFolder    = dFolder;
			BorderFile      = bFolder;
		}

		public uProjectDataAutoCAD Clone()
		{
			return new uProjectDataAutoCAD(SheetFileFolder, 
				XrefFolder, DetailFolder, BorderFile);
		}

	}

	public struct uProjectDataRevit
	{
		public string CDModelFile;
		public string LibraryModelFile;
		public string KeynoteFile;
		public string LinkedFolder;
		public string XrefFolder;

//		public uProjectDataRevit() { }

		public uProjectDataRevit(string cFile,
			string lFile, string kFile, string lFolder, string xFolder)
		{
			CDModelFile      = cFile;
			LibraryModelFile = lFile;
			KeynoteFile      = kFile;
			LinkedFolder     = lFolder;
			XrefFolder       = xFolder;
		}

		public uProjectDataRevit Clone()
		{
			return new uProjectDataRevit(CDModelFile, 
					LibraryModelFile, KeynoteFile, 
					LinkedFolder, XrefFolder);
		}
	}

}
