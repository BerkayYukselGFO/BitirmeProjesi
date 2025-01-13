using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorGunGame : MonoBehaviour
{
     [SerializeField] private LayerMask targetLayerMask; // Hedef katmanı için bir LayerMask

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Sol tık ya da ekrana dokunma
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); // Ekran dokunma noktasından bir ray oluştur
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, targetLayerMask)) // Belirtilen katmandaki bir nesneye çarpıp çarpmadığını kontrol et
            {
                if (hit.collider != null && hit.collider.gameObject.layer == LayerMask.NameToLayer("Target"))
                {
                    // Hedefe çarptı, yok et
                    Target target = hit.collider.GetComponent<Target>();
                    if (target != null)
                    {
                        target.Destroy(); // Target scriptindeki Destroy fonksiyonu çağrılıyor
                    }
                }
            }
        }
    }
}
