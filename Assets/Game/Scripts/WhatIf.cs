using UnityEngine;
using UnityEngine.UI;

public class WhatIf : MonoBehaviour
{
    public ScrollRect S;
    public float h;
    public bool i;
    void Update()
    {
        if (i) S.horizontalNormalizedPosition = h;
    }
}
