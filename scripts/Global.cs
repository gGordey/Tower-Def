using Godot;
using System;

public partial class Global : Node
{
	public RandomNumberGenerator random;

	public GridPlace CurrentGrid;
	public Node3D GameSpace;
	public EnemySpewner EnemySpawner;
	public Castle Castle;

	public override void _Ready()
	{
		random = new RandomNumberGenerator();
		random.Randomize();
	}

	public override void _Process(double delta)
	{
	}

	public float ToAngle(float ang) { return ang*3.14f; }
	
	public int RandiRange(int a,int b) { return random.RandiRange(a,b); }
	public float RandiRange(float a, float b) { return random.RandfRange(a,b); }
}
