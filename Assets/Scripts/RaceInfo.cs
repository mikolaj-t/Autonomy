using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RaceInfo : MonoBehaviour
{
    private float endTime;
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI lapsText;
    public TextMeshProUGUI objectiveText;
    public TextMeshProUGUI destination;

    private GameManager gm {
        get {
            return GameManager.instance;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        GameManager.instance.loadTrackEvents.Add(TrackLoaded);
        GameManager.instance.collectedColEvents.Add(CollectibleCollected);
        GameManager.instance.lapsComplEvents.Add(UpdateLaps);
    }

    public void TrackLoaded(){
        endTime = gm.gameInfo.startTime + gm.gameInfo.track.timeLimit;
        var t = (CardioTrack) (gm.gameInfo.track);
        destination.text = t.startingOrgan + " -> " + t.endingOrgan;
        UpdateLaps();
        UpdateObjective();
    }

    public void CollectibleCollected(){
        UpdateObjective();
    }

    private void UpdateObjective(){
        if(gm.gameInfo.track is CardioTrack){
            var cardioTrack = ((CardioTrack)gm.gameInfo.track);
            objectiveText.text = "<font-weight=400>" + gm.gameInfo.score + "/" + cardioTrack.needToCollect + " <font-weight=500><color=#"
            + ColorUtility.ToHtmlStringRGB(cardioTrack.collectibleType.collectibleColor) + "> " + cardioTrack.collectibleType.collectibleName;
        }
    }

    private void UpdateLaps(){
        lapsText.text = (gm.gameInfo.laps+1) + "/" + gm.gameInfo.track.lapsAmount;
    }
    // Update is called once per frame
    void Update()
    {
        var timeRemain = endTime - Time.time;
        timeText.text = Mathf.Max(0,(int)((timeRemain) / 60)) + ":" + Mathf.Max(0, ((int)(timeRemain) % 60)).ToString("D2");  
    }
}
