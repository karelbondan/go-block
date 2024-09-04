using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponClippingPrevention : MonoBehaviour
{
    private GameObject FrontReference;
    [SerializeField]
    private float CheckDistance;
    [SerializeField]
    private Vector3 newRotateDirection;
    [SerializeField]
    private Vector3 newPosition;

    private Vector3 startPos;
    private float lerpPos;
    private RaycastHit hit;

    public void setFrontReference(GameObject frontReference)
    {
        FrontReference = frontReference;
    }

    private void Start()
    {
        startPos = transform.localPosition;
        newPosition += startPos;
    }

    private void Update()
    {
        if (Physics.Raycast(FrontReference.transform.position, FrontReference.transform.forward, out hit, CheckDistance))
        {
            // get percentage from 0 to max distance
            lerpPos = 1 - (hit.distance / CheckDistance);
        } else
        {
            lerpPos = 0; // sets this to 0 when the front reference hitbox raycast is not hitting any wall
        }

        Mathf.Clamp01(lerpPos);

        transform.localRotation = Quaternion.Lerp(
            Quaternion.Euler(Vector3.zero),
            Quaternion.Euler(newRotateDirection),
            lerpPos
            );

        transform.localPosition = Vector3.Lerp(
            startPos,
            newPosition,
            lerpPos
            );

        // fixed scale in case that parent's object also scale
        
    }
}
