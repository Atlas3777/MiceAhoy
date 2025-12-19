using UnityEngine;
using UnityEngine.InputSystem;

public class GOVNOMono : MonoBehaviour
{
    [SerializeField] private GameObject Menu;
    [SerializeField] private GameObject Text;
    [SerializeField] private AudioSource MenuAudio;

    private void Awake()
    {
        if (MenuAudio == null)
        {
            MenuAudio = GetComponent<AudioSource>();
        }
    }

    private void Start()
    {
        MenuAudio.Play();
    }

    public void OnAnyKey(InputValue value)
    {
        if (value.isPressed)
        {
            Menu.SetActive(true);
            Text.SetActive(false);
            Destroy(this);
        }
    }
}