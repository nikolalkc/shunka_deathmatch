using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class Game : MonoBehaviour {
	#region Debug
	//DEBUG
	void Update() {

		if (Input.GetKeyDown (KeyCode.C)) {
			//SetQuestion ();
			//print(TakeRandomFromImagePool(ImagePool));


			if (Time.timeScale == 1) {
				MoveLeft._velocity = 0;
				Time.timeScale = 0;	
			} else {
				MoveLeft._velocity = -0.0314f;
				Time.timeScale = 1;
			}

		}
	}
	#endregion
	#region Init
	public GameObject gStats,gDialogWindow;
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


	void Start () {
		celebrity_images =  Resources.LoadAll<Sprite>("Art");
		RestartGame ();
		CreateProp ();
		CreateCeleb ();
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
			bt_text.text = "**" + celebrity_images [celeb_idx].name.Substring (3)  + "**";

			//oduzmi izabranu licnost iz names pool-a da ne bi upalo na dva dugmeta isto ime
			NamesPool.Remove(celebrity_images [celeb_idx].name);


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
		if (button_idx == idx_for_correct_button) {
		//	print ("To je tačan odgovor!");
			score++;
		} else {
			//print ("GREŠKA");
			health--;
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
		Health.text = "Health:" + health.ToString();
		if (health <= 0) {
			DieAndShowStats ();
		}
	}
	#endregion
	///////////////////////////////////////////////////////////////////////////////////////


	void DieAndShowStats() {
		gDialogWindow.SetActive (false);
		gStats.SetActive (true);
	}

	public void RestartGame() {
		gDialogWindow.SetActive (true);
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

		float time = Random.Range (2f, 7f);
		Invoke ("CreateProp", time);
	}

	void CreateCeleb(){
		Instantiate (celebPrefab, celebSpawn.position, Quaternion.identity);

		float time = Random.Range (2f, 7f);
		Invoke ("CreateCeleb", time);
	}



}

