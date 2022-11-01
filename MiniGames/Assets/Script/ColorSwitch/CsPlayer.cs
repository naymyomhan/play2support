using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class CsPlayer : MonoBehaviour
{   	
	public static CsPlayer Instance;
    void Awake() {
         if(Instance==null)
            Instance=this;
    }

	//Player
    public float jumpForce = 10f;
	public Rigidbody2D rb;
	public SpriteRenderer sr;
	public Vector3 initial_position;

	public GameObject cam;
	public GameObject playerHolder;


	//Color Changer
	public string currentColor;

	public Color colorGreen;
	public Color colorYellow;
	public Color colorBlue;
	public Color colorOrange;

    public GameObject colorDiePrefab;
    public GameObject eatStarPrefab;
    public GameObject colorChangePrefab;
    public GameObject jumpEffectPrefab;

	public int starCount;

	//Level Obstacles
	public GameObject[] obstaclesPrefabsOne;
	public GameObject[] obstaclesPrefabsTwo;
	public GameObject[] obstaclesPrefabsThree;
	int modY;
	private int previousSpawnedObstacleIndex;

	//UI
	public TextMeshProUGUI starCountUI;
	public GameObject gameOverUI;
	public GameObject inGameUI;
	public GameObject continueMenu;
	public GameObject pausedMenu;
	public TextMeshProUGUI highScore;

	public Color[] bg_colors;
	public GameObject background;

	//Player State
	private GameObject dieObstacle;



	void Start ()
	{
		StartGame();
		ColorSwitchAudio.Instance.PlayTheme();
	}

	void StartGame(){
		transform.position=initial_position;
		gameObject.SetActive(true);
		playerHolder.SetActive(true);
		playerHolder.transform.position=new Vector3(0,-9f,0);
		transform.rotation=Quaternion.Euler(0, 0, 0);
		SetRandomColor();
		SetRandBgColor();
		int secondObstacleIndex = Random.Range(0, obstaclesPrefabsOne.Length);
		Vector3 firstObstaclePosition=new Vector3(0,20,0);
		Vector3 secondObstaclePosModifier=new Vector3(0,16,0);
		GameObject firstObstacle = Instantiate(obstaclesPrefabsOne[0],transform.position+firstObstaclePosition,Quaternion.identity);
		Instantiate(obstaclesPrefabsOne[secondObstacleIndex],firstObstacle.transform.position+secondObstaclePosModifier,Quaternion.identity);
		previousSpawnedObstacleIndex=secondObstacleIndex;
		UpdateGui();
		GameManager.Instance.isPaused=false;
		Time.timeScale=1;
		Debug.Log(SystemInfo.deviceUniqueIdentifier);
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown("Jump") || Input.GetMouseButtonDown(0))
		{
			rb.velocity = Vector2.up * jumpForce;
			Instantiate(jumpEffectPrefab,transform.position,Quaternion.identity);
			rb.AddTorque(100, ForceMode2D.Impulse);
			if(!GameManager.Instance.isPaused){
				playerHolder.SetActive(false);
				ColorSwitchAudio.Instance.Jump();
			}
		}
	}

	public void Pause(){
		pausedMenu.SetActive(true);
		inGameUI.SetActive(false);
		Time.timeScale=0;
		GameManager.Instance.isPaused=true;
	}

	public void Resume(){
		pausedMenu.SetActive(false);
		inGameUI.SetActive(true);
		Time.timeScale=1;
		GameManager.Instance.isPaused=false;
	}

	void OnTriggerEnter2D (Collider2D col)
	{
		if (col.tag == "ColorChanger")
		{
			SetRandomColor();
			Destroy(col.gameObject);
            Instantiate(colorChangePrefab,transform.position,transform.rotation);
			ColorSwitchAudio.Instance.ColorChange();
			return;
		}

		if(col.tag == "Star")
		{
			Destroy(col.gameObject);
			starCount++;
			UpdateGui();
            Instantiate(eatStarPrefab,transform.position,transform.rotation);
			ColorSwitchAudio.Instance.EatStar();

			//create new obstacle
			GameObject[] targetPrefab;
			if(starCount<=6){
				targetPrefab=obstaclesPrefabsOne;
			}else if(starCount>6&& starCount<=15){
				targetPrefab=obstaclesPrefabsTwo;
			}else if(starCount>15){
				targetPrefab=obstaclesPrefabsThree;
			}else{
				targetPrefab=obstaclesPrefabsThree;
			}
			modY=34;

			int newObstacleIndex = Random.Range(0, targetPrefab.Length);
			Instantiate(targetPrefab[newObstacleIndex],transform.position+new Vector3(0,modY,0),Quaternion.identity);
			previousSpawnedObstacleIndex=newObstacleIndex;
			return;
		}

		if (col.tag != currentColor)
		{	
			if(starCount>0){
				Invoke("TempGameOver",1f);
			}else{
				Invoke("GameOver",1f);
			}
			dieObstacle=col.transform.root.gameObject;
            gameObject.SetActive(false);
			ColorSwitchAudio.Instance.Die();
            Instantiate(colorDiePrefab,transform.position,transform.rotation);
			
			//update high score
			int currentBest=PlayerPrefs.GetInt("Best",0);
			if(currentBest<starCount){
				PlayerPrefs.SetInt("Best",starCount);
			}
		}
	}

	void GameOver(){
		GameManager.Instance.isPaused=true;
		inGameUI.SetActive(false);
		gameOverUI.SetActive(true);
		GameManager.Instance.AddStar(starCount);
		highScore.text="High Score : "+ PlayerPrefs.GetInt("Best",0);
	}

	void TempGameOver(){
		GameManager.Instance.isPaused=true;
		inGameUI.SetActive(false);
		continueMenu.SetActive(true);
	}

	void UpdateGui(){
		starCountUI.text=starCount.ToString();
	}

	void SetRandomColor ()
	{
		int index = Random.Range(0, 4);

		switch (index)
		{
			case 0:
				currentColor = "Green";
				sr.color = colorGreen;
				break;
			case 1:
				currentColor = "Yellow";
				sr.color = colorYellow;
				break;
			case 2:
				currentColor = "Blue";
				sr.color = colorBlue;
				break;
			case 3:
				currentColor = "Orange";
				sr.color = colorOrange;
				break;
		}
	}

	public void Restart(){
		GameObject[] obstacles = GameObject.FindGameObjectsWithTag("Obstacle");
		foreach(GameObject obstacle in obstacles){GameObject.Destroy(obstacle);}
		cam.transform.position=new Vector3(0,0,-10);
		starCount=0;
		gameObject.SetActive(false);
		StartGame();
		inGameUI.SetActive(true);
		gameOverUI.SetActive(false);
		pausedMenu.SetActive(false);
	}

	public void Continue(){
		//reborn the player
		gameObject.transform.position=new Vector3(0,dieObstacle.transform.position.y-13f,0);
		GameObject[] obstacles = GameObject.FindGameObjectsWithTag("Obstacle");
		foreach(GameObject obstacle in obstacles){
			if(obstacle.transform.position.y<transform.position.y){
				GameObject.Destroy(obstacle);
			}
		}
        gameObject.SetActive(true);
		transform.rotation=Quaternion.Euler(0, 0, 0);
		playerHolder.SetActive(true);
		playerHolder.transform.position=transform.position-new Vector3(0,1.2f,0);
		cam.transform.position=transform.position+new Vector3(0,7f,-10f);

		inGameUI.SetActive(true);
		continueMenu.SetActive(false);
		GameManager.Instance.isPaused=false;
	}

	public void Next(){
		continueMenu.SetActive(false);
		gameOverUI.SetActive(true);
		GameManager.Instance.AddStar(starCount);
		highScore.text="High Score : "+ PlayerPrefs.GetInt("Best",0);
	}

	public void SetRandBgColor(){
		int bgColorIndex = Random.Range(0, 3);
		background.GetComponent<SpriteRenderer>().color = bg_colors[bgColorIndex];
	}
	
}
