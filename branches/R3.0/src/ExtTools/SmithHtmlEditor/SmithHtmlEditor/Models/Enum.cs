namespace Smith.WPF.HtmlEditor
{
    #region Enumerations

    public enum EditMode
    {
        Visual,
        Source
    }

    public enum HtmlDocumentState
    {
        Uninitialized,
        Loading,
        Loaded,
        Interactive,
        Complete
    }

    #endregion Enumerations
}