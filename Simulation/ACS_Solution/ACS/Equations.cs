using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACS
{
    internal class Equations
    {

        private int bestPosSol;
        private int leastAccSol;
        private int worstSol;
        private double initialPheromone;

        private Random random = new Random();

        //Calculates the known solutions for the graph
        public void calcKnownSolutions(int cols)
        {
            bestPosSol = (cols*cols + 1) / 2 + cols;

            leastAccSol = (cols*cols + 3*cols) / 2 +2;

            worstSol = (3*cols*cols + 5) / 2;
        }

        public int randPath(int min, int max)
        {
            return random.Next(min, max);
        }


        public int[] greedyPath(Nodes[,] nodes)//Creates the greedy path for the intial pheromone levels
        {
            int rows = nodes.GetLength(0);
            int cols = nodes.GetLength(1);

            int[] path = new int[cols];
            path[0] = 0;
            //The shortest path on the first edge is always 3 and greater so on is randomly chosen
            for (int i = 0; i < rows-1; i++)
            {
                if (nodes[0,0].getEdgeCosts(path[0]) > nodes[0,0].getEdgeCosts(i + 1))
                {
                    path[0] = i + 1;
                }else if (nodes[0, 0].getEdgeCosts(path[0]) == nodes[0,0].getEdgeCosts(i + 1))
                {
                    int num = random.Next(0, 2);
                    if (num == 1)
                    {
                        path[0] = i;
                    }
                    else
                    {
                        path[0] = i + 1;
                    }
                }
            }
            int curNode = (int)path[0];
            //i is the current node
            //j is an optional edge to next node
            for (int i = 1; i < cols; i++)
            {
                for (int j = 0; j < rows - 1; j++)//starts at 1 because the first edge is already set
                {
                    //if the next node option is less than the current node option, that edge is chosen
                    if (nodes[curNode, i].getEdgeCosts(j) < nodes[curNode, i].getEdgeCosts(j + 1))
                    {
                        //next node is set to the edge that is less than the one selected in path[] and updates curNode for the next iteration
                        path[i] = j;
                        //Console.WriteLineLine("Path: " + path[i] + " CurNode j: " + curNode);
                    }
                    //If the edges are equal, a random edge is chosen
                    else if (nodes[curNode, i].getEdgeCosts(path[i]) == nodes[curNode, i].getEdgeCosts(j + 1))
                    {
                        int num = random.Next(0, 2);
                        if (num == 1)
                        {
                            path[i] = j;
                        }
                        else
                        {
                            path[i] = j + 1;
                        }
                    }
                }
                //Updates the current node to the next node in the path
                curNode = path[i];
            }
            return path;
        }

        public Nodes[,] setInitialPheromones(Nodes[,] graph, int[] greedyPath, double[] lastEdges)
        {
            int rows = graph.GetLength(0);
            int cols = graph.GetLength(1);
            initialPheromone = 1 / ((cols + 1) * calcSolutionCost(graph, greedyPath, lastEdges));

            //Used to set the initial pheromone levels for all nodes and reduce loops
            
            for(int i = 0; i < rows; i++)
            {
                graph[0, 0].setPheromone(i, initialPheromone);//First node is updated
            }
           
            for (int i = 1; i < cols; i++)
            {
                
                for (int j = 0; j < rows; j++)
                {
                   for(int k = 0; k < rows; k++)
                    {
                        graph[j, i].setPheromone(k, initialPheromone);
                    }
                }
            }
            return graph;
        }

        public double calcSolutionCost(Nodes[,] graph, int[] path, double[] lastEdges)//Used to calculate the costs of all solutions
        {
            int rows = graph.GetLength(0);
            int cols = graph.GetLength(1);

            double cost = 0;
            //Gets the cost of the first edge
            cost += graph[0, 0].getEdgeCosts(path[0]);

            //Gets the cost of all edges between the start and end points
            for (int i = 1; i < cols; i++)
            {
                cost += graph[path[i - 1], i].getEdgeCosts( path[i] );
            }

            //This if-else statement is used to get the cost of the last edge
            cost += lastEdges[ path[cols - 1]];

            return cost;
        }

        public double[] calcPathStrengths(double[] edgeCosts, double[] pheromones, double alpha, double beta)
        {
            double[] pathStrengths = new double[edgeCosts.Length];

            for (int i = 0; i < edgeCosts.Length; i++)
            {
                pathStrengths[i] = Math.Pow(pheromones[i], alpha) * Math.Pow((1 / (double)edgeCosts[i]), beta);
               //Console.WriteLine($"PS iteration: {i}, pathStrengths contents: {string.Join(", ", pathStrengths)}");
            }
            return pathStrengths;
        }

        public bool exploitOrExplore(double q0)
        {
            if (random.NextDouble() <= q0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public int selectStrongestPath(double[] pathStrengths)
        {
            int strongest = 0;
            for(int i = 0; i < pathStrengths.Length-1; i++)
            {
                if(pathStrengths[i] < pathStrengths[i+1])
                {
                    strongest = i+1;

                }else if(pathStrengths[i] == pathStrengths[i+1])
                {
                    int num = random.Next(0, 2);
                    //Console.WriteLine($"Random number: {num}");
                    if(num == 1)
                    {
                        strongest = i+1;
                    }
                }
            }
            //Console.WriteLine($"Strongest path chosen: {strongest}, Strongest value: {pathStrengths[strongest]}");
            return strongest;
        }


        public double[] createRandomPathProbability(double[] pathStrengths)
        {
            double sumOfStrengths = 0;
            ///Calculates the sum of the path strengths
            for (int i = 0; i < pathStrengths.Length; i++)
            {
                sumOfStrengths += pathStrengths[i];
            }
            //Console.WriteLine($"Sum of strengths: {sumOfStrengths}");
            //Calculates the probability of each path which the sum of all probabilities is 1
            for (int i = 0; i < pathStrengths.Length; i++)
            {
                //Console.WriteLine($"Path strength {i} before: {pathStrengths[i]}");
                pathStrengths[i] = pathStrengths[i] / sumOfStrengths;
                //Console.WriteLine($"Path strength {i} after: {pathStrengths[i]}");
            }

            return pathStrengths;
        }

        //If explore is chosen, this method is used to select a random path
        public int selectRandomPath(double[] pathStrengths)
        {

            int selectedPath = 0;

            bool stop = false;

            double rand;

            //Selects a random number between 0 and 1 to decide the path
            rand = random.NextDouble();
            //Console.WriteLine($"Random number for random path: {rand}");

            //This is used to check if the selected number is within the range of the path probabilities
            double check = pathStrengths[0];

            //This loop continues until a path is selected and the stop variable is set to true
            //This works becuase is probability represents and percentage of 1. If the random number is
            //less than the check, the path is selected. If not, the check is updated to the next path probability by 
            //adding the next path probability to the check. This gives each probability its equivalent range of 1.
            for (int i = 0; i < pathStrengths.Length && stop == false; i++)
            {
                //Console.WriteLine($"Cap of range: {check}");
                if (rand < check)
                {
                    selectedPath = i;
                    stop = true;
                }
                else if (i < pathStrengths.Length-1)
                {
                    check += pathStrengths[i+1];
                }
            }

            return selectedPath;
        }

        public double localPheromoneUpdate(double pheromone, double rho)
        {
            pheromone = (1 - rho) * pheromone + rho*initialPheromone;

            return pheromone;
        }

        public Nodes[,] globalPheromoneUpdate(Graph graph, double rho, int[] bestPath, double bestPathLength)
        {
            int rows = graph.getRows();
            int cols = graph.getCols();

            //Updates the pheromones of the first column of edges
            for(int i = 0; i < rows; i++)
            {
                
                graph.getNodes(0,0).setPheromone(
                    i, 
                    //Gets the phermone of the edge and multiplies it by the decay rate
                    graph.getNodes(0,0).getPheromone(i) * (1.0 - rho));
                   
            }

            //Updates the pheromones of the rest of the edges
            for(int i = 1; i < cols; i++)
            {
                for(int j = 0; j < rows; j++)
                {
                    for(int k = 0; k < rows; k++)
                    {
                        graph.getNodes(j, i).setPheromone(
                            k,
                            //Gets the phermone of the edge and multiplies it by the decay rate
                            graph.getNodes(j, i).getPheromone(k) * (1.0 - rho));
   
                    }
                }
            }

            //Updates the pheromones of the best path so far
            //Updates the first edge of the best path
            graph.getNodes(0, 0).setPheromone(
                bestPath[0], 
                //Gets the phermone of the edge and adds the inverse of the best path length multiplied by the decay rate.
                graph.getNodes(0, 0).getPheromone(bestPath[0])  + (rho * (1.0/bestPathLength)));

            for(int i = 1; i < cols; i++)
            {

                graph.getNodes(bestPath[i-1], i).setPheromone(
                    bestPath[i],
                    //Gets the phermone of the edge and adds the inverse of the best path length multiplied by the decay rate.
                    graph.getNodes(bestPath[i-1], i).getPheromone(bestPath[i]) + (rho * (1.0 / bestPathLength)));
                
            }

            return graph.getGraph();
        }

        public int getBestPosSol()
        {
            return bestPosSol;
        }
        public int getLeastAccSol()
        {
            return leastAccSol;
        }
        public int getWorstSol()
        {
            return worstSol;
        }
    }
}
