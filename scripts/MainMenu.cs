using System;
using Godot;

public partial class MainMenu : Control
{
    private string PhysicsProcessPath = "res://scenes/physics_process.tscn";

    [Export]
    public NodePath MainMenuPath = "."; // Drag node into this in the editor

    private PackedScene physicsProcessScene;

    private PhysicsProcess physicsProcess;

    private PackedScene packedScene;

    private MainMenu mainMenu;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready() { }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta) { }

    // Called when play button is pressed
    public void _on_play_button_pressed()
    {
        LoadPhysicsProcess();
    }

    // Loads physics process
    private void LoadPhysicsProcess()
    {
        // Load scene
        physicsProcessScene = (PackedScene)ResourceLoader.Load(PhysicsProcessPath);

        // Instanciate rocket from scene
        physicsProcess = (PhysicsProcess)physicsProcessScene.Instantiate();

        // Remove menu
        GetNode(MainMenuPath).QueueFree();

        // Add rocket
        AddSibling(physicsProcess);
    }
}
