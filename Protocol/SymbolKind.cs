using System.Runtime.Serialization;

namespace Protocol
{
    //
    // Summary:
    //     Enum which represents the various kinds of symbols.
    [DataContract]
    public enum SymbolKind
    {
        //
        // Summary:
        //     Symbol is a file.
        File = 1,
        //
        // Summary:
        //     Symbol is a module.
        Module = 2,
        //
        // Summary:
        //     Symbol is a namespace.
        Namespace = 3,
        //
        // Summary:
        //     Symbol is a package.
        Package = 4,
        //
        // Summary:
        //     Symbol is a class.
        Class = 5,
        //
        // Summary:
        //     Symbol is a method.
        Method = 6,
        //
        // Summary:
        //     Symbol is a property.
        Property = 7,
        //
        // Summary:
        //     Symbol is a field.
        Field = 8,
        //
        // Summary:
        //     Symbol is a constructor.
        Constructor = 9,
        //
        // Summary:
        //     Symbol is an enum.
        Enum = 10,
        //
        // Summary:
        //     Symbol is an interface.
        Interface = 11,
        //
        // Summary:
        //     Symbol is a function.
        Function = 12,
        //
        // Summary:
        //     Symbol is a variable.
        Variable = 13,
        //
        // Summary:
        //     Symbol is a constant.
        Constant = 14,
        //
        // Summary:
        //     Symbol is a string.
        String = 15,
        //
        // Summary:
        //     Symbol is a number.
        Number = 16,
        //
        // Summary:
        //     Symbol is a boolean.
        Boolean = 17,
        //
        // Summary:
        //     Symbol is an array.
        Array = 18,
        //
        // Summary:
        //     Symbol is an object.
        Object = 19,
        //
        // Summary:
        //     Symbol is a key.
        Key = 20,
        //
        // Summary:
        //     Symbol is null.
        Null = 21,
        //
        // Summary:
        //     Symbol is an enum member.
        EnumMember = 22,
        //
        // Summary:
        //     Symbol is a struct.
        Struct = 23,
        //
        // Summary:
        //     Symbol is an event.
        Event = 24,
        //
        // Summary:
        //     Symbol is an operator.
        Operator = 25,
        //
        // Summary:
        //     Symbol is a type parameter.
        TypeParameter = 26
    }
}
