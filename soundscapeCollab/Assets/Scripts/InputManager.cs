using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InputManager : MonoBehaviour
{

    public Transform anchor;        // get the transform of the OculusGo Controller device
    public GameObject indicatorObj; // get the object to use to indicate the proposed teleportation spot
    public GameObject player;
    public static UnityAction onTriggerDown = null;
    public float MAX_DISTANCE;
    public ControllerGrabber leftGrabber;
    public ControllerGrabber rightGrabber;

     
    private void Awake()
    {
        InputManager.onTriggerDown += TriggerDown;

    }
    private void OnDestroy()
    {
        InputManager.onTriggerDown -= TriggerDown;

    }
    // Use this for initialization
    void Start()
    {
        indicatorObj.SetActive(false);  // indicator is invisible unless the pointer intersects the ground

    }

    // Update is called once per frame
    void Update()
    {

        Ray ray = new Ray(anchor.position, anchor.forward); // cast a ray from the controller out towards where it is pointing
        RaycastHit hit;                                     // returns the hit variable to indicate what and where the ray 
                                                            // was intersected if at all

        //if (Physics.Raycast(ray, out hit, MAX_DISTANCE))  // a maxdistance should be set so you can't teleport too far away

        if (Physics.Raycast(ray, out hit, MAX_DISTANCE))
        {
            if (hit.transform.gameObject.CompareTag("ground"))
            {
                // valid object was hit
                Vector3 newPosition = new Vector3(hit.point.x, hit.point.y + 0.1f, hit.point.z); // WARNING: assumes target is just above ground 
                indicatorObj.transform.position = newPosition;
                if (!indicatorObj.activeSelf) indicatorObj.SetActive(true); // make sure it is visible
            }
            else
            {
                // valid object was not hit
                if (indicatorObj.activeSelf) indicatorObj.SetActive(false); // if nothihng is hit make it invisible
            }
        }
        else
        {
            // valid object was not hit
            if (indicatorObj.activeSelf) indicatorObj.SetActive(false); // if nothihng is hit make it invisible
        }

        // check for user input: primary trigger 
        if (OVRInput.GetDown(OVRInput.RawButton.RIndexTrigger, OVRInput.Controller.Touch))
        {
            if (onTriggerDown != null)
                onTriggerDown();
        }

        // secondary controller
        // check for user input: secondary trigger down on left grabber.
        if (OVRInput.GetDown(OVRInput.RawButton.LHandTrigger, OVRInput.Controller.Touch))
        {
            leftGrabber.userGrab = true;
        }

        // check for user input: secondary trigger down on right grabber
        if (OVRInput.GetDown(OVRInput.RawButton.RHandTrigger, OVRInput.Controller.Touch))
        {
            rightGrabber.userGrab = true;
        }

        // check for user input: secondary trigger up
        if (OVRInput.GetUp(OVRInput.RawButton.LHandTrigger, OVRInput.Controller.Touch))
        {
            leftGrabber.userGrab = false;
        }

        // check for user input: secondary trigger up
        if (OVRInput.GetUp(OVRInput.RawButton.RHandTrigger, OVRInput.Controller.Touch))
        {
            rightGrabber.userGrab = false;
        }

    }

    // function called when user pulls trigger
    private void TriggerDown()
    {
        // refresh hit to get exact location for teleportation
        Ray ray = new Ray(anchor.position, anchor.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, MAX_DISTANCE))
        {
            if (hit.transform.gameObject.CompareTag("ground"))
            {
                //transform the player to the hit position (X and Z plane only)
                Vector3 newpos = new Vector3(hit.point.x, player.transform.position.y, hit.point.z); // WARNING: assumes only moving along the ground plane
                player.transform.position = newpos;
            }
        }
    }

}