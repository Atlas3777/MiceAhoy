using Unity.Cinemachine;
using UnityEngine;

namespace Game.Scripts
{
    public class WindowCameraTrigger : MonoBehaviour
    {
        [SerializeField] private CinemachineTargetGroup targetGroup;
        [SerializeField] private int targetIndex = 1; 
        [SerializeField] private float activeWeight = 5f;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player")) 
            {
                targetGroup.Targets[targetIndex].Weight = activeWeight;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                targetGroup.Targets[targetIndex].Weight = 0f;
            }
        }
    }
}