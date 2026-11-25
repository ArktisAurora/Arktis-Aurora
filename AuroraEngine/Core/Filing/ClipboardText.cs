using ArctisAurora.Core.Testing;
using ArctisAurora.EngineWork;
using ArctisAurora.EngineWork.Rendering;
using Silk.NET.GLFW;
using System.Runtime.InteropServices;

namespace ArctisAurora.Core.Filing
{
    // Text on the OS clipboard. A test run keeps it in process, so it never touches the user's.
    public static unsafe class ClipboardText
    {
        private static string? testText;
        private static delegate* unmanaged<WindowHandle*, byte*> getClipboardString;

        public static string? Get()
        {
            if (TestRunner.active) return testText;
            if (Engine.primary == null) return null;

            if (getClipboardString == null)
                getClipboardString = (delegate* unmanaged<WindowHandle*, byte*>)AGlfwWindow._glfw.Context.GetProcAddress("glfwGetClipboardString");
            return Marshal.PtrToStringUTF8((nint)getClipboardString(Engine.primary.os.handle));
        }

        public static void Set(string text)
        {
            if (TestRunner.active)
            {
                testText = text;
                return;
            }
            if (Engine.primary == null) return;

            AGlfwWindow._glfw.SetClipboardString(Engine.primary.os.handle, text);
        }
    }
}
