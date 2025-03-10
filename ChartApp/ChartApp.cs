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

            DrawGraph();
        }

        private void DrawGraph()
        {
            // horizontal - ID - 1 to N
            // vertical - Temp - 50 to 150
            // TODO: add dynamic ranges to the app
            SolidBrush blackBrush = new SolidBrush(Color.Black);
            Pen blackPen = new Pen(blackBrush);
            Graphics g = this.CreateGraphics();

            int startX = 100;
            int startY = 10;
            int sizeX = 300;
            int sizeY = 300;

            Point topLeft = new Point(startX, startY);
            Point bottomRight = new Point(startX + sizeX, startY + sizeY);


            g.DrawLine(blackPen, topLeft, bottomRight);
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
    }
}
