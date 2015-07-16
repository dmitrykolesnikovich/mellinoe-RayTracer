using RayTracer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace RayTracerCore
{
    public unsafe class RenderBuffer
    {
        private IntPtr _bufferLocation;
        private Color* _basePtr;
        private int _width;
        private int _height;

        public unsafe Color* BasePtr
        {
            get
            {
                return _basePtr;
            }
        }

        public RenderBuffer(int width, int height)
        {
            Resize(width, height);
        }

        public void Resize(int width, int height)
        {
            if (_bufferLocation != IntPtr.Zero)
            {
                Marshal.FreeHGlobal(_bufferLocation);
            }
            unsafe
            {
                _width = width;
                _height = height;
                _bufferLocation = Marshal.AllocHGlobal(width * height * sizeof(Color));
                _basePtr = (Color*)_bufferLocation;
            }
        }

        public void SetColor(int x, int y, ref Color color)
        {
            unsafe
            {
                _basePtr[x + (y * _width)] = color;
            }
        }
    }
}
