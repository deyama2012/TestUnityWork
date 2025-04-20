using AxGrid.FSM;
using AxGrid.Model;

namespace Slots
{
    [State(nameof(ExIdleState))]
    public class ExIdleState : FSMState
    {
        [Enter]
        private void EnterThis()
        {
            this.LogTransition(true);
            Model.Set(Names.START_BUTTON_ENABLE, true);
        }


        [Bind(Names.START_BUTTON_CLICKED)]
        private void OnStartClicked()
        {
            Model.EventManager.Invoke(Names.PARTICLES_STOP_REQUEST);
            Parent.Change<ExScrollingState>();
        }


        [Exit] private void ExitThis() => this.LogTransition(false);
    }
}
