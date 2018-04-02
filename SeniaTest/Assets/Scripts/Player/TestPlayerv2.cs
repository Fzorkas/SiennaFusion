using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayerv2 : MonoBehaviour
{
    public TextBoxTyper testTextBox;

    // in editor inputs
    public Vector2 offset;
    public GameObject baseGrassObject;
    public Sprite grassImage1, grassImage2, grassImage3;

    // variables to be set at start
    private Animator animator; // animator for the player
    private float transitionTime; // base time to move from one tile to the next
    private GameObject grass1, grass2, grassTop1, grassTop2; // grass efects for going throgh tall grass

    // internal use variables
        //memoty between frames
        private bool moveing = false;
        private Vector3 remPosition;
        private float totalTime;

	void Start () {
        animator = gameObject.GetComponent<Animator>();

        transitionTime = 0.2f;

        transform.position = new Vector2(Mathf.RoundToInt(transform.position.x - offset.x), Mathf.RoundToInt(transform.position.y-offset.y)) + offset;
    }

    void Update()
    {
        if (moveing)
        {
            PosAnimate();
        }
        else
        {
            moveing = PlayerDetect.InputToDirection();
            //Debug.Log(PlayerDetect.direction);
            animator.SetInteger("Direction", PlayerDetect.direction);
            if(moveing)
            {
                remPosition = transform.position;
                moveing = true;

                moveing = PlayerDetect.Wall(transform.position);
                if (moveing)
                {
                    /*string[] strings = new string[] { "Hello this is a test message to see if the text typer is running corectily",
                        "Testing Testing one two three dose this text wraping work corectly at all",
                        "yep here is another test mesage to test the text wraping system"};
                    testTextBox.SetDispley("test", 0, strings[Random.Range(0,3)]);*/
                    PlayerDetect.Grass(ref baseGrassObject, ref grass2, grassImage1, grassImage2, grassImage3);
                } 
            }
        }
    }

    private void PosAnimate()
    {
        totalTime += Time.deltaTime;
        if (baseGrassObject.activeSelf == true)
        {
            baseGrassObject.GetComponent<Animator>().SetBool("Moveing", false);
        }

        if(totalTime > transitionTime)
        {
            totalTime = 0;
            transform.position = remPosition + (Vector3)PlayerDetect.dirVec;
            moveing = false;
        }
        else
        {
            transform.position += (Vector3)(PlayerDetect.dirVec * (Time.deltaTime/transitionTime));
            moveing = true;
        }
    }
}

public static class PlayerDetect
{
    // key inputs to a Direction value (0 = no mvoement, 1 = up(y), 2 = right(x), 3 = down(-y), 4 = left(-x))
    public static int direction = 0;
    public static Vector2 dirVec = new Vector2(0, 0);

    public static bool InputToDirection()
    {
        // get unit value for x
        int dx = 0;
        if (Input.GetKey(KeyCode.RightArrow))
            dx = 1;
        if (Input.GetKey(KeyCode.LeftArrow))
            dx += -1;

        // get unit value for y
        int dy = 0;
        if (Input.GetKey(KeyCode.UpArrow))
            dy = 1;
        if (Input.GetKey(KeyCode.DownArrow))
            dy += -1;


        // Traslate unit values and privius movement direction into new direction value and vector
        if (dy != 0 && dx != 0)
        {
            // figures out wich direction to move on diagnals
            if ((direction == 0) || (direction == 1) || (direction == 3))
            {
                // last movement in y --> now move in x
                direction = (-dx + 3);
                dirVec = new Vector2(dx, 0);
            }
            else if ((direction == 2) || (direction == 4))
            {
                // last movement in x --> now move in y
                direction = (-dy + 2);
                dirVec = new Vector2(0, dy);
            }
            return true;
        }
        else if (dy != 0)
        {
            // up/down movement calculations
            direction = (-dy + 2);
            dirVec = new Vector2(0, dy);
            return true;
        }
        else if (dx != 0)
        {
            // right/left movement calculations
            direction = (-dx + 3);
            dirVec = new Vector2(dx, 0);
            return true;
        }
        else if (direction != 0)
        {
            direction = 0;
            dirVec = new Vector2(0, 0);
        }

        return false;
    }

    // detection of suronding trigers and walls
    public static GameObject par;
    public static GameObject grass;
    public static Vector2 positon;
    public static Vector2 front;
    public static bool grassOn;
    public static bool wall;

    public static bool Wall(Vector2 Position)
    {
        positon = Position;
        front = positon + dirVec;

        Collider2D t1 = Physics2D.OverlapPoint(front);
        if (t1)
        {
            wall = t1.isTrigger;
            return wall;
        }

        wall = true;
        return true;
    }

    public static bool Grass(ref GameObject g1, ref GameObject g2, Sprite im1, Sprite im2, Sprite im3)
    {
        Vector2 frontDown = front + Vector2.down;

        bool t0 = Physics2D.OverlapPoint(positon, 256);
        Collider2D t1 = Physics2D.OverlapPoint(front, 256);
        Collider2D t2 = Physics2D.OverlapPoint(frontDown, 256);

        if (t1 || t0)
        {
            if (t1)
            {
                g1.SetActive(true);
                g1.GetComponent<Animator>().SetBool("Moveing", true);
            }
            else if (t0 && wall)
            {
                g1.SetActive(false);
            }
        }
        else
        {
            g1.SetActive(false);
        }
        return t1;
    }
}
