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
    /// NightingaleRose.xaml 的交互逻辑
    /// </summary>
    public partial class NightingaleRose : UserControl
    {
        public NightingaleRose()
        {
            InitializeComponent();
        }

        #region Property

        /// <summary>
        /// 数据
        /// </summary>
        public List<RadarObj> Datas
        {
            get { return (List<RadarObj>)GetValue(DatasProperty); }
            set { SetValue(DatasProperty, value); }
        }

        /// <summary>
        /// 数值的总数
        /// </summary>
        public int Count
        {
            get
            {
                return Datas.Sum(i => i.DataValue);
            }
        }

        public static readonly DependencyProperty DatasProperty = DependencyProperty.Register("Datas", typeof(List<RadarObj>),
        typeof(NightingaleRose), new PropertyMetadata(new List<RadarObj>()));

        /// <summary>
        /// 当前绘制大区域
        /// </summary>
        private double MaxSize
        {
            get
            {
                var par = this.Parent as FrameworkElement;
                return par.ActualHeight > par.ActualWidth ? par.ActualWidth : par.ActualHeight;
            }
        }

        /// <summary>
        /// 停靠间距
        /// </summary>
        public int RoseMargin
        {
            get { return (int)GetValue(RoseMarginProperty); }
            set { SetValue(RoseMarginProperty, value); }
        }

        public static readonly DependencyProperty RoseMarginProperty = DependencyProperty.Register("RoseMargin", typeof(int),
        typeof(NightingaleRose), new PropertyMetadata(50));

        /// <summary>
        /// 空心内环半径
        /// </summary>
        public int RoseInsideMargin
        {
            get { return (int)GetValue(RoseInsideMarginProperty); }
            set { SetValue(RoseInsideMarginProperty, value); }
        }

        public static readonly DependencyProperty RoseInsideMarginProperty = DependencyProperty.Register("RoseInsideMargin", typeof(int),
        typeof(NightingaleRose), new PropertyMetadata(20));

        /// <summary>
        /// 显示值标注
        /// </summary>
        public bool ShowValuesLabel
        {
            get { return (bool)GetValue(ShowValuesLabelProperty); }
            set { SetValue(ShowValuesLabelProperty, value); }
        }

        public static readonly DependencyProperty ShowValuesLabelProperty = DependencyProperty.Register("ShowValuesLabel", typeof(bool),
        typeof(NightingaleRose), new PropertyMetadata(true));


        public static readonly DependencyProperty ShowToolTipProperty = DependencyProperty.Register("ShowToolTip", typeof(bool),
        typeof(NightingaleRose), new PropertyMetadata(false));

        /// <summary>
        /// 延伸线长
        /// </summary>
        public int LabelPathLength
        {
            get { return (int)GetValue(LabelPathLengthProperty); }
            set { SetValue(LabelPathLengthProperty, value); }
        }

        public static readonly DependencyProperty LabelPathLengthProperty = DependencyProperty.Register("LabelPathLength", typeof(int),
        typeof(NightingaleRose), new PropertyMetadata(50));

        #endregion Property

        #region Method

        /// <summary>
        /// 初始化数据
        /// </summary>
        private void initData()
        {
            CanvasPanel.Children.Clear();
            if (this.Datas != null && this.Datas.Count > 0)
            {
                this.CanvasPanel.Width = this.CanvasPanel.Height = 0;

                //求角度比例尺  (每个值占多大的角度  可以算到每一块图所占的角度)
                var angelScale = 360.00 / Datas.Sum(i => i.DataValue);

                //最大半径
                var maxRadius = (MaxSize / 2) - RoseMargin - (ShowValuesLabel ? LabelPathLength : 0);

                //半径比例尺  （值和比例尺相乘等于每一块图的半径）
                var radiusScale = maxRadius / Datas.Max(o => o.DataValue);

                //计算半径宽度值
                for (int i = 0; i < Datas.Count; i++)
                {
                    Datas[i].DataRaidus = Datas[i].DataValue * radiusScale;
                }
                //扇形角度初始化
                double angleSectorStart = 0;
                double angleSectorEnd = 0;

                //循环绘制扇形区域
                for (int index = 0; index < Datas.Count; index++)
                {
                    //计算扇形角度
                    if (index == 0)
                    {
                        angleSectorStart = 0;
                        angleSectorEnd = Datas[index].DataValue * angelScale;
                    }
                    else if (index + 1 == Datas.Count)
                    {
                        angleSectorStart += Datas[index - 1].DataValue * angelScale;
                        angleSectorEnd = 360;
                    }
                    else
                    {
                        angleSectorStart += Datas[index - 1].DataValue * angelScale;
                        angleSectorEnd = angleSectorStart + Datas[index].DataValue * angelScale;
                    }
                    var currentRadius = RoseInsideMargin + Datas[index].DataRaidus;

                    Point ptOutSideStart = GetPoint(currentRadius, angleSectorStart * Math.PI / 180);
                    Point ptOutSideEnd = GetPoint(currentRadius, angleSectorEnd * Math.PI / 180);
                    Point ptInSideStart = GetPoint(RoseInsideMargin, angleSectorStart * Math.PI / 180);
                    Point ptInSideEnd = GetPoint(RoseInsideMargin, angleSectorEnd * Math.PI / 180);
                    if (string.IsNullOrEmpty(Datas[index].RColor) )
                        Datas[index].RColor = ChartColorPool.ColorStrings[index];
                    Path pthSector = new Path() { Fill = Datas[index].Fill };
                    //PATH数据格式 M0,100  L50,100 A50,50 0 0 1 100,50 L100,0 A100,100 0 0 0 0,100 Z
                    StringBuilder datastrb = new StringBuilder();

                    #region BuilderPathData

                    datastrb.Append("M");
                    datastrb.Append(ptOutSideStart.X.ToString());
                    datastrb.Append(",");
                    datastrb.Append(ptOutSideStart.Y.ToString());
                    datastrb.Append(" L");
                    datastrb.Append(ptInSideStart.X.ToString());
                    datastrb.Append(",");
                    datastrb.Append(ptInSideStart.Y.ToString());
                    datastrb.Append(" A");
                    datastrb.Append(RoseInsideMargin.ToString());
                    datastrb.Append(",");
                    datastrb.Append(RoseInsideMargin.ToString());
                    datastrb.Append(" 0 0 1 ");
                    datastrb.Append(ptInSideEnd.X.ToString());
                    datastrb.Append(",");
                    datastrb.Append(ptInSideEnd.Y.ToString());
                    datastrb.Append(" L");
                    datastrb.Append(ptOutSideEnd.X.ToString());
                    datastrb.Append(",");
                    datastrb.Append(ptOutSideEnd.Y.ToString());
                    datastrb.Append(" A");
                    datastrb.Append(currentRadius.ToString());
                    datastrb.Append(",");
                    datastrb.Append(currentRadius.ToString());
                    datastrb.Append(" 0 0 0 ");
                    datastrb.Append(ptOutSideStart.X.ToString());
                    datastrb.Append(",");
                    datastrb.Append(ptOutSideStart.Y.ToString());
                    datastrb.Append(" Z");

                    #endregion BuilderPathData
                    try
                    {
                        pthSector.Data = (Geometry)new GeometryConverter().ConvertFromString(datastrb.ToString());
                    }
                    catch (Exception exp)
                    { }
                    CanvasPanel.Children.Add(pthSector);

                    if (ShowValuesLabel)
                    {
                        //计算延伸线角度
                        double lbPathAngle = angleSectorStart + (angleSectorEnd - angleSectorStart) / 2;
                        //起点
                        Point ptLbStart = GetPoint(currentRadius, lbPathAngle * Math.PI / 180);
                        //终点
                        Point ptLbEnd = GetPoint(maxRadius + LabelPathLength, lbPathAngle * Math.PI / 180);
                        Path pthLb = new Path() { Stroke = Datas[index].Stroke, StrokeThickness = 1 };
                        pthLb.Data = (Geometry)new GeometryConverter().ConvertFromString(string.Format("M{0},{1} {2},{3}", ptLbStart.X.ToString(), ptLbStart.Y.ToString(), ptLbEnd.X.ToString(), ptLbEnd.Y.ToString()));
                        CanvasPanel.Children.Add(pthLb);
                        SetLabel(Datas[index], ptLbEnd);
                    }
                }
                this.SizeChanged -= RadarControl_SizeChanged;
                this.SizeChanged += RadarControl_SizeChanged;
            }
        }

        public void InitalControl()
        {
        }

        /// <summary>
        /// 初始化数据
        /// </summary>
        /// <param name="dataobj"></param>
        public void SetData(object dataobj)
        {
            this.Datas = (dataobj) as List<RadarObj>;
            this.initData();
        }

        private void RadarControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            initData();
        }

        #endregion Method

        #region Compare

        /// <summary>
        /// 计算点位
        /// </summary>
        /// <param name="radius"></param>
        /// <param name="angel"></param>
        /// <returns></returns>
        private Point GetPoint(double radius, double angel)
        {
            return new Point(radius * Math.Cos(angel), radius * Math.Sin(angel));
        }

        private void SetLabel(RadarObj obj, Point location)
        {
            //计算偏移量
            bool x = true;
            bool y = true;

            if (location.X < 0)
                x = false;
            if (location.Y < 0)
                y = false;
            //obj.Name + " " + 
            TextBlock txb = new TextBlock() { Text = " "+obj.Name+" "+Getbfb(Count.ToString(), obj.DataValue.ToString(), 2)+ " ", Foreground = this.Foreground, FontSize = this.FontSize };
            Size s = ControlSizeUtils.GetTextAreaSize(txb.Text, this.FontSize);
            CanvasPanel.Children.Add(txb);
            if (location.X > -5 && location.X < 5)
                Canvas.SetLeft(txb, location.X - (s.Width / 2));
            else
                Canvas.SetLeft(txb, location.X + (x ? 0 : -(s.Width)));

            if (location.Y > -5 && location.Y < 5)
                Canvas.SetTop(txb, location.Y - (s.Height / 2));
            else
                Canvas.SetTop(txb, location.Y + (y ? 0 : -(s.Height)));
        }
        /// <summary>
        /// 计算百分比
        /// </summary>
        /// <param name="zs">总数</param>
        /// <param name="tj">当前项的值</param>
        /// <param name="num">保留的小数点几位</param>
        /// <returns></returns>
        public static string Getbfb(string zs, string tj, int num)
        {
            try
            {
                if (zs.Equals("0"))
                {
                    return "0";
                }
                double bfb = (double.Parse(tj) / double.Parse(zs)) * 100;
                if (bfb >= 100)
                {
                    bfb = 100;
                }

                return Math.Round(bfb, num).ToString() + "%";
            }
            catch (Exception ex)
            {
                return "0%";
            }
        }

        #endregion Compare

        #region ToolTipEvent

        private void CanvasPanel_MouseEnter(object sender, MouseEventArgs e)
        {
            List<RadarObj> list = Datas;
            for (int i = 0; i < Datas.Count; i++)
            {
                Grid g = new Grid();
                g.Margin = new Thickness() { Left = 5, Right = 5, Bottom = 5, Top = 5 };
                ColumnDefinition column1 = new ColumnDefinition();
                column1.Width = GridLength.Auto;
                ColumnDefinition column2 = new ColumnDefinition();
                column2.Width = GridLength.Auto;
                ColumnDefinition column3 = new ColumnDefinition();
                column3.Width = GridLength.Auto;
                g.ColumnDefinitions.Add(column1);
                g.ColumnDefinitions.Add(column2);
                g.ColumnDefinitions.Add(column3);
                Rectangle rectangle = new Rectangle();
                rectangle.Stroke = Datas[i].Stroke;
                rectangle.Fill = Datas[i].Fill;
                rectangle.Width = 20;
                rectangle.Height = 15;
                rectangle.RadiusX = 2;
                rectangle.RadiusY = 2;
                TextBlock textBlock = new TextBlock();
                textBlock.Margin = new Thickness() { Left = 5, Right = 5 };
                textBlock.Text = Datas[i].Name;
                textBlock.Foreground = Brushes.White;
                textBlock.VerticalAlignment = VerticalAlignment.Center;

                TextBlock textBlockValue = new TextBlock();
                textBlockValue.Margin = new Thickness() { Left = 0, Right = 0 };
                textBlockValue.Text = Datas[i].DataValue.ToString();
                textBlockValue.Foreground = Brushes.White;
                textBlockValue.VerticalAlignment = VerticalAlignment.Center;

                g.Children.Add(rectangle);
                g.Children.Add(textBlock);
                g.Children.Add(textBlockValue);
                Grid.SetColumn(rectangle, 0);
                Grid.SetColumn(textBlock, 1);
                Grid.SetColumn(textBlockValue, 2);
            }
        }
        #endregion ToolTipEvent
    }
}
