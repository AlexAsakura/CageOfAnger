using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{    
    public Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float xScale = 0.01f * (float)animator.GetInteger("HP");
        transform.localScale = new Vector3(Mathf.Max(0f, xScale), 1f, 1f);
    }
}
