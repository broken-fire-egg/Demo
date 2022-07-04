using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace GameFrame.Scripts
{
    public class EnemyBullet
    {
        
        double width;
        double height;
        public double x;
        public double y;
        string name;
        public Rectangle rec;
        public Storyboard sb;
        public EnemyBullet(string name, string img_path, double width, double height, double x, double y, Canvas myCanvas,double rotation = 0)
        {
            this.width = width;
            this.height = height;
            this.name = name;
            this.x = System.Windows.SystemParameters.PrimaryScreenWidth/2 - x - this.width / 2;
            this.y = System.Windows.SystemParameters.PrimaryScreenHeight/2 - y - this.height / 2;
            BitmapImage img = new BitmapImage(new Uri(img_path));

            rec = new Rectangle();
            rec.Width = this.width;
            rec.Height = this.height;
            rec.RenderTransformOrigin = new System.Windows.Point(0.5,0.5);

            RotateTransform rt = new RotateTransform();
            rec.RenderTransform = rt;
            rec.Fill = new ImageBrush(img);
            rec.Fill.Opacity = 1;

            rec.Name = this.name;


            Storyboard stb = new Storyboard();
            DoubleAnimation da = new DoubleAnimation(0,1, new TimeSpan(0, 0, 1));
            Storyboard.SetTarget(da, rec);
            Storyboard.SetTargetProperty(da, new PropertyPath(Control.OpacityProperty));
            stb.Children.Add(da);

            DoubleAnimation da2 = new DoubleAnimation(0, 360 + rotation + 10, new TimeSpan(0, 0, 1));
            DoubleAnimation da3 = new DoubleAnimation(360 + rotation + 10, 360 + rotation, new TimeSpan(500));
            da3.BeginTime = new TimeSpan(0, 0, 1);

            myCanvas.Children.Add(rec);
            Canvas.SetLeft(rec, this.x);
            Canvas.SetTop(rec, this.y);

            rec.RenderTransform.BeginAnimation(RotateTransform.AngleProperty, da2, HandoffBehavior.Compose);
            rec.RenderTransform.BeginAnimation(RotateTransform.AngleProperty, da3, HandoffBehavior.Compose);
            stb.Begin();


            PointAnimation myPointAnimation = new PointAnimation();
            myPointAnimation.From = new Point(this.x, this.y);
            myPointAnimation.To = new Point(0, 0);
            Storyboard.SetTarget(rec,myPointAnimation);
            Storyboard.SetTargetProperty(
                myPointAnimation, new PropertyPath(EllipseGeometry.CenterProperty));
            sb = new Storyboard();
            sb.Children.Add(myPointAnimation);
        }
    }
}
