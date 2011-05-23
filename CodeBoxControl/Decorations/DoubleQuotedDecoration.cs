using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace CodeBoxControl.Decorations
{
    class DoubleQuotedDecoration : Decoration 
    {
        static Regex rx = new Regex ("\".*?\"");
        public override List<Pair> Ranges(string Text)
        {

            List<Pair> pairs = new List<Pair>();
          
                try
                {
                    MatchCollection mc = rx.Matches(Text);
                    foreach (Match m in mc)
                    {
                        if (m.Length > 0)
                        {
                            pairs.Add(new Pair(m.Index, m.Length));
                        }
                    }
                }
                catch { }
           
            IsDirty = false;
            return pairs;
        }


        public override bool AreRangesSorted
        {
            get { return true; }
        }
    }
}
