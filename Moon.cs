using System;
using Godot;

public partial class Moon : StaticBody3D
{
	// Constants vvv

	const double moonRadius = 1737.4e3; // Constant for moons radius

	// Constants ^^^
	//
	// Properties vvv

	public double MoonMass { get; set; } // Property for moons mass

	public double Radius { get; set; } // Property for moons radius

	// Use global position for position

	// Properties ^^^
	//
	// Methods vvv

	// Constructor
	public Moon() { }

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// Set initial values
		MoonMass = 7.342e22; // Mass of moon
		Radius = moonRadius; // Radius of moon
		this.GlobalPosition = new Vector3(0, -(float)moonRadius, 0); // Center of moon (origo is approx at surface of moon)
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta) { }
}
