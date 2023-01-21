using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Surfer
{

    public partial class SUElement
    {


        void CheckGroups()
        {

            if (!_elementData.IsGroup)
                return;

            var states = _elementData.GroupStates.AllNames;

            for (int i = 0; i < states.Count; i++)
            {
                var child = GetGroupChild(i);

                if (child == null)
                    continue;

                if (_elementData.IsGroupStates)
                    SetUpGroupState(child, states[i]);
                else if (_elementData.IsGroupButtons)
                    SetUpGroupButtons(child, states[i]);

            }

        }


        void SetUpGroupState(Transform child, string state)
        {
            SUElementCanvas cp = child.gameObject.AddComponent<SUElementCanvas>();
            cp.InjectStateElementData(state, _elementData.PlayerID, SUElementData.StateCloseMode_ID.Siblings);

            SUBehavioursData bhvsEnter = new SUBehavioursData();
            SUBehavioursData bhvsExit = new SUBehavioursData();
            List<SUReactionData> reacsEnter = new List<SUReactionData>();
            List<SUReactionData> reacsExit = new List<SUReactionData>();

            if (_elementData.Type == SUElementData.Type_ID.GroupStates_InOut
            || _elementData.Type == SUElementData.Type_ID.GroupStates_OnOffAndInOut)
            {

                var cpRect = child.GetComponent<RectTransform>();

                //enter
                reacsEnter.Add(new SUReactionData(DefaultCustomReactions.kSetAnchoredPosition)
                {
                    FieldsValues = new SUFieldValuesData()
                    {
                        Object_1 = cpRect,
                        Vector3_1 = Vector3.zero
                    }
                });

                //exit
                reacsExit.Add(new SUReactionData(DefaultCustomReactions.kSetAnchoredPosition)
                {
                    FieldsValues = new SUFieldValuesData()
                    {
                        Object_1 = cpRect,
                        Vector3_1 = SurferHelper.OutPos
                    }
                });

            }

            if (_elementData.Type == SUElementData.Type_ID.GroupStates_OnOff
            || _elementData.Type == SUElementData.Type_ID.GroupStates_OnOffAndInOut)
            {
                //enter
                reacsEnter.Add(new SUReactionData(DefaultCustomReactions.kGOEnable)
                {
                    FieldsValues = new SUFieldValuesData()
                    {
                        Object_1 = child.gameObject
                    }
                });

                //exit
                reacsExit.Add(new SUReactionData(DefaultCustomReactions.kGODisable)
                {
                    FieldsValues = new SUFieldValuesData()
                    {
                        Object_1 = child.gameObject
                    }
                });

            }

            foreach (var enter in reacsEnter)
                bhvsEnter.AddCustomReaction(enter, SUEvent.Type_ID.State_MyStateEnter);
            foreach (var exit in reacsExit)
                bhvsExit.AddCustomReaction(exit, SUEvent.Type_ID.State_MyStateExit);

            cp.Events.Add(SUEvent.Type_ID.State_MyStateEnter, bhvsEnter);
            cp.Events.Add(SUEvent.Type_ID.State_MyStateExit, bhvsExit);
            cp.Initialize();
        }


        void SetUpGroupButtons(Transform child, string state)
        {

            SUElementCanvas cp = child.gameObject.AddComponent<SUElementCanvas>();
            cp.InjectNormalElementData();

            SUBehavioursData bhvsOpen = new SUBehavioursData();
            SUReactionData openReaction = new SUReactionData(DefaultCustomReactions.kOpenState);
            openReaction.StateData = new SUStateData(state);
            openReaction.FieldsValues = new SUFieldValuesData()
            {
                Int_1 = 0,
                Int_2 = _elementData.PlayerID,
                Float_1 = 0,
                Object_1 = null
            };

            bhvsOpen.AddCustomReaction(openReaction, SUEvent.Type_ID.UIGeneric_OnClick);
            cp.Events.Add(SUEvent.Type_ID.UIGeneric_OnClick, bhvsOpen);
            cp.Initialize();


        }



        Transform GetGroupChild(int rightIndex)
        {


            if (rightIndex < 0)
                return null;

            int total = ElementData.GroupStates.List.Count - 1;

            if (total <= 0)
                return null;

            int totToSkip = ElementData.IntVal;
            rightIndex += totToSkip;


            if (rightIndex >= transform.childCount)
                return null;



            return transform.GetChild(rightIndex);

        }


    }

}
