# DrivingAndExploration
Creating vehicle mechanics for a driving and exploration project. This project has been in development since 29 Jan 2021 week.

I am experimenting with (basic) physics based gameplay in this project, unlike the Character Controller in my FirstPersonProject which implemented movement by transforming GameObject vectors directly. The exploration aspect of this project is referring to an intro to 3D modelling and level design.

As a physics based project I had to learn basic physics concepts and how they function in Unity: friction, angular drag, angular velocity, centre of mass, box collider mass, angular torque etc.

## Overview of Features
Vehicle mechanics (using Rigidbody):
- Exterior Vehicle Camera
- Vehicle is driven by applying forces to affect it's physics
- Vehicle is turned along Y Axis (Left and Right) by applying torque
- Vehicle can only be driven when front wheels are grounded
- Vehicle has a turning radius
- Vehicle has realistic turning at very low speeds
- Vehicle has correct/realistic reverse turning
- Centre of mass of vehicle has been positioned several float units behind the main object for the driving force to feel more natural.
- Vehicle uses balanced box colliders to represent AWD, can be modified for FWD or RWD behaviour

Vehicle's 3D Model was "kit bashed" using Asset Forge 2 by Kenney (https://kenney.itch.io/assetforge). Using Kenney's free open source assets enabled me to test the vehicle mechanics on some form of a race track. The final race track was created in Blender using curves and arrays and continues to use Kenney's assets for environment art and design.

First version of vehicle mechanics:
![](https://github.com/bM7tcHF88GBxDni/README-GIF-Storage/blob/main/car%20453px.gif)

Some footage of iteration of vehicle mechanics and level design:
![](https://github.com/bM7tcHF88GBxDni/README-GIF-Storage/blob/main/car%202.gif)
![](https://github.com/bM7tcHF88GBxDni/README-GIF-Storage/blob/main/car%203.gif)

Final iteration of box colliders on vehicle and vehicle mechanics, used Blender to 3D model the road and environmental design using open source Kenney assets (https://kenney.itch.io/assetforge):
![](https://github.com/bM7tcHF88GBxDni/README-GIF-Storage/blob/main/car%204.gif)
