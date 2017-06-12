using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Runtime.InteropServices;

namespace ImageViewer
{
    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct IMG24BITS
    {
        byte R;
        byte G;
        byte B;
    }

    public partial class Form1 : Form
    {
        // member variables.
        private string[] mFileNames;
        private Bitmap[] mBitmap;
        private int mIdxShow = 0;

        private bool mIsMouseDown = false;

        private int mMousePosX = 0;
        private int mMousePosY = 0;

        private int mScrPosX = 0;
        private int mScrPosY = 0;

        //---------------------------------------------------------------------
        public Form1()
        {
            InitializeComponent();

            this.Text = Application.ProductName;

            this.openFileDialog1.Multiselect = true;

            this.WindowState = FormWindowState.Maximized;
        }

        //---------------------------------------------------------------------
        private void quitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //---------------------------------------------------------------------
        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                this.mFileNames = this.openFileDialog1.FileNames;
                Array.Sort(this.mFileNames);

                int numOfFiles = this.mFileNames.Length;
                this.mBitmap = new Bitmap[numOfFiles];

                for (int nFiles = 0; nFiles < numOfFiles; nFiles++)
                {
                    this.mBitmap[nFiles] = new Bitmap(this.mFileNames[nFiles]);
                }

                this.mIdxShow = 0;
                this.toolStripStatusLabel1.Text = this.mIdxShow.ToString();
                this.toolStripStatusLabel2.Text = this.mFileNames[this.mIdxShow];

                this.pictureBox1.Width  = this.mBitmap[this.mIdxShow].Width;
                this.pictureBox1.Height = this.mBitmap[this.mIdxShow].Height;

                this.panel1.Width  = this.mBitmap[this.mIdxShow].Width;
                this.panel1.Height = this.mBitmap[this.mIdxShow].Height;

                this.pictureBox1.Invalidate();
            }
        }

        //---------------------------------------------------------------------
        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            if (this.mBitmap == null)
            {
                return;
            }
            else if (this.mBitmap[this.mIdxShow] == null)
            {
                return;
            }
            else {

                int width  = this.mBitmap[this.mIdxShow].Width;
                int height = this.mBitmap[this.mIdxShow].Height;

                e.Graphics.DrawImage(this.mBitmap[this.mIdxShow], 0, 0, width, height);
            }
            
        }

        //---------------------------------------------------------------------
        private void previousToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.mIdxShow > 0)
            {
                this.mIdxShow--;
                this.pictureBox1.Invalidate();
                this.toolStripStatusLabel1.Text = this.mIdxShow.ToString();
                this.toolStripStatusLabel2.Text = this.mFileNames[this.mIdxShow];
            }
        }

        //---------------------------------------------------------------------
        private void nextToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.mIdxShow < this.mFileNames.Length-1)
            {
                this.mIdxShow++;
                this.pictureBox1.Invalidate();
                this.toolStripStatusLabel1.Text = this.mIdxShow.ToString();
                this.toolStripStatusLabel2.Text = this.mFileNames[this.mIdxShow];
            }
        }

        //---------------------------------------------------------------------
        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            this.mIsMouseDown = true;

            this.mMousePosX = e.Location.X;
            this.mMousePosY = e.Location.Y;

            this.mScrPosX = this.panel1.AutoScrollPosition.X;
            this.mScrPosY = this.panel1.AutoScrollPosition.Y;
        }

        //---------------------------------------------------------------------
        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            this.mIsMouseDown = false;
        }

        //---------------------------------------------------------------------
        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (this.mIsMouseDown == false) return;

            int diffX = e.Location.X - this.mMousePosX;
            int diffY = e.Location.Y - this.mMousePosY;

            if ((diffX == 0) && (diffY == 0))
            {
                return;
            }

            this.panel1.AutoScrollPosition = new Point(-this.panel1.AutoScrollPosition.X - diffX, -this.panel1.AutoScrollPosition.Y - diffY);

            this.pictureBox1.Invalidate();
        }

        //---------------------------------------------------------------------
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyData)
            {
                case Keys.Left:
                    this.previousToolStripMenuItem_Click(sender, e);
                    break;
                case Keys.Right:
                    this.nextToolStripMenuItem_Click(sender, e);
                    break;
            }
        }
    }
}
