using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Painter
{
    /// <summary>
    /// _3DTest.xaml 的交互逻辑
    /// </summary>
    public partial class _3DTest : UserControl
    {
        public _3DTest()
        {
            InitializeComponent();
            InitializeComponent();

            Random r = new Random();
            for (int i = 0; i < 12; i++)
            {
                StackPanel sp = Draw3DHitsgram((Double)(r.Next(20, 50)), (Double)(r.Next(40, 200)), i);
                Canvas.SetLeft(sp, 20 + 70 * i);
                Canvas.SetTop(sp, 250 - sp.Height);
                this.PicBox.Children.Add(sp);
            }
        }

        private Color[,] colorsSolution = new Color[12, 3]
             {
             {Color.FromRgb(246,188,16),Color.FromRgb(194,153,11),Color.FromRgb(158,123,3)},
             {Color.FromRgb(175,216,248),Color.FromRgb(135,173,196),Color.FromRgb(111,139,161)},
             {Color.FromRgb(215,69,70),Color.FromRgb(167,55,54),Color.FromRgb(138,44,44)},
             {Color.FromRgb(140,186,0),Color.FromRgb(112,147,1),Color.FromRgb(92,121,2)},
             {Color.FromRgb(253,143,68),Color.FromRgb(200,114,55),Color.FromRgb(165,95,76)},
             {Color.FromRgb(0,142,143),Color.FromRgb(0,113,113),Color.FromRgb(2,92,93)},
             {Color.FromRgb(142,70,143),Color.FromRgb(117,56,116),Color.FromRgb(88,46,90)},
             {Color.FromRgb(90,133,41),Color.FromRgb(70,107,30),Color.FromRgb(58,87,23)},
             {Color.FromRgb(178,170,21),Color.FromRgb(142,133,0),Color.FromRgb(115,108,2)},
             {Color.FromRgb(1,141,216),Color.FromRgb(3,112,175),Color.FromRgb(1,92,137)},
             {Color.FromRgb(158,9,13),Color.FromRgb(130,7,10),Color.FromRgb(101,5,7)},
             {Color.FromRgb(161,134,189),Color.FromRgb(127,105,151),Color.FromRgb(107,86,125)}
             };


        public StackPanel Draw3DHitsgram(Double width, Double height, int colorSolutionIndex)
        {
            StackPanel sp = new StackPanel()
            {
                Orientation = Orientation.Horizontal,
                Height = height + width / 3,
                Width = width / 3 * 4,Background= Brushes.Gray
            };

            Rectangle rect1 = new Rectangle()
            {
                Fill = new SolidColorBrush(colorsSolution[colorSolutionIndex, 0]),
                Width = width,
                Height = height,
                VerticalAlignment = VerticalAlignment.Bottom
            };
            sp.Children.Add(rect1);
            Rectangle rect2 = new Rectangle()
            {
                Fill = new SolidColorBrush(colorsSolution[colorSolutionIndex, 1]),
                Width = width,
                Height = width / 3,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(-width / 3 * 2, 0, 0, 0),
                RenderTransform = new SkewTransform(-45, 0, 0, 0)
            };
            sp.Children.Add(rect2);
            Rectangle rect3 = new Rectangle()
            {
                Fill = new SolidColorBrush(colorsSolution[colorSolutionIndex, 2]),
                Width = width / 3,
                Height = height,
                VerticalAlignment = VerticalAlignment.Bottom,
                Margin = new Thickness(-width / 3, 0, 0, 0),
                RenderTransform = new SkewTransform(0, -45, 0, 0)
            };
            sp.Children.Add(rect3);

            return sp;
        }
    }
}
