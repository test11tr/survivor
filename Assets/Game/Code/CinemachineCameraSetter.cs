using Unity.Cinemachine;
using UnityEngine;

public class CinemachineCameraSetter : MonoBehaviour
{
    public Camera mainCamera;
    public CinemachineBrain cinemachineBrain;

    void Start()
    {
        if(mainCamera == null)
        {
            mainCamera = Camera.main;
        }
        if (cinemachineBrain == null)
        {
            cinemachineBrain = mainCamera.GetComponent<CinemachineBrain>();
        }
        
        var currentCamera = cinemachineBrain.ActiveVirtualCamera as CinemachineCamera;
        currentCamera.Follow = this.transform;
        currentCamera.LookAt = this.transform;
    }
}
