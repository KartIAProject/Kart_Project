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
			int[] weight = {2500,10,-15};
			pop.calculateFitness(actualIndexIndividu,generateAttribute(nbCheckpoints,checkpointTab[nbCheckpoints],timeZonelente),weight);

			// On remet le temps à 0 ainsi que les checkpoints à 0
			bool[] temp = new bool[4];
			for(int k=0; k<4; k++){temp[k] = false;}
			GetParent<Mario_Kart_du_Bled>().setAll_Passed(temp);
			GetParent<Mario_Kart_du_Bled>().setNbCheckpoints(0);
			GetParent<Mario_Kart_du_Bled>().setTime(0);
			GetParent<Mario_Kart_du_Bled>().setNbZoneLente(0);
			
			// On met l'index du tableau de mouvements à 0
			actualIndexOfIndividu = 0;

			GD.Print("[+] New Individu ["+actualIndexIndividu+"]");
			
			//On incrémente pour passe à l'individu suivant
			actualIndexIndividu++;

			// On repositionne le nouveau individu sur la grille de départ
			this.Position = new Vector2(1232,2517);
			this.RotationDegrees = -90;
		}

		input(tab);
    }

	// Gère les attributs pour la fonction de fitness
	public double[] generateAttribute(int nbCheckpoints, CollisionShape2D cp, float timeZonelente){
		double[] res = new double[3];
		res[0] = nbCheckpoints;
		res[1] = 10000/cp.Position.DistanceTo(this.Position);
		res[2] = timeZonelente/0.1;
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
	
	
	
}
