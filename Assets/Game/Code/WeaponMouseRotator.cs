using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class WeaponMouseRotator : MonoBehaviour
{
    public WeaponManager weaponManager; // Silahlara erişmek için
    private LineRenderer _lineRenderer;

    private Vector3 _currentAimPoint;   // Fareden hesapladığımız nokta

    private void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _lineRenderer.positionCount = 2;
        _lineRenderer.enabled = false;
    }

    private void Update()
    {
        // Sağ tık: Aim
        if (Input.GetMouseButton(1))
        {
            // 1) "XZ düzlemine ray-plane" veya "y=0 plane" yaklaşımıyla aimPoint alalım
            _currentAimPoint = GetXZPointFromMouse(); 
            
            // 2) Bütün silahları bu noktaya döndür (Y sabit vs.)
            RotateAllWeapons(_currentAimPoint);

            // 3) Çizgiyi çiz
            DrawAimLine(_currentAimPoint);
            
            // Sol tık: Ateş
            if (Input.GetMouseButtonDown(0))
            {
                // Tüm silahlara “FireWeaponAtPoint(_currentAimPoint)” komutu ver
                foreach (var weapon in weaponManager.currentWeapons)
                {
                    if (weapon != null)
                    {
                        weapon.FireWeapon(_currentAimPoint);
                    }
                }
            }
        }
        else
        {
            // Sağ tık bırakıldıysa çizgiyi kapat
            _lineRenderer.enabled = false;
        }
    }

    private Vector3 GetXZPointFromMouse()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane plane = new Plane(Vector3.up, Vector3.zero); // y=0 düzlemi
        float distance;
        if (plane.Raycast(ray, out distance))
        {
            Vector3 point = ray.GetPoint(distance);
            point.y = 0f;
            return point;
        }
        return Vector3.zero;
    }

    private void RotateAllWeapons(Vector3 aimPoint)
    {
        // Silahları y ekseninde döndürelim
        for (int i = 0; i < weaponManager.currentWeaponSlots.Length; i++)
        {
            Weapon weapon = weaponManager.currentWeaponSlots[i];
            if (weapon != null)
            {
                Transform wTransform = weapon.transform;
                Vector3 lookPos = new Vector3(aimPoint.x, wTransform.position.y, aimPoint.z);
                wTransform.LookAt(lookPos);
            }
        }
    }

    private void DrawAimLine(Vector3 aimPoint)
    {
        // İlk silahı referans al
        Weapon firstWeapon = weaponManager.currentWeaponSlots.Length > 0
                                ? weaponManager.currentWeaponSlots[0]
                                : null;

        if (firstWeapon != null)
        {
            _lineRenderer.enabled = true;
            Vector3 weaponPos = firstWeapon.transform.position;

            // Start = silahın pozisyonu
            _lineRenderer.SetPosition(0, weaponPos);
            // End = aimPoint’in x ve z’si, y’si silahın y’si
            Vector3 endPos = new Vector3(aimPoint.x, weaponPos.y, aimPoint.z);
            _lineRenderer.SetPosition(1, endPos);
        }
        else
        {
            _lineRenderer.enabled = false;
        }
    }
}
