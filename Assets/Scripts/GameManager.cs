using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

	private static GameManager instance;
	public static GameManager Instance { get { return instance; } }

	private void Awake()
	{
		if (instance != null && instance != this)
		{
			Destroy(this.gameObject);
		}
		else
		{
			instance = this;
			DontDestroyOnLoad(this.gameObject);
		}
	}

	[SerializeField]
	private string prevSceneName;

	[SerializeField]
	private Stage currentStage;

	void Start()
	{
		SceneManager.activeSceneChanged += ChangedActiveScene;
		SceneManager.sceneLoaded += OnSceneLoaded;
	}

	private void ChangedActiveScene(Scene current, Scene next)
	{
		prevSceneName = current.name;
		print(prevSceneName);
	}
	private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{

	}

	public void SetCurrentStage(Stage stage)
	{
		currentStage = stage;
	}

	public bool IsCurrentStage(Stage stage)
	{
		return currentStage == stage;
	}
}