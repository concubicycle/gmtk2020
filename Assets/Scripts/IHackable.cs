using UnityEngine;

namespace Assets.Scripts
{
    public interface IHackable
    {
        bool IsHacked { get; set; }
        string Name { get; }
        bool IsControlled { get; set; }
        GameObject RootObject { get; }
    }
}
