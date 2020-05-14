
using System;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float panSpeed = 20f; //camera movement speed
    public float panBorderThickness = 10f;
    public Vector2 panLimit;
   
    
    public float scrollSpeed = 20;
    public float minY = 20f;
    public float maxY = 120f;

    public GameManager gameManager;

	public static event Action OnMouseWheelMoved;

    void Start()
    {
   
        // gameManager.startGameEvent += CameraMovementEvent;
       

    }

    // Update is called once per frame
    void Update()
    {
       CameraMovementEvent();
    }

    private void CameraMovementEvent()
    {
        Vector3 pos = transform.position;
        
        if (Input.GetKey("w")) //|| Input.mousePosition.y >= Screen.height - panBorderThickness) //panning up
        {
            pos.z += panSpeed * Time.deltaTime;
        }

        if (Input.GetKey("s")) //  || Input.mousePosition.y <= panBorderThickness) //panning down
        {
            pos.z -= panSpeed * Time.deltaTime; 
        }
        
        if (Input.GetKey("d")) //|| Input.mousePosition.x >=Screen.width - panBorderThickness) //panning right
        {
            pos.x += panSpeed * Time.deltaTime; 
        }
       
        if (Input.GetKey("a")) //|| Input.mousePosition.x <= panBorderThickness) //panning left
        {
            pos.x -= panSpeed * Time.deltaTime; 
        }

        float scroll = Input.GetAxis("Mouse ScrollWheel");

		// Hack for tutorials etc to know about mouse wheels. should use new input system
		if (scroll > 0 || scroll < 0)
		{
			OnMouseWheelMoved?.Invoke();
		}
		
        pos.y -= scroll * scrollSpeed * 100f * Time.deltaTime;
       
        pos.x = Mathf.Clamp(pos.x, -panLimit.x, panLimit.x); //clamps allows a set min and max value
        pos.y = Mathf.Clamp(pos.y, minY, maxY);
        pos.z = Mathf.Clamp(pos.z, -panLimit.y, panLimit.y);
        
        transform.position = pos; //getting the new position of the camera
    }
}
