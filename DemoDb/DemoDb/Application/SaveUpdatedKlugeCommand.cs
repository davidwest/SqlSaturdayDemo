namespace DemoDb.Application
{
    public class SaveUpdatedKlugeCommand
    {
        public Kluge UpdatedKluge { get; set; }
    }

    public interface ISaveUpdatedKlugeCommandHandler
    {
        void Execute(SaveUpdatedKlugeCommand cmd);
    }
}
