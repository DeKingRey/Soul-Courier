using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; 

public class FpsCounter : MonoBehaviour
{
    public TextMeshProUGUI FpsText;

    private float pollingTime = 3f;
    private float time;
    private float frameCount;


    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;

        frameCount++;

        if(time >= pollingTime) {
            int frameRate = Mathf.RoundToInt(frameCount / time);
            FpsText.text = frameRate.ToString() + " FPS";   

            time -= pollingTime;
            frameCount = 0;
        }
    }
}
