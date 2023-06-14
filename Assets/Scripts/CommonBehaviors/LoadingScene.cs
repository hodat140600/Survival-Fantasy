using Assets._SDK.Ads.AdsClient;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingScene : Singleton<LoadingScene>
{
	public TextMeshProUGUI textMeshProGUi;
	public GameObject loadingScene;
	public ProgressBar bar;
	public float timeDelay = 0.5f;
	public int IsFirst;
	private void Awake()
	{
		//SceneManager.LoadSceneAsync((int)SceneIndex.Loading, LoadSceneMode.Additive);
		_ = AdsClientManager.Instance;
		LoadScene();
	}

	public void LoadScene()
	{
		IsFirst = PlayerPrefs.GetInt("IsFirst");
		StartCoroutine(LoadGame());
	}
	List<AsyncOperation> sceneLoading = new List<AsyncOperation>();
	IEnumerator LoadGame()
	{
		//SceneManager.LoadSceneAsync((int)SceneIndex.Logo, LoadSceneMode.Additive);
		yield return new WaitForSeconds(timeDelay);
		//sceneLoading.Add(SceneManager.UnloadSceneAsync((int)SceneIndex.Logo));
		yield return new WaitForSeconds(0.5f);
		if (IsFirst == 0)
		{
			//Do stuff on the first time
			Debug.Log("first run");
			PlayerPrefs.SetInt("IsFirst", 1);
			sceneLoading.Add(SceneManager.LoadSceneAsync((int)SceneIndex.Cutscene, LoadSceneMode.Additive));
			StartCoroutine(GetSceneLoadProgress(0));
		}
		else
		{
			//Do stuff other times
			//sceneLoading.Add(SceneManager.UnloadSceneAsync((int)SceneIndex.Cutscene));
			Debug.Log("welcome again!");
			sceneLoading.Add(SceneManager.LoadSceneAsync((int)SceneIndex.Main, LoadSceneMode.Additive));
			StartCoroutine(GetSceneLoadProgressHasAOA());
		}
		loadingScene.SetActive(true);

		for (int index = 0; index < sceneLoading.Count; index++)
		{
			sceneLoading[index].allowSceneActivation = false;
		}

	}

	IEnumerator GetSceneLoadProgress( float progressValue)
	{
		yield return new WaitForSeconds(0.1f);

		float progress = progressValue;

		float progressCurent = 0;

		while (progress < 0.9f)
		{
			progressCurent = 0;

			for (int index = 0; index < sceneLoading.Count; index++)
			{
				progressCurent += sceneLoading[index].progress;
			}

			progress = Mathf.MoveTowards(progress, progressCurent, Time.deltaTime);
			bar.current = (int)(progress * 100);
			textMeshProGUi.text = string.Format("Loading Enviroments: {0}%", bar.current);
			yield return null;
		}


		if (progress >= 0.9f)
		{
			bar.current = 100;
			textMeshProGUi.text = string.Format("Loading Enviroments: {0}%", bar.current);
			for (int index = 0; index < sceneLoading.Count; index++)
			{
				sceneLoading[index].allowSceneActivation = true;
			}
		}

		loadingScene.SetActive(false);
	}

	IEnumerator GetSceneLoadProgressHasAOA()
	{
		yield return new WaitForSeconds(0.1f);

		float progress = 0;
		if (AdsConfig.TypeAdsUse.HasFlag(TypeAdsMax.AOA))
		{
			float _timeToRun = AdsConfig.CONST_TIME_WAIT_FOR_SHOW_FIRST_AOA;
			while ((_timeToRun -= Time.deltaTime) >= 0)
			{
				progress = Mathf.Clamp01((1 - _timeToRun / AdsConfig.CONST_TIME_WAIT_FOR_SHOW_FIRST_AOA) * .7f);
				bar.current = (int)((progress * 100));
				textMeshProGUi.text = string.Format("Loading Enviroments: {0}%", bar.current);
				yield return null;
			}
			AdsClientManager.Instance.AdsClient.ShowAOA();
		}

		StartCoroutine(GetSceneLoadProgress(progress));
	}

}
