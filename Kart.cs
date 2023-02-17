using Godot;
using System;

public class Kart : KinematicBody2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    private Vector2 _velocity = Vector2.Zero;
    private float _speed = 200f;

    public override void _Process(float delta)
    {
        if (Input.IsActionPressed("ui_right"))
        {
            _velocity.x = _speed;
        }
        else if (Input.IsActionPressed("ui_left"))
        {
            _velocity.x = -_speed;
        }

        else if (Input.IsActionPressed("ui_down"))
        {
            _velocity.y = _speed;
        }
        else if (Input.IsActionPressed("ui_up"))
        {
            _velocity.y = -_speed;
        }
        else
        {
            _velocity.x = 0f;
            _velocity.y = 0f;
        }

        MoveAndSlide(_velocity);
    }


//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
