using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public MazeGenerator mazeGenerator = null;
    public float initialDelay = 2f;
    public float speed = 2f;
    public bool underShield = false;
    public Material activeShield;
    public Button shieldButton = null;
    public GameObject destructablePlayer = null;

    public float speedOfDestruction = 4f;

    private Material playerMaterial;
    private bool isPlayerRunning = false;
    private int trackIndex = 1;

    private float timeToWaitForShield = 2f;
    private bool buttonShieldDisabled = false;

    private void Start() {
        playerMaterial = GetComponent<Renderer>().material;
    }

    void Update()
    {
        if(Time.time > initialDelay) {
            PlayerRun();
        }
        
    }

    private void OnTriggerEnter(Collider other) {
        if(other.CompareTag("Danger")) {
            Destroy(gameObject);
            GameObject newPlayer = Instantiate(destructablePlayer, transform.position, transform.rotation);
            Rigidbody[] rbDestructable = newPlayer.GetComponentsInChildren<Rigidbody>();
            foreach(Rigidbody rb in rbDestructable) {
                rb.AddRelativeForce(Random.onUnitSphere * speedOfDestruction);
            }
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

    public void MakeUnderShield(RectTransform shield) {
        if(buttonShieldDisabled == true)
            return;

        Invoke("DisableShieldButton", timeToWaitForShield);
        underShield = true;
        SwitchPlayerMaterial();
    }

    public void ReleaseShield(RectTransform shield) {
        underShield = false;
        SwitchPlayerMaterial();
        buttonShieldDisabled = false;
        shieldButton.interactable = true;
    }

    void SwitchPlayerMaterial() {
        if(underShield == true) {
            GetComponent<Renderer>().material = activeShield;
        } else {
            GetComponent<Renderer>().material = playerMaterial;
        }
    }

    private void DisableShieldButton()
    {
        shieldButton.interactable = false;
        buttonShieldDisabled = true;
        underShield = false;
        SwitchPlayerMaterial();
    }
  
}
