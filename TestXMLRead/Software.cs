using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestXMLRead
{
	public class Software : IEnumerable<Software>
	{

		private enum Type
		{
			VOID,
			AUTOCAD,
			REVIT
		}

		private static int count = 0;

		// define members
		public readonly int Ordinal;
		public readonly string Name;
		private readonly Type type;

		private Software(Type type)
		{
			Ordinal = count++;
			Name = type.ToString();
			this.type = type;

		}

		public Software()
		{
			Ordinal = -1;
		}


		public static readonly Software VOIDsw = new Software(Type.VOID);
		public static readonly Software AutoCAD = new Software(Type.AUTOCAD);
		public static readonly Software Revit = new Software(Type.REVIT);

		private static List<Software> list = new List<Software>() { VOIDsw, AutoCAD, Revit };

		public Software this[long index]
		{
			get
			{
				if (index < 0 || index > count - 1)
					return null;

				return list[(int)index];
			}
		}

		public int Count { get { return count; } }

		public static int Size { get { return count; } }

		public IEnumerator<Software> GetEnumerator()
		{
			return list.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		public static Software Find(string name)
		{
			return list.Find(s => s.type.ToString().Equals(name)) ?? new Software();
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
