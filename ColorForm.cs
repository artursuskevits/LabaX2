using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProjectLabratornaja
{
    public partial class ColorForm : Form
    {
        Color Colorresult;
        PictureBox Pb;
        Button OkBtn, CancelBtn, OtherBtn;
        Label Greenlbl, Redlbl, Bluelbl;
        HScrollBar Greenhsb, Redhsb, Bluehsb;
        NumericUpDown GreenNum, RedNum, BlueNum;
        public ColorForm(Color color)
        {
            //GREEN

            this.Width = 400;
            this.Height = 250;
            this.Text = "Color";

            Greenlbl = new Label();
            Greenlbl.Width = 40;
            Greenlbl.Height = 30;
            Greenlbl.Text = "Green";
            Greenlbl.Location = new Point(10, 30);

            this.Controls.Add(Greenlbl);


            Greenhsb = new HScrollBar();
            Greenhsb.Minimum = 0;
            Greenhsb.Maximum = 255;
            Greenhsb.LargeChange = 1;
            Greenhsb.Location = new Point(Greenlbl.Location.X + Greenlbl.Width + 10, 30);
            this.Controls.Add(Greenhsb);
            Greenhsb.ValueChanged += Greenhsb_ValueChanged;



            GreenNum = new NumericUpDown();
            GreenNum.Width = 40;
            GreenNum.Height = 30;
            GreenNum.Minimum = 0;
            GreenNum.Maximum = 255;
            GreenNum.Increment = 1;
            GreenNum.Location = new Point(Greenhsb.Location.X + 20 + Greenhsb.Width + 10, 30);
            this.Controls.Add(GreenNum);
            GreenNum.ValueChanged += GreenNum_ValueChanged;

            //RED
            Redlbl = new Label();
            Redlbl.Width = 40;
            Redlbl.Height = 30;
            Redlbl.Text = "Red";
            Redlbl.Location = new Point(10, Greenlbl.Location.Y + Greenlbl.Height + 10);

            this.Controls.Add(Redlbl);


            Redhsb = new HScrollBar();
            Redhsb.Minimum = 0;
            Redhsb.Maximum = 255;
            Redhsb.LargeChange = 1;
            Redhsb.Location = new Point(Redlbl.Location.X + Redlbl.Width + 10, Greenlbl.Location.Y + Greenlbl.Height + 10);
            this.Controls.Add(Redhsb);
            Redhsb.ValueChanged += Redhsb_ValueChanged;



            RedNum = new NumericUpDown();
            RedNum.Width = 40;
            RedNum.Height = 30;
            RedNum.Minimum = 0;
            RedNum.Maximum = 255;
            RedNum.Increment = 1;
            RedNum.Location = new Point(Redhsb.Location.X + 20 + Redhsb.Width + 10, Greenlbl.Location.Y + Greenlbl.Height + 10);
            this.Controls.Add(RedNum);
            RedNum.ValueChanged += RedNum_ValueChanged;

            //BLUE
            Bluelbl = new Label();
            Bluelbl.Width = 40;
            Bluelbl.Height = 30;
            Bluelbl.Text = "Blue";
            Bluelbl.Location = new Point(10, Redlbl.Location.Y + Redlbl.Height + 10);

            this.Controls.Add(Bluelbl);


            Bluehsb = new HScrollBar();
            Bluehsb.Minimum = 0;
            Bluehsb.Maximum = 255;
            Bluehsb.LargeChange = 1;
            Bluehsb.Location = new Point(Bluelbl.Location.X + Greenlbl.Width + 10, Redlbl.Location.Y + Redlbl.Height + 10);
            this.Controls.Add(Bluehsb);
            Bluehsb.ValueChanged += Bluehsb_ValueChanged;



            BlueNum = new NumericUpDown();
            BlueNum.Width = 40;
            BlueNum.Height = 30;
            BlueNum.Minimum = 0;
            BlueNum.Maximum = 255;
            BlueNum.Increment = 1;
            BlueNum.Location = new Point(Bluehsb.Location.X + 20 + Greenhsb.Width + 10, Redlbl.Location.Y + Redlbl.Height + 10);
            this.Controls.Add(BlueNum);
            BlueNum.ValueChanged += BlueNum_ValueChanged;

            Pb = new PictureBox();
            Pb.Width = 100;
            Pb.Height = 100;
            Pb.Location = new Point(GreenNum.Location.X + 20 + GreenNum.Width, 30);
            Pb.BackColor = Color.Black;
            this.Controls.Add(Pb);


            //Buttons

            OkBtn = new Button();
            OkBtn.Width = 100;
            OkBtn.Height = 30;
            OkBtn.Text = "Ok";
            OkBtn.Location = new Point(10, Bluelbl.Location.Y + Bluelbl.Height + 30);
            this.Controls.Add(OkBtn);

            CancelBtn = new Button();
            CancelBtn.Width = 100;
            CancelBtn.Height = 30;
            CancelBtn.Text = "Cancel";
            CancelBtn.Location = new Point(10 + OkBtn.Width, Bluelbl.Location.Y + Bluelbl.Height + 30);
            this.Controls.Add(CancelBtn);

            OtherBtn = new Button();
            OtherBtn.Width = 100;
            OtherBtn.Height = 30;
            OtherBtn.Text = "Other Colors";
            OtherBtn.Location = new Point(Pb.Location.X, Bluelbl.Location.Y + Bluelbl.Height + 30);
            this.Controls.Add(OtherBtn);
            OtherBtn.Click += OtherBtn_Click;

            Redhsb.Tag = RedNum;
            Greenhsb.Tag = GreenNum;
            Bluehsb.Tag = BlueNum;

            RedNum.Tag = Redhsb;
            GreenNum.Tag = Greenhsb;
            BlueNum.Tag = Bluehsb;

            RedNum.Value = color.R;
            GreenNum.Value = color.G;
            BlueNum.Value = color.B;
        }

        private void OtherBtn_Click(object? sender, EventArgs e)
        {
            ColorDialog cDia = new ColorDialog();
            cDia.ShowDialog();
        }

        private void RedNum_ValueChanged(object? sender, EventArgs e)
        {
            NumericUpDown numericupdown = (NumericUpDown)sender;
            ScrollBar scrollbar = (ScrollBar)numericupdown.Tag;
            scrollbar.Value = (int)numericupdown.Value;
            UpadteColor();
        }

        private void Redhsb_ValueChanged(object? sender, EventArgs e)
        {
            ScrollBar scrollbar = (ScrollBar)sender;
            NumericUpDown numericupdown = (NumericUpDown)scrollbar.Tag;
            numericupdown.Value = scrollbar.Value;
            UpadteColor();
        }

        private void BlueNum_ValueChanged(object? sender, EventArgs e)
        {
            NumericUpDown numericupdown = (NumericUpDown)sender;
            ScrollBar scrollbar = (ScrollBar)numericupdown.Tag;
            scrollbar.Value = (int)numericupdown.Value;
            UpadteColor();
        }

        private void Bluehsb_ValueChanged(object? sender, EventArgs e)
        {
            ScrollBar scrollbar = (ScrollBar)sender;
            NumericUpDown numericupdown = (NumericUpDown)scrollbar.Tag;
            numericupdown.Value = scrollbar.Value;
            UpadteColor();
        }

        private void GreenNum_ValueChanged(object? sender, EventArgs e)
        {
            NumericUpDown numericupdown = (NumericUpDown)sender;
            ScrollBar scrollbar = (ScrollBar)numericupdown.Tag;
            scrollbar.Value = (int)numericupdown.Value;
            UpadteColor();
        }

        private void Greenhsb_ValueChanged(object? sender, EventArgs e)
        {
            ScrollBar scrollbar = (ScrollBar)sender;
            NumericUpDown numericupdown = (NumericUpDown)scrollbar.Tag;
            numericupdown.Value = scrollbar.Value;
            UpadteColor();
        }

        private void UpadteColor()
        {
            Colorresult = Color.FromArgb(Redhsb.Value, Greenhsb.Value, Bluehsb.Value);
            Pb.BackColor = Colorresult;
        }
    }
}
