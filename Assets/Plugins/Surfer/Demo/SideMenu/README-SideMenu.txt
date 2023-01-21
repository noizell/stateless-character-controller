(Game window size should be vertical like 1080x1920)

Because Surfer State names are based on GUID created the moment you import Surfer, the scene must be setup in 2 steps before it can work.

1)Canvas->SideMenu->SUElementCanvas : State : “choose the state you want from the list”

2)Canvas->SideMenu->Toggle-> SUElementCanvas : 1)UIGeneric-OnClick : DO... : Surfer/OpenState : State : “choose the state used in step1 ”

Hit play, and see the scene in action.