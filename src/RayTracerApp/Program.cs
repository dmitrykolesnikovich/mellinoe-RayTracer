using System;
using System.Drawing;

namespace RayTracer.App
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                using (RayTracerWindow window = new RayTracerWindow(640, 480))
                {
                    window.Show();
                }
            }
            catch (Exception e)
            {
                Console.Error.WriteLine("Unhandled Exception: " + Environment.NewLine + e.ToString());
            }

        }
    }
}
