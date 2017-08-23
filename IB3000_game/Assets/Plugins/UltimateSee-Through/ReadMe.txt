Note: -I have a video tutorial on youtube: https://youtu.be/lm0V5gFx1u8

-There is a player prefab included with this project which has corrected movement for any rotation of the camera, to help you get started with the test scene if nothing else. The prefab is very basic and only supports movement and two animations.
If you want to use the system which makes the camera follow the player, add the 'FollowPlayerExample' script to the main camera.



Important: Make sure you have a camera with the tag 'Main Camera' in the scene, and that all your objects you want to make transparent have colliders attached to themselves or their children.
If you are updating to version 1.1, I recommend you to move the old asset folder to 'Plugins' before updating.



Linked transparency

In the example scene, one of the clocks and one of the cars are linked together, so if one of the objects is blocking the view of the player, both of them will turn transparent. All you have to do to achieve this is to add the transforms of the other objects colliders to the array 'Array Of Transforms With Colliders' on the 'Change Material' script.




Tool functions
To locate the tool functions, go to the 'Window' tab, select 'Ultimate See-Through', and then you will be given four options:


-Regular Transparency (standard): "Ctrl/Cmd + g", creates transparent materials and scripts needed for the system.

-Use Collider To Trigger Transparency: "Ctrl/Cmd + h", useful for triggering the transparency effect for NPCs.

-Don't Make Bark Materials Transparent: "Ctrl/Cmd + j", described in step 7 below.

-Dont Make Leaf Materials Transparent: "Ctrl/Cmd + k", described in step 8 below.





Step-by-step guide:

1. Make sure your playable character is named "Player", and that he's in the scene (hierarchy).

2. Select the gameobjects you want to make transparent when they are blocking the view of the player (children with materials will get the treatment too, as will meshes with multiple materials). You can also select prefabs in the 'Project' window.

3. Use the 'Regular Transparency (standard)' or 'Linked Transparency' tool option. The script 'Change Material' will be added to the objects, and this is the script used for the steps below. The main camera will also receive two scripts that are used for this system, more on that in step 6 below.

4. Now it's time to change the parameters to suit your needs.

4-1. 'Time To Turn Transparent' is how many seconds it should take to reach the desired transparency value. This value will be adjusted in the following steps.

4-2. 'Time To Turn 'Non Transparent' determines the required time to become non transparent again.

4-3. Play the scene, move the player behind one of the objects, and change the values of the 'Alpha Amounts Array' to affect the level of transparency. A lower alpha value means a higher opacity. If you want to know which array element is which, expand the array 'Transparent Materials'.

Once you have found good alpha values for each material, you can play with the value of 'Time To Turn Transparent' and 'Time To Turn Non Transparent' to find the best transition to and from transparency respectively for your object.

You also have the option to set the 'Easing Method To Transparent', 'Easing Type To Transparent', 'Easing Method To Non Transparent', 'Easing Type To Non Transparent'  for each material. The easing determines what curve should be used for transparency. Play around with it to find something fitting.

After you're happy with the values for the transition, use the button 'Copy Values' of the component, and go to the next object you want to adjust transparency for. Doing this for only one instance of each prefab is enough. The values saved by clicking the button 'Copy Values' are saved as long as you don't crash while in play mode, even when you exit Unity.
When you're done with the gathering of all the different values for the transparency, end play mode, and then click on the button 'Paste Values' to update all the values gathered while in play mode (all objects are updated). The prefabs are updated with the new values as well.


4-4. Depending on the situation, you might have to check the box 'Use Instanced Materials Instead' for some of your prefabs or objects. This is useful when you have gameobjects with the same materials very close to one another, and the transparency don't have time to revert before a new object needs the same transparent material.

4-5. There is also a button called 'Delete All Saved Values' in case you need it.

4-6. For objects using any leaf shader, a button called 'Change To Other Tree Shader' will be showed. Clicking this once will switch to a legacy transparency shader, pushing it again will switch back to the nature shader. Using the legacy transparency shader will make the transitions look pretty bad but it grants true transparency compared to the alpha cutoff of the nature shader.

4-7. 'Disable Mesh When At Lowest Transparency': check this box to disable a mesh completely, which is preferable for roofs.

4-8. 'Array Of Transforms With Colliders': all the transforms of the child colliders of the object will be added to this array, which will be used to check if hit by raycast.

5. The arrays called 'Transparent Materials' and 'NonTransparent Materials' lists all the transparent and non-transparent materials used for the object.

6. The 'Camera Tranpsarent Script', which is added to the 'Main Camera' script, has a 'Raycast Interval' variable which decides how often to check for objects blocking the view of the player. It is set to 0 by default, and that means it checks every turn, a 1 means every other turn etc. The 'Saved Lists' script stores all the copied values.

7. If you don't want the bark materials of a tree to turn transparent: select the object, and then use the hotkey "Ctrl/Cmd + j". The prefab will be automatically updated. If you want to switch back to making the bark transparent, just use the hotkey "Ctrl/Cmd + g", and don't forget to update the prefab if you do - by hitting the 'Apply' button.

8. In the case you don't want the leaf materials to turn transparent: select the object, and then use the hotkey "Ctrl/Cmd + k". The prefab will be automatically updated. If you want to switch back to making the leaves transparent, just use the hotkey "Ctrl/Cmd + g", and don't forget to update the prefab if you do - by hitting the 'Apply' button.



'Use Collider To Trigger Transparency'
If you want to use this method, you only have to use the hotkey "Ctrl/Cmd + h" to create a new gameobject with a collider and the needed script 'Collider Trigger'. This script contains an array that you can use to call objects to turn transparent. Drag the scripts of the objects you want this effect for to this array. Don't forget to set the physics layer collision for the trigger to trigger for enemies and/or player.