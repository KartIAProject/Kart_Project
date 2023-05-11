using Godot;
using System;
using System.Collections;
using NeuronDotNet.Core;
using NeuronDotNet.Core.Backpropagation;
using SharpNeat.Genomes.Neat;
using SharpNeat.Network;

public class newIA{

    public LinearLayer inputLayer;
    public SigmoidLayer hiddenLayer;
    public SigmoidLayer outputLayer;
    public BackpropagationNetwork network;

    public double lastCheckpoint;
    public float lastCheckpointTime;

    public void init(){
            // Create input layer with 9 neurons
            inputLayer = new LinearLayer(9);

            // Create hidden layer with 10 neurons
            hiddenLayer = new SigmoidLayer(20);

            // Create output layer with 4 neurons
            outputLayer = new SigmoidLayer(4);

            // Connect layers
            new BackpropagationConnector(inputLayer, hiddenLayer).Initialize();
            new BackpropagationConnector(hiddenLayer, outputLayer).Initialize();

            // Create network
            network = new BackpropagationNetwork(inputLayer, outputLayer);

            // Set training parameters
            network.SetLearningRate(0.1);
            //network.SetMomentum(0.9);
            lastCheckpointTime = 0;
            lastCheckpoint = 999999;
    }

    private double[] heuristique(RayCast2D[] sensors,Vector2 playerPos,Vector2 speed){
        double[] res = new double[sensors.Length+1];
        for(int i=0; i<sensors.Length; i++){
            res[i] = (double)sensors[i].GetCollisionPoint().DistanceTo(playerPos);
        }
        res[8] = Math.Sqrt(speed.x*speed.x + speed.y*speed.y);
        return res;
    }

public bool[] launch(Vector2 playerPos, Vector2 speed, RayCast2D[] sensors,CollisionShape2D checkpoint,float time)
{
    double[] h = heuristique(sensors,playerPos,speed);

    // Train network using reinforcement learning
    // Get input values from sensors
    double[] inputValues = h;

    // Run network to get output values
    double[] outputValues = network.Run(inputValues);

    // Calculate reward based on car's actions
    double reward = CalculateReward(outputValues, checkpoint, playerPos, time);

    // Train network using reinforcement learning
    // Create a TrainingSet with appropriate size for input and output vectors
    TrainingSet trainingSet = new TrainingSet(inputValues.Length, outputValues.Length);

    // Create a TrainingSample with inputValues and outputValues
    TrainingSample trainingSample = new TrainingSample(inputValues, outputValues);

    // Update the output values with the reward
    double[] updatedOutputValues = (double[])outputValues.Clone();
    for (int i = 0; i < updatedOutputValues.Length; i++)
    {
        updatedOutputValues[i] += reward;
    }

    // Create a TrainingSample with inputValues and updatedOutputValues
    TrainingSample updatedTrainingSample = new TrainingSample(inputValues, updatedOutputValues);

    // Add the original and updated TrainingSamples to the TrainingSet
    trainingSet.Add(trainingSample);
    trainingSet.Add(updatedTrainingSample);

    // Train network using backpropagation
    network.Learn(trainingSet,50);

    // Determine which action to take based on the updated output values
    bool[] tab = new bool[4];
    double maxOutputValue = updatedOutputValues[0];
    int maxOutputIndex = 0;
    for (int i = 1; i < updatedOutputValues.Length; i++)
    {
        if (updatedOutputValues[i] > maxOutputValue)
        {
            maxOutputValue = updatedOutputValues[i];
            maxOutputIndex = i;
        }
    }
    tab[maxOutputIndex] = true;

    return tab;
}


        private double CalculateReward(double[] outputValues, CollisionShape2D checkpoint, Vector2 playerPos, float time)
        {
            double reward = 71;
            // Ajouter votre logique pour calculer la récompense en fonction des actions de la voiture
            // Par exemple, vous pouvez augmenter la récompense si la voiture atteint un nouveau point de contrôle
            if ((checkpoint.Position).DistanceTo(playerPos) < lastCheckpoint)
            {
                reward += 10;
                lastCheckpoint = (checkpoint.Position).DistanceTo(playerPos);
            }
            else
            {
                reward -= 10;
            }
            // Vous pouvez également pénaliser la voiture si elle prend trop de temps pour atteindre le prochain point de contrôle
            if (time - lastCheckpointTime > 0.5)
            {
                reward -= 10;
                lastCheckpointTime = time;
            }
            // Pénaliser la voiture si elle dépasse une certaine distance par rapport à son dernier checkpoint
            if ((checkpoint.Position).DistanceTo(playerPos) > 300)
            {
                reward -= 10;
            }
            // Pénaliser la voiture si elle va hors-piste ou heurte un obstacle
            double threshold = 0.5;
            if (outputValues[0] < threshold || outputValues[1] < threshold || outputValues[2] < threshold || outputValues[3] < threshold)
            {
                reward -= 20;
            }
            return reward;
        }
}