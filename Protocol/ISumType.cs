namespace Protocol
{
    //
    // Summary:
    //     Abstracts over the idea of a "sum type". Sum types are types that can contain
    //     one value of various types. This abstraction is guaranteed to be typesafe, meaning
    //     you cannot access the underlying value without knowing its specific type.
    public interface ISumType
    {
        //
        // Summary:
        //     Gets the value stored in the SumType. This can be matched against using the "is"
        //     operator.
        object Value { get; }
    }
}
