using UnityEngine;

public class KeepStaticRotation : MonoBehaviour
{
    [SerializeField] private Vector3 fixedRotation = Vector3.zero;

    void LateUpdate()
    {
        transform.rotation = Quaternion.Euler(fixedRotation);
    }
}