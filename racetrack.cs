using Godot;
using System;

public class racetrack : Node2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
    public float time = 0F;
    public float best_time = 999F;
    public int nb_checkpoint_passed = 0;
    private int nb_max_checkpoints = 4;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        
    }

  // Called every frame. 'delta' is the elapsed time since the previous frame.
  public override void _Process(float delta)
  {
      time+=delta;
      var msg = (GetNode<CanvasLayer>("HUD")).GetNode<Label>("time");
      msg.Text = "TIME :"+time;
      msg.Show();
  }

  public void _on_checkpoint_body_entered(Node2D body)
  {
    if(body.Name == "player"){
        nb_checkpoint_passed++;
        var msg2 = (GetNode<CanvasLayer>("HUD")).GetNode<Label>("nbcp");
        msg2.Text = "CHECKPOINT PASSED :"+nb_checkpoint_passed;
        msg2.Show();
    }
  }

  public void _on_finish_line_body_entered(Node2D body)
  {
    if(body.Name == "player"){
        if((time < best_time) && (nb_checkpoint_passed >= nb_max_checkpoints)){
            best_time = time;
        }
        var msg2 = (GetNode<CanvasLayer>("HUD")).GetNode<Label>("best");
        msg2.Text = "BEST :"+best_time;
        msg2.Show();
        time = 0;
    }
  }
}
