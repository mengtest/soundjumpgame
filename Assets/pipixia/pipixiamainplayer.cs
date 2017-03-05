using UnityEngine;
using System.Collections;

public class pipixiamainplayer : MonoBehaviour {
    public mainpipixia mMain;
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    void OnCollisionEnter2D(Collision2D coll)
    {
        if(coll.gameObject.tag == "dimain")
        {
            mMain.ColliderDimain();
        }
        else
            mMain.StopGame();
    }
}
