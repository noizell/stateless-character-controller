(Game window size should be vertical like 1080x1920)

Because Surfer State names are based on GUID created the moment you import Surfer, the scene must be setup in 6 steps before it can work.

1)Canvas->Popup->SUElementCanvas : State : “choose the state you want from the list”

2)Canvas->Bar->v1->SUElementCanvas : 1)UIGeneric-OnClick : DO... : Surfer/OpenState : State : “choose the state used in step1 ”
3)Canvas->Bar->v2->SUElementCanvas : 1)UIGeneric-OnClick : DO... : Surfer/OpenState : State : “choose the state used in step1 ”


4)Canvas->Popup->Version1->Bar->Ok-> SUElementCanvas : 1)UIGeneric-OnClick : DO... : Surfer/CloseState : State : “choose the state used in step1 ”

5)Canvas->Popup->Version1->Bar->Cancel-> SUElementCanvas : 1)UIGeneric-OnClick : DO... : Surfer/CloseState : State : “choose the state used in step1 ”

6)Canvas->Popup->Version2->Bar->Ok-> SUElementCanvas : 1)UIGeneric-OnClick : DO... : Surfer/CloseState : State : “choose the state used in step1 ”




Hit play, and see the scene in action.