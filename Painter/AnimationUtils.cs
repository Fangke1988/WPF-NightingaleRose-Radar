using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Painter
{
    /// <summary>
    /// 动画帮助类
    /// </summary>
    public class AnimationUtils
    {
        /// <summary>
        /// 渐影动画
        /// </summary>
        /// <param name="element">控件对象</param>
        /// <param name="seconds">播放时间长度</param>
        /// <param name="lateSeconds">延迟播放时间</param>
        public static void CtrlDoubleAnimation(UIElement element, double mseconds, double mlateSeconds)
        {
            element.Visibility = Visibility.Collapsed;
            DoubleAnimation doubleAnimation = new DoubleAnimation();
            doubleAnimation.From = 0;
            doubleAnimation.To = 1;
            doubleAnimation.Duration = TimeSpan.FromMilliseconds(mseconds);
            EventHandler handler;
            doubleAnimation.Completed += handler = (s, e) =>{
                element.Visibility = Visibility.Visible;
            };
            doubleAnimation.BeginTime = TimeSpan.FromMilliseconds(mlateSeconds);
            element.BeginAnimation(UIElement.OpacityProperty, doubleAnimation);

        }

        /// <summary>
        /// 透明度动画 
        /// </summary>
        /// <param name="elem"></param>
        /// <param name="to"></param>
        public static void FloatElement(UIElement elem, double to, double mseconds, double mlateSeconds)
        {
            lock (elem)
            {
                if (to == 1)
                {
                    elem.Visibility = Visibility.Collapsed;
                }
                //else
                //{
                //    elem.Visibility = Visibility.Visible;
                //}
                DoubleAnimation opacity = new DoubleAnimation()
                {
                    To = to,
                    Duration = TimeSpan.FromMilliseconds(mseconds),
                    BeginTime = TimeSpan.FromMilliseconds(mlateSeconds)
            };
                EventHandler handler = null;
                opacity.Completed += handler = (s, e) =>
                {
                    opacity.Completed -= handler;
                    if (to == 1)
                    {
                        elem.Visibility = Visibility.Visible;
                    }
                    //else
                    //{
                    //    elem.Visibility = Visibility.Visible;
                    //}
                    opacity = null;
                };
                elem.BeginAnimation(UIElement.OpacityProperty, opacity);
            }
        }


        /// <summary>
        /// 支撑同时旋转和缩放的动画
        /// </summary>
        /// <param name="element">控件</param>
        /// <param name="from">元素开始的大小</param>
        /// <param name="to">元素到达的大小</param>
        /// <param name="time">动画世界</param>
        /// <param name="completed">结束事件</param>
        public static void ScaleRotateEasingAnimationShow(UIElement element, double from, double to, double mseconds, double mlateSeconds, EventHandler completed)
        {
            //旋转
            RotateTransform angle = new RotateTransform();

            //缩放
            ScaleTransform scale = new ScaleTransform();
            TransformGroup group = new TransformGroup();
            group.Children.Add(scale);
            group.Children.Add(angle);
            element.RenderTransform = group;

            //定义圆心位置
            element.RenderTransformOrigin = new Point(0.5, 0.5);
            EasingFunctionBase easeFunction = new PowerEase()
            {
                EasingMode = EasingMode.EaseInOut,
                Power = 2
            };

            // 动画参数
            DoubleAnimation scaleAnimation = new DoubleAnimation()
            {
                From = from,
                To = to,
                EasingFunction = easeFunction,
                Duration = TimeSpan.FromMilliseconds(mseconds),
                BeginTime = TimeSpan.FromMilliseconds(mlateSeconds),
                FillBehavior = FillBehavior.Stop
            };

            // 动画参数
            DoubleAnimation angleAnimation = new DoubleAnimation()
            {
                From = 0,
                To = 360,
                EasingFunction = easeFunction,
                Duration = TimeSpan.FromMilliseconds(mseconds),
                BeginTime = TimeSpan.FromMilliseconds(mlateSeconds),
                FillBehavior = FillBehavior.Stop,

            };
            //angleAnimation.Completed += completed;

            // 执行动画
            scale.BeginAnimation(ScaleTransform.ScaleXProperty, scaleAnimation);
            scale.BeginAnimation(ScaleTransform.ScaleYProperty, scaleAnimation);
            //angle.BeginAnimation(RotateTransform.AngleProperty, angleAnimation);
        }
    }
}
