# Simulation

> AutoCore simulation tool provides test environment for Autoware and still during early development, contents below may changed during updates.

[![GitHub license](https://img.shields.io/github/license/autowarefoundation/autocore_sim.svg)](https://github.com/autowarefoundation/autocore_sim)

## Overview

AutoCore simulation tool is developed based on Unity engine, which focuses help developer test Autoware functions. It could simulate multi type of input topics, so that the autonomous driving system could be tested.

## Requirement

### Simulator PC Requirement

#### Windows

OS: Windows 10
CPU：Intel i5 9100 or higher  
GPU：GTX 750Ti or higher DX11 Support
Memory：>8G  
Free disk：>1GB  

#### Ubuntu

OS: Ubuntu 18.04
CPU：Intel i5 9100 or higher  
GPU：GTX 750Ti or higher Vulkan Support
Memory：>8G  
Free disk：>1GB  

## Quick Start

### Get Released Simulator

* [Windows](https://github.com/autowarefoundation/autocore_sim/releases/download/0.3.3/Simulator_windows_0.3.3.zip)
* [Ubuntu](https://github.com/autowarefoundation/autocore_sim/releases/download/0.3.3/Simulator_ubuntu_0.3.3.zip)

### Guide with Autoware.ai

[How to config autoware.ai 1.14 with simulator](https://github.com/autocore-ai/autocore_pcu_doc/blob/master/docs/Simulation_autoware.md)

### Start Simulator

1. Config Autoware ROS environment, launch Autoware runtime manager and rviz.

2. On windows PC download the simulator archive file, and extract it to a customized destination.

3. Double click `Simulator.exe` to run the simulator, enable all windows firewall dialogs.
  
4. Configure the ROS Master URI and ROS IP, and choose the driving scenario, then click launch.

5. Simulator will enter driving scenario and rviz will show maps.

6. Start `voxel_grid_filter` and `ndt_matching` for localization

7. Enable `vel_pose_connect` without `Simulation Mode`, lidar points will display.

8. Launch `op_global_planner`, drag a available goal in rviz.

9. Launch `op_local_planner`、 `pure_pursuit`、`twist_filter`, Simulator will start driving.  

## Simulator Instruction

There are some buttons on the top of the main interfaces:

1. Car Reset  
   Click to reset the ego car location to the default starting point.

2. Car Pose Set  
   Click to pick a location for ego car, then move the mouse to set the orientation.

3. Add Static Obstacle  
   Click to drop a obstacle at target place.

4. Add Human  
   Click to add a pedestrian at the target place, multiple pedestrians could be added via multiple clicks. Right click to finish. 

5. Add CarAI  
   Click to add a AI car at the target place, then click to add the destination for the AI car. AI car will run in the map according to the destination and follow traffic rules autonomously.

6. Remove All Obstacle  
   Click to remove all obstacles.

7. Settings  
   Click to open setting panel.

8. Exit Simu  
   To exit simulator and go back to the launch panel.

### Other operations

- Left click to select target
- Right click to cancel
- Press the wheel and drag to move camera
- Move the wheel to zoom
- Press Ctrl and move the wheel to change the size of target
- Space: Reset camera
- "W": Move forward
- "S": Move backward
- "A": Left turn
- "D": Right turn
- "X": Brake
- "Ctrl + A": Switch Drive Mode

## Build from source

* Clone source
```
git clone https://github.com/autowarefoundation/autocore_sim.git
```
* Install [UnityHub](https://unity3d.com/get-unity/download) and Unity(2019.3 or above)
* Add source folder to Unity project and Open the project.
* Build to standard alone application by **Ctrl + Shift + B** and build.
* Copy autoware and ros contents unzip to **Simulator_Data/Plugins** folder, they are autoware map loader nodes and ros melodic built on [windows](https://github.com/autowarefoundation/autocore_sim/releases/download/0.3.3/Plugins_windows.zip) and [ubuntu](https://github.com/autowarefoundation/autocore_sim/releases/download/0.3.3/Plugins_ubuntu.zip).
* Copy and unzip autoware [maps](https://github.com/autowarefoundation/autocore_sim/releases/download/0.3.3/city_maps.zip) to **Simulator_Data/StreamingAssets** folder.
* Double click Simulator.exe on windows, or set Simulator.x86_64 **Allow executing file as program** and run it on ubuntu.

## ROS Info
* autocore_sim
   * Publications:  
      * /camera/camera_info [sensor_msgs/CameraInfo]
      * /camera/image_raw [sensor_msgs/Image]
      * /gnss_pose [geometry_msgs/PoseStamped]
      * /points_raw [sensor_msgs/PointCloud2]
      * /tf [tf2_msgs/TFMessage]  
      * /vehicle_status [autoware_msgs/VehicleStatus]  

   * Subscriptions:
      * /vehicle_cmd [autoware_msgs/VehicleCmd]
* vector_map_loader
   * Publications:
      * /vector_map [visualization_msgs/MarkerArray]
      * /vector_map_info/* [vector_map_msgs/*]
* points_map_loader
   * Publications:
      * /points_map [sensor_msgs/PointCloud2]

## Roadmap

* ROS2 topics for Autoware.Auto

## Video tutorial

[![AutoCore Open Source Simulator](https://img.youtube.com/vi/CUFMKZAbgCk/0.jpg)](https://www.youtube.com/watch?v=CUFMKZAbgCk "AutoCore Open Source Simulator")
