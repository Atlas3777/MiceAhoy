using UnityEngine;
using UnityEngine.EventSystems; 

public class CustomButton : MonoBehaviour, IPointerEnterHandler
{
    [SerializeField] private AudioSource audioSource;
    
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("HoverSoundPlayer: AudioSource component not found on this GameObject.");
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (audioSource != null && !audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }
}