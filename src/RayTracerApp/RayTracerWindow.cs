using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace RayTracer.App
{
    public class RayTracerWindow : IDisposable
    {
        private NativeWindow _nativeWindow;
        private GraphicsContext _graphicsContext;
        private bool _active;
        private Scene _scene;
        private RenderTexture _renderTexture;

        public RenderTexture RenderTexture
        {
            get { return _renderTexture; }
        }

        public RayTracerWindow(int width, int height, string title)
        {
            _nativeWindow = new NativeWindow(width, height, title, GameWindowFlags.Default, GraphicsMode.Default, DisplayDevice.Default);
            GraphicsContextFlags flags = GraphicsContextFlags.Default;
#if DEBUG
            //flags |= GraphicsContextFlags.Debug;
#endif
            _graphicsContext = new GraphicsContext(GraphicsMode.Default, _nativeWindow.WindowInfo, 3, 0, flags);
            _graphicsContext.MakeCurrent(_nativeWindow.WindowInfo);
            ((IGraphicsContextInternal)_graphicsContext).LoadAll(); // wtf is this?

            SetInitialStates();
            SetViewport();

            _nativeWindow.Resize += OnGameWindowResized;
            _nativeWindow.Closing += OnWindowClosing;

            _scene = Scene.DefaultScene;

            _renderTexture = new RenderTexture(_nativeWindow.Width, _nativeWindow.Height);
        }

        public void Show()
        {
            _nativeWindow.Visible = true;
            _active = true;
            while (_active)
            {
                RenderFrame();
                _nativeWindow.ProcessEvents();
            }
        }

        private void RenderFrame()
        {
            // render graphics
            GL.Clear(ClearBufferMask.ColorBufferBit);

            Bitmap bitmap = _scene.Camera.RenderSceneToBitmapThreaded(_scene, _nativeWindow.Width, _nativeWindow.Height).Result;
            _renderTexture.UpdateImage(bitmap);
            _renderTexture.Render();
            _graphicsContext.SwapBuffers();
        }

        private void SetInitialStates()
        {
            GL.ClearColor(System.Drawing.Color.CornflowerBlue);
        }

        private void SetViewport()
        {
            _graphicsContext.Update(_nativeWindow.WindowInfo);
            GL.Viewport(0, 0, _nativeWindow.Width, _nativeWindow.Height);
        }

        private void OnGameWindowResized(object sender, EventArgs e)
        {
            SetViewport();
        }

        private void OnWindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _active = false;
        }

        public void Dispose()
        {
            // Dispose of GL context, resources, etc.
            _graphicsContext.Dispose();
            _nativeWindow.Dispose();
        }
    }
}
