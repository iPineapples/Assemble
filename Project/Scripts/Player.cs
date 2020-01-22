// PLAYER.CS

using Godot;
using System;

public class Player : Node2D {

//	MOVEMENT VARIABLES
    public static Vector2 motion = new Vector2();	                            // velocity force vector.
    public static float slippery = 0.08f;			                            // slippery of the floor/air.
    public static int gravityForce = 180;				                        // self explanatory.
	public static int jumpForce = -500;				                            // self explanatory.
	public static int speed;					                                // speed. do not change this value, use baseSpeed or runSpeed instead.
	public static int baseSpeed = 80;				                            // the base speed of the player.
	public static int runSpeed = 130;				                            // the run speed of the player.

	public static string keyPressed;					                        // current movement key pressed used in GgetInput().

//	CONTROL VARIABLES
    public static string playerStatus = "alive";                                // not yet implemented, will handle the player stati.
    public static string currentAnim = "idle";
    public static bool isBlock = false;
    public static bool canMove = true;


            

    public override void _Ready() {
        Main.playerNode = GetNode<Node2D>("/root/Main/Objects/Player");
        Main.playerBodyNode = GetNode<KinematicBody2D>("/root/Main/Objects/Player/body");
        Main.playerSpriteNode = GetNode<AnimatedSprite>("/root/Main/Objects/Player/body/sprite");
    }



    public override void _PhysicsProcess(float delta) {

        // debug terminal commands.
        GD.Print(currentAnim);
        GD.Print(isBlock);
        //GD.Print(Main.playerPos);

        // call basic methods.
        GetInput();                 // handles all the input, including calling methods like Jump() and JumpCut().
        MoveAndGravity();           // handles all the basic physics. except jumping (on Jump() and JumpCut()).
        HandleAnim();
    }


    public void GetInput() {

        // gets the input from the player and stores it in a variable.
        if (Input.IsActionPressed("RIGHT") && !Input.IsActionPressed("LEFT")) {
            if (canMove) { keyPressed = "right"; } else { keyPressed = "nil"; }
        } else if (Input.IsActionPressed("LEFT") && !Input.IsActionPressed("RIGHT")) {
            if (canMove) { keyPressed = "left"; } else { keyPressed = "nil"; }
        } else if (!Input.IsActionPressed("LEFT") && !Input.IsActionPressed("RIGHT")) {
            keyPressed = "nil";
        } else {
            keyPressed = "both";
        }

        if (Input.IsActionPressed("RUN")) {
            speed = runSpeed;
        } else {
            speed = baseSpeed;
        }

        // handles jumping.
        if (Input.IsActionJustPressed("JUMP") && Main.playerBodyNode.IsOnFloor()) {
            Jump();
        }

        if (Input.IsActionJustReleased("JUMP")) {
            JumpCut();
        }

        // update Main Script with player's current position.
        Main.playerPos = Main.playerBodyNode.Position;
    }


    public void MoveAndGravity() {

        // moves the character using the physics engine, based on ----
        // motion, i.e. velocity force applied in a single direction -
        // and initialize gravity physics. ---------------------------
        // EDIT: NOW, ONLY IF IT CAN MOVE, PHYSICS WILL BE APPLIED.

        motion = Main.playerBodyNode.MoveAndSlide(motion, Main.UP);
        motion.y = Mathf.Lerp(motion.y, gravityForce, slippery);

        // switch the keys pressed and modify the motion to be applied.
        switch (keyPressed) {

            case ("right"):
                // flips the image.
                Main.playerSpriteNode.FlipH = false;
                // changes motion so it moves to the right.
                motion.x = Mathf.Lerp(motion.x, speed, slippery);

                if (currentAnim != "wake" && currentAnim != "block") {
                    currentAnim = "walk";
                }

                break;

            case ("left"):
                // flips the image.
                Main.playerSpriteNode.FlipH = true;
                // changes motion so it moves to the left.
                motion.x = Mathf.Lerp(motion.x, -speed, slippery);
                if (currentAnim != "wake" && currentAnim != "block") {
                    currentAnim = "walk";
                }

                break;

            // case nor right or left key is pressed.
            case ("nil"):
                // makes it stop, since it has no motion.
                motion.x = Mathf.Lerp(motion.x, 0, slippery);

                if (currentAnim != "wake" && currentAnim != "block") {
                    currentAnim = "idle";    
                }
                break;

            default:
                if (currentAnim != "wake" && currentAnim != "block") {
                    currentAnim = "idle";
                }
                break;

        }

        if ((currentAnim == "block") || (currentAnim == "wake")) {
            canMove = false;
        } else {
            canMove = true;
        }

    }


    public void Jump() {

        // if is on floor, apply jump force.
        if ((Main.playerBodyNode.IsOnFloor()) && (!isBlock) && (currentAnim != "wake")) {
            motion.y = jumpForce;
        }
    }


    public void JumpCut() {

	// if motion is big, cut the motion to mere -80.
        if (motion.y < -80) {
            motion.y = -80;
        }
    }


    public void HandleAnim() {

        if ((Main.playerBodyNode.IsOnFloor() == false) 
            && (currentAnim != "jump")
            && (currentAnim != "wake")
            && (currentAnim != "block"))
            {
                currentAnim = "jump";
        }

        switch (currentAnim) {
            case "idle":
                Main.playerSpriteNode.Animation = "idle";
                break;

            case "walk":
                Main.playerSpriteNode.Animation = "walk";
                break;

            case "jump":
                Main.playerSpriteNode.Animation = "jump";
                break;

            case "wake":
                Main.playerSpriteNode.Animation = "wake";
                GD.Print("WAKING..............");
                if (Main.playerSpriteNode.Frame >= 5) {
                    Main.playerSpriteNode.Frame = 0;
                    // animation ended
                    currentAnim = "idle";
                    break;
                }

                break;

            case "block":
                Main.playerSpriteNode.Animation = "block";
                break;
        }

        if (Input.IsActionJustPressed("BLOCK")) {

            if (!isBlock) {  
                currentAnim = "block"; 
                GD.Print("BLOCKFY"); 
                isBlock = true; 
            } else { 
                currentAnim = "wake"; 
                GD.Print("WAKE"); 
                isBlock = false; 
            }
        
        
        }

    }

}
