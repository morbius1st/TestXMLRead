using System;

namespace TestXMLRead
{

	public struct ListingTypeStruct : IEquatable<ListingTypeStruct>
	{
		private static int count;

		private int ordinal;
		private string name;

		public ListingTypeStruct(string name)
		{
			this.ordinal = count++;
			this.name = name;
		}

		public int Ordinal
		{
			get { return ordinal; }
		}

		public string Name
		{
			get { return name; }
		}

		public bool Equals(ListingTypeStruct lts)
		{
			return lts.ordinal == ordinal;
		}

		public override bool Equals(object obj)
		{
			return false;
		}

		public static bool operator ==(ListingTypeStruct lts1,
			ListingTypeStruct lts2)
		{
			return lts1.Ordinal == lts2.Ordinal;
		}

		public static bool operator !=(ListingTypeStruct lts1,
			ListingTypeStruct lts2)
		{
			return lts1.Ordinal != lts2.Ordinal;
		}
	}



	public class ListingType
	{
		public static ListingTypeStruct All = new ListingTypeStruct("All");
		public static ListingTypeStruct Active = new ListingTypeStruct("Active");
		public static ListingTypeStruct Inactive = new ListingTypeStruct("InActive");

	}
}