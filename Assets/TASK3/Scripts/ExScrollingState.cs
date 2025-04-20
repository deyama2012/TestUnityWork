using AxGrid;
using AxGrid.FSM;
using AxGrid.Model;

namespace Slots
{
    [State(nameof(ExScrollingState))]
    public class ExScrollingState : FSMState
    {
        private const float DELAY = 3;

        [Enter]
        private void EnterThis()
        {
            this.LogTransition(true);
            Log.Debug($" нопка <b>Stop</b> станет активна через {DELAY} секунды");

            Model.Set(Names.START_BUTTON_ENABLE, false);
            Model.Set(Names.STOP_BUTTON_ENABLE, false);

            Model.EventManager.Invoke(Names.SPINNER_START_REQUEST);
        }


        [One(DELAY)]
        private void EnableStopButton() =>
            Model.Set(Names.STOP_BUTTON_ENABLE, true);


        [Bind(Names.STOP_BUTTON_CLICKED)]
        private void OnStopClicked()
        {
            Log.Debug($" нопка <b>Start</b> станет активна после полной остановки спиннера");

            Model.Set(Names.STOP_BUTTON_ENABLE, false);

            System.Action<Fruit> onSpinnerStopped = fruit =>
            {
                Parent.Change<ExIdleState>();
                Model.EventManager.Invoke(Names.PARTICLES_START_REQUEST, fruit);
            };

            Model.EventManager.Invoke(Names.SPINNER_STOP_REQUEST, onSpinnerStopped);
        }


        [Exit] private void ExitThis() => this.LogTransition(false);
    }
}
