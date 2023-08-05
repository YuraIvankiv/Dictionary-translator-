using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Translation
{
    public class UsaDictionary
    {
        public string Word { get; set; }

        public UsaDictionary() { }

        public UsaDictionary(string _word)
        {
            Word = _word;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is UsaDictionary))
                return false;

            return Word == ((UsaDictionary)obj).Word;
        }

        public override int GetHashCode()
        {
            return Word?.GetHashCode() ?? 0;
        }
    }
}

