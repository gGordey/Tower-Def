using Godot;
using System;


public partial class TileInterface : Sprite3D
{
	private float Visibility 
	{
		set
		{
			Modulate = new Color(1,1,1, value);
		}
	}

	public override void _Ready()
	{
		HideTile();
	}

	public void Activate() { Visibility = 0.5f; }

	public void Deactivate() { Visibility = 0.1f; }

	public void HideTile() { Visibility = 0.0f; }
	
}
