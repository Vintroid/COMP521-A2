# COMP521-A2

Vincent St-Pierre ID: 260986454<br>
Note: I am using colliders for raycast purposes only. They are not<br>
being used for collision detection or resolution.<br>

Verlet Constraints:<br>
Verlets have an offset from the eye given on initialization.<br>
This offset is multiplied by a rotation matrix that follows <br>
the current velocity of the fish eye. This means that while <br>
the offset varies in its x,y coordinates, the relative distance<br>
between points and the fish eye stays the same. This relative <br>
distance is the constraint. <br> 

Unfortunately I have not implemented the verlets to interact with<br>
the environment. This means that verlets will penetrate other<br>
obstacles.