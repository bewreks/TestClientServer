using UnityEngine;
using Zenject;

namespace Settings.Installers
{
    [CreateAssetMenu(fileName = "MatchmakingInstaller", menuName = "Installers/MatchmakingInstaller")]
    public class MatchmakingSettingsInstaller : ScriptableObjectInstaller<MatchmakingSettingsInstaller>
    {
        [SerializeField] private MatchmakingSettings matchmakingSettings;
        
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<MatchmakingSettings>().FromInstance(matchmakingSettings).AsSingle();
        }
    }
}