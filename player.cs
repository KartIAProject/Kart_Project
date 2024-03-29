using Godot;
using System;
using System.Collections.Generic;
using System.Linq;


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
	
	private float AccelerationInit, AccelerationZL;
	
	//Nombre de collisions avec les murs	
	int nbMurCogne = 0;
	bool canDetectCollision = true;

	// Index de l'individu
	public int actualIndexIndividu;

	// Index du tableau de mouvement de l'individu
	public int actualIndexOfIndividu;

	// Liste de mouvemements de l'individu actuel
	public List<string> actualIndividu;

	public int tailleIndividu = 1000;

	private CollisionShape2D[] checkpointTab;

	private Population pop;

	private int nbIndividus = 30;

	private int numPopulation = 1;

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

		// On récupère les checkpoints
		CollisionShape2D cs1 = GetNode<Area2D>("/root/Mario_Kart_du_Bled/checkpoint1").GetChild<CollisionShape2D>(0);
		CollisionShape2D cs2 = GetNode<Area2D>("/root/Mario_Kart_du_Bled/checkpoint2").GetChild<CollisionShape2D>(0);
		CollisionShape2D cs3 = GetNode<Area2D>("/root/Mario_Kart_du_Bled/checkpoint3").GetChild<CollisionShape2D>(0);
		CollisionShape2D cs4 = GetNode<Area2D>("/root/Mario_Kart_du_Bled/checkpoint4").GetChild<CollisionShape2D>(0);
		CollisionShape2D cs5 = GetNode<Area2D>("/root/Mario_Kart_du_Bled/finish line").GetChild<CollisionShape2D>(0);
		checkpointTab = new CollisionShape2D[]{cs1,cs2,cs3,cs4,cs5};

		actualIndexIndividu = 0;

		actualIndexOfIndividu = 0;

		actualIndividu = new List<string>();

		pop = new Population(nbIndividus,tailleIndividu,5);
		pop.generatePopulation();

	}
	public override void _PhysicsProcess(float delta)
	{   

		int nbCheckpoints = GetParent<Mario_Kart_du_Bled>().getNbCheckpoints();
		float time = GetParent<Mario_Kart_du_Bled>().getTime();
		float timeZonelente = GetParent<Mario_Kart_du_Bled>().getNbZoneLente();

		// Quand on à parcouru tous les individus on fait évolué la population
		if(actualIndexIndividu == nbIndividus){
			GD.Print("[+] New Population");
			// On affiche le numero de la population dans le HUD
			this.numPopulation++;
			GetParent<Mario_Kart_du_Bled>().setTextPopulation(this.numPopulation);
			// Et on remet le numero de l'individu a 0
			GetParent<Mario_Kart_du_Bled>().setTextIndividu(1);
			pop.evoluate();

			 // On remet la vitesse à 0
			this.LinearVelocity = new Vector2(0,0);
			this.Inertia = 0;
			this.AngularVelocity = 0;
			
			// On remet les indexs à 0 aussi
			actualIndexOfIndividu = 0;
			actualIndexIndividu = 0;
		}

		// On modifie le vecteur de mouvements pour chaque nouveau individu
		if(actualIndexOfIndividu == 0){
			actualIndividu = pop.getIndividu(actualIndexIndividu);
		}

		// Tableau qui stocks les inputs de l'invidus
		bool[] tab = stringArrayToBool(actualIndividu[actualIndexOfIndividu]);
		actualIndexOfIndividu++;

		// Quand on arrive à la fin d'un individu on passe au suivant
		if(actualIndexOfIndividu == tailleIndividu){

			// On remet sa vitesse à 0
			this.LinearVelocity = new Vector2(0,0);
			this.Inertia = 0;
			this.AngularVelocity = 0;

			// On calcul son score
			int[] weight = {10000,10,-15,200};
			pop.calculateFitness(actualIndexIndividu,generateAttribute(nbCheckpoints,checkpointTab[nbCheckpoints],timeZonelente,nbMurCogne),weight);

			// On remet le temps à 0 ainsi que les checkpoints à 0 ainsi que le nombre de mur collisioné
			bool[] temp = new bool[4];
			for(int k=0; k<4; k++){temp[k] = false;}
			GetParent<Mario_Kart_du_Bled>().setAll_Passed(temp);
			GetParent<Mario_Kart_du_Bled>().setNbCheckpoints(0);
			GetParent<Mario_Kart_du_Bled>().setTime(0);
			GetParent<Mario_Kart_du_Bled>().setNbZoneLente(0);
			nbMurCogne = 0;
			
			// On met l'index du tableau de mouvements à 0
			actualIndexOfIndividu = 0;

			GD.Print("[+] New Individu ["+actualIndexIndividu+"]");
			
			//On incrémente pour passe à l'individu suivant
			actualIndexIndividu++;
			// On affiche le numero de l'individu dans le HUD
			GetParent<Mario_Kart_du_Bled>().setTextIndividu(this.actualIndexIndividu+1);

			// On repositionne le nouveau individu sur la grille de départ
			this.Position = new Vector2(1232,2517);
			this.RotationDegrees = -90;
		}

		input(tab);
	}

	// Gère les attributs pour la fonction de fitness
	public double[] generateAttribute(int nbCheckpoints, CollisionShape2D cp, double timeZonelente, int nbMurCogne){
		double[] res = new double[4];
		res[0] = nbCheckpoints;
		res[1] = 10000/cp.Position.DistanceTo(this.Position);
		res[2] = timeZonelente/0.1;
		res[3] = - nbMurCogne;
		return res;
	}

	public bool[] stringArrayToBool(string direction){
		
		bool[] res = new bool[4];
		
		if(direction.Equals("t")) res[2] = true;
		else if(direction.Equals("d")) res[3] = true;
		else if(direction.Equals("r")) res[1] = true;
		else if(direction.Equals("l")) res[0] = true;

		return res;
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
	
	//Collisions avec les murs -> +1 à chaque fois
	public void _on_player_body_entered(Node body)
	{
		if (canDetectCollision)
		{
			nbMurCogne += 1;
			//GD.Print("[###] NB_Mur_cogne = "+nbMurCogne);

			// Désactiver la détection des collisions pour 0.5 seconde
			canDetectCollision = false;
			StartTimer(2);
		}
	}
	
	//Fonction pour attendre avant de redetecter une collision
	private async void StartTimer(float duration)
	{
		await ToSignal(GetTree().CreateTimer(duration), "timeout");

		// Réactiver la détection des collisions
		canDetectCollision = true;
	}
	
}
