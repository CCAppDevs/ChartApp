using CsvHelper;
using System.Globalization;

namespace ChartApp
{
    public partial class ChartApp : Form
    {
        private string _filePath = "";
        private string _rawFile = "";
        private List<WeatherEvent> _events = new List<WeatherEvent>();

        public ChartApp()
        {
            InitializeComponent();
        }

        private void LoadData()
        {
            lblDisplay.Text = "";

            for (int i = 0; i < _events.Count; i++)
            {
                lblDisplay.Text += _events[i].ToString() + "\n";
            }
        }

        private void DrawGraph()
        {
            // horizontal - ID - 1 to N
            // vertical - Temp - 50 to 150
            // TODO: add dynamic ranges to the app
            SolidBrush blackBrush = new SolidBrush(Color.Black);
            Pen blackPen = new Pen(blackBrush);
            Graphics g = this.CreateGraphics();

            int startX = 150;
            int startY = 10;
            int sizeX = 300;
            int sizeY = 300;

            Point topLeft = new Point(startX, startY);
            Point topRight = new Point(startX + sizeX, startY);
            Point bottomRight = new Point(startX + sizeX, startY + sizeY);
            Point bottomLeft = new Point(startX, startY + sizeY);


            // Draw Graph Boundries
            g.DrawLine(blackPen, topLeft, bottomLeft);
            g.DrawLine(blackPen, bottomLeft, bottomRight);


            // Draw x hashes
            int xStep = 10;



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
        }
    }
}
