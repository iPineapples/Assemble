using Godot;
using System;

public class Main : Node {

    public static Vector2 UP                        = new Vector2(0, -1);
    public static Node2D playerNode                 = new Node2D();
    public static KinematicBody2D playerBodyNode    = new KinematicBody2D();
    public static AnimatedSprite playerSpriteNode   = new AnimatedSprite();
    public static Vector2 playerPos                 = new Vector2();


    public override void _Ready() {

    }


}