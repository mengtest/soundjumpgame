using UnityEngine;
using System.Collections;

public class playertriger : MonoBehaviour {
	public main mMain; 
	// Use this for initialization
	void Start () { 
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    
	void OnCollisionEnter2D(Collision2D coll) {
		mMain.StopGame ();
    }
}
