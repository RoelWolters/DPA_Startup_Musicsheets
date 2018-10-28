using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Managers
{
	public interface ISaver
	{
		void save(string fileName, string lilypondText);
	}
}
