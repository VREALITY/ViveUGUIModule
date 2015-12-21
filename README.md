# ViveUGUIModule

## Overview
While doing rapid prototyping at VREAL it has been really nice to be able to interact with Unity UI elements with the Vive controllers. Displaying a lot of debug information or just trying to make quick interactions is really complex if you're trying to do it in physical space. So I created a Unity UI input module for the Vive that lets you interact with UGUI elements in world space. It works for most of the basic use cases and is easily expandable. To help get other people started and so we're not duplicating work we've decided to release it on github. The meat of the project is ViveControllerInput.cs in the root directory of the project but you'll also need UIIgnoreRaycast.cs. I've included the standard 4.6 UGUI example as well to start you off. In the scenes folder you'll find the unmodified scenes and in the root is Menu 3D which I've added the Vive camera rig to and the input module as an example.

## Disclaimer
Carefully consider whether using a 2D UI is the best input method for your VR game before you use it for a consumer facing project. We have the opportunity to invent new and interesting ways to interact with our worlds, and we should do so. But, there is the occasional situation where interacting with a 2d plane using point and click UI is the best route. And sometimes, we just don't have time to reinvent human to computer interfaces.

## Notes
If you generate canvases at runtime you will need to set their Canvas.worldCamera to ViveControllerInput.Instance.ControllerCamera. The script does this on start for existing canvases (line 81)
Using two Input Modules in an event system doesn't seem to be supported at time of writing.

## Setup
Start off by downloading the project off github at this link: https://github.com/VREALITY/ViveUGUIModule
If you are using the valve camera rig this is very straight forward. On the camera rig create a child object called Input Module. On that gameobject add a ViveControllerInput component to it. Setup the sprite you want to use as a cursor, and optionally give it a material. You can modify scale later if it's too big / small. That's it. Everything else should be automatic.

## How it works
With this system I wanted to allow players to be looking at something else but still hit buttons on a UI. The unity UI event system is relatively straight forward. You tell it when and where a "pointer" is and it will do the rest. The tricky bit was the UI raycasting, which seems like it can only be done from a camera. On start I create a camera, set it to not actually render anything, then on each frame I set its position to each controller, and ui raycast from there. It's a bit hacky but it looks like it works well and has a tiny overhead.
