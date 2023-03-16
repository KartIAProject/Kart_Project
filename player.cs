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

    private IA Ia = new IA();

    public override void _PhysicsProcess(float delta)
    {   
        bool[] tab = Ia.launch(this.LinearVelocity);
        input(tab);
    }
    
    public void input(bool[] tab)
    {
        this.LinearDamp = FRICTION;
        if (Input.IsActionPressed("ui_left") || tab[0])
        {
            ApplyTorqueImpulse(-STEERING);
        }
        if (Input.IsActionPressed("ui_right") || tab[1])
        {
            ApplyTorqueImpulse(STEERING);
        }
        if (Input.IsActionPressed("ui_up") || tab[2])
        {
            ApplyCentralImpulse((new Vector2(0, -ACCELERATION)).Rotated(Rotation));
        }
        if (Input.IsActionPressed("ui_down") || tab[3])
        {
            ApplyCentralImpulse((new Vector2(0, ACCELERATION)).Rotated(Rotation));
        }
        
    }
}
