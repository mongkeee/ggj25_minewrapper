using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class bubbleBehavior : MonoBehaviour
{
    [SerializeField] private gameEngine ge;
    [SerializeField] private SpriteRenderer objSpr;
    [SerializeField] private Sprite[] bomb;
    [SerializeField] private Sprite[] flagBubble;
    [SerializeField] private Sprite[] blankNum;
    [SerializeField] private GameObject[] bubbles = new GameObject[6];
    [SerializeField] private AudioClip[] pop;
    [SerializeField] private AudioClip duar;
    [SerializeField] private AudioSource aud;
    public int lane;
    public bool isBomb = false;
    public bool unpop = true;
    public bool flagged = false;

    [SerializeField]private int bombNear = 0;

    private float[] radOth = new float[6];

    // Start is called before the first frame update
    void Start()
    {
        aud = this.GetComponent<AudioSource>();
        ge = GameObject.Find("LevelConfig").GetComponent<gameEngine>();
        objSpr = this.GetComponent<SpriteRenderer>();
        lane = this.transform.parent.gameObject.GetComponent<spawnerBehavior>().laneNum;

        int rng = Random.Range(0, 10);
        if(lane <= 1) isBomb = false; //for the first 2 lane
        else if(rng >= 8) isBomb = true; //rng bomb

        for (int i = 0; i < 6; i++)
        {
            radOth[i] = i * 60 * Mathf.Deg2Rad;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (ge.openAllBomb && isBomb) //if other bomb revealed by player
        {
            objSpr.sprite = bomb[0];
            if (unpop) playBomb();
            unpop = false;
        }
        else if (!unpop) //if bubble popped by player
        {
            if (isBomb) //player lose func
            {
                objSpr.sprite = bomb[1];
                ge.LoseByBomb();
            }
            else objSpr.sprite = blankNum[bombNear];

            if (bombNear == 0) StartCoroutine(PoppedOther());
            if (lane == 0 && !ge.lose) ge.StartGame();
        }
        else
        {
            if (flagged) objSpr.sprite = flagBubble[1];
            else objSpr.sprite = flagBubble[0];
            int jum = 0;
            RaycastHit2D[] hitOth = new RaycastHit2D[6];
            for (int i = 0; i < 6; i++)
            {
                hitOth[i] = Physics2D.Raycast(this.transform.position + RadToCoord(radOth[i]), RadToCoord(radOth[i]), 0.5f);
                Debug.DrawRay(this.transform.position + RadToCoord(radOth[i]), RadToCoord(radOth[i]) * 0.5f, Color.white, 0); //only show up with gizmos on
                if (hitOth[i].collider != null)
                {
                    if (hitOth[i].collider.gameObject.tag == "pop")
                    {
                        bubbleBehavior othBubb = hitOth[i].collider.gameObject.GetComponent<bubbleBehavior>();
                        bubbles[i] = hitOth[i].collider.gameObject;
                        if (othBubb.isBomb) jum++;
                    }
                }
            }
            bombNear = jum;
        }
    }

    public void OnMouseOver()//mouse input
    {
        //beranak anak
        if (!IsMouseOverUI())
        {
            if (Input.GetMouseButtonDown(0)) //klik kiri
            {
                if (!flagged)
                {
                    if (countFlagged() == bombNear && !unpop)
                    {
                        for (int i = 0; i < bubbles.Length; i++)
                        {
                            if (bubbles[i] != null && !bubbles[i].GetComponent<bubbleBehavior>().flagged)
                            {
                                if (bubbles[i].GetComponent<bubbleBehavior>().unpop) playPop();
                                bubbles[i].GetComponent<bubbleBehavior>().unpop = false;
                            }
                        }
                    }
                    else 
                    {
                        if(unpop) playPop();
                        unpop = false;                        
                    }
                    
                }
            }
            else if (Input.GetMouseButtonDown(1)) //klik kanan
            {
                if (!flagged) flagged = true;
                else flagged = false;
            }
        }

        
    }

    private Vector3 RadToCoord(float rad) //radians ke koordinat di lingkaran
    {
        Vector3 coord = new Vector3(Mathf.Cos(rad), Mathf.Sin(rad), 0) * 0.7f; //radius lingkaran 0.7

        return coord;
    }

    private IEnumerator PoppedOther()
    {
        yield return new WaitForSeconds(0.3f); //delay between automatic bubble pop
        //capek
        for (int i = 0;i < bubbles.Length; i++)
        {
            if (bubbles[i] != null && bubbles[i].GetComponent<bubbleBehavior>().unpop)
            {
                bubbles[i].GetComponent<bubbleBehavior>().unpop = false;
                playPop();
            }
        }
    }

    private int countFlagged()
    {
        int flagged = 0;
        for (int i = 0; i < bubbles.Length; i++)
        {
            if (bubbles[i] != null)
            {
                if (bubbles[i].GetComponent<bubbleBehavior>().flagged) flagged++;
            }
        }
        return flagged;
    }

    private bool IsMouseOverUI()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }

    private void playPop()
    {
        int rng = Random.Range(0, pop.Length);
        aud.PlayOneShot(pop[rng]);
    }

    private void playBomb()
    {
        aud.PlayOneShot(duar);
    }

    public void falseFlagged()
    {
        flagBubble[1] = flagBubble[2];
    }
}
