namespace Symtab
{

    /// <summary>
    /// A symbol within an aggregate like a class or struct. Each
    ///  symbol in an aggregate knows its slot number so we can order
    ///  elements in memory, for example, or keep overridden method slots
    ///  in sync for vtables.
    /// </summary>
    public interface IMemberSymbol : ISymbol
    {
        int getSlotNumber();
    }

}