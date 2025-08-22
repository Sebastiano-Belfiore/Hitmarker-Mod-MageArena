namespace HitMarker.Console
{
    public interface ICommand
    {
        string Name { get; }

        void Execute(string args);
    }
}
