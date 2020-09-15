using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;

namespace Protocol
{

    //
    // Summary:
    //     Struct that may contain a T1 or a T2.
    //
    // Type parameters:
    //   T1:
    //     The first type this struct is designed to contain.
    //
    //   T2:
    //     The second type this struct is designed to contain.
    [JsonConverter(typeof(SumConverter))]
    public struct SumType<T1, T2> : ISumType, IEquatable<SumType<T1, T2>>
    {
        //
        // Summary:
        //     Initializes a new instance of the Microsoft.VisualStudio.LanguageServer.Protocol.SumType`2
        //     struct containing a T1.
        //
        // Parameters:
        //   val:
        //     The value to store in the Microsoft.VisualStudio.LanguageServer.Protocol.SumType`2.
        public SumType(T1 val) { throw new NotImplementedException(); }
        //
        // Summary:
        //     Initializes a new instance of the Microsoft.VisualStudio.LanguageServer.Protocol.SumType`2
        //     struct containing a T2.
        //
        // Parameters:
        //   val:
        //     The value to store in the Microsoft.VisualStudio.LanguageServer.Protocol.SumType`2.
        public SumType(T2 val) { throw new NotImplementedException(); }

        public object Value { get; }

        public override bool Equals(object obj) { throw new NotImplementedException(); }
        public bool Equals(SumType<T1, T2> other) { throw new NotImplementedException(); }
        public override int GetHashCode() { throw new NotImplementedException(); }
        //
        // Summary:
        //     Runs a delegate corresponding to which type is contained inside this instance.
        //
        // Parameters:
        //   firstMatch:
        //     Delegate to handle the case where this instance contains a T1.
        //
        //   secondMatch:
        //     Delegate to handle the case where this instance contains a T2.
        //
        //   defaultMatch:
        //     Delegate to handle the case where this instance is uninhabited. If this delegate
        //     isn't provided the default TResult will be returned instead.
        //
        // Type parameters:
        //   TResult:
        //     The type that all the delegates will return.
        //
        // Returns:
        //     The TResult instance created by the delegate corresponding to the current type
        //     stored in this instance.
        public TResult Match<TResult>(Func<T1, TResult> firstMatch, Func<T2, TResult> secondMatch, Func<TResult> defaultMatch = null) { throw new NotImplementedException(); }

        public static bool operator ==(SumType<T1, T2> left, SumType<T1, T2> right) { throw new NotImplementedException(); }
        public static bool operator !=(SumType<T1, T2> left, SumType<T1, T2> right) { throw new NotImplementedException(); }

        //
        // Summary:
        //     Implicitly wraps a value of type T1 with a Microsoft.VisualStudio.LanguageServer.Protocol.SumType`2.
        //
        // Parameters:
        //   val:
        //     Value to be wrapped.
        public static implicit operator SumType<T1, T2>(T1 val) { throw new NotImplementedException(); }
        //
        // Summary:
        //     Implicitly wraps a value of type T2 with a Microsoft.VisualStudio.LanguageServer.Protocol.SumType`2.
        //
        // Parameters:
        //   val:
        //     Value to be wrapped.
        public static implicit operator SumType<T1, T2>(T2 val) { throw new NotImplementedException(); }
        //
        // Summary:
        //     Attempts to cast an instance of Microsoft.VisualStudio.LanguageServer.Protocol.SumType`2
        //     to an instance of T1.
        //
        // Parameters:
        //   sum:
        //     Instance to unwrap.
        //
        // Exceptions:
        //   T:System.InvalidCastException:
        //     Thrown if this instance of Microsoft.VisualStudio.LanguageServer.Protocol.SumType`2
        //     does not contain an instance of T1.
        public static explicit operator T1(SumType<T1, T2> sum) { throw new NotImplementedException(); }
        //
        // Summary:
        //     Attempts to cast an instance of Microsoft.VisualStudio.LanguageServer.Protocol.SumType`2
        //     to an instance of T2.
        //
        // Parameters:
        //   sum:
        //     Instance to unwrap.
        //
        // Exceptions:
        //   T:System.InvalidCastException:
        //     Thrown if this instance of Microsoft.VisualStudio.LanguageServer.Protocol.SumType`2
        //     does not contain an instance of T2.
        public static explicit operator T2(SumType<T1, T2> sum) { throw new NotImplementedException(); }
    }

    //
    // Summary:
    //     Struct that may contain a T1, a T2, or a T3.
    //
    // Type parameters:
    //   T1:
    //     The first type this struct is designed to contain.
    //
    //   T2:
    //     The second type this struct is designed to contain.
    //
    //   T3:
    //     The third type this struct is designed to contain.
    [JsonConverter(typeof(SumConverter))]
    public struct SumType<T1, T2, T3> : ISumType, IEquatable<SumType<T1, T2, T3>>
    {
        //
        // Summary:
        //     Initializes a new instance of the Microsoft.VisualStudio.LanguageServer.Protocol.SumType`3
        //     struct containing a T1.
        //
        // Parameters:
        //   val:
        //     The value to store in the Microsoft.VisualStudio.LanguageServer.Protocol.SumType`3.
        public SumType(T1 val) { throw new NotImplementedException(); }
        //
        // Summary:
        //     Initializes a new instance of the Microsoft.VisualStudio.LanguageServer.Protocol.SumType`3
        //     struct containing a T2.
        //
        // Parameters:
        //   val:
        //     The value to store in the Microsoft.VisualStudio.LanguageServer.Protocol.SumType`3.
        public SumType(T2 val) { throw new NotImplementedException(); }
        //
        // Summary:
        //     Initializes a new instance of the Microsoft.VisualStudio.LanguageServer.Protocol.SumType`3
        //     struct containing a T3.
        //
        // Parameters:
        //   val:
        //     The value to store in the Microsoft.VisualStudio.LanguageServer.Protocol.SumType`3.
        public SumType(T3 val) { throw new NotImplementedException(); }

        public object Value { get; }

        public override bool Equals(object obj) { throw new NotImplementedException(); }
        public bool Equals(SumType<T1, T2, T3> other) { throw new NotImplementedException(); }
        public override int GetHashCode() { throw new NotImplementedException(); }
        //
        // Summary:
        //     Runs a delegate corresponding to which type is contained inside this instance.
        //
        // Parameters:
        //   firstMatch:
        //     Delegate to handle the case where this instance contains a T1.
        //
        //   secondMatch:
        //     Delegate to handle the case where this instance contains a T2.
        //
        //   thirdMatch:
        //     Delegate to handle the case where this instance contains a T3.
        //
        //   defaultMatch:
        //     Delegate to handle the case where this instance is uninhabited. If this delegate
        //     isn't provided the default TResult will be returned instead.
        //
        // Type parameters:
        //   TResult:
        //     The type that all the delegates will return.
        //
        // Returns:
        //     The TResult instance created by the delegate corresponding to the current type
        //     stored in this instance.
        public TResult Match<TResult>(Func<T1, TResult> firstMatch, Func<T2, TResult> secondMatch, Func<T3, TResult> thirdMatch, Func<TResult> defaultMatch = null) { throw new NotImplementedException(); }

        public static bool operator ==(SumType<T1, T2, T3> left, SumType<T1, T2, T3> right) { throw new NotImplementedException(); }
        public static bool operator !=(SumType<T1, T2, T3> left, SumType<T1, T2, T3> right) { throw new NotImplementedException(); }

        //
        // Summary:
        //     Implicitly wraps a value of type T2 with a Microsoft.VisualStudio.LanguageServer.Protocol.SumType`3.
        //
        // Parameters:
        //   val:
        //     Value to be wrap.
        public static implicit operator SumType<T1, T2, T3>(T2 val) { throw new NotImplementedException(); }
        //
        // Summary:
        //     Implicitly wraps a value of type T3 with a Microsoft.VisualStudio.LanguageServer.Protocol.SumType`3.
        //
        // Parameters:
        //   val:
        //     Value to be wrap.
        public static implicit operator SumType<T1, T2, T3>(T3 val) { throw new NotImplementedException(); }
        //
        // Summary:
        //     Implicitly wraps an instance of Microsoft.VisualStudio.LanguageServer.Protocol.SumType`2
        //     with a Microsoft.VisualStudio.LanguageServer.Protocol.SumType`3.
        //
        // Parameters:
        //   sum:
        //     Sum instance to wrap.
        public static implicit operator SumType<T1, T2, T3>(SumType<T1, T2> sum) { throw new NotImplementedException(); }
        //
        // Summary:
        //     Implicitly wraps a value of type T1 with a Microsoft.VisualStudio.LanguageServer.Protocol.SumType`3.
        //
        // Parameters:
        //   val:
        //     Value to be wrap.
        public static implicit operator SumType<T1, T2, T3>(T1 val) { throw new NotImplementedException(); }
        //
        // Summary:
        //     Attempts to cast an instance of Microsoft.VisualStudio.LanguageServer.Protocol.SumType`3
        //     into a Microsoft.VisualStudio.LanguageServer.Protocol.SumType`2.
        //
        // Parameters:
        //   sum:
        //     Sum instance to downcast.
        public static explicit operator SumType<T1, T2>(SumType<T1, T2, T3> sum) { throw new NotImplementedException(); }
        //
        // Summary:
        //     Attempts to cast an instance of Microsoft.VisualStudio.LanguageServer.Protocol.SumType`2
        //     to an instance of T2.
        //
        // Parameters:
        //   sum:
        //     Instance to unwrap.
        //
        // Exceptions:
        //   T:System.InvalidCastException:
        //     Thrown if this instance of Microsoft.VisualStudio.LanguageServer.Protocol.SumType`3
        //     does not contain an instance of T2.
        public static explicit operator T2(SumType<T1, T2, T3> sum) { throw new NotImplementedException(); }
        //
        // Summary:
        //     Attempts to cast an instance of Microsoft.VisualStudio.LanguageServer.Protocol.SumType`3
        //     to an instance of T3.
        //
        // Parameters:
        //   sum:
        //     Instance to unwrap.
        //
        // Exceptions:
        //   T:System.InvalidCastException:
        //     Thrown if this instance of Microsoft.VisualStudio.LanguageServer.Protocol.SumType`3
        //     does not contain an instance of T3.
        public static explicit operator T3(SumType<T1, T2, T3> sum) { throw new NotImplementedException(); }
        //
        // Summary:
        //     Attempts to cast an instance of Microsoft.VisualStudio.LanguageServer.Protocol.SumType`3
        //     to an instance of T1.
        //
        // Parameters:
        //   sum:
        //     Instance to unwrap.
        //
        // Exceptions:
        //   T:System.InvalidCastException:
        //     Thrown if this instance of Microsoft.VisualStudio.LanguageServer.Protocol.SumType`3
        //     does not contain an instance of T1.
        public static explicit operator T1(SumType<T1, T2, T3> sum) { throw new NotImplementedException(); }
    }
}
