using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public static ItemManager Instance { get; private set; }
    public GameObject item;
    private void Awake()
    {
        if (Instance != null)
            Destroy(Instance.gameObject);
        else
            Instance = this;
    }
}
