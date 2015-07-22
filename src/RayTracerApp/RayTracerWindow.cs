using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Reflection;

namespace RayTracer.App
{
    public class RayTracerWindow : IDisposable
    {
        private NativeWindow _nativeWindow;
        private GraphicsContext _graphicsContext;
        private bool _active;
        private Scene _scene;
        private RenderTexture _renderTexture;

        // Config
        private const int DefaultReflectionDepth = 5;

        public RenderTexture RenderTexture
        {
            get { return _renderTexture; }
        }

        private List<Scene> _scenes;

        private const string Title = "RayTracer | F5: Render | F1/F2: Change Scene | -/+: Reflection Depth {0}";

        public RayTracerWindow(int width, int height)
        {
            _nativeWindow = new NativeWindow(width, height, "", GameWindowFlags.Default, GraphicsMode.Default, DisplayDevice.Default);
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
            _nativeWindow.KeyDown += OnKeyDown;

            _scene = Scene.TwoPlanes;
            _scene.Camera.ReflectionDepth = DefaultReflectionDepth;

            SetTitle();

            _renderTexture = new RenderTexture(_nativeWindow.Width, _nativeWindow.Height);

            _scenes = typeof(Scene).GetProperties()
                .Where(pi => pi.PropertyType == typeof(Scene))
                .Select(pi => pi.GetValue(null))
                .Cast<Scene>().ToList();
        }

        private void SetTitle()
        {
            _nativeWindow.Title = string.Format(Title, _scene.Camera.ReflectionDepth);
        }

        private void OnKeyDown(object sender, OpenTK.Input.KeyboardKeyEventArgs e)
        {
            if (e.Key == OpenTK.Input.Key.F5)
            {
                RefreshButtonPressed();
            }

            if (e.Key == OpenTK.Input.Key.Plus)
            {
                AddReflectionDepth(1);
            }
            if (e.Key == OpenTK.Input.Key.Minus)
            {
                AddReflectionDepth(-1);
            }

            if (e.Key == OpenTK.Input.Key.F2)
            {
                MoveScene(1);
            }
            if (e.Key == OpenTK.Input.Key.F1)
            {
                MoveScene(-1);
            }
        }

        private void AddReflectionDepth(int amount)
        {
            _scene.Camera.ReflectionDepth += amount;
            SetTitle();
        }

        private void MoveScene(int direction)
        {
            int index = _scenes.IndexOf(_scene);
            index = (index + 1) % _scenes.Count;
            _scene = _scenes[index];
            IssueNewRender();
        }

        private void RefreshButtonPressed()
        {
            IssueNewRender();
        }

        private int _previousFrameTick;
        private double desiredFrameTime = (1.0 / 30.0) * 1000.0;

        public void Show()
        {
            _nativeWindow.Visible = true;
            _active = true;
            RenderSceneAsync();
            while (_active)
            {
                int newTickTime = Environment.TickCount;
                int elapsed = newTickTime - _previousFrameTick;

                while (elapsed < desiredFrameTime)
                {
                    Thread.Sleep(0);
                    newTickTime = Environment.TickCount;
                    elapsed = newTickTime - _previousFrameTick;
                }

                RenderFrame();
                _nativeWindow.ProcessEvents();
                _previousFrameTick = newTickTime;
            }
        }

        private CancellationTokenSource _tokenSource;
        private Task _currentTask;
        private void RenderSceneAsync()
        {
            _currentTask = Task.Run(() =>
            {
                _scene.Camera.RenderSceneToBitmapThreaded(_scene, _renderTexture.RenderBuffer, _nativeWindow.Width, _nativeWindow.Height);
            });
        }

        private void RenderFrame()
        {
            // render graphics
            GL.Clear(ClearBufferMask.ColorBufferBit);
            _renderTexture.UpdateImage();
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

        private void IssueNewRender()
        {
            _currentTask.Wait();
            _renderTexture.Resize(_nativeWindow.Width, _nativeWindow.Height);
            RenderSceneAsync();
        }

        private void CancelPreviousTask()
        {
            if (_tokenSource != null)
            {
                _tokenSource.Cancel(false);
            }
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
