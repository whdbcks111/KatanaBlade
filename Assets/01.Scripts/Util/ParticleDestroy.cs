using UnityEngine;

namespace _01.Scripts.Util
{
    [RequireComponent(typeof(ParticleSystem))]
    public class ParticleDestroy : MonoBehaviour
    {
        private ParticleSystem _system;

        private void Awake()
        {
            _system = GetComponent<ParticleSystem>();
        }

        private void Update()
        {
            if(_system.isStopped) Destroy(gameObject);
        }
    }
}
