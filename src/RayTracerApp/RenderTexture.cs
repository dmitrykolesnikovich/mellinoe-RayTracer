using OpenTK.Graphics.OpenGL;
using RayTracerCore;
using System;
using System.Numerics;

namespace RayTracer.App
{
    public class RenderTexture
    {
        private readonly RenderBuffer _renderBuffer;
        private int _width;
        private int _height;

        private int _textureBufferId;

        public RenderBuffer RenderBuffer
        {
            get { return _renderBuffer; }
        }

        public RenderTexture(int width, int height)
        {
            _width = width;
            _height = height;
            _renderBuffer = new RenderBuffer(width, height);

            _textureBufferId = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, _textureBufferId);

            //the following code sets certian parameters for the texture
            GL.TexEnv(TextureEnvTarget.TextureEnv, TextureEnvParameter.TextureEnvMode, (float)TextureEnvMode.Modulate);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
        }

        public void Resize(int width, int height)
        {
            _width = width;
            _height = height;
            _renderBuffer.Resize(width, height);
        }

        public void UpdateImage()
        {
            unsafe
            {
                GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, _width, _height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Rgba, PixelType.Float, (IntPtr)_renderBuffer.BasePtr);
            }
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
