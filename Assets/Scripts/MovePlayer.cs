using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlayer : Fighter
{
    public Transform opponentsContainer;
    Transform[] opponents;
    public float engagingRange = 4f;
    public Transform currentOpponent = null;
    float timeSinceChangedOpponent = 0f;
    // Start is called before the first frame update
    void Start()
    {
        GetCommonComponents();
        opponents = new Transform[opponentsContainer.childCount];
        for (int i = 0; i < opponents.Length; i++)
            opponents[i] = opponentsContainer.GetChild(i);
    }

    // Update is called once per frame
    void Update()
    {
        GetUserInput();
        FighterUpdate();
        OrientPlayerToFaceOpponent();
        if (Input.GetButtonDown("Fire1"))
            animator.SetTrigger("Punch");

        ComputeTimers();
    }

    private void ComputeTimers()
    {
        timeSinceChangedOpponent += Time.deltaTime;
    }

    private void OrientPlayerToFaceOpponent()
    {
        float distToClosestOpponent = float.MaxValue;
        int closestOpponentIndex = -1;
        Transform previousOpponent = currentOpponent;
        currentOpponent = null;
        Vector3 dirToClosestOpponent = Vector3.left;
        for (int i = 0; i < opponents.Length; i++)
        {
            Vector3 dirToOpponent = opponents[i].position - transform.position;
            float distToOpp = dirToOpponent.magnitude;
            if (distToOpp < engagingRange && distToOpp < distToClosestOpponent &&
                Vector3.Dot(transform.forward, dirToOpponent.normalized) > 0f) //e cu fata la inamic
            {
                distToClosestOpponent = distToOpp;
                closestOpponentIndex = i;
                currentOpponent = opponents[i];
                dirToClosestOpponent = dirToOpponent.normalized;
            }
        }
        if (closestOpponentIndex != -1)
        {//avem oponent la mai putin de 5 metri
            if (currentOpponent != opponents[closestOpponentIndex])
            {//vrea sa il schimbe
                if (timeSinceChangedOpponent < 0.25f)
                {
                    currentOpponent = previousOpponent;
                    return; //nu il schimba mai des de 4 ori pe secunda
                }
                else
                {
                    timeSinceChangedOpponent = 0f;
                }
            }

            float playerFwdDotToOpponent = Vector3.Dot(transform.forward, dirToClosestOpponent);
            if (playerFwdDotToOpponent > 0f)
                transform.forward = Vector3.Slerp(transform.forward, dirToClosestOpponent, Time.deltaTime * 20f);
            ApplyRootRotation(dirToClosestOpponent.normalized);
            animator.SetFloat("DistToEnemy", dirToClosestOpponent.magnitude);
        }
        else 
        {
            ApplyRootRotation(moveDir);
            animator.SetFloat("DistToEnemy", 5.1f);
        }
    }

    private void GetUserInput()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        moveDir = (Vector3.right * x + Vector3.forward * z).normalized;
    }
}
