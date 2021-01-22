using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
	private delegate void VoidDelegate();
	private VoidDelegate cancelDelegate;

	public string levelSelectionSceneName = "Level Selection";
	// 
	[Header("Continue")]
	public GameObject continueBtn;

	public string continueSceneName;
	public bool hasSavedGame = false;

	[Header("New Game")]
	public CanvasGroup newgameConfirm;

	[Header("Museum")]
	public string museumSceneName;

	[Header("Setting")]
	public CanvasGroup settingMenu;

	[Header("About")]
	public CanvasGroup aboutPage;

	[Header("Exit")]
	public CanvasGroup exitConfirm;

	void Start()
	{
		hasSavedGame = PlayerPrefs.GetInt("HasSavedGame") == 1;
		continueBtn.SetActive(hasSavedGame);
		Hide(newgameConfirm);
		Hide(settingMenu);
		Hide(aboutPage);
		Hide(exitConfirm);
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			cancelDelegate.Invoke();
		}
	}

	private void Hide(CanvasGroup canvasGroup)
	{
		canvasGroup.alpha = 0;
		canvasGroup.interactable = false;
		canvasGroup.blocksRaycasts = false;
	}

	private void Show(CanvasGroup canvasGroup)
	{
		canvasGroup.alpha = 1;
		canvasGroup.interactable = true;
		canvasGroup.blocksRaycasts = true;
	}

	private void Load(string sceneName)
	{
		SceneManager.LoadScene(sceneName);
	}

	// Co
	public void Continue()
	{
		// Load(continueSceneName);
		Load(levelSelectionSceneName);
	}

#region New Game
	public void NewGame()
	{
		// 
		if (hasSavedGame) { ShowNewGameConfirm(); }
		else
		{
			// PlayerPrefs.SetInt("HasSavedGame", 1);
			ConfirmNewGame();
			// Load(levelSelectionSceneName);
		}
	}

	private void ShowNewGameConfirm()
	{
		Show(newgameConfirm);
		cancelDelegate = HideNewGameConfirm;

	}

	public void HideNewGameConfirm()
	{
		Hide(newgameConfirm);
		cancelDelegate = Exit;
	}

	public void ConfirmNewGame()
	{
		Debug.Log("Clear Saved data");
		PlayerPrefs.SetInt("HasSavedGame", 1);
		ClearData();
		Load(levelSelectionSceneName);
	}
#endregion

	public void Museum()
	{
		if (museumSceneName == null || museumSceneName == string.Empty) { return; }
		Load(museumSceneName);
	}

#region Setting
	public void Setting()
	{
		Show(settingMenu);
		cancelDelegate = HideSetting;

	}

	public void HideSetting()
	{
		Hide(settingMenu);
		cancelDelegate = Exit;
	}
#endregion

#region About
	public void About()
	{
		Show(aboutPage);
		cancelDelegate = HideAbout;
	}
	public void HideAbout()
	{
		Hide(aboutPage);
		cancelDelegate = Exit;
	}
#endregion

#region Exit
	// Show confirm exit panel
	public void Exit()
	{
		Show(exitConfirm);
		cancelDelegate = HideExitConfirm;
	}
	//Hide Confirm exit panel
	public void HideExitConfirm()
	{
		Hide(exitConfirm);
		cancelDelegate = Exit;
	}
	public void ConfirmExit()
	{
		Application.Quit();
	}
#endregion

	public void ClearData()
	{

		// PlayerPrefs.SetInt("HasSavedGame", 1);
		PlayerPrefs.SetInt("CurStage", 1);
		PlayerPrefs.SetInt("scene_1-0", 0);
		PlayerPrefs.SetInt("scene_1-1", 0);
		PlayerPrefs.SetInt("scene_2-1", 0);
	}
}