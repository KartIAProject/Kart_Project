using Godot;
using System;

public class IA{

   public const int NB_POP = 100;

    private Individu[] population;

    private class Individu{

     public const int NB_HEURISTIQUE = 4;

        private double[] heuristique;
        private double[] ponderation;
        private double fitness;

        public Individu(){
            this.heuristique = new double[NB_HEURISTIQUE];
            this.ponderation = new double[NB_HEURISTIQUE];
            init();
            calculFitness();
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
        init();
    }

    public void init(){
        for(int i=0; i<NB_POP; i++){
            this.population[i] = new Individu();
        }
    }

    public bool[] launch(Vector2 settings){
        bool[] tab = train(settings);
        updateGeneration();
        return tab;
    }

    public bool[] train(Vector2 settings){
        bool[] tab = new bool[4];
        double speed = Math.Sqrt(settings.x*settings.x + settings.y*settings.y);
        GD.Print(speed);
        tab[0] = true;
        tab[2] = true;
        return tab;
    }

    public void updateGeneration(){
        return;
        //Array.Sort(this.population,new IndividuComparer);
    }

}