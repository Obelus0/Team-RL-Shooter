using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using UnityEditor;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    public float range = 50f;
    public float movement = 0.3f;
    AreaScript areaScript;
    void FixedUpdate()
    {
        GameObject parent = transform.parent.gameObject;
        Transform tf = parent.transform.Find("Agent").transform;
        if(Vector3.Distance(tf.position,transform.position) <= range)
        {
            transform.LookAt(tf);
            transform.position = Vector3.MoveTowards(transform.position, tf.position, movement);
        }
       
    }

    
}
