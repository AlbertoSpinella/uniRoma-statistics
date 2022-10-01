namespace CS_Example01
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (this.richTextBox1.BackColor == Color.Orange) {
                this.richTextBox1.BackColor = Color.Red;
            } else {
                this.richTextBox1.BackColor = Color.Orange;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (this.richTextBox1.ForeColor == Color.Green) {
                this.richTextBox1.ForeColor = Color.Blue;
            } else {
                this.richTextBox1.ForeColor = Color.Green;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.richTextBox1.ForeColor = Color.Black;
            this.richTextBox1.BackColor = Color.White;
        }
    }
}