using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;

namespace CodeBoxControl.Decorations
{
    /// <summary>
    /// Decoration Based on a regular expression and a match 
    /// </summary>
 public class RegexMatchDecoration:Decoration 
    {
        #region RegexString
     public static DependencyProperty RegexStringProperty = DependencyProperty.Register("RegexString", typeof(String), typeof(RegexMatchDecoration),
        new PropertyMetadata("", new PropertyChangedCallback(RegexMatchDecoration.OnRegexStringChanged)));

        public String RegexString
        {
            get { return (String)GetValue(RegexStringProperty); }
            set { SetValue(RegexStringProperty, value); }
        }

     

        private static void OnRegexStringChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue != e.OldValue)
            {
                RegexMatchDecoration dObj = (RegexMatchDecoration)d;
                dObj.IsDirty = true;
            }
        }

        #endregion


        public static DependencyProperty RegexMatchProperty = DependencyProperty.Register("RegexMatch", typeof(String), typeof(RegexMatchDecoration),
          new PropertyMetadata("selected", new PropertyChangedCallback(RegexMatchDecoration.OnRegexStringChanged)));

     /// <summary>
     /// The Name of the group that to be selected, the default group is "selected"
     /// </summary>
        public String RegexMatch
        {
            get { return (String)GetValue(RegexMatchProperty); }
            set { SetValue(RegexMatchProperty, value); }
        }


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
                        if (m.Length > 0 )
                        {
                            pairs.Add(new Pair(m.Groups[RegexMatch].Index, m.Groups[RegexMatch].Length));
                        }
                    }
                }
                catch { }
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
