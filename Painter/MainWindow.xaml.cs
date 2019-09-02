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
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

       

   
        Random rdm = new Random();

        private void RoseClick(object sender, RoutedEventArgs e)
        {
            NightingaleRose rdc = new NightingaleRose();
            this.GrdMain.Children.Clear();
            this.GrdMain.Children.Add(rdc);
            rdc.SetData(CrData());
        }
        private void RadarClick(object sender, RoutedEventArgs e)
        {
            RadarControl rdc = new RadarControl() { AreaBrush = Brushes.Black, RadarNetBrush = Brushes.Black, AreaPointBrush = Brushes.Orange, BorderBrush = Brushes.Gray };
            this.GrdMain.Children.Clear();
            this.GrdMain.Children.Add(rdc);

            rdc.SetData(CrData());
        }
        private void RadarsClick(object sender, RoutedEventArgs e)
        {
            RadarControl rdc = new RadarControl() { MoreGraphics = true, AreaBrush = Brushes.Black, RadarNetBrush = Brushes.Black, AreaPointBrush = Brushes.Orange, BorderBrush = Brushes.Gray, RadarNetBrushes = new List<Brush> { Brushes.LightSkyBlue, Brushes.Violet } };
            this.GrdMain.Children.Clear();
            this.GrdMain.Children.Add(rdc);
            List<RadarObj>[] lst = { CrData(), CrData() };
            rdc.SetData(lst);
        }
        private List<RadarObj> CrData()
        {
            List<RadarObj> list = new List<RadarObj>();
            list.Add(new RadarObj() { Name="A", DataValue= rdm.Next(20,100) });
            list.Add(new RadarObj() { Name = "B", DataValue = rdm.Next(20, 100) });
            list.Add(new RadarObj() { Name = "C", DataValue = rdm.Next(20, 100) });
            list.Add(new RadarObj() { Name = "D", DataValue = rdm.Next(20, 100) });
            list.Add(new RadarObj() { Name = "E", DataValue = rdm.Next(20, 100) });
            list.Add(new RadarObj() { Name = "F", DataValue = rdm.Next(20, 100) });
            list.Add(new RadarObj() { Name = "F", DataValue = rdm.Next(20, 100) });
            return list;
        }

       

        private void RoseSortClick(object sender, RoutedEventArgs e)
        {
            NightingaleRose rdc = new NightingaleRose();
            this.GrdMain.Children.Clear();
            this.GrdMain.Children.Add(rdc);
            rdc.SetData(CrData().OrderByDescending(o=>o.DataValue).ToList());
        }
    }
}
