using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;

namespace CodeBoxControl.Decorations
{
   public  class DoubleRegexDecoration:Decoration
   {
       #region RegexString

       /// <summary>
       /// The Outer Regular expression used to evaluate the regex expressed as a string
       /// </summary>
       public static DependencyProperty OuterRegexStringProperty = DependencyProperty.Register("OuterRegexString", typeof(String), typeof(DoubleRegexDecoration),
       new PropertyMetadata("", new PropertyChangedCallback(DoubleRegexDecoration.OnRegexStringChanged)));

       public String OuterRegexString
       {
           get { return (String)GetValue(OuterRegexStringProperty); }
           set { SetValue(OuterRegexStringProperty, value); }
       }


       /// <summary>
       /// The Inner Regular expression used to evaluate the regex expressed as a string
       /// </summary>
       public static DependencyProperty InnerRegexStringProperty = DependencyProperty.Register("InnerRegexString", typeof(String), typeof(DoubleRegexDecoration),
       new PropertyMetadata("", new PropertyChangedCallback(DoubleRegexDecoration.OnRegexStringChanged)));

       public String InnerRegexString
       {
           get { return (String)GetValue(InnerRegexStringProperty); }
           set { SetValue(InnerRegexStringProperty, value); }
       }





       private static void OnRegexStringChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
       {
           if (e.NewValue != e.OldValue)
           {
               DoubleRegexDecoration dObj = (DoubleRegexDecoration)d;
               dObj.IsDirty = true;
           }
       }

       #endregion

       public override List<Pair> Ranges(string Text)
       {
           List<Pair> pairs = new List<Pair>();
           if (OuterRegexString != "" && InnerRegexString != "")
           {
               try
               {
                   Regex orx = new Regex(OuterRegexString);
                   Regex irx = new Regex(InnerRegexString);

                   MatchCollection omc = orx.Matches(Text);
                   foreach (Match om in omc)
                   {
                       if (om.Length > 0)
                       {
                           MatchCollection imc = irx.Matches(om.Value);
                           foreach (Match im in imc)
                           {
                               if (im.Length > 0)
                               {
                                pairs.Add(new Pair(om.Index + im.Index , im.Length));
                               }

                           }
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
