using UnityEngine;
using PrimeTween; 

public class DoorStaticController : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float openAngle = 90f;
    [SerializeField] private float duration = 0.5f;
    [SerializeField] private Ease ease = Ease.OutQuad;

    private Rigidbody rb;
    private bool isOpen = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        
        if (rb != null)
        {
            rb.isKinematic = true;
        }
    }

    [ContextMenu("Toggle Door")]
    public void ToggleDoor()
    {
        float targetY = isOpen ? 0f : openAngle;
        isOpen = !isOpen;

        Tween.LocalRotation(transform, endValue: Quaternion.Euler(0, targetY, 0), duration: duration, ease: ease);
    }

    public void Open()
    {
        if (isOpen) return;
        isOpen = true;
        Tween.LocalRotation(transform, endValue: Quaternion.Euler(0, openAngle, 0), duration: duration, ease: ease);
    }
}