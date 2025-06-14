using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class gameEngine : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI txScore;
    [SerializeField] public Animator canva;
    [SerializeField] private GameObject cam;
    [SerializeField] private spawnerBehavior bubLane;
    [SerializeField] private int futureLane;
    [SerializeField] private AudioClip duar;
    [SerializeField] private AudioSource aud;
    public bool lose = false;
    private int falseLane = 0;

    public int laneRn = 0;
    public bool openAllBomb = false;

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1.0f;
        for (int i = 0; i < 30; i++) //spawn bubbles' lane
        {
            spawnerBehavior ln = Instantiate(bubLane, new Vector3( i%2 == 0 ? -7.5f : -8.25f,i* 1.4f, 0), Quaternion.identity);
            ln.laneNum = i;
            futureLane = i+1;
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(0, cam.transform.position.y, 0);

        if (lose) cam.transform.position = new Vector3(0, Mathf.Lerp(cam.transform.position.y, falseLane * 1.4f, 0.05f), 0); 
        else
        {
            txScore.text = "length " + laneRn.ToString() + " cm";
            cam.transform.position = new Vector3(0, Mathf.Lerp(cam.transform.position.y, laneRn * 1.4f, 0.05f), 0);
        }
    }

    public void LaneDone(int lane)
    {
        if(lane > laneRn) // if lane finished isn't the current one / newer one
        {
            return;
        }
        else if (lane == laneRn)
        {
            spawnerBehavior ln = Instantiate(bubLane, new Vector3(futureLane % 2 == 0 ? -7.5f : -8.25f, futureLane * 1.4f, 0), Quaternion.identity);
            ln.laneNum = futureLane;
            futureLane++;
            laneRn++;
        }
    }

    public void LoseByBomb()
    {
        aud.PlayOneShot(duar);
        canva.SetInteger("gameState", 2);
        openAllBomb = true;
        //add some UI shenanigans, show score or something
    }

    public void LoseByFalseFlagged(int lane)
    {
        canva.SetInteger("gameState", 2);
        lose = true;
        falseLane = lane;
    }

    public void StartGame()
    {
        canva.SetInteger("gameState", 1);
    }
}
