using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Models {
	public interface SongComponent {

		// A ModelNote should return a list containing only itself.
		// A RepeatComposite should return a list of all its getAll() outputs, 
		// repeated as often as its 'repeat' value indicates, factoring in its alternative endings.
		// BarComposite and TempoComposite should simply return a list of all its getAll() outputs.

		// A BarComposite simply contains its inner SongComponents.
		// A TempoComposite contains its inner SongComponents, plus a Tempo.
		// A RepeatComposite contains the SongComponents up to the first alternative,
		// the SongComponents in the first alternative and those in the second alternative
		List<NoteLeaf> getAll();
	}
}
