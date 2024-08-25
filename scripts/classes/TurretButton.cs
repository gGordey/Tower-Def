using Godot;
using System;

public partial class TurretButton : TextureButton
{
	[Export] public PackedScene Tower;
	[Export] public Texture2D Icon;

	private Global Global;

	public override void _Ready()
	{
		TextureNormal = Icon;
		Global = GetNode<Global>("/root/Global");
		ButtonDown += SpawnTower;
	}

	public void SpawnTower()
	{
		TowerClass NewTower = Tower.Instantiate<TowerClass>();
		Global.GameSpace.AddChild(NewTower);
		NewTower.Dragger.is_activated = true;
	}
}
