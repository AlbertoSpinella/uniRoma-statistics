namespace CS_Random_And_Timer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public Random randomNumber = new Random();
        public int occurencies1 = 0;
        public int occurencies2 = 0;
        public int occurencies3 = 0;
        public int occurencies4 = 0;
        public int occurencies5 = 0;
        public int occurencies6 = 0;
        public int totalOccurencies = 0;

        private void button1_Click(object sender, EventArgs e)
        {
            this.timer1.Interval = 1000;
            this.timer1.Start();
            this.button2.Visible = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.timer1.Stop();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.timer1.Interval = 10;
            this.timer1.Start();
            this.button1.Visible = false;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (this.timer1.Interval == 10 && totalOccurencies > 999)
            {
                this.timer1.Stop();
            } else
            {
                int RandomInRange = randomNumber.Next(1, 7);
                this.richTextBox2.AppendText(RandomInRange + Environment.NewLine);

                totalOccurencies += 1;
                this.richTextBox9.Text = totalOccurencies.ToString();

                switch (RandomInRange)
                {
                    case 1:
                        occurencies1 += 1;
                        this.richTextBox3.Text = occurencies1.ToString();
                        break;
                    case 2:
                        occurencies2 += 1;
                        this.richTextBox4.Text = occurencies2.ToString();
                        break;
                    case 3:
                        occurencies3 += 1;
                        this.richTextBox5.Text = occurencies3.ToString();
                        break;
                    case 4:
                        occurencies4 += 1;
                        this.richTextBox6.Text = occurencies4.ToString();
                        break;
                    case 5:
                        occurencies5 += 1;
                        this.richTextBox7.Text = occurencies5.ToString();
                        break;
                    case 6:
                        occurencies6 += 1;
                        this.richTextBox8.Text = occurencies6.ToString();
                        break;
                    default:
                        this.timer1.Stop();
                        this.richTextBox1.Text = "Something went wrong!";
                        break;
                }
            }
        }
    }
}