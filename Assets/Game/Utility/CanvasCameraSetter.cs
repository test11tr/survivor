using UnityEngine;

public class CanvasCameraSetter : MonoBehaviour
{
    public Canvas canvas;
    private void Awake() {
        canvas.worldCamera = Camera.main;
    }
}
