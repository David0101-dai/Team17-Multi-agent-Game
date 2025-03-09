using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FxManager : MonoBehaviour
{
    public static FxManager Instance { get; private set; }
    public GameObject fx;
    private void Awake()
    {
        if (Instance != null)
            Destroy(Instance.gameObject);
        else
            Instance = this;
    }
}
