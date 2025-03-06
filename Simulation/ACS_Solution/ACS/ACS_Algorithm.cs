using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ACS
{
    internal class ACS_Algorithm(int rows, int cols, int totAnts, int maxIterations, double alpha, 
        double beta, double rho, double q0, int maxTests, string filename)
    {
        //intialize the varibales and array
        private double bestSol;
        private int iterationCount;
        private int testCount;
        private int bestCount = 0;
        private int lASCount = 0;
        private int bestPosSol;
        private int[] bestPath;
        private double[] recordedSol = new double[maxTests];
        private int[] recordedIterations = new int[maxTests];
        private int[] iterationsLAS = new int[maxTests];
        private double[] recordedLAS = new double[maxTests];
        private bool lASFound;


        //Create the simulation models
        private AntColony antColonyObj = new AntColony(totAnts, cols);
        private Graph graphObj = new Graph(rows, cols);
        private Equations equationsObj = new Equations();

        //Creates the display and file read/write objects
        private Display displayObj = new Display();
        private FileReadWrite fileObj = new FileReadWrite();

        public void setInitialPheromone()//Sets the initial pheromone levels
        {
            
            //Creates the greedy path
            int[] greedyPath = equationsObj.greedyPath(graphObj.getGraph());

            //Sets the initial pheromone levels by using the greedy path.
            graphObj.setGraph(equationsObj.setInitialPheromones(
                graphObj.getGraph(), 
                greedyPath, 
                graphObj.getLastEdgeColumn()));

        }

        public void runACS()//Runs the algorithm
        {
            //Tracks the number of tests that have been run
            for (int i = 0; i < maxTests; i++)
            {

                iterationCount = 0;

                lASFound = false;

                testCount = i;

                bestPath = new int[cols];

                graphObj.createGraph();

                antColonyObj.createAntColony();

                setInitialPheromone();

                equationsObj.calcKnownSolutions(cols);

                bestSol = equationsObj.getWorstSol()+1;

                bestPosSol = equationsObj.getBestPosSol();

                displayObj.displayResults(bestPosSol, bestSol, bestPath, testCount, iterationCount, totAnts, maxTests, maxIterations, antColonyObj.getAllPaths(), rows, cols);

                //Determines if the termination methods for the current test have been met
                while (iterationCount < maxIterations && bestPosSol < bestSol)
                {
                    //Tracks which ant is traversing the graph
                    for (int j = 0; j < totAnts; j++)
                    {
                        //Records the edges chosen by the ant
                        int[] path = new int[cols];
                        //Hold the results of the path strength calculations that is used later to determine the next edge to traverse
                        double[] pathStrengths = new double[rows];

                        //Tracks the edges traversed by the ant
                        for (int k = 0; k < cols; k++)
                        {
                            //Determines if the ant is at the start of the graph
                            if (k > 0)
                            {
                                //Sets the optional path strength values. The nodes sumbitted to the calcPathStrengths method is
                                //determined by the path array.
                                //With function calls that contain arguments with more funciton calls, the argurments needed
                                //for each method are indented below the method call
                                //to make the code more readable while reducing the need to create additional variables.
                                //This was disgned under the assumption that this could reduce the amount of memory used by the program
                                //and make the tests run faster.
                                pathStrengths = equationsObj.calcPathStrengths(
                                    graphObj.getNodes(path[k - 1], k).getAllEdgeCosts(), 
                                    graphObj.getNodes(path[k - 1], k).getAllPher(), 
                                    alpha,
                                    beta);

                                //Determines if the ant should exploit or explore the graph
                                if (equationsObj.exploitOrExplore(q0))
                                {

                                    path[k] = equationsObj.selectStrongestPath(pathStrengths);
                                }
                                else
                                {
                                    pathStrengths = equationsObj.createRandomPathProbability(pathStrengths);

                                    path[k] = equationsObj.selectRandomPath(pathStrengths);
                                }

                                //Local pheromone update
                                graphObj.getNodes(path[k - 1], k).setPheromone(
                                    path[k],
                                    equationsObj.localPheromoneUpdate(
                                        graphObj.getNodes(path[k - 1], k).getPheromone(path[k]),
                                        rho));

                            }
                            else//The ant is at the start of the graph 
                            {

                                pathStrengths = equationsObj.calcPathStrengths(
                                    graphObj.getNodes(0, 0).getAllEdgeCosts(), 
                                    graphObj.getNodes(0, 0).getAllPher(), 
                                    alpha, 
                                    beta);

                                //Determines if the ant should exploit or explore the graph
                                if (equationsObj.exploitOrExplore(q0))
                                {

                                    path[k] = equationsObj.selectStrongestPath(pathStrengths);
                                }
                                else
                                {
                                    pathStrengths = equationsObj.createRandomPathProbability(pathStrengths);

                                    path[k] = equationsObj.selectRandomPath(pathStrengths);
                                }

                                //Local pheromone update
                                graphObj.getNodes(0, 0).setPheromone(
                                    path[k],
                                    equationsObj.localPheromoneUpdate(
                                        graphObj.getNodes(0, 0).getPheromone(path[k]),
                                        rho));

                            }

                        }

                        //Updates the ants recorded path using the path array
                        antColonyObj.setAllPathsforAnt(j, path);

                        antColonyObj.setDistance(
                            j, 
                            equationsObj.calcSolutionCost(
                                graphObj.getGraph(), 
                                path, 
                                graphObj.getLastEdgeColumn()));

                        if(bestSol > antColonyObj.getDistance(j))
                        {
                            bestSol = antColonyObj.getDistance(j);
                            bestPath = path;
                            
                        }
                        displayObj.displayResults(bestPosSol, bestSol, bestPath, testCount, iterationCount, totAnts, maxTests,
                            maxIterations, antColonyObj.getAllPaths(), rows, cols);

                    }
                    //Iteration count updated
                    iterationCount++;
                    //Global pheromone update
                    graphObj.setGraph(
                        equationsObj.globalPheromoneUpdate(
                            graphObj, 
                            rho, 
                            bestPath, 
                            bestSol));
                    //Displays the results of the current test
                    displayObj.displayResults(bestPosSol, bestSol, bestPath, testCount, iterationCount, totAnts, maxTests,
                        maxIterations, antColonyObj.getAllPaths(), rows, cols);
                    //Checks if the best solution has been found at the end of the test and records the results
                    if(bestPosSol == bestSol)
                    {
                        bestCount++;
                    }
                    if(bestSol <= equationsObj.getLeastAccSol() && lASFound == false)
                    {
                        iterationsLAS[lASCount] = iterationCount;
                        recordedLAS[lASCount] = bestSol;
                        lASCount++;
                        lASFound = true;
                    }
                    Console.WriteLine($"Best Count: {bestCount}");
                }
                recordedSol[i] = bestSol;
                recordedIterations[i] = iterationCount;
   
            }
            //Add recording code here?
            fileObj.CreateExcelFile(filename, maxTests, maxIterations, rows, cols, totAnts, alpha, beta, rho, q0,
                bestCount, recordedSol, recordedIterations, bestPosSol, equationsObj.getLeastAccSol(), equationsObj.getWorstSol(),
                lASCount, iterationsLAS, recordedLAS);
        }

    }
}
