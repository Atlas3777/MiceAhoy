using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[CustomStyle("GuestGroupSpawnMarker")] 
public class GuestGroupSpawnEventMarker : Marker, INotification
{
    [Min(1)] public int count = 1; // Сколько штук
    
    // Свойство ID нужно для интерфейса INotification, можно оставить пустым
    public PropertyName id => new PropertyName();
}