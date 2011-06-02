// Type: System.Windows.Media.FormattedText
// Assembly: PresentationCore, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// Assembly location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\v3.0\PresentationCore.dll

using System.Globalization;
using System.Windows;

namespace System.Windows.Media
{
    public class FormattedText
    {
        public FormattedText(string textToFormat, CultureInfo culture, FlowDirection flowDirection, Typeface typeface,
                             double emSize, Brush foreground);

        public FormattedText(string textToFormat, CultureInfo culture, FlowDirection flowDirection, Typeface typeface,
                             double emSize, Brush foreground, NumberSubstitution numberSubstitution);

        public string Text { get; }
        public FlowDirection FlowDirection { get; set; }
        public TextAlignment TextAlignment { get; set; }
        public double LineHeight { get; set; }
        public double MaxTextWidth { get; set; }
        public double MaxTextHeight { get; set; }
        public int MaxLineCount { get; set; }
        public TextTrimming Trimming { get; set; }
        public double Height { get; }
        public double Extent { get; }
        public double Baseline { get; }
        public double OverhangAfter { get; }
        public double OverhangLeading { get; }
        public double OverhangTrailing { get; }
        public double Width { get; }
        public double WidthIncludingTrailingWhitespace { get; }
        public double MinWidth { get; }

        public void SetForegroundBrush(Brush foregroundBrush);
        public void SetForegroundBrush(Brush foregroundBrush, int startIndex, int count);
        public void SetFontFamily(string fontFamily);
        public void SetFontFamily(string fontFamily, int startIndex, int count);
        public void SetFontFamily(FontFamily fontFamily);
        public void SetFontFamily(FontFamily fontFamily, int startIndex, int count);
        public void SetFontSize(double emSize);
        public void SetFontSize(double emSize, int startIndex, int count);
        public void SetCulture(CultureInfo culture);
        public void SetCulture(CultureInfo culture, int startIndex, int count);
        public void SetNumberSubstitution(NumberSubstitution numberSubstitution);
        public void SetNumberSubstitution(NumberSubstitution numberSubstitution, int startIndex, int count);
        public void SetFontWeight(FontWeight weight);
        public void SetFontWeight(FontWeight weight, int startIndex, int count);
        public void SetFontStyle(FontStyle style);
        public void SetFontStyle(FontStyle style, int startIndex, int count);
        public void SetFontStretch(FontStretch stretch);
        public void SetFontStretch(FontStretch stretch, int startIndex, int count);
        public void SetFontTypeface(Typeface typeface);
        public void SetFontTypeface(Typeface typeface, int startIndex, int count);
        public void SetTextDecorations(TextDecorationCollection textDecorations);
        public void SetTextDecorations(TextDecorationCollection textDecorations, int startIndex, int count);
        public void SetMaxTextWidths(double[] maxTextWidths);
        public double[] GetMaxTextWidths();
        public Geometry BuildHighlightGeometry(Point origin);
        public Geometry BuildGeometry(Point origin);
        public Geometry BuildHighlightGeometry(Point origin, int startIndex, int count);
    }
}
