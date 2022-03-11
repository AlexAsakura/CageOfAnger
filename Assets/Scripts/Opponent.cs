using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Opponent : Fighter
{
    public Transform player;
    public float stopDistThreshold = 1f;
    public float attackDistThreshold = 1.2f;
    public enum MovementPhase { TARGET_PLAYER, CIRCLE_RIGHT, ANOTHER_RIGHT_CIRCLE ,
                                               CIRCLE_LEFT, ANOTHER_LEFT_CIRCLE,
                                               BACK_OFF, ANOTHER_BACK_OFF, NUM_PHAHSES};
    MovementPhase currentPhase;
    Vector3 dirOffset;
    public float dirOffsetMagnitude = 1f;
    bool offensive = false;
    [Range(0f, 1f)]
    public float difficulty;
    public float invasionArea = 7f;
    public float stopSpeed = 2f;
    Transform[] peers;
    // Start is called before the first frame update
    void Start()
    {
        GetCommonComponents();
        StartCoroutine(ChangeMovementPhase(0f));
        StartCoroutine(ChangeOffensiveBehavior(0f));
        StartCoroutine(RebindAnimator(Random.Range(0,2f)));
        peers = new Transform[transform.parent.childCount - 1];
        int peerIndex = 0;
        for(int i =0;i < peers.Length + 1; i++)
        {
            var peer = transform.parent.GetChild(i);
            if (peer == transform) //el insusi
                continue;
            peers[peerIndex] = peer;
            peerIndex++;
        }
       
    }
    IEnumerator RebindAnimator(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        animator.Rebind();
    }
    IEnumerator ChangeOffensiveBehavior(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        offensive = Random.Range(0f, 1f) < difficulty;
        yield return StartCoroutine(ChangeOffensiveBehavior(Random.Range(0.5f, 1.5f)));
    }
    IEnumerator ChangeMovementPhase(float waitTime) 
    {
        yield return new WaitForSeconds(waitTime);
        currentPhase = (MovementPhase)Random.Range(0, (int)MovementPhase.NUM_PHAHSES + 3);
        switch (currentPhase)
        {
            case MovementPhase.TARGET_PLAYER:
                dirOffset = Vector3.zero;
                break;
            case MovementPhase.CIRCLE_RIGHT:   case MovementPhase.ANOTHER_RIGHT_CIRCLE:
                dirOffset = ( transform.right) * dirOffsetMagnitude;
                break;
            case MovementPhase.CIRCLE_LEFT:    case MovementPhase.ANOTHER_LEFT_CIRCLE:
                dirOffset = ( - transform.right) * dirOffsetMagnitude;
                break;
            case MovementPhase.BACK_OFF:       case  MovementPhase.ANOTHER_BACK_OFF:
                dirOffset = -transform.forward * dirOffsetMagnitude;
                break;
            default:
                dirOffset = Vector3.zero;
                break;
        }
        yield return StartCoroutine(ChangeMovementPhase(Random.RandomRange(0.5f, 2.5f)));
    }
    // Update is called once per frame
    void Update()
    {
        Vector3 toPlayer = (player.position - transform.position);
        if (toPlayer.magnitude > stopDistThreshold && toPlayer.magnitude < invasionArea)
        {
            Vector3 awayFromPeer = Vector3.zero;
            int nearbyPeers = 0;
            for (int i = 0; i < peers.Length; i++)
            {
                Vector3 dirToPeer = peers[i].position - transform.position;
                float distToPeer = dirToPeer.magnitude;
                if (distToPeer < 1.6f)
                {
                    awayFromPeer += -dirToPeer / distToPeer;
                    nearbyPeers++;
                }
            }
            if (nearbyPeers > 0)
                awayFromPeer /= nearbyPeers;
            Vector3 offset = (toPlayer.magnitude < 6f ? dirOffset/* + awayFromPeer * 0.8f */: Vector3.zero);
            moveDir = (toPlayer.normalized + offset).normalized;
        }
        else
            moveDir = Vector3.zero;
        FighterUpdate();

        Debug.Log("Velocity " + rigidbody.velocity.magnitude.ToString());
        if (rigidbody.velocity.magnitude < stopSpeed)
        {
            animator.SetFloat("Forward", 0f, 0.05f, Time.deltaTime);
            animator.SetFloat("Right", 0f, 0.05f, Time.deltaTime);
        }
        
        ApplyRootRotation(toPlayer.normalized);

        if (offensive && toPlayer.magnitude < attackDistThreshold)
            animator.SetTrigger("Punch");

        animator.SetFloat("DistToEnemy", toPlayer.magnitude);
    }
}
