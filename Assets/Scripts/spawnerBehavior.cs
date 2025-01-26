using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnerBehavior : MonoBehaviour
{
    [SerializeField] private gameEngine ge;
    [SerializeField] private GameObject bubble;
    [SerializeField] private GameObject[] blank;
    [SerializeField] private List<GameObject> bubbleHolder = new List<GameObject>();

    public int laneNum;
    private int manyBubble;

    // Start is called before the first frame update
    void Start()
    {
        ge = GameObject.Find("LevelConfig").GetComponent<gameEngine>();
        if (laneNum % 2 == 0) manyBubble = 11;
        else manyBubble = 12;

        for (int i = 0; i < manyBubble; i++)
        {
            bubbleHolder.Add(Instantiate(bubble, this.transform) as GameObject);
            bubbleHolder[i].transform.position = new Vector3(1.5f * i + this.transform.position.x, this.transform.position.y, this.transform.position.z);
        }
        Instantiate(blank[0], new Vector3(1.5f * -1 + this.transform.position.x, this.transform.position.y, this.transform.position.z), Quaternion.identity, this.transform);
        Instantiate(blank[1], new Vector3(1.5f * manyBubble + this.transform.position.x, this.transform.position.y, this.transform.position.z), Quaternion.identity, this.transform);
    }

    // Update is called once per frame
    void Update()
    {
        if (CheckPoppedFlagged())
        {
            //camera go up
            //Debug.Log("ye");
            ge.LaneDone(laneNum);
        }
    }

    private bool CheckPoppedFlagged()
    {
        int many = 0;
        for (int i = 0;i < bubbleHolder.Count; i++)
        {
            bubbleBehavior babel = bubbleHolder[i].GetComponent<bubbleBehavior>();
            if(babel.flagged || !babel.unpop) many++;
        }

        if(many >= manyBubble) return true;
        else return false;
    }
}
