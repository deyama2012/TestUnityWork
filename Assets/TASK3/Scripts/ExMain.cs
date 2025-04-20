using AxGrid;
using AxGrid.Base;
using AxGrid.FSM;
using AxGrid.Model;
using UnityEngine;

namespace Slots
{
    public class ExMain : MonoBehaviourExtBind
    {
        [OnStart]
        private void StartThis()
        {
            Settings.Fsm = new FSM();
            Settings.Fsm.Add(new ExIdleState());
            Settings.Fsm.Add(new ExScrollingState());
            Settings.Fsm.Start<ExIdleState>();
        }


        [OnUpdate]
        private void UpdateThis() =>
            Settings.Fsm.Update(Time.deltaTime);


        [Bind(Names.START_BUTTON_CLICKED)]
        private void OnStartClick() =>
            Settings.Fsm.Invoke(Names.START_BUTTON_CLICKED);


        [Bind(Names.STOP_BUTTON_CLICKED)]
        private void OnStopClick() =>
            Settings.Fsm.Invoke(Names.STOP_BUTTON_CLICKED);
    }
}
