using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Managers
{
	// When thinking about how to seperate concerns in this program, the realization that I could use a Factory was a real IOpener! ;)
	public interface ILoader {
		string load(string fileName);
	}
}
