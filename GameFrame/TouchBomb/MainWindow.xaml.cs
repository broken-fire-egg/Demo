using System;
using System.Collections.Generic;
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
using System.IO;
using System.IO.Pipes;
using System.Threading;
using System.Diagnostics;
using System.Windows.Threading;

namespace TouchBomb
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        NamedPipeClientStream namedPipeClientStream;
        StreamString streamString;
        private bool connectionStatus = false;
        private DateTime m_datetimeNow;
        private DateTime m_datetimeLastTick = DateTime.Now;
        private TimeSpan TimePerFrame = TimeSpan.FromSeconds(0.075f);
        private TimeSpan FireTime = new TimeSpan(0, 0, 6);
        private TimeSpan m_timespanElapsed = TimeSpan.Zero;
        private TimeSpan m_timespanElapsed2 = TimeSpan.Zero;
        Random random = new Random();
        TimeSpan[] seconds;
        BitmapImage[] bgs;
        BitmapImage[] hitbgs;
        BitmapImage[] numbers;
        int currentnum = 0;
        TimeSpan scaleup = TimeSpan.Zero;
        TimeSpan scaledown = TimeSpan.Zero;
        bool hit = false;
        int hp = 5;
        float pow = 0;
        public MainWindow()
        {
            InitializeComponent();
            bgs = new BitmapImage[5];
            hitbgs = new BitmapImage[5];
            numbers = new BitmapImage[6];
            seconds = new TimeSpan[6];
            for (int i=0; i < 5; i++)
            {
                hitbgs[i] = new BitmapImage(new Uri("/Resource/enemy_boss_red_jujak_eft_bomb_w_" + i.ToString() + ".png", UriKind.Relative));
                seconds[i] = new TimeSpan(0, 0, i);
                bgs[i] = new BitmapImage(new Uri("/Resource/enemy_boss_red_jujak_eft_bomb_"+i.ToString()+".png", UriKind.Relative));
                numbers[i] = new BitmapImage(new Uri("/Resource/enemy_boss_red_jujak_eft_boom_nb_"+i.ToString()+".png", UriKind.Relative));
            }
            seconds[5] = new TimeSpan(0, 0, 5);
            // bg.Source = new BitmapImage(new Uri(@"\Resource\enemy_boss_red_jujak_eft_bomb_0.png",UriKind.Relative));
            Left = random.NextDouble() * (SystemParameters.WorkArea.Width - 512);
            Top = random.NextDouble() * (SystemParameters.WorkArea.Height - 512);


            CompositionTarget.Rendering += this.OnUpdate;
            Deactivated += this.Window_Deactivated;
            Init();

        }
        private void TouchEvent(object sender, MouseEventArgs e)
        {
            Trace.WriteLine("touched");
            hp--;
            if (hp > 0)
            {
                bg_img.Opacity = (double)hp / 5;
                num_img.Opacity = (double)hp / 5;
                hit = true;
            }
            if(hp == 0)
            {
                if (connectionStatus)
                {
                    streamString.WriteString("Bomb Defused!");
                    namedPipeClientStream.Close();
                }
                Environment.Exit(0);
            }
        }
        public void Init()
        {
            Thread clientWriteTread = new Thread(ClientThread_Write);
            clientWriteTread.Start();
        }

        void ClientThread_Write()
        {
            namedPipeClientStream = new NamedPipeClientStream(".", "BombPipe", PipeDirection.Out);
            namedPipeClientStream.Connect();
            streamString = new StreamString(namedPipeClientStream);

            string posmsg = " ";

            double xpos, ypos;

            Dispatcher.Invoke(new Action(delegate { 
                xpos = (this.Left + this.Left + this.Width) / 2;
                ypos = (this.Top + this.Top + this.Height) / 2;
                posmsg += xpos.ToString() + "," + ypos.ToString();
                streamString.WriteString("Bomb Planted!" + posmsg);
            }), DispatcherPriority.Normal);
            
            


            connectionStatus = true;
        }
        private void Window_Deactivated(object sender, EventArgs e)
        {
            Window window = (Window)sender;
            window.Topmost = true;
        }
        public void OnUpdate(object sender, object e)
        {
            if (hp <= 0)
                return;

            m_datetimeNow = DateTime.Now;
            m_timespanElapsed += m_datetimeNow - m_datetimeLastTick;
            m_timespanElapsed2 += m_datetimeNow - m_datetimeLastTick;
            m_datetimeLastTick = m_datetimeNow;



            if (m_timespanElapsed2 >= TimePerFrame)
            {
                if(!hit)
                    bg_img.Source = bgs[currentnum];
                else
                {
                    bg_img.Source = hitbgs[currentnum];
                    hit = false;
                }

                currentnum = (currentnum + 1) % 5;
                num_img.Margin = new Thickness((random.NextDouble() - 0.5) * 20 * pow, (random.NextDouble() - 0.5) * 20 * pow, 0, 0);

                m_timespanElapsed2 = TimeSpan.Zero;
            }
            else if (m_timespanElapsed >= seconds[5])
            {
                num_img.Source = numbers[0];
                pow = 2.5f;
            }
            else if (m_timespanElapsed >= seconds[4])
            {
                num_img.Source = numbers[1];
                pow = 2f;
            }
            else if (m_timespanElapsed >= seconds[3])
            {
                num_img.Source = numbers[2];
                pow = 1.5f;
            }
            else if (m_timespanElapsed >= seconds[2])
            {
                num_img.Source = numbers[3];
                pow = 1f;
            }
            else if (m_timespanElapsed >= seconds[1])
            {
                num_img.Source = numbers[4];
                pow = 0.5f;
            }

            if (m_timespanElapsed >= FireTime)
            {
                if (connectionStatus)
                {
                    streamString.WriteString("Bomb Exploded!");
                    namedPipeClientStream.Close();
                }
                Environment.Exit(0);
            }

        }
        public class StreamString
        {
            private Stream ioStream;
            private UnicodeEncoding streamEncoding;
            public StreamString(Stream ioStream)
            {
                this.ioStream = ioStream;
                streamEncoding = new UnicodeEncoding();
            }
            public string ReadString()
            {
                int len = 0;

                len = ioStream.ReadByte() * 256;
                len += ioStream.ReadByte();
                byte[] inBuffer = new byte[len];
                ioStream.Read(inBuffer, 0, len);
                return streamEncoding.GetString(inBuffer);
            }
            public int WriteString(string outString)
            {
                byte[] outBuffer = streamEncoding.GetBytes(outString);
                int len = outBuffer.Length;
                if (len > UInt16.MaxValue)
                {
                    len = (int)UInt16.MaxValue;
                }
                ioStream.WriteByte((byte)(len / 256));
                ioStream.WriteByte((byte)(len & 255));
                ioStream.Write(outBuffer, 0, len);
                ioStream.Flush();

                return outBuffer.Length + 2;
            }
        }

        private void bg_img_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }
    }
}
