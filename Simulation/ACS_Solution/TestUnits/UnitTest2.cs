using ACS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestUnits
{
    [TestClass]
    //This class is used to test the equations class
    public class UnitTest2
    {
        //Initializes the nodes with 5 edges and the array for the test graph as 5 x 5
        Nodes nodes = new Nodes(5);
        Nodes[,] testGraph = new Nodes[5, 5];

        int[] expectedGreedyPath = { 2, 2, 2, 2, 2 };

        int[] actualGreedyPath = new int[5];

        double[] testLastColumn = { 5, 6, 1, 9, 10 };

        Equations equations = new Equations();

        [TestMethod]//Test Case 2: Tests the greedy path
        public void testGreedyPath()
        {
            createTestGraph();
            actualGreedyPath = equations.greedyPath(testGraph);

            //Checks the rest of the edges
            for(int i = 1; i < 5; i++)
            {
                Assert.AreEqual(expectedGreedyPath[i], actualGreedyPath[i], $"Path {i} is incorrect.");
            }

        }

        [TestMethod]//Test Case 3: Tests the initial pheromone levels
        public void testSetInitialPheromone()
        {
            double expectPheromone = 1.0/(6.0*6.0);

            createTestGraph();

            testGraph = equations.setInitialPheromones(testGraph, expectedGreedyPath, testLastColumn);

            //This loop checks the pheromone levels of the test graph
            for(int i=0; i<5; i++)
            {
                Assert.AreEqual(expectPheromone, testGraph[0, 0].getPheromone(i), $"The test failed on node( 0, 0) edge {i}.");
            }
            for(int i=1; i < 5; i++)
            {
                for(int j = 0; j < 5; j++)
                {
                    for(int k = 0; k < 5; k++)
                    {
                        Assert.AreEqual(expectPheromone, testGraph[j, i].getPheromone(k), $"The test failed on node({j},{i}) edge {k}.");
                    }
                }
            }
        }

        [TestMethod]//Test Case 6: Tests the local pheromone update
        public void testLocalPheromoneUpdate()
        {
            double rho = 0.5;
            double initialPheromone = 1.0 / (6.0 * 6.0);
            double pheromone = 0.5;
            double expectedPheromone = (1 - rho) * .5 + rho * initialPheromone;

            createTestGraph();

            testGraph = equations.setInitialPheromones(testGraph, expectedGreedyPath, testLastColumn);

            pheromone = equations.localPheromoneUpdate(pheromone, rho);

            Assert.AreEqual(expectedPheromone, pheromone);

        }



        [TestMethod]//Test Case 5: Tests the global pheromone update
        public void testGlobalPheromoneUpdate()
        {
            double rho = 0.5;
            double initialPheromone = 1.0 / (6.0 * 6.0);
            double bestPathLength = 6.0;
            int[] bestPath = { 0, 0, 0, 0, 0 };
            double bestPheromone = initialPheromone * (1.0 - rho) + rho * (1 / bestPathLength);
            double nonBestPheromone = initialPheromone * (1 - rho);
            double secBest= 0.13194444444444442;
            double secNonBest = ((initialPheromone * 0.5)) * 0.5;

            Graph graph = new Graph(5, 5);

            createTestGraph();

            graph.setGraph(testGraph);

            graph.setGraph(equations.setInitialPheromones(graph.getGraph(), expectedGreedyPath, testLastColumn));

            graph.setGraph(equations.globalPheromoneUpdate(graph, rho, bestPath, bestPathLength));

            //These loops check the pheromone levels of the test graph, both those included in the best path and those not.
            //Checks the rest of the edges of the first node
            for (int i = 1; i < 5; i++)
            {
                Assert.AreEqual(nonBestPheromone, graph.getNodes(0, 0).getPheromone(i), $"The test failed on node( 0, 0) edge {i}.");
            }
            //Checks the rest of the edges
            for (int i = 1; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    for (int k = 0; k < 5; k++)
                    {
                        if (j != 0 && k != 0)
                        {
                            Assert.AreEqual(nonBestPheromone, graph.getNodes(j, i).getPheromone(k), $"The test failed on node( {j}, {i}) edge {k}.");
                        }
                    }
                }

            }
            //Checks the edges of the best path
            for (int i = 0; i < 5; i++)
            {
                Assert.AreEqual(bestPheromone, graph.getNodes(0, i).getPheromone(0), $"The test failed on node( 0, {i}) edge 0.");

            }

            graph.setGraph(equations.globalPheromoneUpdate(graph, rho, bestPath, bestPathLength));


            //These loops check the pheromone levels of the test graph after a second update, both those included in the best path and those not.

            //Checks the first node edges not included in the best path
            for (int i = 1; i < 5; i++)
            {
                Assert.AreEqual(secNonBest, graph.getNodes(0, 0).getPheromone(i), $"The test failed on node( 0, 0) edge {i}.");
            }
            //Checks the rest of the node edges not included in the best path
            for (int i = 1; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    for (int k = 0; k < 5; k++)
                    {
                        if (j != 0 && k != 0)
                        {
                            Assert.AreEqual(secNonBest, graph.getNodes(j, i).getPheromone(k), $"The test failed on node( {j}, {i}) edge {k}.");
                        }
                    }
                }

            }
            //Checks the edges of the best path
            for (int i = 0; i < 5; i++)
            {
                Assert.AreEqual(secBest, graph.getNodes(0, i).getPheromone(0), $"The test failed on node( 0, {i}) edge 0.");

            }
        }

        //Creates a test graph with known greedy path
        public void createTestGraph()
        {
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    //initializes the nodes in the graph
                    testGraph[j, i] = new Nodes(5);

                    for (int k = 0; k < 5; k++)
                    {
                        if (j == 2 && k == 2)
                        {
                            testGraph[j, i].setEdgeCosts(k, 1);

                        }else if(i == 0 && j == 0 && k == 2)
                        {
                            testGraph[j, i].setEdgeCosts(k, 1);
                            
                        }else
                        {
                            testGraph[j, i].setEdgeCosts(k, 5);
                        }
                    }
                }
            }
        }
        //Used to check the first edge of the test graph
        public void inRange(double num, int min, int max)
        {
            Assert.IsTrue(num >= min && num <= max);
        }
    }
}
