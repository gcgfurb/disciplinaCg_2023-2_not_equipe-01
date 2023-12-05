using CG_N4_V2.Common;
using CG_N4_V2.Lights;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace CG_N4_V2;

public class Mundo : GameWindow
{
    private Camera _camera;

    private bool _firstMove = true;

    private Vector2 _lastPos;

    private Cubo _principalCube;
    private Cubo _rotationCube;
    private float _orbitAngle;
    private float _spinAngle;

    public Mundo(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings)
        : base(gameWindowSettings, nativeWindowSettings)
    {
    }

    protected override void OnLoad()
    {
        base.OnLoad();

        GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);

        GL.Enable(EnableCap.DepthTest);
        GL.Enable(EnableCap.CullFace);

        _principalCube = new BasicLight();

        _rotationCube = new RotationalCube();
        
        _camera = new Camera(Vector3.UnitZ * 3, Size.X / (float)Size.Y);

        CursorState = CursorState.Grabbed;
    }

    protected override void OnRenderFrame(FrameEventArgs e)
    {
        base.OnRenderFrame(e);

        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

        _orbitAngle += 1.5f * (float)e.Time;

        _spinAngle += 2.0f * (float)e.Time;

        const float orbitRadius = 3.0f;

        var smallCubePosition = new Vector3(
            orbitRadius * MathF.Cos(_orbitAngle),
            orbitRadius * MathF.Sin(_orbitAngle),
            0.0f);

        var rotationMatrix = Matrix4.CreateRotationZ(_spinAngle);
        var translationMatrix = Matrix4.CreateTranslation(smallCubePosition);

        _rotationCube.Model = rotationMatrix * translationMatrix;

        _principalCube.Renderizar(new Transformacao4D(), _camera);
        _rotationCube.Renderizar(new Transformacao4D(), _camera);
        
        SwapBuffers();
    }

    protected override void OnUpdateFrame(FrameEventArgs e)
    {
        base.OnUpdateFrame(e);

        if (!IsFocused)
        {
            return;
        }

        if (KeyboardState.IsKeyDown(Keys.Escape))
        {
            Close();
        }

        const float cameraSpeed = 1.5f;
        const float sensitivity = 0.2f;

        if (KeyboardState.IsKeyDown(Keys.W))
        {
            _camera.Position += _camera.Front * cameraSpeed * (float)e.Time; // Forward
        }

        if (KeyboardState.IsKeyDown(Keys.S))
        {
            _camera.Position -= _camera.Front * cameraSpeed * (float)e.Time; // Backwards
        }

        if (KeyboardState.IsKeyDown(Keys.A))
        {
            _camera.Position -= _camera.Right * cameraSpeed * (float)e.Time; // Left
        }

        if (KeyboardState.IsKeyDown(Keys.D))
        {
            _camera.Position += _camera.Right * cameraSpeed * (float)e.Time; // Right
        }

        if (KeyboardState.IsKeyDown(Keys.Space))
        {
            _camera.Position += _camera.Up * cameraSpeed * (float)e.Time; // Up
        }

        if (KeyboardState.IsKeyDown(Keys.LeftShift))
        {
            _camera.Position -= _camera.Up * cameraSpeed * (float)e.Time; // Down
        }

        if (KeyboardState.IsKeyPressed(Keys.D1))
        {
            _principalCube = new BasicLight();
        }
        else if (KeyboardState.IsKeyPressed(Keys.D2))
        {
            _principalCube = new LightingMaps();
        }
        else if (KeyboardState.IsKeyPressed(Keys.D3))
        {
            _principalCube = new DirectionalLights();
        }
        else if (KeyboardState.IsKeyPressed(Keys.D4))
        {
            _principalCube = new PointLights();
        }
        else if (KeyboardState.IsKeyPressed(Keys.D5))
        {
            _principalCube = new SpotLight();
        }
        else if (KeyboardState.IsKeyPressed(Keys.D6))
        {
            _principalCube = new MultipleLights();
        }

        if (_firstMove)
        {
            _lastPos = new Vector2(MouseState.X, MouseState.Y);
            _firstMove = false;
        }
        else
        {
            var deltaX = MouseState.X - _lastPos.X;
            var deltaY = MouseState.Y - _lastPos.Y;
            _lastPos = new Vector2(MouseState.X, MouseState.Y);

            _camera.Yaw += deltaX * sensitivity;
            _camera.Pitch -= deltaY * sensitivity;
        }
    }

    protected override void OnMouseWheel(MouseWheelEventArgs e)
    {
        base.OnMouseWheel(e);

        _camera.Fov -= e.OffsetY;
    }

    protected override void OnResize(ResizeEventArgs e)
    {
        base.OnResize(e);

        GL.Viewport(0, 0, Size.X, Size.Y);
        _camera.AspectRatio = Size.X / (float)Size.Y;
    }
}