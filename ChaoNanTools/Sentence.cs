using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChaoNanTools
{
    class SentenceInfomation
    {
        public bool HasN { get; set; }
        public bool HasV { get; set; }
        public bool HasADJ { get; set; }
        public string Sentence { get; set; }

        public SentenceInfomation(string sentence)
        {
            Sentence = sentence;
            HasN = true;
            HasV = true;
            HasADJ = true;
        }

        public SentenceInfomation()
        {
            Sentence = null;
            HasN = true;
            HasV = true;
            HasADJ = true;         
        }
    }
}
