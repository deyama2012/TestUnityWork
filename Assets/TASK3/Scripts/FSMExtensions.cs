using AxGrid;
using AxGrid.FSM;

namespace Slots
{
    public static class FSMExtensions
    {
        public static void LogTransition(this FSMState state, bool isEnter)
        {
            Log.Debug($"<i><b>{state.Parent.CurrentStateName}</b></i> <color=#{(isEnter ? "00dd44>ENTER" : "dd9900>EXIT")}</color>");
        }

        public static void Start<T>(this FSM fsm) where T : FSMState
        {
            fsm.Start(typeof(T).Name);
        }

        public static void Change<T>(this FSM fsm) where T : FSMState
        {
            fsm.Change(typeof(T).Name);
        }
    }
}
