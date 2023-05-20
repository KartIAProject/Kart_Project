using Godot;
using System;

public class Mario_Kart_du_Bled : Node2D
{

	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";
	

	// Member variables

	public float time = 0F;
	public float best_time = 999F;
	public int nb_checkpoint_passed = 0;
	public int nb_mur_cogne = 0;
	public bool[] all_passed = {false, false, false, false};

	public int nbZoneLente = 0;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		
	}

	public float getTime(){
	return time;
}

public int getNbZoneLente(){
	return nbZoneLente;
}

public void setTime(float t){
	time = t;
}

public void setAll_Passed(bool[] tab){
	all_passed = tab;
}

public int getNbCheckpoints(){
	return nb_checkpoint_passed;
}

public void setNbCheckpoints(int c){
	nb_checkpoint_passed = c;
}



  // Called every frame. 'delta' is the elapsed time since the previous frame.
  // Each checkpoint (Area2D objects) must be passd trought at least once before the finish line allow the best time to be displayed
  public override void _Process(float delta)
  {
	  time+=delta;
	  var msg = (GetNode<CanvasLayer>("HUD")).GetNode<Label>("time");
	  msg.Text = "TIME :"+time;
	  msg.Show();
	  if(!all_passed[0] && !all_passed[1] && !all_passed[2] && !all_passed[3]){
			var msg2 = (GetNode<CanvasLayer>("HUD")).GetNode<Label>("nbcp");
			msg2.Text = "CHECKPOINT PASSED :"+nb_checkpoint_passed;
			msg2.Show();
	  }
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

// the finish line displays the best time on the car entering it's collider, ony if the lap was done "correctly".
// it also reinitialize the timer and allows to run another lap by reinitializing the all_passed array.
  public void _on_finish_line_body_entered(Node2D body)
  {
	if(body.Name == "player"){
		if((time < best_time) && (all_passed[0] && all_passed[1] && all_passed[2] && all_passed[3])){
			best_time = time;
			time = 0F;
			var msg = (GetNode<CanvasLayer>("HUD")).GetNode<Label>("time");
	  		msg.Text = "TIME :"+time;
	  		msg.Show();
			msg = (GetNode<CanvasLayer>("HUD")).GetNode<Label>("best");
			msg.Text = "BEST :"+best_time;
			msg.Show();
			nb_checkpoint_passed = 0;
			msg = (GetNode<CanvasLayer>("HUD")).GetNode<Label>("nbcp");
			msg.Text = "CHECKPOINT PASSED :"+nb_checkpoint_passed;
			msg.Show();
			for(int i = 0; i<4; i++){
				all_passed[i] = false;
			}
		}
	}
  }


	// Changement vélocité Zone Lente 
	public void _on_ZoneLente_body_entered(player body)
	{
		if(body.Name == "player"){
			body.VelocityZL();
	}
	}

	public void _on_ZoneLente_body_exited(player body)
	{
		if(body.Name == "player"){
			body.VelocityInit();
			nbZoneLente++;
		}
	}


	// Pour remettre "tout" à zéro avec un nouvel individu :
	public void resetAll(){
		time = 0F;
		var msg = (GetNode<CanvasLayer>("HUD")).GetNode<Label>("time");
	  	msg.Text = "TIME :"+time;
	  	msg.Show();
		for(int i = 0; i<all_passed.Length; i++){
			all_passed[i] = false;
		}
		nb_checkpoint_passed = 0;
		var msg2 = (GetNode<CanvasLayer>("HUD")).GetNode<Label>("nbcp");
		msg2.Text = "CHECKPOINT PASSED :"+nb_checkpoint_passed;
		msg2.Show();
	}

}






