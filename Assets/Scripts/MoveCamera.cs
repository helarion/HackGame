using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour {

    public float axisSpeed = 0.2f;
    public float panSpeed = 10;
    public float zoomSpeed = 10;

    bool bDragging;
    Vector3 oldPos;
    Vector3 panOrigin;

	// Update is called once per frame
	void Update () {

        // Partie Clavier
        float hAxis = Input.GetAxis("Horizontal");
        float vAxis = Input.GetAxis("Vertical");
        float zAxis = Input.GetAxis("Mouse ScrollWheel");

        if(hAxis !=0 || vAxis!= 0 || zAxis != 0)
        transform.Translate(new Vector3(hAxis * axisSpeed * Time.deltaTime, vAxis * axisSpeed * Time.deltaTime, zAxis * zoomSpeed * Time.deltaTime));

        // Partie souris
        if (Input.GetMouseButtonDown(0))
        {
            bDragging = true;
            oldPos = transform.position;
            panOrigin = Camera.main.ScreenToViewportPoint(Input.mousePosition);                    //Get the ScreenVector the mouse clicked
        }

        if (Input.GetMouseButton(0))
        {
            Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition) - panOrigin;    //Get the difference between where the mouse clicked and where it moved
            transform.position = oldPos + -pos * panSpeed;                                         //Move the position of the camera to simulate a drag, speed * 10 for screen to worldspace conversion
        }

        if (Input.GetMouseButtonUp(0))
        {
            bDragging = false;
        }
    }

}
