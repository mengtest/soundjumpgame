using UnityEngine;
using System.Collections;

using UnityEngine.UI;
[RequireComponent(typeof(AudioSource))]



    public class mainpipixia : MonoBehaviour {
    public GameObject mBgImageTemplateObj;
    public RectTransform mMainPlayer;
    RecordSound mRecordSound = null;
    public Slider mVolumeSilder;
    public GameObject mTips;
    Rigidbody2D mMainPlayerRigidbody;
    ArrayList mListDiMian = new ArrayList();
    float mDimainBaseX = 0;
    float mDimainBaseY = 0;
    float mDiMianSpeed = 100;
    float mDiMianWidth = 100;
    float mMainPlayerBasePosX = 0;
    bool mbStartGame = false;
    bool mPlayerOnDimian = true;

    public InputField mQitiaoVolum;
    public InputField myidongspeed;
    public InputField mdimianmocha;
    public InputField tiaoyuechangdu;
    public InputField mtiaoyuegaodu;
    public InputField mzhongli;
    public GameObject mQiangA;
    public GameObject mQiangB;
    public GameObject mQiangC;

    float mPlayInitPosX = 0;
    float mPlayInitPosY = 0;
    // Use this for initialization
    void Start () {
        mPlayInitPosX = mMainPlayer.anchoredPosition3D.x;
        mPlayInitPosY = mMainPlayer.anchoredPosition3D.y;

        mMainPlayerRigidbody = mMainPlayer.GetComponent<Rigidbody2D>();
        mMainPlayerBasePosX = mMainPlayer.GetComponent<RectTransform>().anchoredPosition3D.x;

        mRecordSound = new RecordSound(this.gameObject);

        mRecordSound.StartRecord();

        mDimainBaseX  = mBgImageTemplateObj.GetComponent<RectTransform>().anchoredPosition3D.x;
        mDimainBaseY = mBgImageTemplateObj.GetComponent<RectTransform>().anchoredPosition3D.y;

        mDiMianWidth = mBgImageTemplateObj.GetComponent<RectTransform>().sizeDelta.x;

        mBgImageTemplateObj.transform.SetAsFirstSibling();
        GameObject dimian1 = (GameObject)Instantiate(mBgImageTemplateObj, mBgImageTemplateObj.transform.position, mBgImageTemplateObj.transform.rotation);
        GameObject dimian2 = (GameObject)Instantiate(mBgImageTemplateObj, mBgImageTemplateObj.transform.position, mBgImageTemplateObj.transform.rotation);
        dimian1.SetActive(true);
        dimian2.SetActive(true);
        dimian1.tag = "dimain";
        dimian2.tag = "dimain";
        Vector3 vPos = dimian1.GetComponent<RectTransform>().anchoredPosition3D;
        vPos.x = mDimainBaseX;
        vPos.y = mDimainBaseY;
        dimian1.transform.SetParent(this.transform);
        dimian1.GetComponent<RectTransform>().anchoredPosition3D = vPos;
        dimian1.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);


        vPos = dimian2.GetComponent<RectTransform>().anchoredPosition3D;
        vPos.x = mDimainBaseX + mDiMianWidth;
        vPos.y = mDimainBaseY;
        dimian2.transform.SetParent(this.transform);
        dimian2.GetComponent<RectTransform>().anchoredPosition3D = vPos;
        dimian2.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);

        mListDiMian.Add(dimian1);
        mListDiMian.Add(dimian2);
        dimian1.transform.SetAsFirstSibling();
        dimian2.transform.SetAsFirstSibling();

    }
    public void StartGame()
    {
        mbStartGame = true;
        mStartGameTimes = 0;
        mPlayerOnDimian = true;

        mTips.SetActive(false);
        // 删除老的wall
        for (int n = 0; n < mListMonster.Count; n++)
        {
            GameObject curM = (GameObject)mListMonster[n];
            curM.SetActive(false);
            Destroy(curM);
        }
        mListMonster.Clear();
        Vector3 vpos = mMainPlayer.anchoredPosition3D;
        vpos.x = mPlayInitPosX;
        vpos.y = mPlayInitPosY;
        mMainPlayer.anchoredPosition3D = vpos;
        mMainPlayer.rotation = Quaternion.identity;
        NewMonster();
    }
    int mStartGameTimes = 0;
    ArrayList mListMonster = new ArrayList();
    public void NewMonster()
    {
        GameObject randM = null;
        int n = Random.Range(1, 3);
        if(n == 0)
        {
            randM = mQiangA;
        }else if(n == 1)
        {
            randM = mQiangB;
        }else if(n == 2)
        {
            randM = mQiangC;
        }
        GameObject newM = (GameObject)Instantiate(randM, mQiangA.transform.position, mQiangA.transform.rotation);

        newM.transform.SetParent(this.transform); 
        Vector3 vCurPos = randM.GetComponent<RectTransform>().anchoredPosition3D;
        newM.GetComponent<RectTransform>().anchoredPosition3D = vCurPos;
        newM.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);

        mListMonster.Add(newM);
    }
    public void StopGame()
    {
        mbStartGame = false;
        mTips.SetActive(true);
    }
    public void ColliderDimain()
    {
        mPlayerOnDimian = true;
    }
    ArrayList mDeadMonster = new ArrayList();
    void deleteDeadMonster()
    {
        mDeadMonster.Clear();
        for (int n = 0; n < mListMonster.Count; n++)
        {
            GameObject curM = (GameObject)mListMonster[n];
            if (curM.GetComponent<RectTransform>().anchoredPosition3D.x < -130)
            {
                mDeadMonster.Add(curM);

            }
        }
        for (int m = 0; m < mDeadMonster.Count; m++)
        {
            mListMonster.Remove(mDeadMonster[m]);
            GameObject curM = (GameObject)mDeadMonster[m];
            Destroy(curM);
        }

    }
    void MoveDimain(float xoffset)
    { 
        GameObject firstDimian = (GameObject)mListDiMian[0];
        GameObject secondDimain = (GameObject)mListDiMian[1];

        Vector3 vPos = firstDimian.GetComponent<RectTransform>().anchoredPosition3D;
        
        vPos.x -= xoffset;
        firstDimian.GetComponent<RectTransform>().anchoredPosition3D = vPos;
        vPos.x += mDiMianWidth;
        secondDimain.GetComponent<RectTransform>().anchoredPosition3D = vPos;

        if (firstDimian.GetComponent<RectTransform>().anchoredPosition3D.x < mDimainBaseX - mDiMianWidth)
        {
            mListDiMian.Remove(firstDimian);

            vPos.x += mDiMianWidth;

            firstDimian.GetComponent<RectTransform>().anchoredPosition3D = vPos;

            mListDiMian.Add(firstDimian);
        }
      
        for (int n = 0;n< mListMonster.Count; n++)
        {
            GameObject obj = (GameObject)(mListMonster[n]);
            Vector3 vCurPos = obj.GetComponent<RectTransform>().anchoredPosition3D;
            vCurPos.x -= xoffset;
            obj.GetComponent<RectTransform>().anchoredPosition3D = vCurPos;
 
        }
        if (mListMonster.Count > 0)
        {
            GameObject backM = (GameObject)mListMonster[mListMonster.Count - 1];
            float localx = backM.GetComponent<RectTransform>().anchoredPosition3D.x;
            if (localx < 260)
            {
                NewMonster();
            }
        }


    }
    void MovePlayer(float xoffset)
    {
        if (mMainPlayer.anchoredPosition3D.x > mMainPlayerBasePosX)
        {
            Vector3 v = mMainPlayer.anchoredPosition3D;
            v.x -= xoffset;
            if (v.x < mMainPlayerBasePosX)
            {
                v.x = mMainPlayerBasePosX;
            }
            mMainPlayer.anchoredPosition3D = v;
            this.MoveDimain(xoffset);
        }
    }

    float mDimainVecl = 0;
    float mTiaoYueUpVel = 0;
    void FixedUpdate()
    {
        mMainPlayerRigidbody.gravityScale = float.Parse(mzhongli.text);
        float volume = mRecordSound.GetVolume(); 
        mVolumeSilder.value = volume;
        if (volume > 0.5f && !mbStartGame)
        {
            this.StartGame();
        }
        else if (mbStartGame)
        {
            mStartGameTimes++;
            if(mStartGameTimes < 20)
            {
                return;
            }
            if (mListDiMian.Count == 2)
            {
                if(mPlayerOnDimian)// 小于0.5且在地面
                {
                    if(volume < float.Parse(mQitiaoVolum.text))
                    {
                        // Debug.Log(volume);
                        mDimainVecl = volume * float.Parse(myidongspeed.text);
                        float xoffset = mDimainVecl * Time.fixedDeltaTime;
                        mDimainVecl += -(float.Parse(mdimianmocha.text) * Time.fixedDeltaTime);
                        if (mDimainVecl < 0)
                        {
                            mDimainVecl = 0;
                        }
                        this.MoveDimain(xoffset);
                    }
                    else
                    {
                        Debug.Log(volume);
                        mDimainVecl = volume * float.Parse(tiaoyuechangdu.text);

                        mMainPlayerRigidbody.velocity = new Vector2(0, (volume * float.Parse(mtiaoyuegaodu.text)));

                  //      mMainPlayerRigidbody.AddRelativeForce(new Vector2(0, (volume    * float.Parse(mtiaoyuegaodu.text))), ForceMode2D.Impulse);
                        mPlayerOnDimian = false;
                        
                        
                    }
                }else
                {
                     float xoffset = mDimainVecl * Time.fixedDeltaTime; 
                     this.MoveDimain(xoffset);

                    // donothing

                }
                

            }
        }
        deleteDeadMonster();


    }
    // Update is called once per frame
    void Update () {
	
	}
}
