using Unity.Cinemachine;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public Vector3 originalPos;
    public CinemachineCamera camera;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        originalPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
