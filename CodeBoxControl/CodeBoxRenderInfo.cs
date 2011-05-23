using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Windows;
using CodeBoxControl.Decorations;
namespace CodeBoxControl
{
    class CodeBoxRenderInfo
    {
       public FormattedText BoxText { get; set; }
       public FormattedText LineNumbers{get;set;}
       public Point RenderPoint { get; set; }

      public Dictionary<EDecorationType, Dictionary<Decoration, List<Geometry>>> PreparedDecorations { get; set; }
      public Dictionary<EDecorationType, Dictionary<Decoration, List<Geometry>>> BasePreparedDecorations { get; set; }

    }
}
