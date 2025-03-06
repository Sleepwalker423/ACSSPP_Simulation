using ACS;
namespace TestUnits
{
    [TestClass]
    public class UnitTest1
    {
        //This is created to test the edges that have a range of possible values
        public void inRange(double num, int min, int max)
        {
            Assert.IsTrue(num >= min && num <= max);
        }

        [DataTestMethod]//Test case 1 : Tests the creation of the graph
        [DataRow(6, 7, 8, 6, 14)]
        [DataRow(5, 5, 6, 4, 10)]
        [DataRow(5, 9, 10, 8, 18)]
        public void TestCreateGraph(int rows, int cols, int colAdd1, int colSub1, int colMul2)
        {
 
            Graph graph = new Graph(rows, cols);
            graph.createGraph();

            //The below code checks the first edges of the graph. 
            Assert.AreEqual(cols, graph.getNodes(0, 0).getEdgeCosts(0));
            Assert.AreEqual(2, graph.getNodes(0, 0).getEdgeCosts(1));

            //These for loops check the edges from the first node to all rows greater than 2
            for (int i = 2; i < rows; i++)
            {
                Assert.AreEqual(1, graph.getNodes(0, 0).getEdgeCosts(i));
            }

            //These loops checks the nodes in column zero that dont exist in the graph to ensure they equal 0.
            for (int i = 1; i < rows; i++)
            {
                for (int j = 0; j < rows; j++)
                {
                    Assert.AreEqual(0, graph.getNodes(i, 0).getEdgeCosts(j));
                }
            }

            //The below code checks the even and odd edges of the graph, excluding the last edge, following the rules in the document.
            //i is the edges' column to determine what rule set is being applied
            //j is the node 
            //k is the edge going from node j to the node k in the next column
            //The last column of edges are not checked becuase they are checked in the distance calculation process.
            for (int i = 1; i < cols; i++)
            {

                if (i % 2 != 0)
                { //Even edge columns(this counts odd bc the array starts at 0)
                    for (int j = 0; j < rows; j++)
                    {
                        for (int k = 0; k < rows; k++)
                        {
                            if (j == 0)
                            {//This is the rules set for the 1st node of the even edge columns

                                if (k == 0)
                                {//first edge of node 1

                                    Assert.AreEqual(1, graph.getNodes(j, i).getEdgeCosts(k));

                                }
                                else
                                {//all other edges of node 1

                                    Assert.AreEqual(2, graph.getNodes(j, i).getEdgeCosts(k));
                                }
                            }
                            else if (j == 1 && k < 2)
                            {//first two edges of node 2

                                Assert.AreEqual(colAdd1, graph.getNodes(j, i).getEdgeCosts(k));

                            }
                            else if ((j == 1 && k == 4) || (j == 4 && k == 4))
                            {//5th edge of node 2

                                Assert.AreEqual(colMul2, graph.getNodes(j, i).getEdgeCosts(k));

                            }
                            else
                            {//all other edges

                                inRange(graph.getNodes(j, i).getEdgeCosts(k), colAdd1, colMul2);
                                
                            }
                        }
                    }
                }
                else if (i % 2 == 0)
                {//Odd edge columns(this counts even bc the array starts at 0)
                    for (int j = 0; j < rows; j++)
                    {
                        for (int k = 0; k < rows; k++)
                        {

                            if (j == 0)
                            {//This is the rules set for the first node of the odd edge columns

                                if (k == 0)
                                {//first edge of node 1

                                    Assert.AreEqual(cols, graph.getNodes(j, i).getEdgeCosts(k));

                                }
                                else
                                {//all other edges of node 1

                                    Assert.AreEqual(colSub1, graph.getNodes(j, i).getEdgeCosts(k));
                                }
                            }
                            else if (j != 0 && k == 0)
                            {//the first edge of all nodes greater than 1

                                Assert.AreEqual(colAdd1, graph.getNodes(j, i).getEdgeCosts(k));

                            }
                            else if (j == 1 && k == 1)
                            {//the second edge of node 2

                                Assert.AreEqual(1, graph.getNodes(j, i).getEdgeCosts(k));

                            }
                            else if (j == 4 && k == 4)
                            {//the 5th edge of node 5

                                Assert.AreEqual(colSub1, graph.getNodes(j, i).getEdgeCosts(k));

                            }
                            else
                            {//all other edges

                                inRange(graph.getNodes(j, i).getEdgeCosts(k), 2, colSub1);
                            }

                        }
                    }
                }
            }
        }
        [DataTestMethod]//Test Case 7: Tests the calculation of the known path solutions
        [DataRow(6, 7)]
        [DataRow(5, 5)]
        [DataRow(5, 9)]
        public void testCalcKnownSolutions(int rows, int cols)
        {
            int[] bestPath = new int[cols];
            int[] leastAccPath = new int[cols];
            int[] worstPath = new int[cols];

            Graph graph = new Graph(rows, cols);
            graph.createGraph();

            for (int i = 0; i < cols; i++)
            {
                if(i == 0)
                {
                    worstPath[i] = 1;
                }
                else
                {
                    worstPath[i] = 4;
                }
                bestPath[i] = 0;
                leastAccPath[i] = 1;
            }

            Equations equations = new Equations();
            //These calculate the costs of the known path solutions in the graph
            double actBest = equations.calcSolutionCost(graph.getGraph(), bestPath, graph.getLastEdgeColumn());
            double actLeastAcc = equations.calcSolutionCost(graph.getGraph(), leastAccPath, graph.getLastEdgeColumn());
            double actWorst = equations.calcSolutionCost(graph.getGraph(), worstPath, graph.getLastEdgeColumn());

            //These calculate the costs of the known path solutions in the graph via the tested equations from the method
            equations.calcKnownSolutions(cols);

            //These ensure that the mehtods are successful
            Assert.AreEqual(equations.getBestPosSol(), actBest);
            Assert.AreEqual(equations.getLeastAccSol(), actLeastAcc);
            Assert.AreEqual(equations.getWorstSol(), actWorst);
            
        }

    }
}
