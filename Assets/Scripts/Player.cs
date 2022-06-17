using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public float initialDelay = 2f;
    public float speed = 2f;

    private Material activeShieldMaterial;
    private ShieldButton shieldButton = null; 
    private GameObject destructablePlayer = null;
    public float speedOfDestruction = 4f;



    private MazeGenerator mazeGenerator = null;
    private Material playerMaterial;
    private int trackIndex = 1;



    private void Start() {
        playerMaterial = GetComponent<Renderer>().material;
        mazeGenerator = SpawnManager.instance.mazeGenerator;
        shieldButton = SpawnManager.instance.shieldButton;
        destructablePlayer = SpawnManager.instance.destructablePlayer;
        activeShieldMaterial = SpawnManager.instance.activeShieldMaterial;
    }

    void Update()
    {
        if(Time.time > initialDelay) {
            PlayerRun();
        }
        SwitchPlayerMaterial();
    }

    private void OnTriggerEnter(Collider other) {
        if(other.CompareTag("Danger") && shieldButton.shieldActive == false) {
            Die();
        }
    }

 

    void Die() {
        Destroy(gameObject);
        GameObject newPlayer = Instantiate(destructablePlayer, transform.position, transform.rotation);
        Rigidbody[] rbDestructable = newPlayer.GetComponentsInChildren<Rigidbody>();
        foreach(Rigidbody rb in rbDestructable) {
            rb.AddRelativeForce(Random.onUnitSphere * speedOfDestruction);
        }
    }


    void PlayerRun() {
        Vector2Int newPosV2 = mazeGenerator.track[trackIndex];
        Vector3 newPos = new Vector3(newPosV2.y, 0, newPosV2.x);
        transform.position = Vector3.MoveTowards(transform.position, newPos, Time.deltaTime * speed);
        if(transform.position == newPos && trackIndex < mazeGenerator.track.Count - 1) {
            trackIndex++;
        }
    }

    void SwitchPlayerMaterial() {
        if(shieldButton.shieldActive == true) {
            GetComponent<Renderer>().material = activeShieldMaterial;
        } else {
            GetComponent<Renderer>().material = playerMaterial;
        }
    } 
}
