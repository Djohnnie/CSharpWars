using Assets.Scripts.Model;
using UnityEngine;

namespace Assets.Scripts.Controllers.Interfaces
{
    public interface IBotDependant
    {
        void UpdateBot(Bot bot);
        void Destroy();
        void SetVariableOffset(float offset);
        GameObject GetGameObject();
    }
}