// MAIN.CS

using Godot;
using System;

public class Main : Node {

	// MOVEMENT
	public static Vector2 UP                        = new Vector2(0, -1);

	// GLBOAL NODES
	public static Node2D playerNode                 = new Node2D();
	public static KinematicBody2D playerBodyNode    = new KinematicBody2D();
	public static AnimatedSprite playerSpriteNode   = new AnimatedSprite();

	// GLOBAL PROPERTIES/VARIABLES
	public static Vector2 playerPos                 = new Vector2();
	public static float tDelta;
	public static int score;
	public override void _PhysicsProcess(float delta) {

        // globally available delta time.
        tDelta = delta;
		
	}


}
