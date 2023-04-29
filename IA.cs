using Godot;
using System;
using System.Collections;

public class IA{

   public const int NB_POP = 5;

   public double ancienDist;

   public int ancienMov;

   public int actualIndividu;

   public int nbGen;

	private Individu[] population;

	private class Individu{

     public const int NB_HEURISTIQUE = 4;
        public double[] heuristique;
        public double[] ponderation;
        public double fitness;
		public float time;

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

		public void hybridation(Individu ind,int alea){
			Random rnd = new Random();
			int value = rnd.Next(NB_HEURISTIQUE);
			for(int i=value; i<NB_HEURISTIQUE; i++){
				this.heuristique[i] = ind.heuristique[i];
			}
			if(rnd.Next(alea) == 0){
				this.heuristique[rnd.Next(NB_HEURISTIQUE)] = rnd.NextDouble();
			};
		}

	}

	public IA(){
		this.population = new Individu[NB_POP];
        this.ancienDist = 9999999999999999;
        this.ancienMov = 2;
		this.actualIndividu = 0;
		this.nbGen = 0;
		init();
	}

	public void init(){
		for(int i=0; i<NB_POP; i++){
			this.population[i] = new Individu();
		}
	}

    public bool[] launch(Mario_Kart_du_Bled mario, Vector2 settings,int checkpass,RayCast2D s1,RayCast2D s2,RayCast2D s3,Vector2 pos1, Vector2 pos2, float time){
        bool[] tab = train(mario,settings,checkpass,s1,s2,s3,pos2,pos1,time);
        return tab;
    }


	public bool[] train(Mario_Kart_du_Bled mario,Vector2 settings,int nbCheckpoints,RayCast2D s1,RayCast2D s2,RayCast2D s3, Vector2 pos1, Vector2 pos2, float time){
		Random rnd = new Random();
		bool[] tab = new bool[4];
		if(nbCheckpoints >= 1){
			for(int i=0; i<Individu.NB_HEURISTIQUE; i++){
				tab[i] = false;
			}
			if(actualIndividu == 0){
				this.population[actualIndividu].time = time;
			} else {
				this.population[actualIndividu].time = time - this.population[actualIndividu-1].time;
			}
			if(actualIndividu == NB_POP-1){
				this.actualIndividu = 0;
				this.evolution();
			} else {
				this.actualIndividu ++;
			}
			mario.setAll_Passed(new bool[]{false,false,false,false});
		} else {
			for(int i=0; i<Individu.NB_HEURISTIQUE; i++){
				tab[i] = this.population[actualIndividu].heuristique[i] > rnd.NextDouble();
			}
		}
		return tab;
	}

	int ComparerIndividu(Individu x, Individu y)
{
    if (x.time > y.time)
        return 1;
    else if (x.time < y.time)
        return -1;
    else
        return 0;
}

	public void evolution(){
		Array.Sort(population, ComparerIndividu);
		GD.Print("--------TIME GENERATION " + nbGen + "--------");
		foreach (Individu i in population){
			GD.Print(i.time);
		}
		for(int i=1; i<NB_POP; i++){
			this.population[i].hybridation(this.population[0],20);
		}
		GD.Print("[+] New  heuristique vector for pop :");
		for(int i=0; i<NB_POP; i++){
			String tmp = "";
			for(int j=0; j<Individu.NB_HEURISTIQUE; j++){
				tmp += this.population[i].heuristique[j] + ";";
			}
			GD.Print(tmp);
		}
		this.nbGen++;
	}

}
