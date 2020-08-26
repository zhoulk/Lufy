/*-------------------------------------------------------------------------------
 * 创建者：huangyechuan
 * 修改者列表：
 * 创建日期：2018/11/28
 * 模块描述：游戏手柄服务
 * 
 * ------------------------------------------------------------------------------*/
using BasketBall;
using UnityEngine;

namespace LF.GamepadServer
{
    /// <summary>
    /// 游戏手柄服务
    /// </summary>
    public class ProviderGamepadServer : MonoBehaviour
    {
        GamepadServer server = null;

        private void Awake()
        {
            if (Define.platForm.Equals(PlatForm.TV))
            {
                Init();
            }
        }

        private void Update()
        {
            if(server != null)
            {
                server.Update();
            }
        }

        /// <summary>
        /// 注册服务
        /// </summary>
        public void Register()
        {
            //App.Singleton<GamepadServer>().Alias<IGamepadServer>();
        }

        /// <summary>
        /// 初始化服务
        /// </summary>
        public void Init()
        {
            //App.Make<IGamepadServer>();
            server = new GamepadServer(this);
        }

        private void OnDestroy()
        {
            
        }
    }
}