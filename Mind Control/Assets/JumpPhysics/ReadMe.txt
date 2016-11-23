=============================================
Jump Physics 

email:   poemdexter@gmail.com
twitter: @poemdexter
website: http://poemdexter.com/
=============================================

= Description = 

Jump Physics does what it says on the box. It provides jump physics for your
2D or 3D game. There are two scripts: JumpPhysics.cs and JumpPhysics2D.cs 
for 3D games and 2D games respectively as the physics engines are different in
Unity. 

Jump Physics will provide a single height jump by default when added to a
player GameObject. Collisions are detected through RayCasts for both 2D and 3D
Options for double jump and variable jump height can be activated by a simple
checkbox.

There is also a 'Sample' folder containing a scene with both 2D and 3D player
GameObjects with some example configurations on each. A web player version of
the sample scene is available at: http://poemdexter.com/unity-player/jump-
physics.html

= Notes =

Jump Physics will only provide vertical movement for the attached GameObject.
Horizontal movement should be taken care of by the user and is beyond the
scope of these scripts.

While setting a layer is not necessary for Jump Physics to work, the layer
will be change from Default to Ignore Raycast on GameObjects with the Jump
Physics script on it.

= Optional Feature Behavior =

[Double Jump]
Player will jump normally when grounded, and if the player is still in the air
when Jump is pressed again, the player's gravity and vertical velocity values
will reset and a new jump will start. Any vertical momentum is removed when
the second jump is started.

[Variable Jump Height]
Player holding down Jump key will jump higher until letting go of the key or
hitting the specified maximum height. Movement while holding Jump key is linear
velocity. After letting go of Jump key or hitting max height, a normal jump
occurs.

= Setup =

For 3D, drag the JumpPhysics script onto your player GameObject. 3D Jump
Physics requires a Collider component on the GameObject for it to work
correctly.

For 2D, drag the JumpPhysics2D script onto your player GameObject. 2D Jump
Physics requires a SpriteRenderer component on the GameObject for it to work
correctly.

Beyond this initial step, the rest can be accomplished through changing values
in the Inspector window.

= Configuration = 

This section will go through each option in the script as seen in the
Inspector window.

[Show Debug Rays]
This debug flag will draw the rays casted for collision detection. Start the
scene and then pause to view the Ray lines in the Scene tab.

[Jump Power]
Determines the strength of the jump. Bigger numbers means more height.

[Gravity]
Force pulling the player down. Gravity must be a negative number if natural
gravity is expected (pulls player down).

[Allow Variable Jump Height]
This flag enables variable jump height feature.

[Hold Jump Upward Speed]
Linear velocity of the variable portion of the jump before hitting maximum
height.

[Hold Jump Max Height]
Maximum height allowed before the regular jump portion of arc occurs.

[Allow Double Jump]
This flag enables the double jump feature.

[Double Jump Power]
Determines the strength of the second jump of double jump.

[Double Jump Upward Speed]
If variable height feature is enabled as well, sets the linear velocity of the
variable portion of the jump before hitting maximum height of the second jump.

[Double Jump Max Height]
If variable height feature is enabled, sets the maximum height allowed before
the regular jump portion of arc occurs of the second jump.

[Player Layer Name]
The name of the layer that the player's GameObject resides. This prevents the
Raycast from colliding with player. If not set, the player's GameObject layer
will be set to 'Ignore Raycast'.

[Colliding Tag List]
A list of all the tags that the player should collide with.

[Colliding Layer Mask]
A list of all the layers that the player should collide with. The layers
should be checked if we want to collide.

[Collision Ray Depth Up]
The distance the ray should extrude up beyond the top of the Collider in 3D or
top of Sprite in 2D. The depth is more than like the portion that will
register the collision.

[Collision Ray Depth Down]
The distance the ray should extrude down beyond the bottom of the Collider in
3D or bottom of Sprite in 2D. The depth is more than like the portion that
will register the collision.

[Collision Ray Left Offset]
The distance left from center the ray should be cast.

[Collision Ray Right Offset]
The distance right from center the ray should be cast.