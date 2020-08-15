using SolidEdge.Application;
using SolidEdge.Events.EventEnum;
using SolidEdgeFramework;
using System;
using System.Collections.Generic;

namespace SolidEdge.Events.Helper
{
    /// <summary>
    /// SolidEdge系统事件帮助类
    /// </summary>
    internal sealed class SolidEdgeSystemEventHelper
    {
        /// <summary>
        /// Application相关事件
        /// </summary>
        /// <param name="ObjectKey">唯一键</param>
        /// <param name="EventType">事件枚举类型</param>
        /// <param name="RegisterAction">事件注册方法</param>
        internal SolidEdgeSystemEventHelper(object ObjectKey, SEEvent EventType, Action<object[]> RegisterAction)
        {
            _mObjKey = ObjectKey;
            _mSEEventType = EventType;
            _mRegisterMethod = RegisterAction;
        }

        /// <summary>
        /// 唯一键
        /// </summary>
        private object _mObjKey = null;

        /// <summary>
        /// 唯一环境对应类型以及方法
        /// </summary>
        private readonly static Dictionary<object, Dictionary<SEEvent, Action<object[]>>> _mDicRelation
            = new Dictionary<object, Dictionary<SEEvent, Action<object[]>>>();

        /// <summary>
        /// 类型--多播方法
        /// </summary>
        private readonly static Dictionary<SEEvent, Action<object[]>> _mDicMethods = new Dictionary<SEEvent, Action<object[]>>(new EnumComparer<SEEvent>());

        /// <summary>
        /// 事件类型
        /// </summary>
        private SEEvent _mSEEventType;

        /// <summary>
        /// SolidEdge系统事件
        /// </summary>
        private static ISEApplicationEvents_Event _applicationEvents;

        /// <summary>
        /// 当前需要注册的方法
        /// </summary>
        private Action<object[]> _mRegisterMethod = null;


        /// <summary>
        /// 注册事件
        /// </summary>
        internal void RegisterEvent()
        {
            if (_applicationEvents == null)
            {
                _applicationEvents = (ISEApplicationEvents_Event)SolidEdgeApplication.Instance.ApplicationEvents;
            }

            //存在当前环境
            if (_mDicRelation.ContainsKey(_mObjKey))
            {
                //存在当前类型
                if (_mDicRelation[_mObjKey].ContainsKey(_mSEEventType))
                {
                    //添加关系  事件类型--方法
                    _mDicRelation[_mObjKey][_mSEEventType] = _mRegisterMethod;
                }
                //不存在
                else
                {
                    _mDicRelation[_mObjKey].Add(_mSEEventType, _mRegisterMethod);
                }
            }
            else
            {
                _mDicRelation.Add(_mObjKey, new Dictionary<SEEvent, Action<object[]>>(new EnumComparer<SEEvent>())
                {
                    [_mSEEventType] = _mRegisterMethod
                });
            }

            if (!_mDicMethods.ContainsKey(_mSEEventType))
            {
                RemoveEvent();
                RegEvent();
            }

            AddMethod();
        }

        /// <summary>
        /// 注销事件
        /// </summary>
        internal void UnregisterEvent()
        {
            if (_applicationEvents == null) return;

            //当前主键(唯一环境)
            if (!_mDicRelation.ContainsKey(_mObjKey)) return;

            //该环境的系统事件类型
            if (!_mDicRelation[_mObjKey].ContainsKey(_mSEEventType)) return;

            //注销方法
            var method = _mDicRelation[_mObjKey][_mSEEventType];

            //多播委托处理,移除方法
            RemoveMethod(method);

            //移除关系
            _mDicRelation[_mObjKey].Remove(_mSEEventType);

            if (_mDicRelation[_mObjKey].Count == 0)
            {
                _mDicRelation.Remove(_mObjKey);
            }
        }

        /// <summary>
        /// 添加当前事件类型和方法
        /// </summary>
        private void AddMethod()
        {
            if (_mDicMethods.ContainsKey(_mSEEventType))
            {
                _mDicMethods[_mSEEventType] += _mRegisterMethod;
            }
            else
            {
                _mDicMethods.Add(_mSEEventType, _mRegisterMethod);
            }
        }

        /// <summary>
        /// 移除当前事件类型的多播委托挂钩的方法
        /// </summary>
        private void RemoveMethod(Action<object[]> Method)
        {
            if (_mDicMethods.ContainsKey(_mSEEventType))
            {
                _mDicMethods[_mSEEventType] -= Method;
            }
        }


        /// <summary>
        /// 注销事件
        /// </summary>
        private void RemoveEvent()
        {
            switch (_mSEEventType)
            {
                case SEEvent.ApplicationAfterActiveDocumentChange:
                    _applicationEvents.AfterActiveDocumentChange -= _applicationEvents_AfterActiveDocumentChange;
                    break;
                case SEEvent.ApplicationAfterCommandRun:
                    _applicationEvents.AfterCommandRun -= _applicationEvents_AfterCommandRun;
                    break;
                case SEEvent.ApplicationAfterDocumentOpen:
                    _applicationEvents.AfterDocumentOpen -= _applicationEvents_AfterDocumentOpen;
                    break;
                case SEEvent.ApplicationAfterDocumentPrint:
                    _applicationEvents.AfterDocumentPrint -= _applicationEvents_AfterDocumentPrint;
                    break;
                case SEEvent.ApplicationAfterDocumentSave:
                    _applicationEvents.AfterDocumentSave -= _applicationEvents_AfterDocumentSave;
                    break;
                case SEEvent.ApplicationAfterEnvironmentActivate:
                    _applicationEvents.AfterEnvironmentActivate -= _applicationEvents_AfterEnvironmentActivate;
                    break;
                case SEEvent.ApplicationAfterNewDocumentOpen:
                    _applicationEvents.AfterNewDocumentOpen -= _applicationEvents_AfterNewDocumentOpen;
                    break;
                case SEEvent.ApplicationAfterNewWindow:
                    _applicationEvents.AfterNewWindow -= _applicationEvents_AfterNewWindow;
                    break;
                case SEEvent.ApplicationAfterWindowActivate:
                    _applicationEvents.AfterWindowActivate -= _applicationEvents_AfterWindowActivate;
                    break;
                case SEEvent.ApplicationBeforeCommandRun:
                    _applicationEvents.BeforeCommandRun -= _applicationEvents_BeforeCommandRun;
                    break;
                case SEEvent.ApplicationBeforeDocumentClose:
                    _applicationEvents.BeforeDocumentClose -= _applicationEvents_BeforeDocumentClose;
                    break;
                case SEEvent.ApplicationBeforeDocumentPrint:
                    _applicationEvents.BeforeDocumentPrint -= _applicationEvents_BeforeDocumentPrint;
                    break;
                case SEEvent.ApplicationBeforeEnvironmentDeactivate:
                    _applicationEvents.BeforeEnvironmentDeactivate -= _applicationEvents_BeforeEnvironmentDeactivate;
                    break;
                case SEEvent.ApplicationBeforeWindowDeactivate:
                    _applicationEvents.BeforeWindowDeactivate -= _applicationEvents_BeforeWindowDeactivate;
                    break;
                case SEEvent.ApplicationBeforeQuit:
                    _applicationEvents.BeforeQuit -= _applicationEvents_BeforeQuit;
                    break;
                case SEEvent.ApplicationBeforeDocumentSave:
                    _applicationEvents.BeforeDocumentSave -= _applicationEvents_BeforeDocumentSave;
                    break;
            }
        }

        /// <summary>
        /// 挂钩事件
        /// </summary>
        private void RegEvent()
        {
            switch (_mSEEventType)
            {
                case SEEvent.ApplicationAfterActiveDocumentChange:
                    _applicationEvents.AfterActiveDocumentChange += _applicationEvents_AfterActiveDocumentChange;
                    break;
                case SEEvent.ApplicationAfterCommandRun:
                    _applicationEvents.AfterCommandRun += _applicationEvents_AfterCommandRun;
                    break;
                case SEEvent.ApplicationAfterDocumentOpen:
                    _applicationEvents.AfterDocumentOpen += _applicationEvents_AfterDocumentOpen;
                    break;
                case SEEvent.ApplicationAfterDocumentPrint:
                    _applicationEvents.AfterDocumentPrint += _applicationEvents_AfterDocumentPrint;
                    break;
                case SEEvent.ApplicationAfterDocumentSave:
                    _applicationEvents.AfterDocumentSave += _applicationEvents_AfterDocumentSave;
                    break;
                case SEEvent.ApplicationAfterEnvironmentActivate:
                    _applicationEvents.AfterEnvironmentActivate += _applicationEvents_AfterEnvironmentActivate;
                    break;
                case SEEvent.ApplicationAfterNewDocumentOpen:
                    _applicationEvents.AfterNewDocumentOpen += _applicationEvents_AfterNewDocumentOpen;
                    break;
                case SEEvent.ApplicationAfterNewWindow:
                    _applicationEvents.AfterNewWindow += _applicationEvents_AfterNewWindow;
                    break;
                case SEEvent.ApplicationAfterWindowActivate:
                    _applicationEvents.AfterWindowActivate += _applicationEvents_AfterWindowActivate;
                    break;
                case SEEvent.ApplicationBeforeCommandRun:
                    _applicationEvents.BeforeCommandRun += _applicationEvents_BeforeCommandRun;
                    break;
                case SEEvent.ApplicationBeforeDocumentClose:
                    _applicationEvents.BeforeDocumentClose += _applicationEvents_BeforeDocumentClose;
                    break;
                case SEEvent.ApplicationBeforeDocumentPrint:
                    _applicationEvents.BeforeDocumentPrint += _applicationEvents_BeforeDocumentPrint;
                    break;
                case SEEvent.ApplicationBeforeEnvironmentDeactivate:
                    _applicationEvents.BeforeEnvironmentDeactivate += _applicationEvents_BeforeEnvironmentDeactivate;
                    break;
                case SEEvent.ApplicationBeforeWindowDeactivate:
                    _applicationEvents.BeforeWindowDeactivate += _applicationEvents_BeforeWindowDeactivate;
                    break;
                case SEEvent.ApplicationBeforeQuit:
                    _applicationEvents.BeforeQuit += _applicationEvents_BeforeQuit;
                    break;
                case SEEvent.ApplicationBeforeDocumentSave:
                    _applicationEvents.BeforeDocumentSave += _applicationEvents_BeforeDocumentSave;
                    break;
            }
        }

        /// <summary>
        ///多播委托方法执行
        /// </summary>
        /// <param name="objects">事件参数</param>
        private void ExecuteMethods(object[] objects)
        {
            if (_mDicMethods.TryGetValue(_mSEEventType, out var Act))
            {
                if (Act == null) return;

                var methods = Act.GetInvocationList();

                if (methods != null && methods.Length > 0)
                {
                    foreach (var method in methods)
                    {
                        (method as Action<object[]>)?.Invoke(objects);
                    }
                }
            }
        }



        #region SolidEdge Event 
        private void _applicationEvents_BeforeDocumentSave(object theDocument)
        {
            ExecuteMethods(new object[] { theDocument });
        }

        private void _applicationEvents_BeforeQuit()
        {
            ExecuteMethods(null);
        }

        private void _applicationEvents_BeforeWindowDeactivate(object theWindow)
        {
            ExecuteMethods(new object[] { theWindow });
        }

        private void _applicationEvents_BeforeEnvironmentDeactivate(object theEnvironment)
        {
            ExecuteMethods(new object[] { theEnvironment });
        }

        private void _applicationEvents_BeforeDocumentPrint(object theDocument, int hDC, ref double ModelToDC, ref int Rect)
        {
            ExecuteMethods(new object[] { theDocument, hDC, ModelToDC, Rect });
        }

        private void _applicationEvents_BeforeDocumentClose(object theDocument)
        {
            ExecuteMethods(new object[] { theDocument });
        }

        private void _applicationEvents_BeforeCommandRun(int theCommandID)
        {
            ExecuteMethods(new object[] { theCommandID });
        }

        private void _applicationEvents_AfterWindowActivate(object theWindow)
        {
            ExecuteMethods(new object[] { theWindow });
        }

        private void _applicationEvents_AfterNewWindow(object theWindow)
        {
            ExecuteMethods(new object[] { theWindow });
        }

        private void _applicationEvents_AfterNewDocumentOpen(object theDocument)
        {
            ExecuteMethods(new object[] { theDocument });
        }

        private void _applicationEvents_AfterEnvironmentActivate(object theEnvironment)
        {
            ExecuteMethods(new object[] { theEnvironment });
        }

        private void _applicationEvents_AfterDocumentSave(object theDocument)
        {
            ExecuteMethods(new object[] { theDocument });
        }

        private void _applicationEvents_AfterDocumentPrint(object theDocument, int hDC, ref double ModelToDC, ref int Rect)
        {
            ExecuteMethods(new object[] { theDocument, hDC, ModelToDC, Rect });
        }

        private void _applicationEvents_AfterDocumentOpen(object theDocument)
        {
            ExecuteMethods(new object[] { theDocument });
        }

        private void _applicationEvents_AfterCommandRun(int theCommandID)
        {
            ExecuteMethods(new object[] { theCommandID });
        }

        private void _applicationEvents_AfterActiveDocumentChange(object theDocument)
        {
            ExecuteMethods(new object[] { theDocument });
        }
        #endregion


    }
}
