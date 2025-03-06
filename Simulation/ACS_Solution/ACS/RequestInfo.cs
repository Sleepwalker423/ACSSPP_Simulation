namespace ACS
{
    internal class RequestInput
    {

        private int numRows;
        private int numCols;
        private int totIterations;
        private int totAnts;
        private double alpha;//pheromone importance
        private double beta;//distance importance
        private double rho;//pheromone decay rate
        private double q0;//probability of choosing the best path
        private int totTests;
        private string filename;

        public void requestInput()
        {
            Console.WriteLine("Enter the desired number of rows for the graph:");
            bool isValidInput = false;
            while (!isValidInput)
            {
                string input = Console.ReadLine();
                isValidInput = int.TryParse(input, out numRows) && numRows > 4;
                if (!isValidInput)
                {
                    Console.WriteLine("Invalid input. Please enter a number greater than 4:");
                }
            }

            Console.WriteLine($"Enter the desired number of columns for the graph:");
            isValidInput = false;
            while (!isValidInput)
            {
                string input = Console.ReadLine();
                isValidInput = int.TryParse(input, out numCols) && numCols >= 5 && numCols % 2 != 0;
                if (!isValidInput)
                {
                    Console.WriteLine("Invalid input. Please enter an odd number greater than or equal to 5:");
                }
            }

            Console.WriteLine("Enter the desired number of iterations for the simulation:");
            isValidInput = false;
            while (!isValidInput){
                string input = Console.ReadLine();
                isValidInput = int.TryParse(input, out totIterations) && totIterations > 0;
                if (!isValidInput){
                    Console.WriteLine("Invalid input. Please enter a number greater than 0:");
                }
            }

            Console.WriteLine("Enter the desired number of ants for the simulation:");
            isValidInput = false;
            while (!isValidInput){
                string input = Console.ReadLine();
                isValidInput = int.TryParse(input, out totAnts) && totAnts > 0;
                if (!isValidInput){
                    Console.WriteLine("Invalid input. Please enter a number greater than 0:");
                }
            }

            Console.WriteLine("Enter the desired pheromone importance:");
            isValidInput = false;  
            while (!isValidInput){
                string input = Console.ReadLine();
                isValidInput = double.TryParse(input, out alpha) && alpha > 0;
                if (!isValidInput){
                    Console.WriteLine("Invalid input. Please enter a number greater than 0:");
                }
            }

            Console.WriteLine("Enter the desired distance importance:");
            isValidInput = false;
            while (!isValidInput){
                string input = Console.ReadLine();
                isValidInput = double.TryParse(input, out beta) && beta > 0;
                if (!isValidInput){
                    Console.WriteLine("Invalid input. Please enter a number greater than 0:");
                }
            }

            Console.WriteLine("Enter the desired pheromone decay rate:");
            isValidInput = false;
            while (!isValidInput){
                string input = Console.ReadLine();
                isValidInput = double.TryParse(input, out rho) && rho > 0;
                if (!isValidInput){
                    Console.WriteLine("Invalid input. Please enter a number greater than 0:");
                }
            }

            Console.WriteLine("Enter the desired probability of choosing the best path, between 0 and 1:");
            isValidInput = false;
            while (!isValidInput){
                string input = Console.ReadLine();
                isValidInput = double.TryParse(input, out q0) && q0 > 0 && q0 < 1;
                if (!isValidInput){
                    Console.WriteLine("Invalid input. Please enter a number between 0 and 1:");
                }
            }

            Console.WriteLine("Enter the desired number of tests for the simulation:");
            isValidInput = false;
            while (!isValidInput){
                string input = Console.ReadLine();
                isValidInput = int.TryParse(input, out totTests) && totTests > 0;
                if (!isValidInput){
                    Console.WriteLine("Invalid input. Please enter a number greater than 0:");
                }
            }

            Console.WriteLine("Enter the desired filename for the experiment:");
            filename = Console.ReadLine();
        }

        public int getNumRows()
        {
            return numRows;
        }
        public int getNumCols()
        {
            return numCols;
        }
        public int getTotIterations()
        {
            return totIterations;
        }
        public int getTotAnts()
        {
            return totAnts;
        }
        public double getAlpha()
        {
            return alpha;
        }
        public double getBeta()
        {
            return beta;
        }
        public double getRho()
        {
            return rho;
        }
        public double getQ0()
        {
            return q0;
        }
        public int getTotTests()
        {
            return totTests;
        }
        public string getFilename()
        {
            return filename;
        }

    }
}
