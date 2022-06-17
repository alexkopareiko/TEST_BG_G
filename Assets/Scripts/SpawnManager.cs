using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    #region Singleton
    public static SpawnManager instance;
    private void Awake()
    {
        instance = this;
    }
    #endregion

    public GameObject playerPrefab = null;
    public MazeGenerator mazeGenerator = null;
    public float restartDelay = 3f;
    public ShieldButton shieldButton = null; 
    public GameObject destructablePlayer = null;
    public Material activeShieldMaterial;


    private bool restartActivated = false;


    // Update is called once per frame
    void Update()
    {
        if(!GameObject.FindGameObjectWithTag("Player") && !restartActivated) {
            StartCoroutine(Restart(restartDelay));
            restartActivated = true;
        }
    }

    private IEnumerator Restart(float seconds) {
        yield return new WaitForSeconds(seconds);
        Vector3 pos = new Vector3(mazeGenerator.initialX, transform.position.y, mazeGenerator.initialZ);
        Instantiate(playerPrefab, pos, playerPrefab.transform.rotation);
        Destroy(GameObject.FindGameObjectWithTag("DestructMe"));
        restartActivated = false;
    }
}
