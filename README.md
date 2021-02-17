# DrivingAndExploration
Creating vehicle mechanics for a driving and exploration project. This project has been in development since 29 Jan 2021 week. 11 Feb 2021, JetBrains Rider refusing to launch. Project was coming to an end anyway, cancelled in favour of returning to Web Development.

I am experimenting with (basic) physics based gameplay in this project, unlike the Character Controller in my FirstPersonProject which implemented movement by transforming GameObject vectors directly. The exploration aspect of this project is referring to 3D modelling and level design which will begin much later on.

As a physics based project I had to learn basic physics concepts and how they function in Unity: friction, angular drag, angular velocity, centre of mass, box collider mass, angular torque etc.

## Overview of Features
Vehicle mechanics (using Rigidbody):
- Exterior Vehicle Camera
- Vehicle is driven by applying forces to affect it's physics
- Vehicle is turned along Y Axis (Left and Right) by applying torque
- Vehicle can only be driven when front wheels are grounded
- Vehicle has a turning radius
- Vehicle cannot easily turn at very low speeds
- Vehicle has correct/realistic reverse turning
- Centre of mass of vehicle has been posiitoned several float units behind the main object for the driving force to feel more natural.
- Rigidbody properties have to be constantly fine tuned to keep the gameplay tight.

Vehicle's 3D Model was "kit bashed" using Asset Forge 2 by Kenney (https://kenney.itch.io/assetforge).
![](https://github.com/bM7tcHF88GBxDni/README-GIF-Storage/blob/main/car%20453px.gif)
