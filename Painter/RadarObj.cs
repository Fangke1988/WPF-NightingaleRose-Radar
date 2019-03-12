using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace Painter
{
    public class RadarObj
    {
        public string RColor { get; set; }
        public string Name { get; set; }
        public int DataValue { get; set; }
        public double DataRaidus { get; set; }

        /// <summary>
        /// Series stroke
        /// </summary>
        public Brush Stroke
        {
            get
            {
                return new SolidColorBrush((Color)ColorConverter.ConvertFromString(RColor)); ;
            }
        }

        /// <summary>
        /// Series Fill
        /// </summary>
        public Brush Fill
        {
            get
            {
                return new SolidColorBrush((Color)ColorConverter.ConvertFromString(RColor));
            }
        }
    }
}
