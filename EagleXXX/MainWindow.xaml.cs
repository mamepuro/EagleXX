using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;


namespace EagleXXX
{
    public partial class MainWindow : Window
    {
        float eyeX = 1.0f;
        float eyeY = 1.0f;
        float eyeZ = 1.0f;
        static GraphicsMode mode = new GraphicsMode(
            GraphicsMode.Default.ColorFormat,
            GraphicsMode.Default.Depth,
            8,
            8,
            GraphicsMode.Default.AccumulatorFormat,
            GraphicsMode.Default.Buffers,
            GraphicsMode.Default.Stereo);

        GLControl glControl = new GLControl(mode);
        public MainWindow()
        {
            InitializeComponent();

            glControl.Load += glControl_Load;
            glControl.Paint += glControl_Paint;
            glControl.Resize += glControl_Resize;

            glHost.Child = glControl;

        }
        private void glControl_Load(object sender, EventArgs e)
        {
            GL.ClearColor(Color4.White);

            GL.Viewport(0, 0, glControl.Width, glControl.Height);


            //視体積の設定
            GL.MatrixMode(MatrixMode.Projection);
            Matrix4 proj = Matrix4.CreatePerspectiveFieldOfView(MathHelper.PiOver4, glControl.AspectRatio, 0.2f, 5);
            GL.LoadMatrix(ref proj);


            //視界の設定
            GL.MatrixMode(MatrixMode.Modelview);
            Matrix4 look = Matrix4.LookAt(new Vector3(1, 1, 1), new Vector3(0, 0, 0.75f), Vector3.UnitZ);
            GL.LoadMatrix(ref look);

            //デプスバッファの使用
            GL.Enable(EnableCap.DepthTest);



            //光源の使用
            GL.Enable(EnableCap.Lighting);
        }

        private void glControl_Resize(object sender, EventArgs e)
        {
            GL.Viewport(0, 0, glControl.Width, glControl.Height);
        }

        private void glControl_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            GL.Material(MaterialFace.Front, MaterialParameter.Emission, Color4.Blue);
            Draw_tube(2, 0.1f, 0.1f, eyeX, eyeY, eyeZ);

            glControl.SwapBuffers();

        }

        float rx, ry;
        void Draw_tube(float length, float radius1, float radius2, float rotateX, float rotateY, float rotateZ)
        {
            rotateX = rotateX % 360;
            rotateY = rotateY % 360;
            rotateZ = rotateZ % 360;
            GL.PushMatrix();
            {
                GL.Rotate(rotateX, 1, 0, 0);
                GL.Rotate(rotateY, 0, 1, 0);
                GL.Rotate(rotateZ, 0, 0, 1);
                GL.Begin(PrimitiveType.TriangleStrip);
                GL.Normal3(Vector3.One);

                for (int degree = 0; degree <= 360; degree += 3)
                {
                    rx = (float)Math.Cos((float)Math.PI * degree / 180);
                    ry = (float)Math.Sin((float)Math.PI * degree / 180);
                    GL.Vertex3(rx * radius2, ry * radius2, length / 2);
                    GL.Vertex3(rx * radius1, ry * radius1, -length / 2);
                }
            }
            GL.PopMatrix();
            GL.End();
            eyeX = 0;
            eyeY = 0;
            eyeZ = 0;

        }
        private void OnkeyDownhandler(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Up)
            {
                eyeY += 1f;
                //Debug.Print(eyeX, eyeY, eyeZ);
            }
            if (e.Key == Key.Down)
            {
                eyeY -= 1f;
            }
            if (e.Key == Key.Right)
            {
                eyeX += 1f;
            }
            if (e.Key == Key.Left)
            {
                eyeX -= 1f;
            }
            ResetCam();
        }
        private void ResetCam()
        {

            glControl.Refresh();

        }
    }
}
