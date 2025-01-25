using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameEngine : MonoBehaviour
{
    [SerializeField] public Animator canva;
    [SerializeField] private GameObject cam;
    [SerializeField] private spawnerBehavior bubLane;
    [SerializeField] private int laneRn = 0;
    [SerializeField] private int futureLane;
    [SerializeField] private List<int> laneFinished;

    public bool openAllBomb = false;

    // Start is called before the first frame update
    void Start()
    {
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
        cam.transform.position = new Vector3( 0, Mathf.Lerp(cam.transform.position.y, laneRn * 1.4f, 0.05f), 0);
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
        canva.SetInteger("gameState", 2);
        openAllBomb = true;
        //add some UI shenanigans, show score or something
    }

    public void StartGame()
    {
        canva.SetInteger("gameState", 1);
    }
}
