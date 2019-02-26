using UnityEngine;

namespace Dictus
{
    public interface IControllable
    {
        void RegisterToAdapter();
        ActionSet actionSet { get; }
    }
}
