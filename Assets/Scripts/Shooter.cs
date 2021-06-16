using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    
    AreaScript areaScript;
    private WaitForSeconds shotDuration = new WaitForSeconds(0.07f); 
    private LineRenderer laserLine; 
    public float weaponRange = 20f;   

    void Start() {
        laserLine = transform.GetComponent<LineRenderer>();
    }

    public void shoot()
    {
        
        areaScript = transform.parent.parent.GetComponentInParent<AreaScript>();
        laserLine.SetPosition (0, transform.position);
        StartCoroutine (ShotEffect()); 
        
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, 20))
        {
            GameObject shot = hit.transform.gameObject;
            laserLine.SetPosition (1, hit.point);

            if(shot.CompareTag("enemy"))
            {
                areaScript.DestroyEnemy(shot);
                Debug.Log("Hit Enemy");
            }

        }
        else
            {
                // If we did not hit anything, set the end of the line to a position directly in front of the camera at the distance of weaponRange
                laserLine.SetPosition (1, transform.position + (transform.forward * weaponRange));
            }
        
    }

    private IEnumerator ShotEffect()
    {

        // Turn on our line renderer
        laserLine.enabled = true;

        //Wait for .07 seconds
        yield return shotDuration;

        // Deactivate our line renderer after waiting
        laserLine.enabled = false;
    }
}
