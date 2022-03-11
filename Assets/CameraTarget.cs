using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTarget : MonoBehaviour
{
    public MovePlayer player;
    public float followspeed = 3f;
    // Start is called before the first frame update
    void Start()
    {
        transform.position = player.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 target = player.transform.position;
        if (player.currentOpponent != null) 
        {
            target = Vector3.Lerp(target, player.currentOpponent.position, 0.5f);
        }
        transform.position = Vector3.Lerp(transform.position, target, Time.deltaTime * followspeed);
    }
}
