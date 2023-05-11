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

	private IA Ia = new IA();
	private newIA Ia2 = new newIA();
	
	private float AccelerationInit, AccelerationZL;

    private RayCast2D s1;
    private RayCast2D s2;
    private RayCast2D s3;
	private RayCast2D s4;
	private RayCast2D s5;
	private RayCast2D s6;
	private RayCast2D s7;
	private RayCast2D s8;

	public int actualCheckpoint;
	public int actualIndexIndividu;

	public int actualIndexOfIndividu;

	public List<string> actualIndividu;

	private CollisionShape2D[] checkpointTab;

	private Population pop;

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
        s1 = GetNode<RayCast2D>("/root/Mario_Kart_du_Bled/player/sensors/s1");
        s2 = GetNode<RayCast2D>("/root/Mario_Kart_du_Bled/player/sensors/s2");
        s3 = GetNode<RayCast2D>("/root/Mario_Kart_du_Bled/player/sensors/s3");
		s4 = GetNode<RayCast2D>("/root/Mario_Kart_du_Bled/player/sensors/s4");
		s5 = GetNode<RayCast2D>("/root/Mario_Kart_du_Bled/player/sensors/s5");
		s6 = GetNode<RayCast2D>("/root/Mario_Kart_du_Bled/player/sensors/s6");
		s7 = GetNode<RayCast2D>("/root/Mario_Kart_du_Bled/player/sensors/s7");
		s8 = GetNode<RayCast2D>("/root/Mario_Kart_du_Bled/player/sensors/s8");
        //GetChild<Area2D>(4).GetChild<CollisionShape2D>(0).Position = new Vector2(pos1,pos2);

		CollisionShape2D cs1 = GetNode<Area2D>("/root/Mario_Kart_du_Bled/checkpoint1").GetChild<CollisionShape2D>(0);
		CollisionShape2D cs2 = GetNode<Area2D>("/root/Mario_Kart_du_Bled/checkpoint2").GetChild<CollisionShape2D>(0);
		CollisionShape2D cs3 = GetNode<Area2D>("/root/Mario_Kart_du_Bled/checkpoint3").GetChild<CollisionShape2D>(0);
		CollisionShape2D cs4 = GetNode<Area2D>("/root/Mario_Kart_du_Bled/checkpoint4").GetChild<CollisionShape2D>(0);
		CollisionShape2D cs5 = GetNode<Area2D>("/root/Mario_Kart_du_Bled/finish line").GetChild<CollisionShape2D>(0);
		checkpointTab = new CollisionShape2D[]{cs1,cs2,cs3,cs4,cs5};

		actualCheckpoint = 0;

		actualIndexIndividu = 0;

		actualIndexOfIndividu = 0;

		actualIndividu = new List<string>();

		pop = new Population(10,500,1,2);
		pop.generatePopulation();

		/*var neatGenomeFactory = new NeatGenomeFactory(12, 4);
		var genomeList = neatGenomeFactory.CreateGenomeList(100, 0);

		var neatParameters = new NeatEvolutionAlgorithmParameters
		{
			SpecieCount = 100
		};

		var distanceMetric = new ManhattanDistanceMetric();
		var speciationStrategy = new KMeansClusteringStrategy<NeatGenome>
			(distanceMetric);

		var complexityRegulationStrategy = new NullComplexityRegulationStrategy();

		var network = new NeatEvolutionAlgorithm<NeatGenome>
			(neatParameters, speciationStrategy, complexityRegulationStrategy);

		var activationScheme = NetworkActivationScheme.CreateCyclicFixedTimestepsScheme(1);
		var genomeDecoder = new NeatGenomeDecoder(activationScheme);

		var phenomeEvaluator = new YourPhenomeEvaluator();
		var genomeListEvaluator = 
			new ParallelGenomeListEvaluator<NeatGenome, IBlackBox>
				(genomeDecoder, phenomeEvaluator);

		network.Initialize(genomeListEvaluator, neatGenomeFactory, genomeList);*/
		//Ia2.init();
    }
    public override void _PhysicsProcess(float delta)
    {   
        s1.ForceRaycastUpdate();
        s2.ForceRaycastUpdate();
        s3.ForceRaycastUpdate();
		s4.ForceRaycastUpdate();
		s5.ForceRaycastUpdate();
		s6.ForceRaycastUpdate();
		s7.ForceRaycastUpdate();
		s8.ForceRaycastUpdate();

        int nbCheckpoints = GetParent<Mario_Kart_du_Bled>().getNbCheckpoints();
		float time = GetParent<Mario_Kart_du_Bled>().getTime();

		/*if(nbCheckpoints >= 1){
			GetParent<Mario_Kart_du_Bled>().setNbCheckpoints(0);
			this.Position = new Vector2(1232,2517);
		}*/

        //bool[] tab = Ia.launch(this,GetParent<Mario_Kart_du_Bled>(),this.LinearVelocity,nbCheckpoints,s1,s2,s3,cs1.Position,cs2.Position,time);
		RayCast2D[]sens = {s1,s2,s3,s4,s5,s6,s7,s8};
		
		// préparer les entrées pour le réseau de neurones
    	float[] inputs = new float[12];
    	inputs[0] = s1.GetCollisionPoint().DistanceTo(this.Position);
    	inputs[1] = s2.GetCollisionPoint().DistanceTo(this.Position);
		inputs[2] = s3.GetCollisionPoint().DistanceTo(this.Position);
		inputs[3] = s4.GetCollisionPoint().DistanceTo(this.Position);
		inputs[4] = s5.GetCollisionPoint().DistanceTo(this.Position);
		inputs[5] = s6.GetCollisionPoint().DistanceTo(this.Position);
		inputs[6] = s7.GetCollisionPoint().DistanceTo(this.Position);
		inputs[7] = s8.GetCollisionPoint().DistanceTo(this.Position);
		inputs[8] = this.LinearVelocity.x;
		inputs[9] = this.LinearVelocity.y;
		inputs[10] = this.Position.x;
		inputs[11] = this.Position.y;

    	// prendre une décision avec le réseau de neurones

		if(actualIndexIndividu == 10){
			GD.Print("[+] New Population");
			pop.evoluate();
			actualIndexOfIndividu = 0;
			actualIndexIndividu = 0;
		}
		if(actualIndexOfIndividu == 0){
			actualIndividu = pop.getIndividu(actualIndexIndividu);
		}

		bool[] tab = stringArrayToBool(actualIndividu[actualIndexOfIndividu]);
		actualIndexOfIndividu++;

		if(actualIndexOfIndividu == 500){
			pop.calculateFitness(actualIndexIndividu,nbCheckpoints);
			bool[] temp = new bool[4];
			for(int k=0; k<4; k++){temp[k] = false;}
			GetParent<Mario_Kart_du_Bled>().setAll_Passed(temp);
			GetParent<Mario_Kart_du_Bled>().setNbCheckpoints(0);
			actualIndexOfIndividu = 0;
			GD.Print("[+] New Individu");
			actualIndexIndividu++;
			this.Position = new Vector2(1232,2517);
			this.RotationDegrees = -90;
		}

    	//bool[] keysPressed = launch(inputs,checkpointTab[nbCheckpoints]);
		//GD.Print(nbCheckpoints);
		//bool[] tab = Ia2.launch(this.Position,this.LinearVelocity,sens,checkpointTab[nbCheckpoints],time);
		input(tab);
    }

	public bool[] stringArrayToBool(string direction){
		
		bool[] res = new bool[4];
		
		if(direction.Equals("t")) res[2] = true;
		else if(direction.Equals("d")) res[3] = true;
		else if(direction.Equals("r")) res[1] = true;
		else if(direction.Equals("l")) res[0] = true;

		return res;
	}

	public bool[] launch(float[] inputs, CollisionShape2D checkpoint){
	
	NeuralNetwork nn = new NeuralNetwork(new int[]{12, 2, 1, 4}); // initialisation d'un réseau de neurones avec 3 entrées, 2 couches cachées, 4 sorties et 1 neurone de biais
		float[] outputs = nn.FeedForward(inputs); // alimenter le réseau avec les entrées et récupérer les sorties

		GD.Print(outputs.Length);

		bool[] result = new bool[4]; // initialisation d'un tableau de booléens pour stocker les résultats
		result[0] = outputs[0] > 0.5f; // la première sortie détermine si la voiture doit tourner à gauche
		result[1] = outputs[1] > 0.5f; // la deuxième sortie détermine si la voiture doit tourner à droite
		result[2] = outputs[2] > 0.5f; 
		result[3] = outputs[3] > 0.5f;

		return result;
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
