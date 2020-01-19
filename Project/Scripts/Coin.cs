using Godot;
using System;

public class Coin : Node2D
{

    public bool isDying = false;


    public override void _PhysicsProcess(float delta) {
        checkDie();
    }


    private void _OnAreaBodyEntered(KinematicBody2D body) {
        
        // if the node touching the coin area name is Player--
        // add score once, then delete the Area2D Collision2D-
        // then call checkDie() every frame from -------------
        // _PhysicsProcess(). --------------------------------
        if (body.GetParent().Name == "Player") {
            Main.score++;
            GetNode(GetPath() + "/area").QueueFree();
            isDying = true;
        }
    }


    private void checkDie() {

        // if it is dying, animate a scaling up and delete the node.
        if (isDying == true) {
            Scale = new Vector2(Mathf.Lerp(Scale.x, 1.6f, 0.02f), Mathf.Lerp(Scale.y, 1.6f, 0.02f));
            Modulate = new Color(Modulate.r, Modulate.g, Modulate.b, Mathf.Lerp(Modulate.a, 0, 0.05f));
        }

        // if the scale is big enough, the animation ended: delete the node.
        if (Scale.x >= 1.5f) {
            QueueFree();
        }
    }


}