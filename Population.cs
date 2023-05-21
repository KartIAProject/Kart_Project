using Godot;
using System;
using System.Collections.Generic;

class Population{

	public class Individu {

		// Le score attribué à un individu        
		public double score;

		// Le tableau des déplacements fait par l'individu
		public string[] vecteurDeplacement;

		// La taille de l'individu
		public int tailleIndividu;

		// Premier constructeur pour les nouveaux individus
		public Individu(int tailleIndividu){
			this.tailleIndividu = tailleIndividu;
			this.vecteurDeplacement = new string[tailleIndividu];
			this.score = 0;
			this.generateRandomIndividu();
		}

		// Deuxième constructeur pour les individus après cross-over pour éviter les copies d'adresse
		public Individu(int tailleIndividu,string[] vecteurDeplacement){
			this.tailleIndividu = tailleIndividu;
			this.score = 0;
			this.vecteurDeplacement = vecteurDeplacement;
		}


		// Génère le vecteur de déplacement d'un individu
		private void generateRandomIndividu(){
			Random rnd = new Random();
			for(int i=0; i<tailleIndividu; i++){
				vecteurDeplacement[i] = functionsArray[rnd.Next(functionsArray.Length-1)];
			}
		}

	}

	// Tableau qui contient tous les set de mouvements
	// t: top, b: none, r: right, l: left
	// Nous avons désactivé le fait de pouvoir reculé pour l'individu car cela nous faisait perdre trop de temps
	// A la place l'individu n'effectue aucune action, mais avec l'inertie de la voiture il continue d'avancer
	// 4 variantes par possibilités car obtenir 8 fois top serait trop long sinon donc directement implémenté dans le tableau
	public static string[] functionsArray = {"1t","2t","4t","8t","1b","2b","4b","8b","1r","2r","4r","8r","1l","2l","4l","8l"};
	
	// Taille de la population
	private int taillePopulation;

	// Taille d'un vecteur pour un individu
	private int tailleIndividu;

	// Taille des Groupes pour la séléctions par tournois
	private int tailleGroupe;

	// Tableau contenant tous les individus
	private Individu[] individusArray;

	// Le meilleur individu
	private Individu bestIndividu;

	public Population(int taillePopulation,int tailleIndividu, int tailleGroupe){
		this.taillePopulation = taillePopulation;
		this.tailleIndividu = tailleIndividu;
		this.tailleGroupe = tailleGroupe;
		this.individusArray = new Individu[taillePopulation];
		this.bestIndividu = new Individu(tailleIndividu);
		bestIndividu.score = 0;
	}

	// Génèration d'une population à partir de nouveaux individus
	public void generatePopulation(){
		for(int i=0; i<taillePopulation; i++){
			individusArray[i] = new Individu(tailleIndividu);
		}
	}

	// Récupère le meilleur individu afin de le garder en mémoire dans l'attribut bestIndividu
	public void getBestIndividu(){

		double maxScore = bestIndividu.score;
		for(int i=0; i<taillePopulation; i++){
			if(individusArray[i].score > maxScore){
				maxScore = individusArray[i].score;
				bestIndividu = new Individu(tailleIndividu);
				bestIndividu.vecteurDeplacement = individusArray[i].vecteurDeplacement;
				bestIndividu.score = individusArray[i].score;
			}
		}

	}

	// Génèration de 2 nouveaux individus avec cross-over, on choisit 2 sous-groupes de taille tailleGroupe, on garde le meilleur de chaque sous-groupes
	// Puis on fait un cross-over entre les 2 meilleurs individus, c'est une sélection par tournois
	public (Individu, Individu) generateNewIndividu()
	{
		Random rnd = new Random();

		Individu best1 = new Individu(tailleIndividu);
		Individu best2 = new Individu(tailleIndividu);

		// On garde les 2 meilleurs individus parmis les 2 sous-groupes
		for (int i = 0; i < tailleGroupe; i++)
		{
			int nombreAleatoire1 = rnd.Next(0, taillePopulation - 1);
			int nombreAleatoire2 = rnd.Next(0, taillePopulation - 1);
			
			if(individusArray[nombreAleatoire1].score > best1.score){
				best1.score = individusArray[nombreAleatoire1].score;
				best1 = individusArray[nombreAleatoire1];
			}

			if(individusArray[nombreAleatoire2].score > best2.score){
				best2.score = individusArray[nombreAleatoire2].score;
				best1 = individusArray[nombreAleatoire2];
			}

		}

		// Initialisation des 2 valeurs d'index pour le cross-over
		int sectionNumber = rnd.Next(functionsArray.Length - 1);
		int sectionNumber2 = rnd.Next(functionsArray.Length - 1);

		// On s'assure de mettre les index dans l'ordre croissant
		if(sectionNumber > sectionNumber2){
			int tmp = sectionNumber;
			sectionNumber2 = sectionNumber;
			sectionNumber = tmp;
		}

		string[] newTab1 = new string[tailleIndividu];
		string[] newTab2 = new string[tailleIndividu];

		// On recopie la première partie non changée
		for (int i = 0; i < sectionNumber; i++)
		{
			newTab1[i] = best1.vecteurDeplacement[i];
			newTab2[i] = best2.vecteurDeplacement[i];
		}

		// On réalise le cross-over
		for (int i = sectionNumber; i < sectionNumber2; i++)
		{
			newTab1[i] = best2.vecteurDeplacement[i];
			newTab2[i] = best1.vecteurDeplacement[i];
		}

		// on recopie la deuxième partie non changée
		for (int i = sectionNumber2; i < tailleIndividu; i++)
		{
			newTab1[i] = best1.vecteurDeplacement[i];
			newTab2[i] = best2.vecteurDeplacement[i];
		}

		// Mutation taux de 10%
		int nbMutations = rnd.Next(50); // On réalise 50 mutations max sur un vecteur de mouvement de taille d'environ 1000 car un simple virage à gauche peut tout changer
		int tauxMutation = 9;

		if(rnd.Next(10) > tauxMutation){
			for(int i=0; i<nbMutations; i++){
				newTab1[rnd.Next(tailleIndividu-1)] = functionsArray[rnd.Next(functionsArray.Length-1)];
				newTab2[rnd.Next(tailleIndividu-1)] = functionsArray[rnd.Next(functionsArray.Length-1)];
			}
		}

		best1.vecteurDeplacement = newTab1;
		best2.vecteurDeplacement = newTab2;

		return (best1, best2);
	}

	// Evolution de la population
	public void evoluate(){
		
		// On met à jour le meilleur individu
		getBestIndividu();
		
		GD.Print(bestIndividu.score);

		Individu individu1;
		Individu individu2;

		Individu[] newIndividusArray = new Individu[taillePopulation];
		
		// On réalise le cross-over entre plusieurs individus pour créer notre nouvelle population
		for(int i=0; i<Convert.ToInt32((taillePopulation))-1; i+=2){
			(individu1, individu2) = this.generateNewIndividu();
			newIndividusArray[i] = individu1;
			newIndividusArray[i+1] = individu2;
		}

		// On ajoute notre meilleur individu à la population pour qu'on ne régresse pas
		// N.B : Le meilleur individu semble sauter des générations parfois, sûrement dû à un problème de physique de collision avec godot, cependant il revient toujours toutes les 2 générations
		newIndividusArray[taillePopulation-1] = new Individu(tailleIndividu, bestIndividu.vecteurDeplacement);

		// On recopie la nouvelle population dans le tableau d'individu
		for(int i=0; i<taillePopulation; i++){
			individusArray[i] = newIndividusArray[i];
		}

	}

	// Transforme le tabeau de mouvement en développant c.a.d si on a 2d il ajoute 2 fois d
	// Exemple:
	//  Input: [2d,5t]
	//  Output: List(d,d,t,t,t,t,t)
	public List<string> functionsArrayToArrayString(string[] func){

		List<string> res = new List<string>();

		for(int i=0; i<func.Length; i++){
			string tmp = func[i];
			int value = (int)Char.GetNumericValue(tmp[0]);
			string direction = tmp[1] + "";
			for(int j=0; j<value; j++){
				res.Add(direction);
			}
		}

		return res;

	}

	// Calcul le score d'un individu
	public void calculateFitness(int index, double[] attr, int[] weight){
		double tmp = 0;
		for(int i=0; i<attr.Length; i++){
			tmp += attr[i]*weight[i];
		}
		GD.Print("[+] Score :" + tmp);
		individusArray[index].score = tmp;      // Temporaire  
	}

	// Retourne l'individu à l'index passé en paramètre
	public List<string> getIndividu(int index){
		return functionsArrayToArrayString(individusArray[index].vecteurDeplacement);
	}

}
