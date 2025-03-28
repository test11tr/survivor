using UnityEngine;

public class WeaponMouseRotator : MonoBehaviour
{
    public LayerMask groundLayer;

    void Update()
    {
        if (Input.GetMouseButton(1))
        {
            RotateWeaponsToMouse();
        }
    }

    void RotateWeaponsToMouse()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 100f, groundLayer))
        {
            Vector3 targetPosition = hit.point;
            targetPosition.y = transform.position.y; // Yüksekliği sabitle

            // Rotate manager veya bu scriptin objesi
            transform.LookAt(targetPosition);
        }
    }
}
