using Godot;
using System;

public class player : RigidBody2D
{
	[Export] 
	public float STEERING=300.0F;
	[Export] 
	public float ACCELERATION=50.0F;
	[Export] 
	public float FRICTION=2.0F;
	[Export] 
	public float DRIFT_FRICTION=0.8F;
	[Export] 
	public float DRIFT_STEERING=600.0F;

	public override void _PhysicsProcess(float delta)
	{
		input();
	}
	
	public void input()
	{
		this.LinearDamp = FRICTION;
		if (Input.IsActionPressed("ui_left"))
		{
			ApplyTorqueImpulse(-STEERING);
		}
		if (Input.IsActionPressed("ui_right"))
		{
			ApplyTorqueImpulse(STEERING);
		}
		if (Input.IsActionPressed("ui_up"))
		{
			ApplyCentralImpulse((new Vector2(0, -ACCELERATION)).Rotated(Rotation));
		}
		if (Input.IsActionPressed("ui_down"))
		{
			ApplyCentralImpulse((new Vector2(0, ACCELERATION)).Rotated(Rotation));
		}
		
	}
}
