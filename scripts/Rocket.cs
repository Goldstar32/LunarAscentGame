using System;
using System.Collections.Generic;
using Godot;

public partial class Rocket : StaticBody3D
{
    // Constants vvv

    private PackedScene thrusterScene = (PackedScene)
        ResourceLoader.Load("res://scenes/thruster.tscn"); // Scene path to thruster

    // Constants ^^^
    //
    // Properties vvv

    public float MFuel { get; set; } // Mass of fuel [kg]

    // (!) Fuel consumption is performed inside Thruster.GetThrustForce()

    public float MDry { get; set; } // Dry mass (non fuel mass) [kg]

    public float MTot => MFuel + MDry; // Total mass [kg]

    public float Radius { get; set; } // Rockets radius [m]

    // Use Node3D.GlobalPosition directly for position [m]

    public Vector3 Velocity { get; set; } // Velocity relative to space [m/s]

    public Vector3 Acceleration { get; set; } // Acceleration relative to space [m/s^2]

    // Use Node3D.GlobalBasis directly for rotation [rad]

    public Vector3 AngularVelocity { get; set; } // Property for angular velocity relative to space [rad/s]

    public Vector3 AngularAcceleration { get; set; } // Property for angular acceleration relative to space [rad/s^2]

    public List<Thruster> Thrusters { get; set; } = new List<Thruster>(); // List of thrusters attached to the rocket

    // Properties ^^^
    //
    // Methods vvv

    // Called when the node enters the scene tree for the first time.
    public override void _Ready() { }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta) { }

    // Creates new instance of engine and attaches it to this rocket
    public void AddNewThruster(
        string thrusterId,
        Vector3 position, // Position relative to parent rocket
        Basis rotation, // Rotation relative to parent rocket
        double mdot,
        double exhaustVelocity
    )
    {
        Thruster newThruster = (Thruster)thrusterScene.Instantiate(); // Instantiate new thruster

        newThruster.Id = thrusterId; // Assign id
        newThruster.Mdot = mdot; // Set mass flow rate
        newThruster.ExhaustVelocity = exhaustVelocity; // Set exhaust velocity

        AddChild(newThruster); // Add new thruster as child to this rocket

        // Set the transform of the new thruster relative to the parent rocket
        newThruster.Transform = new Transform3D(rotation, position);

        Thrusters.Add(newThruster); // Add new thruster to list of thrusters
    }

    // Get combined thrust force of all engines
    public Vector3 GetTotalThrustForce(double delta)
    {
        Vector3 totalThrust = Vector3.Zero;

        foreach (Thruster thruster in Thrusters)
        {
            if (Input.IsActionPressed(thruster.Id)) // Check if thruster is activated
            {
                totalThrust += thruster.GetThrustForce(delta);
            }
        }

        return totalThrust;
    }

    // Get combined torque from all engines
    public Vector3 GetTotalThrustTorque(double delta)
    {
        Vector3 totalTorque = Vector3.Zero;

        foreach (Thruster thruster in Thrusters)
        {
            if (Input.IsActionPressed(thruster.Id)) // Check if thruster is activated
            {
                totalTorque += thruster.GetTorque(delta);
            }
        }

        return totalTorque;
    }
}
