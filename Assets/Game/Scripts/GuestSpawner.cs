using UnityEngine;
using VContainer;

public class GuestSpawner : MonoBehaviour
{
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private GameObject guestGroupPrefab;

    [Inject]
    public void Init(GameResources gameResources)
    {
        guestGroupPrefab = gameResources.GuestGroup.gameObject;
    }

    public void CreateGuest()
    {
        Instantiate(guestGroupPrefab, spawnPoint.position, spawnPoint.rotation);
    }
}