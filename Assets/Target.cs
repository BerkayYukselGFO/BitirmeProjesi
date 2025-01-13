using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

public class Target : MonoBehaviour
{
	[SerializeField] private Transform fragmentParent;
	[SerializeField] private List<Transform> fragments;
	public System.Action OnTargetDestroyed;
	public void Destroy()
	{
		fragmentParent.gameObject.SetActive(true);
		fragmentParent.transform.SetParent(null);
		for (int i = 0; i < fragments.Count; i++)
		{
			var rb = fragments[i].AddComponent<Rigidbody>();
			rb.AddExplosionForce(100f, transform.position, 1f);
			var fragment = fragments[i];
			DOVirtual.DelayedCall(2.5f, () =>
			{
				fragment.transform.DOScale(Vector3.zero,0.5f).OnComplete(()=>
				{
					Destroy(fragment.gameObject);
				});
			}, false);
			OnTargetDestroyed?.Invoke();
		}
		Destroy(this.gameObject);
	}
}
