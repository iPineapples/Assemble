// PLAYER.CS

using Godot;
using System;

public class Player : Node2D {


    public static float tJump, tbJump   = 1.0f;

    public static Vector2 motion        = new Vector2();
    public static int speed             = 70;
    public static int runSpeed          = 100;
    public static int jumpForce         = -600;
    public static int gravityForce      = 180;
    public static float slippery        = 0.1f;
    public static string playerStatus   = "alive";
    public static string keyPressed;
    public static bool jumping          = false;


    public override void _Ready() {
        Main.playerNode = GetNode<Node2D>("/root/Main/Objects/Player");
        Main.playerBodyNode = GetNode<KinematicBody2D>("/root/Main/Objects/Player/body");
        Main.playerSpriteNode = GetNode<AnimatedSprite>("/root/Main/Objects/Player/body/sprite");

    }


    public override void _PhysicsProcess(float delta) {
        // debug terminal commands.
        //GD.Print(motion);
        //GD.Print(Main.playerPos);
        
        // call basic methods.
        GetInput();
        if (jumping == true) { Jump(); }
        MoveAndGravity();
        // Jump();
    }


    public static void GetInput() {
        // gets the input from the player and stores it in a variable.
        if (Input.IsActionPressed("ui_right") && !Input.IsActionPressed("ui_left")) {
            keyPressed = "right";
        } else if (Input.IsActionPressed("ui_left") && !Input.IsActionPressed("ui_right")) {
            keyPressed = "left";
        } else if (!Input.IsActionPressed("ui_left") && !Input.IsActionPressed("ui_right")) {
            keyPressed = "nil";
        } else {
            keyPressed = "both";
        }

        
        if ((Input.IsActionJustPressed("ui_up") && Main.playerBodyNode.IsOnFloor() && jumping == false)) { 
            jumping = true;
            }
        
    /*
        if (Input.IsActionJustPressed("ui_up")) {
            if (Main.playerBodyNode.IsOnFloor()) {
                jumping = true;
            }
        }
    */
        /*if (!Main.playerBodyNode.IsOnFloor()) {
            jumping = false;
        }*/

        // update Main Script with player's current position.
        Main.playerPos = Main.playerBodyNode.Position; 

    }


    public static void MoveAndGravity() {
        // moves the character using the physics engine, based on ----
        // motion, i.e. velocity force applied in a single direction -
        // and initialize gravity physics. ---------------------------
        motion = Main.playerBodyNode.MoveAndSlide(motion, Main.UP);
        motion.y = Mathf.Lerp(motion.y, gravityForce, slippery);


        // switch the keys pressed and modify the motion to be applied.
        switch (keyPressed) {
            case ("right"):
                // flips the image.
                Main.playerSpriteNode.FlipH = false;
                // changes motion so it moves to the right.
                motion.x = Mathf.Lerp(motion.x, speed, slippery); 
                break;

            case ("left"):
                // flips the image.
                Main.playerSpriteNode.FlipH = true;
                // changes motion so it moves to the left.
                motion.x = Mathf.Lerp(motion.x, -speed, slippery);
                break;
            // case nor right or left key is pressed.
            case ("nil"):   
                // makes it stop, since it has no motion.
                motion.x = Mathf.Lerp(motion.x, 0, slippery); 
                break;

            default:
                break;
        }

    }


    public static void Jump() {

        if (Main.playerBodyNode.IsOnFloor()) {
            motion.y = jumpForce;
        }

        jumping = false;


/*  THIS ONE EXECUTES ON 3 SECOND PERIODS
        if (tJump >= 0.1f) {
            tJump -= Main.tDelta;
            if (motion.y <= -190) {
                motion.y = Mathf.Lerp(motion.y, 200, 0.2f);
            } else {
                motion.y = Mathf.Lerp(motion.y, -200, 0.5f);
            }
            
        } else {
            tJump = 1.0f;
            jumping = false;
        }
*/

        // motion.y = Mathf.Lerp(motion.y, -600, 0.2f);


/*        if (tJump >= 0.1f && jumping == true) {
            GD.Print("JUMPING TIMER");
            motion.y = Mathf.Lerp(motion.y, -300, 0.1f);
            tJump -= Main.tDelta;
            if (Main.playerBodyNode.IsOnFloor()) {
                jumping = false;
            }

        } else {
            GD.Print("ABC BITCHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHH");
            tJump = tbJump;
            jumping = false;
        }
*/
/*        if (Input.IsActionJustPressed("ui_up") && Main.playerBodyNode.IsOnFloor()) {
                motion.y = -200;
                GD.Print("UP");
                while (!Main.playerBodyNode.IsOnFloor()) {
                    motion.y = Mathf.Lerp(motion.y, -300, 0.1f);
                    GD.Print("AAA");
                
                }
            }
        }
*/

/* 
            JUMPING
            
            when key up is pressed once:
                if player is on the floor:
                    StartJumping();
            
            StartJumping() {
                while not on floor {
                    lerp my motion.y to my jumpForce var;
                }
            }

        */
    }
}