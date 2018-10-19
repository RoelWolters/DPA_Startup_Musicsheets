using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Models {
	public interface SongComponent {
		// ------ SongComponent:	All of the components (composite or individual) that make up a song.
		// --- SongLeaf:			Individual song elements and/or starts of non-nestable groups of notes ('changes').
		// NoteLeaf:				Notes and rests; song elements with a length and optionally a tone.
		// BarlineLeaf:				A barline. Barlines have no variable properties.
		// TempoLeaf:				A change in tempo or time. Basically, any song element that changes our speed.
		// --- SongComposite:		Groups of notes that are nestable and/or return their list of NoteLeaves in a special way.
		// RepeatComposite:			A repeat section. Repeats a certain number of times and may have an alternative ending.

		// TODO: Handle clefs.
		List<SongLeaf> getAll();
	}
}
