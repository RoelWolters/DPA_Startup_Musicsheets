using DPA_Musicsheets.Parsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Factories
{
    public class ParserFactory
    {
        public IParser CreateParser(string type)
        {
            IParser parser;

            switch (type)
            {
                case ".mid":
                    parser = new MidiParser();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }

            return parser;
        }
    }
}
