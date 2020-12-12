# Box Shooter
 
 Project developed on October 16, 2020, during the Introduction to Game Development class at Coursera, offered by Michigan State University.
 There is a simple objective, shoot the boxes until you reach a high score (50 for level 1 and 100 for level 2). You have initially 15 seconds and each target can increase or reduce your time and score, so shoot wisely. Use "WASD" or the arrow keys to navigate, the mouse to shoot and control the camera, and the "ESC" button to release the mouse from the game scene.
 
 Play the game <a href="https://play.unity.com/mg/other/webgl-builds-2534" target="_blank" rel="noopener noreferrer">here</a>
 
 
 # Level 1
 
 Level 1 was built in class and latter modified.
 - Modified the Spawn Game Object script to add more control over the number of targets, the spawn position, and the chance to spawn each object.
 - Added a Timer Penalty UI to display how much time is reduced or increased by destroying each target.
 - Added an animated 3D text to each target explosion prefab, which displays the number of points lost or gained by destroying each target.
 - Modified the Target Behavior script to provide means to randomly vary the number of points given by destroying each target.
 - Player projectile is destroyed after hitting anything.
 
 
 # Level 2
 
 Level 2 was developed by me.
 - Included a new type of enemy that shoots in the player direction, gives a high number of points when destroyed, but also reduces the player time.
 - Modified the platform design in which the player stands.
 - Added "wood" barriers, which protects the player, and is destroyed after a certain amount of damage.
 - Added audio effects for the new enemy (when shooting or being destroyed), for the barriers being shot and for the player being shot.
