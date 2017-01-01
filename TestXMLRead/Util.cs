using System;
	using System.Diagnostics;
	using System.Xml.Serialization;
	using System.Collections;
	using System.Xml.Schema;
	using System.ComponentModel;
	using System.IO;
	using System.Text;
	using System.Collections.Generic;
	using System.Runtime.Serialization;

namespace TestXMLRead
{
	static class Util
	{
		public static string nl = System.Environment.NewLine;
		public static string diamonds = "♦♦♦♦♦♦";
		public static int columnAdjust = 2;
		public static string AODWG = ".dwg";
		public static int column = 0;
		public static int dashwidth = 120;
		public static int colonColumn = 48;
		public static string all = "*";


		public static StringBuilder FormatItemDivider()
		{
			return new StringBuilder(new String('-', dashwidth));
		}

		public static StringBuilder FormatItemDividerN()
		{
			return new StringBuilder(nl).Append(FormatItemDivider());
		
		}

		public static StringBuilder FormatItem(string description, string value)
		{
			return FormatItem(0, description, value);
		}

		public static StringBuilder FormatItem(int column, string description, string value)
		{
			string width = (column - colonColumn).ToString();

			if (String.IsNullOrWhiteSpace(value))
			{
				return new StringBuilder().AppendFormat("{0," + width + "}", description);
			}

			return new StringBuilder().AppendFormat("{0," + width + "}: {1}", description, value);
		}

		public static StringBuilder FormatItemN(string description, string value)
		{
			return new StringBuilder(nl).Append(FormatItem(0, description, value));
		}


		public static StringBuilder FormatItemN(int column, string description, string value)
		{
			StringBuilder sb = new StringBuilder(nl);

			sb.Append(ColumnOffset(column));

			return sb.Append(FormatItem(column, description, value));
		}

		public static StringBuilder FormatItemN(int column, string description)
		{
			StringBuilder sb = new StringBuilder(nl);

			sb.Append(ColumnOffset(column));

			return sb.Append(description);
		}

		public static StringBuilder ColumnOffset(int column)
		{
			StringBuilder sb = new StringBuilder();

			if (column > 0 && column < 24)
			{
				for (int i = 0; i < column; i++)
				{
					sb.Append(" ");
				}
			}
			return sb;
		}

		public static string FormatTaskPhaseBuilding(uProject upx)
		{
			StringBuilder sb = new StringBuilder();

			sb.Append(FormatName(upx.Task.Number));

			sb.Append(FormatName(upx.Phase.Number));

			sb.Append(FormatName(upx.Building.Number));

			return sb.ToString();
		}

		public static string FormatName(string name)
		{
			return diamonds.Substring(0, 6 - name.Length) + name;
		}
	}
}
