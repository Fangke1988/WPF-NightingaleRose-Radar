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
    /// RadarControl.xaml 的交互逻辑
    /// </summary>
    public partial class RadarControl : UserControl
    {
        public RadarControl()
        {
            InitializeComponent();
        }

        #region 属性

        /// <summary>
        /// 数值区域填充色
        /// </summary>
        public Brush AreaBrush
        {
            get { return (Brush)GetValue(AreaBrushProperty); }
            set { SetValue(AreaBrushProperty, value); }
        }

        public static readonly DependencyProperty AreaBrushProperty = DependencyProperty.Register("AreaBrush", typeof(Brush),
        typeof(RadarControl), new PropertyMetadata(Brushes.Black));

        /// <summary>
        /// 数值区域点填充色
        /// </summary>
        public Brush AreaPointBrush
        {
            get { return (Brush)GetValue(AreaPointBrushProperty); }
            set { SetValue(AreaPointBrushProperty, value); }
        }

        public static readonly DependencyProperty AreaPointBrushProperty = DependencyProperty.Register("AreaPointBrush", typeof(Brush),
        typeof(RadarControl), new PropertyMetadata(Brushes.Black));

        /// <summary>
        /// 雷达网格线填充充色
        /// </summary>
        public Brush RadarNetBrush
        {
            get { return (Brush)GetValue(RadarNetBrushProperty); }
            set { SetValue(RadarNetBrushProperty, value); }
        }

        public static readonly DependencyProperty RadarNetBrushProperty = DependencyProperty.Register("RadarNetBrush", typeof(Brush),
        typeof(RadarControl), new PropertyMetadata(Brushes.Black));

        /// <summary>
        /// 雷达网格线宽度
        /// </summary>
        public double RadarNetThickness
        {
            get { return (double)GetValue(RadarNetThicknessProperty); }
            set { SetValue(RadarNetThicknessProperty, value); }
        }

        public static readonly DependencyProperty RadarNetThicknessProperty = DependencyProperty.Register("RadarNetThickness", typeof(double),
        typeof(RadarControl), new PropertyMetadata(1.0));

        /// <summary>
        /// 数值点高宽度,0为不显示
        /// </summary>
        public double AreaPointSize
        {
            get { return (double)GetValue(AreaPointSizeProperty); }
            set { SetValue(AreaPointSizeProperty, value); }
        }

        public static readonly DependencyProperty AreaPointSizeProperty = DependencyProperty.Register("AreaPointSize", typeof(double),
        typeof(RadarControl), new PropertyMetadata(10.0));

        /// <summary>
        /// 纬线数量
        /// </summary>
        public int LatitudeCount
        {
            get { return (int)GetValue(LatitudeCountProperty); }
            set { SetValue(LatitudeCountProperty, value); }
        }

        public static readonly DependencyProperty LatitudeCountProperty = DependencyProperty.Register("LatitudeCount", typeof(int),
        typeof(RadarControl), new PropertyMetadata(5));

        /// <summary>
        /// 网格图停靠间距
        /// </summary>
        public int RadarNetMargin
        {
            get { return (int)GetValue(RadarNetMarginProperty); }
            set { SetValue(RadarNetMarginProperty, value); }
        }

        public static readonly DependencyProperty RadarNetMarginProperty = DependencyProperty.Register("RadarNetMargin", typeof(int),
        typeof(RadarControl), new PropertyMetadata(50));

        /// <summary>
        /// 显示值标注
        /// </summary>
        public bool ShowValuesLabel
        {
            get { return (bool)GetValue(ShowValuesLabelProperty); }
            set { SetValue(ShowValuesLabelProperty, value); }
        }

        public static readonly DependencyProperty ShowValuesLabelProperty = DependencyProperty.Register("ShowValuesLabel", typeof(bool),
        typeof(RadarControl), new PropertyMetadata(true));

        /// <summary>
        /// 显示组标签
        /// </summary>
        public bool ShowGroupsLabel
        {
            get { return (bool)GetValue(ShowGroupsLabelProperty); }
            set { SetValue(ShowGroupsLabelProperty, value); }
        }

        public static readonly DependencyProperty ShowGroupsLabelProperty = DependencyProperty.Register("ShowGroupsLabel", typeof(bool),
        typeof(RadarControl), new PropertyMetadata(true));

        /// <summary>
        /// 是否为多重绘图模式(默认否)
        /// </summary>
        public bool MoreGraphics
        {
            get { return (bool)GetValue(MoreGraphicsProperty); }
            set { SetValue(MoreGraphicsProperty, value); }
        }

        public static readonly DependencyProperty MoreGraphicsProperty = DependencyProperty.Register("MoreGraphics", typeof(bool),
        typeof(RadarControl), new PropertyMetadata(false));

        /// <summary>
        /// 分隔角度
        /// </summary>
        private double Angle
        {
            get
            {
                int count = MoreGraphics ? MoreDatas[0].Count : Datas.Count;
                double angle = 360 / count;
                return angle;
            }
        }

        /// <summary>
        /// 数据
        /// </summary>
        public List<RadarObj> Datas
        {
            get { return (List<RadarObj>)GetValue(DatasProperty); }
            set { SetValue(DatasProperty, value); }
        }

        public static readonly DependencyProperty DatasProperty = DependencyProperty.Register("Datas", typeof(List<RadarObj>),
        typeof(RadarControl), new PropertyMetadata(new List<RadarObj>()));
        /// <summary>
        /// 多元数据
        /// </summary>
        public List<RadarObj>[] MoreDatas
        {
            get { return (List<RadarObj>[])GetValue(MoreDatasProperty); }
            set { SetValue(MoreDatasProperty, value); }
        }

        public static readonly DependencyProperty MoreDatasProperty = DependencyProperty.Register("MoreDatas", typeof(List<RadarObj>[]),
        typeof(RadarControl), new PropertyMetadata(new List<RadarObj>[2]));

        /// <summary>
        /// 多元数据画笔
        /// </summary>
        public List<Brush> RadarNetBrushes
        {
            get { return (List<Brush>)GetValue(RadarNetBrushesProperty); }
            set { SetValue(RadarNetBrushesProperty, value); }
        }

        public static readonly DependencyProperty RadarNetBrushesProperty = DependencyProperty.Register("RadarNetBrushes", typeof(List<Brush>),
        typeof(RadarControl), new PropertyMetadata(new List<Brush>()));

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

        #endregion 属性

        private void RadarControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (!MoreGraphics)
                InitalData();
            else
                InitalMoreData();

        }

        /// <summary>
        /// 设置标注
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="location"></param>
        /// <param name="isGroupLabel"></param>
        private void SetLabel(RadarObj obj, Point location, bool isGroupLabel)
        {
            //计算偏移量
            bool x = true;
            bool y = true;

            if (location.X < 0)
                x = false;
            if (location.Y < 0)
                y = false;

            TextBlock txb = new TextBlock() { Text = isGroupLabel ? obj.Name : obj.DataValue.ToString(), Foreground = this.Foreground, FontSize = this.FontSize };
            Size s = ControlSizeUtils.GetTextAreaSize(txb.Text,this.FontSize);
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
        /// 设置数据显示
        /// </summary>
        private void InitalData()
        {
            CanvasPanel.Children.Clear();
            if (Datas != null && Datas.Count > 0)
            {
                this.CanvasPanel.Width = this.CanvasPanel.Height = 0;

                //计算比例尺
                var scale = ((MaxSize / 2) - RadarNetMargin) / Datas.Max(i => i.DataValue);

                //计算实际半径
                for (int i = 0; i < Datas.Count; i++)
                {
                    Datas[i].DataRaidus = Datas[i].DataValue * scale;
                }

                //获取最大数值
                double maxData = Datas.Max(i => i.DataRaidus);

                //计算纬线间距半径
                double length = maxData / LatitudeCount;

                for (int index = 1; index < LatitudeCount + 1; index++)
                {
                    //多边形半径
                    var r = length * index;
                    Polygon polygonBorder = new Polygon() { Fill = Brushes.Transparent, Stroke = RadarNetBrush, StrokeThickness = RadarNetThickness };
                    //绘制多边形
                    for (int currentIndex = 0; currentIndex < Datas.Count; currentIndex++)
                    {
                        double angle = ((Angle * currentIndex + 90) / 360) * 2 * Math.PI;
                        polygonBorder.Points.Add(new Point(r * Math.Cos(angle), r * Math.Sin(angle)));
                    }
                    CanvasPanel.Children.Add(polygonBorder);
                }

                //数值区域多边形
                Polygon polygonArea = new Polygon() { Fill = AreaBrush, Opacity = 0.5, Stroke = AreaPointBrush, StrokeThickness = 5 };

                //经线长度
                var maxRadius = LatitudeCount * length;

                List<Ellipse> ellipselst = new List<Ellipse>();
                Dictionary<RadarObj, Point> valuesLabelLocations = new Dictionary<RadarObj, Point>();
                Dictionary<RadarObj, Point> groupLabelLocations = new Dictionary<RadarObj, Point>();

                //绘制数据多边形
                for (int Index = 0; Index < Datas.Count; Index++)
                {
                    //计算角度
                    double angle = ((Angle * Index + 90) / 360) * 2 * Math.PI;
                    //计算距离值
                    var cou = Datas[Index].DataRaidus / length;  //计算倍距
                    var rac = Datas[Index].DataRaidus % length;//计算余距
                    double Radius = cou * length + rac;

                    //超过最大半径则设置为最大半径
                    if (Radius > maxRadius)
                    {
                        Radius = maxRadius;
                    }
                    Point pt = new Point(Radius * Math.Cos(angle), Radius * Math.Sin(angle));
                    polygonArea.Points.Add(pt);
                    valuesLabelLocations.Add(Datas[Index], new Point((Radius) * Math.Cos(angle), (Radius) * Math.Sin(angle)));//记录点位标注标识
                                                                                                                              //设置数值点，如果数值点尺寸大于0则绘制
                    if (AreaPointSize > 0)
                    {
                        var ellipse = new Ellipse() { Width = AreaPointSize, Height = AreaPointSize, Fill = AreaPointBrush };
                        //var ellipse = new Ellipse() { Width = AreaPointSize, Height = AreaPointSize, Fill = Datas[Index].Fill }; AreaPointBrush
                        Canvas.SetLeft(ellipse, pt.X - (AreaPointSize / 2));
                        Canvas.SetTop(ellipse, pt.Y - (AreaPointSize / 2));
                        ellipselst.Add(ellipse);
                    }

                    Point ptMax = new Point(maxRadius * Math.Cos(angle), maxRadius * Math.Sin(angle));

                    //记录组点位标注标识
                    groupLabelLocations.Add(Datas[Index], new Point((maxRadius + 20) * Math.Cos(angle), (maxRadius + 20) * Math.Sin(angle)));

                    //绘制经线
                    Path pth = new Path() { Stroke = RadarNetBrush, StrokeThickness = RadarNetThickness };
                    // Path pth = new Path() { Stroke = Datas[Index].Stroke, StrokeThickness = RadarNetThickness };
                    pth.Data = (Geometry)new GeometryConverter().ConvertFromString(String.Format("M0,0 {0},{1}", ptMax.X.ToString(), ptMax.Y.ToString()));
                    CanvasPanel.Children.Add(pth);
                }
                CanvasPanel.Children.Add(polygonArea);

                //绘点
                foreach (var elc in ellipselst)
                    CanvasPanel.Children.Add(elc);

                //标注值标签
                if (ShowValuesLabel)
                {
                    foreach (var item in valuesLabelLocations)
                    {
                        // SetLabel(item.Key, item.Value, false);
                    }
                }
                //标注组标签
                if (ShowGroupsLabel)
                {
                    foreach (var item in groupLabelLocations)
                        SetLabel(item.Key, item.Value, true);
                }
                this.SizeChanged -= RadarControl_SizeChanged;
                this.SizeChanged += RadarControl_SizeChanged;
            }
        }

        /// <summary>
        /// 设置数据显示
        /// </summary>
        private void InitalMoreData()
        {
            CanvasPanel.Children.Clear();
            if (this.MoreDatas != null && MoreDatas.Length > 0)
            {
                this.CanvasPanel.Width = this.CanvasPanel.Height = 0;

                //计算比例尺
                var max = 0.00;
                foreach (var item in MoreDatas)
                    if (item.Max(i => i.DataValue) > max)
                        max = item.Max(i => i.DataValue);
                var scale = ((MaxSize / 2) - RadarNetMargin) / max;

                //计算实际半径
                for (int index = 0; index < MoreDatas.Length; index++)
                    for (int i = 0; i < MoreDatas[index].Count; i++)
                        MoreDatas[index][i].DataRaidus = MoreDatas[index][i].DataValue * scale;


                //获取最大数值
                double maxData = 0.000;
                foreach (var item in MoreDatas)
                    if (item.Max(i => i.DataRaidus) > maxData)
                        maxData = item.Max(i => i.DataRaidus);

                //计算纬线间距半径
                double length = maxData / LatitudeCount;

                for (int index = 1; index < LatitudeCount + 1; index++)
                {
                    //多边形半径
                    var r = length * index;//RadarNetBrush
                    Polygon polygonBorder = new Polygon() { Fill = Brushes.Transparent, Stroke = RadarNetBrush, StrokeThickness = RadarNetThickness };
                    //绘制多边形
                    for (int currentIndex = 0; currentIndex < MoreDatas[0].Count; currentIndex++)
                    {
                        double angle = ((Angle * currentIndex + 90) / 360) * 2 * Math.PI;
                        polygonBorder.Points.Add(new Point(r * Math.Cos(angle), r * Math.Sin(angle)));
                    }
                    CanvasPanel.Children.Add(polygonBorder);
                }

                //数值区域多边形
                List<Polygon> polygonAreaList = new List<Polygon>();
                for (int index = 0; index < this.MoreDatas.Length; index++)
                    polygonAreaList.Add(new Polygon() { Fill = RadarNetBrushes[index], Opacity = 0.5, Stroke = Brushes.LightGreen, StrokeThickness = 3 });

                //经线长度
                var maxRadius = LatitudeCount * length;

                List<Ellipse> ellipselst = new List<Ellipse>();
                Dictionary<RadarObj, Point> valuesLabelLocations = new Dictionary<RadarObj, Point>();
                Dictionary<RadarObj, Point> groupLabelLocations = new Dictionary<RadarObj, Point>();


                //绘制数据多边形
                for (int Index = 0; Index < MoreDatas[0].Count; Index++)
                {
                    //计算角度
                    double angle = ((Angle * Index + 90) / 360) * 2 * Math.PI;

                    //逐步生成每类数据的各组实际数据:例如A、B、C、D的交通数据数据
                    for (int dataIndex = 0; dataIndex < MoreDatas.Length; dataIndex++)
                    {
                        //计算距离值
                        var cou = MoreDatas[dataIndex][Index].DataRaidus / length;  //计算倍距
                        var rac = MoreDatas[dataIndex][Index].DataRaidus % length;//计算余距
                        double Radius = cou * length + rac;

                        //超过最大半径则设置为最大半径
                        if (Radius > maxRadius)
                        {
                            Radius = maxRadius;
                        }
                        Point pt = new Point(Radius * Math.Cos(angle), Radius * Math.Sin(angle));
                        polygonAreaList[dataIndex].Points.Add(pt);
                        //valuesLabelLocations.Add(Datas[Index], new Point((Radius) * Math.Cos(angle), (Radius) * Math.Sin(angle)));//记录点位标注标识
                        //设置数值点，如果数值点尺寸大于0则绘制
                        if (AreaPointSize > 0)
                        {
                            var ellipse = new Ellipse() { Width = AreaPointSize / 2, Height = AreaPointSize / 2, Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString(ChartColorPool.ColorStrings[Index])) };
                            Canvas.SetLeft(ellipse, pt.X - (AreaPointSize / 4));
                            Canvas.SetTop(ellipse, pt.Y - (AreaPointSize / 4));
                            ellipselst.Add(ellipse);
                        }
                    }
                    Point ptMax = new Point(maxRadius * Math.Cos(angle), maxRadius * Math.Sin(angle));

                    //记录组点位标注标识
                    groupLabelLocations.Add(MoreDatas[0][Index], new Point((maxRadius + 20) * Math.Cos(angle), (maxRadius + 20) * Math.Sin(angle)));

                    //绘制经线  RadarNetBrush
                    Path pth = new Path() { Stroke = RadarNetBrush, StrokeThickness = RadarNetThickness };
                    pth.Data = (Geometry)new GeometryConverter().ConvertFromString(String.Format("M0,0 {0},{1}", ptMax.X.ToString(), ptMax.Y.ToString()));
                    CanvasPanel.Children.Add(pth);
                }
                foreach (var polygonArea in polygonAreaList)
                    CanvasPanel.Children.Add(polygonArea);

                //绘点
                foreach (var elc in ellipselst)
                    CanvasPanel.Children.Add(elc);

                //标注组标签
                if (ShowGroupsLabel)
                {
                    foreach (var item in groupLabelLocations)
                        SetLabel(item.Key, item.Value, true);
                }
                this.SizeChanged -= RadarControl_SizeChanged;
                this.SizeChanged += RadarControl_SizeChanged;
            }
        }

        public void InitalControl()
        {
        }

        /// <summary>
        /// 加载数据
        /// </summary>
        /// <param name="dataobj"></param>
        public void SetData(object dataobj)
        {
            if (!MoreGraphics)
            {
                this.Datas = (dataobj) as List<RadarObj>;
                this.InitalData();
            }
            else
            {
                this.MoreDatas = (dataobj) as List<RadarObj>[];
                InitalMoreData();
            }

        }

        private Brush _stroke = Brushes.Yellow;

        /// <summary>
        /// Series stroke
        /// </summary>
        public Brush Stroke
        {
            get
            {
                return _stroke;
            }
            set
            {
                _stroke = value;
            }
        }

        private Brush _fill = Brushes.Yellow;

        /// <summary>
        /// Series Fill
        /// </summary>
        public Brush Fill
        {
            get
            {
                return _fill;
            }
            set
            {
                _fill = value;
            }
        }
    }
}
