# ViveUGUIModule

## Overview

This is a simple module you can add in to your project to emulate a mouse on ugui with the vive controller. It works for most cases I've tested and is easily expandable. The important things in this project are in the root of the project. ViveControllerInput.cs and UIIgnoreRaycast.cs. I've included the standard 4.6 ugui example. The scenes folder contains the unmodified scenes and in the root directory is Menu 3D which I've added a vrplayer to and the input module as an example.

## Disclaimer

In my opinion, using 2D UI in a VR environment should be a last resort. We have the opportunity to invent new and interesting ways to interact with our worlds, and we should do so. But, sometimes we don't have time for that, and it may even be possible that using physical objects to display information is not always the best route.

## Setup

If you are using the valve interaction system this is very straight forward. In VRPlayer there is a child called Input Module. On that module there is a component called VR Input Module. Disable that component and add a ViveControllerInput component to it. Setup the sprite you want to use as a cursor, and optionally give it a material. You can modify scale later if it's too big / small. That's it. Everything else should be automatic.

## Notes

If you generate canvases at runtime you will need to set their Canvas.worldCamera to ViveControllerInput.Instance.ControllerCamera. The script does this on start for existing canvases (line 81)

Using two Input Modules in an event system doesn't seem to be supported.

## How it works

With this system I wanted to allow players to be looking at something else put still hit buttons on a UI. The unity ui event system is relatively straight forward. You tell it when and where a "pointer" is and it will do the rest. The tricky bit was the ui raycasting, which seems like it can only be done from a camera. So on start I create a camera, set it to not actually render anything, then on each frame I set its position to each controller, and raycast from there. It's a bit hacky but it looks like it works well and has a tiny overhead.