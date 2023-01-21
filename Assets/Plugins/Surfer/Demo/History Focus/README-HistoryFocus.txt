(Game window size should be vertical like 1080x1920)

Because Surfer State names are based on GUID created the moment you import Surfer, the scene must be setup in 6 steps before it can work.

1) MainCamera->SUElementCanvas-> 1)Scene-MySceneLoaded : DO... : If Conditions True : Surfer/OpenState : State : “choose the state you want from the list”


2)Canvas->Menu1->SUElementCanvas : State : “choose the state used in step1 ”
3)Canvas->Menu2->SUElementCanvas : State : “choose the state you want from the list”


4)Canvas->Menu1->Bar->Btn-> SUElementCanvas : 1)UIGeneric-OnClick : DO... : If Conditions True : Surfer/OpenState : State : “choose the state used in step3 ”
5)Canvas->Menu1->Bar->Btn (1)-> SUElementCanvas : 1)UIGeneric-OnClick : DO... : If Conditions True : Surfer/OpenState : State : “choose the state used in step3 ”



Hit play, click one of the buttons and you will see that it becomes blue because, based on the history state focus, it is the last UI object selected of that state. 