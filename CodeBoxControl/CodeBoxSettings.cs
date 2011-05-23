using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Windows;
namespace CodeBoxControl
{
    [Serializable]
  public  class CodeBoxSettings:DependencyObject
    {
        #region FontSize
        public static DependencyProperty FontSizeProperty = DependencyProperty.Register("FontSize", typeof(double), typeof(CodeBoxSettings),
        new PropertyMetadata(12d, new PropertyChangedCallback(CodeBoxSettings.OnFontSizeChanged)));

        public double FontSize
        {

            get { return (double)GetValue(FontSizeProperty); }

            set { SetValue(FontSizeProperty, value); }

        }


        private static void OnFontSizeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {


        }
 
        #endregion

        #region BaseColor
        public static DependencyProperty BaseColorProperty = DependencyProperty.Register("BaseColor", typeof(Color), typeof(CodeBoxSettings),
        new PropertyMetadata(Colors.Black , new PropertyChangedCallback(CodeBoxSettings.OnBaseColorChanged)));

        public Color BaseColor
        {

            get { return (Color)GetValue(BaseColorProperty); }

            set { SetValue(BaseColorProperty, value); }

        }


        private static void OnBaseColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {


        }

        #endregion

        #region BackgroundColor
        public static DependencyProperty BackgroundColorProperty = DependencyProperty.Register("BackgroundColor", typeof(Color), typeof(CodeBoxSettings),
        new PropertyMetadata(Colors.White , new PropertyChangedCallback(CodeBoxSettings.OnBackgroundColorChanged) ));

        public Color BackgroundColor
        {

            get { return (Color)GetValue(BackgroundColorProperty); }

            set { SetValue(BackgroundColorProperty, value); }

        }


        private static void OnBackgroundColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

           
        }
      
        #endregion

        #region FontFamily
        public static DependencyProperty FontFamilyProperty = DependencyProperty.Register("FontFamily", typeof(FontFamily), typeof(CodeBoxSettings),
        new PropertyMetadata(new FontFamily("Verdana"), new PropertyChangedCallback(CodeBoxSettings.OnFontFamilyChanged) ));

        public FontFamily FontFamily
        {

            get { return (FontFamily)GetValue(FontFamilyProperty); }

            set { SetValue(FontFamilyProperty, value); }

        }


        private static void OnFontFamilyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {


        }
    
        #endregion

    }
}
