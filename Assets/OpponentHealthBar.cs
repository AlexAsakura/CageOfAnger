using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpponentHealthBar : MonoBehaviour
{
    public Transform healthFill;
    Opponent opponent;
    // Start is called before the first frame update
    void Start()
    {
        opponent = GetComponentInParent<Opponent>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        float hp = 0.01f * (float)opponent.animator.GetInteger("HP");
        healthFill.transform.localScale = new Vector3(Mathf.Max(0f, hp), 1f, 1f);
        transform.rotation = Quaternion.identity;
    }
}
