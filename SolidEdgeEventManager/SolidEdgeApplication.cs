using System.Runtime.InteropServices;

namespace SolidEdge.Application
{
    /// <summary>
    /// 提供窗体使用SolidEdge 对象实例
    /// </summary>
    public class SolidEdgeApplication
    {
        /// <summary>
        /// SolidEdge对象实例
        /// </summary>
        private static SolidEdgeFramework.Application _mApp = null;

        /// <summary>
        /// SolidEdge对象实例
        /// </summary>
        public static SolidEdgeFramework.Application Instance
        {
            get
            {
                if (_mApp == null)
                {
                    _mApp = (SolidEdgeFramework.Application)Marshal.GetActiveObject("SolidEdge.Application");
                }

                return _mApp;
            }
        }
    }
}
