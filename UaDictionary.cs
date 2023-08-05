using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Translation
{
    public class UaDictionary
    {
        public string Word { get; set; }

        public UaDictionary() { }

        public UaDictionary(string _word)
        {
            Word = _word;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is UaDictionary))
                return false;

            return Word == ((UaDictionary)obj).Word;
        }

        public override int GetHashCode()
        {
            return Word?.GetHashCode() ?? 0;
        }
    }
}