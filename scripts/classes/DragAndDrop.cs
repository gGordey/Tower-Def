using Godot;
using System;

public partial class DragAndDrop : Marker3D
{
	// Called when the node enters the scene tree for the first time.
	public float Y_Level = 0.5f;
	public Node2D Screen2D;
	public bool is_activated;
	public override void _Ready()
	{
		Screen2D = new Node2D ();
		AddChild(Screen2D);
	}

	public override void _Process(double delta)
	{
		if (is_activated)
		{
		Vector2 mouse_pos = Screen2D.GetGlobalMousePosition();
		Position = new Vector3 (8.8f*(mouse_pos.X/1920)-4.4f, Y_Level, 5.0f*(mouse_pos.Y/1080)-2.87f);
		GetParent<Node3D>().Position = new Vector3 (8.8f*(mouse_pos.X/1920)-4.4f, Y_Level, 5.0f*(mouse_pos.Y/1080)-2.87f);
		}
	}
}
