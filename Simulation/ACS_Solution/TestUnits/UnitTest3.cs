using ACS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestUnits
{
    [TestClass]
    public class UnitTest3
    {
        double alpha = 1.2;
        double beta = 1.3;
        double[] edgeCosts = { 5, 6, 7, 8, 9 };
        double[] pheromones = { 0.1, 0.2, 0.3, 0.4, 0.5 };

        Equations equations = new Equations();

        [TestMethod]//Test Case 4.1: Tests the selection process of exploit or explore
        public void testExploitOrExplore()
        {
            double exploit = 2;
            double explore = -0.1;
            Assert.IsTrue(equations.exploitOrExplore(exploit));
            Assert.IsFalse(equations.exploitOrExplore(explore));

        }

        [TestMethod]//Test Case 4.2: Tests the calculation of initial product of pheromones and edge costs
        public void testCalcPathStrengths()
        {


            double[] expected = { (Math.Pow((1.0 / 5.0), beta) * Math.Pow(0.1, alpha)), (Math.Pow((1.0 / 6.0), beta) * Math.Pow(0.2, alpha)), (Math.Pow((1.0 / 7.0), beta) * Math.Pow(0.3, alpha)), (Math.Pow((1.0 / 8.0), beta) * Math.Pow(0.4, alpha)), (Math.Pow((1.0 / 9.0), beta) * Math.Pow(0.5, alpha)) };

            double[] actual = equations.calcPathStrengths(edgeCosts, pheromones, alpha, beta);

            CollectionAssert.AreEqual(expected, actual);

        }

        [TestMethod]// Test Case 4.3: Tests the selection of the strongest path
        public void testSelectStrongestPath()
        {
            
            int expected = 4;

            double[] pathStrengths = equations.calcPathStrengths(edgeCosts, pheromones, alpha, beta);

            int actual = equations.selectStrongestPath(pathStrengths);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]// Test Case 4.4: Tests the creation of random path probability
        public void testCreateRandomPathProbability()
        {
            double[] pathStrengths = equations.calcPathStrengths(edgeCosts, pheromones, alpha, beta);

            pathStrengths = equations.createRandomPathProbability(pathStrengths);

            double sum = 0;

            for(int i = 0; i < pathStrengths.Length; i++)
            {
                sum += pathStrengths[i];
            }

            Assert.AreEqual(1, sum);

        }

        [TestMethod]// Test Case 4.5: Tests the selection of a random path
        public void testSelectRandomPath()
        {
            double[] pathStrengths = { -1, -1, -1, -1, 5 };

            int actual = equations.selectRandomPath(pathStrengths);

            Assert.AreEqual(4, actual);
        }
        
    }
}
