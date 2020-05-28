using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SNES_palette_importer_exporter
{
    public partial class Form1 : Form
    {
        byte[] rom = new byte[0x400000];
        string fileName;
        int maxFileNameLength = 108;
        List<List<Color>> palette = new List<List<Color>>();

        public Form1()
        {
            InitializeComponent();
        }

        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog d = new OpenFileDialog();
            d.Filter = "ROM file (*.smc;*.sfc)|*.smc;*.sfc";
            d.Title = "Select an SNES ROM";

            if (d.ShowDialog() == DialogResult.OK)
            {
                //Loading my file and displaying all my content.
                byte[] temp = File.ReadAllBytes(d.FileName);
                // Start reading after the header
                Int32 startingPoint = temp.Length == 0x400000 + 0x200 ? 0x200 : 0;
                Array.Copy(temp, startingPoint, rom, 0, 0x400000);

                panel1.Visible = true;
                fileName = d.FileName;
                this.Text = (fileName.Length > maxFileNameLength) ? fileName.Substring(fileName.Length - maxFileNameLength) : fileName;
                textBox_palette.Focus();

                System.IO.File.WriteAllBytes(fileName, rom);

            }
        }

        private void button_export_Click(object sender, EventArgs e)
        {
            try
            {
                ExportFunc();
            }
            catch (Exception)
            {
                MessageBox.Show("Invalid input");
            }
        }

        private void button_import_Click(object sender, EventArgs e)
        {
            try
            {
                ImportFunc();
            }
            catch (Exception)
            {
                MessageBox.Show("Invalid input");
            }
        }
        private void Import (Bitmap bmp)
        {
            int address = Convert.ToInt32(textBox_palette.Text, 16);

            if (bmp.Height > 160 || (bmp.Width != 320 && bmp.Width != 300))
            {
                MessageBox.Show("Invalid size. Are you loading the right file?");
                return;
            }

            palette = new List<List<Color>>();

            // Loop through selected bmp, getting color of each square
            for (int i = 0; i < bmp.Height; i += 20)
            {
                // Add a new row
                palette.Add(new List<Color>());

                for (int j = 0; j < bmp.Width; j += 20)
                {
                    palette[i / 20].Add(bmp.GetPixel(j, i));
                }
            }
            for (int i = 0; i < palette.Count; i++) {
                for (int j = 0; j < palette[i].Count; j++)
                {
                    var color = palette[i][j];

                    // Break down into the 3 component colors snes uses
                    int r = color.R >> 3 << 0;
                    int g = color.G >> 3 << 5;
                    int b = color.B >> 3 << 10;

                    int value = r | g | b;

                    Write16(value, ref address);
                }
            }
            System.IO.File.WriteAllBytes(fileName, rom); //Include date and random number
            CreateBitmapAtRuntime(bmp.Width);
            MessageBox.Show("Imported!");
            // Empty every time
            palette = new List<List<Color>>();
        }

        public void CreateBitmapAtRuntime(int width)
        {
            // Create bitmap based on selected option
            Bitmap thisPalette = new Bitmap(width, palette.Count * 20);

            using (Graphics g = Graphics.FromImage(thisPalette))
            {
                g.FillRectangle(new SolidBrush(Color.FromArgb(0,0,0)), 0, 0, 20, 20);
                // Loop through all colors in palette
                for (int i = 0; i < palette.Count; i++)
                {
                    for (int j = 0; j < palette[i].Count; j++)
                    {
                        g.FillRectangle(new SolidBrush(palette[i][j]), j * 20, i * 20, 20, 20);
                    }
                }
            }

            pictureBox1.Image = thisPalette;

            //return (Bitmap)thisPalette.Clone();
        }

        public UInt16 Read16(ref Int32 address)
        {
            address &= 0x3fffff;
            return (UInt16)(
                (rom[address++] << 0) |
                (rom[address++] << 8));
        }

        public void Write16(Int32 value, ref Int32 address)
        {
            address &= 0x3fffff;
            rom[address++] = (byte)(value >> 0);
            rom[address++] = (byte)(value >> 8);

        }

        private void ImportFunc ()
        {
            if (textBox_palette.Text.Length == 0)
            {
                MessageBox.Show("Error!");
                return;
            }

            OpenFileDialog d = new OpenFileDialog();
            d.Filter = "Bitmap (*.bmp;)|*.bmp;";
            d.Title = "Select a Bitmap to use";

            if (d.ShowDialog() == DialogResult.OK)
            {
                var byteArray = File.ReadAllBytes(d.FileName);
                Bitmap bmp = BytesToBitmap(byteArray);
                Import(bmp);
            }
        }
        public static Bitmap BytesToBitmap(byte[] byteArray)
        {
            using (MemoryStream ms = new MemoryStream(byteArray))
            {
                ms.Position = 0;
                Bitmap img = (Bitmap)Image.FromStream(ms);
                return img;
            }
        }

        private void ExportFunc ()
        {
            int widthPalette = 15;
            if (textBox_palette.Text.Length == 0)
            {
                MessageBox.Show("Error!");
                return;
            }

            //pictureBox_test = new PictureBox();

            // Always add 1
            palette.Add(new List<Color>());

            if (!radioButton_object.Checked)
            {
                widthPalette = 16;
                // Add enough colors for bg palette
                for (int i = 1; i < 8; i++)
                {
                    palette.Add(new List<Color>());
                }
            }

            int address = Convert.ToInt32(textBox_palette.Text, 16);

            // Loop through every color in ROM
            for (int i = 0; i < palette.Count; i++)
            {
                for (int j = 0; j < widthPalette; j++)
                {
                    // Value at address
                    int value = Read16(ref address);
                    // = _bbbbbgg gggrrrrr
                    int r = value >> 0 & 0x1f;
                    int g = value >> 5 & 0x1f;
                    int b = value >> 10 & 0x1f;

                    // Put in current format
                    r <<= 3;
                    g <<= 3;
                    b <<= 3;

                    // Add color to palette
                    palette[i].Add(Color.FromArgb(r, g, b));
                }
            }

            CreateBitmapAtRuntime(widthPalette * 20);

            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "Image Files (*.bmp)|*.bmp;";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                int width = Convert.ToInt32(pictureBox1.Image.Width);
                int height = Convert.ToInt32(pictureBox1.Image.Height);
                Bitmap bmp = new Bitmap(width, height);
                bmp = (Bitmap)pictureBox1.Image;
                bmp.Save(dialog.FileName.Substring(0, dialog.FileName.Length - 4) + " - 0x" + textBox_palette.Text + ".bmp", ImageFormat.Bmp);
                MessageBox.Show("Exported!");
            }

            //MessageBox.Show(widthPalette.ToString());

            // Reset to default
            palette = new List<List<Color>>();

        }

    }
}
