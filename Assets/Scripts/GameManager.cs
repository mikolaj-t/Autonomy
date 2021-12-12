using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public SCanvas gameCanvas;
    public SCanvas gameOverCanvas;
    public GameOverInfo gameOverInfo;
    
    public int level = 0;
    public Gamemode cardio;
    public GameInfo gameInfo;
    private Color artertyFog = new Color(1f, 0f, 0f, 1f);
    private Color veinFog = new Color(0f, 0f, 1f, 1f);

    public List<Action> loadTrackEvents = new List<Action>();
    public List<Action> collectedColEvents = new List<Action>();
    public List<Action> lapsComplEvents = new List<Action>();

    private bool hasAfterStarted = false;
    
    void Awake(){

        if(instance == null){
            instance = this;
        }
    
    }
    // Start is called before the first frame update
    void Start()
    {
        LoadTrack(cardio, cardio.tracks[level]);
    }

    // Update is called once per frame
    void Update()
    {
        if(!hasAfterStarted){
            AfterStart();
            hasAfterStarted = true;
        }
    }

    void AfterStart(){
        foreach(var action in loadTrackEvents){
            action.Invoke();
        }
    }

    void LoadTrack(Gamemode gamemode, Track track){
        gameCanvas.Show();

        if(gameInfo != null){
            Destroy(gameInfo.spawnedTrackPrefab);
        }

        if(Car.instance != null) { 
            Car.instance.rb.velocity = Vector3.zero;
        }

        gameInfo = new GameInfo();
        gameInfo.spawnedTrackPrefab = Instantiate(track.prefab, track.prefab.transform.position, track.prefab.transform.rotation);

        gameInfo.track = track;
        gameInfo.startTime = Time.time;
        gameInfo.gamemode = gamemode;
        gameInfo.gameState = GameState.PLAYING;

        Car.instance.transform.position = track.startingPoint + new Vector3(0f, 0.01f, 0f);
        Car.instance.transform.rotation = Quaternion.Euler(track.startingRotation);

        if(track is CardioTrack){
            var cardioTrack = (CardioTrack) track;
            if(cardioTrack.type == CardioTrack.Type.ARTERY){
                RenderSettings.fogColor = artertyFog;
            }else{
                RenderSettings.fogColor = veinFog;
            }
        }

        foreach(var action in loadTrackEvents){
            action.Invoke();
        }

        StartCoroutine(FailAfterTime(track.timeLimit));
    }

    private IEnumerator FailAfterTime(float time){
        yield return new WaitForSeconds(time);
        Finished(false, "You've ran out of time!");
    }
    public void CollectedCollectible(){
        gameInfo.score++;
        foreach(var action in collectedColEvents){
            action.Invoke();
        }
    }

    public void CompletedALap(){
        gameInfo.laps++;
        foreach(var action in lapsComplEvents){
            action.Invoke();
        }
        if(gameInfo.laps == gameInfo.track.lapsAmount){
            if(gameInfo.track is CardioTrack){
                var ct = (CardioTrack)gameInfo.track;
                if(gameInfo.score < ct.needToCollect){
                    Finished(false, "Didn't bring enough products!");
                }else{
                    Finished(true, "Level completed!");
                }
            }
        }
    }

    public void Finished(bool success, string bottomtext){
        gameInfo.gameState = GameState.OTHER;
        gameOverCanvas.Show();
        gameOverInfo.Refresh(success, bottomtext);
    }

    public void NextLevel(){
        if(level + 1 != cardio.tracks.Length){
            level++;
        }
        LoadTrack(cardio, cardio.tracks[level]);
    }

    public void Retry(){
        LoadTrack(cardio, cardio.tracks[level]);
    }
}
