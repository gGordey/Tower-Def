using Godot;
using System;

public partial class ProgressBar : Sprite3D
{
	[Export] public Texture2D ProgressBackGround;
	[Export] public Texture2D ProgressForeGround;
	[Export] public float MaxValue;
	[Export] public float BaseSize;

	private float _value;

	private Sprite3D ForeGround;

	public float Value
	{
		get { return _value; }
		set 
		{
			_value = value;
			ChangeValue();
		}
	}

	public override void _Ready()
	{
		ForeGround = GetNode<Sprite3D>("ProgressFill");
	   	Texture = ProgressBackGround;
		ForeGround.Texture = ProgressForeGround;
		Rotation = new Vector3 (75.8f*3.14f, 0,0);
		Value = MaxValue;
		Scale = new Vector3(BaseSize,BaseSize,BaseSize);
	}

	public void ChangeValue()
	{
		ForeGround.Scale = new Vector3 (Value / MaxValue, 1,1);//BaseSize, BaseSize);
	}
}
