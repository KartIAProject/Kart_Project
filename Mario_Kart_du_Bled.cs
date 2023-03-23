using Godot;
using System;

public class Mario_Kart_du_Bled : Node2D
{
      // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
    public float time = 0F;
    public float best_time = 999F;
    public int nb_checkpoint_passed = 0;
    public bool[] all_passed = {false, false, false, false};
    
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
 
    }
  
  public int getNbCheckpoints(){
    return nb_checkpoint_passed;
  }

  // Called every frame. 'delta' is the elapsed time since the previous frame.
  public override void _Process(float delta)
  {
      time+=delta;
      var msg = (GetNode<CanvasLayer>("HUD")).GetNode<Label>("time");
      msg.Text = "TIME :"+time;
      msg.Show();
  }

  public void _on_checkpoint1_body_entered(Node2D body)
  {
    if(body.Name == "player"){
        if(all_passed[0] == false){
            all_passed[0] = true;
            nb_checkpoint_passed++;
            var msg2 = (GetNode<CanvasLayer>("HUD")).GetNode<Label>("nbcp");
            msg2.Text = "CHECKPOINT PASSED :"+nb_checkpoint_passed;
            msg2.Show();
        }
    }
  }
  public void _on_checkpoint2_body_entered(Node2D body)
  {
    if(body.Name == "player"){
        if(all_passed[1] == false){
            all_passed[1] = true;
            nb_checkpoint_passed++;
            var msg2 = (GetNode<CanvasLayer>("HUD")).GetNode<Label>("nbcp");
            msg2.Text = "CHECKPOINT PASSED :"+nb_checkpoint_passed;
            msg2.Show();
        }
    }
  }
  public void _on_checkpoint3_body_entered(Node2D body)
  {
    if(body.Name == "player"){
        if(all_passed[2] == false){
            all_passed[2] = true;
            nb_checkpoint_passed++;
            var msg2 = (GetNode<CanvasLayer>("HUD")).GetNode<Label>("nbcp");
            msg2.Text = "CHECKPOINT PASSED :"+nb_checkpoint_passed;
            msg2.Show();
        }
    }
  }
  public void _on_checkpoint4_body_entered(Node2D body)
  {
    if(body.Name == "player"){
        if(all_passed[3] == false){
            all_passed[3] = true;
            nb_checkpoint_passed++;
            var msg2 = (GetNode<CanvasLayer>("HUD")).GetNode<Label>("nbcp");
            msg2.Text = "CHECKPOINT PASSED :"+nb_checkpoint_passed;
            msg2.Show();
        }
    }
  }

  public void _on_finish_line_body_entered(Node2D body)
  {
    if(body.Name == "player"){
        if((time < best_time) && (all_passed[0] && all_passed[1] && all_passed[2] && all_passed[3])){
            best_time = time;
        }
        var msg2 = (GetNode<CanvasLayer>("HUD")).GetNode<Label>("best");
        msg2.Text = "BEST :"+best_time;
        msg2.Show();
    }
  }
}

