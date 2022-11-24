namespace Homework_8
{
    public partial class Form1 : Form
    {

        private List<PointF> points;
        private PointF[] pointsArray;
        private int numberOfPoints = 100;
        private int start, end;
        private Random r;

        private Graphics grap;
        private Bitmap b;
        private Point maxValue, minValue;
        private Rectangle leftPlotWindow;
        private Rectangle rightPlotWindow;

        private Pen topHistoPen = new Pen(Color.Red, 10);
        private Pen bottomHistoPen = new Pen(Color.Blue, 10);


        public Form1()
        {
            InitializeComponent();
            r = new Random();
            b = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            grap = Graphics.FromImage(b);
            grap.Clear(Color.White);
            pictureBox1.Image = b;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            grap.Clear(Color.White);
            pictureBox1.Image = b;
            points = new List<PointF>();
            leftPlotWindow = new Rectangle(10, 10, pictureBox1.Width/2 - 20, pictureBox1.Height/2 - 20);
            rightPlotWindow  = new Rectangle(leftPlotWindow.X + leftPlotWindow.Width + 10, 10, pictureBox1.Width - leftPlotWindow.Width - 20, pictureBox1.Height/2 - 20);


            for (int i = 0; i < numberOfPoints; i++)
            {
                var radius = (float)r.NextDouble();
                var angle = r.Next(360);
                var point = new PointF(radius * (float)Math.Cos(angle), radius * (float)Math.Sin(angle));
                points.Add(point); 
            }
            pointsArray = points.ToArray();
            start = 0;
            end = 2;
            minValue = new Point(0, 0);
            maxValue = new Point(numberOfPoints, 1);
            timer1.Interval = 500;
            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (end < pointsArray.Length)
            {
                for (int i = start; i < end; i++)
                {
                    var pointTA = fromRealToVirtual(new PointF(i, 0), minValue, maxValue, leftPlotWindow);
                    pointTA.X += 5;
                    var pointTB = fromRealToVirtual(new PointF(i, pointsArray[i].X), minValue, maxValue, leftPlotWindow);
                    pointTB.X += 5;

                    var pointBA = fromRealToVirtual(new PointF(i, 0), minValue, maxValue, rightPlotWindow);
                    pointBA.X += 5;

                    var pointBB = fromRealToVirtual(new PointF(i, pointsArray[i].Y), minValue, maxValue, rightPlotWindow);
                    pointBB.X += 5;

                    grap.DrawLine(topHistoPen, pointTA, pointTB);
                    grap.DrawLine(bottomHistoPen, pointBA, pointBB);
                }
                pictureBox1.Image = b;
                start += 2;
                end += 2;
            }
             else
            {
                timer1.Stop();
            }
        }

        private PointF fromRealToVirtual(PointF point, Point minValue, Point maxValue, Rectangle rect)
        {
            float newX = maxValue.X - minValue.X == 0 ? 0 : (rect.Left + rect.Width * (point.X - minValue.X) / (maxValue.X - minValue.X));
            float newY = maxValue.Y - minValue.Y == 0 ? 0 : (rect.Top + rect.Height - rect.Height * (point.Y - minValue.Y) / (maxValue.Y - minValue.Y));
            return new PointF(newX, newY);
        }
    }
}