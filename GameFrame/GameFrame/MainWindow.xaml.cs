using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
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
using UnityController;
using GameFrame.Scripts;
using System.Windows.Media.Animation;
using System.Runtime.InteropServices;
using System.Windows.Interop;
using System.Diagnostics;

namespace GameFrame
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {

        private const int GWL_EXSTYLE = -20;

        private const int WS_EX_NOACTIVATE = 0x08000000;


        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
        [DllImport("user32.dll")]
        static extern bool SetFocus(IntPtr hWnd);

        private TimeSpan timeTillNextFrame = TimeSpan.Zero;
        private TimeSpan TimePerFrame = TimeSpan.FromSeconds(0.1f);
        private TimeSpan FireTime = new TimeSpan(0,0,20);

        int i = 0;
        protected DateTime m_datetimeLastTick = DateTime.Now;
        private DateTime m_datetimeNow;
        private TimeSpan m_timespanElapsed = TimeSpan.Zero;
        private TimeSpan m_timespanElapsed2 = TimeSpan.Zero;
        const double degtorad = 0.0174533;
        bool notfired = true;
        List<EnemyBullet> ebs;
        public static void TransformToPixels(double unitX,double unitY,out int pixelX,out int pixelY)
        {
            using (Graphics g = Graphics.FromHwnd(IntPtr.Zero))
            {
                System.Diagnostics.Debug.WriteLine(g.DpiX);
                System.Diagnostics.Debug.WriteLine(g.DpiY);
                pixelX = (int)((g.DpiX / 96) * unitX);
                pixelY = (int)((g.DpiY / 96) * unitY);
            }
        }
        public MainWindow()
        {
            InitializeComponent();
            ebs = new List<EnemyBullet>();
            CompositionTarget.Rendering += this.OnUpdate;
            Process[] p = Process.GetProcessesByName("UntitledGame");
            foreach (var unity in p)
            {
                if (unity.ProcessName == "UntitledGame")
                {
                    ShowWindow(unity.MainWindowHandle, 5);
                    SetFocus(unity.MainWindowHandle);
                }
            }
        }
        private void OnUpdate(object sender, object e)
        {
            //지금 시간 기록
            m_datetimeNow = DateTime.Now;
            //경과시간 = 지금시간 - 마지막 시간
            m_timespanElapsed += m_datetimeNow - m_datetimeLastTick;
            m_timespanElapsed2 += m_timespanElapsed;
            //마지막 시간 기록
            m_datetimeLastTick = m_datetimeNow;

            if(m_timespanElapsed >= TimePerFrame)
            {
                if (i < 36)
                {
                    EnemyBullet center = new EnemyBullet("rec"+i.ToString(),"pack://application:,,,/Resource/ice_spear.png", 1004 / 4, 168 / 4, Math.Cos(i * 10 * degtorad) * 550, Math.Sin(i * 10 * degtorad) * 550, myCanvas, 10 * i);
                    ebs.Add(center);
                    //PointAnimation pa0 = new PointAnimation(new System.Windows.Point(0, 0), new TimeSpan(0, 0, 1));
                    //Storyboard.SetTarget(pa0, center.rec);
                    //Storyboard.SetTargetProperty(pa0, new PropertyPath(EllipseGeometry.CenterProperty));




                    
                    i++;
                }
                m_timespanElapsed = TimeSpan.Zero;
            }
            
            if(m_timespanElapsed2 >= FireTime && notfired)
            {
                for (int k = 0; k < ebs.Count; k++)
                {
                    System.Diagnostics.Debug.WriteLine("FIRE!");



                    //DoubleAnimation da11 = new DoubleAnimation(SystemParameters.PrimaryScreenHeight / 2, new TimeSpan(0, 0, 1));


                    DoubleAnimation da0 = new DoubleAnimation(SystemParameters.PrimaryScreenWidth / 2  -115, new TimeSpan(0, 0, 1));
                    DoubleAnimation da1 = new DoubleAnimation(SystemParameters.PrimaryScreenHeight / 2 -15, new TimeSpan(0, 0, 1));

                    ebs[k].rec.BeginAnimation(Canvas.LeftProperty, da0, HandoffBehavior.Compose);
                    ebs[k].rec.BeginAnimation(Canvas.TopProperty, da1, HandoffBehavior.Compose);

                    //ebs[k].sb.Begin();



                    //Storyboard stb2 = new Storyboard();
                    //PointAnimation pa1 = new PointAnimation(new System.Windows.Point(0, 0), new TimeSpan(0, 0, 1));
                    //pa1.BeginTime = new TimeSpan(0, 0, 2);
                    //Storyboard.SetTarget(pa1, recs[k]);
                    //Storyboard.SetTargetProperty(pa1, new PropertyPath(EllipseGeometry.CenterProperty));
                    //stb2.Begin();
                }


                notfired = false;
            }


        }

    }
}
