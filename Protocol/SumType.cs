using Newtonsoft.Json;
using System;
using System.Collections.Generic;

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
        public SumType(T1 val) { this.Value = (object)val; }
        //
        // Summary:
        //     Initializes a new instance of the Microsoft.VisualStudio.LanguageServer.Protocol.SumType`2
        //     struct containing a T2.
        //
        // Parameters:
        //   val:
        //     The value to store in the Microsoft.VisualStudio.LanguageServer.Protocol.SumType`2.
        public SumType(T2 val) { this.Value = (object)val; }

        public object Value { get; }

        public override bool Equals(object obj) { return obj is SumType<T1, T2> other && this.Equals(other); }
        public bool Equals(SumType<T1, T2> other) { return EqualityComparer<object>.Default.Equals(this.Value, other.Value); }
        public override int GetHashCode() { return EqualityComparer<object>.Default.GetHashCode(this.Value) - 1937169414; }
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
        public TResult Match<TResult>(Func<T1, TResult> firstMatch, Func<T2, TResult> secondMatch, Func<TResult> defaultMatch = null)
        {
            if (firstMatch == null)
                throw new ArgumentNullException(nameof(firstMatch));
            if (secondMatch == null)
                throw new ArgumentNullException(nameof(secondMatch));
            if (this.Value is T1 obj1)
                return firstMatch(obj1);
            if (this.Value is T2 obj2)
                return secondMatch(obj2);
            return defaultMatch != null ? defaultMatch() : default(TResult);
        }

        public static bool operator ==(SumType<T1, T2> left, SumType<T1, T2> right) { return left.Equals(right); }
        public static bool operator !=(SumType<T1, T2> left, SumType<T1, T2> right) { return !(left == right); }

        //
        // Summary:
        //     Implicitly wraps a value of type T1 with a Microsoft.VisualStudio.LanguageServer.Protocol.SumType`2.
        //
        // Parameters:
        //   val:
        //     Value to be wrapped.
        public static implicit operator SumType<T1, T2>(T1 val) { return new SumType<T1, T2>(val); }
        //
        // Summary:
        //     Implicitly wraps a value of type T2 with a Microsoft.VisualStudio.LanguageServer.Protocol.SumType`2.
        //
        // Parameters:
        //   val:
        //     Value to be wrapped.
        public static implicit operator SumType<T1, T2>(T2 val) { return new SumType<T1, T2>(val); }
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
        public static explicit operator T1(SumType<T1, T2> sum)
        {
            if (sum.Value is T1 obj1)
                return obj1;
            throw new InvalidCastException();
        }
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
        public static explicit operator T2(SumType<T1, T2> sum)
        {
            if (sum.Value is T2 obj2)
                return obj2;
            throw new InvalidCastException();
        }

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
        public SumType(T1 val) { this.Value = (object)val; }
        //
        // Summary:
        //     Initializes a new instance of the Microsoft.VisualStudio.LanguageServer.Protocol.SumType`3
        //     struct containing a T2.
        //
        // Parameters:
        //   val:
        //     The value to store in the Microsoft.VisualStudio.LanguageServer.Protocol.SumType`3.
        public SumType(T2 val) { this.Value = (object)val; }
        //
        // Summary:
        //     Initializes a new instance of the Microsoft.VisualStudio.LanguageServer.Protocol.SumType`3
        //     struct containing a T3.
        //
        // Parameters:
        //   val:
        //     The value to store in the Microsoft.VisualStudio.LanguageServer.Protocol.SumType`3.
        public SumType(T3 val) { this.Value = (object)val; }

        public object Value { get; }

        public override bool Equals(object obj) { return obj is SumType<T1, T2, T3> other && this.Equals(other); }
        public bool Equals(SumType<T1, T2, T3> other) { return EqualityComparer<object>.Default.Equals(this.Value, other.Value); }
        public override int GetHashCode() { return EqualityComparer<object>.Default.GetHashCode(this.Value) - 1937169414; }
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
        public TResult Match<TResult>(Func<T1, TResult> firstMatch, Func<T2, TResult> secondMatch, Func<T3, TResult> thirdMatch, Func<TResult> defaultMatch = null)
        {
            if (firstMatch == null)
                throw new ArgumentNullException(nameof(firstMatch));
            if (secondMatch == null)
                throw new ArgumentNullException(nameof(secondMatch));
            if (thirdMatch == null)
                throw new ArgumentNullException(nameof(thirdMatch));
            if (this.Value is T1 obj1)
                return firstMatch(obj1);
            if (this.Value is T2 obj2)
                return secondMatch(obj2);
            if (this.Value is T3 obj3)
                return thirdMatch(obj3);
            return defaultMatch != null ? defaultMatch() : default(TResult);
        }

        public static bool operator ==(SumType<T1, T2, T3> left, SumType<T1, T2, T3> right) { return left.Equals(right); }
        public static bool operator !=(SumType<T1, T2, T3> left, SumType<T1, T2, T3> right) { return !(left == right); }

        //
        // Summary:
        //     Implicitly wraps a value of type T2 with a Microsoft.VisualStudio.LanguageServer.Protocol.SumType`3.
        //
        // Parameters:
        //   val:
        //     Value to be wrap.
        public static implicit operator SumType<T1, T2, T3>(T2 val) { return new SumType<T1, T2, T3>(val); }
        //
        // Summary:
        //     Implicitly wraps a value of type T3 with a Microsoft.VisualStudio.LanguageServer.Protocol.SumType`3.
        //
        // Parameters:
        //   val:
        //     Value to be wrap.
        public static implicit operator SumType<T1, T2, T3>(T3 val) { return new SumType<T1, T2, T3>(val); }
        //
        // Summary:
        //     Implicitly wraps an instance of Microsoft.VisualStudio.LanguageServer.Protocol.SumType`2
        //     with a Microsoft.VisualStudio.LanguageServer.Protocol.SumType`3.
        //
        // Parameters:
        //   sum:
        //     Sum instance to wrap.
        public static implicit operator SumType<T1, T2, T3>(SumType<T1, T2> sum)
        {
            throw new NotImplementedException();
            // ISSUE: method pointer
            // ISSUE: method pointer
            //return sum.Match<SumType<T1, T2, T3>>(
            //    SumType<T1, T2, T3>.<>c.<>9__9_0 ??
            //    (SumType<T1, T2, T3>.<>c.<>9__9_0
            //        = new Func<T1, SumType<T1, T2, T3>>(
            //            (object)SumType<T1, T2, T3>.<>c.<>9, __methodptr(<op_Implicit>b__9_0))),
            //            SumType<T1, T2, T3>.<>c.<>9__9_1 ??
            //            (SumType<T1, T2, T3>.<>c.<>9__9_1
            //            = new Func<T2, SumType<T1, T2, T3>>(
            //                (object)SumType<T1, T2, T3>.<>c.<>9, __methodptr(<op_Implicit>b__9_1))),
            //            (Func<SumType<T1, T2, T3>>)null);
        }
        //
        // Summary:
        //     Implicitly wraps a value of type T1 with a Microsoft.VisualStudio.LanguageServer.Protocol.SumType`3.
        //
        // Parameters:
        //   val:
        //     Value to be wrap.
        public static implicit operator SumType<T1, T2, T3>(T1 val) { return new SumType<T1, T2, T3>(val); }
        //
        // Summary:
        //     Attempts to cast an instance of Microsoft.VisualStudio.LanguageServer.Protocol.SumType`3
        //     into a Microsoft.VisualStudio.LanguageServer.Protocol.SumType`2.
        //
        // Parameters:
        //   sum:
        //     Sum instance to downcast.
        public static explicit operator SumType<T1, T2>(SumType<T1, T2, T3> sum)
        {
            if (sum.Value is T1 obj1)
                return (SumType<T1, T2>)obj1;
            if (sum.Value is T2 obj2)
                return (SumType<T1, T2>)obj2;
            throw new InvalidCastException();
        }
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
        public static explicit operator T2(SumType<T1, T2, T3> sum)
        {
            if (sum.Value is T2 obj)
                return obj;
            throw new InvalidCastException();
        }
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
        public static explicit operator T3(SumType<T1, T2, T3> sum)
        {
            if (sum.Value is T3 obj)
                return obj;
            throw new InvalidCastException();
        }
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
        public static explicit operator T1(SumType<T1, T2, T3> sum)
        {
            if (sum.Value is T1 obj)
                return obj;
            throw new InvalidCastException();
        }
    }

    [JsonConverter(typeof(SumConverter))]
    public struct SumType<T1, T2, T3, T4> : ISumType, IEquatable<SumType<T1, T2, T3, T4>>
    {
        public SumType(T1 val) { this.Value = (object)val; }
        public SumType(T2 val) { this.Value = (object)val; }
        public SumType(T3 val) { this.Value = (object)val; }
        public SumType(T4 val) { this.Value = (object)val; }
        public object Value { get; }
        public override bool Equals(object obj) { return obj is SumType<T1, T2, T3, T4> other && this.Equals(other); }
        public bool Equals(SumType<T1, T2, T3, T4> other) { return EqualityComparer<object>.Default.Equals(this.Value, other.Value); }
        public override int GetHashCode() { return EqualityComparer<object>.Default.GetHashCode(this.Value) - 1937169414; }
        public TResult Match<TResult>(Func<T1, TResult> firstMatch, Func<T2, TResult> secondMatch, Func<T3, TResult> thirdMatch, Func<T4, TResult> fourthMatch, Func<TResult> defaultMatch = null)
        {
            if (firstMatch == null)
                throw new ArgumentNullException(nameof(firstMatch));
            if (secondMatch == null)
                throw new ArgumentNullException(nameof(secondMatch));
            if (thirdMatch == null)
                throw new ArgumentNullException(nameof(thirdMatch));
            if (fourthMatch == null)
                throw new ArgumentNullException(nameof(fourthMatch));
            if (this.Value is T1 obj1)
                return firstMatch(obj1);
            if (this.Value is T2 obj2)
                return secondMatch(obj2);
            if (this.Value is T3 obj3)
                return thirdMatch(obj3);
            if (this.Value is T4 obj4)
                return fourthMatch(obj4);
            return defaultMatch != null ? defaultMatch() : default(TResult);
        }
        public static bool operator ==(SumType<T1, T2, T3, T4> left, SumType<T1, T2, T3, T4> right) { return left.Equals(right); }
        public static bool operator !=(SumType<T1, T2, T3, T4> left, SumType<T1, T2, T3, T4> right) { return !(left == right); }
        public static implicit operator SumType<T1, T2, T3, T4>(T1 val) { return new SumType<T1, T2, T3, T4>(val); }
        public static implicit operator SumType<T1, T2, T3, T4>(T2 val) { return new SumType<T1, T2, T3, T4>(val); }
        public static implicit operator SumType<T1, T2, T3, T4>(T3 val) { return new SumType<T1, T2, T3, T4>(val); }
        public static implicit operator SumType<T1, T2, T3, T4>(SumType<T1, T2> sum)
        {
            throw new NotImplementedException();
        }
        public static implicit operator SumType<T1, T2, T3, T4>(SumType<T1, T2, T3> sum)
        {
            throw new NotImplementedException();
        }
        public static explicit operator SumType<T1, T2>(SumType<T1, T2, T3, T4> sum)
        {
            if (sum.Value is T1 obj1)
                return (SumType<T1, T2>)obj1;
            if (sum.Value is T2 obj2)
                return (SumType<T1, T2>)obj2;
            throw new InvalidCastException();
        }
        public static explicit operator SumType<T1, T2, T3>(SumType<T1, T2, T3, T4> sum)
        {
            if (sum.Value is T1 obj1)
                return (SumType<T1, T2, T3>)obj1;
            if (sum.Value is T2 obj2)
                return (SumType<T1, T2, T3>)obj2;
            if (sum.Value is T3 obj3)
                return (SumType<T1, T2, T3>)obj3;
            throw new InvalidCastException();
        }
        public static explicit operator T1(SumType<T1, T2, T3, T4> sum)
        {
            if (sum.Value is T1 obj)
                return obj;
            throw new InvalidCastException();
        }
        public static explicit operator T2(SumType<T1, T2, T3, T4> sum)
        {
            if (sum.Value is T2 obj)
                return obj;
            throw new InvalidCastException();
        }
        public static explicit operator T3(SumType<T1, T2, T3, T4> sum)
        {
            if (sum.Value is T3 obj)
                return obj;
            throw new InvalidCastException();
        }
        public static explicit operator T4(SumType<T1, T2, T3, T4> sum)
        {
            if (sum.Value is T4 obj)
                return obj;
            throw new InvalidCastException();
        }
    }
}
