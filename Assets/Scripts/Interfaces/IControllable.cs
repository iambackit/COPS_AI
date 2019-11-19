using System.Collections.Generic;

namespace Assets.Scripts.Interfaces
{
    interface IControllable
    {
        void Move(List<float> inputs);
    }
}
