using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    public class BaseBehaviour : MonoBehaviour
    {
        public virtual Task Start()
        {
            // Trigger dependency injection on this MonoBehaviour.
            this.Inject();
            
            return Task.CompletedTask;
        }
    }
}