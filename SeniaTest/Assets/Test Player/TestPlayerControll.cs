using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayerControll : MonoBehaviour {
    public Vector2 offset;
    public int framesPerSprite;
    public Sprite[] walkUp, walkDown, walkRight, walkLeft;
    public GameObject grassEfect;

    private Sprite[][] walkSprites = new Sprite[4][]; // [[up], [right], [down], [left]]
    //private Sprite[][] runSprites = new Sprite[4][];  // [[up], [right], [down], [left]]
    private SpriteRenderer rend;


    private int directionSave = 0; // saves direction for animation sequence
    private Vector2 movementSave = new Vector2(0, 0); // used for movement calculations
    private bool moveing = false;
    private int animCounter = 0;
    private int animDelay = 0;

    private bool gtrig = false;
    private GameObject gef1;
    private GameObject gef2;
    private int grassTrigID;

    // Use this for initialization
    void Start () {
        walkSprites = new Sprite[][] { walkUp, walkRight, walkDown, walkLeft}; // sonvert inputs to a eseyr to use form
        rend = gameObject.GetComponent<SpriteRenderer>();
        rend.sprite = walkSprites[0][2];

        gef1 = GameObject.Instantiate<GameObject>(grassEfect);
        gef1.name = "grass efect 1";
        gef1.SetActive(false);
        gef2 = GameObject.Instantiate<GameObject>(grassEfect);
        gef2.name = "grass efect 2";
        gef2.SetActive(false);

        grassTrigID = LayerMask.GetMask("Battle Triger Grass");
    }
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        if (moveing)
        {
            Animate();
        }
        else
        {
            Debug.Log(keyRetrun());
            if(directionSave != 0)
            {
                animCounter = 0;
                animDelay = 0;
                moveing = true;
                Animate();
            }
        }
	}

    public void Animate()
    {
        animDelay += 1;
        if (animDelay == framesPerSprite)
            animDelay = 0;

        if (animDelay == 0)
        {
            transform.position += (Vector3)(movementSave * (1f / 2f));
            rend.sprite = walkSprites[directionSave - 1][animCounter % 2];

            animCounter += 1;
            // test for end of animation
            if (animCounter == 2)
            {
                // error corection
                transform.position = new Vector3(Mathf.Round(transform.position.x - offset.x) + offset.x, Mathf.Round(transform.position.y - offset.y) + offset.y);
                int rem = directionSave;
                if (keyRetrun() == 0)
                {
                    rend.sprite = rend.sprite = walkSprites[rem - 1][2];
                }
                // reset variables
                moveing = false;
                animCounter = 0;
            }
        }
    }

    public int keyRetrun() // convert key inputs to 4 way direction indication intager (0 = not moving , 1-4 = direction)
    {
        int dx = 0;
        if (Input.GetKey(KeyCode.RightArrow))
            dx = 1;
        if (Input.GetKey(KeyCode.LeftArrow))
            dx += -1;

        int dy = 0;
        if (Input.GetKey(KeyCode.UpArrow))
            dy = 1;
        if (Input.GetKey(KeyCode.DownArrow))
            dy += -1;


        int dir = 0;
        if (dy != 0 && dx != 0)
        {
            if ((directionSave == 0 ) || (directionSave == 1) || (directionSave == 3))
            {
                dir = (-dx + 3);
                movementSave = new Vector2(dx, 0);
            }
            else if ((directionSave == 2) || (directionSave == 4))
            {
                dir = (-dy + 2);
                movementSave = new Vector2(0, dy);
            }
        }
        else if (dy != 0)
        {
            dir = (-dy + 2);
            movementSave = new Vector2(0, dy);
        }
        else if (dx != 0)
        {
            dir = (-dx + 3);
            movementSave = new Vector2(dx, 0);
        }


        

        Collider2D col = Physics2D.OverlapPoint((Vector2)transform.position + movementSave);
        Collider2D col2 = Physics2D.OverlapPoint((Vector2)transform.position + movementSave + Vector2.down, grassTrigID);

        if (dir != 0 && gtrig && !(col || col2))
        {
            gef1.SetActive(false);
            gef2.SetActive(false);
            gtrig = false;
        }

        if (dir != 0 && (col || (col2 && col2.isTrigger)))
        {
            if ((col && col.isTrigger) || (col2 && col2.isTrigger))
            {
                Debug.Log("Thats grass");
                if (gtrig)
                {
                    if (dir != 0)
                    {
                        gef1.transform.position = transform.position + (Vector3)(movementSave - offset + new Vector2(0.5f, 0.5f));
                        gef1.SetActive(true);
                    }
                    if (dir != 3)
                    {
                        gef2.transform.position = transform.position + (Vector3)(-offset + new Vector2(0.5f, 0.5f));
                        gef2.SetActive(true);
                    }
                    else
                    {
                        gef2.SetActive(false);
                    }
                }
                else
                {
                    gef1.transform.position = transform.position + (Vector3)(movementSave - offset + new Vector2(0.5f, 0.5f));
                    gef1.SetActive(true);
                }
                gtrig = true;
            }
            else
            {
                Debug.Log("Thats a wall");
                dir = 0;
            }
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            framesPerSprite = 3;
        }
        else
        {
            framesPerSprite = 5;
        }

        directionSave = dir;
        return dir; // up = 1, right = 2, down = 3, left = 4
    }
}
