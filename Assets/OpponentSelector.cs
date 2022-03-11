using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpponentSelector : MonoBehaviour
{
    MeshRenderer renderer;
    public MovePlayer player;
    public float rotSpeed;
    // Start is called before the first frame update
    void Start()
    {
        renderer = GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player.currentOpponent != null)
        {
            transform.position = player.currentOpponent.position;
            renderer.enabled = true;
            transform.rotation = Quaternion.Euler(90f, Time.time * rotSpeed, 0f);
        }
        else
        { 
            renderer.enabled = false;
        }

    }
}
