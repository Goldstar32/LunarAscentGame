using System;
using Godot;

public partial class Moon : StaticBody3D
{
    // Constants vvv

    const float moonRadius = 1000f; // Constant for moons radius 1737.4e3f
    const double moonMass = 1e15f; // Constant for moons mass 7.342e22

    // Constants ^^^
    //
    // Properties vvv

    private CollisionShape3D CollisionShape { get; set; } // Property for collision shape

    private MeshInstance3D MeshInstance { get; set; } // Property for mesh

    public double MoonMass { get; set; } // Property for moons mass

    public double Radius { get; set; } // Property for moons radius

    // Use global position for position

    // Properties ^^^
    //
    // Methods vvv

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        // Initialize properties by getting the nodes
        CollisionShape = GetNode<CollisionShape3D>("MoonCollisionShape");
        MeshInstance = GetNode<MeshInstance3D>("MoonMesh");

        // Set initial values
        MoonMass = moonMass; // Set mass of moon
        Radius = moonRadius; // Set radius of moon
        if (CollisionShape.Shape is SphereShape3D sphereShape) // Make sure shape is sphere
            sphereShape.Radius = moonRadius; // Set collision shape's radius
        if (MeshInstance.Mesh is SphereMesh sphereMesh) // Make sure shape is sphere
        {
            sphereMesh.Radius = moonRadius;
            sphereMesh.Height = moonRadius * 2;
        }
        this.GlobalPosition = new Vector3(0, -moonRadius, 0); // Center of moon (origo is approx at surface of moon)
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta) { }
}
