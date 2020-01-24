// Player.cs

using Godot;
using System;


public class Player : Node2D {


    /* ________________________________________________________________________
     * HEADER.
     * ! IMPORTANT ! : SET VARIABLES IN _Ready().
     * ________________________________________________________________________
     * VARIABLE: DEFINITION
     *
     * * NS: Do not SET this value, only get it.
     * * NYI: Not Yet Implemented.
     *
     *
     * MOVEMENT RELATED
     * motion               : On MoveA*() by GD Physics to apply a mov. force.
     * gravityForce         : On _PhysicsProcess() to apply gravity force.
     * speed                : NS On MoveA*() to handle the current speed.
     * slippery             : On MoveA*() to slide obj.s on motion stop.
     *
     * baseSpeed            : On MoveA*() to set the base, common, speed.
     * runSpeed             : On MoveA*() to set the run speed.
     * jumpForce            : On Jump() to set the jumping motion force.
     * isRunning            : On MoveA*(), bool is it pressing to run?
     *
     * CONTROL RELATED
     * playerStatus         : NYI Handles player state, alive, dead, paused.
     * currentAnim          : On H.Anim() current animation being displayed.
     * isBlock              : On H.Anim(), MoveA*(), bool is it a block?
     * canMove              : On MoveA*(), bool can it move?
     * ________________________________________________________________________
    */


    public static Vector2 motion;
    public static int gravityForce;
    public static int speed;
    public static float slippery;

    public static int baseSpeed;
    public static int runSpeed;
    public static int jumpForce;
    public static bool isRunning;

    public static string keyPressed;
    public static string playerStatus;
    public static string currentAnim;
    public static bool isBlock;
    public static bool canMove;


    // This method is called once when the player is created in the scene.
    // It defines basic variables related to the player.

    public override void _Ready() {


        Main.playerNode =
            GetNode<Node2D>
            ("/root/Main/Objects/Player");

        Main.playerBodyNode =
            GetNode<KinematicBody2D>
            ("/root/Main/Objects/Player/body");

        Main.playerSpriteNode =
            GetNode<AnimatedSprite>
            ("/root/Main/Objects/Player/body/sprite");


        motion = new Vector2();
        gravityForce = 180;
        slippery = 0.08f;

        baseSpeed = 80;
        runSpeed = 130;
        jumpForce = -500;
        isRunning = false;

        playerStatus = "alive";
        currentAnim = "idle";
        isBlock = false;
        canMove = true;


    }


    /* ________________________________________________________________________
     * HEADER.
     * ________________________________________________________________________
     * FIXED UPDATE METHOD.
     *
     * This method is called every single frame, 60 times per second.
     * It calls useful methods, to handle movement, animations etc.
     *
     *
     * CommandDebug()       : Debug for variables. Not useful for builds.
     *
     * HandleInput()        : Handles all the Input Keys, calling methods.
     * HandlePhysics()      : Handles movement, physics and jumping.
     * HandleAnimations()   : Handles animations based on player actions.
     *
     * ________________________________________________________________________
    */


    public override void _PhysicsProcess(float delta) {


        CommandDebug();

        HandleInput();
        HandlePhysics();
        HandleAnimations();


    }


    // Debugs variables.
    // Simply uncomment a line.

    public void CommandDebug() {


        GD.Print(canMove);
        //GD.Print(motion);
        //GD.Print(currentAnim);
        //GD.Print(isBlock);
        //GD.Print(42);
        //GD.Print(42);

        Main.playerPos = Main.playerBodyNode.Position;


    }


    // Gets all the Input.
    // Calls the right methods.

    public void HandleInput() {


        keyPressed =
            (Input.IsActionPressed("RIGHT")
            && !Input.IsActionPressed("LEFT"))
            ? "right" : keyPressed;

        keyPressed =
            (Input.IsActionPressed("LEFT")
            && !Input.IsActionPressed("RIGHT"))
            ? "left" : keyPressed;

        keyPressed =
            (!Input.IsActionPressed("LEFT")
            && !Input.IsActionPressed("RIGHT"))
            ? "nil" : keyPressed;

        keyPressed =
            (Input.IsActionPressed("LEFT")
            && Input.IsActionPressed("RIGHT"))
            ? "both" : keyPressed;


        if (Input.IsActionJustPressed("BLOCK")) {
            currentAnim =
                (!isBlock)
                ? "block" : "wake";
            isBlock = !isBlock;
        }


        if (Input.IsActionJustPressed("JUMP")
            && Main.playerBodyNode.IsOnFloor()) {
            DoJump();
        } else if (Input.IsActionJustReleased("JUMP")) {
            DoCutJump();
        }


        isRunning =
            (Input.IsActionPressed("RUN"));


    }


    // Handles all the momevent.
    // Handles gravity and physics.

    public void HandlePhysics() {


        motion =
            Main.playerBodyNode.MoveAndSlide(motion, Main.UP);

        motion.y =
            Mathf.Lerp(motion.y, gravityForce, slippery);

        speed =
            (isRunning) ? runSpeed : baseSpeed;

        canMove =
            (currentAnim == "block"
            || currentAnim == "wake")
            ? false : true;

        switch (keyPressed) {


            case ("right"):
                Main.playerSpriteNode.FlipH = false;

                motion.x = (canMove)
                    ? Mathf.Lerp(motion.x, speed, slippery)
                    : Mathf.Lerp(motion.x, 0, slippery);

                currentAnim =
                    (currentAnim != "wake"
                    && currentAnim != "block")
                    ? "walk" : currentAnim;
                break;

            case ("left"):
                Main.playerSpriteNode.FlipH = true;

                motion.x = (canMove)
                    ? Mathf.Lerp(motion.x, -speed, slippery)
                    : Mathf.Lerp(motion.x, 0, slippery);

                currentAnim =
                    (currentAnim != "wake"
                    && currentAnim != "block")
                    ? "walk" : currentAnim;
                break;

            case ("nil"):
                motion.x = Mathf.Lerp(motion.x, 0, slippery);
                currentAnim =
                    (currentAnim != "wake"
                    && currentAnim != "block")
                    ? "idle" : currentAnim;
                break;

            default:
                motion.x = Mathf.Lerp(motion.x, 0, slippery);
                currentAnim =
                    (currentAnim != "wake"
                    && currentAnim != "block")
                    ? "idle" : currentAnim;
                break;
        }


    }


    // Applies jump force.
    public void DoJump() {


        motion.y =
            (Main.playerBodyNode.IsOnFloor()
            && canMove
            && currentAnim != "wake"
            && currentAnim != "block")
            ? jumpForce : motion.y;


    }


    // Cuts the jump in the middle
    // if the key is released.

    public void DoCutJump() {


        motion.y =
            (motion.y < -80)
            ? -80 : motion.y;


    }


    // Handles all the animations.
    // State machine hard coded.

    public void HandleAnimations() {


        // Jumping animation.

        currentAnim =
            (!Main.playerBodyNode.IsOnFloor()
            && currentAnim != "jump"
            && currentAnim != "wake"
            && currentAnim != "block")
            ? "jump" : currentAnim;


        // Main animations.

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
                // if the animation ended.
                if (Main.playerSpriteNode.Frame >= 5) {
                    Main.playerSpriteNode.Frame = 0;
                    currentAnim = "idle";
                    break;
                }
                break;

            case "block":
                Main.playerSpriteNode.Animation = "block";
                break;
        }


    }




}
