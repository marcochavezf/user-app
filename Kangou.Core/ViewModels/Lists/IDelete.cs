using System;

namespace Kangou.Core
{
	public interface IDelete
	{
		void DeleteData(int id);
		void DeleteDataByIndex (int index);
	}
}

