using System;

namespace LoxQuest3D.Core
{
    public sealed class RunContext
    {
        public GameState State { get; private set; }
        public event Action<GameState> OnStateChanged;

        public RunContext(GameState initial)
        {
            State = initial;
        }

        public void Apply(Action<GameState> mutator)
        {
            mutator(State);
            OnStateChanged?.Invoke(State);
        }
    }
}

