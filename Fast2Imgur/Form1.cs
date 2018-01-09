using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Fast2Imgur
{
    public partial class Form1 : Form
    {
        private List<string> extensions = new List<string> { ".JPG", ".JPEG", ".BMP", ".GIF", ".PNG" };
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string[] args = Environment.GetCommandLineArgs();
            if (args.Length == 1) return;
            if (args.Length > 2)
            {
                textBox1.Text = "Drag only one file";
            }
            else if (extensions.Contains(Path.GetExtension(args[1]).ToUpper()) == false)
            {
                textBox1.Text = "Supported extensions = .jpg .bmp .gif .png .jpeg";
            }
            else
            {
                pictureBox1.ImageLocation = args[1];
                DialogResult dialogResult = MessageBox.Show("Do you want upload this file to Imgur?", "Upload", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    textBox1.Text = Imgur.UploadImage(args[1]);
                }
            }
        }

        private void Form1_DragDrop(object sender, DragEventArgs e)
        {
            string[] paths = (string[])e.Data.GetData(DataFormats.FileDrop, false);
            if (paths.Length > 1)
            {
                textBox1.Text = "Drag only one file";
            }
            else if (extensions.Contains(Path.GetExtension(paths[0]).ToUpper()) == false)
            {
                textBox1.Text = "Supported extensions = .jpg .bmp .gif .png .jpeg";
            }
            else
            {
                textBox1.Text = "";
                pictureBox1.ImageLocation = paths[0];
                DialogResult dialogResult = MessageBox.Show("Do you want upload this file to Imgur?", "Upload", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    textBox1.Text = Imgur.UploadImage(paths[0]);
                }
                textBox1.Focus();
            }
        }

        private void Form1_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.All;
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.V && e.Modifiers == Keys.Control)
            {
                if (Clipboard.ContainsImage())
                {
                    textBox1.Text = "";
                    Image image = Clipboard.GetImage();
                    pictureBox1.Image = image;

                    DialogResult dialogResult = MessageBox.Show("Do you want upload this file to Imgur?", "Upload", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes)
                    {
                        textBox1.Text = Imgur.UploadImage(image);
                    }
                }
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            textBox1.Focus();
            textBox1.SelectAll();
        }
    }
}
