using System;
using Godot;
using System.Collections.Generic;
using SharpNeat.EvolutionAlgorithms;
using SharpNeat.Genomes.Neat;
using SharpNeat.Network;
using SharpNeat.Phenomes;
using System;
using System.Collections.Generic;


public class NeuralNetwork
{
    private int[] layers;
    private float[][] neurons;
    private float[][] biases;
    private float[][][] weights;

    public NeuralNetwork(int[] layers)
    {
        this.layers = layers;
        this.neurons = new float[layers.Length][];
        this.biases = new float[layers.Length][];
        this.weights = new float[layers.Length][][];
        
        for (int i = 0; i < layers.Length; i++)
        {
            this.neurons[i] = new float[layers[i]];
            this.biases[i] = new float[layers[i]];
            
            if (i > 0)
            {
                this.weights[i] = new float[layers[i]][];
                
                for (int j = 0; j < layers[i]; j++)
                {
                    this.weights[i][j] = new float[layers[i - 1]];
                }
            }
        }
        
        Init();
    }

    private void Init()
    {
        Random rand = new Random();
        for (int i = 1; i < layers.Length; i++)
        {
            for (int j = 0; j < layers[i]; j++)
            {
                biases[i][j] = (float)rand.NextDouble() * 2 - 1;//rand.NextDouble(-1f, 1f);
                
                for (int k = 0; k < layers[i - 1]; k++)
                {
                    weights[i][j][k] = (float)rand.NextDouble() * 2 - 1;//rand.NextDouble(-1f, 1f);
                }
            }
        }
    }

    private float ActivationFunction(float x)
    {
        return (float)Math.Tanh(x);
    }

private float Distance(float x1, float y1, float x2, float y2)
{
    float dx = x2 - x1;
    float dy = y2 - y1;
    return (float)Math.Sqrt(dx*dx + dy*dy);
}

public float[] FeedForward(float[] inputs)
{

    // Calcul des sorties du réseau de neurones
    for (int i = 0; i < inputs.Length; i++)
    {
        neurons[0][i] = inputs[i];
    }
        
    for (int i = 1; i < layers.Length; i++)
    {
        for (int j = 0; j < layers[i]; j++)
        {
            float sum = biases[i][j];
                
            for (int k = 0; k < layers[i - 1]; k++)
            {
                sum += weights[i][j][k] * neurons[i - 1][k];
            }
                
            neurons[i][j] = ActivationFunction(sum);
        }
    }
        
    return neurons[layers.Length - 1];
}
}
