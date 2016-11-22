﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
 
[RequireComponent (typeof(Collider))]
public class JumpPhysics : MonoBehaviour
{
    public bool showDebugRays = false; // debug flag to show the Raycasts used for collision detection
 
    public float jumpPower = 5.0f;
    public float gravity = -.1f;
 
    public bool allowVariableJumpHeight = false;
    public float holdJumpUpwardSpeed = 5.0f; // speed at which object moves up towards max
    public float holdJumpMaxHeight = 10.0f;  // max hold jump height before applying gravity
    private float initialYPosition = 0;      // initial y position used to calculate at if at max
 
    // double jump flag and vars similar to variable jump height
    public bool allowDoubleJump = false;
    public float doubleJumpPower = 5.0f;
    public float doubleJumpUpwardSpeed = 5.0f;
    public float doubleJumpMaxHeight = 10.0f;
 
    // holds name of Layer that belongs to player so we can ignore during raycast
    public string playerLayerName;
    
    // lists that hold tags and layers object can collide with that affect jump
    public List<string> collidingTagList;
    public LayerMask collidingLayerMask;
 
    // amount ray extends beyond collider
    public float collisionRayDepthUp = .01f;
    public float collisionRayDepthDown = .01f;
 
    // offset each ray to the left/right before casting up/down to match character
    public Vector3 collisionRayLeftOffset = new Vector3(-.2f, 0, 0);
    public Vector3 collisionRayRightOffset = new Vector3(.2f, 0, 0);
 
    private float currentGravity = 0;
    private Vector2 moveDirection = Vector2.zero;
    private Vector2 jumpDirection = Vector2.zero;
  
    public bool IsGrounded { get; private set; }
    public bool IsHoldingJump { get; private set; }
    public bool InAir { get; private set; }
    public bool IsMaxHeightReached { get; private set; }
    public bool IsDoubleJumping { get; private set; }
    public bool HasDoubleJumped { get; private set; }
    public bool HasJumpedOnce { get; private set; }
 
    void Awake()
    {
        IsGrounded = false;
        IsHoldingJump = false;
        InAir = false;
        
        // if playerLayerName blank, default to 'Ignore Raycast'
        if (gameObject.layer == 0)
            gameObject.layer = 2;
    }
 
    void Update()
    {
        // check collision with ground
        if (RaycastCollideVertical(Vector3.down)) {
            IsGrounded = true;
            IsMaxHeightReached = false;
            HasDoubleJumped = false;
            IsDoubleJumping = false;
            HasJumpedOnce = false;
            InAir = false;
            currentGravity = 0;
            jumpDirection = Vector2.zero;
            initialYPosition = transform.position.y;
        } 
        // else we're not grounded (probably falling off ledge)
        else if (!InAir) {
            InAir = true;
            IsGrounded = false;
            IsHoldingJump = false;
            currentGravity = gravity;
        }
        
        // perform physics update
        CalculateMoveDirection();
 
        // move via transform component
        transform.Translate(moveDirection * Time.deltaTime);

        // draw the rays for collisions with environment
        if (showDebugRays)
            DebugRays();
    }
    
    bool JumpInputCheck()
    {
        // button press
        return Input.GetButtonDown("Jump");
    }
    
    bool RemovedJumpInputCheck()
    {
        // let go of button
        return Input.GetButtonUp("Jump");
    }
    
    Vector2 GetJumpPowerVector()
    {
        return (HasDoubleJumped) ? new Vector2(0, doubleJumpPower) : new Vector2(0, jumpPower);
    }
  
    void CalculateMoveDirection()
    {
        moveDirection = Vector2.zero;

        // checks: double jump allowed, hasn't double jumped yet, has jumped once, and jump button pushed
        // sets: is double jumping this frame, resets jump key hold, jump velocity, initial Y position
        if (allowDoubleJump && !HasDoubleJumped && HasJumpedOnce && JumpInputCheck()) {
            IsDoubleJumping = true;
            IsHoldingJump = false;
            jumpDirection = Vector2.zero;
            initialYPosition = transform.position.y;
        }

        // checks: on ground or double jumping this frame, jump button pushed, and not hitting ceiling
        if (!IsHoldingJump && (IsGrounded || (allowDoubleJump && IsDoubleJumping && !HasDoubleJumped)) && JumpInputCheck() && !RaycastCollideVertical(Vector3.up)) {
            IsHoldingJump = true;
            InAir = true;
            IsGrounded = false;
            currentGravity = gravity;
            
            // needed for double jump check
            HasJumpedOnce = true;
            if (IsDoubleJumping)
                HasDoubleJumped = true;
 
            // if variable jump height allowed, start us moving at variable jump height speed else regular jump (or second jump of double jump)
            jumpDirection = (allowVariableJumpHeight) ? new Vector2(0, holdJumpUpwardSpeed) : GetJumpPowerVector();
            IsMaxHeightReached = false;
        }
        
        // let go of jump button
        if (RemovedJumpInputCheck()) {
            IsHoldingJump = false;
        }
    
        // if airborne, apply gravity to current jump speed then add to movement vector
        if (InAir) {

            // check if we're at max height from variable jump height or hitting ceiling so we can apply gravity
            if (RaycastCollideVertical(Vector3.up) || (allowVariableJumpHeight && AtMaxVariableJumpHeight())) {
                IsMaxHeightReached = true;
            }

            // if hit ceiling, stop all upward movement.
            if (RaycastCollideVertical(Vector2.up)) {
                jumpDirection = (jumpDirection.y > 0) ? new Vector2(0, 0) : jumpDirection;
            }

            // apply gravity if normal jump or if allowing variable jump height, at max variable jump height or released button
            if (!allowVariableJumpHeight || (allowVariableJumpHeight && (IsMaxHeightReached || !IsHoldingJump)))
                jumpDirection += new Vector2(0, currentGravity);
 
            // moveDirection holds final value of movement delta
            moveDirection += jumpDirection;
        }
    }
 
    bool AtMaxVariableJumpHeight()
    {
        // true if while holding jump, our current y position is equal or over the initial y position before jump
        // if double jump enabled, we use seperate max heights.
        if (allowDoubleJump && IsDoubleJumping)
            return (transform.position.y - initialYPosition) >= doubleJumpMaxHeight;
        else
            return (transform.position.y - initialYPosition) >= holdJumpMaxHeight;
    }
 
    bool RaycastCollideVertical(Vector3 direction)
    {
        // creating rays to cast, 1 for each side of object
        Ray floorRay_L = new Ray(transform.position + collisionRayLeftOffset, direction);
        Ray floorRay_R = new Ray(transform.position + collisionRayRightOffset, direction);
    
        // which way and how deep should we cast
        float directionSign = (direction == Vector3.up) ? 1 : -1;
        float directionDepth = (direction == Vector3.up) ? collisionRayDepthUp : collisionRayDepthDown;
 
        RaycastHit hit;
        // hit on left side
        if (Physics.Raycast(floorRay_L, out hit, this.GetComponent<Collider>().bounds.extents.y + directionDepth, collidingLayerMask)) {
            // we hit explicit tagged object
            if (HitTagOrLayerObject(hit.collider.gameObject)) {
                // set our position to hit point so we don't stutter in place
                transform.position = new Vector3(transform.position.x, hit.point.y + (-directionSign * this.GetComponent<Collider>().bounds.extents.y), transform.position.z);
                return true;
            }
        }

        // hit on right side
        if (Physics.Raycast(floorRay_R, out hit, this.GetComponent<Collider>().bounds.extents.y + directionDepth, collidingLayerMask)) {
            // we hit explicit tagged object
            if (HitTagOrLayerObject(hit.collider.gameObject)) {
                // set our position to hit point so we don't stutter in place
                transform.position = new Vector3(transform.position.x, hit.point.y + (-directionSign * this.GetComponent<Collider>().bounds.extents.y), transform.position.z);
                return true;
            }
        }
        // no hit
        return false;
    }
  
    bool HitTagOrLayerObject(GameObject collisionObject)
    {
        // check if we are colliding by tag
        if (collidingTagList.Count > 0) {
            foreach (string tag in collidingTagList) {
                if (collisionObject.CompareTag(tag))
                    return true;
            }
            return false;
        }
 
        return true;
    }
 
    // draws all rays in up and down direction
    void DebugRays()
    {
        Ray dfloorRay_L = new Ray(transform.position + collisionRayLeftOffset, Vector3.down);
        Ray dfloorRay_R = new Ray(transform.position + collisionRayRightOffset, Vector3.down);
        Ray ufloorRay_L = new Ray(transform.position + collisionRayLeftOffset, Vector3.up);
        Ray ufloorRay_R = new Ray(transform.position + collisionRayRightOffset, Vector3.up);
 
        Debug.DrawLine(dfloorRay_L.origin, dfloorRay_L.origin - new Vector3(0, this.GetComponent<Collider>().bounds.extents.y + collisionRayDepthUp, 0), Color.magenta);
        Debug.DrawLine(dfloorRay_R.origin, dfloorRay_R.origin - new Vector3(0, this.GetComponent<Collider>().bounds.extents.y + collisionRayDepthUp, 0), Color.magenta);
        Debug.DrawLine(ufloorRay_L.origin, ufloorRay_L.origin + new Vector3(0, this.GetComponent<Collider>().bounds.extents.y + collisionRayDepthDown, 0), Color.red);
        Debug.DrawLine(ufloorRay_R.origin, ufloorRay_R.origin + new Vector3(0, this.GetComponent<Collider>().bounds.extents.y + collisionRayDepthDown, 0), Color.red);
    }
}