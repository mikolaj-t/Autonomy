using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameOverInfo : MonoBehaviour
{
    // Start is called before the first frame update
    public TextMeshProUGUI winText;
    public TextMeshProUGUI loseText;
    public GameObject winButton;
    public GameObject loseButton;
    public TextMeshProUGUI bottomText;
    void Start()
    {
        
    }

    public void Refresh(bool success, string text){
        if(success){
            winText.enabled = true;
            loseText.enabled = false;
            winButton.SetActive(true);
            loseButton.SetActive(false);
        }else{
            winText.enabled = false;
            loseText.enabled = true;
            winButton.SetActive(false);
            loseButton.SetActive(true);
        }
        bottomText.text = text;
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
}
