using CG_N4_V2;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

var nativeWindowSettings = new NativeWindowSettings
{
    Size = new Vector2i(800, 600),
    Title = "CG_N4",
    Flags = ContextFlags.ForwardCompatible
};

using var window = new Mundo(GameWindowSettings.Default, nativeWindowSettings);
window.Run();