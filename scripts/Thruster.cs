using System;
using Godot;

public partial class Thruster : Node3D
{
    // Constants vvv

    private const double g = 9.80665; // Standard gravity (used in calculation) [N]

    // Constants ^^^
    //
    // Properties vvv

    public string Id { get; set; } // Id to identify this specific thruster

    public double Mdot { get; set; } // Mass flow rate or fuel consumption [kg/s]

    public double ExhaustVelocity { get; set; } // Velocity of exhausted mass [m/s]

    // Properties ^^^
    //
    // Methods vvv

    // Called when the node enters the scene tree for the first time.
    public override void _Ready() { }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta) { }

    // Returns thrust force as vector based on engine's direction relative to rocket
    public Vector3 GetThrustForce(double delta, bool consumeFuel = true)
    {
        if (GetParent() is Rocket rocket && rocket.MFuel > 0)
        {
            // Fuel consumed in this time step
            float deltaFuel = (float)(Mdot * delta);
            // Consume no more than the remaining fuel if the remaining fuel is less than deltaFuel
            if (deltaFuel > rocket.MFuel)
            {
                deltaFuel = (float)rocket.MFuel;
            }

            if (consumeFuel)
            {
                // Reduce rocket's fuel by deltaFuel
                rocket.MFuel -= deltaFuel;
            }

            // Calculate thrust force magnitude
            float thrustMagnitude = (float)ExhaustVelocity * deltaFuel / (float)delta;

            // Direction of thrust: upwards (thrusters's +Y direction in global space)
            Vector3 thrustDirection = GlobalTransform.Basis.Y.Normalized();

            // Return thrust force as vector
            return thrustDirection * thrustMagnitude;
        }

        // No thrust if out of fuel
        return Vector3.Zero;
    }

    // Returnes the torque as a vector based on engine's direction and position relative to rocket
    public Vector3 GetTorque(double delta)
    {
        // Get thrust force in world space
        Vector3 thrustForce = GetThrustForce(delta, false);

        if (GetParent() is Rocket rocket)
        {
            // Calculate relative position of the thruster in world space
            Vector3 relativePosition = GlobalTransform.Origin - rocket.GlobalTransform.Origin;

            // Torque = r x F (cross product)
            return relativePosition.Cross(thrustForce);
        }

        return Vector3.Zero;
    }
}
