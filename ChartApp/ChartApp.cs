using CsvHelper;
using System.Globalization;

namespace ChartApp
{
    public partial class ChartApp : Form
    {
        private string _filePath = "";
        private string _rawFile = "";
        private List<WeatherEvent> _events = new List<WeatherEvent>();

        // graphics objects
        private SolidBrush blackBrush;
        private Pen blackPen;
        private Graphics g;

        // graph positioning values
        private int startX = 150;
        private int startY = 10;
        private int sizeX = 300;
        private int sizeY = 300;

        private Point topLeft;
        private Point topRight;
        private Point bottomRight;
        private Point bottomLeft;

        public ChartApp()
        {
            InitializeComponent();
            blackBrush = new SolidBrush(Color.Black);
            blackPen = new Pen(blackBrush);
            g = this.CreateGraphics();

            topLeft = new Point(startX, startY);
            topRight = new Point(startX + sizeX, startY);
            bottomRight = new Point(startX + sizeX, startY + sizeY);
            bottomLeft = new Point(startX, startY + sizeY);
        }

        private void LoadData()
        {
            lblDisplay.Text = "";

            for (int i = 0; i < _events.Count; i++)
            {
                lblDisplay.Text += _events[i].ToString() + "\n";
            }
        }

        private void DrawData()
        {
            // for each weather event in our list of weather events
                // Draw a circle at the day/temp

            foreach (WeatherEvent evt in _events)
            {
                //MessageBox.Show("X: " + evt.id + ", Y: " + evt.temp);

                int x = evt.id;
                int y = evt.temp;

                float posXStart = startX + (5 * x);
                float width = 5;
                float posYStart = startY + (5 * y);
                float height = 5;

                g.DrawEllipse(blackPen, posXStart, posYStart, width, height);
            }
        }

        private void DrawGraph()
        {
            // Draw Graph Boundries
            g.DrawLine(blackPen, topLeft, bottomLeft);
            g.DrawLine(blackPen, bottomLeft, bottomRight);


            // Draw x hashes
            int numHashesX = 14;
            int xStep = sizeX / numHashesX;

            // draw vertical hash marks
            for (int i = 0; i < numHashesX; i++)
            {
                Point pt1 = new Point(startX + (xStep * (i + 1)), startY + sizeY + 10);
                Point pt2 = new Point(startX + (xStep * (i + 1)), startY + sizeY);

                Point textPt = new Point(startX + (xStep * (i + 1)), startY + sizeY + 40);

                string label = (i + 1).ToString();

                g.DrawLine(blackPen, pt1, pt2);
                g.DrawString(label, new Font(FontFamily.GenericMonospace, 8.0f), blackBrush, textPt);
            }



            // Draw y hashes
            int numHashesY = 10;
            int yStep = (sizeY - startY) / numHashesY;

            int tempStep = (150 - 50) / numHashesY;

            // draw vertical hash marks
            for (int i = 0; i < numHashesY; i++)
            {
                Point pt1 = new Point(startX - 10, startY + (yStep * i));
                Point pt2 = new Point(startX, startY + (yStep * i));

                Point textPt = new Point(startX - 40, startY + (yStep * i));

                string label = (150 - (tempStep * i)).ToString();

                g.DrawLine(blackPen, pt1, pt2);
                g.DrawString(label, new Font(FontFamily.GenericMonospace, 10.0f), blackBrush, textPt);
            }
        }

        private void btnOpenFile_Click(object sender, EventArgs e)
        {
            // open a file dialog
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                // set the properties on the fileDialog
                openFileDialog.InitialDirectory = Path.Combine(Environment.CurrentDirectory, "Data");
                openFileDialog.Filter = "csv files (*.csv)|*.csv|All files (*.*)|*.*";

                // open the file dialog and do something
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    // process the file information
                    _filePath = openFileDialog.FileName;

                    var fileStream = openFileDialog.OpenFile();

                    using (StreamReader reader = new StreamReader(fileStream))
                    using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                    {
                        _events = csv.GetRecords<WeatherEvent>().ToList();
                        LoadData();
                    }
                }
            }
        }

        private void ChartApp_Paint(object sender, PaintEventArgs e)
        {
            DrawGraph();
            DrawData();
        }
    }
}
