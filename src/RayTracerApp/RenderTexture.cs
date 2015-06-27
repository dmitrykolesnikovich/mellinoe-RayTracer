using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace RayTracer.App
{
    public class RenderTexture
    {
        private int _width;
        private int _height;

        private int _textureBufferId;

        public RenderTexture(int width, int height)
        {
            this._width = width;
            this._height = height;

            _textureBufferId = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, _textureBufferId);

            //the following code sets certian parameters for the texture
            GL.TexEnv(TextureEnvTarget.TextureEnv, TextureEnvParameter.TextureEnvMode, (float)TextureEnvMode.Modulate);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
        }

        public void UpdateImage(Bitmap bitmap)
        {
            _width = bitmap.Width;
            _height = bitmap.Height;

            // Upload the Bitmap to OpenGL.
            BitmapData data = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, _width, _height, 0,
                OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);

            bitmap.UnlockBits(data);
        }

        public void Render()
        {
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();

            GL.MatrixMode(MatrixMode.Projection);
            Matrix4x4 ortho_projection = Matrix4x4.CreateOrthographicOffCenter(0, _width, _height, 0, -1, 1);
            GLEx.LoadMatrix(ref ortho_projection);

            GL.Enable(EnableCap.Texture2D);
            GL.BindTexture(TextureTarget.Texture2D, _textureBufferId);

            GL.Begin(PrimitiveType.Quads);
            GL.TexCoord2(0, 0);
            GL.Vertex2(0, 0);
            GL.TexCoord2(1, 0);
            GL.Vertex2(_width, 0);
            GL.TexCoord2(1, 1);
            GL.Vertex2(_width, _height);
            GL.TexCoord2(0, 1);
            GL.Vertex2(0, _height);
            GL.End();

            GL.Disable(EnableCap.Texture2D);
        }
    }
}
