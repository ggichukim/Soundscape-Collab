using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerGrabber : MonoBehaviour
{
    private bool _grabbingObject;
    private bool _intersectingObject;

    public bool userGrab;
    private GameObject grabbedObject;

    public Material canGrabMaterial;
    public float snapDistance = 0.5f;
    private Material _savedMaterial;

    // Start is called before the first frame update
    void Start()
    {
        _grabbingObject = false;
        grabbedObject = null;

    }

    // Update is called once per frame
    void Update()
    {
        if (!userGrab && _grabbingObject)
        {
            grabbedObject.transform.parent = null;
            // grabbedObject = null;
            _grabbingObject = false;
        }
     
        

    }

    public void OnTriggerEnter(Collider other)
    {
        if (!_intersectingObject)
        {
            _savedMaterial = other.gameObject.GetComponent<Renderer>().material;
            other.gameObject.GetComponent<Renderer>().material = canGrabMaterial;
            _intersectingObject = true;
        }


    }

    public void OnTriggerStay(Collider other)
    {
        if (userGrab && !_grabbingObject)
        {
            if (other.gameObject.CompareTag("grabbable"))
            {
                _grabbingObject = true;
                grabbedObject = other.gameObject;
                grabbedObject.transform.SetParent(this.transform);
            }
        }

        if (_intersectingObject)
        {
            // send a short raycast from grabbed object downwards
            Ray ray = new Ray(grabbedObject.transform.position, -grabbedObject.transform.up);
            RaycastHit hit;
            // if it hits ground: Grabbed object is within snap distance of the ground
            if (Physics.Raycast(ray, out hit, snapDistance))
            {
                if (hit.collider.tag == "ground" && !userGrab)
                {
                    grabbedObject.transform.parent = null; // unparent it from the grabbers.
                    Vector3 newPosition = new Vector3(hit.point.x, hit.point.y, hit.point.z);
                    Vector3 offset = grabbedObject.GetComponent<Collider>().bounds.size;
                    grabbedObject.transform.position = newPosition + new Vector3(0f, (offset.y / 2f), 0f); // move it to the ground
                    grabbedObject.transform.rotation = Quaternion.identity; // set it upright
                }
            }
        }


    }

    public void OnTriggerExit(Collider other)
    {
        if (_intersectingObject)
        {
            other.gameObject.GetComponent<Renderer>().material = _savedMaterial;
            _intersectingObject = false;
            grabbedObject = null;
        }

    }

}