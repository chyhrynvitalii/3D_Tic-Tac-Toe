using Godot;
using System;

public partial class Viewport : Node3D
{   
    public const float rotationSpeed = 5f / 1000f;
    public const float zoomSpeed = 0.05f;
    public const float pitchMax = (float)Math.PI / 2f;
    public const float zoomMin = 5f;
    public const float zoomMax = 15f;
    public const float damp = 0.9f;

    public float twistInput = 0;
    public float pitchInput = 0;
    public float zoomInput = 0;

    public Node3D TwistPivot => GetNode<Node3D>("TwistPivot");
    public Node3D PitchPivot => GetNode<Node3D>("TwistPivot/PitchPivot");
    public Camera3D Camera => GetNode<Camera3D>("TwistPivot/PitchPivot/Camera3D");

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        CameraRotation();
        CameraZoom();
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        base._UnhandledInput(@event);

        if (@event is InputEventMouseMotion motion)
        {
            if (Input.IsMouseButtonPressed(MouseButton.Right))
            {
                CameraRotate(motion);
            }
        }

        if (Input.IsActionJustPressed("zoom_in"))
        {
            CameraZoomIn();
        }
        if (Input.IsActionJustPressed("zoom_out"))
        {
            CameraZoomOut();
        }

        if (Input.IsKeyPressed(Key.Q))
        {
            GetTree().Quit();
        }
    }

    private void CameraRotate(InputEventMouseMotion motion)
    {
        twistInput = -motion.Relative.X * rotationSpeed;
        pitchInput = -motion.Relative.Y * rotationSpeed;
    }

    private void CameraRotation()
    {
        TwistPivot.Rotation = new Vector3(
            TwistPivot.Rotation.X,
            TwistPivot.Rotation.Y + twistInput,
            TwistPivot.Rotation.Z
        );
        
        PitchPivot.Rotation = new Vector3(
            Math.Clamp(PitchPivot.Rotation.X + pitchInput, -pitchMax, pitchMax),
            PitchPivot.Rotation.Y,
            PitchPivot.Rotation.Z
        );

        twistInput *= damp;
        pitchInput *= damp;
    }
    
    private void CameraZoomIn()
    {
        zoomInput -= zoomSpeed;
    }

    private void CameraZoomOut()
    {
        zoomInput += zoomSpeed;
    }

    private void CameraZoom()
    {
        Camera.Position = new Vector3(
            Camera.Position.X,
            Camera.Position.Y,
            Math.Clamp(Camera.Position.Z + zoomInput, zoomMin, zoomMax)
        );

        zoomInput *= damp;
    }
}