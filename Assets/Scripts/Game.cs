using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class Game : MonoBehaviour {
	#region Debug
	//DEBUG
	void Update() {

		if (Input.GetKeyDown (KeyCode.C)) {
			//ToggleMoving (true);
			ChangeLevelSpeed(1.1f);
		}
	}
	#endregion
	#region Init
	
	public static float levelSpeed = 1f;
	public GameObject gStats,gDialogWindow,music;
	public static GameObject question,currentCeleb;
	public Text Score, Health;
	public Button[] button;
	public Image image_slot;
	Sprite[] celebrity_images;
	int idx_for_correct_button;
	public static int health = 3;
	public static int score = 0;
	public Transform propSpawn,celebSpawn;
	public GameObject propPrefab,celebPrefab;
	public static float __velocity = -0.0314f;
	public GameObject floor,shunka;
	Animator floorAnimator,shunkaAnimator;
	public int xp_treshold = 5;
	int current_xp = 0;
	public float level_factor = 1.1f;

	void Start () {
		floorAnimator = floor.GetComponent<Animator> ();
		shunkaAnimator = shunka.GetComponent<Animator> ();

		Instantiate (music);
		question = gDialogWindow;
		celebrity_images =  Resources.LoadAll<Sprite>("Art");
		RestartGame ();
		CreateProp ();
	}
	#endregion

	#region SetQuestion
		public void SetQuestion() {
		
			//izaberi na koje dugme ce se ispisati tačan odgovor
			idx_for_correct_button = Random.Range (0, 3);																

			//izaberi poznatu licnosti i namesti novu sliku
			int celeb_idx = TakeRandomFromImagePool(ImagePool);
			image_slot.sprite = celebrity_images [celeb_idx];

			//ispisi tacan odgovor na odgovarajuce dugme
			Text bt_text = button [idx_for_correct_button].GetComponentInChildren<Text> ();
			bt_text.text = celebrity_images [celeb_idx].name.Substring (3);


			//napravi celeb instance
			string type = celebrity_images [celeb_idx].name.Substring (0,2);
			CreateCeleb (type);

			//oduzmi izabranu licnost iz names pool-a da ne bi upalo na dva dugmeta isto ime
			NamesPool.Remove(celebrity_images [celeb_idx].name.Substring (3));


			//ispisi netacne odgovore
			for (int i = 0; i < button.Length; i++) {
				if (i != idx_for_correct_button) {																		
					button [i].GetComponentInChildren<Text> ().text = TakeRandomFromNamesPool (NamesPool);
				}
			}
		}
	#endregion

	#region Answer
	public bool CheckAnswer(int button_idx) {
		currentCeleb.GetComponent<Celeb> ().isActive = false;
		ShowQuestion (false);
		if (button_idx == idx_for_correct_button) {
		//	print ("To je tačan odgovor!");
			score += Mathf.RoundToInt (10*levelSpeed);
			currentCeleb.GetComponent<MoveLeft> ().velocity *= 5;
			CheckLevel ();
		} else {
			//print ("GREŠKA");
			health--;
			currentCeleb.GetComponent<MoveLeft> ().velocity *= -9;
		}

		UpdateScore ();
		UpdateHealth ();
		SetQuestion ();
		return false;
	}


	public void AnswerOne() {
		CheckAnswer (0);
	}

	public void AnswerTwo() {
		CheckAnswer (1);
	}

	public void AnswerThree() {
		CheckAnswer (2);
	}


	void UpdateScore() {
		Score.text = "Score:" + score.ToString();
	}

	void UpdateHealth() {
		string h = "<";
		for (int i = 0; i < health; i++) {
			h += "3";
		}
		Health.text = h;
			
		if (health <= 0) {
			Health.text = "";
			DieAndShowStats ();
		}
	}
	#endregion
	///////////////////////////////////////////////////////////////////////////////////////

	public static void ToggleMoving(bool status){
		if (status == false) {
			MoveLeft._velocity = 0;
			Time.timeScale = 0;	
		} else {
			MoveLeft._velocity = -0.0314f;
			Time.timeScale = 1;
		}
	}
	void DieAndShowStats() {
		ToggleMoving (false);
		gDialogWindow.SetActive (false);
		gStats.SetActive (true);
	}

	public void RestartGame() {
		levelSpeed = 1f;
		ChangeLevelSpeed (1f);

		ToggleMoving (true);
		gDialogWindow.SetActive (false);
		gStats.SetActive (false);

		health = 3;
		score = 0;

		SetQuestion ();
		UpdateScore ();
		UpdateHealth ();
		RecreateImagePool ();
	}

	public void Quit() {
		Application.Quit ();
	}

	#region ImagePool
	//slike treba da uzima random jednu po jednu iz poola dok se ne isprazne sve, i nakon toga da rekreira pool
	//ponudjene odgovore treba da uzima iz posebnog string niza zavisno od pola i da poredi sa tacnim odgovorom
	List<int> ImagePool = new List<int>();
	int TakeRandomFromImagePool(List<int> pool) {
		if (pool.Count > 0) {
			int idx = Random.Range (0, pool.Count);
			int ret = pool [idx];
			pool.RemoveAt (idx);
			return ret;	
		} else {
			RecreateImagePool ();
			return TakeRandomFromImagePool(pool);
		}
	}

	void RecreateImagePool(){
		ImagePool.Clear ();
		for (int i = 0; i < celebrity_images.Length; i++) {
			ImagePool.Add (i);
		}
	}
	#endregion
	#region Names Pool
	List<string> NamesPool = new List<string>();
	string TakeRandomFromNamesPool(List<string> pool) {
		if (pool.Count > 0) {
			int idx = Random.Range (0, pool.Count);
			string ret = pool [idx];
			pool.RemoveAt (idx);
			return ret;
		} else {
			RecreateNamesPool (NamesPool);
			return TakeRandomFromNamesPool(pool);
		}
	}

	void RecreateNamesPool(List<string> pool){
		pool.Clear ();
		for (int i = 0; i < celebrity_images.Length; i++) {
			string justname = celebrity_images [i].name.Substring (3);
			pool.Add (justname);
		}
	}
	#endregion


	void CreateProp(){
		Instantiate (propPrefab, propSpawn.position, Quaternion.identity);

		float time = Random.Range (1f, 5f);
		Invoke ("CreateProp", time);
	}

	void CreateCeleb(string type){
		GameObject g = Instantiate (celebPrefab, celebSpawn.position, Quaternion.identity) as GameObject;
		Celeb instance = g.GetComponent<Celeb> ();
		if (type == "fb") {
			instance.SetImage (0);
		}
		if (type == "fw") {
			instance.SetImage (1);
		}
		if (type == "mb") {
			instance.SetImage (2);
		}
		if (type == "mw") {
			instance.SetImage (3);
		}

		float time = Random.Range (2f, 7f);
		//Invoke ("CreateCeleb", time);
	}

	public static void ShowQuestion(bool status) {
		if (question != null) {
			question.SetActive (status);
		}
	}

	public void ChangeLevelSpeed(float factor) {
		levelSpeed *= factor;
		floorAnimator.speed = levelSpeed;
		shunkaAnimator.speed = levelSpeed;
	}

	public void CheckLevel() {
		current_xp ++ ;
		if (current_xp % xp_treshold == 0) {
			//print ("JESTE");
			ChangeLevelSpeed (level_factor);
		}

	}


}

