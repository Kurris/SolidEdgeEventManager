using SolidEdge.Events.EventEnum;
using SolidEdgeFramework;
using System;

namespace SolidEdge.Events.Helper
{
    /// <summary>
    /// SolidEdge事件方法帮助类
    /// </summary>
    internal class SolidEdgeDocumentEventHelper
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="EvenType">事件枚举</param>
        /// <param name="RegisterMethod">事件方法</param>
        internal SolidEdgeDocumentEventHelper(SEEvent EvenType, Action<object[]> RegisterMethod)
        {
            _mSEEventType = EvenType;
            _mRegisterMethod = RegisterMethod;
        }

        /// <summary>
        /// 当前执行事件的枚举
        /// </summary>
        public SEEvent _mSEEventType { get; }

        /// <summary>
        /// 事件执行的方法
        /// </summary>
        private Action<object[]> _mRegisterMethod = null;

        private ISEDocumentEvents_Event _documentEvents;

        #region Assembly document events

        private ISEAssemblyChangeEvents_Event _assemblyChangeEvents;
        private ISEAssemblyFamilyEvents_Event _assemblyFamilyEvents;
        private ISEAssemblyRecomputeEvents_Event _assemblyRecomputeEvents;

        #endregion

        #region Draft document events

        private ISEBlockTableEvents_Event _blockTableEvents;
        private ISEConnectorTableEvents_Event _connectorTableEvents;
        private ISEDraftBendTableEvents_Event _draftBendTableEvents;
        private ISEDrawingViewEvents_Event _drawingViewEvents;
        private ISEPartsListEvents_Event _partsListEvents;

        #endregion

        #region Part \ SheetMetal document events

        private ISEDividePartEvents_Event _dividePartEvents;
        private ISEFamilyOfPartsEvents_Event _familyOfPartsEvents;
        private ISEFamilyOfPartsExEvents_Event _familyOfPartsExEvents;

        #endregion


        /// <summary>
        /// 初始化字段
        /// </summary>
        /// <param name="Document">当前活动文档</param>
        private void InitializeFields(SolidEdgeDocument Document)
        {

            if (_mSEEventType == SEEvent.DocumentAfterSave
                || _mSEEventType == SEEvent.DocumentBeforeClose
                || _mSEEventType == SEEvent.DocumentBeforeSave
                || _mSEEventType == SEEvent.DocumentSelectSetChanged)
            {
                _documentEvents = (ISEDocumentEvents_Event)Document.DocumentEvents;
                return;
            }

            switch (Document.Type)
            {
                case DocumentTypeConstants.igAssemblyDocument:

                    var assemblyDocument = (SolidEdgeAssembly.AssemblyDocument)Document;

                    if (_mSEEventType == SEEvent.AssemblyAfterChange
                        || _mSEEventType == SEEvent.AssemblyBeforeChange)
                        _assemblyChangeEvents = (ISEAssemblyChangeEvents_Event)assemblyDocument.AssemblyChangeEvents;

                    else if (_mSEEventType == SEEvent.AssemblyFamilyAfterMemberActivate
                        || _mSEEventType == SEEvent.AssemblyFamilyAfterMemberCreate
                        || _mSEEventType == SEEvent.AssemblyFamilyAfterMemberDelete
                        || _mSEEventType == SEEvent.AssemblyFamilyBeforeMemberActivate
                        || _mSEEventType == SEEvent.AssemblyFamilyBeforeMemberCreate
                        || _mSEEventType == SEEvent.AssemblyFamilyBeforeMemberDelete)
                        _assemblyFamilyEvents = (ISEAssemblyFamilyEvents_Event)assemblyDocument.AssemblyFamilyEvents;

                    else if (_mSEEventType == SEEvent.AssemblyRecomputeAfterAdd
                        || _mSEEventType == SEEvent.AssemblyRecomputeAfterModify
                        || _mSEEventType == SEEvent.AssemblyRecomputeAfterRecompute
                        || _mSEEventType == SEEvent.AssemblyRecomputeBeforeDelete
                        || _mSEEventType == SEEvent.AssemblyRecomputeBeforeRecompute)
                        _assemblyRecomputeEvents = (ISEAssemblyRecomputeEvents_Event)assemblyDocument.AssemblyRecomputeEvents;

                    break;
                case DocumentTypeConstants.igDraftDocument:
                    var draftDocument = (SolidEdgeDraft.DraftDocument)Document;

                    if (_mSEEventType == SEEvent.BlockTableAfterUpdate)
                        _blockTableEvents = (ISEBlockTableEvents_Event)draftDocument.BlockTableEvents;

                    else if (_mSEEventType == SEEvent.ConnectorTableAfterUpdate)
                        _connectorTableEvents = (ISEConnectorTableEvents_Event)draftDocument.ConnectorTableEvents;

                    else if (_mSEEventType == SEEvent.DraftBendTableAfterUpdate)
                        _draftBendTableEvents = (ISEDraftBendTableEvents_Event)draftDocument.DraftBendTableEvents;

                    else if (_mSEEventType == SEEvent.DrawingViewAfterUpdate)
                        _drawingViewEvents = (ISEDrawingViewEvents_Event)draftDocument.DrawingViewEvents;

                    else if (_mSEEventType == SEEvent.PartsListAfterUpdate)
                        _partsListEvents = (ISEPartsListEvents_Event)draftDocument.PartsListEvents;

                    break;
                case DocumentTypeConstants.igPartDocument:
                    var partDocument = (SolidEdgePart.PartDocument)Document;

                    if (_mSEEventType == SEEvent.DividePartAfterDividePartDocumentCreated
                        || _mSEEventType == SEEvent.DividePartAfterDividePartDocumentRenamed
                        || _mSEEventType == SEEvent.DividePartBeforeDividePartDocumentDeleted)
                        _dividePartEvents = (ISEDividePartEvents_Event)partDocument.DividePartEvents;

                    else if (_mSEEventType == SEEvent.FamilyOfPartsAfterMemberDocumentCreated
                        || _mSEEventType == SEEvent.FamilyOfPartsAfterMemberDocumentRenamed
                        || _mSEEventType == SEEvent.FamilyOfPartsBeforeMemberDocumentDeleted)
                        _familyOfPartsEvents = (ISEFamilyOfPartsEvents_Event)partDocument.FamilyOfPartsEvents;

                    else if (_mSEEventType == SEEvent.FamilyOfPartsExAfterMemberDocumentCreated
                        || _mSEEventType == SEEvent.FamilyOfPartsExAfterMemberDocumentRenamed
                        || _mSEEventType == SEEvent.FamilyOfPartsExBeforeMemberDocumentDeleted)
                        _familyOfPartsExEvents = (ISEFamilyOfPartsExEvents_Event)partDocument.FamilyOfPartsExEvents;

                    break;
                case DocumentTypeConstants.igSheetMetalDocument:
                    var sheetMetalDocument = (SolidEdgePart.SheetMetalDocument)Document;

                    if (_mSEEventType == SEEvent.DividePartAfterDividePartDocumentCreated
                        || _mSEEventType == SEEvent.DividePartAfterDividePartDocumentRenamed
                        || _mSEEventType == SEEvent.DividePartBeforeDividePartDocumentDeleted)
                        _dividePartEvents = (ISEDividePartEvents_Event)sheetMetalDocument.DividePartEvents;

                    else if (_mSEEventType == SEEvent.FamilyOfPartsAfterMemberDocumentCreated
                        || _mSEEventType == SEEvent.FamilyOfPartsAfterMemberDocumentRenamed
                        || _mSEEventType == SEEvent.FamilyOfPartsBeforeMemberDocumentDeleted)
                        _familyOfPartsEvents = (ISEFamilyOfPartsEvents_Event)sheetMetalDocument.FamilyOfPartsEvents;

                    else if (_mSEEventType == SEEvent.FamilyOfPartsExAfterMemberDocumentCreated
                        || _mSEEventType == SEEvent.FamilyOfPartsExAfterMemberDocumentRenamed
                        || _mSEEventType == SEEvent.FamilyOfPartsExBeforeMemberDocumentDeleted)
                        _familyOfPartsExEvents = (ISEFamilyOfPartsExEvents_Event)sheetMetalDocument.FamilyOfPartsExEvents;

                    break;
            }
        }

        /// <summary>
        /// 注册事件
        /// </summary>
        internal void RegisterSolidEdgeEvent(SolidEdgeDocument Document)
        {
            InitializeFields(Document);

            if (_documentEvents != null)
            {
                if (_mSEEventType == SEEvent.DocumentAfterSave)
                    _documentEvents.AfterSave += _documentEvents_AfterSave;
                else if (_mSEEventType == SEEvent.DocumentBeforeClose)
                    _documentEvents.BeforeClose += _documentEvents_BeforeClose;
                else if (_mSEEventType == SEEvent.DocumentBeforeSave)
                    _documentEvents.BeforeSave += _documentEvents_BeforeSave;
                else if (_mSEEventType == SEEvent.DocumentSelectSetChanged)
                    _documentEvents.SelectSetChanged += _documentEvents_SelectSetChanged;

                return;
            }

            if (_assemblyChangeEvents != null)
            {
                if (_mSEEventType == SEEvent.AssemblyAfterChange)
                    _assemblyChangeEvents.AfterChange += _assemblyChangeEvents_AfterChange;
                else if (_mSEEventType == SEEvent.AssemblyBeforeChange)
                    _assemblyChangeEvents.BeforeChange += _assemblyChangeEvents_BeforeChange;

                return;
            }

            if (_assemblyFamilyEvents != null)
            {
                if (_mSEEventType == SEEvent.AssemblyFamilyAfterMemberActivate)
                    _assemblyFamilyEvents.AfterMemberActivate += _assemblyFamilyEvents_AfterMemberActivate;
                else if (_mSEEventType == SEEvent.AssemblyFamilyAfterMemberCreate)
                    _assemblyFamilyEvents.AfterMemberCreate += _assemblyFamilyEvents_AfterMemberCreate;
                else if (_mSEEventType == SEEvent.AssemblyFamilyAfterMemberDelete)
                    _assemblyFamilyEvents.AfterMemberDelete += _assemblyFamilyEvents_AfterMemberDelete;
                else if (_mSEEventType == SEEvent.AssemblyFamilyBeforeMemberActivate)
                    _assemblyFamilyEvents.BeforeMemberActivate += _assemblyFamilyEvents_BeforeMemberActivate;
                else if (_mSEEventType == SEEvent.AssemblyFamilyBeforeMemberCreate)
                    _assemblyFamilyEvents.BeforeMemberCreate += _assemblyFamilyEvents_BeforeMemberCreate;
                else if (_mSEEventType == SEEvent.AssemblyFamilyBeforeMemberDelete)
                    _assemblyFamilyEvents.BeforeMemberDelete += _assemblyFamilyEvents_BeforeMemberDelete;

                return;
            }

            if (_assemblyRecomputeEvents != null)
            {
                if (_mSEEventType == SEEvent.AssemblyRecomputeAfterAdd)
                    _assemblyRecomputeEvents.AfterAdd += _assemblyRecomputeEvents_AfterAdd;
                else if (_mSEEventType == SEEvent.AssemblyRecomputeAfterModify)
                    _assemblyRecomputeEvents.AfterModify += _assemblyRecomputeEvents_AfterModify;
                else if (_mSEEventType == SEEvent.AssemblyRecomputeAfterRecompute)
                    _assemblyRecomputeEvents.AfterRecompute += _assemblyRecomputeEvents_AfterRecompute;
                else if (_mSEEventType == SEEvent.AssemblyRecomputeBeforeDelete)
                    _assemblyRecomputeEvents.BeforeDelete += _assemblyRecomputeEvents_BeforeDelete;
                else if (_mSEEventType == SEEvent.AssemblyRecomputeBeforeRecompute)
                    _assemblyRecomputeEvents.BeforeRecompute += _assemblyRecomputeEvents_BeforeRecompute;

                return;
            }

            if (_blockTableEvents != null)
            {
                if (_mSEEventType == SEEvent.BlockTableAfterUpdate)
                    _blockTableEvents.AfterUpdate += _blockTableEvents_AfterUpdate;

                return;
            }

            if (_connectorTableEvents != null)
            {
                if (_mSEEventType == SEEvent.ConnectorTableAfterUpdate)
                    _connectorTableEvents.AfterUpdate += _connectorTableEvents_AfterUpdat;

                return;
            }

            if (_draftBendTableEvents != null)
            {
                if (_mSEEventType == SEEvent.DraftBendTableAfterUpdate)
                    _draftBendTableEvents.AfterUpdate += _draftBendTableEvents_AfterUpdat;

                return;
            }

            if (_drawingViewEvents != null)
            {
                if (_mSEEventType == SEEvent.DrawingViewAfterUpdate)
                    _drawingViewEvents.AfterUpdate += _drawingViewEvents_AfterUpdate;

                return;
            }

            if (_partsListEvents != null)
            {
                if (_mSEEventType == SEEvent.PartsListAfterUpdate)
                    _partsListEvents.AfterUpdate += _partsListEvents_AfterUpdate;

                return;
            }

            if (_dividePartEvents != null)
            {
                if (_mSEEventType == SEEvent.DividePartAfterDividePartDocumentCreated)
                    _dividePartEvents.AfterDividePartDocumentCreated += _dividePartEvents_AfterDividePartDocumentCreated;
                else if (_mSEEventType == SEEvent.DividePartAfterDividePartDocumentRenamed)
                    _dividePartEvents.AfterDividePartDocumentRenamed += _dividePartEvents_AfterDividePartDocumentRenamed;
                if (_mSEEventType == SEEvent.DividePartBeforeDividePartDocumentDeleted)
                    _dividePartEvents.BeforeDividePartDocumentDeleted += _dividePartEvents_BeforeDividePartDocumentDeleted;

                return;
            }

            if (_familyOfPartsEvents != null)
            {
                if (_mSEEventType == SEEvent.FamilyOfPartsAfterMemberDocumentCreated)
                    _familyOfPartsEvents.AfterMemberDocumentCreated += _familyOfPartsEvents_AfterMemberDocumentCreated;
                else if (_mSEEventType == SEEvent.FamilyOfPartsAfterMemberDocumentRenamed)
                    _familyOfPartsEvents.AfterMemberDocumentRenamed += _familyOfPartsEvents_AfterMemberDocumentRenamed;
                else if (_mSEEventType == SEEvent.FamilyOfPartsBeforeMemberDocumentDeleted)
                    _familyOfPartsEvents.BeforeMemberDocumentDeleted += _familyOfPartsEvents_BeforeMemberDocumentDeleted;

                return;
            }

            if (_familyOfPartsExEvents != null)
            {
                if (_mSEEventType == SEEvent.FamilyOfPartsExAfterMemberDocumentCreated)
                    _familyOfPartsExEvents.AfterMemberDocumentCreated += _familyOfPartsExEvents_AfterMemberDocumentCreated;
                else if (_mSEEventType == SEEvent.FamilyOfPartsExAfterMemberDocumentRenamed)
                    _familyOfPartsExEvents.AfterMemberDocumentRenamed += _familyOfPartsExEvents_AfterMemberDocumentRenamed;
                else if (_mSEEventType == SEEvent.FamilyOfPartsExBeforeMemberDocumentDeleted)
                    _familyOfPartsExEvents.BeforeMemberDocumentDeleted += _familyOfPartsExEvents_BeforeMemberDocumentDeleted;

                return;
            }
        }


        /// <summary>
        /// 取消注册事件
        /// </summary>
        internal void UnregisterSolidEdgeEvent()
        {
            if (_documentEvents != null)
            {
                if (_mSEEventType == SEEvent.DocumentAfterSave)
                    _documentEvents.AfterSave -= _documentEvents_AfterSave;
                else if (_mSEEventType == SEEvent.DocumentBeforeClose)
                    _documentEvents.BeforeClose -= _documentEvents_BeforeClose;
                else if (_mSEEventType == SEEvent.DocumentBeforeSave)
                    _documentEvents.BeforeSave -= _documentEvents_BeforeSave;
                else if (_mSEEventType == SEEvent.DocumentSelectSetChanged)
                    _documentEvents.SelectSetChanged -= _documentEvents_SelectSetChanged;

                return;
            }

            if (_assemblyChangeEvents != null)
            {
                if (_mSEEventType == SEEvent.AssemblyAfterChange)
                    _assemblyChangeEvents.AfterChange -= _assemblyChangeEvents_AfterChange;
                else if (_mSEEventType == SEEvent.AssemblyBeforeChange)
                    _assemblyChangeEvents.BeforeChange -= _assemblyChangeEvents_BeforeChange;

                return;
            }

            if (_assemblyFamilyEvents != null)
            {
                if (_mSEEventType == SEEvent.AssemblyFamilyAfterMemberActivate)
                    _assemblyFamilyEvents.AfterMemberActivate -= _assemblyFamilyEvents_AfterMemberActivate;
                else if (_mSEEventType == SEEvent.AssemblyFamilyAfterMemberCreate)
                    _assemblyFamilyEvents.AfterMemberCreate -= _assemblyFamilyEvents_AfterMemberCreate;
                else if (_mSEEventType == SEEvent.AssemblyFamilyAfterMemberDelete)
                    _assemblyFamilyEvents.AfterMemberDelete -= _assemblyFamilyEvents_AfterMemberDelete;
                else if (_mSEEventType == SEEvent.AssemblyFamilyBeforeMemberActivate)
                    _assemblyFamilyEvents.BeforeMemberActivate -= _assemblyFamilyEvents_BeforeMemberActivate;
                else if (_mSEEventType == SEEvent.AssemblyFamilyBeforeMemberCreate)
                    _assemblyFamilyEvents.BeforeMemberCreate -= _assemblyFamilyEvents_BeforeMemberCreate;
                else if (_mSEEventType == SEEvent.AssemblyFamilyBeforeMemberDelete)
                    _assemblyFamilyEvents.BeforeMemberDelete -= _assemblyFamilyEvents_BeforeMemberDelete;

                return;
            }

            if (_assemblyRecomputeEvents != null)
            {
                if (_mSEEventType == SEEvent.AssemblyRecomputeAfterAdd)
                    _assemblyRecomputeEvents.AfterAdd -= _assemblyRecomputeEvents_AfterAdd;
                else if (_mSEEventType == SEEvent.AssemblyRecomputeAfterModify)
                    _assemblyRecomputeEvents.AfterModify -= _assemblyRecomputeEvents_AfterModify;
                else if (_mSEEventType == SEEvent.AssemblyRecomputeAfterRecompute)
                    _assemblyRecomputeEvents.AfterRecompute -= _assemblyRecomputeEvents_AfterRecompute;
                else if (_mSEEventType == SEEvent.AssemblyRecomputeBeforeDelete)
                    _assemblyRecomputeEvents.BeforeDelete -= _assemblyRecomputeEvents_BeforeDelete;
                else if (_mSEEventType == SEEvent.AssemblyRecomputeBeforeRecompute)
                    _assemblyRecomputeEvents.BeforeRecompute -= _assemblyRecomputeEvents_BeforeRecompute;
            }

            if (_blockTableEvents != null)
            {
                if (_mSEEventType == SEEvent.BlockTableAfterUpdate)
                    _blockTableEvents.AfterUpdate -= _blockTableEvents_AfterUpdate;

                return;
            }

            if (_connectorTableEvents != null)
            {
                if (_mSEEventType == SEEvent.ConnectorTableAfterUpdate)
                    _connectorTableEvents.AfterUpdate -= _connectorTableEvents_AfterUpdat;

                return;
            }

            if (_draftBendTableEvents != null)
            {
                if (_mSEEventType == SEEvent.DraftBendTableAfterUpdate)
                    _draftBendTableEvents.AfterUpdate -= _draftBendTableEvents_AfterUpdat;

                return;
            }

            if (_drawingViewEvents != null)
            {
                if (_mSEEventType == SEEvent.DrawingViewAfterUpdate)
                    _drawingViewEvents.AfterUpdate -= _drawingViewEvents_AfterUpdate;

                return;
            }

            if (_partsListEvents != null)
            {
                if (_mSEEventType == SEEvent.PartsListAfterUpdate)
                    _partsListEvents.AfterUpdate -= _partsListEvents_AfterUpdate;

                return;
            }

            if (_dividePartEvents != null)
            {
                if (_mSEEventType == SEEvent.DividePartAfterDividePartDocumentCreated)
                    _dividePartEvents.AfterDividePartDocumentCreated -= _dividePartEvents_AfterDividePartDocumentCreated;
                else if (_mSEEventType == SEEvent.DividePartAfterDividePartDocumentRenamed)
                    _dividePartEvents.AfterDividePartDocumentRenamed -= _dividePartEvents_AfterDividePartDocumentRenamed;
                if (_mSEEventType == SEEvent.DividePartBeforeDividePartDocumentDeleted)
                    _dividePartEvents.BeforeDividePartDocumentDeleted -= _dividePartEvents_BeforeDividePartDocumentDeleted;

                return;
            }

            if (_familyOfPartsEvents != null)
            {
                if (_mSEEventType == SEEvent.FamilyOfPartsAfterMemberDocumentCreated)
                    _familyOfPartsEvents.AfterMemberDocumentCreated -= _familyOfPartsEvents_AfterMemberDocumentCreated;
                else if (_mSEEventType == SEEvent.FamilyOfPartsAfterMemberDocumentRenamed)
                    _familyOfPartsEvents.AfterMemberDocumentRenamed -= _familyOfPartsEvents_AfterMemberDocumentRenamed;
                else if (_mSEEventType == SEEvent.FamilyOfPartsBeforeMemberDocumentDeleted)
                    _familyOfPartsEvents.BeforeMemberDocumentDeleted -= _familyOfPartsEvents_BeforeMemberDocumentDeleted;

                return;
            }

            if (_familyOfPartsExEvents != null)
            {
                if (_mSEEventType == SEEvent.FamilyOfPartsExAfterMemberDocumentCreated)
                    _familyOfPartsExEvents.AfterMemberDocumentCreated -= _familyOfPartsExEvents_AfterMemberDocumentCreated;
                else if (_mSEEventType == SEEvent.FamilyOfPartsExAfterMemberDocumentRenamed)
                    _familyOfPartsExEvents.AfterMemberDocumentRenamed -= _familyOfPartsExEvents_AfterMemberDocumentRenamed;
                else if (_mSEEventType == SEEvent.FamilyOfPartsExBeforeMemberDocumentDeleted)
                    _familyOfPartsExEvents.BeforeMemberDocumentDeleted -= _familyOfPartsExEvents_BeforeMemberDocumentDeleted;

                return;
            }
        }

        private void _assemblyRecomputeEvents_BeforeRecompute(object theDocument)
        {
            _mRegisterMethod(new object[] { theDocument });
        }

        private void _assemblyRecomputeEvents_BeforeDelete(object theDocument, object Object, ObjectType Type)
        {
            _mRegisterMethod(new object[] { theDocument, Object, Type });
        }

        private void _assemblyRecomputeEvents_AfterRecompute(object theDocument)
        {
            _mRegisterMethod(new object[] { theDocument });
        }

        private void _assemblyRecomputeEvents_AfterModify(object theDocument, object Object, ObjectType Type, [System.Runtime.InteropServices.ComAliasName("SolidEdgeFramework.seAssemblyEventConstants")] seAssemblyEventConstants ModifyType)
        {
            _mRegisterMethod(new object[] { theDocument });
        }

        private void _assemblyRecomputeEvents_AfterAdd(object theDocument, object Object, ObjectType Type)
        {
            _mRegisterMethod(new object[] { theDocument });
        }

        private void _dividePartEvents_AfterDividePartDocumentRenamed(object theMember, string OldName)
        {
            _mRegisterMethod(new object[] { theMember, OldName });
        }

        private void _dividePartEvents_AfterDividePartDocumentCreated(object theMember)
        {
            _mRegisterMethod(new object[] { theMember });
        }

        private void _dividePartEvents_BeforeDividePartDocumentDeleted(object theMember)
        {
            _mRegisterMethod(new object[] { theMember });
        }

        private void _familyOfPartsEvents_AfterMemberDocumentCreated(object theMember)
        {
            _mRegisterMethod(new object[] { theMember });
        }

        private void _familyOfPartsEvents_AfterMemberDocumentRenamed(object theMember, string OldName)
        {
            _mRegisterMethod(new object[] { theMember, OldName });
        }

        private void _familyOfPartsEvents_BeforeMemberDocumentDeleted(object theMember)
        {
            _mRegisterMethod(new object[] { theMember });
        }

        private void _familyOfPartsExEvents_AfterMemberDocumentCreated(object theMember, bool DocumentExists)
        {
            _mRegisterMethod(new object[] { theMember, DocumentExists });
        }

        private void _familyOfPartsExEvents_AfterMemberDocumentRenamed(object theMember, string OldName)
        {
            _mRegisterMethod(new object[] { theMember, OldName });
        }

        private void _familyOfPartsExEvents_BeforeMemberDocumentDeleted(object theMember)
        {
            _mRegisterMethod(new object[] { theMember });
        }

        private void _partsListEvents_AfterUpdate(object PartsList)
        {
            _mRegisterMethod(new object[] { PartsList });
        }

        private void _drawingViewEvents_AfterUpdate(object DrawingView)
        {
            _mRegisterMethod(new object[] { DrawingView });
        }

        private void _draftBendTableEvents_AfterUpdat(object DraftBendTable)
        {
            _mRegisterMethod(new object[] { DraftBendTable });
        }

        private void _connectorTableEvents_AfterUpdat(object ConnectorTable)
        {
            _mRegisterMethod(new object[] { ConnectorTable });
        }

        private void _blockTableEvents_AfterUpdate(object BlockTable)
        {
            _mRegisterMethod(new object[] { BlockTable });
        }

        private void _assemblyFamilyEvents_BeforeMemberDelete(object theDocument, string memberName)
        {
            _mRegisterMethod(new object[] { theDocument, memberName });
        }

        private void _assemblyFamilyEvents_BeforeMemberCreate(object theDocument, string memberName)
        {
            _mRegisterMethod(new object[] { theDocument, memberName });
        }

        private void _assemblyFamilyEvents_BeforeMemberActivate(object theDocument, string memberName)
        {
            _mRegisterMethod(new object[] { theDocument, memberName });
        }

        private void _assemblyFamilyEvents_AfterMemberDelete(object theDocument, string memberName)
        {
            _mRegisterMethod(new object[] { theDocument, memberName });
        }

        private void _assemblyFamilyEvents_AfterMemberCreate(object theDocument, string memberName)
        {
            _mRegisterMethod(new object[] { theDocument, memberName });
        }

        private void _assemblyFamilyEvents_AfterMemberActivate(object theDocument, string memberName)
        {
            _mRegisterMethod(new object[] { theDocument, memberName });
        }

        private void _assemblyChangeEvents_BeforeChange(object theDocument, object Object, ObjectType Type, [System.Runtime.InteropServices.ComAliasName("SolidEdgeFramework.seAssemblyChangeEventsConstants")] seAssemblyChangeEventsConstants ChangeType)
        {
            _mRegisterMethod(new object[] { theDocument, Object, Type, ChangeType });
        }

        private void _assemblyChangeEvents_AfterChange(object theDocument, object Object, ObjectType Type, [System.Runtime.InteropServices.ComAliasName("SolidEdgeFramework.seAssemblyChangeEventsConstants")] seAssemblyChangeEventsConstants ChangeType)
        {
            _mRegisterMethod(new object[] { theDocument, Object, Type, ChangeType });
        }

        private void _documentEvents_SelectSetChanged(object SelectSet)
        {
            _mRegisterMethod(new object[] { SelectSet });
        }

        private void _documentEvents_BeforeSave()
        {
            _mRegisterMethod(null);
        }

        private void _documentEvents_BeforeClose()
        {
            _mRegisterMethod(null);
        }

        private void _documentEvents_AfterSave()
        {
            _mRegisterMethod?.Invoke(null);
        }
    }
}
