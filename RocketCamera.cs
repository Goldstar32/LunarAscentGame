using System;
using Godot;

public partial class RocketCamera : Camera3D
{
    // Export makes properties accessible in godot editor
    [Export]
    public NodePath RocketPath = "."; // Drag rocket node into this in the editor

    [Export]
    public float FollowSpeed = 5f; // Speed of following the rocket

    [Export]
    public float RotationSpeed = 1f; // Speed of rotation with arrow keys

    [Export]
    public Vector3 Offset = new Vector3(0, 5, -10); // Default offset from the rocket

    private Node3D _rocket; // Reference to the rocket node
    private float _yaw = 0f; // Rotation around the Y-axis
    private float _pitch = 0f; // Rotation around the X-axis

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        if (RocketPath != null)
        {
            _rocket = GetNode<Node3D>(RocketPath);
        }
        else
        {
            GD.PrintErr("RocketPath is not set in the CameraController script.");
        }
    }

    public override void _Process(double delta)
    {
        if (_rocket == null)
            return;

        // Smoothly follow the rocket
        Vector3 targetPosition = _rocket.GlobalTransform.Origin;

        // Transform the offset based on the rocket's global orientation
        Vector3 globalOffset = _rocket.GlobalTransform.Basis * Offset;

        // Apply the camera position
        Vector3 desiredPosition = targetPosition + globalOffset;
        Vector3 smoothedPosition = GlobalTransform.Origin.Lerp(
            desiredPosition,
            (float)(FollowSpeed * delta)
        );
        GlobalTransform = new Transform3D(GlobalTransform.Basis, smoothedPosition);

        // Align the camera's rotation with the rocket's rotation
        LookAt(targetPosition, _rocket.GlobalTransform.Basis.Y);

        // Handle rotation controls
        HandleRotation((float)delta);
    }

    private void HandleRotation(float delta)
    {
        // Handle user input for rotation
        if (Input.IsActionPressed("uiLeft"))
        {
            _yaw -= RotationSpeed * delta;
        }
        if (Input.IsActionPressed("uiRight"))
        {
            _yaw += RotationSpeed * delta;
        }
        if (Input.IsActionPressed("uiUp"))
        {
            _pitch = Mathf.Clamp(
                _pitch - RotationSpeed * delta,
                Mathf.DegToRad(-90f),
                Mathf.DegToRad(90f)
            );
        }
        if (Input.IsActionPressed("uiDown"))
        {
            _pitch = Mathf.Clamp(
                _pitch + RotationSpeed * delta,
                Mathf.DegToRad(-90f),
                Mathf.DegToRad(90f)
            );
        }
    }
}
