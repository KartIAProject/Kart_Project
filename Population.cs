using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

class Population{

    // Tableau qui contient tous les set de mouvements
    // t: top, b: bottom, r: right, l: left
    public static string[] functionsArray = {"1t","2t","4t","8t","1b","2b","4b","8b","1r","2r","4r","8r","1l","2l","4l","8l"};
    
    // Taille de la population
    private int taillePopulation;

    // Taille d'un vecteur pour un individu
    private int tailleIndividu;

    // Nombre de cycleMax
    private int nbCycleMax;

    // Taille des Groupes pour la séléctions par tournois
    private int tailleGroupe;

    // Tableau contenant tous les individus
    private string[,] individusArray;

    // Tableau contenant le fitness de l'individu i à la ième case
    private double[] fitness;

    public Population(int taillePopulation,int tailleIndividu, int nbCycleMax, int tailleGroupe){
        this.taillePopulation = taillePopulation;
        this.tailleIndividu = tailleIndividu;
        this.nbCycleMax = nbCycleMax;
        this.tailleGroupe = tailleGroupe;
        this.individusArray = new string[taillePopulation,tailleIndividu];
    }

    public void generatePopulation(){
        Random rnd = new Random();
        for(int i=0; i<taillePopulation; i++){
            for(int j=0; j<tailleIndividu; j++){
                individusArray[i,j] = functionsArray[rnd.Next(functionsArray.Length)];
            }
        }
    }

    public void sortPopulation(){
        
        Dictionary<double, string[]> array = new Dictionary<double, string[]>();

        for(int i=0; i<individusArray.Length; i++){
            string[] tmp = new string[tailleIndividu];
            for(int j=0; j<tailleIndividu; j++){
                tmp[j] = individusArray[i,j];
            }
            array.Add(fitness[i],tmp);
        }

        // On trie le tableau pour avoir les meilleurs individus au début
        Dictionary<double, string[]> sortArray  = array.OrderByDescending(x => x.Key).ToDictionary(x => x.Key, x => x.Value);

    }

    public (string[],string[]) generateNewIndividu(){

        Random rnd = new Random();

        Dictionary<double, string[]> selectedIndividus1 = new Dictionary<double, string[]>();
        Dictionary<double, string[]> selectedIndividus2 = new Dictionary<double, string[]>();

        for (int i = 0; i < tailleGroupe; i++){
            int nombreAleatoire1 = rnd.Next(0, tailleIndividu-1);
            int nombreAleatoire2 = rnd.Next(0, tailleIndividu-1);
            string[] tmp1 = new string[tailleIndividu];
            for(int j = 0; j<tailleIndividu; j++){
                tmp1[j] = individusArray[nombreAleatoire1,j];
            }
            string[] tmp2 = new string[tailleIndividu];
            for(int j = 0; j<tailleIndividu; j++){
                tmp2[j] = individusArray[nombreAleatoire2,j];
            }
            selectedIndividus1.Add(fitness[nombreAleatoire1],tmp1);
            selectedIndividus1.Add(fitness[nombreAleatoire1],tmp2);
        }

        KeyValuePair<double,string[]> bestIndividu1 = selectedIndividus1.OrderByDescending(x => x.Key).ToDictionary(x => x.Key, x => x.Value).First();
        KeyValuePair<double,string[]> bestIndividu2 = selectedIndividus1.OrderByDescending(x => x.Key).ToDictionary(x => x.Key, x => x.Value).First();

        int sectionNumber = rnd.Next(functionsArray.Length-1);

        string[] newIndividu1 = new string[tailleIndividu];
        string[] newIndividu2 = new string[tailleIndividu];

        for(int i=0; i<sectionNumber; i++){
            newIndividu1[i] = bestIndividu1.Value[i];
            newIndividu2[i] = bestIndividu2.Value[i];
        }

        for(int i=sectionNumber; i<tailleIndividu; i++){
            newIndividu1[i] = bestIndividu2.Value[i];
            newIndividu2[i] = bestIndividu1.Value[i];
        }

        // TODO: Mutation

        return (newIndividu1,newIndividu2);

    }

    public void evoluate(){

        string[,] newIndividusArray = new string[taillePopulation,tailleIndividu];
        
        // On ajoute le meilleur Individu 
        for(int i=0; i<tailleIndividu; i++){
            newIndividusArray[0,i] = individusArray[0,i];
        }

        for(int i=1; i<Convert.ToInt32((taillePopulation/2)+1); i+=2){
            string[] individu1;
            string[] individu2;
            (individu1, individu2) = this.generateNewIndividu();
            for(int j=0; i<tailleIndividu; j++){
                newIndividusArray[i,j] = individu1[j];
                newIndividusArray[i+1,j] = individu2[j];
            }
        }

        for(int i=0; i<taillePopulation; i++){
            for(int j=0; j<tailleIndividu; j++){
                individusArray[i,j] = newIndividusArray[i,j];
            }
        }

    }

    public List<string> functionsArrayToArrayString(string[] func){

        List<string> res = new List<string>();

        for(int i=0; i<functionsArray.Length; i++){
            string tmp = func[i];
            int value = (int)Char.GetNumericValue(tmp[0]);
            string direction = tmp[1] + "";
            for(int j=0; j<value; j++){
                res.Add(direction);
            }
        }

        return res;

    }

    public void calculateFitness(int index){
        // TODO
        fitness[index] = 2;      // Temporaire  
    }

    public List<string> getIndividu(int index){
        
        string[] tmp = new string[tailleIndividu];

        for(int i=0; i<tailleIndividu; i++){
            tmp[i] = individusArray[index,i]; // OOB mais jsp pq
        }

        return functionsArrayToArrayString(tmp);

    }

}