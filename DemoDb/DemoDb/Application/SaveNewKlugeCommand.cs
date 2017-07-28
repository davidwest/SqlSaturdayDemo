namespace DemoDb.Application
{
    public class SaveNewKlugeCommand
    {
        public Kluge NewKluge { get; set; }
    }

    public interface ISaveNewKlugeCommandHandler
    {
        int Execute(SaveNewKlugeCommand cmd);
    }
}
