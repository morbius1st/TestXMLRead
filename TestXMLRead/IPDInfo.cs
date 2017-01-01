using System.Collections.Generic;

namespace TestXMLRead
{
	public interface IPDInfo
	{
		string itemNumber { get; }
		string itemDescription { get; }
		List<FindItem> FindItems(uProject upx, int level);
		void Sort();
	}
}