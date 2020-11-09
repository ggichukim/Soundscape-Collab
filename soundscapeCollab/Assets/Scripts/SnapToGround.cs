using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnapToGround : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // send a short raycast from object downwards
        Ray ray = new Ray(this.transform.position, -this.transform.up);
        RaycastHit hit; 
        // if it hits ground: It is close enough
        if (Physics.Raycast(ray, out hit, 0.5f))
        {
            if (hit.collider.tag == "ground")
            {
                this.transform.parent = null; // unparent it from the grabbers.
                Vector3 newPosition = new Vector3(hit.point.x, hit.point.y, hit.point.z);
                Vector3 offset = GetComponent<Collider>().bounds.size;
                this.transform.position = newPosition + new Vector3 (0f, (offset.y/2f), 0f); // move it to the ground
                this.transform.rotation = Quaternion.identity; // set it upright
            }
        }

    }
}
