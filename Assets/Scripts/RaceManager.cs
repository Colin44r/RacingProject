using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;



public class RaceManager : MonoBehaviour
{
    private static RaceManager instance;
    public static RaceManager Instance => instance;

    [SerializeField] private RaceData mRaceConfig;
    [SerializeField] private AICar[] mAICars;
    [SerializeField] private GameObject mCheckpointContainer;
    [SerializeField] private Cars mPlayerCar;
    [SerializeField] private Texture2D mStartRaceImage;
    [SerializeField] private Texture2D mDigitOneImage;
    [SerializeField] private Texture2D mDigitTwoImage;
    [SerializeField] private Texture2D mDigitThreeImage;


    private Rigidbody[] mAIBodies;
    private float [] mRespawnCounters;
    private float [] mDistanceLeft;
    private float mCountdownTimerStartTime;
    private Transform[] mWaypoints;
    private int[] mLaps;
    private int mPlayerLaps = 0;
    private int mCurrentCheckpoint = 0;
    private int mCountdownTimerDelay;
    private CheckPoint[] mCheckPoints;
    private Rigidbody mPlayerBody;
    private bool mGameStart = false;
    public bool GameStart => mGameStart; //Getter

    private void Awake()
    {
        if (instance != null && instance != this)
        { 
            Destroy(this.gameObject);
            return;
        }
        else 
        {
            instance = this;
        }

        CountdownTimerReset(3);
    }

    private void Start()
    {
        mAIBodies = new Rigidbody[mAICars.Length];
        mRespawnCounters = new float[mAICars.Length]; 
        mDistanceLeft = new float[mAICars.Length];
        mWaypoints = new Transform[mAICars.Length];
        mLaps = new int[mAICars.Length];
        mCheckPoints = mCheckpointContainer.GetComponentsInChildren<CheckPoint>(); 
        mPlayerBody = mPlayerCar.GetComponent<Rigidbody>();

        for (int i = 0; i < mAICars.Length; i++)
        {
           // Debug.Log(mAIBodies[i]);
            mAIBodies[i] = mAICars[i].gameObject.GetComponent<Rigidbody>();
            mRespawnCounters[i] = mRaceConfig.ReSpawnDelay;
            mDistanceLeft[i] = float.MaxValue;
            mLaps[i] = 0;

        }
    }

    private void Update()
    {
        var carsFinished = 0;

        for (int i = 0; i < mAICars.Length; i++)
        {
            var nextWaypoint = mAICars[i].CurrentWaypoint;
            var distanceCovered = (nextWaypoint.position- mAIBodies[i].position).magnitude;

            if (mDistanceLeft[i] - mRaceConfig.DistanceToCover> distanceCovered ||
                mWaypoints[i] != nextWaypoint)
            {
                mWaypoints[i] = nextWaypoint;
                mRespawnCounters[i] = mRaceConfig.ReSpawnDelay;
                mDistanceLeft[i] = distanceCovered;

            }
            else
            {
                mRespawnCounters[i] -= Time.deltaTime;

                if (mRespawnCounters[i] <= 0)
                {
                    mRespawnCounters[i] = mRaceConfig.ReSpawnDelay;
                    mDistanceLeft[i] = float.MaxValue;
                    mAIBodies[i].velocity = Vector3.zero;
                    var lastWaypoint = mAICars[i].LastWaypoint;
                    mAIBodies[i].position = lastWaypoint.position;
                    mAIBodies[i].rotation = Quaternion.LookRotation(nextWaypoint.position - lastWaypoint.position);
                }

            }

            if (mLaps[i] >= mRaceConfig.RequiredLaps)
            {
                
                carsFinished += 1;

            }
            if (carsFinished == mAICars.Length || mPlayerLaps >= mRaceConfig.RequiredLaps)
            {
                Debug.Log($"Player placed {carsFinished = 1}");
                   SceneManager.LoadScene(0);
            }

        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            mPlayerBody.velocity = Vector3.zero;
            var nextCheckpoint = mCheckPoints[mCurrentCheckpoint].transform.position;
            var lastCheckpoint = mCheckPoints[mCurrentCheckpoint > 0 ? mCurrentCheckpoint - 1:
                mCheckPoints.Length - 1].transform.position;
            mPlayerBody.position = lastCheckpoint;
            mPlayerBody.rotation = Quaternion.LookRotation(nextCheckpoint - lastCheckpoint);
        }

    }

    public void LapFinishByAI(AICar car)
    {
        var i = Array.FindIndex(mAICars, element => element == car);
        if (i != -1)
        {
            mLaps[i] += 1;
        }
    
    }

    public void PlayerCheckPoint(CheckPoint point)
    { 
        if(point == mCheckPoints[mCurrentCheckpoint])
        {
            mCurrentCheckpoint += 1;
            Debug.Log($"PlayerCheckPoint passed checkpoint{mCurrentCheckpoint}");
            if(mCurrentCheckpoint == mCheckPoints.Length)
            { 
                mCurrentCheckpoint = 0;
                mPlayerLaps += 1;
            }
        }
    
    }

    private Texture2D CountdownTimerImage()
    { 
        
        switch (CountDownTimerSecondsRemaining())
        {

            case 3: return mDigitThreeImage;
            case 2: return mDigitTwoImage;
            case 1: return mDigitOneImage;
            case 0: mGameStart = true;  return mStartRaceImage;
                default: return null;
             
        }

    }

    private int CountDownTimerSecondsRemaining()
    {
        var elapsedSeconds = (int)(Time.time - mCountdownTimerStartTime);
        var secondsLeft = (mCountdownTimerDelay - elapsedSeconds);
        return secondsLeft;
    
    }

    private void CountdownTimerReset(int delayInSeconds)
    { 
        mCountdownTimerDelay = delayInSeconds;
        mCountdownTimerStartTime = Time.time;
    
    }

    private void OnGUI()
    {
        GUILayout.BeginArea(new Rect(0,0, Screen.width,Screen.height));
        GUILayout.FlexibleSpace();
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        GUILayout.Label(CountdownTimerImage());
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        GUILayout.FlexibleSpace();
        GUILayout.EndArea();
    
    }
}
