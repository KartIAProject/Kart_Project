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

    private RayCast2D s1;
    private RayCast2D s2;
    private RayCast2D s3;

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
        int pos1 = rnd.Next()%100;
        int pos2 = rnd.Next()%100;
        s1 = GetNode<RayCast2D>("/root/Mario_Kart_du_Bled/player/sensors/s1");
        s1.Position = new Vector2(pos1,pos2);
        s2 = GetNode<RayCast2D>("/root/Mario_Kart_du_Bled/player/sensors/s2");
        s3 = GetNode<RayCast2D>("/root/Mario_Kart_du_Bled/player/sensors/s3");
        //GetChild<Area2D>(4).GetChild<CollisionShape2D>(0).Position = new Vector2(pos1,pos2);
    }
    public override void _PhysicsProcess(float delta)
    {   
        s1.ForceRaycastUpdate();
        s2.ForceRaycastUpdate();
        s3.ForceRaycastUpdate();
        
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
