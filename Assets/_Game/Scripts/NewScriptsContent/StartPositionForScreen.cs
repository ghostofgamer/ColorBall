using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPositionForScreen : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    public GameObject leftObject;
    public GameObject leftWall;
    public GameObject rightWall;
    public GameObject rightObject;
    public GameObject leftPoint;
    public GameObject rightPoint;
    public GameObject upDeadZone;
    public GameObject downDeadZone;
    public float offset = 0.5f; // Отступ от края экрана
    public float offsetPoint = -3f; // Отступ от края экрана
    public float offsetWall;
    public float verticalOffset = 0.5f;
    
    private void Update()
    {
        // float screenWidth = Camera.main.orthographicSize * Camera.main.aspect;

        float screenHeight = _camera.orthographicSize;
        
        // Верхний край
        Vector3 topPosition = new Vector3(upDeadZone.transform.position.x, screenHeight - verticalOffset, upDeadZone.transform.position.z);
        upDeadZone.transform.position = topPosition;

        // Нижний край
        Vector3 bottomPosition = new Vector3(downDeadZone.transform.position.x, -screenHeight + verticalOffset, downDeadZone.transform.position.z);
        downDeadZone.transform.position = bottomPosition;
        
        /*float screenWidth = _camera.orthographicSize * _camera.aspect;
        
        Vector3 leftPosition = new Vector3(-screenWidth + offset, leftObject.transform.position.y, leftObject.transform.position.z);
        Vector3 leftWallPosition = new Vector3(-screenWidth + offsetWall, leftWall.transform.position.y, leftWall.transform.position.z);
        Vector3 leftPositionPoint  = new Vector3(-screenWidth + offsetPoint, leftPoint.transform.position.y, leftPoint.transform.position.z);
        
        leftObject.transform.position = leftPosition;
        leftWall.transform.position = leftWallPosition;
        leftPoint.transform.position = leftPositionPoint;

     
        Vector3 rightPosition = new Vector3(screenWidth - offset, rightObject.transform.position.y, rightObject.transform.position.z);
        Vector3 rightWallPosition = new Vector3(screenWidth - offsetWall, rightWall.transform.position.y, rightWall.transform.position.z);
        Vector3 rightPositionPoint = new Vector3(screenWidth - offsetPoint, rightPoint.transform.position.y, rightPoint.transform.position.z);
        rightObject.transform.position = rightPosition;
        rightPoint.transform.position = rightPositionPoint;
        rightWall.transform.position = rightWallPosition;*/
    }
}
