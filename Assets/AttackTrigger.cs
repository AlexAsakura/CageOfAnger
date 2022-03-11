using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackTrigger : MonoBehaviour
{
    public string testLayer;
    public int damage = 5;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer(testLayer))
        {
            var opponentAnimator = other.transform.parent.GetComponent<Animator>();
            if (opponentAnimator.GetInteger("HP") < 0)
                return;
            opponentAnimator.Play("Hit to Body");
            opponentAnimator.SetInteger("Damage", damage);
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
