using System;
using System.Globalization;
using System.Windows;
using System.Windows.Media;
namespace Painter
{
    /// <summary>
    /// 计算指定字符串占用的高宽 
    /// </summary>
    public class ControlSizeUtils
    {

        private static Size MeasureTextSize(string text, Typeface typeface, double fontSize)
        {
            var ft = new FormattedText(text, CultureInfo.CurrentCulture, FlowDirection.LeftToRight, typeface, fontSize, Brushes.Black);
            return new Size(ft.Width, ft.Height);
        }

        /// <summary>
        /// 衡量字符尺寸
        /// </summary>
        /// <returns></returns>
        public static Size GetTextAreaSize(string text, double fontsize)
        {
            FontFamily fontFamily; FontStyle fontStyle; FontWeight fontWeight; FontStretch fontStretch;
            fontFamily = new FontFamily("微软雅黑"); fontStyle = FontStyles.Normal;
            fontWeight = FontWeights.Normal;
            fontStretch = FontStretches.Normal;
            double fontSize = fontsize;
            if (text == null)
                return new Size(0, 0);

            Typeface typeface = new Typeface(fontFamily, fontStyle, fontWeight, fontStretch);
            GlyphTypeface gt;

            if (!typeface.TryGetGlyphTypeface(out gt))
                return MeasureTextSize(text, typeface, fontSize);

            double totalWidth = 0;
            double totalHeight = 0;

            // 替换换行符
            var array = text.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);

            foreach (var splitted in array)
            {
                double lineWidth = 0;
                double lineHeight = 0;

                // 计算每行的宽度和高度
                for (int n = 0; n < splitted.Length; n++)
                {
                    try
                    {
                        ushort glyphIndex = gt.CharacterToGlyphMap[splitted[n]];
                        double width = gt.AdvanceWidths[glyphIndex] * fontSize;
                        double height = gt.AdvanceHeights[glyphIndex] * fontSize;
                        lineHeight = Math.Max(totalHeight, height);
                        lineWidth += width;
                    }
                    catch (Exception exp)
                    {
                        lineWidth += fontSize;
                    }
                }
                totalWidth = Math.Max(totalWidth, lineWidth);
                totalHeight = Math.Max(totalHeight, lineHeight);
            }
            if (totalWidth < 5)
                totalWidth = 5;
            return new Size(totalWidth, totalHeight);
        }
    }
}
