using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
namespace CodeBoxControl.Decorations
{
    [TypeConverter(typeof(DecorationSchemeTypeConverter))]
 public  class DecorationScheme
    {
     List<Decoration> mDecorations = new List<Decoration>();

         public List<Decoration> BaseDecorations 
         {   get { return mDecorations;}
             set { mDecorations = value; } 
         }

         public string Name { get; set; }

        
        
    }
}
