using UnityEngine;
using Zenject;

namespace Settings.Installers
{
    [CreateAssetMenu(fileName = "MatchmakingInstaller", menuName = "Installers/MatchmakingInstaller")]
    public class MatchmakingSettingsInstaller : ScriptableObjectInstaller<MatchmakingSettingsInstaller>
    {
        [SerializeField] private RoomSettings roomSettings;
        
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<RoomSettings>().FromInstance(roomSettings).AsSingle();
        }
    }
}