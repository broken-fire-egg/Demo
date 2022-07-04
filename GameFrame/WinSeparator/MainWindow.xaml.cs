using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Interop;

using System.IO;


namespace WinSeparator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        class WindowPiece
        {
           



            public Polygon polygon;
            public double xspeed;
            public double yspeed;
            double xoffset = 0;
            double yoffset = 0;

            public void MovePiece()
            {
                List<System.Windows.Point> points = new List<System.Windows.Point>();


                xoffset += xspeed;
                yoffset += yspeed;

                Trace.WriteLine(xoffset.ToString());
                foreach (var po in polygon.Points)
                {
                    points.Add(new System.Windows.Point(po.X + xspeed, po.Y + yspeed));
                }
                polygon.Points.Clear();
                foreach (var po in points)
                {
                    polygon.Points.Add(po);
                }

            }
            public WindowPiece(Polygon _pol)
            {
                polygon = _pol;
            }
        }
        List<WindowPiece> windowPieces;

        private TimeSpan CloseTime = new TimeSpan(0, 0, 2);
        public Random random;
        private DateTime m_datetimeNow;
        private TimeSpan m_timespanElapsed = TimeSpan.Zero;
        private DateTime m_datetimeLastTick = DateTime.Now;

        public MainWindow()
        {
            InitializeComponent();
            Init();
            CompositionTarget.Rendering += this.OnUpdate;
            //Deactivated += this.Window_Deactivated;
        }

        private BitmapSource CopyScreen()
        {
            using (var screenBmp = new Bitmap(
                960,
                540,
                System.Drawing.Imaging.PixelFormat.Format32bppArgb))
            {
                using (var bmpGraphics = Graphics.FromImage(screenBmp))
                {
                    bmpGraphics.CopyFromScreen((int)SystemParameters.WorkArea.Width / 2 - 480 , (int)SystemParameters.WorkArea.Height / 2  - 270, 0, 0, screenBmp.Size);
                    return Imaging.CreateBitmapSourceFromHBitmap(
                        screenBmp.GetHbitmap(),
                        IntPtr.Zero,
                        Int32Rect.Empty,
                        BitmapSizeOptions.FromEmptyOptions());
                }
            }
        }


        public void Init()
        {
            grid.ClipToBounds = true;
            // polygons = new Polygon[12];
            Trace.WriteLine(SystemParameters.WorkArea.Width);
            var image =  CopyScreen();
            
            random = new Random();
            windowPieces = new List<WindowPiece>();
            
            //grid.ClipToBounds = true;
            foreach (Polygon pol in grid.Children)
            {
                double highestX = -100000, highestY = -100000;
                double lowestX = 100000, lowestY = 100000;
                foreach (var p in pol.Points)
                {
                    if (p.X > highestX)
                        highestX = p.X;
                    if (p.Y > highestY)
                        highestY = p.Y;
                    if (p.X < lowestX)
                        lowestX = p.X;
                    if (p.Y < lowestY)
                        lowestY = p.Y;
                }
                CroppedBitmap cb = new CroppedBitmap(image, new Int32Rect((int)lowestX, (int)lowestY, (int)highestX - (int)lowestX, (int)highestY - (int)lowestY));
                
                var ib = new ImageBrush(cb);
                ib.Stretch = Stretch.None;
                pol.Fill = ib;

                windowPieces.Add(new WindowPiece(pol));
                List<System.Windows.Point> points = new List<System.Windows.Point>();
                foreach (var po in pol.Points)
                {
                    points.Add(new System.Windows.Point(po.X + SystemParameters.WorkArea.Width / 2 - 480, po.Y + SystemParameters.WorkArea.Height / 2 - 270));
                }
                pol.Points.Clear();
                for (int i = 0; i < points.Count; i++)
                {
                    pol.Points.Add(points[i]);
                }
            }


            int plusspeed = 0;

            foreach (var item in windowPieces)
            {
                item.xspeed = random.NextDouble() * ((random.Next(1) == 0) ? -1 : 1);
                item.yspeed = random.NextDouble() + 8 + plusspeed++;

            }

        }

        private void Window_Deactivated(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void OnUpdate(object sender, EventArgs e)
        {
            m_datetimeNow = DateTime.Now;
            m_timespanElapsed += m_datetimeNow - m_datetimeLastTick;
            m_datetimeLastTick = m_datetimeNow;
            if(m_timespanElapsed >= CloseTime)
            {
                Environment.Exit(0);
            }

            foreach (var item in windowPieces)
            {
                item.MovePiece();
            }
        }
    }
}
