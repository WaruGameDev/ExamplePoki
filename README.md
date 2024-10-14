# Unity Poki SDK Integration Example

This repository contains an example project using **Unity 2022.3.42f1** that demonstrates how to integrate the Poki SDK into a Unity game. The project includes a scene showcasing all available events and functions tied to the Poki SDK, such as starting and stopping gameplay, handling ads, and more. Additionally, the project includes example music that works seamlessly in the Poki inspector environment.

## Features

- **Poki SDK Integration**: Complete setup with all the main events and features of the Poki SDK, such as gameplay start/stop, rewarded ads, and commercial breaks.
- **Event Logging**: A log system that captures key events (e.g., SDK initialization, gameplay state changes) and displays them in a UI element using TextMeshPro.
- **Audio Control**: Example of how to manage game audio based on SDK events, such as muting during ads or pauses.
- **Gameplay Example**: A simple gameplay loop with a rotating object to test and showcase SDK integration.
  
## Project Structure

- **PokiManager.cs**: Manages the integration with Poki SDK. It handles events such as SDK initialization, commercial breaks, rewarded ads, and gameplay start/stop. The script also includes audio and time control during ads to improve user experience.
- **LogManager.cs**: A simple log manager to display SDK events and game actions using a UI text element (TextMeshPro).
- **Dummy.cs**: A script for basic gameplay, rotating an object in the scene, which serves as an example for gameplay start/stop events.
  
## How to Use

1. **Unity Version**: This project uses Unity 2022.3.42f1, so make sure you have this version installed.
2. **Poki SDK Setup**: Follow the Poki SDK integration instructions found on the official [Poki Unity SDK page](https://sdk.poki.com/unity.html). The SDK is already integrated in this example project.
3. **Run the Example**: Open the provided scene to explore all the Poki SDK events. You can simulate gameplay start, gameplay stop, commercial breaks, and rewarded ads.
4. **Inspector Integration**: This project is compatible with Poki's Inspector tool. Ensure that the project is running in the Poki Inspector environment to fully test its functionality.
  
## Poki SDK Events Implemented

- **SDK Initialization**: Automatically initializes when the game starts, logging "Poki sdk is ready" upon success.
- **Gameplay Start/Stop**: Triggered using the buttons in the UI, logging "Gameplay start!!! :D" or "Gameplay stop!!! :c".
- **Commercial Break**: Simulates a commercial break, pausing gameplay and muting audio.
- **Rewarded Ad**: Plays a rewarded ad, optionally granting in-game rewards such as coins.

## Example Music

The project includes an example music track that is managed by the Unity `AudioMixer`. The audio is controlled based on SDK events (e.g., muted during ads or pauses).

## License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.
