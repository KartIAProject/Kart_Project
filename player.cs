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
	
	private float AccelerationInit, AccelerationZL;

	public player()
		{
			//Variables cgt vitesse zone lente
			AccelerationInit = ACCELERATION;
			AccelerationZL = (ACCELERATION)/5;
			//
			
		}
		
        public override void _Ready()
    {
        Random rnd = new Random();
        GetChild<Area2D>(4).Position = new Vector2(rnd.Next()%100,rnd.Next()%100);
    }
    public override void _PhysicsProcess(float delta)
    {   
        int nbCheckpoints = GetParent<Mario_Kart_du_Bled>().getNbCheckpoints();
        bool[] tab = Ia.launch(this.LinearVelocity,nbCheckpoints);
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
	
	// Variables et Fonctions pour le changement de vitesse en zone lente
		
	public void VelocityZL()
	{
		ACCELERATION = AccelerationZL;
	}
	
	public void VelocityInit()
	{
		ACCELERATION = AccelerationInit;
	}
	
	
	
}
