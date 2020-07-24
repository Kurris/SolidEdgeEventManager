using SolidEdge.Events.Base;
using SolidEdge.Events.EventEnum;
using System;


namespace SolidEdge.Events.Helper
{
    /// <summary>
    /// SolidEdge事件管理类
    /// </summary>
    public sealed class SolidEdgeEventManager : SolidEdgeEventManagerBase
    {
        #region Singleton Implemention

        private SolidEdgeEventManager()
        {
        }

        private static SolidEdgeEventManager _mInstance = null;

        /// <summary>
        /// 实例
        /// </summary>
        public static SolidEdgeEventManager Instance
        {
            get
            {
                if (_mInstance == null)
                {
                    _mInstance = new SolidEdgeEventManager();
                }
                return _mInstance;
            }
        }

        #endregion

        /// <summary>
        /// 添加或者替换事件
        /// </summary>
        /// <param name="Key">唯一键</param>
        /// <param name="EventType">事件枚举类型</param>
        /// <param name="RegisterMethod">事件注册方法</param>
        public void AddOrReplaceEvent(object Key, SEEvent EventType, Action<object[]> RegisterMethod)
        {
            Add(Key, EventType, RegisterMethod, true);
        }

        /// <summary>
        /// 添加或者替换事件
        /// </summary>
        /// <param name="Key">唯一键</param>
        /// <param name="MatchaName">匹配的名称</param>
        /// <param name="EventType">事件枚举类型</param>
        /// <param name="RegisterMethod">事件注册方法</param>
        public void AddOrReplaceEvent(object Key, string MatchaName, SEEvent EventType, Action<object[]> RegisterMethod)
        {
            Add(Key, MatchaName, EventType, RegisterMethod, true);
        }

        /// <summary>
        /// 添加事件
        /// </summary>
        /// <param name="Key">唯一键</param>
        /// <param name="EventType">事件枚举类型</param>
        /// <param name="RegisterMethod">事件注册方法</param>
        public void AddEvent(object Key, SEEvent EventType, Action<object[]> RegisterMethod)
        {
            Add(Key, EventType, RegisterMethod);
        }

        /// <summary>
        /// 添加Occurrence事件
        /// </summary>
        /// <param name="Key">唯一键</param>
        /// <param name="MatchName">匹配名称</param>
        /// <param name="EventType">事件枚举类型</param>
        /// <param name="RegisterMethod">事件注册方法</param>
        public void AddEvent(object Key, string MatchName, SEEvent EventType, Action<object[]> RegisterMethod)
        {
            Add(Key, MatchName, EventType, RegisterMethod);
        }

        /// <summary>
        /// 移除事件
        /// </summary>
        /// <param name="Key">唯一键</param>
        /// <param name="EventType">事件枚举类型</param>
        public void RemoveEvent(object Key, SEEvent EventType)
        {
            Remove(Key, EventType);
        }

        /// <summary>
        /// 移除Occurrence事件
        /// </summary>
        /// <param name="Key">唯一键</param>
        /// <param name="MatchName">匹配的名称</param>
        /// <param name="EventType">事件枚举类型</param>
        public void RemoveEvent(object Key, string MatchName, SEEvent EventType)
        {
            Remove(Key, MatchName, EventType);
        }

        /// <summary>
        /// 移除所有事件
        /// </summary>
        /// <param name="Key">唯一键</param>
        public void RemoveAllEvents(object Key)
        {
            RemoveAll(Key);
        }
    }
}
