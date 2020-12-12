using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

	// make game manager public static so can access this from other scripts
	public static GameManager gm;

	// public variables
	public int score=0;

	public bool canBeatLevel = false;
	public int beatLevelScore=0;

	public float startTime=5.0f;
	
	public Text mainScoreDisplay;
	public Text mainTimerDisplay;
	public Text timerPenaltyDisplay;
	public float resetPenaltyAfterTime = 0.5f;

	public GameObject gameOverScoreOutline;
	public GameObject timerPenaltyUI;
	public GameObject timerPenaltyOutline;

	public AudioSource musicAudioSource;

	public bool gameIsOver = false;

	public GameObject playAgainButtons;
	public string playAgainLevelToLoad;

	public GameObject nextLevelButtons;
	public string nextLevelToLoad;

	private float currentTime;
	private float resetTime = 0;
	private Color initialColor = new Color(90f/255f, 0, 0, 100f / 255f);
	private Color addRedColor = new Color(55f / 255f, 0, 0, 0);
	private Color greenColor = new Color(0, 1, 0, 100f / 255f);
	private Vector3 playerPosition;
	private Vector3 position;
	private float pos, neg;

	// setup the game
	void Start () {

		// set the current time to the startTime specified
		currentTime = startTime;

		// get a reference to the GameManager component for use by other scripts
		if (gm == null) 
			gm = this.gameObject.GetComponent<GameManager>();

		// init scoreboard to 0
		mainScoreDisplay.text = "0";

		// inactivate the gameOverScoreOutline gameObject, if it is set
		if (gameOverScoreOutline)
			gameOverScoreOutline.SetActive (false);

		// inactivate the playAgainButtons gameObject, if it is set
		if (playAgainButtons)
			playAgainButtons.SetActive (false);

		// inactivate the nextLevelButtons gameObject, if it is set
		if (nextLevelButtons)
			nextLevelButtons.SetActive (false);

		// inactivate the TimerPenalty gameObject and set its color to black, if it is set
		if (timerPenaltyUI)	{
			timerPenaltyUI.SetActive(false);
			timerPenaltyUI.GetComponent<Image>().color = initialColor;
		}

		// inactivate the Penalty Timer Outline gameObject, if it is set
		if (timerPenaltyOutline)
			timerPenaltyOutline.SetActive(false);

		Transform floor = GameObject.FindWithTag("Floor").transform;
		pos = floor.position.x + 4f * floor.localScale.x - 8;
		neg = floor.position.x - 4f * floor.localScale.x + 8;
	}

	// this is the main game event loop
	void Update () {
		if (!gameIsOver)	{
			if (canBeatLevel && (score >= beatLevelScore))
			{  // check to see if beat game
				BeatLevel();
			}
			else if (currentTime < 0)
			{ // check to see if timer has run out
				EndGame();
			}
			else
			{ // game playing state, so update the timer
				currentTime -= Time.deltaTime;
				mainTimerDisplay.text = currentTime.ToString("0.00");
			}
		}
		else	{
			playerPosition = GameObject.FindWithTag("Player").transform.position;
			if (playerPosition.x > pos)
				position = new Vector3(pos, 0, 0);
			else if (playerPosition.x < neg)
				position = new Vector3(neg, 0, 0);
			else
				position = new Vector3(playerPosition.x, 0, 0);
			playAgainButtons.transform.SetPositionAndRotation(position, playAgainButtons.transform.rotation);
			nextLevelButtons.transform.SetPositionAndRotation(position, nextLevelButtons.transform.rotation);
		}

		// if it is time to reset the Penalty Timer UI
		if (resetTime != 0 && Time.time > resetTime)	{
			resetTime = 0;
			// reset the Timer Penalty Display animation
			if (timerPenaltyDisplay)
				timerPenaltyDisplay.GetComponent<Animator>().SetBool("Continuous", false);
			// inactivate the Timer Penalty gameObject and set its color to black
			if (timerPenaltyUI)	{
				timerPenaltyUI.SetActive(false);
				timerPenaltyUI.GetComponent<Image>().color = initialColor;
			}
			// inactivate the Timer Penalty Outline gameObject
			if (timerPenaltyOutline)
				timerPenaltyOutline.SetActive(false);
		}
	}

	void EndGame() {
		// game is over
		gameIsOver = true;

		// repurpose the timer to display a message to the player
		mainTimerDisplay.text = "GAME OVER";

		// activate the gameOverScoreOutline gameObject, if it is set 
		if (gameOverScoreOutline)
			gameOverScoreOutline.SetActive (true);
	
		// activate the playAgainButtons gameObject, if it is set 
		if (playAgainButtons)
			playAgainButtons.SetActive (true);

		// reduce the pitch of the background music, if it is set 
		if (musicAudioSource)
			musicAudioSource.pitch = 0.5f; // slow down the music
	}
	
	void BeatLevel() {
		// game is over
		gameIsOver = true;

		// repurpose the timer to display a message to the player
		mainTimerDisplay.text = "LEVEL COMPLETE";

		// activate the gameOverScoreOutline gameObject, if it is set 
		if (gameOverScoreOutline)
			gameOverScoreOutline.SetActive (true);

		// activate the nextLevelButtons gameObject, if it is set 
		if (nextLevelButtons)
			nextLevelButtons.SetActive (true);
		
		// reduce the pitch of the background music, if it is set 
		if (musicAudioSource)
			musicAudioSource.pitch = 0.5f; // slow down the music
	}

	// public function that can be called to update the score or time
	public void targetHit(int scoreAmount, float timeAmount)	{
		// increase the score by the scoreAmount and update the text UI
		score += scoreAmount;
		mainScoreDisplay.text = score.ToString();

		// increase the time by the timeAmount
		currentTime += timeAmount;

		// don't let it go negative
		if (currentTime < 0)
			currentTime = 0.0f;

		// update the text UI
		mainTimerDisplay.text = currentTime.ToString("0.00");

		if (timeAmount != 0)	{
			if (!(resetPenaltyAfterTime > 0))
				resetPenaltyAfterTime = 0.5f;
			resetTime = Time.time + resetPenaltyAfterTime;
			if (timerPenaltyDisplay)
				timerPenaltyDisplay.text = timeAmount.ToString("0.0");
			if (timerPenaltyUI)	{
				timerPenaltyUI.SetActive(true);
				if (timeAmount > 0)	{
					timerPenaltyDisplay.text = "+" + timerPenaltyDisplay.text;
					timerPenaltyDisplay.GetComponent<Animator>().SetBool("Continuous", false);
					timerPenaltyOutline.SetActive(false);
					timerPenaltyUI.GetComponent<Image>().color = greenColor;
				}
				else	{
					// if penalty is received more than once, the background will became more red
					if (timerPenaltyUI.GetComponent<Image>().color.g == 1)
						timerPenaltyUI.GetComponent<Image>().color = initialColor;
					if (timerPenaltyUI.GetComponent<Image>().color.r < 1)
						timerPenaltyUI.GetComponent<Image>().color += addRedColor;
					else	{
						// if it can't be more red, it stops flashing the Timer Penalty Display and show the Timer Penalty Outline
						timerPenaltyDisplay.GetComponent<Animator>().SetBool("Continuous", true);
						if (timerPenaltyOutline)
							timerPenaltyOutline.SetActive(true);
					}
				}
			}
		}
	}

	// public function that can be called to restart the game
	public void RestartGame ()
	{
		// we are just loading a scene (or reloading this scene)
		// which is an easy way to restart the level
        SceneManager.LoadScene(playAgainLevelToLoad);
	}

	// public function that can be called to go to the next level of the game
	public void NextLevel ()
	{
		// we are just loading the specified next level (scene)
        SceneManager.LoadScene(nextLevelToLoad);
	}
	

}
