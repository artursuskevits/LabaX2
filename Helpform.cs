using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProjectLabratornaja
{
    public partial class Helpform : Form
    {
        System.Windows.Forms.Label Lb;
        Button Btn;
        public Helpform()
        {
            this.Width = 500;
            this.Height = 500;

            Lb = new System.Windows.Forms.Label();
            Lb.Height = 300;
            Lb.Width = 300;
            Lb.Location = new Point(100, 0);
            Lb.Text = "See on kasulik info";
            Lb.TextAlign = ContentAlignment.MiddleCenter;

            Btn = new Button();
            Btn.Height = 100;
            Btn.Width = 100;
            Btn.Text = "Sulge";
            Btn.TextAlign = ContentAlignment.MiddleCenter;
            Btn.Location = new Point(200, 350);
            Btn.Click += Btn_Click;

            this.Controls.Add(Lb);
            this.Controls.Add(Btn);

        }

        private void Btn_Click(object? sender, EventArgs e)
        {
            this.Close();
        }
    }
}
