using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class PlayerController : MonoBehaviour
{
    public enum CrateAction { StoreItem, Close}

    // Control Variables
    public GameObject inHandItem;
    public GameObject actionItem;
    public Item inHandItemScript;
    public Item actionItemScript;
    public Craftable actionCraftable;
    public Crate actionCrate;
    public Crate inHandCrateScript;
    public CrateAction crateAction;
    public GameObject throwIsland;
    public GameObject cameraRig;
    public Animator animator;
    public Vector3 throwTarget;
    public bool isSelected = false;
    public int craftingLoop = 0;
    public float throwForce = 10f;

    public string debug;
    public float remaining = 0;
    public float stopping = 0;
    public bool startedWalking = false;

    // Ambient Variables
    NavMeshAgent agent;
    bool selectable = false;

    public Transform hand;
    
    // Public Class functions

    // Allows the character to be selected
    public void wakeUp() {
        selectable = true;
    }


    float GetHorizontalDistance(Vector3 a, Vector3 b)
    {
        return Mathf.Abs((a - b).magnitude);
    }

    void moveToPoint(Vector3 point) {
        animator.SetBool("isWalking", true);
        agent.SetDestination(point);
    }

    void GetItem()
    {
        actionItemScript.PickupItem(hand);
        inHandItem = actionItem;
        inHandItemScript = actionItemScript;
        actionItem = null;
        actionItemScript = null;
        animator.SetBool("pickupItem", false);
        
    }


    void DropItem()
    {
        agent.ResetPath();
        if (inHandCrateScript)
        {
            inHandCrateScript.Drop();
        }
        else
        {
            inHandItemScript.DropItem(hand);
        }
        inHandItem = null;
        inHandItemScript = null;
        inHandCrateScript = null;
        animator.SetBool("dropItem", false);
    }
    void CraftItem()
    {
        actionCraftable.Craft(inHandItemScript.itemType);
        actionCraftable = null;
        animator.SetBool("craftRope", false);

    }

    void BoxAction()
    {
        if ( crateAction == CrateAction.StoreItem && actionCrate.isOpen)
        {
            actionCrate.StoreItem(inHandItemScript.itemType);
            Destroy(inHandItem);
            inHandItem = null;
            inHandItemScript = null;
        }
        else if (crateAction == CrateAction.Close)
        {
            actionCrate.OpenClose();
            actionCrate = null;
        }
        animator.SetBool("boxAction", false);
    }
    void pickupBox()
    {
        actionCrate.PickUp(hand);
        inHandItem = actionCrate.gameObject;
        inHandCrateScript = actionCrate;
        actionCrate = null;

        animator.SetBool("pickupBox", false);
        animator.SetBool("carryingBox", true);
    }

    void throwingBox()
    {
        inHandItem = null;
        inHandCrateScript.Drop();
        inHandCrateScript.Throw(throwTarget);
        inHandCrateScript = null;


        animator.SetBool("throwBox", false);

    }


    // Start is called before the first frame update
    void Start() {
        agent = GetComponent<NavMeshAgent>();
        animator = this.gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update() {
        // Left Button clicks
        // Character selection
        if (Input.GetMouseButtonDown(0))
        {
            if (selectable)
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit))
                {
                    // Character selection
                    if (hit.transform.gameObject == this.gameObject)
                    {
                        if (!isSelected)
                        {
                            isSelected = true;
                            animator.SetBool("isSelected", isSelected);
                            CameraController.instance.followTransform = transform;
                            return;
                        }
                    }
                }
            }
        }


        else if (isSelected)
        {
            // Right Button clicks
            // Character movement
            // Crafting and picking up objects
            if (Input.GetMouseButtonDown(1))
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit))
                {
                    // Character movement
                    if (hit.transform.tag == "Floor" || hit.transform.tag == "Untagged" || hit.transform.tag == "Players" || hit.transform.tag == "OpenCrates")
                    {
                        actionItem = null;
                        actionItemScript = null;
                        actionCraftable = null;
                        actionCrate = null;
                        moveToPoint(hit.point);
                    }
                    // Item interaction
                    else
                    {
                        if( hit.transform.tag == "Item")
                        {
                            actionCraftable = null;
                            actionCrate = null;
                            actionItem = hit.transform.gameObject;
                            actionItemScript = actionItem.GetComponent<Item>();

                        }else if (hit.transform.tag == "Craftable")
                        {
                            actionItem = null;
                            actionItemScript = null;
                            actionCrate = null;
                            actionCraftable = hit.transform.gameObject.GetComponent<Craftable>();
                        }else if (hit.transform.tag == "Crate")
                        {
                            actionItem = null;
                            actionItemScript = null;
                            actionCraftable = null;
                            actionCrate = hit.transform.gameObject.GetComponent<Crate>();
                            crateAction = CrateAction.StoreItem;
                        }

                        moveToPoint(hit.point);
                    }
                }
            }
            // Middle mouse button
            else if (Input.GetMouseButtonDown(2))
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit))
                {
                    if( hit.transform.tag == "Crate")
                    {
                        actionItem = null;
                        actionItemScript = null;
                        actionCraftable = null;
                        actionCrate = hit.transform.gameObject.GetComponent<Crate>();
                        crateAction = CrateAction.Close;
                        moveToPoint(hit.point);
                    }
                    else if (inHandItem && inHandCrateScript && !animator.GetBool("throwBox"))
                    {
                        Island island;
                        if(island = hit.transform.gameObject.GetComponent<Island>())
                        {
                            animator.SetBool("throwBox", true);
                            throwTarget = island.landingCratePosition.position;
                        }
                        
                    }
                }
            }
            else if (Input.GetKeyDown(KeyCode.G))
            {
                if (inHandItem && !animator.GetBool("dropItem"))
                {
                    animator.SetBool("dropItem", true);
                }
            }
            else if (Input.GetKeyDown(KeyCode.Escape))
            {
                isSelected = false;
                animator.SetBool("isSelected", isSelected);
                Vector3 camPos = cameraRig.transform.position;
                Vector3 newPos = new Vector3(camPos.x, camPos.y + 10, camPos.z - 10);
                cameraRig.GetComponent<CameraController>().newPosition = newPos;
            }



            if (actionItem)
            {
                if (!inHandItem)
                {
                    if (!animator.GetBool("pickupItem") && GetHorizontalDistance(transform.position, actionItem.transform.position) <= 1f)
                    {
                        agent.ResetPath();
                        animator.SetBool("pickupItem", true);
                    }
                }
            }else if (actionCraftable)
            {
                if(inHandItem && actionCraftable.HasInteraction(inHandItemScript.itemType) && !animator.GetBool("craftRope") && GetHorizontalDistance(transform.position, actionCraftable.transform.position) <= 1f)
                {
                    agent.ResetPath();
                    animator.SetBool("craftRope", true);
                }
            }
            else if (actionCrate)
            {
                if (((inHandItem && crateAction == CrateAction.StoreItem && actionCrate.isOpen)  || (crateAction == CrateAction.Close)  && !animator.GetBool("boxAction")) && GetHorizontalDistance(transform.position, actionCrate.transform.position) <= 1f)
                {
                    agent.ResetPath();
                    animator.SetBool("boxAction", true);
                }
                else if (!inHandItem && !actionCrate.isOpen && !animator.GetBool("pickupBox") && GetHorizontalDistance(transform.position, actionCrate.transform.position) <= 5f )
                {
                    agent.ResetPath();
                    animator.SetBool("pickupBox", true);
                }
                
            }

        }


        if (!agent.pathPending && (agent.remainingDistance <= agent.stoppingDistance)) {
            animator.SetBool("isWalking", false);
            if (actionItem) {
                this.gameObject.transform.rotation = Quaternion.Slerp(this.gameObject.transform.rotation, Quaternion.LookRotation(actionItem.transform.position), 10 * Time.deltaTime);
            }
        }
    }
}



// Middle button clicks
// Close open crate
// Open closed crate
// Put itens in open crate
// Throw closed box
/*else if (Input.GetMouseButtonDown(2)) {
    if (isSelected) {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit)) {
            if (inHandItem) {
                if (inHandItem.tag == "Boxes") {
                    actionItem = inHandItem;
                    actionItemScript = actionItem.GetComponent<ItemScript>();
                    actionItemScript.player = this;
                    Vector3 hitPoint = hit.point;
                    throwIsland = hit.transform.parent.gameObject;
                    actionItemScript.throwIsland = throwIsland;
                    //actionItemScript.destiny = null;
                    Vector3 mouseDir = hitPoint - this.transform.position;
                    mouseDir = mouseDir.normalized;
                    throwDirection = mouseDir;
                    moveToPoint(hit.point);
                    //this.gameObject.transform.LookAt(actionItem.transform);
                    //transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(actionItem.transform.position), 10 * Time.deltaTime);
                    performAction(2);
                } else {
                    actionItem = hit.transform.gameObject;
                    actionItemScript = actionItem.GetComponent<ItemScript>();
                    actionItemScript.player = this;
                    moveToPoint(hit.point);
                    //this.gameObject.transform.LookAt(actionItem.transform, );
                    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(actionItem.transform.position), 10 * Time.deltaTime);
                    performAction(2);
                }   
            } else {
                actionItem = hit.transform.gameObject;
                actionItemScript = actionItem.GetComponent<ItemScript>();
                actionItemScript.player = this;
                moveToPoint(hit.point);
                //transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(actionItem.transform.position), 10 * Time.deltaTime);
                performAction(2);
            }
        }  
    }
}*/