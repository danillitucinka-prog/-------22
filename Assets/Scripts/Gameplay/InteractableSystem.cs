using LoxQuest3D.Core;
using LoxQuest3D.Interactables;

namespace LoxQuest3D.Gameplay
{
    public sealed class InteractableSystem
    {
        private readonly InteractableRepository _repo;

        public InteractableSystem(InteractableRepository repo)
        {
            _repo = repo;
        }

        public bool TryGetInteractable(GameState state, out InteractableDefinition interactable)
            => _repo.TryPick(state, out interactable);
    }
}

