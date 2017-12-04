using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Text;
using TMPro;

public class GameManager : MonoBehaviour {
	public static GameManager instance;

	[SerializeField] GameObject dialogObj;
	[SerializeField] TextMeshProUGUI dialogText;
	[SerializeField] GameObject uiYoureFired;

	[SerializeField] SushiSprite[] sushiSpritesArr;
	public static Dictionary<SushiType, Sprite> SushiSprites;

	public static string  AXIS_HOR     = "Horizontal";
	public static string  AXIS_VER     = "Vertical";
	public static KeyCode KC_RESTART   = KeyCode.R;
	public static KeyCode KC_PAUSE     = KeyCode.Escape;
	public static KeyCode KC_GRAB      = KeyCode.Space;
	public static int     INT_GRAB_ALT = 1;

	public static string TAG_SUSHI = "Sushi";
	public static string TAG_PLATE = "Plate";

	public static string PP_HIGH_SCORE   = "HighScore";
	public static string PP_VOLUME_SFX   = "VolumeSFX";
	public static string PP_VOLUME_MUSIC = "VolumeMusic";

	public static UIScore uiScore;
	public static UIStrikes uiStrikes;

	public static int HighScore = 0;
	public static int Score = 0;
	public static int ScoreMaxDifficulty = 30;

	public static int Strikes = 0;
	public static int NumStrikesGameOver = 3;

	public static bool IsGameOver = false;
	public static bool IsPaused = false;

	public static Action OnPause;

	public static float volumeSFX = 1;
	public static float volumeMusic = 1;

	void Awake() {
		if (instance == null) {
			instance = this;
			DontDestroyOnLoad(gameObject);

			HighScore = PlayerPrefs.GetInt(PP_HIGH_SCORE, 0);

			volumeSFX = PlayerPrefs.GetFloat(GameManager.PP_VOLUME_SFX, 1);
			volumeMusic = PlayerPrefs.GetFloat(GameManager.PP_VOLUME_MUSIC, 1);

			SushiSprites = new Dictionary<SushiType, Sprite>();
			foreach (SushiSprite ss in sushiSpritesArr) {
				SushiSprites[ss.sushiType] = ss.sprite;
			}
		} else {
			Destroy(gameObject);
		}
	}

	public static void GameOver() {
		IsGameOver = true;
		instance.uiYoureFired.SetActive(true);
	}

	public static void IncrementStrikes() {
		Strikes++;
		uiStrikes.UpdateStrikes();
		if (Strikes >= NumStrikesGameOver) GameOver();
		// Debug.Log($"{Strikes} strikes");
	}

	public static void IncrementScore() {
		Score++;
		uiScore.UpdateScore();
	}

	public static void Pause() {
		IsPaused = true;
		Time.timeScale = 0;
		OnPause?.Invoke();
	}

	public static void Unpause() {
		IsPaused = false;
		Time.timeScale = 1;
		OnPause?.Invoke();
		instance.dialogObj.SetActive(false);
	}

	public static void TogglePause() {
		if (IsPaused) Unpause();
		else Pause();
	}

	void UpdateHighScore() {
		HighScore = Score;
		PlayerPrefs.SetInt(PP_HIGH_SCORE, HighScore);
	}

	public void Restart() {
		if (Score > HighScore) UpdateHighScore();

		Score = 0;
		Strikes = 0;
		IsGameOver = false;
		uiYoureFired.SetActive(false);
		OnPause = null;
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}

	public static void Dialog(string text) {
		Pause();
		instance.dialogText.text = text;
		instance.dialogObj.SetActive(true);
	}

	public void DismissDialog() {
		Unpause();
		dialogObj.SetActive(false);
	}

	void Update() {
		if (Input.GetKeyDown(KC_RESTART)) Restart();
		if (Input.GetKeyDown(KC_PAUSE)) TogglePause();

		#if UNITY_EDITOR
		if (Input.GetKeyDown(KeyCode.P)) Time.timeScale = 24;
		else if (Input.GetKeyUp(KeyCode.P)) Time.timeScale = 1;
		if (Input.GetKeyDown(KeyCode.O)) Camera.main.GetComponent<CameraShake>()?.Shake();
		if (Input.GetKeyDown(KeyCode.Semicolon)) {
			StringBuilder sb = new StringBuilder();
			sb.Append("~/Desktop/Screenshot-");
			for (int i = 0; i < 4; i++) sb.Append(UnityEngine.Random.Range(0, 10));
			sb.Append(".png");
			ScreenCapture.CaptureScreenshot(sb.ToString());
		}
		#endif
	}
}
