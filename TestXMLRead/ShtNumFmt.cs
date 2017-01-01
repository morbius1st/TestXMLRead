using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing.Text;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestXMLRead
{
	public class ShtNumFmt : IEnumerable<ShtNumFmt>
	{
		private enum Type
		{
			VOID,
			LEVEL1,
			LEVEL2,
			LEVEL3
		}

		private static int count = 0;

		// define members
		public readonly int Ordinal;
		public readonly string Name;
		public readonly string Format;
		private readonly Type type;

		private ShtNumFmt(Type type, string format)
		{
			Ordinal = count++;
			Name = type.ToString();
			Format = format;
			this.type = type;
		}

		public ShtNumFmt()
		{
			Ordinal = -1;
		}

		public static readonly ShtNumFmt VOIDsnf = new ShtNumFmt(Type.VOID, null);
		public static readonly ShtNumFmt LEVEL1 = new ShtNumFmt(Type.LEVEL1, "1:X#");
		public static readonly ShtNumFmt LEVEL2 = new ShtNumFmt(Type.LEVEL2, "2:X#.#");
		public static readonly ShtNumFmt LEVEL3 = new ShtNumFmt(Type.LEVEL3, "3:X#.#-#");

		private static List<ShtNumFmt> list = new List<ShtNumFmt>() {VOIDsnf, LEVEL1, LEVEL2, LEVEL3};

		public ShtNumFmt this[long index]
		{
			get
			{
				if (index < 0 || index > count - 1)
					return list[0];

				return list[(int)index];
			}
		}

		public int Count { get { return count; } }

		public static int Size { get { return count; } }

		public string FormatSheetFileName(string projAbbrevNumber,
			string dwgFNameDesignator, string dwgFNameCat, string dwgFNameSubCat, string dwgFNameSeq,
			string dwgName)
		{
			string result = null;

			switch (Ordinal)
			{
				case 1:
					result = string.Format("{0}{1}", dwgFNameDesignator, dwgFNameSeq);
					break;
				case 2:
					result = string.Format("{0}{1}-{2}", dwgFNameDesignator, dwgFNameCat, dwgFNameSeq);
					break;
				case 3:
					result = string.Format("{0}{1}-{2}-{3}", dwgFNameDesignator, dwgFNameCat, dwgFNameSubCat, dwgFNameSeq);
					break;
			}

			return string.Format("{0}-{1} {2}{3}", projAbbrevNumber, result, dwgName, Util.AODWG);
		}

		public IEnumerator<ShtNumFmt> GetEnumerator()
		{
			return list.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		public static ShtNumFmt Find(string name)
		{
			return list.Find(s => s.type.ToString().Equals(name)) ?? new ShtNumFmt();
		}

		public static bool IsMember(string name)
		{
			return Find(name) != null;
		}

		public override string ToString()
		{
			return type.ToString();
		}


	}
}
