using CG_N2_3;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

var nativeWindowSettings = new NativeWindowSettings
{
    Size = new Vector2i(800, 800),
    Title = "CG_N2_1",
    // This is needed to run on macos
    Flags = ContextFlags.ForwardCompatible,
};

using var window = new Mundo(GameWindowSettings.Default, nativeWindowSettings);
window.Run();