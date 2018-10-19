using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Models
{
	public abstract class SongLeaf : SongComponent
	{
		public List<SongLeaf> getAll()
		{
			return new List<SongLeaf>() { this };
		}
	}
}
