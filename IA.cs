using Godot;
using System;
using System.Collections;

public class IA{

   public const int NB_POP = 100;

   public double ancienDist;

   public int ancienMov;

	private Individu[] population;

	private class Individu{

     public const int NB_HEURISTIQUE = 4;
        public double[] heuristique;
        public double[] ponderation;
        public double fitness;

        public Individu(){
            this.heuristique = new double[NB_HEURISTIQUE];
            this.ponderation = new double[NB_HEURISTIQUE];
            init();
            //calculFitness();
        }

		public void init(){
			Random rnd = new Random();
			for(int i=0; i<NB_HEURISTIQUE; i++){
				this.heuristique[i] = rnd.NextDouble();
				this.ponderation[i] = 0; // TODO A changer
			}
		}

		public void calculFitness(){
			double tmp = 0;
			for(int i=0; i<NB_HEURISTIQUE; i++){
				tmp += this.heuristique[i]*this.ponderation[i];
			}
		}

		public void hybridation(Individu ind){
			Random rnd = new Random();
			int value = rnd.Next(NB_HEURISTIQUE);
			for(int i=value; i<NB_HEURISTIQUE; i++){
				this.heuristique[i] = ind.heuristique[i];
			}
			if(rnd.Next(100) == 1){
				this.heuristique[rnd.Next(NB_HEURISTIQUE)] = rnd.NextDouble();
			};
		}

	}

	public IA(){
		this.population = new Individu[NB_POP];
        this.ancienDist = 9999999999999999;
        this.ancienMov = 2;
		init();
	}

	public void init(){
		for(int i=0; i<NB_POP; i++){
			this.population[i] = new Individu();
		}
	}

    public bool[] launch(Vector2 settings,int checkpass,RayCast2D s1,RayCast2D s2,RayCast2D s3,Vector2 pos1, Vector2 pos2){
        bool[] tab = train(settings,checkpass,s1,s2,s3,pos2,pos1);
        updateGeneration();
        return tab;
    }


	public bool[] train(Vector2 settings,int nbCheckpoints,RayCast2D s1,RayCast2D s2,RayCast2D s3, Vector2 pos1, Vector2 pos2){
		bool[] tab = new bool[4];
        double dist = Math.Sqrt((pos1.x-settings.x)*(pos1.x-settings.x)+(pos1.y-settings.y)*(pos1.y-settings.y));
		double speed = Math.Sqrt(settings.x*settings.x + settings.y*settings.y);
        GD.Print(dist);
        this.ancienDist = dist;
        tab[ancienMov] = true;
		return tab;
	}




	public void updateGeneration(){
		return;
		//Array.Sort(this.population,new IndividuComparer);
	}

}
