using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Registry
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            this.SetStyle(ControlStyles.ResizeRedraw | ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, true);
            this.Paint += MainForm_Paint;
        }

        static float SQRT3 = (float)(Math.Sqrt(3.0d));
        private void MainForm_Paint(object sender, PaintEventArgs e)
        {
            Graphics doubledGraphics = e.Graphics;

            using (Bitmap bmp = new Bitmap(this.Width, this.Height))
            {
                using (Graphics g = Graphics.FromImage(bmp))
                {
                    g.FillRectangle(Brushes.Blue, 0, 0, this.Width, this.Height);

                    
                    float left = 10.0f;
                    float top = 10.0f;
                    float len = 40.0f;

                    //见RegistryShape那个图片，left,top是P1的位置。P6的X是根号三/2，Y则是P1到P6长度的Sin30°对应值，就是0.5。
                    //所以有可能是负数，那么就把top加一下。
                    if (top - 0.5f * len < 0.0f) top += 0.5f * len;
                    

                    int colnum = (int)((this.Width - left) / (SQRT3 * len))-1;
                    int rownum = (int)((this.Height - top) / (1.5f*len))-1;

                    var firstreg = GetRegistryShape(left, top, len); 

                    for (int row = 0; row < rownum; row++)
                    {
                        RegistryShape startReg = null;
                        for (int col = 0; col < colnum; col++)
                        {
                            if (col == 0)
                            {
                                if (row % 2 == 0)
                                {
                                    startReg = GetRegistryShape(firstreg.P1.X, firstreg.P1.Y + 3.0f * (row / 2) * len, len);
                                }
                                else
                                {
                                    startReg = GetRegistryShape(firstreg.P3.X, firstreg.P3.Y + 3.0f * (row / 2) * len, len);
                                }
                            }
                            DrawResgitry(g, startReg.P1.X+SQRT3*col*len, startReg.P1.Y, len);                            
                        }
                    }


                    doubledGraphics.DrawImage(bmp, 0, 0);
                }
            }
        }

        private RegistryShape GetRegistryShape(float left, float top, float len)
        {
            return new RegistryShape()
            {
                P1 = new PointF(left, top),
                P2 = new PointF(left, top + 1.0f * len),
                P3 = new PointF(left + len * (SQRT3 / 2.0f), 1.5f * len + top),
                P4 = new PointF(SQRT3 * len + left, top + 1.0f * len),
                P5 = new PointF(SQRT3 * len + left, top),
                P6 = new PointF(left + len * (SQRT3 / 2.0f), top-0.5f*len)
            };
        }
        private void DrawResgitry(Graphics g, float left, float top, float len)
        {
            g.DrawLines(Pens.White, GetRegistryShape(left, top, len).ToPointList());
        }

        internal class RegistryShape
        {
            //见图片RegistryShape.jpg
            public PointF P1;
            public PointF P2;
            public PointF P3;
            public PointF P4;
            public PointF P5;
            public PointF P6;

            public PointF[] ToPointList()
            {
                return new PointF[] { P1, P2, P3, P4, P5, P6, P1 };
            }
     
        }
    }
}
