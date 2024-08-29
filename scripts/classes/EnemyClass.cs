using Godot;
using System;
using System.Collections.Generic;
using System.Net.Http.Headers;

[GlobalClass]
public partial class EnemyClass : CharacterBody3D
{
	[Export] private Texture2D Sprite;
	[Export] private float _speed;
	[Export] private int _hp;
	[Export] private float scale ;
	
	public Global Global;

	private string currentDir;
	private Dictionary<string, Vector2> DirVectors = new()
	{
		{"up",new Vector2(0,-1)},
		{"down",new Vector2(0,1)},
		{"right",new Vector2(1,0)}
	};

	private Sprite3D SpriteNode;
	private ProgressBar ProgressBar;

	public float Speed { get { return _speed; } set { _speed = value; }}
	public int CurentHp { get { return _hp; } 
		set 
			{
				if (value <= 0) { QueueFree(); }
				_hp = value;
				ProgressBar.Value = value;
			}
	}
	public string MovingDir {get { return currentDir; } set { currentDir = value; }}
	public float SpriteScale { set { SpriteNode.Scale = new Vector3(value,value,value); } get { return scale; } }

	public override void _Ready()
	{
		SpriteNode = GetNode<Sprite3D>("EnemySprite");
		ProgressBar = GetNode<ProgressBar>("ProgressBar");
		ProgressBar.MaxValue = _hp;
		SpriteNode.Texture = Sprite;
		MovingDir = "right";
		SpriteNode.Rotation = new Vector3(75.8f*3.14f,0,0);
		SpriteScale = scale;
		Global = GetNode<Global>("/root/Global");
	}

	public override void _Process(double delta)
	{
		if (Global.IsLosed) {return;}
		Move((float)delta);
	}

	private void Move(float delta)
	{
		Position += new Vector3 (DirVectors[currentDir].X*Speed*delta, 0, DirVectors[currentDir].Y*Speed*delta);
	}
}
