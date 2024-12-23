using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimEvents : MonoBehaviour
{
    private test_player player;
    // Start is called before the first frame update
    void Start()
    {
        player = GetComponentInParent<test_player>();
    }

    // Update is called once per frame

    public void AnimationTrigger(){
        player.AttackOver();
        Debug.Log("Stop");
    }

}
