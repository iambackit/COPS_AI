namespace Assets.Scripts.Interfaces
{
    public interface IStatisticMeasurable
    {
        int ActualPopulation { get;}
        int ActualGeneration { get;}
        int BestFitness { get; }
    }
}
