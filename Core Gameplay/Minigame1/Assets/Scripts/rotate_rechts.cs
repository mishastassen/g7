using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class rotate_rechts : MonoBehaviour {
	
	// set the GameObjects
	public GameObject cirkel_rechts;
	public GameObject pijl_rechts;
	
	// initialize the speed;
	public int speed;
	
	// boolean if the arrow is in the green part
	bool erin;
	
	// origin of the 2
	Vector3 pos_cirkel_rechts;
	Vector3 pos_pijl_rechts;
	
	// defines if the arrow goes CW or CCW
	int way;
	
	// attributes for the score
	private int count;
	public Text scoreText;
	public bool finished_right;
	
	// Use this for initialization
	void Start () {
		pos_pijl_rechts = pijl_rechts.transform.position;
		pos_cirkel_rechts = cirkel_rechts.transform.position;
		
		// Set initial variables and a random turn
		way = 1;
		erin = false;
		RandomTurn ();
		count = 0;
		setScoreText ();
		finished_right = false;
	}
	
	// Update is called once per frame
	void Update () {
		// rotate the arrow every frame
		pijl_rechts.transform.RotateAround (pos_pijl_rechts, Vector3.forward, -speed* way * Time.deltaTime);
		
		// if pressed correctly
		if (erin && Input.GetKeyDown ("m")) 
		{
			way = way * -1;
			RandomTurn();
			count = count + 1;
			setScoreText();
		}
		
		// if not pressed correctly
		if (!erin && Input.GetKeyDown ("m")) 
		{
			count = 0;
			setScoreText();
		}
		
		if (count > 7) {
			way = 0;
			finished_right = true;
		}
		
		
	}
	
	// what happens when the arrow enters
	void OnTriggerEnter (Collider other){
		
		if (other.gameObject.name == "redgreen_right")
		{
			erin = true;
		}
	}
	
	// what happens when the arrow exits
	void OnTriggerExit (Collider other){
		if (other.gameObject.name == "redgreen_right")
		{
			erin = false;
		}
	}
	
	// random turn function
	void RandomTurn (){
		int move = Random.Range (0, 360);
		cirkel_rechts.transform.RotateAround (pos_cirkel_rechts, Vector3.forward, move );
	}
	
	void setScoreText(){
		scoreText.text = count.ToString ();
	}
	
}