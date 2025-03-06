using System.Runtime.InteropServices;

namespace ACS
{
    internal class AntColony(int totAnts, int cols)
    {
        private int[][] antColony = new int[totAnts][];

        private double[] distance = new double[totAnts];

        public void createAntColony()
        {
            for (int i = 0; i < totAnts; i++)
            {
                antColony[i] = new int[cols];
            }
        }

        public int[][] getAllPaths()
        {
            return antColony;
        }
        public void setAllPathsforAnt(int antIndex, int[] path)
        {
            antColony[antIndex] = path;
        }
        public void setDistance(int antIndex, double dist)
        {
            distance[antIndex] = dist;
        }
        public double getDistance(int antIndex)
        {
            return distance[antIndex];
        }
    }
}