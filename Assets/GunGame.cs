using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


[System.Serializable]
public class TargetAndSpawnTime
{
	public Target target;
	public float spawnTime;

	public TargetAndSpawnTime(Target target, float spawnTime)
	{
		this.target = target;
		this.spawnTime = spawnTime;
	}
}


public class GunGame : Singleton<GunGame>
{
	[SerializeField] private Transform target;
	[SerializeField] private Button startGameButton;
	public List<TargetAndSpawnTime> targetAndSpawnTimes = new();
	public List<float> responseTime = new();
	public int targetCount = 30;
	[SerializeField] private Transform informationPanel;
	[SerializeField] private TextMeshProUGUI infoText;
	[SerializeField] private Button restartButton;

	private float gameStartTime;
	private int hitCount = 0;
	[SerializeField] private bool editorPlay;

	private void Start()
	{
		SerialReader.Instance.OnCalibrateDone += () =>
		{
			startGameButton.gameObject.SetActive(true);
		};

		if (editorPlay)
		{
			startGameButton.gameObject.SetActive(true);
		}
		startGameButton.onClick.AddListener(() =>
		{
			StartGame();
		});
		restartButton.onClick.AddListener(() =>
		{
			StopAllCoroutines();
			responseTime.Clear();
			targetAndSpawnTimes.Clear();
			informationPanel.gameObject.SetActive(false);
			StartGame();
		});
	}

	private void StartGame()
	{
		gameStartTime = Time.time; // Oyunun başlangıç zamanı
		StartCoroutine(SpawnTarget());
		startGameButton.gameObject.SetActive(false);
	}

	private IEnumerator SpawnTarget()
	{
		var currentCount = 0f;
		while (currentCount < targetCount)
		{
			var targetInstance = Instantiate(target, RandomPos(), Quaternion.Euler(0f, 180f, 0f));
			var targetComponent = targetInstance.GetComponent<Target>();
			targetAndSpawnTimes.Add(new TargetAndSpawnTime(targetComponent, Time.time));

			currentCount++;

			// Hedefin yaşam süresini kontrol et
			StartCoroutine(HandleTargetLifetime(targetComponent));

			yield return new WaitForSeconds(Random.Range(0.5f, 2f));

			while (targetAndSpawnTimes.Count > 5)
			{
				yield return null;
			}
		}

		// Oyun bittiğinde sonuçları göster
		informationPanel.gameObject.SetActive(true);
		float totalTime = Time.time - gameStartTime; // Toplam oyun süresi
		float accuracy = (float)hitCount / targetCount; // Doğruluk oranı
		infoText.text = $"Ortalama Reaksiyon Süresi: {responseTime.Average():F2}\n" +
						$"Doğruluk: {accuracy:P2}\n" +
						$"Toplam Süre: {totalTime:F2} saniye";
	}

	private IEnumerator HandleTargetLifetime(Target target)
	{
		yield return new WaitForSeconds(5f);

		// Son 3 saniyede mesh renderer'ı yanıp söndür
		var renderer = target.GetComponent<MeshRenderer>();
		if (renderer != null)
		{
			float blinkDuration = 0.3f; // Yanıp sönme süresi
			for (float t = 0; t < 3f; t += blinkDuration)
			{
				renderer.enabled = false;
				yield return new WaitForSeconds(0.1f);
				renderer.enabled = true;
				yield return new WaitForSeconds(0.2f);
			}
		}

		// Hedefi yok et
		Destroy(target.gameObject);
	}

	private void Update()
	{
		for (int i = targetAndSpawnTimes.Count - 1; i >= 0; i--)
		{
			if (targetAndSpawnTimes[i].target == null)
			{
				responseTime.Add(Time.time - targetAndSpawnTimes[i].spawnTime);
				targetAndSpawnTimes.RemoveAt(i);
				hitCount++; // Vurulan hedef sayısını artır
			}
		}
	}

	private Vector3 RandomPos()
	{
		return new Vector3(Random.Range(-4f, 4f), Random.Range(0.6f, 3.5f), Random.Range(4f, 6.8f));
	}
}
