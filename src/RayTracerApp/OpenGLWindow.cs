using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayTracer.App
{
    public class OpenGLWindow : IDisposable
    {
        private NativeWindow _nativeWindow;

        public OpenGLWindow(int width, int height, string title)
        {
            _nativeWindow = new NativeWindow(width, height, title, GameWindowFlags.Default, OpenTK.Graphics.GraphicsMode.Default, DisplayDevice.Default);
        }

        public void Dispose()
        {
            // Dispose of GL context, resources, etc.
            throw new NotImplementedException();
        }
    }
}
