using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public MazeGenerator mazeGenerator = null;
    public float initialDelay = 2f;
    public float speed = 2f;



    private bool isPlayerRunning = false;
    private int trackIndex = 1;

    // Update is called once per frame
    void Update()
    {
        if(Time.time > initialDelay) {
            PlayerRun();
        }
        
    }

    void PlayerRun() {
        Vector2Int newPosV2 = mazeGenerator.track[trackIndex];
        Vector3 newPos = new Vector3(newPosV2.y, 0, newPosV2.x);
        if(Input.GetMouseButton(0)) {
            transform.position = Vector3.MoveTowards(transform.position, newPos, Time.deltaTime * speed);
            if(transform.position == newPos && trackIndex < mazeGenerator.track.Count - 1) {
                trackIndex++;
            }
        }
    }
  
}
