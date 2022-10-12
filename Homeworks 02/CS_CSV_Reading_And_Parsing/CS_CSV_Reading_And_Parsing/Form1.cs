using Microsoft.VisualBasic.FileIO;

namespace CS_CSV_Reading_And_Parsing
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        IDictionary<string, int> regions = new Dictionary<string, int>();
        bool isHeader = true;
        public double currentArithmeticMean = 0;
        public int count = 0;

        private void button1_Click(object sender, EventArgs e)
        {

            StreamReader sr = new StreamReader("regions.csv");
            do
            {
                string line = sr.ReadLine();
                if (isHeader)
                {
                    isHeader = false;
                } else
                {
                    string[] row = line.Split(",");




                    regions.Add(row[0], int.Parse(row[1]));
                }
            }
            while (!sr.EndOfStream);

            sr.Dispose();

            this.richTextBox1.AppendText("REGION".PadRight(25)
                + "INHABITANTS".PadRight(25)
                + "MEAN" + Environment.NewLine + Environment.NewLine);

            foreach (var region in regions)
            {
                count += 1;


                currentArithmeticMean += (region.Value - currentArithmeticMean) / count;

                this.richTextBox1.AppendText(region.Key.PadRight(25) 
                    + region.Value.ToString().PadRight(25)
                    + Math.Round(currentArithmeticMean, 2) + Environment.NewLine);
            }

        }
    }
}