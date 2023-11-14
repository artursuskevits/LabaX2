using Microsoft.VisualBasic.ApplicationServices;
using System.Numerics;
using System;
using System.Reflection.Emit;
using System.Security.Cryptography.Xml;
using System.Windows.Forms;
using static System.Windows.Forms.AxHost;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;

namespace ProjectLabratornaja
{
    public partial class MainForm : Form
    {
        List<Image> SecondHistory = new List<Image>();
        public List<Image> History = new List<Image>();
        int Historycounter;
        Color HistoryColor;
        bool drawning;
        GraphicsPath currentPath;
        Point oldLocation;
        public Pen CurrentPen;
        MenuStrip Ms;
        ToolStrip Ts;
        PictureBox Pb;
        Panel Pl, UnderPbPl;
        System.Windows.Forms.TrackBar Tb;
        System.Windows.Forms.Label Lb;

        public MainForm()
        {
            
            drawning = false;
            CurrentPen = new Pen(Color.Black);

            //Foarm settings
            this.Text = "Bad Paint";
            this.Height = 800;
            this.Width = 1200;

            //MenuStrip settings
            Ms = new MenuStrip();
            Ms.Width = this.Width;
            Ms.Height = 30;
            Ms.BackColor = Color.White;
            Ms.Location = new Point(0, 0);

            //ToolStrip settings
            Ts = new ToolStrip();
            Ts.Width = 150;
            Ts.Height = this.Height;
            Ts.Location = new Point(0, Ms.Height);
            Ts.BackColor = Color.White;
            Ts.Anchor = AnchorStyles.Left;

            //PicturBox Settings
            Pb = new PictureBox();
            Pb.Height = 650;
            Pb.Width = 1000;
            Pb.Location = new Point(Ts.Width, Ms.Height);
            Pb.BackColor = Color.Yellow;
            Pb.MouseMove += Pb_MouseMove;
            Pb.MouseDown += Pb_MouseDown;
            Pb.MouseUp += Pb_MouseUp;

            //Panel Settings
            Pl = new Panel();
            Pl.Height = 50;
            Pl.Width = 1000;
            Pl.Location = new Point(Ts.Width, Ms.Height + Pb.Height + 10);
            Pl.BackColor = Color.LightBlue;
            Pl.Text = "0,0";
            Pl.ForeColor = Color.Firebrick;

            //Label Settings
            Lb = new System.Windows.Forms.Label();
            Lb.Height = 30;
            Lb.Width = 300;
            Lb.Text = "0,0";
            Lb.BackColor = Color.LightBlue;
            Pl.Controls.Add(Lb);

            //TrackBar settings
            Tb = new System.Windows.Forms.TrackBar();
            Tb.Height = 30;
            Tb.Width = 300;
            Tb.BackColor = Color.LightBlue;
            Tb.Location = new Point(Ts.Width + 650, Ms.Height + Pb.Height + 12);
            Tb.Maximum = 20;
            Tb.Minimum = 1;
            Tb.Value = 5;
            CurrentPen.Width = Tb.Value;
            Tb.Scroll += Tb_Scroll;

            //Second Panel Settings
            UnderPbPl = new Panel();
            UnderPbPl.Height = 670;
            UnderPbPl.Width = 1020;
            UnderPbPl.Location = new Point(Ts.Width - 10, Ms.Height - 10);
            UnderPbPl.BackColor = Color.Pink;



            //Menu Strip Fill
            ToolStripMenuItem FileItem = new ToolStripMenuItem("File");
            ToolStripMenuItem NewFileItem = new ToolStripMenuItem("New");
            ToolStripMenuItem OpenFileItem = new ToolStripMenuItem("Open");
            ToolStripMenuItem SaveFileItem = new ToolStripMenuItem("Save");
            ToolStripMenuItem ExitFileItem = new ToolStripMenuItem("Exit");
            FileItem.DropDownItems.Add(NewFileItem);
            FileItem.DropDownItems.Add(OpenFileItem);
            FileItem.DropDownItems.Add(SaveFileItem);
            FileItem.DropDownItems.Add(ExitFileItem);
            Ms.Items.Add(FileItem);
            NewFileItem.Click += NewFileItem_Click;
            SaveFileItem.Click += SaveFileItem_Click;
            OpenFileItem.Click += OpenFileItem_Click;

            ToolStripMenuItem EditItem = new ToolStripMenuItem("Edit");
            ToolStripMenuItem UndoEditItem = new ToolStripMenuItem("Undo");
            ToolStripMenuItem RenoEditItem = new ToolStripMenuItem("Reno");
            ToolStripMenuItem PenEditItem = new ToolStripMenuItem("Pen");
            EditItem.DropDownItems.Add(UndoEditItem);
            EditItem.DropDownItems.Add(RenoEditItem);
            EditItem.DropDownItems.Add(PenEditItem);
            UndoEditItem.Click += UndoEditItem_Click;
            RenoEditItem.Click += RenoEditItem_Click;

            PenEditItem.Checked = true;
            ToolStripMenuItem StylePenItem = new ToolStripMenuItem("Style");
            ToolStripMenuItem ColorPenItem = new ToolStripMenuItem("Color");
            PenEditItem.DropDownItems.Add(ColorPenItem);
            PenEditItem.DropDownItems.Add(StylePenItem);
            StylePenItem.Checked = true;
            ColorPenItem.Click += ColorPenItem_Click;
            ToolStripMenuItem SolidStyleItem = new ToolStripMenuItem("Solid");
            ToolStripMenuItem DotStyleItem = new ToolStripMenuItem("Dot");
            ToolStripMenuItem DashDotDotStyleItem = new ToolStripMenuItem("DashDotDot");
            StylePenItem.DropDownItems.Add(SolidStyleItem);
            StylePenItem.DropDownItems.Add(DotStyleItem);
            StylePenItem.DropDownItems.Add(DashDotDotStyleItem);
            SolidStyleItem.Checked = true;
            Ms.Items.Add(EditItem);
            SolidStyleItem.Click += (sender, e) => SolidStyleItem_Click(sender, e, SolidStyleItem, DotStyleItem, DashDotDotStyleItem);
            DotStyleItem.Click += (sender, e) => DotStyleItem_Click(sender, e, SolidStyleItem, DotStyleItem, DashDotDotStyleItem); ;
            DashDotDotStyleItem.Click += (sender, e) => DashDotDotStyleItem_Click(sender, e, SolidStyleItem, DotStyleItem, DashDotDotStyleItem); ;

            ToolStripMenuItem HelpItem = new ToolStripMenuItem("Help");
            ToolStripMenuItem AboutHelpItem = new ToolStripMenuItem("About");
            HelpItem.DropDownItems.Add(AboutHelpItem);
            Ms.Items.Add(HelpItem);
            AboutHelpItem.Click += AboutHelpItem_Click;

            //Tool Strip Fill
            Ts.LayoutStyle = ToolStripLayoutStyle.VerticalStackWithOverflow;
            ToolStripButton NewButton = new ToolStripButton("New");
            NewButton.Size = new Size(50, 30);
            ToolStripButton OpenButton = new ToolStripButton("Open");
            OpenButton.Size = new Size(150, 30);
            ToolStripButton SaveButton = new ToolStripButton("Save");
            SaveButton.Size = new Size(150, 30);
            ToolStripButton ColorButton = new ToolStripButton("Color");
            ColorButton.Size = new Size(150, 30);
            ToolStripButton ExitButton = new ToolStripButton("Exit");
            ExitButton.Size = new Size(150, 30);

            NewButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
            OpenButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
            SaveButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
            ColorButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
            ExitButton.DisplayStyle = ToolStripItemDisplayStyle.Text;


            Ts.Items.Add(NewButton);
            Ts.Items.Add(OpenButton);
            Ts.Items.Add(SaveButton);
            Ts.Items.Add(ColorButton);
            Ts.Items.Add(ExitButton);
            NewButton.Click += NewButton_Click;
            OpenButton.Click += OpenButton_Click;
            SaveButton.Click += SaveButton_Click;

            //Add all elements to form
            this.Controls.Add(Ts);
            this.Controls.Add(Ms);
            this.Controls.Add(UnderPbPl);
            this.Controls.Add(Pb);
            this.Controls.Add(Pl);
            this.Controls.Add(Tb);

            UnderPbPl.SendToBack();

            Pl.SendToBack();
            Tb.BringToFront();


        }

        private void ColorPenItem_Click(object? sender, EventArgs e)
        {
            ColorForm colorform = new ColorForm(CurrentPen.Color);
            colorform.ShowDialog();
        }

        private void DashDotDotStyleItem_Click(object? sender, EventArgs e,
            ToolStripMenuItem SolidStyleItem, ToolStripMenuItem DotStyleItem, ToolStripMenuItem DashDotDotStyleItem)
        {
            CurrentPen.DashStyle = DashStyle.DashDotDot;
            SolidStyleItem.Checked = true;
            DotStyleItem.Checked = false;
            DashDotDotStyleItem.Checked = false;
        }

        private void DotStyleItem_Click(object? sender, EventArgs e,
            ToolStripMenuItem SolidStyleItem, ToolStripMenuItem DotStyleItem, ToolStripMenuItem DashDotDotStyleItem)
        {
            CurrentPen.DashStyle = DashStyle.Dot;
            SolidStyleItem.Checked = true;
            DotStyleItem.Checked = false;
            DashDotDotStyleItem.Checked = false;
        }

        private void SolidStyleItem_Click(object? sender, EventArgs e,
            ToolStripMenuItem SolidStyleItem, ToolStripMenuItem DotStyleItem, ToolStripMenuItem DashDotDotStyleItem)

        {
            CurrentPen.DashStyle = DashStyle.Solid;
            SolidStyleItem.Checked = true;
            DotStyleItem.Checked = false;
            DashDotDotStyleItem.Checked = false;
        }

        private void RenoEditItem_Click(object? sender, EventArgs e)
        {


            if (Historycounter < SecondHistory.Count - 1)
            {
                Pb.Image = new Bitmap(SecondHistory[Historycounter]);
            }
            else { MessageBox.Show("Ajalugu ei ole"); }
        }

        private void UndoEditItem_Click(object? sender, EventArgs e)
        {
            if (History.Count != 0 && Historycounter != 0)
            {
                Pb.Image = new Bitmap(History[Historycounter - 1]);

                History.RemoveAt(History.Count - 1);
                Historycounter = Historycounter - 1;
            }
            else { MessageBox.Show("Ajalugu ei ole"); }
        }

        private void Tb_Scroll(object? sender, EventArgs e)
        {
            CurrentPen.Width = Tb.Value;
        }

        private void SaveButton_Click(object? sender, EventArgs e)
        {
            SaveFileDialog SaveDig = new SaveFileDialog();
            SaveDig.Filter = "JPEG Image|*.jpg|Bitmap Image|*.bmp|GIF Image|*.gif|PNG Image|*.png";
            SaveDig.Title = "Save an Image File";
            SaveDig.FilterIndex = 4;
            SaveDig.ShowDialog();
            if (SaveDig.FileName != "")
            {
                System.IO.FileStream fs = (System.IO.FileStream)SaveDig.OpenFile();

                switch (SaveDig.FilterIndex)
                {
                    case 1:
                        this.Pb.Image.Save(fs, ImageFormat.Jpeg);
                        break;
                    case 2:
                        this.Pb.Image.Save(fs, ImageFormat.Bmp);
                        break;
                    case 3:
                        this.Pb.Image.Save(fs, ImageFormat.Gif);
                        break;
                    case 4:
                        this.Pb.Image.Save(fs, ImageFormat.Png);
                        break;
                }
            }
        }

        private void OpenButton_Click(object? sender, EventArgs e)
        {
            OpenFileDialog Op = new OpenFileDialog();
            Op.Filter = "JPEG Image|*.jpg|Bitmap Image|*.bmp|GIF Image|*.gif|PNG Image|*.png";
            Op.Title = "Open an Image File";
            Op.FilterIndex = 1;
            if (Op.ShowDialog() != DialogResult.Cancel)
            {
                Pb.Load(Op.FileName);
                Pb.AutoSize = false;
            }
        }

        private void NewButton_Click(object? sender, EventArgs e)
        {
            if (Pb.Image != null)
            {
                var result = MessageBox.Show("Save this image befor your create a new one", "Warning", MessageBoxButtons.YesNoCancel);
                switch (result)
                {
                    case DialogResult.No: break;
                    case DialogResult.Yes: SaveFileItem_Click(sender, e); break;
                    case DialogResult.Cancel: return;
                }
            }
            History.Clear();
            Historycounter = 0;
            Bitmap pic = new Bitmap(1000, 650);
            Pb.Image = pic;
            if (Pb != null && Pb.Image != null)
            {
                using (Graphics g = Graphics.FromImage(Pb.Image))
                {
                    g.Clear(Color.White);
                    g.DrawImage(Pb.Image, 0, 0, 1000, 650);
                }
            }
            History.Add(new Bitmap(Pb.Image));
        }

        private void OpenFileItem_Click(object? sender, EventArgs e)
        {
            OpenFileDialog Op = new OpenFileDialog();
            Op.Filter = "JPEG Image|*.jpg|Bitmap Image|*.bmp|GIF Image|*.gif|PNG Image|*.png";
            Op.Title = "Open an Image File";
            Op.FilterIndex = 1;
            if (Op.ShowDialog() != DialogResult.Cancel)
            {
                Pb.Load(Op.FileName);
                Pb.AutoSize = false;
            }
        }

        private void SaveFileItem_Click(object? sender, EventArgs e)
        {
            SaveFileDialog SaveDig = new SaveFileDialog();
            SaveDig.Filter = "JPEG Image|*.jpg|Bitmap Image|*.bmp|GIF Image|*.gif|PNG Image|*.png";
            SaveDig.Title = "Save an Image File";
            SaveDig.FilterIndex = 4;
            SaveDig.ShowDialog();
            if (SaveDig.FileName != "")
            {
                System.IO.FileStream fs = (System.IO.FileStream)SaveDig.OpenFile();

                switch (SaveDig.FilterIndex)
                {
                    case 1:
                        this.Pb.Image.Save(fs, ImageFormat.Jpeg);
                        break;
                    case 2:
                        this.Pb.Image.Save(fs, ImageFormat.Bmp);
                        break;
                    case 3:
                        this.Pb.Image.Save(fs, ImageFormat.Gif);
                        break;
                    case 4:
                        this.Pb.Image.Save(fs, ImageFormat.Png);
                        break;
                }
            }
        }

        private void Pb_MouseDown(object? sender, MouseEventArgs e)
        {
            if (Pb.Image == null)
            {
                MessageBox.Show("Before Create New File");
                return;
            }
            if (e.Button == MouseButtons.Left)
            {
                if (CurrentPen.Color == Color.White)
                {
                    CurrentPen.Color = HistoryColor;
                }
                drawning = true;
                oldLocation = e.Location;
                currentPath = new GraphicsPath();
            }
            if (e.Button == MouseButtons.Right)
            {
                HistoryColor = CurrentPen.Color;
                CurrentPen.Color = Color.White;
                drawning = true;
                oldLocation = e.Location;
                currentPath = new GraphicsPath();

            }
        }

        private void Pb_MouseUp(object? sender, MouseEventArgs e)
        {
            History.RemoveRange(Historycounter + 1, History.Count - (Historycounter + 1));
            History.Add(new Bitmap(Pb.Image));
            SecondHistory = History;
            if (Historycounter + 1 < 10)
            {
                Historycounter++;
            }
            if (History.Count - 1 == 10)
            {
                History.RemoveAt(0);
            }
            drawning = false;
            try
            {
                currentPath.Dispose();
            }
            catch { };
        }

        private void NewFileItem_Click(object? sender, EventArgs e)
        {
            if (Pb.Image != null)
            {
                var result = MessageBox.Show("Save this image befor your create a new one", "Warning", MessageBoxButtons.YesNoCancel);
                switch (result)
                {
                    case DialogResult.No: break;
                    case DialogResult.Yes: SaveFileItem_Click(sender, e); break;
                    case DialogResult.Cancel: return;
                }
            }
            History.Clear();
            Historycounter = 0;
            Bitmap pic = new Bitmap(1000, 650);
            Pb.Image = pic;
            Graphics g = Graphics.FromImage(Pb.Image);
            g.Clear(Color.White);
            g.DrawImage(Pb.Image, 0, 0, 1000, 650);
            History.Add(new Bitmap(Pb.Image));

        }

        private void AboutHelpItem_Click(object? sender, EventArgs e)
        {
            Helpform helpform = new Helpform();
            helpform.ShowDialog();
        }

        private void Pb_MouseMove(object? sender, MouseEventArgs e)
        {
            if (drawning)
            {
                Graphics g = Graphics.FromImage(Pb.Image);
                currentPath.AddLine(oldLocation, e.Location);
                g.DrawPath(CurrentPen, currentPath);
                oldLocation = e.Location;
                g.Dispose();
                Pb.Invalidate();
            }
            Lb.Text = $"X: {e.X}, Y: {e.Y}";

        }



    }
}