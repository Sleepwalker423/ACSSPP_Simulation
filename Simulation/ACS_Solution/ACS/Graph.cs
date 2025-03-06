using System.Reflection.Metadata.Ecma335;

namespace ACS
{
    internal class Graph(int rows, int cols)
    {

        private Nodes[,] graph = new Nodes[rows, cols]; // 2D array of nodes that creates the graph

        private Random random = new Random(); // Used to generate random numbers for edge costs

        private double[] lastEdgeColumn = new double[rows]; // Used to store the cost of each edge in the last column

        //This is the random edge generator for the even edge columns in the graph
        public int randomEvenEdges(int col)
        {
            return random.Next(cols + 1, 2 * cols+1);
        }
        //This is the random edge generator for the odd edge columns in the graph
        public int randomOddEdges(int col)
        {
            return random.Next(2, cols);
        }

        public void createGraph()
        {
            //This for loop initializes the nodes of the graph
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    graph[i, j] = new Nodes(rows);
                }
            }

            //The below code creates the first edges of the graph. The first node has edges to all other nodes.
            graph[0, 0].setEdgeCosts(0, cols);
            graph[0, 0].setEdgeCosts(1, 2);

            //This for loop creates the edges from the first node to all rows greater than 2
            for (int i = 2; i < rows; i++)
            {
                graph[0, 0].setEdgeCosts(i, 1);
            }

            //This loop zeros out the nodes in column zero that dont exist in the graph
            for (int i = 1; i < rows; i++)
            {
                for (int j = 0; j < rows; j++)
                {
                    graph[i, 0].setEdgeCosts(j, 0);
                }
            }

            //The below code creates the even and odd edges of the graph, excluding the last edge, following the rules in the document.
            //i is the edges' column to determine what rule set is being applied
            //j is the node 
            //k is the edge going from node j to the node k in the next column
            //The last column of edges are represented by the lastEdgeColumn[] array
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

                                    graph[j, i].setEdgeCosts(k, 1);

                                }
                                else
                                {//all other edges of node 1

                                    graph[j, i].setEdgeCosts(k, 2);
                                }
                            }
                            else if (j == 1 && k < 2)
                            {//first two edges of node 2

                                graph[j, i].setEdgeCosts(k, cols + 1);

                            }
                            else if (j == 1 && k == 4)
                            {//5th edge of node 2

                                graph[j, i].setEdgeCosts(k, cols * 2);

                            }
                            else if (j == 4 && k == 4)
                            {//5th edge of node 5

                                graph[j, i].setEdgeCosts(k, cols * 2);

                            }
                            else
                            {//all other edges

                                graph[j, i].setEdgeCosts(k, randomEvenEdges(cols));
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

                                    graph[j, i].setEdgeCosts(k, cols);

                                }
                                else
                                {//all other edges of node 1

                                    graph[j, i].setEdgeCosts(k, cols - 1);
                                }
                            }
                            else if (j != 0 && k == 0)
                            {//the first edge of all nodes greater than 1

                                graph[j, i].setEdgeCosts(k, cols + 1);

                            }
                            else if (j == 1 && k == 1)
                            {//the second edge of node 2

                                graph[j, i].setEdgeCosts(k, 1);

                            }
                            else if (j == 4 && k == 4)
                            {//the 5th edge of node 5

                                graph[j, i].setEdgeCosts(k, cols - 1);

                            }
                            else
                            {//all other edges

                                graph[j, i].setEdgeCosts(k, randomOddEdges(cols));
                            }

                        }
                    }
                }
            }
            //These set the constant edge values of the last column and the two random edges that will always be used.
            lastEdgeColumn[0] = 1;
            lastEdgeColumn[1] = cols + 1;
            lastEdgeColumn[2] = randomEvenEdges(cols);
            lastEdgeColumn[3] = randomEvenEdges(cols);
            lastEdgeColumn[4] = cols * 2;
            //This loop sets the rest of the last comlumn of edges if there are more than 5 rows.
            if (rows > 5)
            {
                for (int i = 5; i < rows; i++)
                {
                    lastEdgeColumn[i] = randomEvenEdges(cols);
                }
            }
        }
        //Getters and setters for the nodes

  
        public Nodes getNodes(int row, int col)
        {
            return graph[row, col];
        }
        public Nodes[,] getGraph()
        {
            return graph;
        }
        public void setGraph(Nodes[,] newGraph)
        {
            graph = newGraph;
        }
        public double getLastEdgeColumnValue(int edge)
        {
            return lastEdgeColumn[edge];
        }
        public double[] getLastEdgeColumn()
        {
            return lastEdgeColumn;
        }
        public int getRows()
        {
            return rows;
        }
        public int getCols()
        {
            return cols;
        }
    }


}