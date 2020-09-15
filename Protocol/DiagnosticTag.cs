namespace Protocol
{
    //
    // Summary:
    //     Diagnostic tag enum. Additional metadata about the type of a diagnostic
    public enum DiagnosticTag
    {
        //
        // Summary:
        //     Unused or unnecessary code. Diagnostics with this tag are rendered faded out.
        Unnecessary = 1,
        //
        // Summary:
        //     Deprecated or obsolete code. Clients are allowed to rendered diagnostics with
        //     this tag strike through.
        Deprecated = 2
    }
}
