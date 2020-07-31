namespace Assets.Scripts
{
    public interface IHackable
    {
        bool IsHacked { get; set; }
        string Name { get; }
    }
}
