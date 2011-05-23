using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
namespace CodeBoxControl.Decorations
{
    /// <summary>
    /// Decoration based on a single regular expression string
    /// </summary>
 public   class RegexDecoration:Decoration 
    {
        #region RegexString

     /// <summary>
     /// The Regular expression used to evaluate the regex expressed as a string
     /// </summary>
        public static DependencyProperty RegexStringProperty = DependencyProperty.Register("RegexString", typeof(String), typeof(RegexDecoration),
        new PropertyMetadata(  "", new PropertyChangedCallback(RegexDecoration.OnRegexStringChanged)));

        public String RegexString
        {
            get { return (String)GetValue(RegexStringProperty); }
            set { SetValue(RegexStringProperty, value); }
        }

        private static void OnRegexStringChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue != e.OldValue)
            {
                RegexDecoration dObj = (RegexDecoration)d;
               dObj.IsDirty = true;
            }
        }
       
        #endregion

        public override List<Pair> Ranges(string Text)
        {
            List<Pair> pairs = new List<Pair>();
            if (RegexString != "")
            {
            try
            {
                Regex rx = new Regex(RegexString);
                MatchCollection mc = rx.Matches(Text);
                foreach (Match m in mc)
                {
                    if (m.Length > 0)
                    {
                        pairs.Add(new Pair(m.Index, m.Length));
                    }
                } 
            }catch {}
        }
            IsDirty = false;
            return pairs;
        }

        public override bool AreRangesSorted
        {
            get { return true; }
        }
    }
}
