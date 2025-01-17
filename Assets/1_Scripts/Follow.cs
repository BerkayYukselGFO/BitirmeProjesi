using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{
	[SerializeField] private Transform target;
	[SerializeField] private const float smoothSpeed = 15f;

	private void Update()
	{
		// Hedef rotasyonu ters çevir
		var targetRot = new Quaternion(-target.transform.rotation.z, -target.transform.rotation.y, -target.transform.rotation.x, target.transform.rotation.w);
		// Mevcut dönüş ile hedef dönüş arasında yumuşak geçiş yap
		transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * smoothSpeed);
	}
}
