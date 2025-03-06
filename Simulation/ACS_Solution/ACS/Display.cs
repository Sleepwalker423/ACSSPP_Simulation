using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACS
{
    internal class Display()
    {
        public void displayResults(int bestPos, double bestSol, int[] bestPath, int testCount, int iterationCount, int totAnts, int maxTest, int maxIterations, int[][] AntColony, int rows, int cols)
        {
            Console.Clear(); // Clear the console
            Console.WriteLine($"\rRows: {rows}, Columns: {cols}, Total Ants: {totAnts}");

            Console.WriteLine($"\rTest {testCount + 1} of {maxTest}");

            Console.WriteLine($"\rIterations {iterationCount} of {maxIterations}");

            Console.WriteLine($"\rBest Possible Solution: {bestPos}");

            Console.WriteLine($"\rBest Solution found: {bestSol}");

            Console.WriteLine($"\rbestPath found: {string.Join(", ", bestPath)}");
            for(int i = 0; i < totAnts; i++)
            {
                Console.WriteLine($"\rAnt {i} Path: {string.Join(", ", AntColony[i])}");  
            }

            Console.WriteLine("\r");  
        }
    }
}
