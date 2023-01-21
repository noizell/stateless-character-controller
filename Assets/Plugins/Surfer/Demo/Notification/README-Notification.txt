(Game window size should be vertical like 1080x1920)

Because Surfer State names are based on GUID created the moment you import Surfer, the scene must be setup in 2 steps before it can work.

1) MainCamera->SUElementCanvas-> 1)Scene-MySceneLoaded : DO... : Surfer/OpenState (All entries) : State : “choose the state you want from the list”

2)Canvas->Panel-> SUElementCanvas : State : “choose the state used in step 1”


Hit play, and see the scene in action.