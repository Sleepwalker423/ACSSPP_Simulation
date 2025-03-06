namespace ACS;

internal class Nodes
{
    private double[] edgeCosts;
    private double[] pheromones;
    public Nodes(int rows)
    { 
       edgeCosts = new double[rows];// length of edges
        pheromones = new double[rows];//phermone level of edges
    }
    

    //Get single pheromone level
    public double getPheromone(int edge)
    {
        return this.pheromones[edge];
    }
    //Get all pheromone levels
    public double[] getAllPher()
    {
        return this.pheromones;
    }
    //Set all intial pheromone levels 
    public void setInitPheromones(double[] initial)
    {
        this.pheromones = initial;
    }
    //Set single pheromone level
    public void setPheromone(int edge, double value)
    {
        this.pheromones[edge] = value;
    }
    public double getEdgeCosts(int edge)
    {
        return this.edgeCosts[edge];
    }
    public void setEdgeCosts(int edge, int value)
    {
        this.edgeCosts[edge] = value;
    }
    public double[] getAllEdgeCosts()
    {
        return this.edgeCosts;
    }

}
