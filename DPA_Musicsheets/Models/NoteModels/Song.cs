using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Models {
    public class Song {
        List<SongComponent> components;

		public List<SongLeaf> getAll() {
			List<SongLeaf> notes = new List<SongLeaf>();
			foreach (SongComponent component in components)
			{
				notes.AddRange(component.getAll());
			}
			return notes;
		}
    }
}
