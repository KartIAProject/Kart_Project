using Godot;
using System;

public class Kart : KinematicBody2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    //private Vector2 _velocity = Vector2.Zero;
    //private float _speed = 200f;
    
    private const float MAX_SPEED = 200;
    private const float ACCELERATION = 150;
    private const float ROTATION_SPEED = 1f;
    private const float ROAD_RESISTANCE = 20f;
    
    private Vector2 velocity = Vector2.Zero;
    private float acceleration = 0;
    private float rotation = 0;

    public override void _PhysicsProcess(float delta)
    {
        // Calculate acceleration based on user input
        acceleration = 0;
        if (Input.IsActionPressed("ui_up"))
        {
            acceleration = -ACCELERATION;
        }
        else if (Input.IsActionPressed("ui_down"))
        {
            acceleration = ACCELERATION;
        }

        // Calculate rotation based on user input
        rotation = 0;
        if (Input.IsActionPressed("ui_left"))
        {
            rotation = -ROTATION_SPEED;
        }
        else if (Input.IsActionPressed("ui_right"))
        {
            rotation = ROTATION_SPEED;
        }

        // Calculate new velocity based on acceleration and current rotation
        Vector2 roadResistance = -velocity.Normalized() * ROAD_RESISTANCE;
        Vector2 accelerationVector = new Vector2(0, acceleration).Rotated(rotation);
        velocity += (accelerationVector + roadResistance) * delta;
        velocity = velocity.LimitLength(MAX_SPEED);

        // Rotate car based on current rotation
        Rotation += rotation * delta;

        // Move car based on current velocity and rotation
        MoveAndSlide(velocity.Rotated(Rotation));
        
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
