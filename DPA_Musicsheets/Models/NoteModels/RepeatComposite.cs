using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Models.NoteModels {
	class RepeatComposite : SongComposite {

		int repeats;

		public RepeatComposite(int repeats) {
			if (repeats > 0) {
				this.repeats = repeats;
			} else {
				throw new ArgumentOutOfRangeException("repeats", "The amount of repeats must be greater than 0.");
			}
			components = new List<SongComponent>();
			firstAlternative = new List<SongComponent>();
			lastAlternative = new List<SongComponent>();
		}

		public List<SongComponent> firstAlternative;
		public List<SongComponent> lastAlternative;

		public override List<SongLeaf> getAll() {
			List<SongLeaf> notes = new List<SongLeaf>();
			foreach (SongComponent component in components) {
				for (int i=0;i<repeats;i++) {
					notes.AddRange(component.getAll());
					if (firstAlternative.Count > 0 && lastAlternative.Count > 0) {
						if (component.Equals(components.Last())) {
							foreach (SongComponent lAltComponent in lastAlternative) {
								notes.AddRange(lAltComponent.getAll());
							}
						} else {
							foreach (SongComponent fAltComponent in firstAlternative) {
								notes.AddRange(fAltComponent.getAll());
							}
						}
					}
				}
			}
			return notes;
		}

	}
}
