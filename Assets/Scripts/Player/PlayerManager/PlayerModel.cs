using System.Collections.Generic;
using Player.Player;
using QFramework;

namespace Player.PlayerManager
{
    public interface IPlayerModel : IModel
    {
        public PlayerUnit CurrentControlPlayer { get; set; }

        public List<PlayerUnit> OtherPlayer { get; set; }

        public BindableProperty<int> KillCount { get; set; }

        public bool DoCreateNetWorkPlayer { get; set; }
    }

    public class PlayerModel : AbstractModel, IPlayerModel
    {
        public PlayerUnit CurrentControlPlayer { get; set; }
        public List<PlayerUnit> OtherPlayer { get; set; } = new();
        public BindableProperty<int> KillCount { get; set; }
        public bool DoCreateNetWorkPlayer { get; set; }

        protected override void OnInit()
        {
        }
    }
}