namespace SolidEdge.Events.EventEnum
{
    /// <summary>
    /// SolidEdge事件枚举
    /// </summary>
    public enum SEEvent
    {

        DocumentAfterSave = 10000,
        DocumentBeforeClose = 10001,
        DocumentBeforeSave = 10002,
        DocumentSelectSetChanged = 10003,

        AssemblyAfterChange = 10004,
        AssemblyBeforeChange = 10005,

        AssemblyFamilyAfterMemberActivate = 10006,
        AssemblyFamilyAfterMemberCreate = 10007,
        AssemblyFamilyAfterMemberDelete = 10008,
        AssemblyFamilyBeforeMemberActivate = 10009,
        AssemblyFamilyBeforeMemberCreate = 10010,
        AssemblyFamilyBeforeMemberDelete = 10011,

        AssemblyRecomputeAfterAdd = 10012,
        AssemblyRecomputeAfterModify = 10013,
        AssemblyRecomputeAfterRecompute = 11004,
        AssemblyRecomputeBeforeDelete = 10015,
        AssemblyRecomputeBeforeRecompute = 10016,

        BlockTableAfterUpdate = 10017,

        ConnectorTableAfterUpdate = 10018,

        DraftBendTableAfterUpdate = 10019,

        DrawingViewAfterUpdate = 10020,

        PartsListAfterUpdate = 10021,

        DividePartAfterDividePartDocumentCreated = 10022,
        DividePartAfterDividePartDocumentRenamed = 10023,
        DividePartBeforeDividePartDocumentDeleted = 10024,

        FamilyOfPartsAfterMemberDocumentCreated = 10025,
        FamilyOfPartsAfterMemberDocumentRenamed = 10026,
        FamilyOfPartsBeforeMemberDocumentDeleted = 10027,

        FamilyOfPartsExAfterMemberDocumentCreated = 10028,
        FamilyOfPartsExAfterMemberDocumentRenamed = 10029,
        FamilyOfPartsExBeforeMemberDocumentDeleted = 10030,

        ApplicationAfterActiveDocumentChange = 10031,
        ApplicationAfterCommandRun = 10032,
        ApplicationAfterDocumentOpen = 10033,
        ApplicationAfterDocumentPrint = 10034,
        ApplicationAfterDocumentSave = 10035,
        ApplicationAfterEnvironmentActivate = 10036,
        ApplicationAfterNewDocumentOpen = 10037,
        ApplicationAfterNewWindow = 10038,
        ApplicationAfterWindowActivate = 10039,
        ApplicationBeforeCommandRun = 10040,
        ApplicationBeforeDocumentClose = 10041,
        ApplicationBeforeDocumentPrint = 10042,
        ApplicationBeforeEnvironmentDeactivate = 10043,
        ApplicationBeforeWindowDeactivate = 10044,
        ApplicationBeforeQuit = 10045,
        ApplicationBeforeDocumentSave = 10046,
    }
}
