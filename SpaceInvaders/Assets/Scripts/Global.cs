using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class Global : MonoBehaviour
{
    /*
     * Static variables
     */
    public static int numAliens = 55; // total number of aliens in the beginning
    public static int numShields = 4; // total number of
    public static int closestRow = 0; // current closest alien row
    public static int rowNum = 5; // number of rows for alien group
    public static float alienTimer = 1f; // timer for alien attack
    public static float UFOTimer = 20; // timer for UFO spawn
    public static int playerScore = 0; // Keep track of player score
    public static bool isGameOver = false; // has the player lost all their lives?
    public static bool isPause = false; // is the timer paused?
    public static int shipStreak; // keep track of consecutive target hits
    public static bool isRewardActive = false; // is any reward active?
    public static float rewardDuration = 5; // how long the reward lasts
    public static int activeReward; // index of active reward
    public static List<GameObject> aliens = new List<GameObject>();
    public static List<GameObject> shields = new List<GameObject>();

    /*
     * Instance variables
     */
    public Vector2 alienSpeed; // initial speed
    public GameObject jelly; // alien to spawn
    public GameObject skull; // alien to spawn
    public GameObject crab; // alien to spawn
    public GameObject bullet; // alien bullet to spawn
    public GameObject shield; // shield to spawn
    public GameObject UFO; // UFO to spawn
    public GameObject ship; // ship game object
    public TMP_Text fasterBullet;
    public TMP_Text fasterBuffer;
    // Some resources
    public GameObject extraLife;
    public GameObject bulletSpeed;
    public GameObject fasterReload;

    public Vector2 maxPos; // maximum position an alien or the ship can take
    public bool maxZReached; // determine if the aliens need to go lower
    public GameObject currentUFO; // store current spawned UFO

    static void ResetGameStats()
    {
        // Clean up remaining aliens and shields
        for (int i = 0; i < numAliens; ++i)
        {
            GameObject toRemove = aliens[0];
            aliens.RemoveAt(0);
            Destroy(toRemove);
        }
        for (int i = 0; i < numShields; ++i)
        {
            GameObject toRemove = shields[0];
            shields.RemoveAt(0);
            Destroy(toRemove);
        }
        Global.numAliens = 55;
        Alien.idCount = 0;
        Alien.timer = 1;

        Ship.remainingLives = 3;

        Global.numShields = 4;
        Shield.idCount = 0;

        Global.alienTimer = 1;
        Global.UFOTimer = 20;
        Global.closestRow = 0;
        Global.playerScore = 0; // Reset score
        Global.shipStreak = 0;
    }

    // Generate a reward
    public void generateReward(Vector3 spawnPos)
    {
        // Pick a random reward to generate
        int rewardIdx = Random.Range(0, 9);
        GameObject reward = null;
        if (rewardIdx == 0) {
            // Generate extra life
            reward = Instantiate(extraLife, spawnPos, Quaternion.identity) as GameObject;
        } else if (rewardIdx < 5) {
            // Generate bullet speed
            reward = Instantiate(bulletSpeed, spawnPos, Quaternion.identity) as GameObject;
            rewardIdx = 1;
        } else {
            // Generate faster bullet reload
            reward = Instantiate(fasterReload, spawnPos, Quaternion.identity) as GameObject;
            rewardIdx = 2;
        }
        if (reward != null) {
            reward.GetComponent<RewardScript>().rewardIdx = rewardIdx;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if (currentUFO != null)
        {
            Destroy(currentUFO);
        }
        alienSpeed = new Vector2(0.01f, 0.1f); // initial speed
        // Reset ship state
        ship.GetComponent<Ship>().ResetState();
        // Create the aliens
        float z_coor = 1.2f;
        float x_coor = -7f;
        Vector3 spawnPos = new Vector3(x_coor, 0, z_coor);
        maxPos = new Vector2 (Mathf.Abs(spawnPos.x) + 7f, -3f);
        maxZReached = false;
        int idx = 0;
        for (int row = 0; row < rowNum; ++row)
        {
            for (int col = 0; col < numAliens / rowNum; ++col)
            {
                Quaternion rot = Quaternion.identity;
                rot *= Quaternion.Euler(Vector3.up * -90);
                if (row <= 1)
                {
                    Global.aliens.Add(Instantiate(skull, spawnPos, rot));
                } else if (row <= 3)
                {
                    Global.aliens.Add(Instantiate(crab, spawnPos, rot));
                } else if (row == 4)
                {
                    Global.aliens.Add(Instantiate(jelly, spawnPos, rot));
                }
                spawnPos.x += 1.55f;
                idx++;
            }
            spawnPos.z += 1.25f;
            spawnPos.x = -7f;
        }
        // Make aliens ignore collision between each other
        for (int i = 0; i < numAliens; ++i)
        {
            for (int j = 0; j < numAliens; ++j)
            {
                if (i == j) continue;
                Physics.IgnoreCollision(Global.aliens[i].GetComponent<Collider>(), Global.aliens[j].GetComponent<Collider>());
            }
        }
        // Create the shields
        Global.shields.Add(Instantiate(shield, new Vector3(-6, 0, -4f), Quaternion.identity));
        Global.shields.Add(Instantiate(shield, new Vector3(-2, 0, -4f), Quaternion.identity));
        Global.shields.Add(Instantiate(shield, new Vector3(2, 0, -4f), Quaternion.identity));
        Global.shields.Add(Instantiate(shield, new Vector3(6, 0, -4f), Quaternion.identity));
        Global.isGameOver = false;
    }

    public void RemoveShield(int id)
    {
        for (int i = 0; i < numShields; ++i)
        {
            if (Global.shields[i].GetComponent<Shield>().id == id)
            {
                // Remove the shield
                Global.shields.RemoveAt(i);
                break;
            }
        }
        Global.numShields--;
    }

    public void RemoveAlien(int id)
    {
        int toRemoveIdx = -1;
        // Check if aliens should get closer
        bool getCloser = true;
        for (int i = 0; i < numAliens; ++i)
        {
            int alienId = Global.aliens[i].GetComponent<Alien>().id;
            if (alienId == id)
            {
                toRemoveIdx = i;
            }
            if (alienId / 11 == Global.closestRow && alienId != id)
            {
                getCloser = false;
            }
        }
        if (getCloser && maxZReached)
        {
            maxZReached = false;
            Global.closestRow++;
        }
        if (toRemoveIdx != -1)
        {
            // Add this alien's score value to player score
            playerScore += Global.aliens[toRemoveIdx].GetComponent<Alien>().score;
            Global.aliens.RemoveAt(toRemoveIdx);
            // Update remaining number of aliens
            Global.numAliens--;
        }
        if (numAliens == 0)
        {
            // Reset the shields
            for (int i = 0; i < numShields; ++i)
            {
                GameObject toRemove = shields[0];
                shields.RemoveAt(0);
                Destroy(toRemove);
            }
            // Player killed all the aliens - restore everything!
            Global.numAliens = 55;
            Global.numShields = 4;
            Alien.timer = 1;
            Start();
            return;
        }
        // Update alien speed if needed
        if (Global.numAliens % 8 == 0)
        {
            alienSpeed.x += 0.01f;
            alienSpeed.y += 0.05f;
            Alien.timer /= 1.5f;
            // Speed the ship as well
            ship.GetComponent<Ship>().moveAcceleration += 0.05f;
            //ship.GetComponent<Ship>().bulletSpeed += 50f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire2"))
        {
            Application.Quit();
        }
        if (ship.GetComponent<Ship>().isResurrecting)
        {
            Global.isRewardActive = false;
            Global.rewardDuration = 5;
            fasterBullet.gameObject.SetActive(false);
            fasterBullet.enabled = false;
            fasterBuffer.gameObject.SetActive(false);
            fasterBuffer.enabled = false;
        }
        float deltaT = Time.deltaTime;
        // Make the necessary updates if game is ongoing
        if (!isGameOver && !isPause)
        {
            alienTimer -= deltaT;
            // Make the closest alien shoot every 1 second
            if (alienTimer <= 0 && Global.numAliens > 0)
            {
                float minDist = 9999f; // set to a high enough value
                int alienIdx = 0;
                for (int i = 0; i < numAliens; ++i)
                {
                    // Compute the Euclidian distance
                    float dist = Mathf.Sqrt((Global.aliens[i].transform.position.x - ship.transform.position.x) * (Global.aliens[i].transform.position.x - ship.transform.position.x) +
                                 (Global.aliens[i].transform.position.z - ship.transform.position.z) * (Global.aliens[i].transform.position.z - ship.transform.position.z));
                    if (dist < minDist)
                    {
                        minDist = dist;
                        alienIdx = i;
                    }
                }
                GameObject toShoot = Global.aliens[alienIdx];
                // remove the selected id from the list
                GameObject obj = toShoot.GetComponent<Alien>().Shoot();
                for (int i = 0; i < numAliens; ++i)
                {
                    Physics.IgnoreCollision(Global.aliens[i].GetComponent<Collider>(), obj.GetComponent<Collider>());
                }
                alienTimer = 1f;
            }
            // Check if any alien has reached the max distance
            bool zReached = false;
            for (int i = 0; i < numAliens; ++i)
            {
                if (Global.aliens[i].transform.position.x >= maxPos.x || Global.aliens[i].transform.position.x <= -maxPos.x)
                {
                    for (int j = 0; j < numAliens; ++j)
                    {
                        // Update movement direction
                        Global.aliens[j].GetComponent<Alien>().moveLeft = !Global.aliens[j].GetComponent<Alien>().moveLeft;
                        // Bring aliens closer (unless they're at maximum closeness)
                        Vector3 updatedPosition = Global.aliens[j].GetComponent<Alien>().transform.position;
                        if (!maxZReached)
                        {
                            updatedPosition.z -= alienSpeed.y;
                            if (updatedPosition.z <= maxPos.y)
                            {
                                zReached = true;
                            }
                            Global.aliens[j].GetComponent<Alien>().transform.position = updatedPosition;
                        }
                    }
                    break;
                }
            }
            if (zReached)
            {
                maxZReached = true;
            }
            // Decide if an UFO should be spawned
            UFOTimer -= deltaT;
            if (UFOTimer <= 0)
            {
                // Spawn an UFO
                Vector3 spawnPos = new Vector3(maxPos.x, 0, 7);
                Quaternion rot = Quaternion.identity;
                rot *= Quaternion.Euler(Vector3.up * -90);
                currentUFO = Instantiate(UFO, spawnPos, rot) as GameObject;
                // Set the next UFO spawn to happen at a random time
                UFOTimer = Random.Range(10, 20);
            }
            if (Global.isRewardActive) 
            {
                switch(Global.activeReward)
                {
                    case 1:
                        // Generate bullet speed
                        fasterBullet.text = "Speedy bullet  ending in " + (int) Global.rewardDuration;
                        break;
                    case 2:
                        // Generate faster bullet reload
                        fasterBuffer.text = "Faster reload  ending in " + (int) Global.rewardDuration;
                        break;
                    default:
                        break;
                }
                Global.rewardDuration -= deltaT;
            }
            if (Global.rewardDuration <= 0)
            {
                // Inactivate the reward
                Global.isRewardActive = false;
                switch(Global.activeReward)
                {
                    case 1:
                        // Generate bullet speed
                        ship.GetComponent<Ship>().bulletSpeed /= 2f;
                        fasterBullet.gameObject.SetActive(false);
                        fasterBullet.enabled = false;
                        break;
                    case 2:
                        // Generate faster bullet reload
                        ship.GetComponent<Ship>().bulletBuffer /= 0.35f;
                        fasterBuffer.gameObject.SetActive(false);
                        fasterBuffer.enabled = false;
                        break;
                    default:
                        break;
                }
            Global.rewardDuration = 5;
            }
        } else
        {
            // Game is over - display Game Over title and see if player wants to continue playing
            if (isGameOver) {
                // Reset the game state
                Global.ResetGameStats();
                Global.isPause = false;
                SceneManager.LoadScene("GameOverScene");
            }
        }
    }
}
