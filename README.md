# DrivingAndExploration
Creating vehicle mechanics for a driving and exploration project. This project has been in development since 29 Jan 2021 week.

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
- Implement correct/realistic reverse turning

