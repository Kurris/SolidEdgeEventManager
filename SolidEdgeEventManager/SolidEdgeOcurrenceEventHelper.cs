using SolidEdge.Events.EventEnum;
using System;
using System.Collections.Generic;
using System.Linq;


namespace SolidEdge.Events.Helper
{
    /// <summary>
    /// 自定义Occurrence事件相关存储类
    /// </summary>
    internal sealed class SolidEdgeOcurrenceEventHelper
    {
        /// <summary>
        /// 唯一键对应list存储匹配的名称,事件枚举类型,具体文档帮助类
        /// </summary>
        Dictionary<object, List<List<object>>> _mDicOccurrenceEvent = new Dictionary<object, List<List<object>>>();

        /// <summary>
        /// 添加元素
        /// </summary>
        /// <param name="Key">唯一键</param>
        /// <param name="MatchName">匹配的Occurrence名称</param>
        /// <param name="EventType">事件枚举类型</param>
        /// <param name="Helper">文档事件帮助类</param>
        public void AddElement(object Key, string MatchName, SEEvent EventType, SolidEdgeDocumentEventHelper Helper)
        {
            if (_mDicOccurrenceEvent.TryGetValue(Key, out var Lists))
            {
                Lists.Add(new List<object>() { MatchName, EventType, Helper });
            }
            else
            {
                _mDicOccurrenceEvent.Add(Key, new List<List<object>>() { new List<object>() { MatchName, EventType, Helper } });
            }
        }

        /// <summary>
        /// 移除元素
        /// </summary>
        /// <param name="Key">唯一键</param>
        /// <param name="MatchName">匹配的名称</param>
        /// <param name="EventType">事件枚举类型</param>
        public void RemoveElement(object Key, string MatchName, SEEvent EventType)
        {
            if (_mDicOccurrenceEvent.TryGetValue(Key, out var Lists))
            {
                if (Lists.Count != 0)
                {
                    var list = Lists.Where(x => MatchName.Equals(x[0] + "") && EventType == (SEEvent)x[1]).FirstOrDefault();
                    if (list != null)
                    {
                        _mDicOccurrenceEvent[Key].Remove(list);
                    }
                }
            }
        }

        /// <summary>
        /// 根据唯一键,移除元素
        /// </summary>
        /// <param name="Key">唯一键</param>
        public void RemoveElementByKey(object Key)
        {
            if (_mDicOccurrenceEvent.ContainsKey(Key))
            {
                _mDicOccurrenceEvent.Remove(Key);
            }
        }

        /// <summary>
        /// 判断是否存在元素
        /// </summary>
        /// <param name="Key">唯一键</param>
        /// <param name="MatchName">匹配的名称</param>
        /// <param name="EventType">事件枚举类型</param>
        /// <param name="Helper">事件帮助类</param>
        /// <returns>是否存在</returns>
        public bool ContainsKey(object Key, string MatchName, SEEvent EventType, out SolidEdgeDocumentEventHelper Helper)
        {
            Helper = null;

            if (_mDicOccurrenceEvent.TryGetValue(Key, out var Lists))
            {
                if (Lists.Count == 0) return false;

                var list = Lists.Where(x => MatchName.Equals(x[0] + "") && EventType == (SEEvent)x[1]).FirstOrDefault();

                if (list == null) return false;

                if (list.Count < 3)
                    throw new Exception("元素不匹配");
                else
                    Helper = (SolidEdgeDocumentEventHelper)list[2];

                return true;
            }
            return false;
        }

        /// <summary>
        /// 索引器,根据唯一键,匹配名称,事件枚举类型获取事件帮助类
        /// </summary>
        /// <param name="Key">唯一键</param>
        /// <param name="MatchName">匹配的名称</param>
        /// <param name="EventType">事件枚举类型</param>
        /// <returns><see cref="SEEventMethodHelper"/></returns>
        public SolidEdgeDocumentEventHelper this[object Key, string MatchName, SEEvent EventType]
        {
            get
            {
                if (ContainsKey(Key, MatchName, EventType, out var Helper))
                {
                    return Helper;
                }
                return null;
            }
            set
            {
                if (ContainsKey(Key, MatchName, EventType, out var Helper))
                {
                    Helper = value;
                }
            }
        }

        /// <summary>
        /// 索引器,根据唯一键或者所有事件帮助类
        /// </summary>
        /// <param name="Key">唯一键</param>
        /// <returns><see cref="List{SEEventMethodHelper}"/></returns>
        public List<SolidEdgeDocumentEventHelper> this[object Key]
        {
            get
            {
                if (_mDicOccurrenceEvent.Count == 0) return null;

                if (_mDicOccurrenceEvent.TryGetValue(Key, out var Lists))
                {
                    return Lists.Select(x => (SolidEdgeDocumentEventHelper)x[2])?.ToList();
                }
                return null;
            }
        }

    }
}
