(Game window size should be vertical like 1080x1920)

Because Surfer State names are based on GUID created the moment you import Surfer, the scene must be setup in 7 steps before it can work.

1)Canvas->Panel1-> SUElementCanvas : State : “choose the state you want from the list”
2)Canvas->Panel2-> SUElementCanvas : State : “choose the state you want from the list”
3)Canvas->Panel3-> SUElementCanvas : State : “choose the state you want from the list”

4)Canvas->Bar->1-> SUElementCanvas : 1)UIGeneric-OnEnter : DO... : Surfer/OpenState : State : “choose the state used in step1 ”
5)Canvas->Bar->2-> SUElementCanvas : 1)UIGeneric-OnEnter : DO... : Surfer/OpenState : State : “choose the state used in step2 ”
6)Canvas->Bar->3-> SUElementCanvas : 1)UIGeneric-OnEnter : DO... : Surfer/OpenState : State : “choose the state used in step3 ”

7) MainCamera->SUElementCanvas-> 1)Scene-MySceneLoaded : DO... : Surfer/OpenState : State : “choose the state used in step1 ”


Hit play, and see the scene in action.