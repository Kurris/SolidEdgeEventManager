using SolidEdge.Application;
using SolidEdge.Events.EventEnum;
using SolidEdge.Events.Helper;
using SolidEdgeAssembly;
using SolidEdgeFramework;
using System;
using System.Collections.Generic;

/* 功能 : 事件管理基类
 * 
 * 
 * 修改时间                               修改人                                 修改内容
 * 20200706                              ligy                                  create
 * 20200707                              ligy                                  修改为abstract
 * 20200707                              ligy                                  增加对系统事件支持
 * 20200708                              ligy                                  增加对Occurrence事件添加
 * 
 ***************************************************************************************************************/

namespace SolidEdge.Events.Base
{
    public abstract class SolidEdgeEventManagerBase
    {
        /// <summary>
        /// Document相关
        /// </summary>
        private readonly Dictionary<object, Dictionary<SEEvent, SolidEdgeDocumentEventHelper>> _mDicHelperOfDoc
            = new Dictionary<object, Dictionary<SEEvent, SolidEdgeDocumentEventHelper>>();

        /// <summary>
        /// Application相关
        /// </summary>
        private readonly Dictionary<object, Dictionary<SEEvent, SolidEdgeSystenEventHelper>> _mDicHelprOfApp
            = new Dictionary<object, Dictionary<SEEvent, SolidEdgeSystenEventHelper>>();


        /// <summary>
        /// Occurrence事件相关
        /// </summary>
        private readonly SolidEdgeOcurrenceEventHelper _mOccurrenceEventStorage = new SolidEdgeOcurrenceEventHelper();


        /// <summary>
        /// 获取当前SolidEdge活动文档
        /// <para>
        /// 注:如果传入ASM文档名称,则寻找当前活动文档下的Occurrence为当前活动文档
        /// </para>
        /// </summary>
        /// <param name="ASMDocumentName">ASM文档名称</param>
        /// <returns></returns>
        private SolidEdgeDocument GetActiveObjectDocument(string ASMDocumentName = null)
        {
            if (SolidEdgeApplication.Instance == null) throw new NullReferenceException("SolidEdgew程序对象获取失败!");

            object objDoc = SolidEdgeApplication.Instance.ActiveDocument;

            if (objDoc == null) throw new NullReferenceException("SolidEdge程序当前活动文档不存在!");

            //查找Occurrence
            if (!string.IsNullOrEmpty(ASMDocumentName))
            {
                AssemblyDocument ObjAsm = (AssemblyDocument)objDoc;

                if (ObjAsm.Name.Equals(ASMDocumentName, StringComparison.OrdinalIgnoreCase))
                {
                    return (SolidEdgeDocument)objDoc;
                }

                //工作站环境
                Occurrences occurrences = ObjAsm.Occurrences;
                var tor = occurrences.GetEnumerator();

                while (tor.MoveNext())
                {
                    Occurrence occurrence = (Occurrence)tor.Current;
                    if (ASMDocumentName.Equals(occurrence.Name, StringComparison.OrdinalIgnoreCase))
                    {
                        return (SolidEdgeDocument)occurrence.OccurrenceDocument;
                    }
                }
                throw new NullReferenceException($"SolidEdge程序当前活动文档{ASMDocumentName}不存在!");
            }
            //直接返回
            else
            {
                return (SolidEdgeDocument)objDoc;
            }
        }

        /// <summary>
        /// 锁
        /// </summary>
        private static readonly object _mLock = new object();


        /// <summary>
        /// 添加事件
        /// </summary>
        /// <param name="key">唯一键(如果Key是字符串类型,则寻找当前ASM下面匹配的ASM文档)</param>
        /// <param name="EventType">事件枚举值</param>
        /// <param name="RegisterMethod">事件执行方法</param>
        /// <param name="NeedRemove">是否需要移除已存在事件</param>
        internal void Add(object Key, SEEvent EventType, Action<object[]> RegisterMethod, bool NeedRemove = false)
        {
            lock (_mLock)
            {
                //系统事件
                if (CheckSolidEdgeApplicationEvent(Key, EventType, RegisterMethod, true, NeedRemove)) return;

                //文档事件
                if (NeedRemove)
                {
                    Remove(Key, EventType);
                }

                SolidEdgeDocumentEventHelper Helper = new SolidEdgeDocumentEventHelper(EventType, RegisterMethod);

                if (!_mDicHelperOfDoc.ContainsKey(Key))
                {
                    _mDicHelperOfDoc.Add(Key, new Dictionary<SEEvent, SolidEdgeDocumentEventHelper>()
                    {
                        [EventType] = Helper
                    });
                }
                else
                {
                    if (!_mDicHelperOfDoc[Key].ContainsKey(EventType))
                    {
                        _mDicHelperOfDoc[Key].Add(EventType, Helper);
                    }
                    else
                    {
                        throw new ArgumentException("重复添加事件:" + EventType.ToString());
                    }
                }

                Helper.RegisterSolidEdgeEvent(GetActiveObjectDocument());
            }
        }

        /// <summary>
        /// 添加事件
        /// </summary>
        /// <param name="Key">唯一键</param>
        /// <param name="MatchName">匹配的名称</param>
        /// <param name="EventType">事件枚举类型</param>
        /// <param name="RegisterMethod">事件注册的方法</param>
        /// <param name="NeedRemove">是否先移除</param>
        internal void Add(object Key, string MatchName, SEEvent EventType, Action<object[]> RegisterMethod, bool NeedRemove = false)
        {
            lock (_mLock)
            {
                if (NeedRemove)
                {
                    Remove(Key, MatchName, EventType);
                }

                if (!_mOccurrenceEventStorage.ContainsKey(Key, MatchName, EventType, out SolidEdgeDocumentEventHelper Helper))
                {
                    _mOccurrenceEventStorage.AddElement(Key, MatchName, EventType, Helper = new SolidEdgeDocumentEventHelper(EventType, RegisterMethod));
                }
                else
                {
                    throw new ArgumentException("重复添加事件:" + EventType.ToString());
                }
                Helper.RegisterSolidEdgeEvent(GetActiveObjectDocument(MatchName));
            }
        }

        /// <summary>
        /// 移除事件
        /// </summary>
        /// <param name="Key">唯一键</param>
        /// <param name="MatchName">匹配的名称</param>
        /// <param name="EventType">事件枚举类型</param>
        internal void Remove(object Key, string MatchName, SEEvent EventType)
        {
            lock (_mLock)
            {
                if (_mOccurrenceEventStorage.ContainsKey(Key, MatchName, EventType, out var Helper))
                {
                    Helper.UnregisterSolidEdgeEvent();

                    _mOccurrenceEventStorage.RemoveElement(Key, MatchName, EventType);
                }
            }
        }

        /// <summary>
        /// 移除事件
        /// </summary>
        /// <param name="Key">唯一键</param>
        /// <param name="EventType">事件枚举值</param>
        internal void Remove(object Key, SEEvent EventType)
        {
            lock (_mLock)
            {
                //系统事件
                if (CheckSolidEdgeApplicationEvent(Key, EventType)) return;

                //文档事件
                if (_mDicHelperOfDoc.TryGetValue(Key, out var CurrentDic))
                {
                    if (CurrentDic.TryGetValue(EventType, out var Helper))
                    {
                        Helper.UnregisterSolidEdgeEvent();

                        CurrentDic.Remove(EventType);
                    }
                }
            }
        }

        /// <summary>
        /// 移除所有事件
        /// </summary>
        /// <param name="TKey">唯一键</param>
        internal void RemoveAll(object Key)
        {
            lock (_mLock)
            {
                //系统事件
                if (_mDicHelprOfApp.ContainsKey(Key))
                {
                    var Helpers = _mDicHelprOfApp[Key];

                    foreach (var item in Helpers)
                    {
                        item.Value.UnregisterEvent();
                    }
                    _mDicHelprOfApp.Remove(Key);
                }

                //文档事件
                if (_mDicHelperOfDoc.ContainsKey(Key))
                {
                    var Helpers = _mDicHelperOfDoc[Key];

                    foreach (var item in Helpers)
                    {
                        item.Value.UnregisterSolidEdgeEvent();
                    }
                    _mDicHelperOfDoc.Remove(Key);
                }

                //Occurrence事件
                {
                    var Helpers = _mOccurrenceEventStorage[Key];
                    if (Helpers != null)
                    {
                        foreach (var item in Helpers)
                        {
                            item.UnregisterSolidEdgeEvent();
                        }
                    }
                    _mOccurrenceEventStorage.RemoveElementByKey(Key);
                }
            }
        }

        /// <summary>
        /// 检查是否为系统事件,并且添加/替换或者移除事件
        /// </summary>
        /// <param name="ObjectKey">唯一键</param>
        /// <param name="EventType">事件枚举类型</param>
        /// <param name="RegisterMethod">事件注册方法</param>
        /// <param name="AddOrRemove">添加或者移除</param>
        /// <param name="Replace">如果存在是否替换</param>
        /// <returns></returns>
        private bool CheckSolidEdgeApplicationEvent(object ObjectKey, SEEvent EventType
            , Action<object[]> RegisterMethod = null
            , bool AddOrRemove = false, bool Replace = false)
        {
            switch (EventType)
            {
                case SEEvent.ApplicationAfterActiveDocumentChange:
                case SEEvent.ApplicationAfterCommandRun:
                case SEEvent.ApplicationAfterDocumentOpen:
                case SEEvent.ApplicationAfterDocumentPrint:
                case SEEvent.ApplicationAfterDocumentSave:
                case SEEvent.ApplicationAfterEnvironmentActivate:
                case SEEvent.ApplicationAfterNewDocumentOpen:
                case SEEvent.ApplicationAfterNewWindow:
                case SEEvent.ApplicationAfterWindowActivate:
                case SEEvent.ApplicationBeforeCommandRun:
                case SEEvent.ApplicationBeforeDocumentClose:
                case SEEvent.ApplicationBeforeDocumentPrint:
                case SEEvent.ApplicationBeforeEnvironmentDeactivate:
                case SEEvent.ApplicationBeforeWindowDeactivate:
                case SEEvent.ApplicationBeforeQuit:
                case SEEvent.ApplicationBeforeDocumentSave:

                    //添加
                    if (AddOrRemove)
                    {
                        SolidEdgeSystenEventHelper Helper = new SolidEdgeSystenEventHelper(ObjectKey, EventType, RegisterMethod);
                        //不存在
                        if (!_mDicHelprOfApp.ContainsKey(ObjectKey))
                        {
                            //添加
                            _mDicHelprOfApp.Add(ObjectKey, new Dictionary<SEEvent, SolidEdgeSystenEventHelper>()
                            {
                                [EventType] = Helper
                            });
                        }
                        //存在主键
                        else
                        {
                            //不存在该事件类型
                            if (!_mDicHelprOfApp[ObjectKey].ContainsKey(EventType))
                            {
                                //添加
                                _mDicHelprOfApp[ObjectKey].Add(EventType, Helper);
                            }
                            //已存在
                            else
                            {
                                //需要替换
                                if (Replace)
                                {
                                    //先注销事件
                                    _mDicHelprOfApp[ObjectKey][EventType].UnregisterEvent();
                                    _mDicHelprOfApp[ObjectKey].Remove(EventType);

                                    //添加
                                    _mDicHelprOfApp[ObjectKey].Add(EventType, Helper);
                                }
                                //提示重复
                                else
                                {
                                    throw new ArgumentException("重复添加事件:" + EventType.ToString());
                                }
                            }
                        }
                        //最后统一注册事件
                        Helper.RegisterEvent();
                    }
                    //移除
                    else
                    {
                        if (_mDicHelprOfApp.ContainsKey(ObjectKey))
                        {
                            if (_mDicHelprOfApp[ObjectKey].ContainsKey(EventType))
                            {
                                _mDicHelprOfApp[ObjectKey][EventType].UnregisterEvent();
                            }
                        }
                    }

                    return true;
            }
            return false;
        }
    }
}
