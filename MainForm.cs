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
using System.Runtime.Intrinsics.X86;

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
            
            this.KeyPreview = true;
            drawning = false;
            CurrentPen = new Pen(Color.Black);
            this.KeyDown += Save_ShortKey;
            this.KeyDown += Undo_ShortKey;
            this.KeyDown += Reno_ShortKey;
            //Foarm settings
            this.Text = "Halb Paint";
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
            Ts.BackColor = Color.LightGray;
            Ts.Anchor = AnchorStyles.Left;
            Ts.ImageScalingSize = new Size(130, 70);

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
            Pl.Height = 60;
            Pl.Width = 1000;
            Pl.Location = new Point(Ts.Width, Ms.Height + Pb.Height + 10);
            Pl.BackColor = Color.White;
            Pl.Text = "0,0";
            Pl.ForeColor = Color.Firebrick;

            //Label Settings
            Lb = new System.Windows.Forms.Label();
            Lb.Height = 35;
            Lb.Width = 300;
            Lb.Text = "0,0";
            Lb.BackColor = Color.White;
            Pl.Controls.Add(Lb);

            //TrackBar settings
            Tb = new System.Windows.Forms.TrackBar();
            Tb.Height = 10;
            Tb.Width = 300;
            Tb.BackColor = Color.White;
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
            UnderPbPl.BackColor = Color.LightGray;



            //Menu Strip Fill
            ToolStripMenuItem FileItem = new ToolStripMenuItem("Faili");
            ToolStripMenuItem NewFileItem = new ToolStripMenuItem("Uus");
            ToolStripMenuItem OpenFileItem = new ToolStripMenuItem("Ava");
            ToolStripMenuItem SaveFileItem = new ToolStripMenuItem("Salvesta");
            ToolStripMenuItem ExitFileItem = new ToolStripMenuItem("Välja");
            FileItem.DropDownItems.Add(NewFileItem);
            FileItem.DropDownItems.Add(OpenFileItem);
            FileItem.DropDownItems.Add(SaveFileItem);
            FileItem.DropDownItems.Add(ExitFileItem);
            Ms.Items.Add(FileItem);
            NewFileItem.Click += NewFileItem_Click;
            SaveFileItem.Click += SaveFileItem_Click;
            OpenFileItem.Click += OpenFileItem_Click;
            ExitFileItem.Click += ExitFileItem_Click;

            ToolStripMenuItem EditItem = new ToolStripMenuItem("Muuda");
            ToolStripMenuItem UndoEditItem = new ToolStripMenuItem("´Tagasi");
            ToolStripMenuItem RenoEditItem = new ToolStripMenuItem("Järgmine");
            ToolStripMenuItem PenEditItem = new ToolStripMenuItem("Pliiats");
            EditItem.DropDownItems.Add(UndoEditItem);
            EditItem.DropDownItems.Add(RenoEditItem);
            EditItem.DropDownItems.Add(PenEditItem);
            UndoEditItem.Click += UndoEditItem_Click;
            RenoEditItem.Click += RenoEditItem_Click;

            PenEditItem.Checked = true;
            ToolStripMenuItem StylePenItem = new ToolStripMenuItem("Stiil");
            ToolStripMenuItem ColorPenItem = new ToolStripMenuItem("Värv");
            PenEditItem.DropDownItems.Add(ColorPenItem);
            PenEditItem.DropDownItems.Add(StylePenItem);
            StylePenItem.Checked = true;
            ColorPenItem.Click += ColorPenItem_Click;
            ToolStripMenuItem SolidStyleItem = new ToolStripMenuItem("Soliidne");
            ToolStripMenuItem DotStyleItem = new ToolStripMenuItem("Punkt");
            ToolStripMenuItem DashDotDotStyleItem = new ToolStripMenuItem("KriipsPunktPunkt");
            StylePenItem.DropDownItems.Add(SolidStyleItem);
            StylePenItem.DropDownItems.Add(DotStyleItem);
            StylePenItem.DropDownItems.Add(DashDotDotStyleItem);
            SolidStyleItem.Checked = true;
            Ms.Items.Add(EditItem);
            SolidStyleItem.Click += (sender, e) => SolidStyleItem_Click(sender, e, SolidStyleItem, DotStyleItem, DashDotDotStyleItem);
            DotStyleItem.Click += (sender, e) => DotStyleItem_Click(sender, e, SolidStyleItem, DotStyleItem, DashDotDotStyleItem); ;
            DashDotDotStyleItem.Click += (sender, e) => DashDotDotStyleItem_Click(sender, e, SolidStyleItem, DotStyleItem, DashDotDotStyleItem); ;

            ToolStripMenuItem HelpItem = new ToolStripMenuItem("Abi");
            ToolStripMenuItem AboutHelpItem = new ToolStripMenuItem("Kohta");
            HelpItem.DropDownItems.Add(AboutHelpItem);
            Ms.Items.Add(HelpItem);
            AboutHelpItem.Click += AboutHelpItem_Click;

            //Tool Strip Fill
            Ts.LayoutStyle = ToolStripLayoutStyle.VerticalStackWithOverflow;

            // Creating a resized image
            Image NewImage = Image.FromFile("../../../New.png");
            Image ColorImage = Image.FromFile("../../../ColorImage.png");
            Image OpenImage = Image.FromFile("../../../OpenImage.png");
            Image SaveImage = Image.FromFile("../../../SaveImage.png");
            Image ExitImage = Image.FromFile("../../../ExitImage.png");

            ToolStripButton NewButton = new ToolStripButton("Uus");
            NewButton.Image = NewImage;
            NewButton.ImageAlign = ContentAlignment.MiddleCenter;
            NewButton.DisplayStyle = ToolStripItemDisplayStyle.Image;   

            ToolStripButton OpenButton = new ToolStripButton("Ava");
            OpenButton.Image = OpenImage;
            OpenButton.ImageAlign = ContentAlignment.MiddleCenter;
            OpenButton.DisplayStyle = ToolStripItemDisplayStyle.Image;

            ToolStripButton SaveButton = new ToolStripButton("Salvesta");
            SaveButton.Image = SaveImage;
            SaveButton.ImageAlign = ContentAlignment.MiddleCenter;
            SaveButton.DisplayStyle = ToolStripItemDisplayStyle.Image;

            ToolStripButton ColorButton = new ToolStripButton("Värv");
            ColorButton.ImageAlign = ContentAlignment.MiddleCenter;
            ColorButton.Image = ColorImage;
            ColorButton.DisplayStyle = ToolStripItemDisplayStyle.Image;

            ToolStripButton ExitButton = new ToolStripButton("Välja");
            ExitButton.ImageAlign = ContentAlignment.MiddleCenter;
            ExitButton.Image = ExitImage;
            ExitButton.DisplayStyle = ToolStripItemDisplayStyle.Image;

            Ts.Items.Add(NewButton);
            Ts.Items.Add(OpenButton);
            Ts.Items.Add(SaveButton);
            Ts.Items.Add(ColorButton);
            Ts.Items.Add(ExitButton);

            NewButton.Click += NewButton_Click;
            OpenButton.Click += OpenButton_Click;
            SaveButton.Click += SaveButton_Click;
            ColorButton.Click += ColorButton_Click;
            ExitButton.Click += ExitButton_Click;




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

        private void ExitButton_Click(object? sender, EventArgs e)
        {
            var result = MessageBox.Show("Sa tahad lahkuda?", "Hoiatus", MessageBoxButtons.YesNo);
            switch (result)
            {
                case DialogResult.Yes:
                    this.Close();
                    break;
                case DialogResult.No:
                    break;
            }
        }

        private void ExitFileItem_Click(object? sender, EventArgs e)
        {
            var result = MessageBox.Show("Sa tahad lahkuda?", "Hoiatus", MessageBoxButtons.YesNo);
            switch (result)
            {
                case DialogResult.Yes:
                    this.Close();
                    break;
                case DialogResult.No:
                    break;
            }
        }

        private void ColorButton_Click(object? sender, EventArgs e)
        {
            ColorForm colorform = new ColorForm(CurrentPen.Color);
            colorform.Owner = this;
            colorform.ShowDialog();
        }

        private void ColorPenItem_Click(object? sender, EventArgs e)
        {
            ColorForm colorform = new ColorForm(CurrentPen.Color);
            colorform.Owner = this;
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

            
                if (Historycounter <= History.Count - 1)
                {
                    if (Historycounter + 1 < History.Count)
                    {
                        Pb.Image = new Bitmap(History[Historycounter + 1]);
                        Historycounter = Historycounter + 1;
                    }

                }
                else MessageBox.Show("Ajalugu ei ole");
            
        }

        private void UndoEditItem_Click(object? sender, EventArgs e)
        {
            if (History.Count != 0 && Historycounter != 0)
            {
                Pb.Image = new Bitmap(History[Historycounter - 1]);

                
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
                var result = MessageBox.Show("Salvesta see pilt enne uue", "Hoiatus", MessageBoxButtons.YesNoCancel);
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
                MessageBox.Show("Enne uue faili loomist");
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
                var result = MessageBox.Show("Salvesta see pilt enne uue", "Hoiatus", MessageBoxButtons.YesNoCancel);
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

        ////SHORTKEYS//
        private void Save_ShortKey(object sender, KeyEventArgs e)
        {
            
            if (e.Control && e.KeyCode == Keys.S)
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
        }
        private void Undo_ShortKey(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.Z)
            {
                if (History.Count != 0 && Historycounter != 0)
                {
                    Pb.Image = new Bitmap(History[Historycounter - 1]);


                    Historycounter = Historycounter - 1;
                }
                else { MessageBox.Show("Ajalugu ei ole"); }
            }

        }

        private void Reno_ShortKey(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.X)
            {
                if (Historycounter <= History.Count - 1)
                {
                    if (Historycounter +1 < History.Count)
                    {
                        Pb.Image = new Bitmap(History[Historycounter + 1]);
                        Historycounter = Historycounter + 1;
                    }

                }
                else MessageBox.Show("Ajalugu ei ole");
            }
        }

    }
}