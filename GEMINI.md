# Gemini Project: Tic-Tac-Toe

## Project Overview

This is a Tic-Tac-Toe game project developed using the Unity Engine. Based on the project structure and assets, it appears to be a 2D game utilizing Unity's standard UI (UGUI) and the new Input System.

*   **Engine:** Unity `6000.3.6f1`
*   **Main Technologies:**
    *   Unity Engine
    *   Unity UI (UGUI)
    *   Unity Input System
*   **Key Scenes:**
    *   `Assets/Scenes/Main.unity`
    *   `Assets/Scenes/Game.unity`

The project includes sprite assets for the game board, X's, and O's, indicating a visual implementation of the game.

## Building and Running

To work with this project, you need the Unity Hub and the correct Unity Editor version installed.

1.  **Open in Unity:**
    *   Open Unity Hub.
    *   Click "Open" and select the root directory of this project (`tictactoe_proj`).
    *   Unity will automatically detect the project version (`6000.3.6f1`) and open it. If you don't have this version, Unity Hub will prompt you to install it.

2.  **Run the Game:**
    *   In the Unity Editor, navigate to the `Assets/Scenes` folder in the Project window.
    *   Double-click either `Main.unity` or `Game.unity` to open the scene.
    *   Press the **Play** button at the top of the editor to run the game within the Unity environment.

## Development Conventions

*   **Scripting:** The project contains a single C# script at `Assets/Sources/Main.cs`, which is currently empty. This suggests that much of the game's logic might be configured directly in the Unity Editor on scene objects and prefabs rather than being heavily scripted.
*   **File Structure:** Assets are organized into conventional Unity folders (`Sprites`, `Scenes`, `Sources`). New assets should follow this organization.
*   **Input:** The project uses the `InputSystem_Actions.inputactions` asset, meaning user input is handled through Unity's Input System. To modify controls, edit this asset using the Input Actions editor in Unity.
