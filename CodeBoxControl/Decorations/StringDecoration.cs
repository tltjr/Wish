using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
namespace CodeBoxControl.Decorations
{
    /// <summary>
    /// Decoration based on index positions of a single string
    /// </summary>
 public   class StringDecoration:Decoration 
    {
     /// <summary>
     /// The string to be searched for 
     /// </summary>
        #region String
        public static DependencyProperty StringProperty = DependencyProperty.Register("String", typeof(String), typeof(RegexDecoration),
        new PropertyMetadata("", new PropertyChangedCallback(StringDecoration.OnStringChanged)));

        public String String
        {
            get { return (String)GetValue(StringProperty); }
            set { SetValue(StringProperty, value); }
        }

        private static void OnStringChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue != e.OldValue)
            {
                StringDecoration dObj = (StringDecoration)d;
                //dObj.IsDirty = true;
            }
        }

        #endregion
     /// <summary>
     /// The System.StringComparison value to be used in searching 
     /// </summary>
        public StringComparison StringComparison { get; set; }


        public override List<Pair> Ranges(string Text)
        {
            List<Pair> pairs = new List<Pair>();
            if (Text != null && Text != "")
            {
                int index = Text.IndexOf(String, 0, StringComparison);
                while (index != -1)
                {
                    pairs.Add(new Pair(index, String.Length));
                    index = Text.IndexOf(String, index + String.Length, StringComparison);
                }
            }
            return pairs;
        }

        public override bool AreRangesSorted
        {
            get {return true; }
        }
    }
}
