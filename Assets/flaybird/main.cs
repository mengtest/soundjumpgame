using UnityEngine;
using System.Collections;
using UnityEngine.UI;

//录音
public class RecordSound{
	string [] mMicDevicesArray = null; 
 
	public AudioSource mAudioSource;
	public RecordSound(GameObject obj){
		mAudioSource = obj.GetComponent<AudioSource>();
        mMicDevicesArray = Microphone.devices;
		if (mMicDevicesArray.Length == 0)
		{
			Debug.LogError("Microphone.devices is null");
		}
		foreach (string deviceStr in mMicDevicesArray)
		{
			Debug.Log("device name = " + deviceStr);
		}
	}
	public void StartRecord(){
		if (!mAudioSource)
			return;
		mAudioSource.Stop ();
		if (mMicDevicesArray.Length == 0) {
			Debug.Log ("No Record Device!");
			return;
		} 
		//mAudioSource.mute = true;  
		mAudioSource.clip = Microphone.Start ("Built-in Microphone", true, 2, 44100); 
		while (!(Microphone.GetPosition (null) > 0)) {
		}
		mAudioSource.Play ();
	}
    private float Sum(params float[] samples)
    {
        float result = 0.0f;
        for (int i = 0; i < samples.Length; i++)
        {
            float qiangdu = Mathf.Abs(samples[i]);
            if(qiangdu > result)
            {
                result = qiangdu;
            }
           // result += Mathf.Abs(samples[i]);
        }
        return result;
    }
    public float GetVolume(){

        if (Microphone.IsRecording(null))
		{
             int curpos = Microphone.GetPosition(null); 
            // 采样数
            int sampleSize = 256;
			float[] samples = new float[sampleSize];
            int startPosition = Microphone.GetPosition(null) - (sampleSize+1);
            if(startPosition < 0)
            {
                startPosition = 0;
            }
			// 得到数据
			mAudioSource.clip.GetData(samples, startPosition);
            return Sum(samples);// / sampleSize;

		}
		return 0;
	}
	 
}

public class GroupWall{
	public GameObject mWallGroup;
	public RectTransform mWallGroupRectTransform;
	public RectTransform mTopWall;
	public RectTransform mBottomWall;

	public float mCurVelBottom = 5;
	public float mCurVelTop = 5;
	public float mCurTopY = 0;
	public float mCurBottomY = 0;

	const float topwallmaxy = 40;
	const float topwallminy = -200;
	const float bottomwallmaxy = 220;
	const float bottomwallminy = -120;

    main mMainobj;
    public GroupWall(GameObject newwallobj,main mainobj){
        mMainobj = mainobj;
        mWallGroup = newwallobj;
		mTopWall = newwallobj.transform.FindChild ("walltop").GetComponent<RectTransform> ();
		mBottomWall = newwallobj.transform.FindChild ("wallbottom").GetComponent<RectTransform> ();

		mWallGroupRectTransform = mWallGroup.GetComponent<RectTransform> ();
		Vector3 vCurPos = mWallGroupRectTransform.anchoredPosition3D;
		vCurPos.x = main.mGroupWallInitPos;
		mWallGroupRectTransform.anchoredPosition3D = vCurPos;
		mWallGroupRectTransform.localScale = new Vector3 (1, 1, 1);
        
        mCurVelTop = -float.Parse(mMainobj.mSuiguanSudu.text);// Random.Range (-80, -40);
        mCurVelBottom = float.Parse(mMainobj.mSuiguanSudu.text);// Random.Range (-80, -40);
		mCurTopY = Random.Range (topwallminy, topwallmaxy);
		mCurBottomY = Random.Range (bottomwallminy, bottomwallmaxy);

	}
	public void Update(float xoffset){

		Vector3 vCurPos = mWallGroupRectTransform.position;
		vCurPos.x -= xoffset;
		float fdisbottomy = Time.fixedDeltaTime* mCurVelBottom;
		float fdistopy = mCurVelTop * Time.fixedDeltaTime;
		mCurTopY += fdistopy;
		mCurBottomY += fdisbottomy;

		if (mCurTopY < topwallminy || mCurTopY > topwallmaxy) {
			mCurVelTop = -mCurVelTop;
		}
		if (mCurBottomY < bottomwallminy || mCurBottomY > bottomwallmaxy) {
			mCurVelBottom = -mCurVelBottom;
		}
		mWallGroupRectTransform.position = vCurPos;

		Vector3 v = mTopWall.anchoredPosition3D;
		v.y = mCurTopY;
		mTopWall.anchoredPosition3D = v; 
		v = mBottomWall.anchoredPosition3D;
		v.y = mCurBottomY;
		 mBottomWall.anchoredPosition3D = v;
	}
	public float GetXPos(){
        return mWallGroupRectTransform.anchoredPosition3D.x;
	}
}
[RequireComponent(typeof(AudioSource))]
public class main : MonoBehaviour { 
	public GameObject mWallGroupSource;
	public RectTransform mMainPlayer;
	public GameObject mTips;
    public InputField mBirdXSpeed;
    public InputField mBirdYForce;
    public InputField mSuiguanSudu;
    public InputField mZhonglijiasudu;

	Rigidbody2D mMainPlayerRigidbody;
	public Slider mVolumeSilder;
	RecordSound mRecordSound = null;
	bool mbStartGame = false;
    float mStartGameTime = 0;
    // bool 
    ArrayList mListWall = new ArrayList();

	float mPlayInitPosX = 0;
	float mPlayInitPosY = 0;
    // 
    public AudioSource mDieSound;
    public AudioSource mStartGameSound;
    // config
    public static float mGroupWallInitPos = 780; 
    // Use this for initialization
    void Start() {
        
		mRecordSound = new RecordSound (this.gameObject);

		mRecordSound.StartRecord();

		mPlayInitPosX = mMainPlayer.anchoredPosition3D.x;
		mPlayInitPosY = mMainPlayer.anchoredPosition3D.y;

        mMainPlayerRigidbody = mMainPlayer.GetComponent<Rigidbody2D>();

        
    }

	ArrayList mdeadWall = new ArrayList ();
	void deleteDeadWall(){
		mdeadWall.Clear ();
		for (int n = 0; n < mListWall.Count; n++) {
			GroupWall curwall = (GroupWall)mListWall[n];
			if (curwall.GetXPos() < -40) {
				mdeadWall.Add (curwall);

			}
		}
		for (int m = 0; m < mdeadWall.Count; m++) {
			mListWall.Remove(mdeadWall[m]);
			GroupWall curwall = (GroupWall)mdeadWall[m];
			Destroy(curwall.mWallGroup);
		}
		 
	}
	public void NewGroupWall(){
		GameObject newWallGroup = (GameObject)Instantiate(mWallGroupSource,mWallGroupSource.transform.position,mWallGroupSource.transform.rotation);

        newWallGroup.transform.SetParent(this.transform);
		GroupWall newGroup = new GroupWall (newWallGroup,this);

		mListWall.Add(newGroup);
	}
	public void StartGame(){
		mbStartGame = true;
        mStartGameTime = 0;
        // 删除老的wall
        for (int n = 0; n < mListWall.Count; n++) {
			GroupWall curwall = (GroupWall)mListWall[n];
			Destroy(curwall.mWallGroup);
		}
		mListWall.Clear ();
        // 还原主角y位置
        mMainPlayerRigidbody.isKinematic = false;
        Vector3 vpos = mMainPlayer.anchoredPosition3D;
		vpos.x = mPlayInitPosX;
		vpos.y = mPlayInitPosY;
		mMainPlayer.anchoredPosition3D = vpos;

		this.NewGroupWall(); 
        mTips.SetActive (false);
        mStartGameSound.Play();

    }
	public void StopGame(){
		mbStartGame = false;
        mMainPlayerRigidbody.isKinematic = true;

        mTips.SetActive (true);
        mDieSound.Play();

    }
	void FixedUpdate(){
		float volume = mRecordSound.GetVolume();
		if (volume > 0.5f && !mbStartGame) {
			this.StartGame ();
		}else if (mbStartGame) {
            mStartGameTime += Time.fixedDeltaTime;
       
            mVolumeSilder.value = volume;

            float wallgroupspeed = float.Parse(mBirdXSpeed.text);
			float walldistance =  Time.fixedDeltaTime * wallgroupspeed;
            //Debug.Log(volume);
            // if (volume > 0.4)
            float yforce = float.Parse(mBirdYForce.text);
                 mMainPlayerRigidbody.AddForce (new Vector2 (0, volume* yforce));
                
			for (int n = 0; n < mListWall.Count; n++) {
				GroupWall curWall = (GroupWall)mListWall[n];

				curWall.Update (walldistance);

			}
			// 检查最后一个来确定是否需要添加新的wall
			if (mListWall.Count > 0) {
				GroupWall backWall = (GroupWall)mListWall [mListWall.Count - 1];
				float localx = backWall.GetXPos();
				if ( localx< 400) {
					NewGroupWall ();
				}
			}
			// 删除过线的wall
			deleteDeadWall (); 

			// 
			if (mMainPlayer.anchoredPosition3D.y < -680 || mMainPlayer.anchoredPosition3D.y > 620) {
				StopGame ();
			}
		}

	}
	// Update is called once per frame
	void Update () {
        mMainPlayerRigidbody.gravityScale = float.Parse(mZhonglijiasudu.text);
    }
}
