using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using CharSequence = System.String;

/*
 * Licensed to the Apache Software Foundation (ASF) under one or more
 * contributor license agreements.  See the NOTICE file distributed with
 * this work for additional information regarding copyright ownership.
 * The ASF licenses this file to You under the Apache License, Version 2.0
 * (the "License"); you may not use this file except in compliance with
 * the License.  You may obtain a copy of the License at
 *
 *      http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */
namespace org.antlr.codebuff.misc
{


    /// <summary>
    /// <para>Operations on <seealso cref="java.lang.String"/> that are
    /// {@code null} safe.</para>
    /// 
    /// <ul>
    ///  <li><b>IsEmpty/IsBlank</b>
    ///      - checks if a String contains text</li>
    ///  <li><b>Trim/Strip</b>
    ///      - removes leading and trailing whitespace</li>
    ///  <li><b>Equals/Compare</b>
    ///      - compares two strings null-safe</li>
    ///  <li><b>startsWith</b>
    ///      - check if a String starts with a prefix null-safe</li>
    ///  <li><b>endsWith</b>
    ///      - check if a String ends with a suffix null-safe</li>
    ///  <li><b>IndexOf/LastIndexOf/Contains</b>
    ///      - null-safe index-of checks
    ///  <li><b>IndexOfAny/LastIndexOfAny/IndexOfAnyBut/LastIndexOfAnyBut</b>
    ///      - index-of any of a set of Strings</li>
    ///  <li><b>ContainsOnly/ContainsNone/ContainsAny</b>
    ///      - does String contains only/none/any of these characters</li>
    ///  <li><b>Substring/Left/Right/Mid</b>
    ///      - null-safe substring extractions</li>
    ///  <li><b>SubstringBefore/SubstringAfter/SubstringBetween</b>
    ///      - substring extraction relative to other strings</li>
    ///  <li><b>Split/Join</b>
    ///      - splits a String into an array of substrings and vice versa</li>
    ///  <li><b>Remove/Delete</b>
    ///      - removes part of a String</li>
    ///  <li><b>Replace/Overlay</b>
    ///      - Searches a String and replaces one String with another</li>
    ///  <li><b>Chomp/Chop</b>
    ///      - removes the last part of a String</li>
    ///  <li><b>AppendIfMissing</b>
    ///      - appends a suffix to the end of the String if not present</li>
    ///  <li><b>PrependIfMissing</b>
    ///      - prepends a prefix to the start of the String if not present</li>
    ///  <li><b>LeftPad/RightPad/Center/Repeat</b>
    ///      - pads a String</li>
    ///  <li><b>UpperCase/LowerCase/SwapCase/Capitalize/Uncapitalize</b>
    ///      - changes the case of a String</li>
    ///  <li><b>CountMatches</b>
    ///      - counts the number of occurrences of one String in another</li>
    ///  <li><b>IsAlpha/IsNumeric/IsWhitespace/IsAsciiPrintable</b>
    ///      - checks the characters in a String</li>
    ///  <li><b>DefaultString</b>
    ///      - protects against a null input String</li>
    ///  <li><b>Rotate</b>
    ///      - rotate (circular shift) a String</li>
    ///  <li><b>Reverse/ReverseDelimited</b>
    ///      - reverses a String</li>
    ///  <li><b>Abbreviate</b>
    ///      - abbreviates a string using ellipsis or another given String</li>
    ///  <li><b>Difference</b>
    ///      - compares Strings and reports on their differences</li>
    ///  <li><b>LevenshteinDistance</b>
    ///      - the number of changes needed to change one String into another</li>
    /// </ul>
    /// 
    /// <para>The {@code StringUtils} class defines certain words related to
    /// String handling.</para>
    /// 
    /// <ul>
    ///  <li>null - {@code null}</li>
    ///  <li>empty - a zero-length string ({@code ""})</li>
    ///  <li>space - the space character ({@code ' '}, char 32)</li>
    ///  <li>whitespace - the characters defined by <seealso cref="Character#isWhitespace(char)"/></li>
    ///  <li>trim - the characters &lt;= 32 as in <seealso cref="String#trim()"/></li>
    /// </ul>
    /// 
    /// <para>{@code StringUtils} handles {@code null} input Strings quietly.
    /// That is to say that a {@code null} input will return {@code null}.
    /// Where a {@code boolean} or {@code int} is being returned
    /// details vary by method.</para>
    /// 
    /// <para>A side effect of the {@code null} handling is that a
    /// {@code NullPointerException} should be considered a bug in
    /// {@code StringUtils}.</para>
    /// 
    /// <para>Methods in this class give sample code to explain their operation.
    /// The symbol {@code *} is used to indicate any input including {@code null}.</para>
    /// 
    /// <para>#ThreadSafe#</para> </summary>
    /// <seealso cref= java.lang.String
    /// @since 1.0 </seealso>
    //@Immutable
    public class StringUtils
    {
        // Performance testing notes (JDK 1.4, Jul03, scolebourne)
        // Whitespace:
        // Character.isWhitespace() is faster than WHITESPACE.indexOf()
        // where WHITESPACE is a string of all whitespace characters
        //
        // Character access:
        // String.charAt(n) versus toCharArray(), then array[n]
        // String.charAt(n) is about 15% worse for a 10K string
        // They are about equal for a length 50 string
        // String.charAt(n) is about 4 times better for a length 3 string
        // String.charAt(n) is best bet overall
        //
        // Append:
        // String.concat about twice as fast as StringBuffer.append
        // (not sure who tested this)

        /// <summary>
        /// A String for a space character.
        /// 
        /// @since 3.2
        /// </summary>
        public const string SPACE = " ";

        /// <summary>
        /// The empty String {@code ""}.
        /// @since 2.0
        /// </summary>
        public const string EMPTY = "";

        /// <summary>
        /// A String for linefeed LF ("\n").
        /// </summary>
        /// <seealso cref= <a href="http://docs.oracle.com/javase/specs/jls/se7/html/jls-3.html#jls-3.10.6">JLF: Escape Sequences
        ///      for Character and String Literals</a>
        /// @since 3.2 </seealso>
        public const string LF = "\n";

        /// <summary>
        /// A String for carriage return CR ("\r").
        /// </summary>
        /// <seealso cref= <a href="http://docs.oracle.com/javase/specs/jls/se7/html/jls-3.html#jls-3.10.6">JLF: Escape Sequences
        ///      for Character and String Literals</a>
        /// @since 3.2 </seealso>
        public const string CR = "\r";

        /// <summary>
        /// Represents a failed index search.
        /// @since 2.1
        /// </summary>
        public const int INDEX_NOT_FOUND = -1;

        /// <summary>
        /// <para>The maximum size to which the padding constant(s) can expand.</para>
        /// </summary>
        private const int PAD_LIMIT = 8192;

        /// <summary>
        /// <para>{@code StringUtils} instances should NOT be constructed in
        /// standard programming. Instead, the class should be used as
        /// {@code StringUtils.trim(" foo ");}.</para>
        /// 
        /// <para>This constructor is public to permit tools that require a JavaBean
        /// instance to operate.</para>
        /// </summary>
        public StringUtils() : base()
        {
        }

        // Empty checks
        //-----------------------------------------------------------------------
        /// <summary>
        /// <para>Checks if a CharSequence is empty ("") or null.</para>
        /// 
        /// <pre>
        /// StringUtils.isEmpty(null)      = true
        /// StringUtils.isEmpty("")        = true
        /// StringUtils.isEmpty(" ")       = false
        /// StringUtils.isEmpty("bob")     = false
        /// StringUtils.isEmpty("  bob  ") = false
        /// </pre>
        /// 
        /// <para>NOTE: This method changed in Lang version 2.0.
        /// It no longer trims the CharSequence.
        /// That functionality is available in isBlank().</para>
        /// </summary>
        /// <param name="cs">  the CharSequence to check, may be null </param>
        /// <returns> {@code true} if the CharSequence is empty or null
        /// @since 3.0 Changed signature from isEmpty(String) to isEmpty(CharSequence) </returns>
        //JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
        //ORIGINAL LINE: public static boolean isEmpty(final CharSequence cs)
        public static bool isEmpty(CharSequence cs)
        {
            return cs == null || cs.Length == 0;
        }

        /// <summary>
        /// <para>Checks if a CharSequence is not empty ("") and not null.</para>
        /// 
        /// <pre>
        /// StringUtils.isNotEmpty(null)      = false
        /// StringUtils.isNotEmpty("")        = false
        /// StringUtils.isNotEmpty(" ")       = true
        /// StringUtils.isNotEmpty("bob")     = true
        /// StringUtils.isNotEmpty("  bob  ") = true
        /// </pre>
        /// </summary>
        /// <param name="cs">  the CharSequence to check, may be null </param>
        /// <returns> {@code true} if the CharSequence is not empty and not null
        /// @since 3.0 Changed signature from isNotEmpty(String) to isNotEmpty(CharSequence) </returns>
        //JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
        //ORIGINAL LINE: public static boolean isNotEmpty(final CharSequence cs)
        public static bool isNotEmpty(CharSequence cs)
        {
            return !isEmpty(cs);
        }

        ///// <summary>
        ///// <para>Checks if any of the CharSequences are empty ("") or null.</para>
        ///// 
        ///// <pre>
        ///// StringUtils.isAnyEmpty(null)             = true
        ///// StringUtils.isAnyEmpty(null, "foo")      = true
        ///// StringUtils.isAnyEmpty("", "bar")        = true
        ///// StringUtils.isAnyEmpty("bob", "")        = true
        ///// StringUtils.isAnyEmpty("  bob  ", null)  = true
        ///// StringUtils.isAnyEmpty(" ", "bar")       = false
        ///// StringUtils.isAnyEmpty("foo", "bar")     = false
        ///// </pre>
        ///// </summary>
        ///// <param name="css">  the CharSequences to check, may be null or empty </param>
        ///// <returns> {@code true} if any of the CharSequences are empty or null
        ///// @since 3.2 </returns>
        ////JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
        ////ORIGINAL LINE: public static boolean isAnyEmpty(final CharSequence... css)
        //public static bool isAnyEmpty(params CharSequence[] css)
        //{
        //    if (ArrayUtils.isEmpty(css))
        //    {
        //        return false;
        //    }
        //    foreach (CharSequence cs in css)
        //    {
        //        if (isEmpty(cs))
        //        {
        //            return true;
        //        }
        //    }
        //    return false;
        //}

        ///// <summary>
        ///// <para>Checks if any of the CharSequences are not empty ("") and not null.</para>
        ///// 
        ///// <pre>
        ///// StringUtils.isAnyNotEmpty(null)             = false
        ///// StringUtils.isAnyNotEmpty(new String[] {})  = false
        ///// StringUtils.isAnyNotEmpty(null, "foo")      = true
        ///// StringUtils.isAnyNotEmpty("", "bar")        = true
        ///// StringUtils.isAnyNotEmpty("bob", "")        = true
        ///// StringUtils.isAnyNotEmpty("  bob  ", null)  = true
        ///// StringUtils.isAnyNotEmpty(" ", "bar")       = true
        ///// StringUtils.isAnyNotEmpty("foo", "bar")     = true
        ///// </pre>
        ///// </summary>
        ///// <param name="css">  the CharSequences to check, may be null or empty </param>
        ///// <returns> {@code true} if any of the CharSequences are not empty and not null
        ///// @since 3.6 </returns>
        ////JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
        ////ORIGINAL LINE: public static boolean isAnyNotEmpty(final CharSequence... css)
        //public static bool isAnyNotEmpty(params CharSequence[] css)
        //{
        //    if (ArrayUtils.isEmpty(css))
        //    {
        //        return false;
        //    }
        //    foreach (CharSequence cs in css)
        //    {
        //        if (isNotEmpty(cs))
        //        {
        //            return true;
        //        }
        //    }
        //    return false;
        //}

        ///// <summary>
        ///// <para>Checks if none of the CharSequences are empty ("") or null.</para>
        ///// 
        ///// <pre>
        ///// StringUtils.isNoneEmpty(null)             = false
        ///// StringUtils.isNoneEmpty(null, "foo")      = false
        ///// StringUtils.isNoneEmpty("", "bar")        = false
        ///// StringUtils.isNoneEmpty("bob", "")        = false
        ///// StringUtils.isNoneEmpty("  bob  ", null)  = false
        ///// StringUtils.isNoneEmpty(new String[] {})  = false
        ///// StringUtils.isNoneEmpty(" ", "bar")       = true
        ///// StringUtils.isNoneEmpty("foo", "bar")     = true
        ///// </pre>
        ///// </summary>
        ///// <param name="css">  the CharSequences to check, may be null or empty </param>
        ///// <returns> {@code true} if none of the CharSequences are empty or null
        ///// @since 3.2 </returns>
        ////JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
        ////ORIGINAL LINE: public static boolean isNoneEmpty(final CharSequence... css)
        //public static bool isNoneEmpty(params CharSequence[] css)
        //{
        //    return !isAnyEmpty(css);
        //}

        //      /// <summary>
        //      /// <para>Checks if a CharSequence is empty (""), null or whitespace only.</para>
        //      /// 
        //      /// </p>Whitespace is defined by <seealso cref="Character#isWhitespace(char)"/>.</p>
        //      /// 
        //      /// <pre>
        //      /// StringUtils.isBlank(null)      = true
        //      /// StringUtils.isBlank("")        = true
        //      /// StringUtils.isBlank(" ")       = true
        //      /// StringUtils.isBlank("bob")     = false
        //      /// StringUtils.isBlank("  bob  ") = false
        //      /// </pre>
        //      /// </summary>
        //      /// <param name="cs">  the CharSequence to check, may be null </param>
        //      /// <returns> {@code true} if the CharSequence is null, empty or whitespace only
        //      /// @since 2.0
        //      /// @since 3.0 Changed signature from isBlank(String) to isBlank(CharSequence) </returns>
        //      //JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
        //      //ORIGINAL LINE: public static boolean isBlank(final CharSequence cs)
        //      public static bool isBlank(CharSequence cs)
        //      {
        //          int strLen;
        //          if (cs == null || (strLen = cs.length()) == 0)
        //          {
        //              return true;
        //          }
        //          for (int i = 0; i < strLen; i++)
        //          {
        //              if (char.IsWhiteSpace(cs.charAt(i)) == false)
        //              {
        //                  return false;
        //              }
        //          }
        //          return true;
        //      }

        //      /// <summary>
        //      /// <para>Checks if a CharSequence is not empty (""), not null and not whitespace only.</para>
        //      /// 
        //      /// </p>Whitespace is defined by <seealso cref="Character#isWhitespace(char)"/>.</p>
        //      /// 
        //      /// <pre>
        //      /// StringUtils.isNotBlank(null)      = false
        //      /// StringUtils.isNotBlank("")        = false
        //      /// StringUtils.isNotBlank(" ")       = false
        //      /// StringUtils.isNotBlank("bob")     = true
        //      /// StringUtils.isNotBlank("  bob  ") = true
        //      /// </pre>
        //      /// </summary>
        //      /// <param name="cs">  the CharSequence to check, may be null </param>
        //      /// <returns> {@code true} if the CharSequence is
        //      ///  not empty and not null and not whitespace only
        //      /// @since 2.0
        //      /// @since 3.0 Changed signature from isNotBlank(String) to isNotBlank(CharSequence) </returns>
        //      //JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
        //      //ORIGINAL LINE: public static boolean isNotBlank(final CharSequence cs)
        //      public static bool isNotBlank(CharSequence cs)
        //      {
        //          return !isBlank(cs);
        //      }

        //      /// <summary>
        //      /// <para>Checks if any of the CharSequences are empty ("") or null or whitespace only.</para>
        //      /// 
        //      /// </p>Whitespace is defined by <seealso cref="Character#isWhitespace(char)"/>.</p>
        //      /// 
        //      /// <pre>
        //      /// StringUtils.isAnyBlank(null)             = true
        //      /// StringUtils.isAnyBlank(null, "foo")      = true
        //      /// StringUtils.isAnyBlank(null, null)       = true
        //      /// StringUtils.isAnyBlank("", "bar")        = true
        //      /// StringUtils.isAnyBlank("bob", "")        = true
        //      /// StringUtils.isAnyBlank("  bob  ", null)  = true
        //      /// StringUtils.isAnyBlank(" ", "bar")       = true
        //      /// StringUtils.isAnyBlank(new String[] {})  = false
        //      /// StringUtils.isAnyBlank("foo", "bar")     = false
        //      /// </pre>
        //      /// </summary>
        //      /// <param name="css">  the CharSequences to check, may be null or empty </param>
        //      /// <returns> {@code true} if any of the CharSequences are empty or null or whitespace only
        //      /// @since 3.2 </returns>
        //      //JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
        //      //ORIGINAL LINE: public static boolean isAnyBlank(final CharSequence... css)
        //      public static bool isAnyBlank(params CharSequence[] css)
        //      {
        //          if (ArrayUtils.isEmpty(css))
        //          {
        //              return false;
        //          }
        //          foreach (CharSequence cs in css)
        //          {
        //              if (isBlank(cs))
        //              {
        //                  return true;
        //              }
        //          }
        //          return false;
        //      }

        //      /// <summary>
        //      /// <para>Checks if any of the CharSequences are not empty (""), not null and not whitespace only.</para>
        //      /// 
        //      /// </p>Whitespace is defined by <seealso cref="Character#isWhitespace(char)"/>.</p>
        //      /// 
        //      /// <pre>
        //      /// StringUtils.isAnyNotBlank(null)             = false
        //      /// StringUtils.isAnyNotBlank(null, "foo")      = true
        //      /// StringUtils.isAnyNotBlank(null, null)       = false
        //      /// StringUtils.isAnyNotBlank("", "bar")        = true
        //      /// StringUtils.isAnyNotBlank("bob", "")        = true
        //      /// StringUtils.isAnyNotBlank("  bob  ", null)  = true
        //      /// StringUtils.isAnyNotBlank(" ", "bar")       = true
        //      /// StringUtils.isAnyNotBlank("foo", "bar")     = true
        //      /// StringUtils.isAnyNotBlank(new String[] {})  = false
        //      /// </pre>
        //      /// </summary>
        //      /// <param name="css">  the CharSequences to check, may be null or empty </param>
        //      /// <returns> {@code true} if any of the CharSequences are not empty and not null and not whitespace only
        //      /// @since 3.6 </returns>
        //      //JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
        //      //ORIGINAL LINE: public static boolean isAnyNotBlank(final CharSequence... css)
        //      public static bool isAnyNotBlank(params CharSequence[] css)
        //      {
        //          if (ArrayUtils.isEmpty(css))
        //          {
        //              return false;
        //          }
        //          foreach (CharSequence cs in css)
        //          {
        //              if (isNotBlank(cs))
        //              {
        //                  return true;
        //              }
        //          }
        //          return false;
        //      }

        //      /// <summary>
        //      /// <para>Checks if none of the CharSequences are empty (""), null or whitespace only.</para>
        //      /// 
        //      /// </p>Whitespace is defined by <seealso cref="Character#isWhitespace(char)"/>.</p>
        //      /// 
        //      /// <pre>
        //      /// StringUtils.isNoneBlank(null)             = false
        //      /// StringUtils.isNoneBlank(null, "foo")      = false
        //      /// StringUtils.isNoneBlank(null, null)       = false
        //      /// StringUtils.isNoneBlank("", "bar")        = false
        //      /// StringUtils.isNoneBlank("bob", "")        = false
        //      /// StringUtils.isNoneBlank("  bob  ", null)  = false
        //      /// StringUtils.isNoneBlank(" ", "bar")       = false
        //      /// StringUtils.isNoneBlank(new String[] {})  = false
        //      /// StringUtils.isNoneBlank("foo", "bar")     = true
        //      /// </pre>
        //      /// </summary>
        //      /// <param name="css">  the CharSequences to check, may be null or empty </param>
        //      /// <returns> {@code true} if none of the CharSequences are empty or null or whitespace only
        //      /// @since 3.2 </returns>
        //      //JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
        //      //ORIGINAL LINE: public static boolean isNoneBlank(final CharSequence... css)
        //      public static bool isNoneBlank(params CharSequence[] css)
        //      {
        //          return !isAnyBlank(css);
        //      }

        //      // Trim
        //      //-----------------------------------------------------------------------
        //      /// <summary>
        //      /// <para>Removes control characters (char &lt;= 32) from both
        //      /// ends of this String, handling {@code null} by returning
        //      /// {@code null}.</para>
        //      /// 
        //      /// <para>The String is trimmed using <seealso cref="String#trim()"/>.
        //      /// Trim removes start and end characters &lt;= 32.
        //      /// To strip whitespace use <seealso cref="#strip(String)"/>.</para>
        //      /// 
        //      /// <para>To trim your choice of characters, use the
        //      /// <seealso cref="#strip(String, String)"/> methods.</para>
        //      /// 
        //      /// <pre>
        //      /// StringUtils.trim(null)          = null
        //      /// StringUtils.trim("")            = ""
        //      /// StringUtils.trim("     ")       = ""
        //      /// StringUtils.trim("abc")         = "abc"
        //      /// StringUtils.trim("    abc    ") = "abc"
        //      /// </pre>
        //      /// </summary>
        //      /// <param name="str">  the String to be trimmed, may be null </param>
        //      /// <returns> the trimmed string, {@code null} if null String input </returns>
        //      //JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
        //      //ORIGINAL LINE: public static String trim(final String str)
        //      public static string trim(string str)
        //      {
        //          return string.ReferenceEquals(str, null) ? null : str.Trim();
        //      }

        //      /// <summary>
        //      /// <para>Removes control characters (char &lt;= 32) from both
        //      /// ends of this String returning {@code null} if the String is
        //      /// empty ("") after the trim or if it is {@code null}.
        //      /// 
        //      /// </para>
        //      /// <para>The String is trimmed using <seealso cref="String#trim()"/>.
        //      /// Trim removes start and end characters &lt;= 32.
        //      /// To strip whitespace use <seealso cref="#stripToNull(String)"/>.</para>
        //      /// 
        //      /// <pre>
        //      /// StringUtils.trimToNull(null)          = null
        //      /// StringUtils.trimToNull("")            = null
        //      /// StringUtils.trimToNull("     ")       = null
        //      /// StringUtils.trimToNull("abc")         = "abc"
        //      /// StringUtils.trimToNull("    abc    ") = "abc"
        //      /// </pre>
        //      /// </summary>
        //      /// <param name="str">  the String to be trimmed, may be null </param>
        //      /// <returns> the trimmed String,
        //      ///  {@code null} if only chars &lt;= 32, empty or null String input
        //      /// @since 2.0 </returns>
        //      //JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
        //      //ORIGINAL LINE: public static String trimToNull(final String str)
        //      public static string trimToNull(string str)
        //      {
        //          //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
        //          //ORIGINAL LINE: final String ts = trim(str);
        //          string ts = trim(str);
        //          return isEmpty(ts) ? null : ts;
        //      }

        //      /// <summary>
        //      /// <para>Removes control characters (char &lt;= 32) from both
        //      /// ends of this String returning an empty String ("") if the String
        //      /// is empty ("") after the trim or if it is {@code null}.
        //      /// 
        //      /// </para>
        //      /// <para>The String is trimmed using <seealso cref="String#trim()"/>.
        //      /// Trim removes start and end characters &lt;= 32.
        //      /// To strip whitespace use <seealso cref="#stripToEmpty(String)"/>.</para>
        //      /// 
        //      /// <pre>
        //      /// StringUtils.trimToEmpty(null)          = ""
        //      /// StringUtils.trimToEmpty("")            = ""
        //      /// StringUtils.trimToEmpty("     ")       = ""
        //      /// StringUtils.trimToEmpty("abc")         = "abc"
        //      /// StringUtils.trimToEmpty("    abc    ") = "abc"
        //      /// </pre>
        //      /// </summary>
        //      /// <param name="str">  the String to be trimmed, may be null </param>
        //      /// <returns> the trimmed String, or an empty String if {@code null} input
        //      /// @since 2.0 </returns>
        //      //JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
        //      //ORIGINAL LINE: public static String trimToEmpty(final String str)
        //      public static string trimToEmpty(string str)
        //      {
        //          return string.ReferenceEquals(str, null) ? EMPTY : str.Trim();
        //      }

        //      /// <summary>
        //      /// <para>Truncates a String. This will turn
        //      /// "Now is the time for all good men" into "Now is the time for".</para>
        //      /// 
        //      /// <para>Specifically:</para>
        //      /// <ul>
        //      ///   <li>If {@code str} is less than {@code maxWidth} characters
        //      ///       long, return it.</li>
        //      ///   <li>Else truncate it to {@code substring(str, 0, maxWidth)}.</li>
        //      ///   <li>If {@code maxWidth} is less than {@code 0}, throw an
        //      ///       {@code IllegalArgumentException}.</li>
        //      ///   <li>In no case will it return a String of length greater than
        //      ///       {@code maxWidth}.</li>
        //      /// </ul>
        //      /// 
        //      /// <pre>
        //      /// StringUtils.truncate(null, 0)       = null
        //      /// StringUtils.truncate(null, 2)       = null
        //      /// StringUtils.truncate("", 4)         = ""
        //      /// StringUtils.truncate("abcdefg", 4)  = "abcd"
        //      /// StringUtils.truncate("abcdefg", 6)  = "abcdef"
        //      /// StringUtils.truncate("abcdefg", 7)  = "abcdefg"
        //      /// StringUtils.truncate("abcdefg", 8)  = "abcdefg"
        //      /// StringUtils.truncate("abcdefg", -1) = throws an IllegalArgumentException
        //      /// </pre>
        //      /// </summary>
        //      /// <param name="str">  the String to truncate, may be null </param>
        //      /// <param name="maxWidth">  maximum length of result String, must be positive </param>
        //      /// <returns> truncated String, {@code null} if null String input
        //      /// @since 3.5 </returns>
        //      //JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
        //      //ORIGINAL LINE: public static String truncate(final String str, final int maxWidth)
        //      public static string truncate(string str, int maxWidth)
        //      {
        //          return truncate(str, 0, maxWidth);
        //      }

        //      /// <summary>
        //      /// <para>Truncates a String. This will turn
        //      /// "Now is the time for all good men" into "is the time for all".</para>
        //      /// 
        //      /// <para>Works like {@code truncate(String, int)}, but allows you to specify
        //      /// a "left edge" offset.
        //      /// 
        //      /// </para>
        //      /// <para>Specifically:</para>
        //      /// <ul>
        //      ///   <li>If {@code str} is less than {@code maxWidth} characters
        //      ///       long, return it.</li>
        //      ///   <li>Else truncate it to {@code substring(str, offset, maxWidth)}.</li>
        //      ///   <li>If {@code maxWidth} is less than {@code 0}, throw an
        //      ///       {@code IllegalArgumentException}.</li>
        //      ///   <li>If {@code offset} is less than {@code 0}, throw an
        //      ///       {@code IllegalArgumentException}.</li>
        //      ///   <li>In no case will it return a String of length greater than
        //      ///       {@code maxWidth}.</li>
        //      /// </ul>
        //      /// 
        //      /// <pre>
        //      /// StringUtils.truncate(null, 0, 0) = null
        //      /// StringUtils.truncate(null, 2, 4) = null
        //      /// StringUtils.truncate("", 0, 10) = ""
        //      /// StringUtils.truncate("", 2, 10) = ""
        //      /// StringUtils.truncate("abcdefghij", 0, 3) = "abc"
        //      /// StringUtils.truncate("abcdefghij", 5, 6) = "fghij"
        //      /// StringUtils.truncate("raspberry peach", 10, 15) = "peach"
        //      /// StringUtils.truncate("abcdefghijklmno", 0, 10) = "abcdefghij"
        //      /// StringUtils.truncate("abcdefghijklmno", -1, 10) = throws an IllegalArgumentException
        //      /// StringUtils.truncate("abcdefghijklmno", Integer.MIN_VALUE, 10) = "abcdefghij"
        //      /// StringUtils.truncate("abcdefghijklmno", Integer.MIN_VALUE, Integer.MAX_VALUE) = "abcdefghijklmno"
        //      /// StringUtils.truncate("abcdefghijklmno", 0, Integer.MAX_VALUE) = "abcdefghijklmno"
        //      /// StringUtils.truncate("abcdefghijklmno", 1, 10) = "bcdefghijk"
        //      /// StringUtils.truncate("abcdefghijklmno", 2, 10) = "cdefghijkl"
        //      /// StringUtils.truncate("abcdefghijklmno", 3, 10) = "defghijklm"
        //      /// StringUtils.truncate("abcdefghijklmno", 4, 10) = "efghijklmn"
        //      /// StringUtils.truncate("abcdefghijklmno", 5, 10) = "fghijklmno"
        //      /// StringUtils.truncate("abcdefghijklmno", 5, 5) = "fghij"
        //      /// StringUtils.truncate("abcdefghijklmno", 5, 3) = "fgh"
        //      /// StringUtils.truncate("abcdefghijklmno", 10, 3) = "klm"
        //      /// StringUtils.truncate("abcdefghijklmno", 10, Integer.MAX_VALUE) = "klmno"
        //      /// StringUtils.truncate("abcdefghijklmno", 13, 1) = "n"
        //      /// StringUtils.truncate("abcdefghijklmno", 13, Integer.MAX_VALUE) = "no"
        //      /// StringUtils.truncate("abcdefghijklmno", 14, 1) = "o"
        //      /// StringUtils.truncate("abcdefghijklmno", 14, Integer.MAX_VALUE) = "o"
        //      /// StringUtils.truncate("abcdefghijklmno", 15, 1) = ""
        //      /// StringUtils.truncate("abcdefghijklmno", 15, Integer.MAX_VALUE) = ""
        //      /// StringUtils.truncate("abcdefghijklmno", Integer.MAX_VALUE, Integer.MAX_VALUE) = ""
        //      /// StringUtils.truncate("abcdefghij", 3, -1) = throws an IllegalArgumentException
        //      /// StringUtils.truncate("abcdefghij", -2, 4) = throws an IllegalArgumentException
        //      /// </pre>
        //      /// </summary>
        //      /// <param name="str">  the String to check, may be null </param>
        //      /// <param name="offset">  left edge of source String </param>
        //      /// <param name="maxWidth">  maximum length of result String, must be positive </param>
        //      /// <returns> truncated String, {@code null} if null String input
        //      /// @since 3.5 </returns>
        //      //JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
        //      //ORIGINAL LINE: public static String truncate(final String str, final int offset, final int maxWidth)
        //      public static string truncate(string str, int offset, int maxWidth)
        //      {
        //          if (offset < 0)
        //          {
        //              throw new System.ArgumentException("offset cannot be negative");
        //          }
        //          if (maxWidth < 0)
        //          {
        //              throw new System.ArgumentException("maxWith cannot be negative");
        //          }
        //          if (string.ReferenceEquals(str, null))
        //          {
        //              return null;
        //          }
        //          if (offset > str.Length)
        //          {
        //              return EMPTY;
        //          }
        //          if (str.Length > maxWidth)
        //          {
        //              //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
        //              //ORIGINAL LINE: final int ix = offset + maxWidth > str.length() ? str.length() : offset + maxWidth;
        //              int ix = offset + maxWidth > str.Length ? str.Length : offset + maxWidth;
        //              return str.Substring(offset, ix - offset);
        //          }
        //          return str.Substring(offset);
        //      }

        //      // Stripping
        //      //-----------------------------------------------------------------------
        //      /// <summary>
        //      /// <para>Strips whitespace from the start and end of a String.</para>
        //      /// 
        //      /// <para>This is similar to <seealso cref="#trim(String)"/> but removes whitespace.
        //      /// Whitespace is defined by <seealso cref="Character#isWhitespace(char)"/>.</para>
        //      /// 
        //      /// <para>A {@code null} input String returns {@code null}.</para>
        //      /// 
        //      /// <pre>
        //      /// StringUtils.strip(null)     = null
        //      /// StringUtils.strip("")       = ""
        //      /// StringUtils.strip("   ")    = ""
        //      /// StringUtils.strip("abc")    = "abc"
        //      /// StringUtils.strip("  abc")  = "abc"
        //      /// StringUtils.strip("abc  ")  = "abc"
        //      /// StringUtils.strip(" abc ")  = "abc"
        //      /// StringUtils.strip(" ab c ") = "ab c"
        //      /// </pre>
        //      /// </summary>
        //      /// <param name="str">  the String to remove whitespace from, may be null </param>
        //      /// <returns> the stripped String, {@code null} if null String input </returns>
        //      //JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
        //      //ORIGINAL LINE: public static String strip(final String str)
        //      public static string strip(string str)
        //      {
        //          return strip(str, null);
        //      }

        //      /// <summary>
        //      /// <para>Strips whitespace from the start and end of a String  returning
        //      /// {@code null} if the String is empty ("") after the strip.</para>
        //      /// 
        //      /// <para>This is similar to <seealso cref="#trimToNull(String)"/> but removes whitespace.
        //      /// Whitespace is defined by <seealso cref="Character#isWhitespace(char)"/>.</para>
        //      /// 
        //      /// <pre>
        //      /// StringUtils.stripToNull(null)     = null
        //      /// StringUtils.stripToNull("")       = null
        //      /// StringUtils.stripToNull("   ")    = null
        //      /// StringUtils.stripToNull("abc")    = "abc"
        //      /// StringUtils.stripToNull("  abc")  = "abc"
        //      /// StringUtils.stripToNull("abc  ")  = "abc"
        //      /// StringUtils.stripToNull(" abc ")  = "abc"
        //      /// StringUtils.stripToNull(" ab c ") = "ab c"
        //      /// </pre>
        //      /// </summary>
        //      /// <param name="str">  the String to be stripped, may be null </param>
        //      /// <returns> the stripped String,
        //      ///  {@code null} if whitespace, empty or null String input
        //      /// @since 2.0 </returns>
        //      public static string stripToNull(string str)
        //      {
        //          if (string.ReferenceEquals(str, null))
        //          {
        //              return null;
        //          }
        //          str = strip(str, null);
        //          return str.Length == 0 ? null : str;
        //      }

        //      /// <summary>
        //      /// <para>Strips whitespace from the start and end of a String  returning
        //      /// an empty String if {@code null} input.</para>
        //      /// 
        //      /// <para>This is similar to <seealso cref="#trimToEmpty(String)"/> but removes whitespace.
        //      /// Whitespace is defined by <seealso cref="Character#isWhitespace(char)"/>.</para>
        //      /// 
        //      /// <pre>
        //      /// StringUtils.stripToEmpty(null)     = ""
        //      /// StringUtils.stripToEmpty("")       = ""
        //      /// StringUtils.stripToEmpty("   ")    = ""
        //      /// StringUtils.stripToEmpty("abc")    = "abc"
        //      /// StringUtils.stripToEmpty("  abc")  = "abc"
        //      /// StringUtils.stripToEmpty("abc  ")  = "abc"
        //      /// StringUtils.stripToEmpty(" abc ")  = "abc"
        //      /// StringUtils.stripToEmpty(" ab c ") = "ab c"
        //      /// </pre>
        //      /// </summary>
        //      /// <param name="str">  the String to be stripped, may be null </param>
        //      /// <returns> the trimmed String, or an empty String if {@code null} input
        //      /// @since 2.0 </returns>
        //      //JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
        //      //ORIGINAL LINE: public static String stripToEmpty(final String str)
        //      public static string stripToEmpty(string str)
        //      {
        //          return string.ReferenceEquals(str, null) ? EMPTY : strip(str, null);
        //      }

        //      /// <summary>
        //      /// <para>Strips any of a set of characters from the start and end of a String.
        //      /// This is similar to <seealso cref="String#trim()"/> but allows the characters
        //      /// to be stripped to be controlled.</para>
        //      /// 
        //      /// <para>A {@code null} input String returns {@code null}.
        //      /// An empty string ("") input returns the empty string.</para>
        //      /// 
        //      /// <para>If the stripChars String is {@code null}, whitespace is
        //      /// stripped as defined by <seealso cref="Character#isWhitespace(char)"/>.
        //      /// Alternatively use <seealso cref="#strip(String)"/>.</para>
        //      /// 
        //      /// <pre>
        //      /// StringUtils.strip(null, *)          = null
        //      /// StringUtils.strip("", *)            = ""
        //      /// StringUtils.strip("abc", null)      = "abc"
        //      /// StringUtils.strip("  abc", null)    = "abc"
        //      /// StringUtils.strip("abc  ", null)    = "abc"
        //      /// StringUtils.strip(" abc ", null)    = "abc"
        //      /// StringUtils.strip("  abcyx", "xyz") = "  abc"
        //      /// </pre>
        //      /// </summary>
        //      /// <param name="str">  the String to remove characters from, may be null </param>
        //      /// <param name="stripChars">  the characters to remove, null treated as whitespace </param>
        //      /// <returns> the stripped String, {@code null} if null String input </returns>
        //      //JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
        //      //ORIGINAL LINE: public static String strip(String str, final String stripChars)
        //      public static string strip(string str, string stripChars)
        //      {
        //          if (isEmpty(str))
        //          {
        //              return str;
        //          }
        //          str = stripStart(str, stripChars);
        //          return stripEnd(str, stripChars);
        //      }

        //      /// <summary>
        //      /// <para>Strips any of a set of characters from the start of a String.</para>
        //      /// 
        //      /// <para>A {@code null} input String returns {@code null}.
        //      /// An empty string ("") input returns the empty string.</para>
        //      /// 
        //      /// <para>If the stripChars String is {@code null}, whitespace is
        //      /// stripped as defined by <seealso cref="Character#isWhitespace(char)"/>.</para>
        //      /// 
        //      /// <pre>
        //      /// StringUtils.stripStart(null, *)          = null
        //      /// StringUtils.stripStart("", *)            = ""
        //      /// StringUtils.stripStart("abc", "")        = "abc"
        //      /// StringUtils.stripStart("abc", null)      = "abc"
        //      /// StringUtils.stripStart("  abc", null)    = "abc"
        //      /// StringUtils.stripStart("abc  ", null)    = "abc  "
        //      /// StringUtils.stripStart(" abc ", null)    = "abc "
        //      /// StringUtils.stripStart("yxabc  ", "xyz") = "abc  "
        //      /// </pre>
        //      /// </summary>
        //      /// <param name="str">  the String to remove characters from, may be null </param>
        //      /// <param name="stripChars">  the characters to remove, null treated as whitespace </param>
        //      /// <returns> the stripped String, {@code null} if null String input </returns>
        //      //JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
        //      //ORIGINAL LINE: public static String stripStart(final String str, final String stripChars)
        //      public static string stripStart(string str, string stripChars)
        //      {
        //          int strLen;
        //          if (string.ReferenceEquals(str, null) || (strLen = str.Length) == 0)
        //          {
        //              return str;
        //          }
        //          int start = 0;
        //          if (string.ReferenceEquals(stripChars, null))
        //          {
        //              while (start != strLen && char.IsWhiteSpace(str[start]))
        //              {
        //                  start++;
        //              }
        //          }
        //          else if (stripChars.Length == 0)
        //          {
        //              return str;
        //          }
        //          else
        //          {
        //              while (start != strLen && stripChars.IndexOf(str[start]) != INDEX_NOT_FOUND)
        //              {
        //                  start++;
        //              }
        //          }
        //          return str.Substring(start);
        //      }

        //      /// <summary>
        //      /// <para>Strips any of a set of characters from the end of a String.</para>
        //      /// 
        //      /// <para>A {@code null} input String returns {@code null}.
        //      /// An empty string ("") input returns the empty string.</para>
        //      /// 
        //      /// <para>If the stripChars String is {@code null}, whitespace is
        //      /// stripped as defined by <seealso cref="Character#isWhitespace(char)"/>.</para>
        //      /// 
        //      /// <pre>
        //      /// StringUtils.stripEnd(null, *)          = null
        //      /// StringUtils.stripEnd("", *)            = ""
        //      /// StringUtils.stripEnd("abc", "")        = "abc"
        //      /// StringUtils.stripEnd("abc", null)      = "abc"
        //      /// StringUtils.stripEnd("  abc", null)    = "  abc"
        //      /// StringUtils.stripEnd("abc  ", null)    = "abc"
        //      /// StringUtils.stripEnd(" abc ", null)    = " abc"
        //      /// StringUtils.stripEnd("  abcyx", "xyz") = "  abc"
        //      /// StringUtils.stripEnd("120.00", ".0")   = "12"
        //      /// </pre>
        //      /// </summary>
        //      /// <param name="str">  the String to remove characters from, may be null </param>
        //      /// <param name="stripChars">  the set of characters to remove, null treated as whitespace </param>
        //      /// <returns> the stripped String, {@code null} if null String input </returns>
        //      //JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
        //      //ORIGINAL LINE: public static String stripEnd(final String str, final String stripChars)
        //      public static string stripEnd(string str, string stripChars)
        //      {
        //          int end;
        //          if (string.ReferenceEquals(str, null) || (end = str.Length) == 0)
        //          {
        //              return str;
        //          }

        //          if (string.ReferenceEquals(stripChars, null))
        //          {
        //              while (end != 0 && char.IsWhiteSpace(str[end - 1]))
        //              {
        //                  end--;
        //              }
        //          }
        //          else if (stripChars.Length == 0)
        //          {
        //              return str;
        //          }
        //          else
        //          {
        //              while (end != 0 && stripChars.IndexOf(str[end - 1]) != INDEX_NOT_FOUND)
        //              {
        //                  end--;
        //              }
        //          }
        //          return str.Substring(0, end);
        //      }

        //      // StripAll
        //      //-----------------------------------------------------------------------
        //      /// <summary>
        //      /// <para>Strips whitespace from the start and end of every String in an array.
        //      /// Whitespace is defined by <seealso cref="Character#isWhitespace(char)"/>.</para>
        //      /// 
        //      /// <para>A new array is returned each time, except for length zero.
        //      /// A {@code null} array will return {@code null}.
        //      /// An empty array will return itself.
        //      /// A {@code null} array entry will be ignored.</para>
        //      /// 
        //      /// <pre>
        //      /// StringUtils.stripAll(null)             = null
        //      /// StringUtils.stripAll([])               = []
        //      /// StringUtils.stripAll(["abc", "  abc"]) = ["abc", "abc"]
        //      /// StringUtils.stripAll(["abc  ", null])  = ["abc", null]
        //      /// </pre>
        //      /// </summary>
        //      /// <param name="strs">  the array to remove whitespace from, may be null </param>
        //      /// <returns> the stripped Strings, {@code null} if null array input </returns>
        //      //JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
        //      //ORIGINAL LINE: public static String[] stripAll(final String... strs)
        //      public static string[] stripAll(params string[] strs)
        //      {
        //          return stripAll(strs, null);
        //      }

        //      /// <summary>
        //      /// <para>Strips any of a set of characters from the start and end of every
        //      /// String in an array.</para>
        //      /// <para>Whitespace is defined by <seealso cref="Character#isWhitespace(char)"/>.</para>
        //      /// 
        //      /// <para>A new array is returned each time, except for length zero.
        //      /// A {@code null} array will return {@code null}.
        //      /// An empty array will return itself.
        //      /// A {@code null} array entry will be ignored.
        //      /// A {@code null} stripChars will strip whitespace as defined by
        //      /// <seealso cref="Character#isWhitespace(char)"/>.</para>
        //      /// 
        //      /// <pre>
        //      /// StringUtils.stripAll(null, *)                = null
        //      /// StringUtils.stripAll([], *)                  = []
        //      /// StringUtils.stripAll(["abc", "  abc"], null) = ["abc", "abc"]
        //      /// StringUtils.stripAll(["abc  ", null], null)  = ["abc", null]
        //      /// StringUtils.stripAll(["abc  ", null], "yz")  = ["abc  ", null]
        //      /// StringUtils.stripAll(["yabcz", null], "yz")  = ["abc", null]
        //      /// </pre>
        //      /// </summary>
        //      /// <param name="strs">  the array to remove characters from, may be null </param>
        //      /// <param name="stripChars">  the characters to remove, null treated as whitespace </param>
        //      /// <returns> the stripped Strings, {@code null} if null array input </returns>
        //      //JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
        //      //ORIGINAL LINE: public static String[] stripAll(final String[] strs, final String stripChars)
        //      public static string[] stripAll(string[] strs, string stripChars)
        //      {
        //          int strsLen;
        //          if (strs == null || (strsLen = strs.Length) == 0)
        //          {
        //              return strs;
        //          }
        //          //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
        //          //ORIGINAL LINE: final String[] newArr = new String[strsLen];
        //          string[] newArr = new string[strsLen];
        //          for (int i = 0; i < strsLen; i++)
        //          {
        //              newArr[i] = strip(strs[i], stripChars);
        //          }
        //          return newArr;
        //      }

        //      /// <summary>
        //      /// <para>Removes diacritics (~= accents) from a string. The case will not be altered.</para>
        //      /// <para>For instance, '&agrave;' will be replaced by 'a'.</para>
        //      /// <para>Note that ligatures will be left as is.</para>
        //      /// 
        //      /// <pre>
        //      /// StringUtils.stripAccents(null)                = null
        //      /// StringUtils.stripAccents("")                  = ""
        //      /// StringUtils.stripAccents("control")           = "control"
        //      /// StringUtils.stripAccents("&eacute;clair")     = "eclair"
        //      /// </pre>
        //      /// </summary>
        //      /// <param name="input"> String to be stripped </param>
        //      /// <returns> input text with diacritics removed
        //      /// 
        //      /// @since 3.0 </returns>
        //      // See also Lucene's ASCIIFoldingFilter (Lucene 2.9) that replaces accented characters by their unaccented equivalent (and uncommitted bug fix: https://issues.apache.org/jira/browse/LUCENE-1343?focusedCommentId=12858907&page=com.atlassian.jira.plugin.system.issuetabpanels%3Acomment-tabpanel#action_12858907).
        //      //JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
        //      //ORIGINAL LINE: public static String stripAccents(final String input)
        //      public static string stripAccents(string input)
        //      {
        //          if (string.ReferenceEquals(input, null))
        //          {
        //              return null;
        //          }
        //          //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
        //          //ORIGINAL LINE: final java.util.regex.Pattern pattern = java.util.regex.Pattern.compile("\\p{InCombiningDiacriticalMarks}+");
        //          Pattern pattern = Pattern.compile("\\p{InCombiningDiacriticalMarks}+"); //$NON-NLS-1$
        //                                                                                  //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
        //                                                                                  //ORIGINAL LINE: final StringBuilder decomposed = new StringBuilder(java.text.Normalizer.normalize(input, java.text.Normalizer.Form.NFD));
        //          StringBuilder decomposed = new StringBuilder(Normalizer.normalize(input, Normalizer.Form.NFD));
        //          convertRemainingAccentCharacters(decomposed);
        //          // Note that this doesn't correctly remove ligatures...
        //          return pattern.matcher(decomposed).replaceAll(StringUtils.EMPTY);
        //      }

        //      //JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
        //      //ORIGINAL LINE: private static void convertRemainingAccentCharacters(final StringBuilder decomposed)
        //      private static void convertRemainingAccentCharacters(StringBuilder decomposed)
        //      {
        //          for (int i = 0; i < decomposed.Length; i++)
        //          {
        //              if (decomposed[i] == '\u0141')
        //              {
        //                  decomposed.Remove(i, 1);
        //                  decomposed.Insert(i, 'L');
        //              }
        //              else if (decomposed[i] == '\u0142')
        //              {
        //                  decomposed.Remove(i, 1);
        //                  decomposed.Insert(i, 'l');
        //              }
        //          }
        //      }

        //      // Equals
        //      //-----------------------------------------------------------------------
        //      /// <summary>
        //      /// <para>Compares two CharSequences, returning {@code true} if they represent
        //      /// equal sequences of characters.</para>
        //      /// 
        //      /// <para>{@code null}s are handled without exceptions. Two {@code null}
        //      /// references are considered to be equal. The comparison is case sensitive.</para>
        //      /// 
        //      /// <pre>
        //      /// StringUtils.equals(null, null)   = true
        //      /// StringUtils.equals(null, "abc")  = false
        //      /// StringUtils.equals("abc", null)  = false
        //      /// StringUtils.equals("abc", "abc") = true
        //      /// StringUtils.equals("abc", "ABC") = false
        //      /// </pre>
        //      /// </summary>
        //      /// <seealso cref= Object#equals(Object) </seealso>
        //      /// <param name="cs1">  the first CharSequence, may be {@code null} </param>
        //      /// <param name="cs2">  the second CharSequence, may be {@code null} </param>
        //      /// <returns> {@code true} if the CharSequences are equal (case-sensitive), or both {@code null}
        //      /// @since 3.0 Changed signature from equals(String, String) to equals(CharSequence, CharSequence) </returns>
        //      //JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
        //      //ORIGINAL LINE: public static boolean equals(final CharSequence cs1, final CharSequence cs2)
        //      public static bool Equals(CharSequence cs1, CharSequence cs2)
        //      {
        //          if (cs1 == cs2)
        //          {
        //              return true;
        //          }
        //          if (cs1 == null || cs2 == null)
        //          {
        //              return false;
        //          }
        //          if (cs1.length() != cs2.length())
        //          {
        //              return false;
        //          }
        //          if (cs1 is string && cs2 is string)
        //          {
        //              return cs1.Equals(cs2);
        //          }
        //          return CharSequenceUtils.regionMatches(cs1, false, 0, cs2, 0, cs1.length());
        //      }

        //      /// <summary>
        //      /// <para>Compares two CharSequences, returning {@code true} if they represent
        //      /// equal sequences of characters, ignoring case.</para>
        //      /// 
        //      /// <para>{@code null}s are handled without exceptions. Two {@code null}
        //      /// references are considered equal. Comparison is case insensitive.</para>
        //      /// 
        //      /// <pre>
        //      /// StringUtils.equalsIgnoreCase(null, null)   = true
        //      /// StringUtils.equalsIgnoreCase(null, "abc")  = false
        //      /// StringUtils.equalsIgnoreCase("abc", null)  = false
        //      /// StringUtils.equalsIgnoreCase("abc", "abc") = true
        //      /// StringUtils.equalsIgnoreCase("abc", "ABC") = true
        //      /// </pre>
        //      /// </summary>
        //      /// <param name="str1">  the first CharSequence, may be null </param>
        //      /// <param name="str2">  the second CharSequence, may be null </param>
        //      /// <returns> {@code true} if the CharSequence are equal, case insensitive, or
        //      ///  both {@code null}
        //      /// @since 3.0 Changed signature from equalsIgnoreCase(String, String) to equalsIgnoreCase(CharSequence, CharSequence) </returns>
        //      //JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
        //      //ORIGINAL LINE: public static boolean equalsIgnoreCase(final CharSequence str1, final CharSequence str2)
        //      public static bool equalsIgnoreCase(CharSequence str1, CharSequence str2)
        //      {
        //          if (str1 == null || str2 == null)
        //          {
        //              return str1 == str2;
        //          }
        //          else if (str1 == str2)
        //          {
        //              return true;
        //          }
        //          else if (str1.length() != str2.length())
        //          {
        //              return false;
        //          }
        //          else
        //          {
        //              return CharSequenceUtils.regionMatches(str1, true, 0, str2, 0, str1.length());
        //          }
        //      }

        //      // Compare
        //      //-----------------------------------------------------------------------
        //      /// <summary>
        //      /// <para>Compare two Strings lexicographically, as per <seealso cref="String#compareTo(String)"/>, returning :</para>
        //      /// <ul>
        //      ///  <li>{@code int = 0}, if {@code str1} is equal to {@code str2} (or both {@code null})</li>
        //      ///  <li>{@code int < 0}, if {@code str1} is less than {@code str2}</li>
        //      ///  <li>{@code int > 0}, if {@code str1} is greater than {@code str2}</li>
        //      /// </ul>
        //      /// 
        //      /// <para>This is a {@code null} safe version of :</para>
        //      /// <blockquote><pre>str1.compareTo(str2)</pre></blockquote>
        //      /// 
        //      /// <para>{@code null} value is considered less than non-{@code null} value.
        //      /// Two {@code null} references are considered equal.</para>
        //      /// 
        //      /// <pre>
        //      /// StringUtils.compare(null, null)   = 0
        //      /// StringUtils.compare(null , "a")   &lt; 0
        //      /// StringUtils.compare("a", null)    &gt; 0
        //      /// StringUtils.compare("abc", "abc") = 0
        //      /// StringUtils.compare("a", "b")     &lt; 0
        //      /// StringUtils.compare("b", "a")     &gt; 0
        //      /// StringUtils.compare("a", "B")     &gt; 0
        //      /// StringUtils.compare("ab", "abc")  &lt; 0
        //      /// </pre>
        //      /// </summary>
        //      /// <seealso cref= #compare(String, String, boolean) </seealso>
        //      /// <seealso cref= String#compareTo(String) </seealso>
        //      /// <param name="str1">  the String to compare from </param>
        //      /// <param name="str2">  the String to compare to </param>
        //      /// <returns> &lt; 0, 0, &gt; 0, if {@code str1} is respectively less, equal ou greater than {@code str2}
        //      /// @since 3.5 </returns>
        //      //JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
        //      //ORIGINAL LINE: public static int compare(final String str1, final String str2)
        //      public static int compare(string str1, string str2)
        //      {
        //          return compare(str1, str2, true);
        //      }

        //      /// <summary>
        //      /// <para>Compare two Strings lexicographically, as per <seealso cref="String#compareTo(String)"/>, returning :</para>
        //      /// <ul>
        //      ///  <li>{@code int = 0}, if {@code str1} is equal to {@code str2} (or both {@code null})</li>
        //      ///  <li>{@code int < 0}, if {@code str1} is less than {@code str2}</li>
        //      ///  <li>{@code int > 0}, if {@code str1} is greater than {@code str2}</li>
        //      /// </ul>
        //      /// 
        //      /// <para>This is a {@code null} safe version of :</para>
        //      /// <blockquote><pre>str1.compareTo(str2)</pre></blockquote>
        //      /// 
        //      /// <para>{@code null} inputs are handled according to the {@code nullIsLess} parameter.
        //      /// Two {@code null} references are considered equal.</para>
        //      /// 
        //      /// <pre>
        //      /// StringUtils.compare(null, null, *)     = 0
        //      /// StringUtils.compare(null , "a", true)  &lt; 0
        //      /// StringUtils.compare(null , "a", false) &gt; 0
        //      /// StringUtils.compare("a", null, true)   &gt; 0
        //      /// StringUtils.compare("a", null, false)  &lt; 0
        //      /// StringUtils.compare("abc", "abc", *)   = 0
        //      /// StringUtils.compare("a", "b", *)       &lt; 0
        //      /// StringUtils.compare("b", "a", *)       &gt; 0
        //      /// StringUtils.compare("a", "B", *)       &gt; 0
        //      /// StringUtils.compare("ab", "abc", *)    &lt; 0
        //      /// </pre>
        //      /// </summary>
        //      /// <seealso cref= String#compareTo(String) </seealso>
        //      /// <param name="str1">  the String to compare from </param>
        //      /// <param name="str2">  the String to compare to </param>
        //      /// <param name="nullIsLess">  whether consider {@code null} value less than non-{@code null} value </param>
        //      /// <returns> &lt; 0, 0, &gt; 0, if {@code str1} is respectively less, equal ou greater than {@code str2}
        //      /// @since 3.5 </returns>
        //      //JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
        //      //ORIGINAL LINE: public static int compare(final String str1, final String str2, final boolean nullIsLess)
        //      public static int compare(string str1, string str2, bool nullIsLess)
        //      {
        //          if (string.ReferenceEquals(str1, str2))
        //          {
        //              return 0;
        //          }
        //          if (string.ReferenceEquals(str1, null))
        //          {
        //              return nullIsLess ? -1 : 1;
        //          }
        //          if (string.ReferenceEquals(str2, null))
        //          {
        //              return nullIsLess ? 1 : -1;
        //          }
        //          return str1.CompareTo(str2);
        //      }

        //      /// <summary>
        //      /// <para>Compare two Strings lexicographically, ignoring case differences,
        //      /// as per <seealso cref="String#compareToIgnoreCase(String)"/>, returning :</para>
        //      /// <ul>
        //      ///  <li>{@code int = 0}, if {@code str1} is equal to {@code str2} (or both {@code null})</li>
        //      ///  <li>{@code int < 0}, if {@code str1} is less than {@code str2}</li>
        //      ///  <li>{@code int > 0}, if {@code str1} is greater than {@code str2}</li>
        //      /// </ul>
        //      /// 
        //      /// <para>This is a {@code null} safe version of :</para>
        //      /// <blockquote><pre>str1.compareToIgnoreCase(str2)</pre></blockquote>
        //      /// 
        //      /// <para>{@code null} value is considered less than non-{@code null} value.
        //      /// Two {@code null} references are considered equal.
        //      /// Comparison is case insensitive.</para>
        //      /// 
        //      /// <pre>
        //      /// StringUtils.compareIgnoreCase(null, null)   = 0
        //      /// StringUtils.compareIgnoreCase(null , "a")   &lt; 0
        //      /// StringUtils.compareIgnoreCase("a", null)    &gt; 0
        //      /// StringUtils.compareIgnoreCase("abc", "abc") = 0
        //      /// StringUtils.compareIgnoreCase("abc", "ABC") = 0
        //      /// StringUtils.compareIgnoreCase("a", "b")     &lt; 0
        //      /// StringUtils.compareIgnoreCase("b", "a")     &gt; 0
        //      /// StringUtils.compareIgnoreCase("a", "B")     &lt; 0
        //      /// StringUtils.compareIgnoreCase("A", "b")     &lt; 0
        //      /// StringUtils.compareIgnoreCase("ab", "ABC")  &lt; 0
        //      /// </pre>
        //      /// </summary>
        //      /// <seealso cref= #compareIgnoreCase(String, String, boolean) </seealso>
        //      /// <seealso cref= String#compareToIgnoreCase(String) </seealso>
        //      /// <param name="str1">  the String to compare from </param>
        //      /// <param name="str2">  the String to compare to </param>
        //      /// <returns> &lt; 0, 0, &gt; 0, if {@code str1} is respectively less, equal ou greater than {@code str2},
        //      ///          ignoring case differences.
        //      /// @since 3.5 </returns>
        //      //JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
        //      //ORIGINAL LINE: public static int compareIgnoreCase(final String str1, final String str2)
        //      public static int compareIgnoreCase(string str1, string str2)
        //      {
        //          return compareIgnoreCase(str1, str2, true);
        //      }

        //      /// <summary>
        //      /// <para>Compare two Strings lexicographically, ignoring case differences,
        //      /// as per <seealso cref="String#compareToIgnoreCase(String)"/>, returning :</para>
        //      /// <ul>
        //      ///  <li>{@code int = 0}, if {@code str1} is equal to {@code str2} (or both {@code null})</li>
        //      ///  <li>{@code int < 0}, if {@code str1} is less than {@code str2}</li>
        //      ///  <li>{@code int > 0}, if {@code str1} is greater than {@code str2}</li>
        //      /// </ul>
        //      /// 
        //      /// <para>This is a {@code null} safe version of :</para>
        //      /// <blockquote><pre>str1.compareToIgnoreCase(str2)</pre></blockquote>
        //      /// 
        //      /// <para>{@code null} inputs are handled according to the {@code nullIsLess} parameter.
        //      /// Two {@code null} references are considered equal.
        //      /// Comparison is case insensitive.</para>
        //      /// 
        //      /// <pre>
        //      /// StringUtils.compareIgnoreCase(null, null, *)     = 0
        //      /// StringUtils.compareIgnoreCase(null , "a", true)  &lt; 0
        //      /// StringUtils.compareIgnoreCase(null , "a", false) &gt; 0
        //      /// StringUtils.compareIgnoreCase("a", null, true)   &gt; 0
        //      /// StringUtils.compareIgnoreCase("a", null, false)  &lt; 0
        //      /// StringUtils.compareIgnoreCase("abc", "abc", *)   = 0
        //      /// StringUtils.compareIgnoreCase("abc", "ABC", *)   = 0
        //      /// StringUtils.compareIgnoreCase("a", "b", *)       &lt; 0
        //      /// StringUtils.compareIgnoreCase("b", "a", *)       &gt; 0
        //      /// StringUtils.compareIgnoreCase("a", "B", *)       &lt; 0
        //      /// StringUtils.compareIgnoreCase("A", "b", *)       &lt; 0
        //      /// StringUtils.compareIgnoreCase("ab", "abc", *)    &lt; 0
        //      /// </pre>
        //      /// </summary>
        //      /// <seealso cref= String#compareToIgnoreCase(String) </seealso>
        //      /// <param name="str1">  the String to compare from </param>
        //      /// <param name="str2">  the String to compare to </param>
        //      /// <param name="nullIsLess">  whether consider {@code null} value less than non-{@code null} value </param>
        //      /// <returns> &lt; 0, 0, &gt; 0, if {@code str1} is respectively less, equal ou greater than {@code str2},
        //      ///          ignoring case differences.
        //      /// @since 3.5 </returns>
        //      //JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
        //      //ORIGINAL LINE: public static int compareIgnoreCase(final String str1, final String str2, final boolean nullIsLess)
        //      public static int compareIgnoreCase(string str1, string str2, bool nullIsLess)
        //      {
        //          if (string.ReferenceEquals(str1, str2))
        //          {
        //              return 0;
        //          }
        //          if (string.ReferenceEquals(str1, null))
        //          {
        //              return nullIsLess ? -1 : 1;
        //          }
        //          if (string.ReferenceEquals(str2, null))
        //          {
        //              return nullIsLess ? 1 : -1;
        //          }
        //          return str1.compareToIgnoreCase(str2);
        //      }

        //      /// <summary>
        //      /// <para>Compares given <code>string</code> to a CharSequences vararg of <code>searchStrings</code>,
        //      /// returning {@code true} if the <code>string</code> is equal to any of the <code>searchStrings</code>.</para>
        //      /// 
        //      /// <pre>
        //      /// StringUtils.equalsAny(null, (CharSequence[]) null) = false
        //      /// StringUtils.equalsAny(null, null, null)    = true
        //      /// StringUtils.equalsAny(null, "abc", "def")  = false
        //      /// StringUtils.equalsAny("abc", null, "def")  = false
        //      /// StringUtils.equalsAny("abc", "abc", "def") = true
        //      /// StringUtils.equalsAny("abc", "ABC", "DEF") = false
        //      /// </pre>
        //      /// </summary>
        //      /// <param name="string"> to compare, may be {@code null}. </param>
        //      /// <param name="searchStrings"> a vararg of strings, may be {@code null}. </param>
        //      /// <returns> {@code true} if the string is equal (case-sensitive) to any other element of <code>searchStrings</code>;
        //      /// {@code false} if <code>searchStrings</code> is null or contains no matches.
        //      /// @since 3.5 </returns>
        //      //JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
        //      //ORIGINAL LINE: public static boolean equalsAny(final CharSequence string, final CharSequence... searchStrings)
        //      public static bool equalsAny(CharSequence @string, params CharSequence[] searchStrings)
        //      {
        //          if (ArrayUtils.isNotEmpty(searchStrings))
        //          {
        //              foreach (CharSequence next in searchStrings)
        //              {
        //                  if (Equals(@string, next))
        //                  {
        //                      return true;
        //                  }
        //              }
        //          }
        //          return false;
        //      }


        //      /// <summary>
        //      /// <para>Compares given <code>string</code> to a CharSequences vararg of <code>searchStrings</code>,
        //      /// returning {@code true} if the <code>string</code> is equal to any of the <code>searchStrings</code>, ignoring case.</para>
        //      /// 
        //      /// <pre>
        //      /// StringUtils.equalsAnyIgnoreCase(null, (CharSequence[]) null) = false
        //      /// StringUtils.equalsAnyIgnoreCase(null, null, null)    = true
        //      /// StringUtils.equalsAnyIgnoreCase(null, "abc", "def")  = false
        //      /// StringUtils.equalsAnyIgnoreCase("abc", null, "def")  = false
        //      /// StringUtils.equalsAnyIgnoreCase("abc", "abc", "def") = true
        //      /// StringUtils.equalsAnyIgnoreCase("abc", "ABC", "DEF") = true
        //      /// </pre>
        //      /// </summary>
        //      /// <param name="string"> to compare, may be {@code null}. </param>
        //      /// <param name="searchStrings"> a vararg of strings, may be {@code null}. </param>
        //      /// <returns> {@code true} if the string is equal (case-insensitive) to any other element of <code>searchStrings</code>;
        //      /// {@code false} if <code>searchStrings</code> is null or contains no matches.
        //      /// @since 3.5 </returns>
        //      //JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
        //      //ORIGINAL LINE: public static boolean equalsAnyIgnoreCase(final CharSequence string, final CharSequence...searchStrings)
        //      public static bool equalsAnyIgnoreCase(CharSequence @string, params CharSequence[] searchStrings)
        //      {
        //          if (ArrayUtils.isNotEmpty(searchStrings))
        //          {
        //              foreach (CharSequence next in searchStrings)
        //              {
        //                  if (equalsIgnoreCase(@string, next))
        //                  {
        //                      return true;
        //                  }
        //              }
        //          }
        //          return false;
        //      }

        //      // IndexOf
        //      //-----------------------------------------------------------------------
        //      /// <summary>
        //      /// <para>Finds the first index within a CharSequence, handling {@code null}.
        //      /// This method uses <seealso cref="String#indexOf(int, int)"/> if possible.</para>
        //      /// 
        //      /// <para>A {@code null} or empty ("") CharSequence will return {@code INDEX_NOT_FOUND (-1)}.</para>
        //      /// 
        //      /// <pre>
        //      /// StringUtils.indexOf(null, *)         = -1
        //      /// StringUtils.indexOf("", *)           = -1
        //      /// StringUtils.indexOf("aabaabaa", 'a') = 0
        //      /// StringUtils.indexOf("aabaabaa", 'b') = 2
        //      /// </pre>
        //      /// </summary>
        //      /// <param name="seq">  the CharSequence to check, may be null </param>
        //      /// <param name="searchChar">  the character to find </param>
        //      /// <returns> the first index of the search character,
        //      ///  -1 if no match or {@code null} string input
        //      /// @since 2.0
        //      /// @since 3.0 Changed signature from indexOf(String, int) to indexOf(CharSequence, int) </returns>
        //      //JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
        //      //ORIGINAL LINE: public static int indexOf(final CharSequence seq, final int searchChar)
        //      public static int indexOf(CharSequence seq, int searchChar)
        //      {
        //          if (isEmpty(seq))
        //          {
        //              return INDEX_NOT_FOUND;
        //          }
        //          return CharSequenceUtils.IndexOf(seq, searchChar, 0);
        //      }

        //      /// <summary>
        //      /// <para>Finds the first index within a CharSequence from a start position,
        //      /// handling {@code null}.
        //      /// This method uses <seealso cref="String#indexOf(int, int)"/> if possible.</para>
        //      /// 
        //      /// <para>A {@code null} or empty ("") CharSequence will return {@code (INDEX_NOT_FOUND) -1}.
        //      /// A negative start position is treated as zero.
        //      /// A start position greater than the string length returns {@code -1}.</para>
        //      /// 
        //      /// <pre>
        //      /// StringUtils.indexOf(null, *, *)          = -1
        //      /// StringUtils.indexOf("", *, *)            = -1
        //      /// StringUtils.indexOf("aabaabaa", 'b', 0)  = 2
        //      /// StringUtils.indexOf("aabaabaa", 'b', 3)  = 5
        //      /// StringUtils.indexOf("aabaabaa", 'b', 9)  = -1
        //      /// StringUtils.indexOf("aabaabaa", 'b', -1) = 2
        //      /// </pre>
        //      /// </summary>
        //      /// <param name="seq">  the CharSequence to check, may be null </param>
        //      /// <param name="searchChar">  the character to find </param>
        //      /// <param name="startPos">  the start position, negative treated as zero </param>
        //      /// <returns> the first index of the search character (always &ge; startPos),
        //      ///  -1 if no match or {@code null} string input
        //      /// @since 2.0
        //      /// @since 3.0 Changed signature from indexOf(String, int, int) to indexOf(CharSequence, int, int) </returns>
        //      //JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
        //      //ORIGINAL LINE: public static int indexOf(final CharSequence seq, final int searchChar, final int startPos)
        //      public static int indexOf(CharSequence seq, int searchChar, int startPos)
        //      {
        //          if (isEmpty(seq))
        //          {
        //              return INDEX_NOT_FOUND;
        //          }
        //          return CharSequenceUtils.IndexOf(seq, searchChar, startPos);
        //      }

        //      /// <summary>
        //      /// <para>Finds the first index within a CharSequence, handling {@code null}.
        //      /// This method uses <seealso cref="String#indexOf(String, int)"/> if possible.</para>
        //      /// 
        //      /// <para>A {@code null} CharSequence will return {@code -1}.</para>
        //      /// 
        //      /// <pre>
        //      /// StringUtils.indexOf(null, *)          = -1
        //      /// StringUtils.indexOf(*, null)          = -1
        //      /// StringUtils.indexOf("", "")           = 0
        //      /// StringUtils.indexOf("", *)            = -1 (except when * = "")
        //      /// StringUtils.indexOf("aabaabaa", "a")  = 0
        //      /// StringUtils.indexOf("aabaabaa", "b")  = 2
        //      /// StringUtils.indexOf("aabaabaa", "ab") = 1
        //      /// StringUtils.indexOf("aabaabaa", "")   = 0
        //      /// </pre>
        //      /// </summary>
        //      /// <param name="seq">  the CharSequence to check, may be null </param>
        //      /// <param name="searchSeq">  the CharSequence to find, may be null </param>
        //      /// <returns> the first index of the search CharSequence,
        //      ///  -1 if no match or {@code null} string input
        //      /// @since 2.0
        //      /// @since 3.0 Changed signature from indexOf(String, String) to indexOf(CharSequence, CharSequence) </returns>
        //      //JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
        //      //ORIGINAL LINE: public static int indexOf(final CharSequence seq, final CharSequence searchSeq)
        //      public static int indexOf(CharSequence seq, CharSequence searchSeq)
        //      {
        //          if (seq == null || searchSeq == null)
        //          {
        //              return INDEX_NOT_FOUND;
        //          }
        //          return CharSequenceUtils.IndexOf(seq, searchSeq, 0);
        //      }

        //      /// <summary>
        //      /// <para>Finds the first index within a CharSequence, handling {@code null}.
        //      /// This method uses <seealso cref="String#indexOf(String, int)"/> if possible.</para>
        //      /// 
        //      /// <para>A {@code null} CharSequence will return {@code -1}.
        //      /// A negative start position is treated as zero.
        //      /// An empty ("") search CharSequence always matches.
        //      /// A start position greater than the string length only matches
        //      /// an empty search CharSequence.</para>
        //      /// 
        //      /// <pre>
        //      /// StringUtils.indexOf(null, *, *)          = -1
        //      /// StringUtils.indexOf(*, null, *)          = -1
        //      /// StringUtils.indexOf("", "", 0)           = 0
        //      /// StringUtils.indexOf("", *, 0)            = -1 (except when * = "")
        //      /// StringUtils.indexOf("aabaabaa", "a", 0)  = 0
        //      /// StringUtils.indexOf("aabaabaa", "b", 0)  = 2
        //      /// StringUtils.indexOf("aabaabaa", "ab", 0) = 1
        //      /// StringUtils.indexOf("aabaabaa", "b", 3)  = 5
        //      /// StringUtils.indexOf("aabaabaa", "b", 9)  = -1
        //      /// StringUtils.indexOf("aabaabaa", "b", -1) = 2
        //      /// StringUtils.indexOf("aabaabaa", "", 2)   = 2
        //      /// StringUtils.indexOf("abc", "", 9)        = 3
        //      /// </pre>
        //      /// </summary>
        //      /// <param name="seq">  the CharSequence to check, may be null </param>
        //      /// <param name="searchSeq">  the CharSequence to find, may be null </param>
        //      /// <param name="startPos">  the start position, negative treated as zero </param>
        //      /// <returns> the first index of the search CharSequence (always &ge; startPos),
        //      ///  -1 if no match or {@code null} string input
        //      /// @since 2.0
        //      /// @since 3.0 Changed signature from indexOf(String, String, int) to indexOf(CharSequence, CharSequence, int) </returns>
        //      //JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
        //      //ORIGINAL LINE: public static int indexOf(final CharSequence seq, final CharSequence searchSeq, final int startPos)
        //      public static int indexOf(CharSequence seq, CharSequence searchSeq, int startPos)
        //      {
        //          if (seq == null || searchSeq == null)
        //          {
        //              return INDEX_NOT_FOUND;
        //          }
        //          return CharSequenceUtils.IndexOf(seq, searchSeq, startPos);
        //      }

        //      /// <summary>
        //      /// <para>Finds the n-th index within a CharSequence, handling {@code null}.
        //      /// This method uses <seealso cref="String#indexOf(String)"/> if possible.</para>
        //      /// <para><b>Note:</b> The code starts looking for a match at the start of the target,
        //      /// incrementing the starting index by one after each successful match
        //      /// (unless {@code searchStr} is an empty string in which case the position
        //      /// is never incremented and {@code 0} is returned immediately).
        //      /// This means that matches may overlap.</para>
        //      /// <para>A {@code null} CharSequence will return {@code -1}.</para>
        //      /// 
        //      /// <pre>
        //      /// StringUtils.ordinalIndexOf(null, *, *)          = -1
        //      /// StringUtils.ordinalIndexOf(*, null, *)          = -1
        //      /// StringUtils.ordinalIndexOf("", "", *)           = 0
        //      /// StringUtils.ordinalIndexOf("aabaabaa", "a", 1)  = 0
        //      /// StringUtils.ordinalIndexOf("aabaabaa", "a", 2)  = 1
        //      /// StringUtils.ordinalIndexOf("aabaabaa", "b", 1)  = 2
        //      /// StringUtils.ordinalIndexOf("aabaabaa", "b", 2)  = 5
        //      /// StringUtils.ordinalIndexOf("aabaabaa", "ab", 1) = 1
        //      /// StringUtils.ordinalIndexOf("aabaabaa", "ab", 2) = 4
        //      /// StringUtils.ordinalIndexOf("aabaabaa", "", 1)   = 0
        //      /// StringUtils.ordinalIndexOf("aabaabaa", "", 2)   = 0
        //      /// </pre>
        //      /// 
        //      /// <para>Matches may overlap:</para>
        //      /// <pre>
        //      /// StringUtils.ordinalIndexOf("ababab","aba", 1)   = 0
        //      /// StringUtils.ordinalIndexOf("ababab","aba", 2)   = 2
        //      /// StringUtils.ordinalIndexOf("ababab","aba", 3)   = -1
        //      /// 
        //      /// StringUtils.ordinalIndexOf("abababab", "abab", 1) = 0
        //      /// StringUtils.ordinalIndexOf("abababab", "abab", 2) = 2
        //      /// StringUtils.ordinalIndexOf("abababab", "abab", 3) = 4
        //      /// StringUtils.ordinalIndexOf("abababab", "abab", 4) = -1
        //      /// </pre>
        //      /// 
        //      /// <para>Note that 'head(CharSequence str, int n)' may be implemented as: </para>
        //      /// 
        //      /// <pre>
        //      ///   str.substring(0, lastOrdinalIndexOf(str, "\n", n))
        //      /// </pre>
        //      /// </summary>
        //      /// <param name="str">  the CharSequence to check, may be null </param>
        //      /// <param name="searchStr">  the CharSequence to find, may be null </param>
        //      /// <param name="ordinal">  the n-th {@code searchStr} to find </param>
        //      /// <returns> the n-th index of the search CharSequence,
        //      ///  {@code -1} ({@code INDEX_NOT_FOUND}) if no match or {@code null} string input
        //      /// @since 2.1
        //      /// @since 3.0 Changed signature from ordinalIndexOf(String, String, int) to ordinalIndexOf(CharSequence, CharSequence, int) </returns>
        //      //JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
        //      //ORIGINAL LINE: public static int ordinalIndexOf(final CharSequence str, final CharSequence searchStr, final int ordinal)
        //      public static int ordinalIndexOf(CharSequence str, CharSequence searchStr, int ordinal)
        //      {
        //          return ordinalIndexOf(str, searchStr, ordinal, false);
        //      }

        //      /// <summary>
        //      /// <para>Finds the n-th index within a String, handling {@code null}.
        //      /// This method uses <seealso cref="String#indexOf(String)"/> if possible.</para>
        //      /// <para>Note that matches may overlap<p>
        //      /// 
        //      /// </para>
        //      /// <para>A {@code null} CharSequence will return {@code -1}.</para>
        //      /// </summary>
        //      /// <param name="str">  the CharSequence to check, may be null </param>
        //      /// <param name="searchStr">  the CharSequence to find, may be null </param>
        //      /// <param name="ordinal">  the n-th {@code searchStr} to find, overlapping matches are allowed. </param>
        //      /// <param name="lastIndex"> true if lastOrdinalIndexOf() otherwise false if ordinalIndexOf() </param>
        //      /// <returns> the n-th index of the search CharSequence,
        //      ///  {@code -1} ({@code INDEX_NOT_FOUND}) if no match or {@code null} string input </returns>
        //      // Shared code between ordinalIndexOf(String,String,int) and lastOrdinalIndexOf(String,String,int)
        //      //JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
        //      //ORIGINAL LINE: private static int ordinalIndexOf(final CharSequence str, final CharSequence searchStr, final int ordinal, final boolean lastIndex)
        //      private static int ordinalIndexOf(CharSequence str, CharSequence searchStr, int ordinal, bool lastIndex)
        //      {
        //          if (str == null || searchStr == null || ordinal <= 0)
        //          {
        //              return INDEX_NOT_FOUND;
        //          }
        //          if (searchStr.length() == 0)
        //          {
        //              return lastIndex ? str.length() : 0;
        //          }
        //          int found = 0;
        //          // set the initial index beyond the end of the string
        //          // this is to allow for the initial index decrement/increment
        //          int index = lastIndex ? str.length() : INDEX_NOT_FOUND;
        //          do
        //          {
        //              if (lastIndex)
        //              {
        //                  index = CharSequenceUtils.LastIndexOf(str, searchStr, index - 1); // step backwards thru string
        //              }
        //              else
        //              {
        //                  index = CharSequenceUtils.IndexOf(str, searchStr, index + 1); // step forwards through string
        //              }
        //              if (index < 0)
        //              {
        //                  return index;
        //              }
        //              found++;
        //          } while (found < ordinal);
        //          return index;
        //      }

        //      /// <summary>
        //      /// <para>Case in-sensitive find of the first index within a CharSequence.</para>
        //      /// 
        //      /// <para>A {@code null} CharSequence will return {@code -1}.
        //      /// A negative start position is treated as zero.
        //      /// An empty ("") search CharSequence always matches.
        //      /// A start position greater than the string length only matches
        //      /// an empty search CharSequence.</para>
        //      /// 
        //      /// <pre>
        //      /// StringUtils.indexOfIgnoreCase(null, *)          = -1
        //      /// StringUtils.indexOfIgnoreCase(*, null)          = -1
        //      /// StringUtils.indexOfIgnoreCase("", "")           = 0
        //      /// StringUtils.indexOfIgnoreCase("aabaabaa", "a")  = 0
        //      /// StringUtils.indexOfIgnoreCase("aabaabaa", "b")  = 2
        //      /// StringUtils.indexOfIgnoreCase("aabaabaa", "ab") = 1
        //      /// </pre>
        //      /// </summary>
        //      /// <param name="str">  the CharSequence to check, may be null </param>
        //      /// <param name="searchStr">  the CharSequence to find, may be null </param>
        //      /// <returns> the first index of the search CharSequence,
        //      ///  -1 if no match or {@code null} string input
        //      /// @since 2.5
        //      /// @since 3.0 Changed signature from indexOfIgnoreCase(String, String) to indexOfIgnoreCase(CharSequence, CharSequence) </returns>
        //      //JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
        //      //ORIGINAL LINE: public static int indexOfIgnoreCase(final CharSequence str, final CharSequence searchStr)
        //      public static int indexOfIgnoreCase(CharSequence str, CharSequence searchStr)
        //      {
        //          return indexOfIgnoreCase(str, searchStr, 0);
        //      }

        //      /// <summary>
        //      /// <para>Case in-sensitive find of the first index within a CharSequence
        //      /// from the specified position.</para>
        //      /// 
        //      /// <para>A {@code null} CharSequence will return {@code -1}.
        //      /// A negative start position is treated as zero.
        //      /// An empty ("") search CharSequence always matches.
        //      /// A start position greater than the string length only matches
        //      /// an empty search CharSequence.</para>
        //      /// 
        //      /// <pre>
        //      /// StringUtils.indexOfIgnoreCase(null, *, *)          = -1
        //      /// StringUtils.indexOfIgnoreCase(*, null, *)          = -1
        //      /// StringUtils.indexOfIgnoreCase("", "", 0)           = 0
        //      /// StringUtils.indexOfIgnoreCase("aabaabaa", "A", 0)  = 0
        //      /// StringUtils.indexOfIgnoreCase("aabaabaa", "B", 0)  = 2
        //      /// StringUtils.indexOfIgnoreCase("aabaabaa", "AB", 0) = 1
        //      /// StringUtils.indexOfIgnoreCase("aabaabaa", "B", 3)  = 5
        //      /// StringUtils.indexOfIgnoreCase("aabaabaa", "B", 9)  = -1
        //      /// StringUtils.indexOfIgnoreCase("aabaabaa", "B", -1) = 2
        //      /// StringUtils.indexOfIgnoreCase("aabaabaa", "", 2)   = 2
        //      /// StringUtils.indexOfIgnoreCase("abc", "", 9)        = -1
        //      /// </pre>
        //      /// </summary>
        //      /// <param name="str">  the CharSequence to check, may be null </param>
        //      /// <param name="searchStr">  the CharSequence to find, may be null </param>
        //      /// <param name="startPos">  the start position, negative treated as zero </param>
        //      /// <returns> the first index of the search CharSequence (always &ge; startPos),
        //      ///  -1 if no match or {@code null} string input
        //      /// @since 2.5
        //      /// @since 3.0 Changed signature from indexOfIgnoreCase(String, String, int) to indexOfIgnoreCase(CharSequence, CharSequence, int) </returns>
        //      //JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
        //      //ORIGINAL LINE: public static int indexOfIgnoreCase(final CharSequence str, final CharSequence searchStr, int startPos)
        //      public static int indexOfIgnoreCase(CharSequence str, CharSequence searchStr, int startPos)
        //      {
        //          if (str == null || searchStr == null)
        //          {
        //              return INDEX_NOT_FOUND;
        //          }
        //          if (startPos < 0)
        //          {
        //              startPos = 0;
        //          }
        //          //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
        //          //ORIGINAL LINE: final int endLimit = str.length() - searchStr.length() + 1;
        //          int endLimit = str.length() - searchStr.length() + 1;
        //          if (startPos > endLimit)
        //          {
        //              return INDEX_NOT_FOUND;
        //          }
        //          if (searchStr.length() == 0)
        //          {
        //              return startPos;
        //          }
        //          for (int i = startPos; i < endLimit; i++)
        //          {
        //              if (CharSequenceUtils.regionMatches(str, true, i, searchStr, 0, searchStr.length()))
        //              {
        //                  return i;
        //              }
        //          }
        //          return INDEX_NOT_FOUND;
        //      }

        //      // LastIndexOf
        //      //-----------------------------------------------------------------------
        //      /// <summary>
        //      /// <para>Finds the last index within a CharSequence, handling {@code null}.
        //      /// This method uses <seealso cref="String#lastIndexOf(int)"/> if possible.</para>
        //      /// 
        //      /// <para>A {@code null} or empty ("") CharSequence will return {@code -1}.</para>
        //      /// 
        //      /// <pre>
        //      /// StringUtils.lastIndexOf(null, *)         = -1
        //      /// StringUtils.lastIndexOf("", *)           = -1
        //      /// StringUtils.lastIndexOf("aabaabaa", 'a') = 7
        //      /// StringUtils.lastIndexOf("aabaabaa", 'b') = 5
        //      /// </pre>
        //      /// </summary>
        //      /// <param name="seq">  the CharSequence to check, may be null </param>
        //      /// <param name="searchChar">  the character to find </param>
        //      /// <returns> the last index of the search character,
        //      ///  -1 if no match or {@code null} string input
        //      /// @since 2.0
        //      /// @since 3.0 Changed signature from lastIndexOf(String, int) to lastIndexOf(CharSequence, int) </returns>
        //      //JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
        //      //ORIGINAL LINE: public static int lastIndexOf(final CharSequence seq, final int searchChar)
        //      public static int lastIndexOf(CharSequence seq, int searchChar)
        //      {
        //          if (isEmpty(seq))
        //          {
        //              return INDEX_NOT_FOUND;
        //          }
        //          return CharSequenceUtils.LastIndexOf(seq, searchChar, seq.length());
        //      }

        //      /// <summary>
        //      /// <para>Finds the last index within a CharSequence from a start position,
        //      /// handling {@code null}.
        //      /// This method uses <seealso cref="String#lastIndexOf(int, int)"/> if possible.</para>
        //      /// 
        //      /// <para>A {@code null} or empty ("") CharSequence will return {@code -1}.
        //      /// A negative start position returns {@code -1}.
        //      /// A start position greater than the string length searches the whole string.
        //      /// The search starts at the startPos and works backwards; matches starting after the start
        //      /// position are ignored.
        //      /// </para>
        //      /// 
        //      /// <pre>
        //      /// StringUtils.lastIndexOf(null, *, *)          = -1
        //      /// StringUtils.lastIndexOf("", *,  *)           = -1
        //      /// StringUtils.lastIndexOf("aabaabaa", 'b', 8)  = 5
        //      /// StringUtils.lastIndexOf("aabaabaa", 'b', 4)  = 2
        //      /// StringUtils.lastIndexOf("aabaabaa", 'b', 0)  = -1
        //      /// StringUtils.lastIndexOf("aabaabaa", 'b', 9)  = 5
        //      /// StringUtils.lastIndexOf("aabaabaa", 'b', -1) = -1
        //      /// StringUtils.lastIndexOf("aabaabaa", 'a', 0)  = 0
        //      /// </pre>
        //      /// </summary>
        //      /// <param name="seq">  the CharSequence to check, may be null </param>
        //      /// <param name="searchChar">  the character to find </param>
        //      /// <param name="startPos">  the start position </param>
        //      /// <returns> the last index of the search character (always &le; startPos),
        //      ///  -1 if no match or {@code null} string input
        //      /// @since 2.0
        //      /// @since 3.0 Changed signature from lastIndexOf(String, int, int) to lastIndexOf(CharSequence, int, int) </returns>
        //      //JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
        //      //ORIGINAL LINE: public static int lastIndexOf(final CharSequence seq, final int searchChar, final int startPos)
        //      public static int lastIndexOf(CharSequence seq, int searchChar, int startPos)
        //      {
        //          if (isEmpty(seq))
        //          {
        //              return INDEX_NOT_FOUND;
        //          }
        //          return CharSequenceUtils.LastIndexOf(seq, searchChar, startPos);
        //      }

        //      /// <summary>
        //      /// <para>Finds the last index within a CharSequence, handling {@code null}.
        //      /// This method uses <seealso cref="String#lastIndexOf(String)"/> if possible.</para>
        //      /// 
        //      /// <para>A {@code null} CharSequence will return {@code -1}.</para>
        //      /// 
        //      /// <pre>
        //      /// StringUtils.lastIndexOf(null, *)          = -1
        //      /// StringUtils.lastIndexOf(*, null)          = -1
        //      /// StringUtils.lastIndexOf("", "")           = 0
        //      /// StringUtils.lastIndexOf("aabaabaa", "a")  = 7
        //      /// StringUtils.lastIndexOf("aabaabaa", "b")  = 5
        //      /// StringUtils.lastIndexOf("aabaabaa", "ab") = 4
        //      /// StringUtils.lastIndexOf("aabaabaa", "")   = 8
        //      /// </pre>
        //      /// </summary>
        //      /// <param name="seq">  the CharSequence to check, may be null </param>
        //      /// <param name="searchSeq">  the CharSequence to find, may be null </param>
        //      /// <returns> the last index of the search String,
        //      ///  -1 if no match or {@code null} string input
        //      /// @since 2.0
        //      /// @since 3.0 Changed signature from lastIndexOf(String, String) to lastIndexOf(CharSequence, CharSequence) </returns>
        //      //JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
        //      //ORIGINAL LINE: public static int lastIndexOf(final CharSequence seq, final CharSequence searchSeq)
        //      public static int lastIndexOf(CharSequence seq, CharSequence searchSeq)
        //      {
        //          if (seq == null || searchSeq == null)
        //          {
        //              return INDEX_NOT_FOUND;
        //          }
        //          return CharSequenceUtils.LastIndexOf(seq, searchSeq, seq.length());
        //      }

        //      /// <summary>
        //      /// <para>Finds the n-th last index within a String, handling {@code null}.
        //      /// This method uses <seealso cref="String#lastIndexOf(String)"/>.</para>
        //      /// 
        //      /// <para>A {@code null} String will return {@code -1}.</para>
        //      /// 
        //      /// <pre>
        //      /// StringUtils.lastOrdinalIndexOf(null, *, *)          = -1
        //      /// StringUtils.lastOrdinalIndexOf(*, null, *)          = -1
        //      /// StringUtils.lastOrdinalIndexOf("", "", *)           = 0
        //      /// StringUtils.lastOrdinalIndexOf("aabaabaa", "a", 1)  = 7
        //      /// StringUtils.lastOrdinalIndexOf("aabaabaa", "a", 2)  = 6
        //      /// StringUtils.lastOrdinalIndexOf("aabaabaa", "b", 1)  = 5
        //      /// StringUtils.lastOrdinalIndexOf("aabaabaa", "b", 2)  = 2
        //      /// StringUtils.lastOrdinalIndexOf("aabaabaa", "ab", 1) = 4
        //      /// StringUtils.lastOrdinalIndexOf("aabaabaa", "ab", 2) = 1
        //      /// StringUtils.lastOrdinalIndexOf("aabaabaa", "", 1)   = 8
        //      /// StringUtils.lastOrdinalIndexOf("aabaabaa", "", 2)   = 8
        //      /// </pre>
        //      /// 
        //      /// <para>Note that 'tail(CharSequence str, int n)' may be implemented as: </para>
        //      /// 
        //      /// <pre>
        //      ///   str.substring(lastOrdinalIndexOf(str, "\n", n) + 1)
        //      /// </pre>
        //      /// </summary>
        //      /// <param name="str">  the CharSequence to check, may be null </param>
        //      /// <param name="searchStr">  the CharSequence to find, may be null </param>
        //      /// <param name="ordinal">  the n-th last {@code searchStr} to find </param>
        //      /// <returns> the n-th last index of the search CharSequence,
        //      ///  {@code -1} ({@code INDEX_NOT_FOUND}) if no match or {@code null} string input
        //      /// @since 2.5
        //      /// @since 3.0 Changed signature from lastOrdinalIndexOf(String, String, int) to lastOrdinalIndexOf(CharSequence, CharSequence, int) </returns>
        //      //JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
        //      //ORIGINAL LINE: public static int lastOrdinalIndexOf(final CharSequence str, final CharSequence searchStr, final int ordinal)
        //      public static int lastOrdinalIndexOf(CharSequence str, CharSequence searchStr, int ordinal)
        //      {
        //          return ordinalIndexOf(str, searchStr, ordinal, true);
        //      }

        //      /// <summary>
        //      /// <para>Finds the last index within a CharSequence, handling {@code null}.
        //      /// This method uses <seealso cref="String#lastIndexOf(String, int)"/> if possible.</para>
        //      /// 
        //      /// <para>A {@code null} CharSequence will return {@code -1}.
        //      /// A negative start position returns {@code -1}.
        //      /// An empty ("") search CharSequence always matches unless the start position is negative.
        //      /// A start position greater than the string length searches the whole string.
        //      /// The search starts at the startPos and works backwards; matches starting after the start
        //      /// position are ignored.
        //      /// </para>
        //      /// 
        //      /// <pre>
        //      /// StringUtils.lastIndexOf(null, *, *)          = -1
        //      /// StringUtils.lastIndexOf(*, null, *)          = -1
        //      /// StringUtils.lastIndexOf("aabaabaa", "a", 8)  = 7
        //      /// StringUtils.lastIndexOf("aabaabaa", "b", 8)  = 5
        //      /// StringUtils.lastIndexOf("aabaabaa", "ab", 8) = 4
        //      /// StringUtils.lastIndexOf("aabaabaa", "b", 9)  = 5
        //      /// StringUtils.lastIndexOf("aabaabaa", "b", -1) = -1
        //      /// StringUtils.lastIndexOf("aabaabaa", "a", 0)  = 0
        //      /// StringUtils.lastIndexOf("aabaabaa", "b", 0)  = -1
        //      /// StringUtils.lastIndexOf("aabaabaa", "b", 1)  = -1
        //      /// StringUtils.lastIndexOf("aabaabaa", "b", 2)  = 2
        //      /// StringUtils.lastIndexOf("aabaabaa", "ba", 2)  = -1
        //      /// StringUtils.lastIndexOf("aabaabaa", "ba", 2)  = 2
        //      /// </pre>
        //      /// </summary>
        //      /// <param name="seq">  the CharSequence to check, may be null </param>
        //      /// <param name="searchSeq">  the CharSequence to find, may be null </param>
        //      /// <param name="startPos">  the start position, negative treated as zero </param>
        //      /// <returns> the last index of the search CharSequence (always &le; startPos),
        //      ///  -1 if no match or {@code null} string input
        //      /// @since 2.0
        //      /// @since 3.0 Changed signature from lastIndexOf(String, String, int) to lastIndexOf(CharSequence, CharSequence, int) </returns>
        //      //JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
        //      //ORIGINAL LINE: public static int lastIndexOf(final CharSequence seq, final CharSequence searchSeq, final int startPos)
        //      public static int lastIndexOf(CharSequence seq, CharSequence searchSeq, int startPos)
        //      {
        //          if (seq == null || searchSeq == null)
        //          {
        //              return INDEX_NOT_FOUND;
        //          }
        //          return CharSequenceUtils.LastIndexOf(seq, searchSeq, startPos);
        //      }

        //      /// <summary>
        //      /// <para>Case in-sensitive find of the last index within a CharSequence.</para>
        //      /// 
        //      /// <para>A {@code null} CharSequence will return {@code -1}.
        //      /// A negative start position returns {@code -1}.
        //      /// An empty ("") search CharSequence always matches unless the start position is negative.
        //      /// A start position greater than the string length searches the whole string.</para>
        //      /// 
        //      /// <pre>
        //      /// StringUtils.lastIndexOfIgnoreCase(null, *)          = -1
        //      /// StringUtils.lastIndexOfIgnoreCase(*, null)          = -1
        //      /// StringUtils.lastIndexOfIgnoreCase("aabaabaa", "A")  = 7
        //      /// StringUtils.lastIndexOfIgnoreCase("aabaabaa", "B")  = 5
        //      /// StringUtils.lastIndexOfIgnoreCase("aabaabaa", "AB") = 4
        //      /// </pre>
        //      /// </summary>
        //      /// <param name="str">  the CharSequence to check, may be null </param>
        //      /// <param name="searchStr">  the CharSequence to find, may be null </param>
        //      /// <returns> the first index of the search CharSequence,
        //      ///  -1 if no match or {@code null} string input
        //      /// @since 2.5
        //      /// @since 3.0 Changed signature from lastIndexOfIgnoreCase(String, String) to lastIndexOfIgnoreCase(CharSequence, CharSequence) </returns>
        //      //JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
        //      //ORIGINAL LINE: public static int lastIndexOfIgnoreCase(final CharSequence str, final CharSequence searchStr)
        //      public static int lastIndexOfIgnoreCase(CharSequence str, CharSequence searchStr)
        //      {
        //          if (str == null || searchStr == null)
        //          {
        //              return INDEX_NOT_FOUND;
        //          }
        //          return lastIndexOfIgnoreCase(str, searchStr, str.length());
        //      }

        //      /// <summary>
        //      /// <para>Case in-sensitive find of the last index within a CharSequence
        //      /// from the specified position.</para>
        //      /// 
        //      /// <para>A {@code null} CharSequence will return {@code -1}.
        //      /// A negative start position returns {@code -1}.
        //      /// An empty ("") search CharSequence always matches unless the start position is negative.
        //      /// A start position greater than the string length searches the whole string.
        //      /// The search starts at the startPos and works backwards; matches starting after the start
        //      /// position are ignored.
        //      /// </para>
        //      /// 
        //      /// <pre>
        //      /// StringUtils.lastIndexOfIgnoreCase(null, *, *)          = -1
        //      /// StringUtils.lastIndexOfIgnoreCase(*, null, *)          = -1
        //      /// StringUtils.lastIndexOfIgnoreCase("aabaabaa", "A", 8)  = 7
        //      /// StringUtils.lastIndexOfIgnoreCase("aabaabaa", "B", 8)  = 5
        //      /// StringUtils.lastIndexOfIgnoreCase("aabaabaa", "AB", 8) = 4
        //      /// StringUtils.lastIndexOfIgnoreCase("aabaabaa", "B", 9)  = 5
        //      /// StringUtils.lastIndexOfIgnoreCase("aabaabaa", "B", -1) = -1
        //      /// StringUtils.lastIndexOfIgnoreCase("aabaabaa", "A", 0)  = 0
        //      /// StringUtils.lastIndexOfIgnoreCase("aabaabaa", "B", 0)  = -1
        //      /// </pre>
        //      /// </summary>
        //      /// <param name="str">  the CharSequence to check, may be null </param>
        //      /// <param name="searchStr">  the CharSequence to find, may be null </param>
        //      /// <param name="startPos">  the start position </param>
        //      /// <returns> the last index of the search CharSequence (always &le; startPos),
        //      ///  -1 if no match or {@code null} input
        //      /// @since 2.5
        //      /// @since 3.0 Changed signature from lastIndexOfIgnoreCase(String, String, int) to lastIndexOfIgnoreCase(CharSequence, CharSequence, int) </returns>
        //      //JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
        //      //ORIGINAL LINE: public static int lastIndexOfIgnoreCase(final CharSequence str, final CharSequence searchStr, int startPos)
        //      public static int lastIndexOfIgnoreCase(CharSequence str, CharSequence searchStr, int startPos)
        //      {
        //          if (str == null || searchStr == null)
        //          {
        //              return INDEX_NOT_FOUND;
        //          }
        //          if (startPos > str.length() - searchStr.length())
        //          {
        //              startPos = str.length() - searchStr.length();
        //          }
        //          if (startPos < 0)
        //          {
        //              return INDEX_NOT_FOUND;
        //          }
        //          if (searchStr.length() == 0)
        //          {
        //              return startPos;
        //          }

        //          for (int i = startPos; i >= 0; i--)
        //          {
        //              if (CharSequenceUtils.regionMatches(str, true, i, searchStr, 0, searchStr.length()))
        //              {
        //                  return i;
        //              }
        //          }
        //          return INDEX_NOT_FOUND;
        //      }

        //      // Contains
        //      //-----------------------------------------------------------------------
        //      /// <summary>
        //      /// <para>Checks if CharSequence contains a search character, handling {@code null}.
        //      /// This method uses <seealso cref="String#indexOf(int)"/> if possible.</para>
        //      /// 
        //      /// <para>A {@code null} or empty ("") CharSequence will return {@code false}.</para>
        //      /// 
        //      /// <pre>
        //      /// StringUtils.contains(null, *)    = false
        //      /// StringUtils.contains("", *)      = false
        //      /// StringUtils.contains("abc", 'a') = true
        //      /// StringUtils.contains("abc", 'z') = false
        //      /// </pre>
        //      /// </summary>
        //      /// <param name="seq">  the CharSequence to check, may be null </param>
        //      /// <param name="searchChar">  the character to find </param>
        //      /// <returns> true if the CharSequence contains the search character,
        //      ///  false if not or {@code null} string input
        //      /// @since 2.0
        //      /// @since 3.0 Changed signature from contains(String, int) to contains(CharSequence, int) </returns>
        //      //JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
        //      //ORIGINAL LINE: public static boolean contains(final CharSequence seq, final int searchChar)
        //      public static bool contains(CharSequence seq, int searchChar)
        //      {
        //          if (isEmpty(seq))
        //          {
        //              return false;
        //          }
        //          return CharSequenceUtils.IndexOf(seq, searchChar, 0) >= 0;
        //      }

        //      /// <summary>
        //      /// <para>Checks if CharSequence contains a search CharSequence, handling {@code null}.
        //      /// This method uses <seealso cref="String#indexOf(String)"/> if possible.</para>
        //      /// 
        //      /// <para>A {@code null} CharSequence will return {@code false}.</para>
        //      /// 
        //      /// <pre>
        //      /// StringUtils.contains(null, *)     = false
        //      /// StringUtils.contains(*, null)     = false
        //      /// StringUtils.contains("", "")      = true
        //      /// StringUtils.contains("abc", "")   = true
        //      /// StringUtils.contains("abc", "a")  = true
        //      /// StringUtils.contains("abc", "z")  = false
        //      /// </pre>
        //      /// </summary>
        //      /// <param name="seq">  the CharSequence to check, may be null </param>
        //      /// <param name="searchSeq">  the CharSequence to find, may be null </param>
        //      /// <returns> true if the CharSequence contains the search CharSequence,
        //      ///  false if not or {@code null} string input
        //      /// @since 2.0
        //      /// @since 3.0 Changed signature from contains(String, String) to contains(CharSequence, CharSequence) </returns>
        //      //JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
        //      //ORIGINAL LINE: public static boolean contains(final CharSequence seq, final CharSequence searchSeq)
        //      public static bool contains(CharSequence seq, CharSequence searchSeq)
        //      {
        //          if (seq == null || searchSeq == null)
        //          {
        //              return false;
        //          }
        //          return CharSequenceUtils.IndexOf(seq, searchSeq, 0) >= 0;
        //      }

        //      /// <summary>
        //      /// <para>Checks if CharSequence contains a search CharSequence irrespective of case,
        //      /// handling {@code null}. Case-insensitivity is defined as by
        //      /// <seealso cref="String#equalsIgnoreCase(String)"/>.
        //      /// 
        //      /// </para>
        //      /// <para>A {@code null} CharSequence will return {@code false}.</para>
        //      /// 
        //      /// <pre>
        //      /// StringUtils.containsIgnoreCase(null, *) = false
        //      /// StringUtils.containsIgnoreCase(*, null) = false
        //      /// StringUtils.containsIgnoreCase("", "") = true
        //      /// StringUtils.containsIgnoreCase("abc", "") = true
        //      /// StringUtils.containsIgnoreCase("abc", "a") = true
        //      /// StringUtils.containsIgnoreCase("abc", "z") = false
        //      /// StringUtils.containsIgnoreCase("abc", "A") = true
        //      /// StringUtils.containsIgnoreCase("abc", "Z") = false
        //      /// </pre>
        //      /// </summary>
        //      /// <param name="str">  the CharSequence to check, may be null </param>
        //      /// <param name="searchStr">  the CharSequence to find, may be null </param>
        //      /// <returns> true if the CharSequence contains the search CharSequence irrespective of
        //      /// case or false if not or {@code null} string input
        //      /// @since 3.0 Changed signature from containsIgnoreCase(String, String) to containsIgnoreCase(CharSequence, CharSequence) </returns>
        //      //JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
        //      //ORIGINAL LINE: public static boolean containsIgnoreCase(final CharSequence str, final CharSequence searchStr)
        //      public static bool containsIgnoreCase(CharSequence str, CharSequence searchStr)
        //      {
        //          if (str == null || searchStr == null)
        //          {
        //              return false;
        //          }
        //          //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
        //          //ORIGINAL LINE: final int len = searchStr.length();
        //          int len = searchStr.length();
        //          //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
        //          //ORIGINAL LINE: final int max = str.length() - len;
        //          int max = str.length() - len;
        //          for (int i = 0; i <= max; i++)
        //          {
        //              if (CharSequenceUtils.regionMatches(str, true, i, searchStr, 0, len))
        //              {
        //                  return true;
        //              }
        //          }
        //          return false;
        //      }

        //      /// <summary>
        //      /// <para>Check whether the given CharSequence contains any whitespace characters.</para>
        //      /// 
        //      /// </p>Whitespace is defined by <seealso cref="Character#isWhitespace(char)"/>.</p>
        //      /// </summary>
        //      /// <param name="seq"> the CharSequence to check (may be {@code null}) </param>
        //      /// <returns> {@code true} if the CharSequence is not empty and
        //      /// contains at least 1 (breaking) whitespace character
        //      /// @since 3.0 </returns>
        //      // From org.springframework.util.StringUtils, under Apache License 2.0
        //      //JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
        //      //ORIGINAL LINE: public static boolean containsWhitespace(final CharSequence seq)
        //      public static bool containsWhitespace(CharSequence seq)
        //      {
        //          if (isEmpty(seq))
        //          {
        //              return false;
        //          }
        //          //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
        //          //ORIGINAL LINE: final int strLen = seq.length();
        //          int strLen = seq.length();
        //          for (int i = 0; i < strLen; i++)
        //          {
        //              if (char.IsWhiteSpace(seq.charAt(i)))
        //              {
        //                  return true;
        //              }
        //          }
        //          return false;
        //      }

        //      // IndexOfAny chars
        //      //-----------------------------------------------------------------------
        //      /// <summary>
        //      /// <para>Search a CharSequence to find the first index of any
        //      /// character in the given set of characters.</para>
        //      /// 
        //      /// <para>A {@code null} String will return {@code -1}.
        //      /// A {@code null} or zero length search array will return {@code -1}.</para>
        //      /// 
        //      /// <pre>
        //      /// StringUtils.indexOfAny(null, *)                = -1
        //      /// StringUtils.indexOfAny("", *)                  = -1
        //      /// StringUtils.indexOfAny(*, null)                = -1
        //      /// StringUtils.indexOfAny(*, [])                  = -1
        //      /// StringUtils.indexOfAny("zzabyycdxx",['z','a']) = 0
        //      /// StringUtils.indexOfAny("zzabyycdxx",['b','y']) = 3
        //      /// StringUtils.indexOfAny("aba", ['z'])           = -1
        //      /// </pre>
        //      /// </summary>
        //      /// <param name="cs">  the CharSequence to check, may be null </param>
        //      /// <param name="searchChars">  the chars to search for, may be null </param>
        //      /// <returns> the index of any of the chars, -1 if no match or null input
        //      /// @since 2.0
        //      /// @since 3.0 Changed signature from indexOfAny(String, char[]) to indexOfAny(CharSequence, char...) </returns>
        //      //JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
        //      //ORIGINAL LINE: public static int indexOfAny(final CharSequence cs, final char... searchChars)
        //      public static int indexOfAny(CharSequence cs, params char[] searchChars)
        //      {
        //          if (isEmpty(cs) || ArrayUtils.isEmpty(searchChars))
        //          {
        //              return INDEX_NOT_FOUND;
        //          }
        //          //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
        //          //ORIGINAL LINE: final int csLen = cs.length();
        //          int csLen = cs.length();
        //          //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
        //          //ORIGINAL LINE: final int csLast = csLen - 1;
        //          int csLast = csLen - 1;
        //          //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
        //          //ORIGINAL LINE: final int searchLen = searchChars.length;
        //          int searchLen = searchChars.Length;
        //          //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
        //          //ORIGINAL LINE: final int searchLast = searchLen - 1;
        //          int searchLast = searchLen - 1;
        //          for (int i = 0; i < csLen; i++)
        //          {
        //              //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
        //              //ORIGINAL LINE: final char ch = cs.charAt(i);
        //              char ch = cs.charAt(i);
        //              for (int j = 0; j < searchLen; j++)
        //              {
        //                  if (searchChars[j] == ch)
        //                  {
        //                      if (i < csLast && j < searchLast && char.IsHighSurrogate(ch))
        //                      {
        //                          // ch is a supplementary character
        //                          if (searchChars[j + 1] == cs.charAt(i + 1))
        //                          {
        //                              return i;
        //                          }
        //                      }
        //                      else
        //                      {
        //                          return i;
        //                      }
        //                  }
        //              }
        //          }
        //          return INDEX_NOT_FOUND;
        //      }

        //      /// <summary>
        //      /// <para>Search a CharSequence to find the first index of any
        //      /// character in the given set of characters.</para>
        //      /// 
        //      /// <para>A {@code null} String will return {@code -1}.
        //      /// A {@code null} search string will return {@code -1}.</para>
        //      /// 
        //      /// <pre>
        //      /// StringUtils.indexOfAny(null, *)            = -1
        //      /// StringUtils.indexOfAny("", *)              = -1
        //      /// StringUtils.indexOfAny(*, null)            = -1
        //      /// StringUtils.indexOfAny(*, "")              = -1
        //      /// StringUtils.indexOfAny("zzabyycdxx", "za") = 0
        //      /// StringUtils.indexOfAny("zzabyycdxx", "by") = 3
        //      /// StringUtils.indexOfAny("aba","z")          = -1
        //      /// </pre>
        //      /// </summary>
        //      /// <param name="cs">  the CharSequence to check, may be null </param>
        //      /// <param name="searchChars">  the chars to search for, may be null </param>
        //      /// <returns> the index of any of the chars, -1 if no match or null input
        //      /// @since 2.0
        //      /// @since 3.0 Changed signature from indexOfAny(String, String) to indexOfAny(CharSequence, String) </returns>
        //      //JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
        //      //ORIGINAL LINE: public static int indexOfAny(final CharSequence cs, final String searchChars)
        //      public static int indexOfAny(CharSequence cs, string searchChars)
        //      {
        //          if (isEmpty(cs) || isEmpty(searchChars))
        //          {
        //              return INDEX_NOT_FOUND;
        //          }
        //          return indexOfAny(cs, searchChars.ToCharArray());
        //      }

        //      // ContainsAny
        //      //-----------------------------------------------------------------------
        //      /// <summary>
        //      /// <para>Checks if the CharSequence contains any character in the given
        //      /// set of characters.</para>
        //      /// 
        //      /// <para>A {@code null} CharSequence will return {@code false}.
        //      /// A {@code null} or zero length search array will return {@code false}.</para>
        //      /// 
        //      /// <pre>
        //      /// StringUtils.containsAny(null, *)                = false
        //      /// StringUtils.containsAny("", *)                  = false
        //      /// StringUtils.containsAny(*, null)                = false
        //      /// StringUtils.containsAny(*, [])                  = false
        //      /// StringUtils.containsAny("zzabyycdxx",['z','a']) = true
        //      /// StringUtils.containsAny("zzabyycdxx",['b','y']) = true
        //      /// StringUtils.containsAny("zzabyycdxx",['z','y']) = true
        //      /// StringUtils.containsAny("aba", ['z'])           = false
        //      /// </pre>
        //      /// </summary>
        //      /// <param name="cs">  the CharSequence to check, may be null </param>
        //      /// <param name="searchChars">  the chars to search for, may be null </param>
        //      /// <returns> the {@code true} if any of the chars are found,
        //      /// {@code false} if no match or null input
        //      /// @since 2.4
        //      /// @since 3.0 Changed signature from containsAny(String, char[]) to containsAny(CharSequence, char...) </returns>
        //      //JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
        //      //ORIGINAL LINE: public static boolean containsAny(final CharSequence cs, final char... searchChars)
        //      public static bool containsAny(CharSequence cs, params char[] searchChars)
        //      {
        //          if (isEmpty(cs) || ArrayUtils.isEmpty(searchChars))
        //          {
        //              return false;
        //          }
        //          //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
        //          //ORIGINAL LINE: final int csLength = cs.length();
        //          int csLength = cs.length();
        //          //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
        //          //ORIGINAL LINE: final int searchLength = searchChars.length;
        //          int searchLength = searchChars.Length;
        //          //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
        //          //ORIGINAL LINE: final int csLast = csLength - 1;
        //          int csLast = csLength - 1;
        //          //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
        //          //ORIGINAL LINE: final int searchLast = searchLength - 1;
        //          int searchLast = searchLength - 1;
        //          for (int i = 0; i < csLength; i++)
        //          {
        //              //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
        //              //ORIGINAL LINE: final char ch = cs.charAt(i);
        //              char ch = cs.charAt(i);
        //              for (int j = 0; j < searchLength; j++)
        //              {
        //                  if (searchChars[j] == ch)
        //                  {
        //                      if (char.IsHighSurrogate(ch))
        //                      {
        //                          if (j == searchLast)
        //                          {
        //                              // missing low surrogate, fine, like String.indexOf(String)
        //                              return true;
        //                          }
        //                          if (i < csLast && searchChars[j + 1] == cs.charAt(i + 1))
        //                          {
        //                              return true;
        //                          }
        //                      }
        //                      else
        //                      {
        //                          // ch is in the Basic Multilingual Plane
        //                          return true;
        //                      }
        //                  }
        //              }
        //          }
        //          return false;
        //      }

        //      /// <summary>
        //      /// <para>
        //      /// Checks if the CharSequence contains any character in the given set of characters.
        //      /// </para>
        //      /// 
        //      /// <para>
        //      /// A {@code null} CharSequence will return {@code false}. A {@code null} search CharSequence will return
        //      /// {@code false}.
        //      /// </para>
        //      /// 
        //      /// <pre>
        //      /// StringUtils.containsAny(null, *)               = false
        //      /// StringUtils.containsAny("", *)                 = false
        //      /// StringUtils.containsAny(*, null)               = false
        //      /// StringUtils.containsAny(*, "")                 = false
        //      /// StringUtils.containsAny("zzabyycdxx", "za")    = true
        //      /// StringUtils.containsAny("zzabyycdxx", "by")    = true
        //      /// StringUtils.containsAny("zzabyycdxx", "zy")    = true
        //      /// StringUtils.containsAny("zzabyycdxx", "\tx")   = true
        //      /// StringUtils.containsAny("zzabyycdxx", "$.#yF") = true
        //      /// StringUtils.containsAny("aba","z")             = false
        //      /// </pre>
        //      /// </summary>
        //      /// <param name="cs">
        //      ///            the CharSequence to check, may be null </param>
        //      /// <param name="searchChars">
        //      ///            the chars to search for, may be null </param>
        //      /// <returns> the {@code true} if any of the chars are found, {@code false} if no match or null input
        //      /// @since 2.4
        //      /// @since 3.0 Changed signature from containsAny(String, String) to containsAny(CharSequence, CharSequence) </returns>
        //      //JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
        //      //ORIGINAL LINE: public static boolean containsAny(final CharSequence cs, final CharSequence searchChars)
        //      public static bool containsAny(CharSequence cs, CharSequence searchChars)
        //      {
        //          if (searchChars == null)
        //          {
        //              return false;
        //          }
        //          return containsAny(cs, CharSequenceUtils.ToCharArray(searchChars));
        //      }

        //      /// <summary>
        //      /// <para>Checks if the CharSequence contains any of the CharSequences in the given array.</para>
        //      /// 
        //      /// <para>
        //      /// A {@code null} {@code cs} CharSequence will return {@code false}. A {@code null} or zero
        //      /// length search array will return {@code false}.
        //      /// </para>
        //      /// 
        //      /// <pre>
        //      /// StringUtils.containsAny(null, *)            = false
        //      /// StringUtils.containsAny("", *)              = false
        //      /// StringUtils.containsAny(*, null)            = false
        //      /// StringUtils.containsAny(*, [])              = false
        //      /// StringUtils.containsAny("abcd", "ab", null) = true
        //      /// StringUtils.containsAny("abcd", "ab", "cd") = true
        //      /// StringUtils.containsAny("abc", "d", "abc")  = true
        //      /// </pre>
        //      /// 
        //      /// </summary>
        //      /// <param name="cs"> The CharSequence to check, may be null </param>
        //      /// <param name="searchCharSequences"> The array of CharSequences to search for, may be null.
        //      /// Individual CharSequences may be null as well. </param>
        //      /// <returns> {@code true} if any of the search CharSequences are found, {@code false} otherwise
        //      /// @since 3.4 </returns>
        //      //JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
        //      //ORIGINAL LINE: public static boolean containsAny(final CharSequence cs, final CharSequence... searchCharSequences)
        //      public static bool containsAny(CharSequence cs, params CharSequence[] searchCharSequences)
        //      {
        //          if (isEmpty(cs) || ArrayUtils.isEmpty(searchCharSequences))
        //          {
        //              return false;
        //          }
        //          foreach (CharSequence searchCharSequence in searchCharSequences)
        //          {
        //              if (contains(cs, searchCharSequence))
        //              {
        //                  return true;
        //              }
        //          }
        //          return false;
        //      }

        //      // IndexOfAnyBut chars
        //      //-----------------------------------------------------------------------
        //      /// <summary>
        //      /// <para>Searches a CharSequence to find the first index of any
        //      /// character not in the given set of characters.</para>
        //      /// 
        //      /// <para>A {@code null} CharSequence will return {@code -1}.
        //      /// A {@code null} or zero length search array will return {@code -1}.</para>
        //      /// 
        //      /// <pre>
        //      /// StringUtils.indexOfAnyBut(null, *)                              = -1
        //      /// StringUtils.indexOfAnyBut("", *)                                = -1
        //      /// StringUtils.indexOfAnyBut(*, null)                              = -1
        //      /// StringUtils.indexOfAnyBut(*, [])                                = -1
        //      /// StringUtils.indexOfAnyBut("zzabyycdxx", new char[] {'z', 'a'} ) = 3
        //      /// StringUtils.indexOfAnyBut("aba", new char[] {'z'} )             = 0
        //      /// StringUtils.indexOfAnyBut("aba", new char[] {'a', 'b'} )        = -1
        //      /// 
        //      /// </pre>
        //      /// </summary>
        //      /// <param name="cs">  the CharSequence to check, may be null </param>
        //      /// <param name="searchChars">  the chars to search for, may be null </param>
        //      /// <returns> the index of any of the chars, -1 if no match or null input
        //      /// @since 2.0
        //      /// @since 3.0 Changed signature from indexOfAnyBut(String, char[]) to indexOfAnyBut(CharSequence, char...) </returns>
        //      //JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
        //      //ORIGINAL LINE: public static int indexOfAnyBut(final CharSequence cs, final char... searchChars)
        //      public static int indexOfAnyBut(CharSequence cs, params char[] searchChars)
        //      {
        //          if (isEmpty(cs) || ArrayUtils.isEmpty(searchChars))
        //          {
        //              return INDEX_NOT_FOUND;
        //          }
        //          //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
        //          //ORIGINAL LINE: final int csLen = cs.length();
        //          int csLen = cs.length();
        //          //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
        //          //ORIGINAL LINE: final int csLast = csLen - 1;
        //          int csLast = csLen - 1;
        //          //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
        //          //ORIGINAL LINE: final int searchLen = searchChars.length;
        //          int searchLen = searchChars.Length;
        //          //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
        //          //ORIGINAL LINE: final int searchLast = searchLen - 1;
        //          int searchLast = searchLen - 1;
        //          for (int i = 0; i < csLen; i++)
        //          {
        //              //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
        //              //ORIGINAL LINE: final char ch = cs.charAt(i);
        //              char ch = cs.charAt(i);
        //              for (int j = 0; j < searchLen; j++)
        //              {
        //                  if (searchChars[j] == ch)
        //                  {
        //                      if (i < csLast && j < searchLast && char.IsHighSurrogate(ch))
        //                      {
        //                          if (searchChars[j + 1] == cs.charAt(i + 1))
        //                          {
        //                              goto outerContinue;
        //                          }
        //                      }
        //                      else
        //                      {
        //                          goto outerContinue;
        //                      }
        //                  }
        //              }
        //              return i;
        //              outerContinue:;
        //          }
        //          outerBreak:
        //          return INDEX_NOT_FOUND;
        //      }

        //      /// <summary>
        //      /// <para>Search a CharSequence to find the first index of any
        //      /// character not in the given set of characters.</para>
        //      /// 
        //      /// <para>A {@code null} CharSequence will return {@code -1}.
        //      /// A {@code null} or empty search string will return {@code -1}.</para>
        //      /// 
        //      /// <pre>
        //      /// StringUtils.indexOfAnyBut(null, *)            = -1
        //      /// StringUtils.indexOfAnyBut("", *)              = -1
        //      /// StringUtils.indexOfAnyBut(*, null)            = -1
        //      /// StringUtils.indexOfAnyBut(*, "")              = -1
        //      /// StringUtils.indexOfAnyBut("zzabyycdxx", "za") = 3
        //      /// StringUtils.indexOfAnyBut("zzabyycdxx", "")   = -1
        //      /// StringUtils.indexOfAnyBut("aba","ab")         = -1
        //      /// </pre>
        //      /// </summary>
        //      /// <param name="seq">  the CharSequence to check, may be null </param>
        //      /// <param name="searchChars">  the chars to search for, may be null </param>
        //      /// <returns> the index of any of the chars, -1 if no match or null input
        //      /// @since 2.0
        //      /// @since 3.0 Changed signature from indexOfAnyBut(String, String) to indexOfAnyBut(CharSequence, CharSequence) </returns>
        //      //JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
        //      //ORIGINAL LINE: public static int indexOfAnyBut(final CharSequence seq, final CharSequence searchChars)
        //      public static int indexOfAnyBut(CharSequence seq, CharSequence searchChars)
        //      {
        //          if (isEmpty(seq) || isEmpty(searchChars))
        //          {
        //              return INDEX_NOT_FOUND;
        //          }
        //          //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
        //          //ORIGINAL LINE: final int strLen = seq.length();
        //          int strLen = seq.length();
        //          for (int i = 0; i < strLen; i++)
        //          {
        //              //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
        //              //ORIGINAL LINE: final char ch = seq.charAt(i);
        //              char ch = seq.charAt(i);
        //              //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
        //              //ORIGINAL LINE: final boolean chFound = CharSequenceUtils.indexOf(searchChars, ch, 0) >= 0;
        //              bool chFound = CharSequenceUtils.IndexOf(searchChars, ch, 0) >= 0;
        //              if (i + 1 < strLen && char.IsHighSurrogate(ch))
        //              {
        //                  //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
        //                  //ORIGINAL LINE: final char ch2 = seq.charAt(i + 1);
        //                  char ch2 = seq.charAt(i + 1);
        //                  if (chFound && CharSequenceUtils.IndexOf(searchChars, ch2, 0) < 0)
        //                  {
        //                      return i;
        //                  }
        //              }
        //              else
        //              {
        //                  if (!chFound)
        //                  {
        //                      return i;
        //                  }
        //              }
        //          }
        //          return INDEX_NOT_FOUND;
        //      }

        //      // ContainsOnly
        //      //-----------------------------------------------------------------------
        //      /// <summary>
        //      /// <para>Checks if the CharSequence contains only certain characters.</para>
        //      /// 
        //      /// <para>A {@code null} CharSequence will return {@code false}.
        //      /// A {@code null} valid character array will return {@code false}.
        //      /// An empty CharSequence (length()=0) always returns {@code true}.</para>
        //      /// 
        //      /// <pre>
        //      /// StringUtils.containsOnly(null, *)       = false
        //      /// StringUtils.containsOnly(*, null)       = false
        //      /// StringUtils.containsOnly("", *)         = true
        //      /// StringUtils.containsOnly("ab", '')      = false
        //      /// StringUtils.containsOnly("abab", 'abc') = true
        //      /// StringUtils.containsOnly("ab1", 'abc')  = false
        //      /// StringUtils.containsOnly("abz", 'abc')  = false
        //      /// </pre>
        //      /// </summary>
        //      /// <param name="cs">  the String to check, may be null </param>
        //      /// <param name="valid">  an array of valid chars, may be null </param>
        //      /// <returns> true if it only contains valid chars and is non-null
        //      /// @since 3.0 Changed signature from containsOnly(String, char[]) to containsOnly(CharSequence, char...) </returns>
        //      //JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
        //      //ORIGINAL LINE: public static boolean containsOnly(final CharSequence cs, final char... valid)
        //      public static bool containsOnly(CharSequence cs, params char[] valid)
        //      {
        //          // All these pre-checks are to maintain API with an older version
        //          if (valid == null || cs == null)
        //          {
        //              return false;
        //          }
        //          if (cs.length() == 0)
        //          {
        //              return true;
        //          }
        //          if (valid.Length == 0)
        //          {
        //              return false;
        //          }
        //          return indexOfAnyBut(cs, valid) == INDEX_NOT_FOUND;
        //      }

        //      /// <summary>
        //      /// <para>Checks if the CharSequence contains only certain characters.</para>
        //      /// 
        //      /// <para>A {@code null} CharSequence will return {@code false}.
        //      /// A {@code null} valid character String will return {@code false}.
        //      /// An empty String (length()=0) always returns {@code true}.</para>
        //      /// 
        //      /// <pre>
        //      /// StringUtils.containsOnly(null, *)       = false
        //      /// StringUtils.containsOnly(*, null)       = false
        //      /// StringUtils.containsOnly("", *)         = true
        //      /// StringUtils.containsOnly("ab", "")      = false
        //      /// StringUtils.containsOnly("abab", "abc") = true
        //      /// StringUtils.containsOnly("ab1", "abc")  = false
        //      /// StringUtils.containsOnly("abz", "abc")  = false
        //      /// </pre>
        //      /// </summary>
        //      /// <param name="cs">  the CharSequence to check, may be null </param>
        //      /// <param name="validChars">  a String of valid chars, may be null </param>
        //      /// <returns> true if it only contains valid chars and is non-null
        //      /// @since 2.0
        //      /// @since 3.0 Changed signature from containsOnly(String, String) to containsOnly(CharSequence, String) </returns>
        //      //JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
        //      //ORIGINAL LINE: public static boolean containsOnly(final CharSequence cs, final String validChars)
        //      public static bool containsOnly(CharSequence cs, string validChars)
        //      {
        //          if (cs == null || string.ReferenceEquals(validChars, null))
        //          {
        //              return false;
        //          }
        //          return containsOnly(cs, validChars.ToCharArray());
        //      }

        //      // ContainsNone
        //      //-----------------------------------------------------------------------
        //      /// <summary>
        //      /// <para>Checks that the CharSequence does not contain certain characters.</para>
        //      /// 
        //      /// <para>A {@code null} CharSequence will return {@code true}.
        //      /// A {@code null} invalid character array will return {@code true}.
        //      /// An empty CharSequence (length()=0) always returns true.</para>
        //      /// 
        //      /// <pre>
        //      /// StringUtils.containsNone(null, *)       = true
        //      /// StringUtils.containsNone(*, null)       = true
        //      /// StringUtils.containsNone("", *)         = true
        //      /// StringUtils.containsNone("ab", '')      = true
        //      /// StringUtils.containsNone("abab", 'xyz') = true
        //      /// StringUtils.containsNone("ab1", 'xyz')  = true
        //      /// StringUtils.containsNone("abz", 'xyz')  = false
        //      /// </pre>
        //      /// </summary>
        //      /// <param name="cs">  the CharSequence to check, may be null </param>
        //      /// <param name="searchChars">  an array of invalid chars, may be null </param>
        //      /// <returns> true if it contains none of the invalid chars, or is null
        //      /// @since 2.0
        //      /// @since 3.0 Changed signature from containsNone(String, char[]) to containsNone(CharSequence, char...) </returns>
        //      //JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
        //      //ORIGINAL LINE: public static boolean containsNone(final CharSequence cs, final char... searchChars)
        //      public static bool containsNone(CharSequence cs, params char[] searchChars)
        //      {
        //          if (cs == null || searchChars == null)
        //          {
        //              return true;
        //          }
        //          //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
        //          //ORIGINAL LINE: final int csLen = cs.length();
        //          int csLen = cs.length();
        //          //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
        //          //ORIGINAL LINE: final int csLast = csLen - 1;
        //          int csLast = csLen - 1;
        //          //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
        //          //ORIGINAL LINE: final int searchLen = searchChars.length;
        //          int searchLen = searchChars.Length;
        //          //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
        //          //ORIGINAL LINE: final int searchLast = searchLen - 1;
        //          int searchLast = searchLen - 1;
        //          for (int i = 0; i < csLen; i++)
        //          {
        //              //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
        //              //ORIGINAL LINE: final char ch = cs.charAt(i);
        //              char ch = cs.charAt(i);
        //              for (int j = 0; j < searchLen; j++)
        //              {
        //                  if (searchChars[j] == ch)
        //                  {
        //                      if (char.IsHighSurrogate(ch))
        //                      {
        //                          if (j == searchLast)
        //                          {
        //                              // missing low surrogate, fine, like String.indexOf(String)
        //                              return false;
        //                          }
        //                          if (i < csLast && searchChars[j + 1] == cs.charAt(i + 1))
        //                          {
        //                              return false;
        //                          }
        //                      }
        //                      else
        //                      {
        //                          // ch is in the Basic Multilingual Plane
        //                          return false;
        //                      }
        //                  }
        //              }
        //          }
        //          return true;
        //      }

        //      /// <summary>
        //      /// <para>Checks that the CharSequence does not contain certain characters.</para>
        //      /// 
        //      /// <para>A {@code null} CharSequence will return {@code true}.
        //      /// A {@code null} invalid character array will return {@code true}.
        //      /// An empty String ("") always returns true.</para>
        //      /// 
        //      /// <pre>
        //      /// StringUtils.containsNone(null, *)       = true
        //      /// StringUtils.containsNone(*, null)       = true
        //      /// StringUtils.containsNone("", *)         = true
        //      /// StringUtils.containsNone("ab", "")      = true
        //      /// StringUtils.containsNone("abab", "xyz") = true
        //      /// StringUtils.containsNone("ab1", "xyz")  = true
        //      /// StringUtils.containsNone("abz", "xyz")  = false
        //      /// </pre>
        //      /// </summary>
        //      /// <param name="cs">  the CharSequence to check, may be null </param>
        //      /// <param name="invalidChars">  a String of invalid chars, may be null </param>
        //      /// <returns> true if it contains none of the invalid chars, or is null
        //      /// @since 2.0
        //      /// @since 3.0 Changed signature from containsNone(String, String) to containsNone(CharSequence, String) </returns>
        //      //JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
        //      //ORIGINAL LINE: public static boolean containsNone(final CharSequence cs, final String invalidChars)
        //      public static bool containsNone(CharSequence cs, string invalidChars)
        //      {
        //          if (cs == null || string.ReferenceEquals(invalidChars, null))
        //          {
        //              return true;
        //          }
        //          return containsNone(cs, invalidChars.ToCharArray());
        //      }

        //      // IndexOfAny strings
        //      //-----------------------------------------------------------------------
        //      /// <summary>
        //      /// <para>Find the first index of any of a set of potential substrings.</para>
        //      /// 
        //      /// <para>A {@code null} CharSequence will return {@code -1}.
        //      /// A {@code null} or zero length search array will return {@code -1}.
        //      /// A {@code null} search array entry will be ignored, but a search
        //      /// array containing "" will return {@code 0} if {@code str} is not
        //      /// null. This method uses <seealso cref="String#indexOf(String)"/> if possible.</para>
        //      /// 
        //      /// <pre>
        //      /// StringUtils.indexOfAny(null, *)                     = -1
        //      /// StringUtils.indexOfAny(*, null)                     = -1
        //      /// StringUtils.indexOfAny(*, [])                       = -1
        //      /// StringUtils.indexOfAny("zzabyycdxx", ["ab","cd"])   = 2
        //      /// StringUtils.indexOfAny("zzabyycdxx", ["cd","ab"])   = 2
        //      /// StringUtils.indexOfAny("zzabyycdxx", ["mn","op"])   = -1
        //      /// StringUtils.indexOfAny("zzabyycdxx", ["zab","aby"]) = 1
        //      /// StringUtils.indexOfAny("zzabyycdxx", [""])          = 0
        //      /// StringUtils.indexOfAny("", [""])                    = 0
        //      /// StringUtils.indexOfAny("", ["a"])                   = -1
        //      /// </pre>
        //      /// </summary>
        //      /// <param name="str">  the CharSequence to check, may be null </param>
        //      /// <param name="searchStrs">  the CharSequences to search for, may be null </param>
        //      /// <returns> the first index of any of the searchStrs in str, -1 if no match
        //      /// @since 3.0 Changed signature from indexOfAny(String, String[]) to indexOfAny(CharSequence, CharSequence...) </returns>
        //      //JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
        //      //ORIGINAL LINE: public static int indexOfAny(final CharSequence str, final CharSequence... searchStrs)
        //      public static int indexOfAny(CharSequence str, params CharSequence[] searchStrs)
        //      {
        //          if (str == null || searchStrs == null)
        //          {
        //              return INDEX_NOT_FOUND;
        //          }
        //          //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
        //          //ORIGINAL LINE: final int sz = searchStrs.length;
        //          int sz = searchStrs.Length;

        //          // String's can't have a MAX_VALUEth index.
        //          int ret = int.MaxValue;

        //          int tmp = 0;
        //          for (int i = 0; i < sz; i++)
        //          {
        //              //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
        //              //ORIGINAL LINE: final CharSequence search = searchStrs[i];
        //              CharSequence search = searchStrs[i];
        //              if (search == null)
        //              {
        //                  continue;
        //              }
        //              tmp = CharSequenceUtils.IndexOf(str, search, 0);
        //              if (tmp == INDEX_NOT_FOUND)
        //              {
        //                  continue;
        //              }

        //              if (tmp < ret)
        //              {
        //                  ret = tmp;
        //              }
        //          }

        //          return ret == int.MaxValue ? INDEX_NOT_FOUND : ret;
        //      }

        //      /// <summary>
        //      /// <para>Find the latest index of any of a set of potential substrings.</para>
        //      /// 
        //      /// <para>A {@code null} CharSequence will return {@code -1}.
        //      /// A {@code null} search array will return {@code -1}.
        //      /// A {@code null} or zero length search array entry will be ignored,
        //      /// but a search array containing "" will return the length of {@code str}
        //      /// if {@code str} is not null. This method uses <seealso cref="String#indexOf(String)"/> if possible</para>
        //      /// 
        //      /// <pre>
        //      /// StringUtils.lastIndexOfAny(null, *)                   = -1
        //      /// StringUtils.lastIndexOfAny(*, null)                   = -1
        //      /// StringUtils.lastIndexOfAny(*, [])                     = -1
        //      /// StringUtils.lastIndexOfAny(*, [null])                 = -1
        //      /// StringUtils.lastIndexOfAny("zzabyycdxx", ["ab","cd"]) = 6
        //      /// StringUtils.lastIndexOfAny("zzabyycdxx", ["cd","ab"]) = 6
        //      /// StringUtils.lastIndexOfAny("zzabyycdxx", ["mn","op"]) = -1
        //      /// StringUtils.lastIndexOfAny("zzabyycdxx", ["mn","op"]) = -1
        //      /// StringUtils.lastIndexOfAny("zzabyycdxx", ["mn",""])   = 10
        //      /// </pre>
        //      /// </summary>
        //      /// <param name="str">  the CharSequence to check, may be null </param>
        //      /// <param name="searchStrs">  the CharSequences to search for, may be null </param>
        //      /// <returns> the last index of any of the CharSequences, -1 if no match
        //      /// @since 3.0 Changed signature from lastIndexOfAny(String, String[]) to lastIndexOfAny(CharSequence, CharSequence) </returns>
        //      //JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
        //      //ORIGINAL LINE: public static int lastIndexOfAny(final CharSequence str, final CharSequence... searchStrs)
        //      public static int lastIndexOfAny(CharSequence str, params CharSequence[] searchStrs)
        //      {
        //          if (str == null || searchStrs == null)
        //          {
        //              return INDEX_NOT_FOUND;
        //          }
        //          //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
        //          //ORIGINAL LINE: final int sz = searchStrs.length;
        //          int sz = searchStrs.Length;
        //          int ret = INDEX_NOT_FOUND;
        //          int tmp = 0;
        //          for (int i = 0; i < sz; i++)
        //          {
        //              //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
        //              //ORIGINAL LINE: final CharSequence search = searchStrs[i];
        //              CharSequence search = searchStrs[i];
        //              if (search == null)
        //              {
        //                  continue;
        //              }
        //              tmp = CharSequenceUtils.LastIndexOf(str, search, str.length());
        //              if (tmp > ret)
        //              {
        //                  ret = tmp;
        //              }
        //          }
        //          return ret;
        //      }

        //      // Substring
        //      //-----------------------------------------------------------------------
        //      /// <summary>
        //      /// <para>Gets a substring from the specified String avoiding exceptions.</para>
        //      /// 
        //      /// <para>A negative start position can be used to start {@code n}
        //      /// characters from the end of the String.</para>
        //      /// 
        //      /// <para>A {@code null} String will return {@code null}.
        //      /// An empty ("") String will return "".</para>
        //      /// 
        //      /// <pre>
        //      /// StringUtils.substring(null, *)   = null
        //      /// StringUtils.substring("", *)     = ""
        //      /// StringUtils.substring("abc", 0)  = "abc"
        //      /// StringUtils.substring("abc", 2)  = "c"
        //      /// StringUtils.substring("abc", 4)  = ""
        //      /// StringUtils.substring("abc", -2) = "bc"
        //      /// StringUtils.substring("abc", -4) = "abc"
        //      /// </pre>
        //      /// </summary>
        //      /// <param name="str">  the String to get the substring from, may be null </param>
        //      /// <param name="start">  the position to start from, negative means
        //      ///  count back from the end of the String by this many characters </param>
        //      /// <returns> substring from start position, {@code null} if null String input </returns>
        //      //JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
        //      //ORIGINAL LINE: public static String substring(final String str, int start)
        //      public static string substring(string str, int start)
        //      {
        //          if (string.ReferenceEquals(str, null))
        //          {
        //              return null;
        //          }

        //          // handle negatives, which means last n characters
        //          if (start < 0)
        //          {
        //              start = str.Length + start; // remember start is negative
        //          }

        //          if (start < 0)
        //          {
        //              start = 0;
        //          }
        //          if (start > str.Length)
        //          {
        //              return EMPTY;
        //          }

        //          return str.Substring(start);
        //      }

        //      /// <summary>
        //      /// <para>Gets a substring from the specified String avoiding exceptions.</para>
        //      /// 
        //      /// <para>A negative start position can be used to start/end {@code n}
        //      /// characters from the end of the String.</para>
        //      /// 
        //      /// <para>The returned substring starts with the character in the {@code start}
        //      /// position and ends before the {@code end} position. All position counting is
        //      /// zero-based -- i.e., to start at the beginning of the string use
        //      /// {@code start = 0}. Negative start and end positions can be used to
        //      /// specify offsets relative to the end of the String.</para>
        //      /// 
        //      /// <para>If {@code start} is not strictly to the left of {@code end}, ""
        //      /// is returned.</para>
        //      /// 
        //      /// <pre>
        //      /// StringUtils.substring(null, *, *)    = null
        //      /// StringUtils.substring("", * ,  *)    = "";
        //      /// StringUtils.substring("abc", 0, 2)   = "ab"
        //      /// StringUtils.substring("abc", 2, 0)   = ""
        //      /// StringUtils.substring("abc", 2, 4)   = "c"
        //      /// StringUtils.substring("abc", 4, 6)   = ""
        //      /// StringUtils.substring("abc", 2, 2)   = ""
        //      /// StringUtils.substring("abc", -2, -1) = "b"
        //      /// StringUtils.substring("abc", -4, 2)  = "ab"
        //      /// </pre>
        //      /// </summary>
        //      /// <param name="str">  the String to get the substring from, may be null </param>
        //      /// <param name="start">  the position to start from, negative means
        //      ///  count back from the end of the String by this many characters </param>
        //      /// <param name="end">  the position to end at (exclusive), negative means
        //      ///  count back from the end of the String by this many characters </param>
        //      /// <returns> substring from start position to end position,
        //      ///  {@code null} if null String input </returns>
        //      //JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
        //      //ORIGINAL LINE: public static String substring(final String str, int start, int end)
        //      public static string substring(string str, int start, int end)
        //      {
        //          if (string.ReferenceEquals(str, null))
        //          {
        //              return null;
        //          }

        //          // handle negatives
        //          if (end < 0)
        //          {
        //              end = str.Length + end; // remember end is negative
        //          }
        //          if (start < 0)
        //          {
        //              start = str.Length + start; // remember start is negative
        //          }

        //          // check length next
        //          if (end > str.Length)
        //          {
        //              end = str.Length;
        //          }

        //          // if start is greater than end, return ""
        //          if (start > end)
        //          {
        //              return EMPTY;
        //          }

        //          if (start < 0)
        //          {
        //              start = 0;
        //          }
        //          if (end < 0)
        //          {
        //              end = 0;
        //          }

        //          return str.Substring(start, end - start);
        //      }

        //      // Left/Right/Mid
        //      //-----------------------------------------------------------------------
        //      /// <summary>
        //      /// <para>Gets the leftmost {@code len} characters of a String.</para>
        //      /// 
        //      /// <para>If {@code len} characters are not available, or the
        //      /// String is {@code null}, the String will be returned without
        //      /// an exception. An empty String is returned if len is negative.</para>
        //      /// 
        //      /// <pre>
        //      /// StringUtils.left(null, *)    = null
        //      /// StringUtils.left(*, -ve)     = ""
        //      /// StringUtils.left("", *)      = ""
        //      /// StringUtils.left("abc", 0)   = ""
        //      /// StringUtils.left("abc", 2)   = "ab"
        //      /// StringUtils.left("abc", 4)   = "abc"
        //      /// </pre>
        //      /// </summary>
        //      /// <param name="str">  the String to get the leftmost characters from, may be null </param>
        //      /// <param name="len">  the length of the required String </param>
        //      /// <returns> the leftmost characters, {@code null} if null String input </returns>
        //      //JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
        //      //ORIGINAL LINE: public static String left(final String str, final int len)
        //      public static string left(string str, int len)
        //      {
        //          if (string.ReferenceEquals(str, null))
        //          {
        //              return null;
        //          }
        //          if (len < 0)
        //          {
        //              return EMPTY;
        //          }
        //          if (str.Length <= len)
        //          {
        //              return str;
        //          }
        //          return str.Substring(0, len);
        //      }

        //      /// <summary>
        //      /// <para>Gets the rightmost {@code len} characters of a String.</para>
        //      /// 
        //      /// <para>If {@code len} characters are not available, or the String
        //      /// is {@code null}, the String will be returned without an
        //      /// an exception. An empty String is returned if len is negative.</para>
        //      /// 
        //      /// <pre>
        //      /// StringUtils.right(null, *)    = null
        //      /// StringUtils.right(*, -ve)     = ""
        //      /// StringUtils.right("", *)      = ""
        //      /// StringUtils.right("abc", 0)   = ""
        //      /// StringUtils.right("abc", 2)   = "bc"
        //      /// StringUtils.right("abc", 4)   = "abc"
        //      /// </pre>
        //      /// </summary>
        //      /// <param name="str">  the String to get the rightmost characters from, may be null </param>
        //      /// <param name="len">  the length of the required String </param>
        //      /// <returns> the rightmost characters, {@code null} if null String input </returns>
        //      //JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
        //      //ORIGINAL LINE: public static String right(final String str, final int len)
        //      public static string right(string str, int len)
        //      {
        //          if (string.ReferenceEquals(str, null))
        //          {
        //              return null;
        //          }
        //          if (len < 0)
        //          {
        //              return EMPTY;
        //          }
        //          if (str.Length <= len)
        //          {
        //              return str;
        //          }
        //          return str.Substring(str.Length - len);
        //      }

        //      /// <summary>
        //      /// <para>Gets {@code len} characters from the middle of a String.</para>
        //      /// 
        //      /// <para>If {@code len} characters are not available, the remainder
        //      /// of the String will be returned without an exception. If the
        //      /// String is {@code null}, {@code null} will be returned.
        //      /// An empty String is returned if len is negative or exceeds the
        //      /// length of {@code str}.</para>
        //      /// 
        //      /// <pre>
        //      /// StringUtils.mid(null, *, *)    = null
        //      /// StringUtils.mid(*, *, -ve)     = ""
        //      /// StringUtils.mid("", 0, *)      = ""
        //      /// StringUtils.mid("abc", 0, 2)   = "ab"
        //      /// StringUtils.mid("abc", 0, 4)   = "abc"
        //      /// StringUtils.mid("abc", 2, 4)   = "c"
        //      /// StringUtils.mid("abc", 4, 2)   = ""
        //      /// StringUtils.mid("abc", -2, 2)  = "ab"
        //      /// </pre>
        //      /// </summary>
        //      /// <param name="str">  the String to get the characters from, may be null </param>
        //      /// <param name="pos">  the position to start from, negative treated as zero </param>
        //      /// <param name="len">  the length of the required String </param>
        //      /// <returns> the middle characters, {@code null} if null String input </returns>
        //      //JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
        //      //ORIGINAL LINE: public static String mid(final String str, int pos, final int len)
        //      public static string mid(string str, int pos, int len)
        //      {
        //          if (string.ReferenceEquals(str, null))
        //          {
        //              return null;
        //          }
        //          if (len < 0 || pos > str.Length)
        //          {
        //              return EMPTY;
        //          }
        //          if (pos < 0)
        //          {
        //              pos = 0;
        //          }
        //          if (str.Length <= pos + len)
        //          {
        //              return str.Substring(pos);
        //          }
        //          return str.Substring(pos, len);
        //      }

        //      // SubStringAfter/SubStringBefore
        //      //-----------------------------------------------------------------------
        //      /// <summary>
        //      /// <para>Gets the substring before the first occurrence of a separator.
        //      /// The separator is not returned.</para>
        //      /// 
        //      /// <para>A {@code null} string input will return {@code null}.
        //      /// An empty ("") string input will return the empty string.
        //      /// A {@code null} separator will return the input string.</para>
        //      /// 
        //      /// <para>If nothing is found, the string input is returned.</para>
        //      /// 
        //      /// <pre>
        //      /// StringUtils.substringBefore(null, *)      = null
        //      /// StringUtils.substringBefore("", *)        = ""
        //      /// StringUtils.substringBefore("abc", "a")   = ""
        //      /// StringUtils.substringBefore("abcba", "b") = "a"
        //      /// StringUtils.substringBefore("abc", "c")   = "ab"
        //      /// StringUtils.substringBefore("abc", "d")   = "abc"
        //      /// StringUtils.substringBefore("abc", "")    = ""
        //      /// StringUtils.substringBefore("abc", null)  = "abc"
        //      /// </pre>
        //      /// </summary>
        //      /// <param name="str">  the String to get a substring from, may be null </param>
        //      /// <param name="separator">  the String to search for, may be null </param>
        //      /// <returns> the substring before the first occurrence of the separator,
        //      ///  {@code null} if null String input
        //      /// @since 2.0 </returns>
        //      //JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
        //      //ORIGINAL LINE: public static String substringBefore(final String str, final String separator)
        //      public static string substringBefore(string str, string separator)
        //      {
        //          if (isEmpty(str) || string.ReferenceEquals(separator, null))
        //          {
        //              return str;
        //          }
        //          if (separator.Length == 0)
        //          {
        //              return EMPTY;
        //          }
        //          //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
        //          //ORIGINAL LINE: final int pos = str.indexOf(separator);
        //          int pos = str.IndexOf(separator, StringComparison.Ordinal);
        //          if (pos == INDEX_NOT_FOUND)
        //          {
        //              return str;
        //          }
        //          return str.Substring(0, pos);
        //      }

        //      /// <summary>
        //      /// <para>Gets the substring after the first occurrence of a separator.
        //      /// The separator is not returned.</para>
        //      /// 
        //      /// <para>A {@code null} string input will return {@code null}.
        //      /// An empty ("") string input will return the empty string.
        //      /// A {@code null} separator will return the empty string if the
        //      /// input string is not {@code null}.</para>
        //      /// 
        //      /// <para>If nothing is found, the empty string is returned.</para>
        //      /// 
        //      /// <pre>
        //      /// StringUtils.substringAfter(null, *)      = null
        //      /// StringUtils.substringAfter("", *)        = ""
        //      /// StringUtils.substringAfter(*, null)      = ""
        //      /// StringUtils.substringAfter("abc", "a")   = "bc"
        //      /// StringUtils.substringAfter("abcba", "b") = "cba"
        //      /// StringUtils.substringAfter("abc", "c")   = ""
        //      /// StringUtils.substringAfter("abc", "d")   = ""
        //      /// StringUtils.substringAfter("abc", "")    = "abc"
        //      /// </pre>
        //      /// </summary>
        //      /// <param name="str">  the String to get a substring from, may be null </param>
        //      /// <param name="separator">  the String to search for, may be null </param>
        //      /// <returns> the substring after the first occurrence of the separator,
        //      ///  {@code null} if null String input
        //      /// @since 2.0 </returns>
        //      //JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
        //      //ORIGINAL LINE: public static String substringAfter(final String str, final String separator)
        //      public static string substringAfter(string str, string separator)
        //      {
        //          if (isEmpty(str))
        //          {
        //              return str;
        //          }
        //          if (string.ReferenceEquals(separator, null))
        //          {
        //              return EMPTY;
        //          }
        //          //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
        //          //ORIGINAL LINE: final int pos = str.indexOf(separator);
        //          int pos = str.IndexOf(separator, StringComparison.Ordinal);
        //          if (pos == INDEX_NOT_FOUND)
        //          {
        //              return EMPTY;
        //          }
        //          return str.Substring(pos + separator.Length);
        //      }

        //      /// <summary>
        //      /// <para>Gets the substring before the last occurrence of a separator.
        //      /// The separator is not returned.</para>
        //      /// 
        //      /// <para>A {@code null} string input will return {@code null}.
        //      /// An empty ("") string input will return the empty string.
        //      /// An empty or {@code null} separator will return the input string.</para>
        //      /// 
        //      /// <para>If nothing is found, the string input is returned.</para>
        //      /// 
        //      /// <pre>
        //      /// StringUtils.substringBeforeLast(null, *)      = null
        //      /// StringUtils.substringBeforeLast("", *)        = ""
        //      /// StringUtils.substringBeforeLast("abcba", "b") = "abc"
        //      /// StringUtils.substringBeforeLast("abc", "c")   = "ab"
        //      /// StringUtils.substringBeforeLast("a", "a")     = ""
        //      /// StringUtils.substringBeforeLast("a", "z")     = "a"
        //      /// StringUtils.substringBeforeLast("a", null)    = "a"
        //      /// StringUtils.substringBeforeLast("a", "")      = "a"
        //      /// </pre>
        //      /// </summary>
        //      /// <param name="str">  the String to get a substring from, may be null </param>
        //      /// <param name="separator">  the String to search for, may be null </param>
        //      /// <returns> the substring before the last occurrence of the separator,
        //      ///  {@code null} if null String input
        //      /// @since 2.0 </returns>
        //      //JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
        //      //ORIGINAL LINE: public static String substringBeforeLast(final String str, final String separator)
        //      public static string substringBeforeLast(string str, string separator)
        //      {
        //          if (isEmpty(str) || isEmpty(separator))
        //          {
        //              return str;
        //          }
        //          //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
        //          //ORIGINAL LINE: final int pos = str.lastIndexOf(separator);
        //          int pos = str.LastIndexOf(separator, StringComparison.Ordinal);
        //          if (pos == INDEX_NOT_FOUND)
        //          {
        //              return str;
        //          }
        //          return str.Substring(0, pos);
        //      }

        //      /// <summary>
        //      /// <para>Gets the substring after the last occurrence of a separator.
        //      /// The separator is not returned.</para>
        //      /// 
        //      /// <para>A {@code null} string input will return {@code null}.
        //      /// An empty ("") string input will return the empty string.
        //      /// An empty or {@code null} separator will return the empty string if
        //      /// the input string is not {@code null}.</para>
        //      /// 
        //      /// <para>If nothing is found, the empty string is returned.</para>
        //      /// 
        //      /// <pre>
        //      /// StringUtils.substringAfterLast(null, *)      = null
        //      /// StringUtils.substringAfterLast("", *)        = ""
        //      /// StringUtils.substringAfterLast(*, "")        = ""
        //      /// StringUtils.substringAfterLast(*, null)      = ""
        //      /// StringUtils.substringAfterLast("abc", "a")   = "bc"
        //      /// StringUtils.substringAfterLast("abcba", "b") = "a"
        //      /// StringUtils.substringAfterLast("abc", "c")   = ""
        //      /// StringUtils.substringAfterLast("a", "a")     = ""
        //      /// StringUtils.substringAfterLast("a", "z")     = ""
        //      /// </pre>
        //      /// </summary>
        //      /// <param name="str">  the String to get a substring from, may be null </param>
        //      /// <param name="separator">  the String to search for, may be null </param>
        //      /// <returns> the substring after the last occurrence of the separator,
        //      ///  {@code null} if null String input
        //      /// @since 2.0 </returns>
        //      //JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
        //      //ORIGINAL LINE: public static String substringAfterLast(final String str, final String separator)
        //      public static string substringAfterLast(string str, string separator)
        //      {
        //          if (isEmpty(str))
        //          {
        //              return str;
        //          }
        //          if (isEmpty(separator))
        //          {
        //              return EMPTY;
        //          }
        //          //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
        //          //ORIGINAL LINE: final int pos = str.lastIndexOf(separator);
        //          int pos = str.LastIndexOf(separator, StringComparison.Ordinal);
        //          if (pos == INDEX_NOT_FOUND || pos == str.Length - separator.Length)
        //          {
        //              return EMPTY;
        //          }
        //          return str.Substring(pos + separator.Length);
        //      }

        //      // Substring between
        //      //-----------------------------------------------------------------------
        //      /// <summary>
        //      /// <para>Gets the String that is nested in between two instances of the
        //      /// same String.</para>
        //      /// 
        //      /// <para>A {@code null} input String returns {@code null}.
        //      /// A {@code null} tag returns {@code null}.</para>
        //      /// 
        //      /// <pre>
        //      /// StringUtils.substringBetween(null, *)            = null
        //      /// StringUtils.substringBetween("", "")             = ""
        //      /// StringUtils.substringBetween("", "tag")          = null
        //      /// StringUtils.substringBetween("tagabctag", null)  = null
        //      /// StringUtils.substringBetween("tagabctag", "")    = ""
        //      /// StringUtils.substringBetween("tagabctag", "tag") = "abc"
        //      /// </pre>
        //      /// </summary>
        //      /// <param name="str">  the String containing the substring, may be null </param>
        //      /// <param name="tag">  the String before and after the substring, may be null </param>
        //      /// <returns> the substring, {@code null} if no match
        //      /// @since 2.0 </returns>
        //      //JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
        //      //ORIGINAL LINE: public static String substringBetween(final String str, final String tag)
        //      public static string substringBetween(string str, string tag)
        //      {
        //          return substringBetween(str, tag, tag);
        //      }

        //      /// <summary>
        //      /// <para>Gets the String that is nested in between two Strings.
        //      /// Only the first match is returned.</para>
        //      /// 
        //      /// <para>A {@code null} input String returns {@code null}.
        //      /// A {@code null} open/close returns {@code null} (no match).
        //      /// An empty ("") open and close returns an empty string.</para>
        //      /// 
        //      /// <pre>
        //      /// StringUtils.substringBetween("wx[b]yz", "[", "]") = "b"
        //      /// StringUtils.substringBetween(null, *, *)          = null
        //      /// StringUtils.substringBetween(*, null, *)          = null
        //      /// StringUtils.substringBetween(*, *, null)          = null
        //      /// StringUtils.substringBetween("", "", "")          = ""
        //      /// StringUtils.substringBetween("", "", "]")         = null
        //      /// StringUtils.substringBetween("", "[", "]")        = null
        //      /// StringUtils.substringBetween("yabcz", "", "")     = ""
        //      /// StringUtils.substringBetween("yabcz", "y", "z")   = "abc"
        //      /// StringUtils.substringBetween("yabczyabcz", "y", "z")   = "abc"
        //      /// </pre>
        //      /// </summary>
        //      /// <param name="str">  the String containing the substring, may be null </param>
        //      /// <param name="open">  the String before the substring, may be null </param>
        //      /// <param name="close">  the String after the substring, may be null </param>
        //      /// <returns> the substring, {@code null} if no match
        //      /// @since 2.0 </returns>
        //      //JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
        //      //ORIGINAL LINE: public static String substringBetween(final String str, final String open, final String close)
        //      public static string substringBetween(string str, string open, string close)
        //      {
        //          if (string.ReferenceEquals(str, null) || string.ReferenceEquals(open, null) || string.ReferenceEquals(close, null))
        //          {
        //              return null;
        //          }
        //          //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
        //          //ORIGINAL LINE: final int start = str.indexOf(open);
        //          int start = str.IndexOf(open, StringComparison.Ordinal);
        //          if (start != INDEX_NOT_FOUND)
        //          {
        //              //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
        //              //ORIGINAL LINE: final int end = str.indexOf(close, start + open.length());
        //              int end = str.IndexOf(close, start + open.Length, StringComparison.Ordinal);
        //              if (end != INDEX_NOT_FOUND)
        //              {
        //                  return StringHelperClass.SubstringSpecial(str, start + open.Length, end);
        //              }
        //          }
        //          return null;
        //      }

        //      /// <summary>
        //      /// <para>Searches a String for substrings delimited by a start and end tag,
        //      /// returning all matching substrings in an array.</para>
        //      /// 
        //      /// <para>A {@code null} input String returns {@code null}.
        //      /// A {@code null} open/close returns {@code null} (no match).
        //      /// An empty ("") open/close returns {@code null} (no match).</para>
        //      /// 
        //      /// <pre>
        //      /// StringUtils.substringsBetween("[a][b][c]", "[", "]") = ["a","b","c"]
        //      /// StringUtils.substringsBetween(null, *, *)            = null
        //      /// StringUtils.substringsBetween(*, null, *)            = null
        //      /// StringUtils.substringsBetween(*, *, null)            = null
        //      /// StringUtils.substringsBetween("", "[", "]")          = []
        //      /// </pre>
        //      /// </summary>
        //      /// <param name="str">  the String containing the substrings, null returns null, empty returns empty </param>
        //      /// <param name="open">  the String identifying the start of the substring, empty returns null </param>
        //      /// <param name="close">  the String identifying the end of the substring, empty returns null </param>
        //      /// <returns> a String Array of substrings, or {@code null} if no match
        //      /// @since 2.3 </returns>
        //      //JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
        //      //ORIGINAL LINE: public static String[] substringsBetween(final String str, final String open, final String close)
        //      public static string[] substringsBetween(string str, string open, string close)
        //      {
        //          if (string.ReferenceEquals(str, null) || isEmpty(open) || isEmpty(close))
        //          {
        //              return null;
        //          }
        //          //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
        //          //ORIGINAL LINE: final int strLen = str.length();
        //          int strLen = str.Length;
        //          if (strLen == 0)
        //          {
        //              return ArrayUtils.EMPTY_STRING_ARRAY;
        //          }
        //          //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
        //          //ORIGINAL LINE: final int closeLen = close.length();
        //          int closeLen = close.Length;
        //          //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
        //          //ORIGINAL LINE: final int openLen = open.length();
        //          int openLen = open.Length;
        //          //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
        //          //ORIGINAL LINE: final java.util.List<String> list = new java.util.ArrayList<>();
        //          IList<string> list = new List<string>();
        //          int pos = 0;
        //          while (pos < strLen - closeLen)
        //          {
        //              int start = str.IndexOf(open, pos, StringComparison.Ordinal);
        //              if (start < 0)
        //              {
        //                  break;
        //              }
        //              start += openLen;
        //              //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
        //              //ORIGINAL LINE: final int end = str.indexOf(close, start);
        //              int end = str.IndexOf(close, start, StringComparison.Ordinal);
        //              if (end < 0)
        //              {
        //                  break;
        //              }
        //              list.Add(str.Substring(start, end - start));
        //              pos = end + closeLen;
        //          }
        //          if (list.Count == 0)
        //          {
        //              return null;
        //          }
        //          return list.ToArray();
        //      }

        //      // Nested extraction
        //      //-----------------------------------------------------------------------

        //      // Splitting
        //      //-----------------------------------------------------------------------
        //      /// <summary>
        //      /// <para>Splits the provided text into an array, using whitespace as the
        //      /// separator.
        //      /// Whitespace is defined by <seealso cref="Character#isWhitespace(char)"/>.</para>
        //      /// 
        //      /// <para>The separator is not included in the returned String array.
        //      /// Adjacent separators are treated as one separator.
        //      /// For more control over the split use the StrTokenizer class.</para>
        //      /// 
        //      /// <para>A {@code null} input String returns {@code null}.</para>
        //      /// 
        //      /// <pre>
        //      /// StringUtils.split(null)       = null
        //      /// StringUtils.split("")         = []
        //      /// StringUtils.split("abc def")  = ["abc", "def"]
        //      /// StringUtils.split("abc  def") = ["abc", "def"]
        //      /// StringUtils.split(" abc ")    = ["abc"]
        //      /// </pre>
        //      /// </summary>
        //      /// <param name="str">  the String to parse, may be null </param>
        //      /// <returns> an array of parsed Strings, {@code null} if null String input </returns>
        //      //JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
        //      //ORIGINAL LINE: public static String[] split(final String str)
        //      public static string[] split(string str)
        //      {
        //          return split(str, null, -1);
        //      }

        //      /// <summary>
        //      /// <para>Splits the provided text into an array, separator specified.
        //      /// This is an alternative to using StringTokenizer.</para>
        //      /// 
        //      /// <para>The separator is not included in the returned String array.
        //      /// Adjacent separators are treated as one separator.
        //      /// For more control over the split use the StrTokenizer class.</para>
        //      /// 
        //      /// <para>A {@code null} input String returns {@code null}.</para>
        //      /// 
        //      /// <pre>
        //      /// StringUtils.split(null, *)         = null
        //      /// StringUtils.split("", *)           = []
        //      /// StringUtils.split("a.b.c", '.')    = ["a", "b", "c"]
        //      /// StringUtils.split("a..b.c", '.')   = ["a", "b", "c"]
        //      /// StringUtils.split("a:b:c", '.')    = ["a:b:c"]
        //      /// StringUtils.split("a b c", ' ')    = ["a", "b", "c"]
        //      /// </pre>
        //      /// </summary>
        //      /// <param name="str">  the String to parse, may be null </param>
        //      /// <param name="separatorChar">  the character used as the delimiter </param>
        //      /// <returns> an array of parsed Strings, {@code null} if null String input
        //      /// @since 2.0 </returns>
        //      //JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
        //      //ORIGINAL LINE: public static String[] split(final String str, final char separatorChar)
        //      public static string[] split(string str, char separatorChar)
        //      {
        //          return splitWorker(str, separatorChar, false);
        //      }

        //      /// <summary>
        //      /// <para>Splits the provided text into an array, separators specified.
        //      /// This is an alternative to using StringTokenizer.</para>
        //      /// 
        //      /// <para>The separator is not included in the returned String array.
        //      /// Adjacent separators are treated as one separator.
        //      /// For more control over the split use the StrTokenizer class.</para>
        //      /// 
        //      /// <para>A {@code null} input String returns {@code null}.
        //      /// A {@code null} separatorChars splits on whitespace.</para>
        //      /// 
        //      /// <pre>
        //      /// StringUtils.split(null, *)         = null
        //      /// StringUtils.split("", *)           = []
        //      /// StringUtils.split("abc def", null) = ["abc", "def"]
        //      /// StringUtils.split("abc def", " ")  = ["abc", "def"]
        //      /// StringUtils.split("abc  def", " ") = ["abc", "def"]
        //      /// StringUtils.split("ab:cd:ef", ":") = ["ab", "cd", "ef"]
        //      /// </pre>
        //      /// </summary>
        //      /// <param name="str">  the String to parse, may be null </param>
        //      /// <param name="separatorChars">  the characters used as the delimiters,
        //      ///  {@code null} splits on whitespace </param>
        //      /// <returns> an array of parsed Strings, {@code null} if null String input </returns>
        //      //JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
        //      //ORIGINAL LINE: public static String[] split(final String str, final String separatorChars)
        //      public static string[] split(string str, string separatorChars)
        //      {
        //          return splitWorker(str, separatorChars, -1, false);
        //      }

        //      /// <summary>
        //      /// <para>Splits the provided text into an array with a maximum length,
        //      /// separators specified.</para>
        //      /// 
        //      /// <para>The separator is not included in the returned String array.
        //      /// Adjacent separators are treated as one separator.</para>
        //      /// 
        //      /// <para>A {@code null} input String returns {@code null}.
        //      /// A {@code null} separatorChars splits on whitespace.</para>
        //      /// 
        //      /// <para>If more than {@code max} delimited substrings are found, the last
        //      /// returned string includes all characters after the first {@code max - 1}
        //      /// returned strings (including separator characters).</para>
        //      /// 
        //      /// <pre>
        //      /// StringUtils.split(null, *, *)            = null
        //      /// StringUtils.split("", *, *)              = []
        //      /// StringUtils.split("ab cd ef", null, 0)   = ["ab", "cd", "ef"]
        //      /// StringUtils.split("ab   cd ef", null, 0) = ["ab", "cd", "ef"]
        //      /// StringUtils.split("ab:cd:ef", ":", 0)    = ["ab", "cd", "ef"]
        //      /// StringUtils.split("ab:cd:ef", ":", 2)    = ["ab", "cd:ef"]
        //      /// </pre>
        //      /// </summary>
        //      /// <param name="str">  the String to parse, may be null </param>
        //      /// <param name="separatorChars">  the characters used as the delimiters,
        //      ///  {@code null} splits on whitespace </param>
        //      /// <param name="max">  the maximum number of elements to include in the
        //      ///  array. A zero or negative value implies no limit </param>
        //      /// <returns> an array of parsed Strings, {@code null} if null String input </returns>
        //      //JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
        //      //ORIGINAL LINE: public static String[] split(final String str, final String separatorChars, final int max)
        //      public static string[] split(string str, string separatorChars, int max)
        //      {
        //          return splitWorker(str, separatorChars, max, false);
        //      }

        //      /// <summary>
        //      /// <para>Splits the provided text into an array, separator string specified.</para>
        //      /// 
        //      /// <para>The separator(s) will not be included in the returned String array.
        //      /// Adjacent separators are treated as one separator.</para>
        //      /// 
        //      /// <para>A {@code null} input String returns {@code null}.
        //      /// A {@code null} separator splits on whitespace.</para>
        //      /// 
        //      /// <pre>
        //      /// StringUtils.splitByWholeSeparator(null, *)               = null
        //      /// StringUtils.splitByWholeSeparator("", *)                 = []
        //      /// StringUtils.splitByWholeSeparator("ab de fg", null)      = ["ab", "de", "fg"]
        //      /// StringUtils.splitByWholeSeparator("ab   de fg", null)    = ["ab", "de", "fg"]
        //      /// StringUtils.splitByWholeSeparator("ab:cd:ef", ":")       = ["ab", "cd", "ef"]
        //      /// StringUtils.splitByWholeSeparator("ab-!-cd-!-ef", "-!-") = ["ab", "cd", "ef"]
        //      /// </pre>
        //      /// </summary>
        //      /// <param name="str">  the String to parse, may be null </param>
        //      /// <param name="separator">  String containing the String to be used as a delimiter,
        //      ///  {@code null} splits on whitespace </param>
        //      /// <returns> an array of parsed Strings, {@code null} if null String was input </returns>
        //      //JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
        //      //ORIGINAL LINE: public static String[] splitByWholeSeparator(final String str, final String separator)
        //      public static string[] splitByWholeSeparator(string str, string separator)
        //      {
        //          return splitByWholeSeparatorWorker(str, separator, -1, false);
        //      }

        //      /// <summary>
        //      /// <para>Splits the provided text into an array, separator string specified.
        //      /// Returns a maximum of {@code max} substrings.</para>
        //      /// 
        //      /// <para>The separator(s) will not be included in the returned String array.
        //      /// Adjacent separators are treated as one separator.</para>
        //      /// 
        //      /// <para>A {@code null} input String returns {@code null}.
        //      /// A {@code null} separator splits on whitespace.</para>
        //      /// 
        //      /// <pre>
        //      /// StringUtils.splitByWholeSeparator(null, *, *)               = null
        //      /// StringUtils.splitByWholeSeparator("", *, *)                 = []
        //      /// StringUtils.splitByWholeSeparator("ab de fg", null, 0)      = ["ab", "de", "fg"]
        //      /// StringUtils.splitByWholeSeparator("ab   de fg", null, 0)    = ["ab", "de", "fg"]
        //      /// StringUtils.splitByWholeSeparator("ab:cd:ef", ":", 2)       = ["ab", "cd:ef"]
        //      /// StringUtils.splitByWholeSeparator("ab-!-cd-!-ef", "-!-", 5) = ["ab", "cd", "ef"]
        //      /// StringUtils.splitByWholeSeparator("ab-!-cd-!-ef", "-!-", 2) = ["ab", "cd-!-ef"]
        //      /// </pre>
        //      /// </summary>
        //      /// <param name="str">  the String to parse, may be null </param>
        //      /// <param name="separator">  String containing the String to be used as a delimiter,
        //      ///  {@code null} splits on whitespace </param>
        //      /// <param name="max">  the maximum number of elements to include in the returned
        //      ///  array. A zero or negative value implies no limit. </param>
        //      /// <returns> an array of parsed Strings, {@code null} if null String was input </returns>
        //      //JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
        //      //ORIGINAL LINE: public static String[] splitByWholeSeparator(final String str, final String separator, final int max)
        //      public static string[] splitByWholeSeparator(string str, string separator, int max)
        //      {
        //          return splitByWholeSeparatorWorker(str, separator, max, false);
        //      }

        //      /// <summary>
        //      /// <para>Splits the provided text into an array, separator string specified. </para>
        //      /// 
        //      /// <para>The separator is not included in the returned String array.
        //      /// Adjacent separators are treated as separators for empty tokens.
        //      /// For more control over the split use the StrTokenizer class.</para>
        //      /// 
        //      /// <para>A {@code null} input String returns {@code null}.
        //      /// A {@code null} separator splits on whitespace.</para>
        //      /// 
        //      /// <pre>
        //      /// StringUtils.splitByWholeSeparatorPreserveAllTokens(null, *)               = null
        //      /// StringUtils.splitByWholeSeparatorPreserveAllTokens("", *)                 = []
        //      /// StringUtils.splitByWholeSeparatorPreserveAllTokens("ab de fg", null)      = ["ab", "de", "fg"]
        //      /// StringUtils.splitByWholeSeparatorPreserveAllTokens("ab   de fg", null)    = ["ab", "", "", "de", "fg"]
        //      /// StringUtils.splitByWholeSeparatorPreserveAllTokens("ab:cd:ef", ":")       = ["ab", "cd", "ef"]
        //      /// StringUtils.splitByWholeSeparatorPreserveAllTokens("ab-!-cd-!-ef", "-!-") = ["ab", "cd", "ef"]
        //      /// </pre>
        //      /// </summary>
        //      /// <param name="str">  the String to parse, may be null </param>
        //      /// <param name="separator">  String containing the String to be used as a delimiter,
        //      ///  {@code null} splits on whitespace </param>
        //      /// <returns> an array of parsed Strings, {@code null} if null String was input
        //      /// @since 2.4 </returns>
        //      //JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
        //      //ORIGINAL LINE: public static String[] splitByWholeSeparatorPreserveAllTokens(final String str, final String separator)
        //      public static string[] splitByWholeSeparatorPreserveAllTokens(string str, string separator)
        //      {
        //          return splitByWholeSeparatorWorker(str, separator, -1, true);
        //      }

        //      /// <summary>
        //      /// <para>Splits the provided text into an array, separator string specified.
        //      /// Returns a maximum of {@code max} substrings.</para>
        //      /// 
        //      /// <para>The separator is not included in the returned String array.
        //      /// Adjacent separators are treated as separators for empty tokens.
        //      /// For more control over the split use the StrTokenizer class.</para>
        //      /// 
        //      /// <para>A {@code null} input String returns {@code null}.
        //      /// A {@code null} separator splits on whitespace.</para>
        //      /// 
        //      /// <pre>
        //      /// StringUtils.splitByWholeSeparatorPreserveAllTokens(null, *, *)               = null
        //      /// StringUtils.splitByWholeSeparatorPreserveAllTokens("", *, *)                 = []
        //      /// StringUtils.splitByWholeSeparatorPreserveAllTokens("ab de fg", null, 0)      = ["ab", "de", "fg"]
        //      /// StringUtils.splitByWholeSeparatorPreserveAllTokens("ab   de fg", null, 0)    = ["ab", "", "", "de", "fg"]
        //      /// StringUtils.splitByWholeSeparatorPreserveAllTokens("ab:cd:ef", ":", 2)       = ["ab", "cd:ef"]
        //      /// StringUtils.splitByWholeSeparatorPreserveAllTokens("ab-!-cd-!-ef", "-!-", 5) = ["ab", "cd", "ef"]
        //      /// StringUtils.splitByWholeSeparatorPreserveAllTokens("ab-!-cd-!-ef", "-!-", 2) = ["ab", "cd-!-ef"]
        //      /// </pre>
        //      /// </summary>
        //      /// <param name="str">  the String to parse, may be null </param>
        //      /// <param name="separator">  String containing the String to be used as a delimiter,
        //      ///  {@code null} splits on whitespace </param>
        //      /// <param name="max">  the maximum number of elements to include in the returned
        //      ///  array. A zero or negative value implies no limit. </param>
        //      /// <returns> an array of parsed Strings, {@code null} if null String was input
        //      /// @since 2.4 </returns>
        //      //JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
        //      //ORIGINAL LINE: public static String[] splitByWholeSeparatorPreserveAllTokens(final String str, final String separator, final int max)
        //      public static string[] splitByWholeSeparatorPreserveAllTokens(string str, string separator, int max)
        //      {
        //          return splitByWholeSeparatorWorker(str, separator, max, true);
        //      }

        //      /// <summary>
        //      /// Performs the logic for the {@code splitByWholeSeparatorPreserveAllTokens} methods.
        //      /// </summary>
        //      /// <param name="str">  the String to parse, may be {@code null} </param>
        //      /// <param name="separator">  String containing the String to be used as a delimiter,
        //      ///  {@code null} splits on whitespace </param>
        //      /// <param name="max">  the maximum number of elements to include in the returned
        //      ///  array. A zero or negative value implies no limit. </param>
        //      /// <param name="preserveAllTokens"> if {@code true}, adjacent separators are
        //      /// treated as empty token separators; if {@code false}, adjacent
        //      /// separators are treated as one separator. </param>
        //      /// <returns> an array of parsed Strings, {@code null} if null String input
        //      /// @since 2.4 </returns>
        //      //JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
        //      //ORIGINAL LINE: private static String[] splitByWholeSeparatorWorker(final String str, final String separator, final int max, final boolean preserveAllTokens)
        //      private static string[] splitByWholeSeparatorWorker(string str, string separator, int max, bool preserveAllTokens)
        //      {
        //          if (string.ReferenceEquals(str, null))
        //          {
        //              return null;
        //          }

        //          //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
        //          //ORIGINAL LINE: final int len = str.length();
        //          int len = str.Length;

        //          if (len == 0)
        //          {
        //              return ArrayUtils.EMPTY_STRING_ARRAY;
        //          }

        //          if (string.ReferenceEquals(separator, null) || EMPTY.Equals(separator))
        //          {
        //              // Split on whitespace.
        //              return splitWorker(str, null, max, preserveAllTokens);
        //          }

        //          //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
        //          //ORIGINAL LINE: final int separatorLength = separator.length();
        //          int separatorLength = separator.Length;

        //          //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
        //          //ORIGINAL LINE: final java.util.ArrayList<String> substrings = new java.util.ArrayList<>();
        //          List<string> substrings = new List<string>();
        //          int numberOfSubstrings = 0;
        //          int beg = 0;
        //          int end = 0;
        //          while (end < len)
        //          {
        //              end = str.IndexOf(separator, beg, StringComparison.Ordinal);

        //              if (end > -1)
        //              {
        //                  if (end > beg)
        //                  {
        //                      numberOfSubstrings += 1;

        //                      if (numberOfSubstrings == max)
        //                      {
        //                          end = len;
        //                          substrings.Add(str.Substring(beg));
        //                      }
        //                      else
        //                      {
        //                          // The following is OK, because String.substring( beg, end ) excludes
        //                          // the character at the position 'end'.
        //                          substrings.Add(str.Substring(beg, end - beg));

        //                          // Set the starting point for the next search.
        //                          // The following is equivalent to beg = end + (separatorLength - 1) + 1,
        //                          // which is the right calculation:
        //                          beg = end + separatorLength;
        //                      }
        //                  }
        //                  else
        //                  {
        //                      // We found a consecutive occurrence of the separator, so skip it.
        //                      if (preserveAllTokens)
        //                      {
        //                          numberOfSubstrings += 1;
        //                          if (numberOfSubstrings == max)
        //                          {
        //                              end = len;
        //                              substrings.Add(str.Substring(beg));
        //                          }
        //                          else
        //                          {
        //                              substrings.Add(EMPTY);
        //                          }
        //                      }
        //                      beg = end + separatorLength;
        //                  }
        //              }
        //              else
        //              {
        //                  // String.substring( beg ) goes from 'beg' to the end of the String.
        //                  substrings.Add(str.Substring(beg));
        //                  end = len;
        //              }
        //          }

        //          return substrings.ToArray();
        //      }

        //      // -----------------------------------------------------------------------
        //      /// <summary>
        //      /// <para>Splits the provided text into an array, using whitespace as the
        //      /// separator, preserving all tokens, including empty tokens created by
        //      /// adjacent separators. This is an alternative to using StringTokenizer.
        //      /// Whitespace is defined by <seealso cref="Character#isWhitespace(char)"/>.</para>
        //      /// 
        //      /// <para>The separator is not included in the returned String array.
        //      /// Adjacent separators are treated as separators for empty tokens.
        //      /// For more control over the split use the StrTokenizer class.</para>
        //      /// 
        //      /// <para>A {@code null} input String returns {@code null}.</para>
        //      /// 
        //      /// <pre>
        //      /// StringUtils.splitPreserveAllTokens(null)       = null
        //      /// StringUtils.splitPreserveAllTokens("")         = []
        //      /// StringUtils.splitPreserveAllTokens("abc def")  = ["abc", "def"]
        //      /// StringUtils.splitPreserveAllTokens("abc  def") = ["abc", "", "def"]
        //      /// StringUtils.splitPreserveAllTokens(" abc ")    = ["", "abc", ""]
        //      /// </pre>
        //      /// </summary>
        //      /// <param name="str">  the String to parse, may be {@code null} </param>
        //      /// <returns> an array of parsed Strings, {@code null} if null String input
        //      /// @since 2.1 </returns>
        //      //JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
        //      //ORIGINAL LINE: public static String[] splitPreserveAllTokens(final String str)
        //      public static string[] splitPreserveAllTokens(string str)
        //      {
        //          return splitWorker(str, null, -1, true);
        //      }

        //      /// <summary>
        //      /// <para>Splits the provided text into an array, separator specified,
        //      /// preserving all tokens, including empty tokens created by adjacent
        //      /// separators. This is an alternative to using StringTokenizer.</para>
        //      /// 
        //      /// <para>The separator is not included in the returned String array.
        //      /// Adjacent separators are treated as separators for empty tokens.
        //      /// For more control over the split use the StrTokenizer class.</para>
        //      /// 
        //      /// <para>A {@code null} input String returns {@code null}.</para>
        //      /// 
        //      /// <pre>
        //      /// StringUtils.splitPreserveAllTokens(null, *)         = null
        //      /// StringUtils.splitPreserveAllTokens("", *)           = []
        //      /// StringUtils.splitPreserveAllTokens("a.b.c", '.')    = ["a", "b", "c"]
        //      /// StringUtils.splitPreserveAllTokens("a..b.c", '.')   = ["a", "", "b", "c"]
        //      /// StringUtils.splitPreserveAllTokens("a:b:c", '.')    = ["a:b:c"]
        //      /// StringUtils.splitPreserveAllTokens("a\tb\nc", null) = ["a", "b", "c"]
        //      /// StringUtils.splitPreserveAllTokens("a b c", ' ')    = ["a", "b", "c"]
        //      /// StringUtils.splitPreserveAllTokens("a b c ", ' ')   = ["a", "b", "c", ""]
        //      /// StringUtils.splitPreserveAllTokens("a b c  ", ' ')   = ["a", "b", "c", "", ""]
        //      /// StringUtils.splitPreserveAllTokens(" a b c", ' ')   = ["", a", "b", "c"]
        //      /// StringUtils.splitPreserveAllTokens("  a b c", ' ')  = ["", "", a", "b", "c"]
        //      /// StringUtils.splitPreserveAllTokens(" a b c ", ' ')  = ["", a", "b", "c", ""]
        //      /// </pre>
        //      /// </summary>
        //      /// <param name="str">  the String to parse, may be {@code null} </param>
        //      /// <param name="separatorChar">  the character used as the delimiter,
        //      ///  {@code null} splits on whitespace </param>
        //      /// <returns> an array of parsed Strings, {@code null} if null String input
        //      /// @since 2.1 </returns>
        //      //JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
        //      //ORIGINAL LINE: public static String[] splitPreserveAllTokens(final String str, final char separatorChar)
        //      public static string[] splitPreserveAllTokens(string str, char separatorChar)
        //      {
        //          return splitWorker(str, separatorChar, true);
        //      }

        //      /// <summary>
        //      /// Performs the logic for the {@code split} and
        //      /// {@code splitPreserveAllTokens} methods that do not return a
        //      /// maximum array length.
        //      /// </summary>
        //      /// <param name="str">  the String to parse, may be {@code null} </param>
        //      /// <param name="separatorChar"> the separate character </param>
        //      /// <param name="preserveAllTokens"> if {@code true}, adjacent separators are
        //      /// treated as empty token separators; if {@code false}, adjacent
        //      /// separators are treated as one separator. </param>
        //      /// <returns> an array of parsed Strings, {@code null} if null String input </returns>
        //      //JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
        //      //ORIGINAL LINE: private static String[] splitWorker(final String str, final char separatorChar, final boolean preserveAllTokens)
        //      private static string[] splitWorker(string str, char separatorChar, bool preserveAllTokens)
        //      {
        //          // Performance tuned for 2.0 (JDK1.4)

        //          if (string.ReferenceEquals(str, null))
        //          {
        //              return null;
        //          }
        //          //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
        //          //ORIGINAL LINE: final int len = str.length();
        //          int len = str.Length;
        //          if (len == 0)
        //          {
        //              return ArrayUtils.EMPTY_STRING_ARRAY;
        //          }
        //          //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
        //          //ORIGINAL LINE: final java.util.List<String> list = new java.util.ArrayList<>();
        //          IList<string> list = new List<string>();
        //          int i = 0, start = 0;
        //          bool match = false;
        //          bool lastMatch = false;
        //          while (i < len)
        //          {
        //              if (str[i] == separatorChar)
        //              {
        //                  if (match || preserveAllTokens)
        //                  {
        //                      list.Add(str.Substring(start, i - start));
        //                      match = false;
        //                      lastMatch = true;
        //                  }
        //                  start = ++i;
        //                  continue;
        //              }
        //              lastMatch = false;
        //              match = true;
        //              i++;
        //          }
        //          if (match || preserveAllTokens && lastMatch)
        //          {
        //              list.Add(str.Substring(start, i - start));
        //          }
        //          return list.ToArray();
        //      }

        //      /// <summary>
        //      /// <para>Splits the provided text into an array, separators specified,
        //      /// preserving all tokens, including empty tokens created by adjacent
        //      /// separators. This is an alternative to using StringTokenizer.</para>
        //      /// 
        //      /// <para>The separator is not included in the returned String array.
        //      /// Adjacent separators are treated as separators for empty tokens.
        //      /// For more control over the split use the StrTokenizer class.</para>
        //      /// 
        //      /// <para>A {@code null} input String returns {@code null}.
        //      /// A {@code null} separatorChars splits on whitespace.</para>
        //      /// 
        //      /// <pre>
        //      /// StringUtils.splitPreserveAllTokens(null, *)           = null
        //      /// StringUtils.splitPreserveAllTokens("", *)             = []
        //      /// StringUtils.splitPreserveAllTokens("abc def", null)   = ["abc", "def"]
        //      /// StringUtils.splitPreserveAllTokens("abc def", " ")    = ["abc", "def"]
        //      /// StringUtils.splitPreserveAllTokens("abc  def", " ")   = ["abc", "", def"]
        //      /// StringUtils.splitPreserveAllTokens("ab:cd:ef", ":")   = ["ab", "cd", "ef"]
        //      /// StringUtils.splitPreserveAllTokens("ab:cd:ef:", ":")  = ["ab", "cd", "ef", ""]
        //      /// StringUtils.splitPreserveAllTokens("ab:cd:ef::", ":") = ["ab", "cd", "ef", "", ""]
        //      /// StringUtils.splitPreserveAllTokens("ab::cd:ef", ":")  = ["ab", "", cd", "ef"]
        //      /// StringUtils.splitPreserveAllTokens(":cd:ef", ":")     = ["", cd", "ef"]
        //      /// StringUtils.splitPreserveAllTokens("::cd:ef", ":")    = ["", "", cd", "ef"]
        //      /// StringUtils.splitPreserveAllTokens(":cd:ef:", ":")    = ["", cd", "ef", ""]
        //      /// </pre>
        //      /// </summary>
        //      /// <param name="str">  the String to parse, may be {@code null} </param>
        //      /// <param name="separatorChars">  the characters used as the delimiters,
        //      ///  {@code null} splits on whitespace </param>
        //      /// <returns> an array of parsed Strings, {@code null} if null String input
        //      /// @since 2.1 </returns>
        //      //JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
        //      //ORIGINAL LINE: public static String[] splitPreserveAllTokens(final String str, final String separatorChars)
        //      public static string[] splitPreserveAllTokens(string str, string separatorChars)
        //      {
        //          return splitWorker(str, separatorChars, -1, true);
        //      }

        //      /// <summary>
        //      /// <para>Splits the provided text into an array with a maximum length,
        //      /// separators specified, preserving all tokens, including empty tokens
        //      /// created by adjacent separators.</para>
        //      /// 
        //      /// <para>The separator is not included in the returned String array.
        //      /// Adjacent separators are treated as separators for empty tokens.
        //      /// Adjacent separators are treated as one separator.</para>
        //      /// 
        //      /// <para>A {@code null} input String returns {@code null}.
        //      /// A {@code null} separatorChars splits on whitespace.</para>
        //      /// 
        //      /// <para>If more than {@code max} delimited substrings are found, the last
        //      /// returned string includes all characters after the first {@code max - 1}
        //      /// returned strings (including separator characters).</para>
        //      /// 
        //      /// <pre>
        //      /// StringUtils.splitPreserveAllTokens(null, *, *)            = null
        //      /// StringUtils.splitPreserveAllTokens("", *, *)              = []
        //      /// StringUtils.splitPreserveAllTokens("ab de fg", null, 0)   = ["ab", "cd", "ef"]
        //      /// StringUtils.splitPreserveAllTokens("ab   de fg", null, 0) = ["ab", "cd", "ef"]
        //      /// StringUtils.splitPreserveAllTokens("ab:cd:ef", ":", 0)    = ["ab", "cd", "ef"]
        //      /// StringUtils.splitPreserveAllTokens("ab:cd:ef", ":", 2)    = ["ab", "cd:ef"]
        //      /// StringUtils.splitPreserveAllTokens("ab   de fg", null, 2) = ["ab", "  de fg"]
        //      /// StringUtils.splitPreserveAllTokens("ab   de fg", null, 3) = ["ab", "", " de fg"]
        //      /// StringUtils.splitPreserveAllTokens("ab   de fg", null, 4) = ["ab", "", "", "de fg"]
        //      /// </pre>
        //      /// </summary>
        //      /// <param name="str">  the String to parse, may be {@code null} </param>
        //      /// <param name="separatorChars">  the characters used as the delimiters,
        //      ///  {@code null} splits on whitespace </param>
        //      /// <param name="max">  the maximum number of elements to include in the
        //      ///  array. A zero or negative value implies no limit </param>
        //      /// <returns> an array of parsed Strings, {@code null} if null String input
        //      /// @since 2.1 </returns>
        //      //JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
        //      //ORIGINAL LINE: public static String[] splitPreserveAllTokens(final String str, final String separatorChars, final int max)
        //      public static string[] splitPreserveAllTokens(string str, string separatorChars, int max)
        //      {
        //          return splitWorker(str, separatorChars, max, true);
        //      }

        //      /// <summary>
        //      /// Performs the logic for the {@code split} and
        //      /// {@code splitPreserveAllTokens} methods that return a maximum array
        //      /// length.
        //      /// </summary>
        //      /// <param name="str">  the String to parse, may be {@code null} </param>
        //      /// <param name="separatorChars"> the separate character </param>
        //      /// <param name="max">  the maximum number of elements to include in the
        //      ///  array. A zero or negative value implies no limit. </param>
        //      /// <param name="preserveAllTokens"> if {@code true}, adjacent separators are
        //      /// treated as empty token separators; if {@code false}, adjacent
        //      /// separators are treated as one separator. </param>
        //      /// <returns> an array of parsed Strings, {@code null} if null String input </returns>
        //      //JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
        //      //ORIGINAL LINE: private static String[] splitWorker(final String str, final String separatorChars, final int max, final boolean preserveAllTokens)
        //      private static string[] splitWorker(string str, string separatorChars, int max, bool preserveAllTokens)
        //      {
        //          // Performance tuned for 2.0 (JDK1.4)
        //          // Direct code is quicker than StringTokenizer.
        //          // Also, StringTokenizer uses isSpace() not isWhitespace()

        //          if (string.ReferenceEquals(str, null))
        //          {
        //              return null;
        //          }
        //          //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
        //          //ORIGINAL LINE: final int len = str.length();
        //          int len = str.Length;
        //          if (len == 0)
        //          {
        //              return ArrayUtils.EMPTY_STRING_ARRAY;
        //          }
        //          //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
        //          //ORIGINAL LINE: final java.util.List<String> list = new java.util.ArrayList<>();
        //          IList<string> list = new List<string>();
        //          int sizePlus1 = 1;
        //          int i = 0, start = 0;
        //          bool match = false;
        //          bool lastMatch = false;
        //          if (string.ReferenceEquals(separatorChars, null))
        //          {
        //              // Null separator means use whitespace
        //              while (i < len)
        //              {
        //                  if (char.IsWhiteSpace(str[i]))
        //                  {
        //                      if (match || preserveAllTokens)
        //                      {
        //                          lastMatch = true;
        //                          if (sizePlus1++ == max)
        //                          {
        //                              i = len;
        //                              lastMatch = false;
        //                          }
        //                          list.Add(str.Substring(start, i - start));
        //                          match = false;
        //                      }
        //                      start = ++i;
        //                      continue;
        //                  }
        //                  lastMatch = false;
        //                  match = true;
        //                  i++;
        //              }
        //          }
        //          else if (separatorChars.Length == 1)
        //          {
        //              // Optimise 1 character case
        //              //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
        //              //ORIGINAL LINE: final char sep = separatorChars.charAt(0);
        //              char sep = separatorChars[0];
        //              while (i < len)
        //              {
        //                  if (str[i] == sep)
        //                  {
        //                      if (match || preserveAllTokens)
        //                      {
        //                          lastMatch = true;
        //                          if (sizePlus1++ == max)
        //                          {
        //                              i = len;
        //                              lastMatch = false;
        //                          }
        //                          list.Add(str.Substring(start, i - start));
        //                          match = false;
        //                      }
        //                      start = ++i;
        //                      continue;
        //                  }
        //                  lastMatch = false;
        //                  match = true;
        //                  i++;
        //              }
        //          }
        //          else
        //          {
        //              // standard case
        //              while (i < len)
        //              {
        //                  if (separatorChars.IndexOf(str[i]) >= 0)
        //                  {
        //                      if (match || preserveAllTokens)
        //                      {
        //                          lastMatch = true;
        //                          if (sizePlus1++ == max)
        //                          {
        //                              i = len;
        //                              lastMatch = false;
        //                          }
        //                          list.Add(str.Substring(start, i - start));
        //                          match = false;
        //                      }
        //                      start = ++i;
        //                      continue;
        //                  }
        //                  lastMatch = false;
        //                  match = true;
        //                  i++;
        //              }
        //          }
        //          if (match || preserveAllTokens && lastMatch)
        //          {
        //              list.Add(str.Substring(start, i - start));
        //          }
        //          return list.ToArray();
        //      }

        //      /// <summary>
        //      /// <para>Splits a String by Character type as returned by
        //      /// {@code java.lang.Character.getType(char)}. Groups of contiguous
        //      /// characters of the same type are returned as complete tokens.
        //      /// <pre>
        //      /// StringUtils.splitByCharacterType(null)         = null
        //      /// StringUtils.splitByCharacterType("")           = []
        //      /// StringUtils.splitByCharacterType("ab de fg")   = ["ab", " ", "de", " ", "fg"]
        //      /// StringUtils.splitByCharacterType("ab   de fg") = ["ab", "   ", "de", " ", "fg"]
        //      /// StringUtils.splitByCharacterType("ab:cd:ef")   = ["ab", ":", "cd", ":", "ef"]
        //      /// StringUtils.splitByCharacterType("number5")    = ["number", "5"]
        //      /// StringUtils.splitByCharacterType("fooBar")     = ["foo", "B", "ar"]
        //      /// StringUtils.splitByCharacterType("foo200Bar")  = ["foo", "200", "B", "ar"]
        //      /// StringUtils.splitByCharacterType("ASFRules")   = ["ASFR", "ules"]
        //      /// </pre>
        //      /// </para>
        //      /// </summary>
        //      /// <param name="str"> the String to split, may be {@code null} </param>
        //      /// <returns> an array of parsed Strings, {@code null} if null String input
        //      /// @since 2.4 </returns>
        //      //JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
        //      //ORIGINAL LINE: public static String[] splitByCharacterType(final String str)
        //      public static string[] splitByCharacterType(string str)
        //      {
        //          return splitByCharacterType(str, false);
        //      }

        //      /// <summary>
        //      /// <para>Splits a String by Character type as returned by
        //      /// {@code java.lang.Character.getType(char)}. Groups of contiguous
        //      /// characters of the same type are returned as complete tokens, with the
        //      /// following exception: the character of type
        //      /// {@code Character.UPPERCASE_LETTER}, if any, immediately
        //      /// preceding a token of type {@code Character.LOWERCASE_LETTER}
        //      /// will belong to the following token rather than to the preceding, if any,
        //      /// {@code Character.UPPERCASE_LETTER} token.
        //      /// <pre>
        //      /// StringUtils.splitByCharacterTypeCamelCase(null)         = null
        //      /// StringUtils.splitByCharacterTypeCamelCase("")           = []
        //      /// StringUtils.splitByCharacterTypeCamelCase("ab de fg")   = ["ab", " ", "de", " ", "fg"]
        //      /// StringUtils.splitByCharacterTypeCamelCase("ab   de fg") = ["ab", "   ", "de", " ", "fg"]
        //      /// StringUtils.splitByCharacterTypeCamelCase("ab:cd:ef")   = ["ab", ":", "cd", ":", "ef"]
        //      /// StringUtils.splitByCharacterTypeCamelCase("number5")    = ["number", "5"]
        //      /// StringUtils.splitByCharacterTypeCamelCase("fooBar")     = ["foo", "Bar"]
        //      /// StringUtils.splitByCharacterTypeCamelCase("foo200Bar")  = ["foo", "200", "Bar"]
        //      /// StringUtils.splitByCharacterTypeCamelCase("ASFRules")   = ["ASF", "Rules"]
        //      /// </pre>
        //      /// </para>
        //      /// </summary>
        //      /// <param name="str"> the String to split, may be {@code null} </param>
        //      /// <returns> an array of parsed Strings, {@code null} if null String input
        //      /// @since 2.4 </returns>
        //      //JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
        //      //ORIGINAL LINE: public static String[] splitByCharacterTypeCamelCase(final String str)
        //      public static string[] splitByCharacterTypeCamelCase(string str)
        //      {
        //          return splitByCharacterType(str, true);
        //      }

        //      /// <summary>
        //      /// <para>Splits a String by Character type as returned by
        //      /// {@code java.lang.Character.getType(char)}. Groups of contiguous
        //      /// characters of the same type are returned as complete tokens, with the
        //      /// following exception: if {@code camelCase} is {@code true},
        //      /// the character of type {@code Character.UPPERCASE_LETTER}, if any,
        //      /// immediately preceding a token of type {@code Character.LOWERCASE_LETTER}
        //      /// will belong to the following token rather than to the preceding, if any,
        //      /// {@code Character.UPPERCASE_LETTER} token.
        //      /// </para>
        //      /// </summary>
        //      /// <param name="str"> the String to split, may be {@code null} </param>
        //      /// <param name="camelCase"> whether to use so-called "camel-case" for letter types </param>
        //      /// <returns> an array of parsed Strings, {@code null} if null String input
        //      /// @since 2.4 </returns>
        //      //JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
        //      //ORIGINAL LINE: private static String[] splitByCharacterType(final String str, final boolean camelCase)
        //      private static string[] splitByCharacterType(string str, bool camelCase)
        //      {
        //          if (string.ReferenceEquals(str, null))
        //          {
        //              return null;
        //          }
        //          if (str.Length == 0)
        //          {
        //              return ArrayUtils.EMPTY_STRING_ARRAY;
        //          }
        //          //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
        //          //ORIGINAL LINE: final char[] c = str.toCharArray();
        //          char[] c = str.ToCharArray();
        //          //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
        //          //ORIGINAL LINE: final java.util.List<String> list = new java.util.ArrayList<>();
        //          IList<string> list = new List<string>();
        //          int tokenStart = 0;
        //          int currentType = char.GetUnicodeCategory(c[tokenStart]);
        //          for (int pos = tokenStart + 1; pos < c.Length; pos++)
        //          {
        //              //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
        //              //ORIGINAL LINE: final int type = Character.getType(c[pos]);
        //              int type = char.GetUnicodeCategory(c[pos]);
        //              if (type == currentType)
        //              {
        //                  continue;
        //              }
        //              if (camelCase && type == UnicodeCategory.LowercaseLetter && currentType == UnicodeCategory.UppercaseLetter)
        //              {
        //                  //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
        //                  //ORIGINAL LINE: final int newTokenStart = pos - 1;
        //                  int newTokenStart = pos - 1;
        //                  if (newTokenStart != tokenStart)
        //                  {
        //                      list.Add(new string(c, tokenStart, newTokenStart - tokenStart));
        //                      tokenStart = newTokenStart;
        //                  }
        //              }
        //              else
        //              {
        //                  list.Add(new string(c, tokenStart, pos - tokenStart));
        //                  tokenStart = pos;
        //              }
        //              currentType = type;
        //          }
        //          list.Add(new string(c, tokenStart, c.Length - tokenStart));
        //          return list.ToArray();
        //      }

        //      // Joining
        //      //-----------------------------------------------------------------------
        //      /// <summary>
        //      /// <para>Joins the elements of the provided array into a single String
        //      /// containing the provided list of elements.</para>
        //      /// 
        //      /// <para>No separator is added to the joined String.
        //      /// Null objects or empty strings within the array are represented by
        //      /// empty strings.</para>
        //      /// 
        //      /// <pre>
        //      /// StringUtils.join(null)            = null
        //      /// StringUtils.join([])              = ""
        //      /// StringUtils.join([null])          = ""
        //      /// StringUtils.join(["a", "b", "c"]) = "abc"
        //      /// StringUtils.join([null, "", "a"]) = "a"
        //      /// </pre>
        //      /// </summary>
        //      /// @param <T> the specific type of values to join together </param>
        //      /// <param name="elements">  the values to join together, may be null </param>
        //      /// <returns> the joined String, {@code null} if null array input
        //      /// @since 2.0
        //      /// @since 3.0 Changed signature to use varargs </returns>
        //      //JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
        //      //ORIGINAL LINE: @SafeVarargs public static <T> String join(final T... elements)
        //      //JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
        //      public static string join<T>(params T[] elements)
        //      {
        //          return join(elements, null);
        //      }

        //      /// <summary>
        //      /// <para>Joins the elements of the provided array into a single String
        //      /// containing the provided list of elements.</para>
        //      /// 
        //      /// <para>No delimiter is added before or after the list.
        //      /// Null objects or empty strings within the array are represented by
        //      /// empty strings.</para>
        //      /// 
        //      /// <pre>
        //      /// StringUtils.join(null, *)               = null
        //      /// StringUtils.join([], *)                 = ""
        //      /// StringUtils.join([null], *)             = ""
        //      /// StringUtils.join(["a", "b", "c"], ';')  = "a;b;c"
        //      /// StringUtils.join(["a", "b", "c"], null) = "abc"
        //      /// StringUtils.join([null, "", "a"], ';')  = ";;a"
        //      /// </pre>
        //      /// </summary>
        //      /// <param name="array">  the array of values to join together, may be null </param>
        //      /// <param name="separator">  the separator character to use </param>
        //      /// <returns> the joined String, {@code null} if null array input
        //      /// @since 2.0 </returns>
        //      //JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
        //      //ORIGINAL LINE: public static String join(final Object[] array, final char separator)
        //      public static string join(object[] array, char separator)
        //      {
        //          if (array == null)
        //          {
        //              return null;
        //          }
        //          return join(array, separator, 0, array.Length);
        //      }

        //      /// <summary>
        //      /// <para>
        //      /// Joins the elements of the provided array into a single String containing the provided list of elements.
        //      /// </para>
        //      /// 
        //      /// <para>
        //      /// No delimiter is added before or after the list. Null objects or empty strings within the array are represented
        //      /// by empty strings.
        //      /// </para>
        //      /// 
        //      /// <pre>
        //      /// StringUtils.join(null, *)               = null
        //      /// StringUtils.join([], *)                 = ""
        //      /// StringUtils.join([null], *)             = ""
        //      /// StringUtils.join([1, 2, 3], ';')  = "1;2;3"
        //      /// StringUtils.join([1, 2, 3], null) = "123"
        //      /// </pre>
        //      /// </summary>
        //      /// <param name="array">
        //      ///            the array of values to join together, may be null </param>
        //      /// <param name="separator">
        //      ///            the separator character to use </param>
        //      /// <returns> the joined String, {@code null} if null array input
        //      /// @since 3.2 </returns>
        //      //JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
        //      //ORIGINAL LINE: public static String join(final long[] array, final char separator)
        //      public static string join(long[] array, char separator)
        //      {
        //          if (array == null)
        //          {
        //              return null;
        //          }
        //          return join(array, separator, 0, array.Length);
        //      }

        //      /// <summary>
        //      /// <para>
        //      /// Joins the elements of the provided array into a single String containing the provided list of elements.
        //      /// </para>
        //      /// 
        //      /// <para>
        //      /// No delimiter is added before or after the list. Null objects or empty strings within the array are represented
        //      /// by empty strings.
        //      /// </para>
        //      /// 
        //      /// <pre>
        //      /// StringUtils.join(null, *)               = null
        //      /// StringUtils.join([], *)                 = ""
        //      /// StringUtils.join([null], *)             = ""
        //      /// StringUtils.join([1, 2, 3], ';')  = "1;2;3"
        //      /// StringUtils.join([1, 2, 3], null) = "123"
        //      /// </pre>
        //      /// </summary>
        //      /// <param name="array">
        //      ///            the array of values to join together, may be null </param>
        //      /// <param name="separator">
        //      ///            the separator character to use </param>
        //      /// <returns> the joined String, {@code null} if null array input
        //      /// @since 3.2 </returns>
        //      //JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
        //      //ORIGINAL LINE: public static String join(final int[] array, final char separator)
        //      public static string join(int[] array, char separator)
        //      {
        //          if (array == null)
        //          {
        //              return null;
        //          }
        //          return join(array, separator, 0, array.Length);
        //      }

        //      /// <summary>
        //      /// <para>
        //      /// Joins the elements of the provided array into a single String containing the provided list of elements.
        //      /// </para>
        //      /// 
        //      /// <para>
        //      /// No delimiter is added before or after the list. Null objects or empty strings within the array are represented
        //      /// by empty strings.
        //      /// </para>
        //      /// 
        //      /// <pre>
        //      /// StringUtils.join(null, *)               = null
        //      /// StringUtils.join([], *)                 = ""
        //      /// StringUtils.join([null], *)             = ""
        //      /// StringUtils.join([1, 2, 3], ';')  = "1;2;3"
        //      /// StringUtils.join([1, 2, 3], null) = "123"
        //      /// </pre>
        //      /// </summary>
        //      /// <param name="array">
        //      ///            the array of values to join together, may be null </param>
        //      /// <param name="separator">
        //      ///            the separator character to use </param>
        //      /// <returns> the joined String, {@code null} if null array input
        //      /// @since 3.2 </returns>
        //      //JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
        //      //ORIGINAL LINE: public static String join(final short[] array, final char separator)
        //      public static string join(short[] array, char separator)
        //      {
        //          if (array == null)
        //          {
        //              return null;
        //          }
        //          return join(array, separator, 0, array.Length);
        //      }

        //      /// <summary>
        //      /// <para>
        //      /// Joins the elements of the provided array into a single String containing the provided list of elements.
        //      /// </para>
        //      /// 
        //      /// <para>
        //      /// No delimiter is added before or after the list. Null objects or empty strings within the array are represented
        //      /// by empty strings.
        //      /// </para>
        //      /// 
        //      /// <pre>
        //      /// StringUtils.join(null, *)               = null
        //      /// StringUtils.join([], *)                 = ""
        //      /// StringUtils.join([null], *)             = ""
        //      /// StringUtils.join([1, 2, 3], ';')  = "1;2;3"
        //      /// StringUtils.join([1, 2, 3], null) = "123"
        //      /// </pre>
        //      /// </summary>
        //      /// <param name="array">
        //      ///            the array of values to join together, may be null </param>
        //      /// <param name="separator">
        //      ///            the separator character to use </param>
        //      /// <returns> the joined String, {@code null} if null array input
        //      /// @since 3.2 </returns>
        //      //JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
        //      //ORIGINAL LINE: public static String join(final byte[] array, final char separator)
        //      public static string join(sbyte[] array, char separator)
        //      {
        //          if (array == null)
        //          {
        //              return null;
        //          }
        //          return join(array, separator, 0, array.Length);
        //      }

        //      /// <summary>
        //      /// <para>
        //      /// Joins the elements of the provided array into a single String containing the provided list of elements.
        //      /// </para>
        //      /// 
        //      /// <para>
        //      /// No delimiter is added before or after the list. Null objects or empty strings within the array are represented
        //      /// by empty strings.
        //      /// </para>
        //      /// 
        //      /// <pre>
        //      /// StringUtils.join(null, *)               = null
        //      /// StringUtils.join([], *)                 = ""
        //      /// StringUtils.join([null], *)             = ""
        //      /// StringUtils.join([1, 2, 3], ';')  = "1;2;3"
        //      /// StringUtils.join([1, 2, 3], null) = "123"
        //      /// </pre>
        //      /// </summary>
        //      /// <param name="array">
        //      ///            the array of values to join together, may be null </param>
        //      /// <param name="separator">
        //      ///            the separator character to use </param>
        //      /// <returns> the joined String, {@code null} if null array input
        //      /// @since 3.2 </returns>
        //      //JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
        //      //ORIGINAL LINE: public static String join(final char[] array, final char separator)
        //      public static string join(char[] array, char separator)
        //      {
        //          if (array == null)
        //          {
        //              return null;
        //          }
        //          return join(array, separator, 0, array.Length);
        //      }

        //      /// <summary>
        //      /// <para>
        //      /// Joins the elements of the provided array into a single String containing the provided list of elements.
        //      /// </para>
        //      /// 
        //      /// <para>
        //      /// No delimiter is added before or after the list. Null objects or empty strings within the array are represented
        //      /// by empty strings.
        //      /// </para>
        //      /// 
        //      /// <pre>
        //      /// StringUtils.join(null, *)               = null
        //      /// StringUtils.join([], *)                 = ""
        //      /// StringUtils.join([null], *)             = ""
        //      /// StringUtils.join([1, 2, 3], ';')  = "1;2;3"
        //      /// StringUtils.join([1, 2, 3], null) = "123"
        //      /// </pre>
        //      /// </summary>
        //      /// <param name="array">
        //      ///            the array of values to join together, may be null </param>
        //      /// <param name="separator">
        //      ///            the separator character to use </param>
        //      /// <returns> the joined String, {@code null} if null array input
        //      /// @since 3.2 </returns>
        //      //JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
        //      //ORIGINAL LINE: public static String join(final float[] array, final char separator)
        //      public static string join(float[] array, char separator)
        //      {
        //          if (array == null)
        //          {
        //              return null;
        //          }
        //          return join(array, separator, 0, array.Length);
        //      }

        //      /// <summary>
        //      /// <para>
        //      /// Joins the elements of the provided array into a single String containing the provided list of elements.
        //      /// </para>
        //      /// 
        //      /// <para>
        //      /// No delimiter is added before or after the list. Null objects or empty strings within the array are represented
        //      /// by empty strings.
        //      /// </para>
        //      /// 
        //      /// <pre>
        //      /// StringUtils.join(null, *)               = null
        //      /// StringUtils.join([], *)                 = ""
        //      /// StringUtils.join([null], *)             = ""
        //      /// StringUtils.join([1, 2, 3], ';')  = "1;2;3"
        //      /// StringUtils.join([1, 2, 3], null) = "123"
        //      /// </pre>
        //      /// </summary>
        //      /// <param name="array">
        //      ///            the array of values to join together, may be null </param>
        //      /// <param name="separator">
        //      ///            the separator character to use </param>
        //      /// <returns> the joined String, {@code null} if null array input
        //      /// @since 3.2 </returns>
        //      //JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
        //      //ORIGINAL LINE: public static String join(final double[] array, final char separator)
        //      public static string join(double[] array, char separator)
        //      {
        //          if (array == null)
        //          {
        //              return null;
        //          }
        //          return join(array, separator, 0, array.Length);
        //      }


        //      /// <summary>
        //      /// <para>Joins the elements of the provided array into a single String
        //      /// containing the provided list of elements.</para>
        //      /// 
        //      /// <para>No delimiter is added before or after the list.
        //      /// Null objects or empty strings within the array are represented by
        //      /// empty strings.</para>
        //      /// 
        //      /// <pre>
        //      /// StringUtils.join(null, *)               = null
        //      /// StringUtils.join([], *)                 = ""
        //      /// StringUtils.join([null], *)             = ""
        //      /// StringUtils.join(["a", "b", "c"], ';')  = "a;b;c"
        //      /// StringUtils.join(["a", "b", "c"], null) = "abc"
        //      /// StringUtils.join([null, "", "a"], ';')  = ";;a"
        //      /// </pre>
        //      /// </summary>
        //      /// <param name="array">  the array of values to join together, may be null </param>
        //      /// <param name="separator">  the separator character to use </param>
        //      /// <param name="startIndex"> the first index to start joining from.  It is
        //      /// an error to pass in an end index past the end of the array </param>
        //      /// <param name="endIndex"> the index to stop joining from (exclusive). It is
        //      /// an error to pass in an end index past the end of the array </param>
        //      /// <returns> the joined String, {@code null} if null array input
        //      /// @since 2.0 </returns>
        //      //JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
        //      //ORIGINAL LINE: public static String join(final Object[] array, final char separator, final int startIndex, final int endIndex)
        //      public static string join(object[] array, char separator, int startIndex, int endIndex)
        //      {
        //          if (array == null)
        //          {
        //              return null;
        //          }
        //          //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
        //          //ORIGINAL LINE: final int noOfItems = endIndex - startIndex;
        //          int noOfItems = endIndex - startIndex;
        //          if (noOfItems <= 0)
        //          {
        //              return EMPTY;
        //          }
        //          //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
        //          //ORIGINAL LINE: final StringBuilder buf = new StringBuilder(noOfItems * 16);
        //          StringBuilder buf = new StringBuilder(noOfItems * 16);
        //          for (int i = startIndex; i < endIndex; i++)
        //          {
        //              if (i > startIndex)
        //              {
        //                  buf.Append(separator);
        //              }
        //              if (array[i] != null)
        //              {
        //                  buf.Append(array[i]);
        //              }
        //          }
        //          return buf.ToString();
        //      }

        //      /// <summary>
        //      /// <para>
        //      /// Joins the elements of the provided array into a single String containing the provided list of elements.
        //      /// </para>
        //      /// 
        //      /// <para>
        //      /// No delimiter is added before or after the list. Null objects or empty strings within the array are represented
        //      /// by empty strings.
        //      /// </para>
        //      /// 
        //      /// <pre>
        //      /// StringUtils.join(null, *)               = null
        //      /// StringUtils.join([], *)                 = ""
        //      /// StringUtils.join([null], *)             = ""
        //      /// StringUtils.join([1, 2, 3], ';')  = "1;2;3"
        //      /// StringUtils.join([1, 2, 3], null) = "123"
        //      /// </pre>
        //      /// </summary>
        //      /// <param name="array">
        //      ///            the array of values to join together, may be null </param>
        //      /// <param name="separator">
        //      ///            the separator character to use </param>
        //      /// <param name="startIndex">
        //      ///            the first index to start joining from. It is an error to pass in an end index past the end of the
        //      ///            array </param>
        //      /// <param name="endIndex">
        //      ///            the index to stop joining from (exclusive). It is an error to pass in an end index past the end of
        //      ///            the array </param>
        //      /// <returns> the joined String, {@code null} if null array input
        //      /// @since 3.2 </returns>
        //      //JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
        //      //ORIGINAL LINE: public static String join(final long[] array, final char separator, final int startIndex, final int endIndex)
        //      public static string join(long[] array, char separator, int startIndex, int endIndex)
        //      {
        //          if (array == null)
        //          {
        //              return null;
        //          }
        //          //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
        //          //ORIGINAL LINE: final int noOfItems = endIndex - startIndex;
        //          int noOfItems = endIndex - startIndex;
        //          if (noOfItems <= 0)
        //          {
        //              return EMPTY;
        //          }
        //          //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
        //          //ORIGINAL LINE: final StringBuilder buf = new StringBuilder(noOfItems * 16);
        //          StringBuilder buf = new StringBuilder(noOfItems * 16);
        //          for (int i = startIndex; i < endIndex; i++)
        //          {
        //              if (i > startIndex)
        //              {
        //                  buf.Append(separator);
        //              }
        //              buf.Append(array[i]);
        //          }
        //          return buf.ToString();
        //      }

        //      /// <summary>
        //      /// <para>
        //      /// Joins the elements of the provided array into a single String containing the provided list of elements.
        //      /// </para>
        //      /// 
        //      /// <para>
        //      /// No delimiter is added before or after the list. Null objects or empty strings within the array are represented
        //      /// by empty strings.
        //      /// </para>
        //      /// 
        //      /// <pre>
        //      /// StringUtils.join(null, *)               = null
        //      /// StringUtils.join([], *)                 = ""
        //      /// StringUtils.join([null], *)             = ""
        //      /// StringUtils.join([1, 2, 3], ';')  = "1;2;3"
        //      /// StringUtils.join([1, 2, 3], null) = "123"
        //      /// </pre>
        //      /// </summary>
        //      /// <param name="array">
        //      ///            the array of values to join together, may be null </param>
        //      /// <param name="separator">
        //      ///            the separator character to use </param>
        //      /// <param name="startIndex">
        //      ///            the first index to start joining from. It is an error to pass in an end index past the end of the
        //      ///            array </param>
        //      /// <param name="endIndex">
        //      ///            the index to stop joining from (exclusive). It is an error to pass in an end index past the end of
        //      ///            the array </param>
        //      /// <returns> the joined String, {@code null} if null array input
        //      /// @since 3.2 </returns>
        //      //JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
        //      //ORIGINAL LINE: public static String join(final int[] array, final char separator, final int startIndex, final int endIndex)
        //      public static string join(int[] array, char separator, int startIndex, int endIndex)
        //      {
        //          if (array == null)
        //          {
        //              return null;
        //          }
        //          //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
        //          //ORIGINAL LINE: final int noOfItems = endIndex - startIndex;
        //          int noOfItems = endIndex - startIndex;
        //          if (noOfItems <= 0)
        //          {
        //              return EMPTY;
        //          }
        //          //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
        //          //ORIGINAL LINE: final StringBuilder buf = new StringBuilder(noOfItems * 16);
        //          StringBuilder buf = new StringBuilder(noOfItems * 16);
        //          for (int i = startIndex; i < endIndex; i++)
        //          {
        //              if (i > startIndex)
        //              {
        //                  buf.Append(separator);
        //              }
        //              buf.Append(array[i]);
        //          }
        //          return buf.ToString();
        //      }

        //      /// <summary>
        //      /// <para>
        //      /// Joins the elements of the provided array into a single String containing the provided list of elements.
        //      /// </para>
        //      /// 
        //      /// <para>
        //      /// No delimiter is added before or after the list. Null objects or empty strings within the array are represented
        //      /// by empty strings.
        //      /// </para>
        //      /// 
        //      /// <pre>
        //      /// StringUtils.join(null, *)               = null
        //      /// StringUtils.join([], *)                 = ""
        //      /// StringUtils.join([null], *)             = ""
        //      /// StringUtils.join([1, 2, 3], ';')  = "1;2;3"
        //      /// StringUtils.join([1, 2, 3], null) = "123"
        //      /// </pre>
        //      /// </summary>
        //      /// <param name="array">
        //      ///            the array of values to join together, may be null </param>
        //      /// <param name="separator">
        //      ///            the separator character to use </param>
        //      /// <param name="startIndex">
        //      ///            the first index to start joining from. It is an error to pass in an end index past the end of the
        //      ///            array </param>
        //      /// <param name="endIndex">
        //      ///            the index to stop joining from (exclusive). It is an error to pass in an end index past the end of
        //      ///            the array </param>
        //      /// <returns> the joined String, {@code null} if null array input
        //      /// @since 3.2 </returns>
        //      //JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
        //      //ORIGINAL LINE: public static String join(final byte[] array, final char separator, final int startIndex, final int endIndex)
        //      public static string join(sbyte[] array, char separator, int startIndex, int endIndex)
        //      {
        //          if (array == null)
        //          {
        //              return null;
        //          }
        //          //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
        //          //ORIGINAL LINE: final int noOfItems = endIndex - startIndex;
        //          int noOfItems = endIndex - startIndex;
        //          if (noOfItems <= 0)
        //          {
        //              return EMPTY;
        //          }
        //          //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
        //          //ORIGINAL LINE: final StringBuilder buf = new StringBuilder(noOfItems * 16);
        //          StringBuilder buf = new StringBuilder(noOfItems * 16);
        //          for (int i = startIndex; i < endIndex; i++)
        //          {
        //              if (i > startIndex)
        //              {
        //                  buf.Append(separator);
        //              }
        //              buf.Append(array[i]);
        //          }
        //          return buf.ToString();
        //      }

        //      /// <summary>
        //      /// <para>
        //      /// Joins the elements of the provided array into a single String containing the provided list of elements.
        //      /// </para>
        //      /// 
        //      /// <para>
        //      /// No delimiter is added before or after the list. Null objects or empty strings within the array are represented
        //      /// by empty strings.
        //      /// </para>
        //      /// 
        //      /// <pre>
        //      /// StringUtils.join(null, *)               = null
        //      /// StringUtils.join([], *)                 = ""
        //      /// StringUtils.join([null], *)             = ""
        //      /// StringUtils.join([1, 2, 3], ';')  = "1;2;3"
        //      /// StringUtils.join([1, 2, 3], null) = "123"
        //      /// </pre>
        //      /// </summary>
        //      /// <param name="array">
        //      ///            the array of values to join together, may be null </param>
        //      /// <param name="separator">
        //      ///            the separator character to use </param>
        //      /// <param name="startIndex">
        //      ///            the first index to start joining from. It is an error to pass in an end index past the end of the
        //      ///            array </param>
        //      /// <param name="endIndex">
        //      ///            the index to stop joining from (exclusive). It is an error to pass in an end index past the end of
        //      ///            the array </param>
        //      /// <returns> the joined String, {@code null} if null array input
        //      /// @since 3.2 </returns>
        //      //JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
        //      //ORIGINAL LINE: public static String join(final short[] array, final char separator, final int startIndex, final int endIndex)
        //      public static string join(short[] array, char separator, int startIndex, int endIndex)
        //      {
        //          if (array == null)
        //          {
        //              return null;
        //          }
        //          //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
        //          //ORIGINAL LINE: final int noOfItems = endIndex - startIndex;
        //          int noOfItems = endIndex - startIndex;
        //          if (noOfItems <= 0)
        //          {
        //              return EMPTY;
        //          }
        //          //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
        //          //ORIGINAL LINE: final StringBuilder buf = new StringBuilder(noOfItems * 16);
        //          StringBuilder buf = new StringBuilder(noOfItems * 16);
        //          for (int i = startIndex; i < endIndex; i++)
        //          {
        //              if (i > startIndex)
        //              {
        //                  buf.Append(separator);
        //              }
        //              buf.Append(array[i]);
        //          }
        //          return buf.ToString();
        //      }

        //      /// <summary>
        //      /// <para>
        //      /// Joins the elements of the provided array into a single String containing the provided list of elements.
        //      /// </para>
        //      /// 
        //      /// <para>
        //      /// No delimiter is added before or after the list. Null objects or empty strings within the array are represented
        //      /// by empty strings.
        //      /// </para>
        //      /// 
        //      /// <pre>
        //      /// StringUtils.join(null, *)               = null
        //      /// StringUtils.join([], *)                 = ""
        //      /// StringUtils.join([null], *)             = ""
        //      /// StringUtils.join([1, 2, 3], ';')  = "1;2;3"
        //      /// StringUtils.join([1, 2, 3], null) = "123"
        //      /// </pre>
        //      /// </summary>
        //      /// <param name="array">
        //      ///            the array of values to join together, may be null </param>
        //      /// <param name="separator">
        //      ///            the separator character to use </param>
        //      /// <param name="startIndex">
        //      ///            the first index to start joining from. It is an error to pass in an end index past the end of the
        //      ///            array </param>
        //      /// <param name="endIndex">
        //      ///            the index to stop joining from (exclusive). It is an error to pass in an end index past the end of
        //      ///            the array </param>
        //      /// <returns> the joined String, {@code null} if null array input
        //      /// @since 3.2 </returns>
        //      //JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
        //      //ORIGINAL LINE: public static String join(final char[] array, final char separator, final int startIndex, final int endIndex)
        //      public static string join(char[] array, char separator, int startIndex, int endIndex)
        //      {
        //          if (array == null)
        //          {
        //              return null;
        //          }
        //          //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
        //          //ORIGINAL LINE: final int noOfItems = endIndex - startIndex;
        //          int noOfItems = endIndex - startIndex;
        //          if (noOfItems <= 0)
        //          {
        //              return EMPTY;
        //          }
        //          //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
        //          //ORIGINAL LINE: final StringBuilder buf = new StringBuilder(noOfItems * 16);
        //          StringBuilder buf = new StringBuilder(noOfItems * 16);
        //          for (int i = startIndex; i < endIndex; i++)
        //          {
        //              if (i > startIndex)
        //              {
        //                  buf.Append(separator);
        //              }
        //              buf.Append(array[i]);
        //          }
        //          return buf.ToString();
        //      }

        //      /// <summary>
        //      /// <para>
        //      /// Joins the elements of the provided array into a single String containing the provided list of elements.
        //      /// </para>
        //      /// 
        //      /// <para>
        //      /// No delimiter is added before or after the list. Null objects or empty strings within the array are represented
        //      /// by empty strings.
        //      /// </para>
        //      /// 
        //      /// <pre>
        //      /// StringUtils.join(null, *)               = null
        //      /// StringUtils.join([], *)                 = ""
        //      /// StringUtils.join([null], *)             = ""
        //      /// StringUtils.join([1, 2, 3], ';')  = "1;2;3"
        //      /// StringUtils.join([1, 2, 3], null) = "123"
        //      /// </pre>
        //      /// </summary>
        //      /// <param name="array">
        //      ///            the array of values to join together, may be null </param>
        //      /// <param name="separator">
        //      ///            the separator character to use </param>
        //      /// <param name="startIndex">
        //      ///            the first index to start joining from. It is an error to pass in an end index past the end of the
        //      ///            array </param>
        //      /// <param name="endIndex">
        //      ///            the index to stop joining from (exclusive). It is an error to pass in an end index past the end of
        //      ///            the array </param>
        //      /// <returns> the joined String, {@code null} if null array input
        //      /// @since 3.2 </returns>
        //      //JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
        //      //ORIGINAL LINE: public static String join(final double[] array, final char separator, final int startIndex, final int endIndex)
        //      public static string join(double[] array, char separator, int startIndex, int endIndex)
        //      {
        //          if (array == null)
        //          {
        //              return null;
        //          }
        //          //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
        //          //ORIGINAL LINE: final int noOfItems = endIndex - startIndex;
        //          int noOfItems = endIndex - startIndex;
        //          if (noOfItems <= 0)
        //          {
        //              return EMPTY;
        //          }
        //          //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
        //          //ORIGINAL LINE: final StringBuilder buf = new StringBuilder(noOfItems * 16);
        //          StringBuilder buf = new StringBuilder(noOfItems * 16);
        //          for (int i = startIndex; i < endIndex; i++)
        //          {
        //              if (i > startIndex)
        //              {
        //                  buf.Append(separator);
        //              }
        //              buf.Append(array[i]);
        //          }
        //          return buf.ToString();
        //      }

        //      /// <summary>
        //      /// <para>
        //      /// Joins the elements of the provided array into a single String containing the provided list of elements.
        //      /// </para>
        //      /// 
        //      /// <para>
        //      /// No delimiter is added before or after the list. Null objects or empty strings within the array are represented
        //      /// by empty strings.
        //      /// </para>
        //      /// 
        //      /// <pre>
        //      /// StringUtils.join(null, *)               = null
        //      /// StringUtils.join([], *)                 = ""
        //      /// StringUtils.join([null], *)             = ""
        //      /// StringUtils.join([1, 2, 3], ';')  = "1;2;3"
        //      /// StringUtils.join([1, 2, 3], null) = "123"
        //      /// </pre>
        //      /// </summary>
        //      /// <param name="array">
        //      ///            the array of values to join together, may be null </param>
        //      /// <param name="separator">
        //      ///            the separator character to use </param>
        //      /// <param name="startIndex">
        //      ///            the first index to start joining from. It is an error to pass in an end index past the end of the
        //      ///            array </param>
        //      /// <param name="endIndex">
        //      ///            the index to stop joining from (exclusive). It is an error to pass in an end index past the end of
        //      ///            the array </param>
        //      /// <returns> the joined String, {@code null} if null array input
        //      /// @since 3.2 </returns>
        //      //JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
        //      //ORIGINAL LINE: public static String join(final float[] array, final char separator, final int startIndex, final int endIndex)
        //      public static string join(float[] array, char separator, int startIndex, int endIndex)
        //      {
        //          if (array == null)
        //          {
        //              return null;
        //          }
        //          //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
        //          //ORIGINAL LINE: final int noOfItems = endIndex - startIndex;
        //          int noOfItems = endIndex - startIndex;
        //          if (noOfItems <= 0)
        //          {
        //              return EMPTY;
        //          }
        //          //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
        //          //ORIGINAL LINE: final StringBuilder buf = new StringBuilder(noOfItems * 16);
        //          StringBuilder buf = new StringBuilder(noOfItems * 16);
        //          for (int i = startIndex; i < endIndex; i++)
        //          {
        //              if (i > startIndex)
        //              {
        //                  buf.Append(separator);
        //              }
        //              buf.Append(array[i]);
        //          }
        //          return buf.ToString();
        //      }


        //      /// <summary>
        //      /// <para>Joins the elements of the provided array into a single String
        //      /// containing the provided list of elements.</para>
        //      /// 
        //      /// <para>No delimiter is added before or after the list.
        //      /// A {@code null} separator is the same as an empty String ("").
        //      /// Null objects or empty strings within the array are represented by
        //      /// empty strings.</para>
        //      /// 
        //      /// <pre>
        //      /// StringUtils.join(null, *)                = null
        //      /// StringUtils.join([], *)                  = ""
        //      /// StringUtils.join([null], *)              = ""
        //      /// StringUtils.join(["a", "b", "c"], "--")  = "a--b--c"
        //      /// StringUtils.join(["a", "b", "c"], null)  = "abc"
        //      /// StringUtils.join(["a", "b", "c"], "")    = "abc"
        //      /// StringUtils.join([null, "", "a"], ',')   = ",,a"
        //      /// </pre>
        //      /// </summary>
        //      /// <param name="array">  the array of values to join together, may be null </param>
        //      /// <param name="separator">  the separator character to use, null treated as "" </param>
        //      /// <returns> the joined String, {@code null} if null array input </returns>
        //      //JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
        //      //ORIGINAL LINE: public static String join(final Object[] array, final String separator)
        //      public static string join(object[] array, string separator)
        //      {
        //          if (array == null)
        //          {
        //              return null;
        //          }
        //          return join(array, separator, 0, array.Length);
        //      }

        //      /// <summary>
        //      /// <para>Joins the elements of the provided array into a single String
        //      /// containing the provided list of elements.</para>
        //      /// 
        //      /// <para>No delimiter is added before or after the list.
        //      /// A {@code null} separator is the same as an empty String ("").
        //      /// Null objects or empty strings within the array are represented by
        //      /// empty strings.</para>
        //      /// 
        //      /// <pre>
        //      /// StringUtils.join(null, *, *, *)                = null
        //      /// StringUtils.join([], *, *, *)                  = ""
        //      /// StringUtils.join([null], *, *, *)              = ""
        //      /// StringUtils.join(["a", "b", "c"], "--", 0, 3)  = "a--b--c"
        //      /// StringUtils.join(["a", "b", "c"], "--", 1, 3)  = "b--c"
        //      /// StringUtils.join(["a", "b", "c"], "--", 2, 3)  = "c"
        //      /// StringUtils.join(["a", "b", "c"], "--", 2, 2)  = ""
        //      /// StringUtils.join(["a", "b", "c"], null, 0, 3)  = "abc"
        //      /// StringUtils.join(["a", "b", "c"], "", 0, 3)    = "abc"
        //      /// StringUtils.join([null, "", "a"], ',', 0, 3)   = ",,a"
        //      /// </pre>
        //      /// </summary>
        //      /// <param name="array">  the array of values to join together, may be null </param>
        //      /// <param name="separator">  the separator character to use, null treated as "" </param>
        //      /// <param name="startIndex"> the first index to start joining from. </param>
        //      /// <param name="endIndex"> the index to stop joining from (exclusive). </param>
        //      /// <returns> the joined String, {@code null} if null array input; or the empty string
        //      /// if {@code endIndex - startIndex <= 0}. The number of joined entries is given by
        //      /// {@code endIndex - startIndex} </returns>
        //      /// <exception cref="ArrayIndexOutOfBoundsException"> ife<br>
        //      /// {@code startIndex < 0} or <br>
        //      /// {@code startIndex >= array.length()} or <br>
        //      /// {@code endIndex < 0} or <br>
        //      /// {@code endIndex > array.length()} </exception>
        //      //JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
        //      //ORIGINAL LINE: public static String join(final Object[] array, String separator, final int startIndex, final int endIndex)
        //      public static string join(object[] array, string separator, int startIndex, int endIndex)
        //      {
        //          if (array == null)
        //          {
        //              return null;
        //          }
        //          if (string.ReferenceEquals(separator, null))
        //          {
        //              separator = EMPTY;
        //          }

        //          // endIndex - startIndex > 0:   Len = NofStrings *(len(firstString) + len(separator))
        //          //           (Assuming that all Strings are roughly equally long)
        //          //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
        //          //ORIGINAL LINE: final int noOfItems = endIndex - startIndex;
        //          int noOfItems = endIndex - startIndex;
        //          if (noOfItems <= 0)
        //          {
        //              return EMPTY;
        //          }

        //          //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
        //          //ORIGINAL LINE: final StringBuilder buf = new StringBuilder(noOfItems * 16);
        //          StringBuilder buf = new StringBuilder(noOfItems * 16);

        //          for (int i = startIndex; i < endIndex; i++)
        //          {
        //              if (i > startIndex)
        //              {
        //                  buf.Append(separator);
        //              }
        //              if (array[i] != null)
        //              {
        //                  buf.Append(array[i]);
        //              }
        //          }
        //          return buf.ToString();
        //      }

        //      /// <summary>
        //      /// <para>Joins the elements of the provided {@code Iterator} into
        //      /// a single String containing the provided elements.</para>
        //      /// 
        //      /// <para>No delimiter is added before or after the list. Null objects or empty
        //      /// strings within the iteration are represented by empty strings.</para>
        //      /// 
        //      /// <para>See the examples here: <seealso cref="#join(Object[],char)"/>. </para>
        //      /// </summary>
        //      /// <param name="iterator">  the {@code Iterator} of values to join together, may be null </param>
        //      /// <param name="separator">  the separator character to use </param>
        //      /// <returns> the joined String, {@code null} if null iterator input
        //      /// @since 2.0 </returns>
        //      //JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
        //      //ORIGINAL LINE: public static String join(final java.util.Iterator<?> iterator, final char separator)
        //      public static string join<T1>(IEnumerator<T1> iterator, char separator)
        //      {

        //          // handle null, zero and one elements before building a buffer
        //          if (iterator == null)
        //          {
        //              return null;
        //          }
        //          //JAVA TO C# CONVERTER TODO TASK: Java iterators are only converted within the context of 'while' and 'for' loops:
        //          if (!iterator.hasNext())
        //          {
        //              return EMPTY;
        //          }
        //          //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
        //          //ORIGINAL LINE: final Object first = iterator.next();
        //          //JAVA TO C# CONVERTER TODO TASK: Java iterators are only converted within the context of 'while' and 'for' loops:
        //          object first = iterator.next();
        //          //JAVA TO C# CONVERTER TODO TASK: Java iterators are only converted within the context of 'while' and 'for' loops:
        //          if (!iterator.hasNext())
        //          {
        //              //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
        //              //ORIGINAL LINE: final String result = java.util.Objects.toString(first, "");
        //              string result = Objects.ToString(first, "");
        //              return result;
        //          }

        //          // two or more elements
        //          //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
        //          //ORIGINAL LINE: final StringBuilder buf = new StringBuilder(256);
        //          StringBuilder buf = new StringBuilder(256); // Java default is 16, probably too small
        //          if (first != null)
        //          {
        //              buf.Append(first);
        //          }

        //          while (iterator.MoveNext())
        //          {
        //              buf.Append(separator);
        //              //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
        //              //ORIGINAL LINE: final Object obj = iterator.Current;
        //              object obj = iterator.Current;
        //              if (obj != null)
        //              {
        //                  buf.Append(obj);
        //              }
        //          }

        //          return buf.ToString();
        //      }

        //      /// <summary>
        //      /// <para>Joins the elements of the provided {@code Iterator} into
        //      /// a single String containing the provided elements.</para>
        //      /// 
        //      /// <para>No delimiter is added before or after the list.
        //      /// A {@code null} separator is the same as an empty String ("").</para>
        //      /// 
        //      /// <para>See the examples here: <seealso cref="#join(Object[],String)"/>. </para>
        //      /// </summary>
        //      /// <param name="iterator">  the {@code Iterator} of values to join together, may be null </param>
        //      /// <param name="separator">  the separator character to use, null treated as "" </param>
        //      /// <returns> the joined String, {@code null} if null iterator input </returns>
        //      //JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
        //      //ORIGINAL LINE: public static String join(final java.util.Iterator<?> iterator, final String separator)
        //      public static string join<T1>(IEnumerator<T1> iterator, string separator)
        //      {

        //          // handle null, zero and one elements before building a buffer
        //          if (iterator == null)
        //          {
        //              return null;
        //          }
        //          //JAVA TO C# CONVERTER TODO TASK: Java iterators are only converted within the context of 'while' and 'for' loops:
        //          if (!iterator.hasNext())
        //          {
        //              return EMPTY;
        //          }
        //          //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
        //          //ORIGINAL LINE: final Object first = iterator.next();
        //          //JAVA TO C# CONVERTER TODO TASK: Java iterators are only converted within the context of 'while' and 'for' loops:
        //          object first = iterator.next();
        //          //JAVA TO C# CONVERTER TODO TASK: Java iterators are only converted within the context of 'while' and 'for' loops:
        //          if (!iterator.hasNext())
        //          {
        //              //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
        //              //ORIGINAL LINE: final String result = java.util.Objects.toString(first, "");
        //              string result = Objects.ToString(first, "");
        //              return result;
        //          }

        //          // two or more elements
        //          //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
        //          //ORIGINAL LINE: final StringBuilder buf = new StringBuilder(256);
        //          StringBuilder buf = new StringBuilder(256); // Java default is 16, probably too small
        //          if (first != null)
        //          {
        //              buf.Append(first);
        //          }

        //          while (iterator.MoveNext())
        //          {
        //              if (!string.ReferenceEquals(separator, null))
        //              {
        //                  buf.Append(separator);
        //              }
        //              //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
        //              //ORIGINAL LINE: final Object obj = iterator.Current;
        //              object obj = iterator.Current;
        //              if (obj != null)
        //              {
        //                  buf.Append(obj);
        //              }
        //          }
        //          return buf.ToString();
        //      }

        //      /// <summary>
        //      /// <para>Joins the elements of the provided {@code Iterable} into
        //      /// a single String containing the provided elements.</para>
        //      /// 
        //      /// <para>No delimiter is added before or after the list. Null objects or empty
        //      /// strings within the iteration are represented by empty strings.</para>
        //      /// 
        //      /// <para>See the examples here: <seealso cref="#join(Object[],char)"/>. </para>
        //      /// </summary>
        //      /// <param name="iterable">  the {@code Iterable} providing the values to join together, may be null </param>
        //      /// <param name="separator">  the separator character to use </param>
        //      /// <returns> the joined String, {@code null} if null iterator input
        //      /// @since 2.3 </returns>
        //      //JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
        //      //ORIGINAL LINE: public static String join(final Iterable<?> iterable, final char separator)
        //      public static string join<T1>(IEnumerable<T1> iterable, char separator)
        //      {
        //          if (iterable == null)
        //          {
        //              return null;
        //          }
        //          return join(iterable.GetEnumerator(), separator);
        //      }

        //      /// <summary>
        //      /// <para>Joins the elements of the provided {@code Iterable} into
        //      /// a single String containing the provided elements.</para>
        //      /// 
        //      /// <para>No delimiter is added before or after the list.
        //      /// A {@code null} separator is the same as an empty String ("").</para>
        //      /// 
        //      /// <para>See the examples here: <seealso cref="#join(Object[],String)"/>. </para>
        //      /// </summary>
        //      /// <param name="iterable">  the {@code Iterable} providing the values to join together, may be null </param>
        //      /// <param name="separator">  the separator character to use, null treated as "" </param>
        //      /// <returns> the joined String, {@code null} if null iterator input
        //      /// @since 2.3 </returns>
        //      //JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
        //      //ORIGINAL LINE: public static String join(final Iterable<?> iterable, final String separator)
        //      public static string join<T1>(IEnumerable<T1> iterable, string separator)
        //      {
        //          if (iterable == null)
        //          {
        //              return null;
        //          }
        //          return join(iterable.GetEnumerator(), separator);
        //      }

        //      /// <summary>
        //      /// <para>Joins the elements of the provided varargs into a
        //      /// single String containing the provided elements.</para>
        //      /// 
        //      /// <para>No delimiter is added before or after the list.
        //      /// {@code null} elements and separator are treated as empty Strings ("").</para>
        //      /// 
        //      /// <pre>
        //      /// StringUtils.joinWith(",", {"a", "b"})        = "a,b"
        //      /// StringUtils.joinWith(",", {"a", "b",""})     = "a,b,"
        //      /// StringUtils.joinWith(",", {"a", null, "b"})  = "a,,b"
        //      /// StringUtils.joinWith(null, {"a", "b"})       = "ab"
        //      /// </pre>
        //      /// </summary>
        //      /// <param name="separator"> the separator character to use, null treated as "" </param>
        //      /// <param name="objects"> the varargs providing the values to join together. {@code null} elements are treated as "" </param>
        //      /// <returns> the joined String. </returns>
        //      /// <exception cref="java.lang.IllegalArgumentException"> if a null varargs is provided
        //      /// @since 3.5 </exception>
        //      //JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
        //      //ORIGINAL LINE: public static String joinWith(final String separator, final Object... objects)
        //      public static string joinWith(string separator, params object[] objects)
        //      {
        //          if (objects == null)
        //          {
        //              throw new System.ArgumentException("Object varargs must not be null");
        //          }

        //          //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
        //          //ORIGINAL LINE: final String sanitizedSeparator = defaultString(separator, StringUtils.EMPTY);
        //          string sanitizedSeparator = defaultString(separator, StringUtils.EMPTY);

        //          //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
        //          //ORIGINAL LINE: final StringBuilder result = new StringBuilder();
        //          StringBuilder result = new StringBuilder();

        //          //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
        //          //ORIGINAL LINE: final java.util.Iterator<Object> iterator = java.util.Arrays.asList(objects).iterator();
        //          IEnumerator<object> iterator = Arrays.asList(objects).GetEnumerator();
        //          while (iterator.MoveNext())
        //          {
        //              //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
        //              //ORIGINAL LINE: final String value = java.util.Objects.toString(iterator.Current, "");
        //              string value = Objects.ToString(iterator.Current, "");
        //              result.Append(value);

        //              //JAVA TO C# CONVERTER TODO TASK: Java iterators are only converted within the context of 'while' and 'for' loops:
        //              if (iterator.hasNext())
        //              {
        //                  result.Append(sanitizedSeparator);
        //              }
        //          }

        //          return result.ToString();
        //      }

        //      // Delete
        //      //-----------------------------------------------------------------------
        //      /// <summary>
        //      /// <para>Deletes all whitespaces from a String as defined by
        //      /// <seealso cref="Character#isWhitespace(char)"/>.</para>
        //      /// 
        //      /// <pre>
        //      /// StringUtils.deleteWhitespace(null)         = null
        //      /// StringUtils.deleteWhitespace("")           = ""
        //      /// StringUtils.deleteWhitespace("abc")        = "abc"
        //      /// StringUtils.deleteWhitespace("   ab  c  ") = "abc"
        //      /// </pre>
        //      /// </summary>
        //      /// <param name="str">  the String to delete whitespace from, may be null </param>
        //      /// <returns> the String without whitespaces, {@code null} if null String input </returns>
        //      //JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
        //      //ORIGINAL LINE: public static String deleteWhitespace(final String str)
        //      public static string deleteWhitespace(string str)
        //      {
        //          if (isEmpty(str))
        //          {
        //              return str;
        //          }
        //          //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
        //          //ORIGINAL LINE: final int sz = str.length();
        //          int sz = str.Length;
        //          //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
        //          //ORIGINAL LINE: final char[] chs = new char[sz];
        //          char[] chs = new char[sz];
        //          int count = 0;
        //          for (int i = 0; i < sz; i++)
        //          {
        //              if (!char.IsWhiteSpace(str[i]))
        //              {
        //                  chs[count++] = str[i];
        //              }
        //          }
        //          if (count == sz)
        //          {
        //              return str;
        //          }
        //          return new string(chs, 0, count);
        //      }

        //      // Remove
        //      //-----------------------------------------------------------------------
        //      /// <summary>
        //      /// <para>Removes a substring only if it is at the beginning of a source string,
        //      /// otherwise returns the source string.</para>
        //      /// 
        //      /// <para>A {@code null} source string will return {@code null}.
        //      /// An empty ("") source string will return the empty string.
        //      /// A {@code null} search string will return the source string.</para>
        //      /// 
        //      /// <pre>
        //      /// StringUtils.removeStart(null, *)      = null
        //      /// StringUtils.removeStart("", *)        = ""
        //      /// StringUtils.removeStart(*, null)      = *
        //      /// StringUtils.removeStart("www.domain.com", "www.")   = "domain.com"
        //      /// StringUtils.removeStart("domain.com", "www.")       = "domain.com"
        //      /// StringUtils.removeStart("www.domain.com", "domain") = "www.domain.com"
        //      /// StringUtils.removeStart("abc", "")    = "abc"
        //      /// </pre>
        //      /// </summary>
        //      /// <param name="str">  the source String to search, may be null </param>
        //      /// <param name="remove">  the String to search for and remove, may be null </param>
        //      /// <returns> the substring with the string removed if found,
        //      ///  {@code null} if null String input
        //      /// @since 2.1 </returns>
        //      //JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
        //      //ORIGINAL LINE: public static String removeStart(final String str, final String remove)
        //      public static string removeStart(string str, string remove)
        //      {
        //          if (isEmpty(str) || isEmpty(remove))
        //          {
        //              return str;
        //          }
        //          if (str.StartsWith(remove, StringComparison.Ordinal))
        //          {
        //              return str.Substring(remove.Length);
        //          }
        //          return str;
        //      }

        //      /// <summary>
        //      /// <para>Case insensitive removal of a substring if it is at the beginning of a source string,
        //      /// otherwise returns the source string.</para>
        //      /// 
        //      /// <para>A {@code null} source string will return {@code null}.
        //      /// An empty ("") source string will return the empty string.
        //      /// A {@code null} search string will return the source string.</para>
        //      /// 
        //      /// <pre>
        //      /// StringUtils.removeStartIgnoreCase(null, *)      = null
        //      /// StringUtils.removeStartIgnoreCase("", *)        = ""
        //      /// StringUtils.removeStartIgnoreCase(*, null)      = *
        //      /// StringUtils.removeStartIgnoreCase("www.domain.com", "www.")   = "domain.com"
        //      /// StringUtils.removeStartIgnoreCase("www.domain.com", "WWW.")   = "domain.com"
        //      /// StringUtils.removeStartIgnoreCase("domain.com", "www.")       = "domain.com"
        //      /// StringUtils.removeStartIgnoreCase("www.domain.com", "domain") = "www.domain.com"
        //      /// StringUtils.removeStartIgnoreCase("abc", "")    = "abc"
        //      /// </pre>
        //      /// </summary>
        //      /// <param name="str">  the source String to search, may be null </param>
        //      /// <param name="remove">  the String to search for (case insensitive) and remove, may be null </param>
        //      /// <returns> the substring with the string removed if found,
        //      ///  {@code null} if null String input
        //      /// @since 2.4 </returns>
        //      //JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
        //      //ORIGINAL LINE: public static String removeStartIgnoreCase(final String str, final String remove)
        //      public static string removeStartIgnoreCase(string str, string remove)
        //      {
        //          if (isEmpty(str) || isEmpty(remove))
        //          {
        //              return str;
        //          }
        //          if (startsWithIgnoreCase(str, remove))
        //          {
        //              return str.Substring(remove.Length);
        //          }
        //          return str;
        //      }

        /// <summary>
        /// <para>Removes a substring only if it is at the end of a source string,
        /// otherwise returns the source string.</para>
        /// 
        /// <para>A {@code null} source string will return {@code null}.
        /// An empty ("") source string will return the empty string.
        /// A {@code null} search string will return the source string.</para>
        /// 
        /// <pre>
        /// StringUtils.removeEnd(null, *)      = null
        /// StringUtils.removeEnd("", *)        = ""
        /// StringUtils.removeEnd(*, null)      = *
        /// StringUtils.removeEnd("www.domain.com", ".com.")  = "www.domain.com"
        /// StringUtils.removeEnd("www.domain.com", ".com")   = "www.domain"
        /// StringUtils.removeEnd("www.domain.com", "domain") = "www.domain.com"
        /// StringUtils.removeEnd("abc", "")    = "abc"
        /// </pre>
        /// </summary>
        /// <param name="str">  the source String to search, may be null </param>
        /// <param name="remove">  the String to search for and remove, may be null </param>
        /// <returns> the substring with the string removed if found,
        ///  {@code null} if null String input
        /// @since 2.1 </returns>
        //JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
        //ORIGINAL LINE: public static String removeEnd(final String str, final String remove)
        public static string removeEnd(string str, string remove)
        {
            if (isEmpty(str) || isEmpty(remove))
            {
                return str;
            }
            if (str.EndsWith(remove, StringComparison.Ordinal))
            {
                return str.Substring(0, str.Length - remove.Length);
            }
            return str;
        }

        //      /// <summary>
        //      /// <para>Case insensitive removal of a substring if it is at the end of a source string,
        //      /// otherwise returns the source string.</para>
        //      /// 
        //      /// <para>A {@code null} source string will return {@code null}.
        //      /// An empty ("") source string will return the empty string.
        //      /// A {@code null} search string will return the source string.</para>
        //      /// 
        //      /// <pre>
        //      /// StringUtils.removeEndIgnoreCase(null, *)      = null
        //      /// StringUtils.removeEndIgnoreCase("", *)        = ""
        //      /// StringUtils.removeEndIgnoreCase(*, null)      = *
        //      /// StringUtils.removeEndIgnoreCase("www.domain.com", ".com.")  = "www.domain.com"
        //      /// StringUtils.removeEndIgnoreCase("www.domain.com", ".com")   = "www.domain"
        //      /// StringUtils.removeEndIgnoreCase("www.domain.com", "domain") = "www.domain.com"
        //      /// StringUtils.removeEndIgnoreCase("abc", "")    = "abc"
        //      /// StringUtils.removeEndIgnoreCase("www.domain.com", ".COM") = "www.domain")
        //      /// StringUtils.removeEndIgnoreCase("www.domain.COM", ".com") = "www.domain")
        //      /// </pre>
        //      /// </summary>
        //      /// <param name="str">  the source String to search, may be null </param>
        //      /// <param name="remove">  the String to search for (case insensitive) and remove, may be null </param>
        //      /// <returns> the substring with the string removed if found,
        //      ///  {@code null} if null String input
        //      /// @since 2.4 </returns>
        //      //JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
        //      //ORIGINAL LINE: public static String removeEndIgnoreCase(final String str, final String remove)
        //      public static string removeEndIgnoreCase(string str, string remove)
        //      {
        //          if (isEmpty(str) || isEmpty(remove))
        //          {
        //              return str;
        //          }
        //          if (endsWithIgnoreCase(str, remove))
        //          {
        //              return str.Substring(0, str.Length - remove.Length);
        //          }
        //          return str;
        //      }

        //      /// <summary>
        //      /// <para>Removes all occurrences of a substring from within the source string.</para>
        //      /// 
        //      /// <para>A {@code null} source string will return {@code null}.
        //      /// An empty ("") source string will return the empty string.
        //      /// A {@code null} remove string will return the source string.
        //      /// An empty ("") remove string will return the source string.</para>
        //      /// 
        //      /// <pre>
        //      /// StringUtils.remove(null, *)        = null
        //      /// StringUtils.remove("", *)          = ""
        //      /// StringUtils.remove(*, null)        = *
        //      /// StringUtils.remove(*, "")          = *
        //      /// StringUtils.remove("queued", "ue") = "qd"
        //      /// StringUtils.remove("queued", "zz") = "queued"
        //      /// </pre>
        //      /// </summary>
        //      /// <param name="str">  the source String to search, may be null </param>
        //      /// <param name="remove">  the String to search for and remove, may be null </param>
        //      /// <returns> the substring with the string removed if found,
        //      ///  {@code null} if null String input
        //      /// @since 2.1 </returns>
        //      //JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
        //      //ORIGINAL LINE: public static String remove(final String str, final String remove)
        //      public static string remove(string str, string remove)
        //      {
        //          if (isEmpty(str) || isEmpty(remove))
        //          {
        //              return str;
        //          }
        //          return replace(str, remove, EMPTY, -1);
        //      }

        //      /// <summary>
        //      /// <para>
        //      /// Case insensitive removal of all occurrences of a substring from within
        //      /// the source string.
        //      /// </para>
        //      /// 
        //      /// <para>
        //      /// A {@code null} source string will return {@code null}. An empty ("")
        //      /// source string will return the empty string. A {@code null} remove string
        //      /// will return the source string. An empty ("") remove string will return
        //      /// the source string.
        //      /// </para>
        //      /// 
        //      /// <pre>
        //      /// StringUtils.removeIgnoreCase(null, *)        = null
        //      /// StringUtils.removeIgnoreCase("", *)          = ""
        //      /// StringUtils.removeIgnoreCase(*, null)        = *
        //      /// StringUtils.removeIgnoreCase(*, "")          = *
        //      /// StringUtils.removeIgnoreCase("queued", "ue") = "qd"
        //      /// StringUtils.removeIgnoreCase("queued", "zz") = "queued"
        //      /// StringUtils.removeIgnoreCase("quEUed", "UE") = "qd"
        //      /// StringUtils.removeIgnoreCase("queued", "zZ") = "queued"
        //      /// </pre>
        //      /// </summary>
        //      /// <param name="str">
        //      ///            the source String to search, may be null </param>
        //      /// <param name="remove">
        //      ///            the String to search for (case insensitive) and remove, may be
        //      ///            null </param>
        //      /// <returns> the substring with the string removed if found, {@code null} if
        //      ///         null String input
        //      /// @since 3.5 </returns>
        //      //JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
        //      //ORIGINAL LINE: public static String removeIgnoreCase(final String str, final String remove)
        //      public static string removeIgnoreCase(string str, string remove)
        //      {
        //          if (isEmpty(str) || isEmpty(remove))
        //          {
        //              return str;
        //          }
        //          return replaceIgnoreCase(str, remove, EMPTY, -1);
        //      }

        //      /// <summary>
        //      /// <para>Removes all occurrences of a character from within the source string.</para>
        //      /// 
        //      /// <para>A {@code null} source string will return {@code null}.
        //      /// An empty ("") source string will return the empty string.</para>
        //      /// 
        //      /// <pre>
        //      /// StringUtils.remove(null, *)       = null
        //      /// StringUtils.remove("", *)         = ""
        //      /// StringUtils.remove("queued", 'u') = "qeed"
        //      /// StringUtils.remove("queued", 'z') = "queued"
        //      /// </pre>
        //      /// </summary>
        //      /// <param name="str">  the source String to search, may be null </param>
        //      /// <param name="remove">  the char to search for and remove, may be null </param>
        //      /// <returns> the substring with the char removed if found,
        //      ///  {@code null} if null String input
        //      /// @since 2.1 </returns>
        //      //JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
        //      //ORIGINAL LINE: public static String remove(final String str, final char remove)
        //      public static string remove(string str, char remove)
        //      {
        //          if (isEmpty(str) || str.IndexOf(remove) == INDEX_NOT_FOUND)
        //          {
        //              return str;
        //          }
        //          //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
        //          //ORIGINAL LINE: final char[] chars = str.toCharArray();
        //          char[] chars = str.ToCharArray();
        //          int pos = 0;
        //          for (int i = 0; i < chars.Length; i++)
        //          {
        //              if (chars[i] != remove)
        //              {
        //                  chars[pos++] = chars[i];
        //              }
        //          }
        //          return new string(chars, 0, pos);
        //      }

        //      /// <summary>
        //      /// <para>Removes each substring of the text String that matches the given regular expression.</para>
        //      /// 
        //      /// This method is a {@code null} safe equivalent to:
        //      /// <ul>
        //      ///  <li>{@code text.replaceAll(regex, StringUtils.EMPTY)}</li>
        //      ///  <li>{@code Pattern.compile(regex).matcher(text).replaceAll(StringUtils.EMPTY)}</li>
        //      /// </ul>
        //      /// 
        //      /// <para>A {@code null} reference passed to this method is a no-op.</para>
        //      /// 
        //      /// <para>Unlike in the <seealso cref="#removePattern(String, String)"/> method, the <seealso cref="Pattern#DOTALL"/> option
        //      /// is NOT automatically added.
        //      /// To use the DOTALL option prepend <code>"(?s)"</code> to the regex.
        //      /// DOTALL is also know as single-line mode in Perl.</para>
        //      /// 
        //      /// <pre>
        //      /// StringUtils.removeAll(null, *)      = null
        //      /// StringUtils.removeAll("any", null)  = "any"
        //      /// StringUtils.removeAll("any", "")    = "any"
        //      /// StringUtils.removeAll("any", ".*")  = ""
        //      /// StringUtils.removeAll("any", ".+")  = ""
        //      /// StringUtils.removeAll("abc", ".?")  = ""
        //      /// StringUtils.removeAll("A&lt;__&gt;\n&lt;__&gt;B", "&lt;.*&gt;")      = "A\nB"
        //      /// StringUtils.removeAll("A&lt;__&gt;\n&lt;__&gt;B", "(?s)&lt;.*&gt;")  = "AB"
        //      /// StringUtils.removeAll("ABCabc123abc", "[a-z]")     = "ABC123"
        //      /// </pre>
        //      /// </summary>
        //      /// <param name="text">  text to remove from, may be null </param>
        //      /// <param name="regex">  the regular expression to which this string is to be matched </param>
        //      /// <returns>  the text with any removes processed,
        //      ///              {@code null} if null String input
        //      /// </returns>
        //      /// <exception cref="java.util.regex.PatternSyntaxException">
        //      ///              if the regular expression's syntax is invalid
        //      /// </exception>
        //      /// <seealso cref= #replaceAll(String, String, String) </seealso>
        //      /// <seealso cref= #removePattern(String, String) </seealso>
        //      /// <seealso cref= String#replaceAll(String, String) </seealso>
        //      /// <seealso cref= java.util.regex.Pattern </seealso>
        //      /// <seealso cref= java.util.regex.Pattern#DOTALL
        //      /// @since 3.5 </seealso>
        //      //JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
        //      //ORIGINAL LINE: public static String removeAll(final String text, final String regex)
        //      public static string removeAll(string text, string regex)
        //      {
        //          return replaceAll(text, regex, StringUtils.EMPTY);
        //      }

        //      /// <summary>
        //      /// <para>Removes the first substring of the text string that matches the given regular expression.</para>
        //      /// 
        //      /// This method is a {@code null} safe equivalent to:
        //      /// <ul>
        //      ///  <li>{@code text.replaceFirst(regex, StringUtils.EMPTY)}</li>
        //      ///  <li>{@code Pattern.compile(regex).matcher(text).replaceFirst(StringUtils.EMPTY)}</li>
        //      /// </ul>
        //      /// 
        //      /// <para>A {@code null} reference passed to this method is a no-op.</para>
        //      /// 
        //      /// <para>The <seealso cref="Pattern#DOTALL"/> option is NOT automatically added.
        //      /// To use the DOTALL option prepend <code>"(?s)"</code> to the regex.
        //      /// DOTALL is also know as single-line mode in Perl.</para>
        //      /// 
        //      /// <pre>
        //      /// StringUtils.removeFirst(null, *)      = null
        //      /// StringUtils.removeFirst("any", null)  = "any"
        //      /// StringUtils.removeFirst("any", "")    = "any"
        //      /// StringUtils.removeFirst("any", ".*")  = ""
        //      /// StringUtils.removeFirst("any", ".+")  = ""
        //      /// StringUtils.removeFirst("abc", ".?")  = "bc"
        //      /// StringUtils.removeFirst("A&lt;__&gt;\n&lt;__&gt;B", "&lt;.*&gt;")      = "A\n&lt;__&gt;B"
        //      /// StringUtils.removeFirst("A&lt;__&gt;\n&lt;__&gt;B", "(?s)&lt;.*&gt;")  = "AB"
        //      /// StringUtils.removeFirst("ABCabc123", "[a-z]")          = "ABCbc123"
        //      /// StringUtils.removeFirst("ABCabc123abc", "[a-z]+")      = "ABC123abc"
        //      /// </pre>
        //      /// </summary>
        //      /// <param name="text">  text to remove from, may be null </param>
        //      /// <param name="regex">  the regular expression to which this string is to be matched </param>
        //      /// <returns>  the text with the first replacement processed,
        //      ///              {@code null} if null String input
        //      /// </returns>
        //      /// <exception cref="java.util.regex.PatternSyntaxException">
        //      ///              if the regular expression's syntax is invalid
        //      /// </exception>
        //      /// <seealso cref= #replaceFirst(String, String, String) </seealso>
        //      /// <seealso cref= String#replaceFirst(String, String) </seealso>
        //      /// <seealso cref= java.util.regex.Pattern </seealso>
        //      /// <seealso cref= java.util.regex.Pattern#DOTALL
        //      /// @since 3.5 </seealso>
        //      //JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
        //      //ORIGINAL LINE: public static String removeFirst(final String text, final String regex)
        //      public static string removeFirst(string text, string regex)
        //      {
        //          return replaceFirst(text, regex, StringUtils.EMPTY);
        //      }

        //      // Replacing
        //      //-----------------------------------------------------------------------
        //      /// <summary>
        //      /// <para>Replaces a String with another String inside a larger String, once.</para>
        //      /// 
        //      /// <para>A {@code null} reference passed to this method is a no-op.</para>
        //      /// 
        //      /// <pre>
        //      /// StringUtils.replaceOnce(null, *, *)        = null
        //      /// StringUtils.replaceOnce("", *, *)          = ""
        //      /// StringUtils.replaceOnce("any", null, *)    = "any"
        //      /// StringUtils.replaceOnce("any", *, null)    = "any"
        //      /// StringUtils.replaceOnce("any", "", *)      = "any"
        //      /// StringUtils.replaceOnce("aba", "a", null)  = "aba"
        //      /// StringUtils.replaceOnce("aba", "a", "")    = "ba"
        //      /// StringUtils.replaceOnce("aba", "a", "z")   = "zba"
        //      /// </pre>
        //      /// </summary>
        //      /// <seealso cref= #replace(String text, String searchString, String replacement, int max) </seealso>
        //      /// <param name="text">  text to search and replace in, may be null </param>
        //      /// <param name="searchString">  the String to search for, may be null </param>
        //      /// <param name="replacement">  the String to replace with, may be null </param>
        //      /// <returns> the text with any replacements processed,
        //      ///  {@code null} if null String input </returns>
        //      //JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
        //      //ORIGINAL LINE: public static String replaceOnce(final String text, final String searchString, final String replacement)
        //      public static string replaceOnce(string text, string searchString, string replacement)
        //      {
        //          return replace(text, searchString, replacement, 1);
        //      }

        //      /// <summary>
        //      /// <para>Case insensitively replaces a String with another String inside a larger String, once.</para>
        //      /// 
        //      /// <para>A {@code null} reference passed to this method is a no-op.</para>
        //      /// 
        //      /// <pre>
        //      /// StringUtils.replaceOnceIgnoreCase(null, *, *)        = null
        //      /// StringUtils.replaceOnceIgnoreCase("", *, *)          = ""
        //      /// StringUtils.replaceOnceIgnoreCase("any", null, *)    = "any"
        //      /// StringUtils.replaceOnceIgnoreCase("any", *, null)    = "any"
        //      /// StringUtils.replaceOnceIgnoreCase("any", "", *)      = "any"
        //      /// StringUtils.replaceOnceIgnoreCase("aba", "a", null)  = "aba"
        //      /// StringUtils.replaceOnceIgnoreCase("aba", "a", "")    = "ba"
        //      /// StringUtils.replaceOnceIgnoreCase("aba", "a", "z")   = "zba"
        //      /// StringUtils.replaceOnceIgnoreCase("FoOFoofoo", "foo", "") = "Foofoo"
        //      /// </pre>
        //      /// </summary>
        //      /// <seealso cref= #replaceIgnoreCase(String text, String searchString, String replacement, int max) </seealso>
        //      /// <param name="text">  text to search and replace in, may be null </param>
        //      /// <param name="searchString">  the String to search for (case insensitive), may be null </param>
        //      /// <param name="replacement">  the String to replace with, may be null </param>
        //      /// <returns> the text with any replacements processed,
        //      ///  {@code null} if null String input
        //      /// @since 3.5 </returns>
        //      //JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
        //      //ORIGINAL LINE: public static String replaceOnceIgnoreCase(final String text, final String searchString, final String replacement)
        //      public static string replaceOnceIgnoreCase(string text, string searchString, string replacement)
        //      {
        //          return replaceIgnoreCase(text, searchString, replacement, 1);
        //      }

        //      /// <summary>
        //      /// <para>Replaces each substring of the source String that matches the given regular expression with the given
        //      /// replacement using the <seealso cref="Pattern#DOTALL"/> option. DOTALL is also know as single-line mode in Perl.</para>
        //      /// 
        //      /// This call is a {@code null} safe equivalent to:
        //      /// <ul>
        //      /// <li>{@code source.replaceAll(&quot;(?s)&quot; + regex, replacement)}</li>
        //      /// <li>{@code Pattern.compile(regex, Pattern.DOTALL).matcher(source).replaceAll(replacement)}</li>
        //      /// </ul>
        //      /// 
        //      /// <para>A {@code null} reference passed to this method is a no-op.</para>
        //      /// 
        //      /// <pre>
        //      /// StringUtils.replacePattern(null, *, *)       = null
        //      /// StringUtils.replacePattern("any", null, *)   = "any"
        //      /// StringUtils.replacePattern("any", *, null)   = "any"
        //      /// StringUtils.replacePattern("", "", "zzz")    = "zzz"
        //      /// StringUtils.replacePattern("", ".*", "zzz")  = "zzz"
        //      /// StringUtils.replacePattern("", ".+", "zzz")  = ""
        //      /// StringUtils.replacePattern("&lt;__&gt;\n&lt;__&gt;", "&lt;.*&gt;", "z")       = "z"
        //      /// StringUtils.replacePattern("ABCabc123", "[a-z]", "_")       = "ABC___123"
        //      /// StringUtils.replacePattern("ABCabc123", "[^A-Z0-9]+", "_")  = "ABC_123"
        //      /// StringUtils.replacePattern("ABCabc123", "[^A-Z0-9]+", "")   = "ABC123"
        //      /// StringUtils.replacePattern("Lorem ipsum  dolor   sit", "( +)([a-z]+)", "_$2")  = "Lorem_ipsum_dolor_sit"
        //      /// </pre>
        //      /// </summary>
        //      /// <param name="source">
        //      ///            the source string </param>
        //      /// <param name="regex">
        //      ///            the regular expression to which this string is to be matched </param>
        //      /// <param name="replacement">
        //      ///            the string to be substituted for each match </param>
        //      /// <returns> The resulting {@code String} </returns>
        //      /// <seealso cref= #replaceAll(String, String, String) </seealso>
        //      /// <seealso cref= String#replaceAll(String, String) </seealso>
        //      /// <seealso cref= Pattern#DOTALL
        //      /// @since 3.2
        //      /// @since 3.5 Changed {@code null} reference passed to this method is a no-op. </seealso>
        //      //JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
        //      //ORIGINAL LINE: public static String replacePattern(final String source, final String regex, final String replacement)
        //      public static string replacePattern(string source, string regex, string replacement)
        //      {
        //          if (string.ReferenceEquals(source, null) || string.ReferenceEquals(regex, null) || string.ReferenceEquals(replacement, null))
        //          {
        //              return source;
        //          }
        //          return Pattern.compile(regex, Pattern.DOTALL).matcher(source).replaceAll(replacement);
        //      }

        //      /// <summary>
        //      /// <para>Removes each substring of the source String that matches the given regular expression using the DOTALL option.
        //      /// </para>
        //      /// 
        //      /// This call is a {@code null} safe equivalent to:
        //      /// <ul>
        //      /// <li>{@code source.replaceAll(&quot;(?s)&quot; + regex, StringUtils.EMPTY)}</li>
        //      /// <li>{@code Pattern.compile(regex, Pattern.DOTALL).matcher(source).replaceAll(StringUtils.EMPTY)}</li>
        //      /// </ul>
        //      /// 
        //      /// <para>A {@code null} reference passed to this method is a no-op.</para>
        //      /// 
        //      /// <pre>
        //      /// StringUtils.removePattern(null, *)       = null
        //      /// StringUtils.removePattern("any", null)   = "any"
        //      /// StringUtils.removePattern("A&lt;__&gt;\n&lt;__&gt;B", "&lt;.*&gt;")  = "AB"
        //      /// StringUtils.removePattern("ABCabc123", "[a-z]")    = "ABC123"
        //      /// </pre>
        //      /// </summary>
        //      /// <param name="source">
        //      ///            the source string </param>
        //      /// <param name="regex">
        //      ///            the regular expression to which this string is to be matched </param>
        //      /// <returns> The resulting {@code String} </returns>
        //      /// <seealso cref= #replacePattern(String, String, String) </seealso>
        //      /// <seealso cref= String#replaceAll(String, String) </seealso>
        //      /// <seealso cref= Pattern#DOTALL
        //      /// @since 3.2
        //      /// @since 3.5 Changed {@code null} reference passed to this method is a no-op. </seealso>
        //      //JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
        //      //ORIGINAL LINE: public static String removePattern(final String source, final String regex)
        //      public static string removePattern(string source, string regex)
        //      {
        //          return replacePattern(source, regex, StringUtils.EMPTY);
        //      }

        //      /// <summary>
        //      /// <para>Replaces each substring of the text String that matches the given regular expression
        //      /// with the given replacement.</para>
        //      /// 
        //      /// This method is a {@code null} safe equivalent to:
        //      /// <ul>
        //      ///  <li>{@code text.replaceAll(regex, replacement)}</li>
        //      ///  <li>{@code Pattern.compile(regex).matcher(text).replaceAll(replacement)}</li>
        //      /// </ul>
        //      /// 
        //      /// <para>A {@code null} reference passed to this method is a no-op.</para>
        //      /// 
        //      /// <para>Unlike in the <seealso cref="#replacePattern(String, String, String)"/> method, the <seealso cref="Pattern#DOTALL"/> option
        //      /// is NOT automatically added.
        //      /// To use the DOTALL option prepend <code>"(?s)"</code> to the regex.
        //      /// DOTALL is also know as single-line mode in Perl.</para>
        //      /// 
        //      /// <pre>
        //      /// StringUtils.replaceAll(null, *, *)       = null
        //      /// StringUtils.replaceAll("any", null, *)   = "any"
        //      /// StringUtils.replaceAll("any", *, null)   = "any"
        //      /// StringUtils.replaceAll("", "", "zzz")    = "zzz"
        //      /// StringUtils.replaceAll("", ".*", "zzz")  = "zzz"
        //      /// StringUtils.replaceAll("", ".+", "zzz")  = ""
        //      /// StringUtils.replaceAll("abc", "", "ZZ")  = "ZZaZZbZZcZZ"
        //      /// StringUtils.replaceAll("&lt;__&gt;\n&lt;__&gt;", "&lt;.*&gt;", "z")      = "z\nz"
        //      /// StringUtils.replaceAll("&lt;__&gt;\n&lt;__&gt;", "(?s)&lt;.*&gt;", "z")  = "z"
        //      /// StringUtils.replaceAll("ABCabc123", "[a-z]", "_")       = "ABC___123"
        //      /// StringUtils.replaceAll("ABCabc123", "[^A-Z0-9]+", "_")  = "ABC_123"
        //      /// StringUtils.replaceAll("ABCabc123", "[^A-Z0-9]+", "")   = "ABC123"
        //      /// StringUtils.replaceAll("Lorem ipsum  dolor   sit", "( +)([a-z]+)", "_$2")  = "Lorem_ipsum_dolor_sit"
        //      /// </pre>
        //      /// </summary>
        //      /// <param name="text">  text to search and replace in, may be null </param>
        //      /// <param name="regex">  the regular expression to which this string is to be matched </param>
        //      /// <param name="replacement">  the string to be substituted for each match </param>
        //      /// <returns>  the text with any replacements processed,
        //      ///              {@code null} if null String input
        //      /// </returns>
        //      /// <exception cref="java.util.regex.PatternSyntaxException">
        //      ///              if the regular expression's syntax is invalid
        //      /// </exception>
        //      /// <seealso cref= #replacePattern(String, String, String) </seealso>
        //      /// <seealso cref= String#replaceAll(String, String) </seealso>
        //      /// <seealso cref= java.util.regex.Pattern </seealso>
        //      /// <seealso cref= java.util.regex.Pattern#DOTALL
        //      /// @since 3.5 </seealso>
        //      //JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
        //      //ORIGINAL LINE: public static String replaceAll(final String text, final String regex, final String replacement)
        //      public static string replaceAll(string text, string regex, string replacement)
        //      {
        //          if (string.ReferenceEquals(text, null) || string.ReferenceEquals(regex, null) || string.ReferenceEquals(replacement, null))
        //          {
        //              return text;
        //          }
        //          return text.replaceAll(regex, replacement);
        //      }

        //      /// <summary>
        //      /// <para>Replaces the first substring of the text string that matches the given regular expression
        //      /// with the given replacement.</para>
        //      /// 
        //      /// This method is a {@code null} safe equivalent to:
        //      /// <ul>
        //      ///  <li>{@code text.replaceFirst(regex, replacement)}</li>
        //      ///  <li>{@code Pattern.compile(regex).matcher(text).replaceFirst(replacement)}</li>
        //      /// </ul>
        //      /// 
        //      /// <para>A {@code null} reference passed to this method is a no-op.</para>
        //      /// 
        //      /// <para>The <seealso cref="Pattern#DOTALL"/> option is NOT automatically added.
        //      /// To use the DOTALL option prepend <code>"(?s)"</code> to the regex.
        //      /// DOTALL is also know as single-line mode in Perl.</para>
        //      /// 
        //      /// <pre>
        //      /// StringUtils.replaceFirst(null, *, *)       = null
        //      /// StringUtils.replaceFirst("any", null, *)   = "any"
        //      /// StringUtils.replaceFirst("any", *, null)   = "any"
        //      /// StringUtils.replaceFirst("", "", "zzz")    = "zzz"
        //      /// StringUtils.replaceFirst("", ".*", "zzz")  = "zzz"
        //      /// StringUtils.replaceFirst("", ".+", "zzz")  = ""
        //      /// StringUtils.replaceFirst("abc", "", "ZZ")  = "ZZabc"
        //      /// StringUtils.replaceFirst("&lt;__&gt;\n&lt;__&gt;", "&lt;.*&gt;", "z")      = "z\n&lt;__&gt;"
        //      /// StringUtils.replaceFirst("&lt;__&gt;\n&lt;__&gt;", "(?s)&lt;.*&gt;", "z")  = "z"
        //      /// StringUtils.replaceFirst("ABCabc123", "[a-z]", "_")          = "ABC_bc123"
        //      /// StringUtils.replaceFirst("ABCabc123abc", "[^A-Z0-9]+", "_")  = "ABC_123abc"
        //      /// StringUtils.replaceFirst("ABCabc123abc", "[^A-Z0-9]+", "")   = "ABC123abc"
        //      /// StringUtils.replaceFirst("Lorem ipsum  dolor   sit", "( +)([a-z]+)", "_$2")  = "Lorem_ipsum  dolor   sit"
        //      /// </pre>
        //      /// </summary>
        //      /// <param name="text">  text to search and replace in, may be null </param>
        //      /// <param name="regex">  the regular expression to which this string is to be matched </param>
        //      /// <param name="replacement">  the string to be substituted for the first match </param>
        //      /// <returns>  the text with the first replacement processed,
        //      ///              {@code null} if null String input
        //      /// </returns>
        //      /// <exception cref="java.util.regex.PatternSyntaxException">
        //      ///              if the regular expression's syntax is invalid
        //      /// </exception>
        //      /// <seealso cref= String#replaceFirst(String, String) </seealso>
        //      /// <seealso cref= java.util.regex.Pattern </seealso>
        //      /// <seealso cref= java.util.regex.Pattern#DOTALL
        //      /// @since 3.5 </seealso>
        //      //JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
        //      //ORIGINAL LINE: public static String replaceFirst(final String text, final String regex, final String replacement)
        //      public static string replaceFirst(string text, string regex, string replacement)
        //      {
        //          if (string.ReferenceEquals(text, null) || string.ReferenceEquals(regex, null) || string.ReferenceEquals(replacement, null))
        //          {
        //              return text;
        //          }
        //          return text.replaceFirst(regex, replacement);
        //      }

        //      /// <summary>
        //      /// <para>Replaces all occurrences of a String within another String.</para>
        //      /// 
        //      /// <para>A {@code null} reference passed to this method is a no-op.</para>
        //      /// 
        //      /// <pre>
        //      /// StringUtils.replace(null, *, *)        = null
        //      /// StringUtils.replace("", *, *)          = ""
        //      /// StringUtils.replace("any", null, *)    = "any"
        //      /// StringUtils.replace("any", *, null)    = "any"
        //      /// StringUtils.replace("any", "", *)      = "any"
        //      /// StringUtils.replace("aba", "a", null)  = "aba"
        //      /// StringUtils.replace("aba", "a", "")    = "b"
        //      /// StringUtils.replace("aba", "a", "z")   = "zbz"
        //      /// </pre>
        //      /// </summary>
        //      /// <seealso cref= #replace(String text, String searchString, String replacement, int max) </seealso>
        //      /// <param name="text">  text to search and replace in, may be null </param>
        //      /// <param name="searchString">  the String to search for, may be null </param>
        //      /// <param name="replacement">  the String to replace it with, may be null </param>
        //      /// <returns> the text with any replacements processed,
        //      ///  {@code null} if null String input </returns>
        //      //JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
        //      //ORIGINAL LINE: public static String replace(final String text, final String searchString, final String replacement)
        //      public static string replace(string text, string searchString, string replacement)
        //      {
        //          return replace(text, searchString, replacement, -1);
        //      }

        //      /// <summary>
        //      /// <para>Case insensitively replaces all occurrences of a String within another String.</para>
        //      /// 
        //      /// <para>A {@code null} reference passed to this method is a no-op.</para>
        //      /// 
        //      /// <pre>
        //      /// StringUtils.replaceIgnoreCase(null, *, *)        = null
        //      /// StringUtils.replaceIgnoreCase("", *, *)          = ""
        //      /// StringUtils.replaceIgnoreCase("any", null, *)    = "any"
        //      /// StringUtils.replaceIgnoreCase("any", *, null)    = "any"
        //      /// StringUtils.replaceIgnoreCase("any", "", *)      = "any"
        //      /// StringUtils.replaceIgnoreCase("aba", "a", null)  = "aba"
        //      /// StringUtils.replaceIgnoreCase("abA", "A", "")    = "b"
        //      /// StringUtils.replaceIgnoreCase("aba", "A", "z")   = "zbz"
        //      /// </pre>
        //      /// </summary>
        //      /// <seealso cref= #replaceIgnoreCase(String text, String searchString, String replacement, int max) </seealso>
        //      /// <param name="text">  text to search and replace in, may be null </param>
        //      /// <param name="searchString">  the String to search for (case insensitive), may be null </param>
        //      /// <param name="replacement">  the String to replace it with, may be null </param>
        //      /// <returns> the text with any replacements processed,
        //      ///  {@code null} if null String input
        //      /// @since 3.5 </returns>
        //      //JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
        //      //ORIGINAL LINE: public static String replaceIgnoreCase(final String text, final String searchString, final String replacement)
        //      public static string replaceIgnoreCase(string text, string searchString, string replacement)
        //      {
        //          return replaceIgnoreCase(text, searchString, replacement, -1);
        //      }

        //      /// <summary>
        //      /// <para>Replaces a String with another String inside a larger String,
        //      /// for the first {@code max} values of the search String.</para>
        //      /// 
        //      /// <para>A {@code null} reference passed to this method is a no-op.</para>
        //      /// 
        //      /// <pre>
        //      /// StringUtils.replace(null, *, *, *)         = null
        //      /// StringUtils.replace("", *, *, *)           = ""
        //      /// StringUtils.replace("any", null, *, *)     = "any"
        //      /// StringUtils.replace("any", *, null, *)     = "any"
        //      /// StringUtils.replace("any", "", *, *)       = "any"
        //      /// StringUtils.replace("any", *, *, 0)        = "any"
        //      /// StringUtils.replace("abaa", "a", null, -1) = "abaa"
        //      /// StringUtils.replace("abaa", "a", "", -1)   = "b"
        //      /// StringUtils.replace("abaa", "a", "z", 0)   = "abaa"
        //      /// StringUtils.replace("abaa", "a", "z", 1)   = "zbaa"
        //      /// StringUtils.replace("abaa", "a", "z", 2)   = "zbza"
        //      /// StringUtils.replace("abaa", "a", "z", -1)  = "zbzz"
        //      /// </pre>
        //      /// </summary>
        //      /// <param name="text">  text to search and replace in, may be null </param>
        //      /// <param name="searchString">  the String to search for, may be null </param>
        //      /// <param name="replacement">  the String to replace it with, may be null </param>
        //      /// <param name="max">  maximum number of values to replace, or {@code -1} if no maximum </param>
        //      /// <returns> the text with any replacements processed,
        //      ///  {@code null} if null String input </returns>
        //      //JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
        //      //ORIGINAL LINE: public static String replace(final String text, final String searchString, final String replacement, final int max)
        //      public static string replace(string text, string searchString, string replacement, int max)
        //      {
        //          return replace(text, searchString, replacement, max, false);
        //      }

        //      /// <summary>
        //      /// <para>Replaces a String with another String inside a larger String,
        //      /// for the first {@code max} values of the search String, 
        //      /// case sensitively/insensisitively based on {@code ignoreCase} value.</para>
        //      /// 
        //      /// <para>A {@code null} reference passed to this method is a no-op.</para>
        //      /// 
        //      /// <pre>
        //      /// StringUtils.replace(null, *, *, *, false)         = null
        //      /// StringUtils.replace("", *, *, *, false)           = ""
        //      /// StringUtils.replace("any", null, *, *, false)     = "any"
        //      /// StringUtils.replace("any", *, null, *, false)     = "any"
        //      /// StringUtils.replace("any", "", *, *, false)       = "any"
        //      /// StringUtils.replace("any", *, *, 0, false)        = "any"
        //      /// StringUtils.replace("abaa", "a", null, -1, false) = "abaa"
        //      /// StringUtils.replace("abaa", "a", "", -1, false)   = "b"
        //      /// StringUtils.replace("abaa", "a", "z", 0, false)   = "abaa"
        //      /// StringUtils.replace("abaa", "A", "z", 1, false)   = "abaa"
        //      /// StringUtils.replace("abaa", "A", "z", 1, true)   = "zbaa"
        //      /// StringUtils.replace("abAa", "a", "z", 2, true)   = "zbza"
        //      /// StringUtils.replace("abAa", "a", "z", -1, true)  = "zbzz"
        //      /// </pre>
        //      /// </summary>
        //      /// <param name="text">  text to search and replace in, may be null </param>
        //      /// <param name="searchString">  the String to search for (case insensitive), may be null </param>
        //      /// <param name="replacement">  the String to replace it with, may be null </param>
        //      /// <param name="max">  maximum number of values to replace, or {@code -1} if no maximum </param>
        //      /// <param name="ignoreCase"> if true replace is case insensitive, otherwise case sensitive </param>
        //      /// <returns> the text with any replacements processed,
        //      ///  {@code null} if null String input </returns>
        //      //JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
        //      //ORIGINAL LINE: private static String replace(final String text, String searchString, final String replacement, int max, final boolean ignoreCase)
        //      private static string replace(string text, string searchString, string replacement, int max, bool ignoreCase)
        //      {
        //          if (isEmpty(text) || isEmpty(searchString) || string.ReferenceEquals(replacement, null) || max == 0)
        //          {
        //              return text;
        //          }
        //          string searchText = text;
        //          if (ignoreCase)
        //          {
        //              searchText = text.ToLower();
        //              searchString = searchString.ToLower();
        //          }
        //          int start = 0;
        //          int end = searchText.IndexOf(searchString, start, StringComparison.Ordinal);
        //          if (end == INDEX_NOT_FOUND)
        //          {
        //              return text;
        //          }
        //          //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
        //          //ORIGINAL LINE: final int replLength = searchString.length();
        //          int replLength = searchString.Length;
        //          int increase = replacement.Length - replLength;
        //          increase = increase < 0 ? 0 : increase;
        //          increase *= max < 0 ? 16 : max > 64 ? 64 : max;
        //          //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
        //          //ORIGINAL LINE: final StringBuilder buf = new StringBuilder(text.length() + increase);
        //          StringBuilder buf = new StringBuilder(text.Length + increase);
        //          while (end != INDEX_NOT_FOUND)
        //          {
        //              buf.Append(text.Substring(start, end - start)).Append(replacement);
        //              start = end + replLength;
        //              if (--max == 0)
        //              {
        //                  break;
        //              }
        //              end = searchText.IndexOf(searchString, start, StringComparison.Ordinal);
        //          }
        //          buf.Append(text.Substring(start));
        //          return buf.ToString();
        //      }

        //      /// <summary>
        //      /// <para>Case insensitively replaces a String with another String inside a larger String,
        //      /// for the first {@code max} values of the search String.</para>
        //      /// 
        //      /// <para>A {@code null} reference passed to this method is a no-op.</para>
        //      /// 
        //      /// <pre>
        //      /// StringUtils.replaceIgnoreCase(null, *, *, *)         = null
        //      /// StringUtils.replaceIgnoreCase("", *, *, *)           = ""
        //      /// StringUtils.replaceIgnoreCase("any", null, *, *)     = "any"
        //      /// StringUtils.replaceIgnoreCase("any", *, null, *)     = "any"
        //      /// StringUtils.replaceIgnoreCase("any", "", *, *)       = "any"
        //      /// StringUtils.replaceIgnoreCase("any", *, *, 0)        = "any"
        //      /// StringUtils.replaceIgnoreCase("abaa", "a", null, -1) = "abaa"
        //      /// StringUtils.replaceIgnoreCase("abaa", "a", "", -1)   = "b"
        //      /// StringUtils.replaceIgnoreCase("abaa", "a", "z", 0)   = "abaa"
        //      /// StringUtils.replaceIgnoreCase("abaa", "A", "z", 1)   = "zbaa"
        //      /// StringUtils.replaceIgnoreCase("abAa", "a", "z", 2)   = "zbza"
        //      /// StringUtils.replaceIgnoreCase("abAa", "a", "z", -1)  = "zbzz"
        //      /// </pre>
        //      /// </summary>
        //      /// <param name="text">  text to search and replace in, may be null </param>
        //      /// <param name="searchString">  the String to search for (case insensitive), may be null </param>
        //      /// <param name="replacement">  the String to replace it with, may be null </param>
        //      /// <param name="max">  maximum number of values to replace, or {@code -1} if no maximum </param>
        //      /// <returns> the text with any replacements processed,
        //      ///  {@code null} if null String input
        //      /// @since 3.5 </returns>
        //      //JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
        //      //ORIGINAL LINE: public static String replaceIgnoreCase(final String text, final String searchString, final String replacement, final int max)
        //      public static string replaceIgnoreCase(string text, string searchString, string replacement, int max)
        //      {
        //          return replace(text, searchString, replacement, max, true);
        //      }

        //      /// <summary>
        //      /// <para>
        //      /// Replaces all occurrences of Strings within another String.
        //      /// </para>
        //      /// 
        //      /// <para>
        //      /// A {@code null} reference passed to this method is a no-op, or if
        //      /// any "search string" or "string to replace" is null, that replace will be
        //      /// ignored. This will not repeat. For repeating replaces, call the
        //      /// overloaded method.
        //      /// </para>
        //      /// 
        //      /// <pre>
        //      ///  StringUtils.replaceEach(null, *, *)        = null
        //      ///  StringUtils.replaceEach("", *, *)          = ""
        //      ///  StringUtils.replaceEach("aba", null, null) = "aba"
        //      ///  StringUtils.replaceEach("aba", new String[0], null) = "aba"
        //      ///  StringUtils.replaceEach("aba", null, new String[0]) = "aba"
        //      ///  StringUtils.replaceEach("aba", new String[]{"a"}, null)  = "aba"
        //      ///  StringUtils.replaceEach("aba", new String[]{"a"}, new String[]{""})  = "b"
        //      ///  StringUtils.replaceEach("aba", new String[]{null}, new String[]{"a"})  = "aba"
        //      ///  StringUtils.replaceEach("abcde", new String[]{"ab", "d"}, new String[]{"w", "t"})  = "wcte"
        //      ///  (example of how it does not repeat)
        //      ///  StringUtils.replaceEach("abcde", new String[]{"ab", "d"}, new String[]{"d", "t"})  = "dcte"
        //      /// </pre>
        //      /// </summary>
        //      /// <param name="text">
        //      ///            text to search and replace in, no-op if null </param>
        //      /// <param name="searchList">
        //      ///            the Strings to search for, no-op if null </param>
        //      /// <param name="replacementList">
        //      ///            the Strings to replace them with, no-op if null </param>
        //      /// <returns> the text with any replacements processed, {@code null} if
        //      ///         null String input </returns>
        //      /// <exception cref="IllegalArgumentException">
        //      ///             if the lengths of the arrays are not the same (null is ok,
        //      ///             and/or size 0)
        //      /// @since 2.4 </exception>
        //      //JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
        //      //ORIGINAL LINE: public static String replaceEach(final String text, final String[] searchList, final String[] replacementList)
        //      public static string replaceEach(string text, string[] searchList, string[] replacementList)
        //      {
        //          return replaceEach(text, searchList, replacementList, false, 0);
        //      }

        //      /// <summary>
        //      /// <para>
        //      /// Replaces all occurrences of Strings within another String.
        //      /// </para>
        //      /// 
        //      /// <para>
        //      /// A {@code null} reference passed to this method is a no-op, or if
        //      /// any "search string" or "string to replace" is null, that replace will be
        //      /// ignored.
        //      /// </para>
        //      /// 
        //      /// <pre>
        //      ///  StringUtils.replaceEachRepeatedly(null, *, *) = null
        //      ///  StringUtils.replaceEachRepeatedly("", *, *) = ""
        //      ///  StringUtils.replaceEachRepeatedly("aba", null, null) = "aba"
        //      ///  StringUtils.replaceEachRepeatedly("aba", new String[0], null) = "aba"
        //      ///  StringUtils.replaceEachRepeatedly("aba", null, new String[0]) = "aba"
        //      ///  StringUtils.replaceEachRepeatedly("aba", new String[]{"a"}, null) = "aba"
        //      ///  StringUtils.replaceEachRepeatedly("aba", new String[]{"a"}, new String[]{""}) = "b"
        //      ///  StringUtils.replaceEachRepeatedly("aba", new String[]{null}, new String[]{"a"}) = "aba"
        //      ///  StringUtils.replaceEachRepeatedly("abcde", new String[]{"ab", "d"}, new String[]{"w", "t"}) = "wcte"
        //      ///  (example of how it repeats)
        //      ///  StringUtils.replaceEachRepeatedly("abcde", new String[]{"ab", "d"}, new String[]{"d", "t"}) = "tcte"
        //      ///  StringUtils.replaceEachRepeatedly("abcde", new String[]{"ab", "d"}, new String[]{"d", "ab"}) = IllegalStateException
        //      /// </pre>
        //      /// </summary>
        //      /// <param name="text">
        //      ///            text to search and replace in, no-op if null </param>
        //      /// <param name="searchList">
        //      ///            the Strings to search for, no-op if null </param>
        //      /// <param name="replacementList">
        //      ///            the Strings to replace them with, no-op if null </param>
        //      /// <returns> the text with any replacements processed, {@code null} if
        //      ///         null String input </returns>
        //      /// <exception cref="IllegalStateException">
        //      ///             if the search is repeating and there is an endless loop due
        //      ///             to outputs of one being inputs to another </exception>
        //      /// <exception cref="IllegalArgumentException">
        //      ///             if the lengths of the arrays are not the same (null is ok,
        //      ///             and/or size 0)
        //      /// @since 2.4 </exception>
        //      //JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
        //      //ORIGINAL LINE: public static String replaceEachRepeatedly(final String text, final String[] searchList, final String[] replacementList)
        //      public static string replaceEachRepeatedly(string text, string[] searchList, string[] replacementList)
        //      {
        //          // timeToLive should be 0 if not used or nothing to replace, else it's
        //          // the length of the replace array
        //          //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
        //          //ORIGINAL LINE: final int timeToLive = searchList == null ? 0 : searchList.length;
        //          int timeToLive = searchList == null ? 0 : searchList.Length;
        //          return replaceEach(text, searchList, replacementList, true, timeToLive);
        //      }

        //      /// <summary>
        //      /// <para>
        //      /// Replace all occurrences of Strings within another String.
        //      /// This is a private recursive helper method for <seealso cref="#replaceEachRepeatedly(String, String[], String[])"/> and
        //      /// <seealso cref="#replaceEach(String, String[], String[])"/>
        //      /// </para>
        //      /// 
        //      /// <para>
        //      /// A {@code null} reference passed to this method is a no-op, or if
        //      /// any "search string" or "string to replace" is null, that replace will be
        //      /// ignored.
        //      /// </para>
        //      /// 
        //      /// <pre>
        //      ///  StringUtils.replaceEach(null, *, *, *, *) = null
        //      ///  StringUtils.replaceEach("", *, *, *, *) = ""
        //      ///  StringUtils.replaceEach("aba", null, null, *, *) = "aba"
        //      ///  StringUtils.replaceEach("aba", new String[0], null, *, *) = "aba"
        //      ///  StringUtils.replaceEach("aba", null, new String[0], *, *) = "aba"
        //      ///  StringUtils.replaceEach("aba", new String[]{"a"}, null, *, *) = "aba"
        //      ///  StringUtils.replaceEach("aba", new String[]{"a"}, new String[]{""}, *, >=0) = "b"
        //      ///  StringUtils.replaceEach("aba", new String[]{null}, new String[]{"a"}, *, >=0) = "aba"
        //      ///  StringUtils.replaceEach("abcde", new String[]{"ab", "d"}, new String[]{"w", "t"}, *, >=0) = "wcte"
        //      ///  (example of how it repeats)
        //      ///  StringUtils.replaceEach("abcde", new String[]{"ab", "d"}, new String[]{"d", "t"}, false, >=0) = "dcte"
        //      ///  StringUtils.replaceEach("abcde", new String[]{"ab", "d"}, new String[]{"d", "t"}, true, >=2) = "tcte"
        //      ///  StringUtils.replaceEach("abcde", new String[]{"ab", "d"}, new String[]{"d", "ab"}, *, *) = IllegalStateException
        //      /// </pre>
        //      /// </summary>
        //      /// <param name="text">
        //      ///            text to search and replace in, no-op if null </param>
        //      /// <param name="searchList">
        //      ///            the Strings to search for, no-op if null </param>
        //      /// <param name="replacementList">
        //      ///            the Strings to replace them with, no-op if null </param>
        //      /// <param name="repeat"> if true, then replace repeatedly
        //      ///       until there are no more possible replacements or timeToLive < 0 </param>
        //      /// <param name="timeToLive">
        //      ///            if less than 0 then there is a circular reference and endless
        //      ///            loop </param>
        //      /// <returns> the text with any replacements processed, {@code null} if
        //      ///         null String input </returns>
        //      /// <exception cref="IllegalStateException">
        //      ///             if the search is repeating and there is an endless loop due
        //      ///             to outputs of one being inputs to another </exception>
        //      /// <exception cref="IllegalArgumentException">
        //      ///             if the lengths of the arrays are not the same (null is ok,
        //      ///             and/or size 0)
        //      /// @since 2.4 </exception>
        //      //JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
        //      //ORIGINAL LINE: private static String replaceEach(final String text, final String[] searchList, final String[] replacementList, final boolean repeat, final int timeToLive)
        //      private static string replaceEach(string text, string[] searchList, string[] replacementList, bool repeat, int timeToLive)
        //      {

        //          // mchyzer Performance note: This creates very few new objects (one major goal)
        //          // let me know if there are performance requests, we can create a harness to measure

        //          if (string.ReferenceEquals(text, null) || text.Length == 0 || searchList == null || searchList.Length == 0 || replacementList == null || replacementList.Length == 0)
        //          {
        //              return text;
        //          }

        //          // if recursing, this shouldn't be less than 0
        //          if (timeToLive < 0)
        //          {
        //              throw new System.InvalidOperationException("Aborting to protect against StackOverflowError - " + "output of one loop is the input of another");
        //          }

        //          //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
        //          //ORIGINAL LINE: final int searchLength = searchList.length;
        //          int searchLength = searchList.Length;
        //          //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
        //          //ORIGINAL LINE: final int replacementLength = replacementList.length;
        //          int replacementLength = replacementList.Length;

        //          // make sure lengths are ok, these need to be equal
        //          if (searchLength != replacementLength)
        //          {
        //              throw new System.ArgumentException("Search and Replace array lengths don't match: " + searchLength + " vs " + replacementLength);
        //          }

        //          // keep track of which still have matches
        //          //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
        //          //ORIGINAL LINE: final boolean[] noMoreMatchesForReplIndex = new boolean[searchLength];
        //          bool[] noMoreMatchesForReplIndex = new bool[searchLength];

        //          // index on index that the match was found
        //          int textIndex = -1;
        //          int replaceIndex = -1;
        //          int tempIndex = -1;

        //          // index of replace array that will replace the search string found
        //          // NOTE: logic duplicated below START
        //          for (int i = 0; i < searchLength; i++)
        //          {
        //              if (noMoreMatchesForReplIndex[i] || string.ReferenceEquals(searchList[i], null) || searchList[i].Length == 0 || string.ReferenceEquals(replacementList[i], null))
        //              {
        //                  continue;
        //              }
        //              tempIndex = text.IndexOf(searchList[i], StringComparison.Ordinal);

        //              // see if we need to keep searching for this
        //              if (tempIndex == -1)
        //              {
        //                  noMoreMatchesForReplIndex[i] = true;
        //              }
        //              else
        //              {
        //                  if (textIndex == -1 || tempIndex < textIndex)
        //                  {
        //                      textIndex = tempIndex;
        //                      replaceIndex = i;
        //                  }
        //              }
        //          }
        //          // NOTE: logic mostly below END

        //          // no search strings found, we are done
        //          if (textIndex == -1)
        //          {
        //              return text;
        //          }

        //          int start = 0;

        //          // get a good guess on the size of the result buffer so it doesn't have to double if it goes over a bit
        //          int increase = 0;

        //          // count the replacement text elements that are larger than their corresponding text being replaced
        //          for (int i = 0; i < searchList.Length; i++)
        //          {
        //              if (string.ReferenceEquals(searchList[i], null) || string.ReferenceEquals(replacementList[i], null))
        //              {
        //                  continue;
        //              }
        //              //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
        //              //ORIGINAL LINE: final int greater = replacementList[i].length() - searchList[i].length();
        //              int greater = replacementList[i].Length - searchList[i].Length;
        //              if (greater > 0)
        //              {
        //                  increase += 3 * greater; // assume 3 matches
        //              }
        //          }
        //          // have upper-bound at 20% increase, then let Java take over
        //          increase = Math.Min(increase, text.Length / 5);

        //          //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
        //          //ORIGINAL LINE: final StringBuilder buf = new StringBuilder(text.length() + increase);
        //          StringBuilder buf = new StringBuilder(text.Length + increase);

        //          while (textIndex != -1)
        //          {

        //              for (int i = start; i < textIndex; i++)
        //              {
        //                  buf.Append(text[i]);
        //              }
        //              buf.Append(replacementList[replaceIndex]);

        //              start = textIndex + searchList[replaceIndex].Length;

        //              textIndex = -1;
        //              replaceIndex = -1;
        //              tempIndex = -1;
        //              // find the next earliest match
        //              // NOTE: logic mostly duplicated above START
        //              for (int i = 0; i < searchLength; i++)
        //              {
        //                  if (noMoreMatchesForReplIndex[i] || string.ReferenceEquals(searchList[i], null) || searchList[i].Length == 0 || string.ReferenceEquals(replacementList[i], null))
        //                  {
        //                      continue;
        //                  }
        //                  tempIndex = text.IndexOf(searchList[i], start, StringComparison.Ordinal);

        //                  // see if we need to keep searching for this
        //                  if (tempIndex == -1)
        //                  {
        //                      noMoreMatchesForReplIndex[i] = true;
        //                  }
        //                  else
        //                  {
        //                      if (textIndex == -1 || tempIndex < textIndex)
        //                      {
        //                          textIndex = tempIndex;
        //                          replaceIndex = i;
        //                      }
        //                  }
        //              }
        //              // NOTE: logic duplicated above END

        //          }
        //          //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
        //          //ORIGINAL LINE: final int textLength = text.length();
        //          int textLength = text.Length;
        //          for (int i = start; i < textLength; i++)
        //          {
        //              buf.Append(text[i]);
        //          }
        //          //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
        //          //ORIGINAL LINE: final String result = buf.toString();
        //          string result = buf.ToString();
        //          if (!repeat)
        //          {
        //              return result;
        //          }

        //          return replaceEach(result, searchList, replacementList, repeat, timeToLive - 1);
        //      }

        //      // Replace, character based
        //      //-----------------------------------------------------------------------
        //      /// <summary>
        //      /// <para>Replaces all occurrences of a character in a String with another.
        //      /// This is a null-safe version of <seealso cref="String#replace(char, char)"/>.</para>
        //      /// 
        //      /// <para>A {@code null} string input returns {@code null}.
        //      /// An empty ("") string input returns an empty string.</para>
        //      /// 
        //      /// <pre>
        //      /// StringUtils.replaceChars(null, *, *)        = null
        //      /// StringUtils.replaceChars("", *, *)          = ""
        //      /// StringUtils.replaceChars("abcba", 'b', 'y') = "aycya"
        //      /// StringUtils.replaceChars("abcba", 'z', 'y') = "abcba"
        //      /// </pre>
        //      /// </summary>
        //      /// <param name="str">  String to replace characters in, may be null </param>
        //      /// <param name="searchChar">  the character to search for, may be null </param>
        //      /// <param name="replaceChar">  the character to replace, may be null </param>
        //      /// <returns> modified String, {@code null} if null string input
        //      /// @since 2.0 </returns>
        //      //JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
        //      //ORIGINAL LINE: public static String replaceChars(final String str, final char searchChar, final char replaceChar)
        //      public static string replaceChars(string str, char searchChar, char replaceChar)
        //      {
        //          if (string.ReferenceEquals(str, null))
        //          {
        //              return null;
        //          }
        //          return str.Replace(searchChar, replaceChar);
        //      }

        //      /// <summary>
        //      /// <para>Replaces multiple characters in a String in one go.
        //      /// This method can also be used to delete characters.</para>
        //      /// 
        //      /// <para>For example:<br>
        //      /// <code>replaceChars(&quot;hello&quot;, &quot;ho&quot;, &quot;jy&quot;) = jelly</code>.</para>
        //      /// 
        //      /// <para>A {@code null} string input returns {@code null}.
        //      /// An empty ("") string input returns an empty string.
        //      /// A null or empty set of search characters returns the input string.</para>
        //      /// 
        //      /// <para>The length of the search characters should normally equal the length
        //      /// of the replace characters.
        //      /// If the search characters is longer, then the extra search characters
        //      /// are deleted.
        //      /// If the search characters is shorter, then the extra replace characters
        //      /// are ignored.</para>
        //      /// 
        //      /// <pre>
        //      /// StringUtils.replaceChars(null, *, *)           = null
        //      /// StringUtils.replaceChars("", *, *)             = ""
        //      /// StringUtils.replaceChars("abc", null, *)       = "abc"
        //      /// StringUtils.replaceChars("abc", "", *)         = "abc"
        //      /// StringUtils.replaceChars("abc", "b", null)     = "ac"
        //      /// StringUtils.replaceChars("abc", "b", "")       = "ac"
        //      /// StringUtils.replaceChars("abcba", "bc", "yz")  = "ayzya"
        //      /// StringUtils.replaceChars("abcba", "bc", "y")   = "ayya"
        //      /// StringUtils.replaceChars("abcba", "bc", "yzx") = "ayzya"
        //      /// </pre>
        //      /// </summary>
        //      /// <param name="str">  String to replace characters in, may be null </param>
        //      /// <param name="searchChars">  a set of characters to search for, may be null </param>
        //      /// <param name="replaceChars">  a set of characters to replace, may be null </param>
        //      /// <returns> modified String, {@code null} if null string input
        //      /// @since 2.0 </returns>
        //      //JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
        //      //ORIGINAL LINE: public static String replaceChars(final String str, final String searchChars, String replaceChars)
        //      public static string replaceChars(string str, string searchChars, string replaceChars)
        //      {
        //          if (isEmpty(str) || isEmpty(searchChars))
        //          {
        //              return str;
        //          }
        //          if (string.ReferenceEquals(replaceChars, null))
        //          {
        //              replaceChars = EMPTY;
        //          }
        //          bool modified = false;
        //          //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
        //          //ORIGINAL LINE: final int replaceCharsLength = replaceChars.length();
        //          int replaceCharsLength = replaceChars.Length;
        //          //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
        //          //ORIGINAL LINE: final int strLength = str.length();
        //          int strLength = str.Length;
        //          //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
        //          //ORIGINAL LINE: final StringBuilder buf = new StringBuilder(strLength);
        //          StringBuilder buf = new StringBuilder(strLength);
        //          for (int i = 0; i < strLength; i++)
        //          {
        //              //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
        //              //ORIGINAL LINE: final char ch = str.charAt(i);
        //              char ch = str[i];
        //              //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
        //              //ORIGINAL LINE: final int index = searchChars.indexOf(ch);
        //              int index = searchChars.IndexOf(ch);
        //              if (index >= 0)
        //              {
        //                  modified = true;
        //                  if (index < replaceCharsLength)
        //                  {
        //                      buf.Append(replaceChars[index]);
        //                  }
        //              }
        //              else
        //              {
        //                  buf.Append(ch);
        //              }
        //          }
        //          if (modified)
        //          {
        //              return buf.ToString();
        //          }
        //          return str;
        //      }

        //      // Overlay
        //      //-----------------------------------------------------------------------
        //      /// <summary>
        //      /// <para>Overlays part of a String with another String.</para>
        //      /// 
        //      /// <para>A {@code null} string input returns {@code null}.
        //      /// A negative index is treated as zero.
        //      /// An index greater than the string length is treated as the string length.
        //      /// The start index is always the smaller of the two indices.</para>
        //      /// 
        //      /// <pre>
        //      /// StringUtils.overlay(null, *, *, *)            = null
        //      /// StringUtils.overlay("", "abc", 0, 0)          = "abc"
        //      /// StringUtils.overlay("abcdef", null, 2, 4)     = "abef"
        //      /// StringUtils.overlay("abcdef", "", 2, 4)       = "abef"
        //      /// StringUtils.overlay("abcdef", "", 4, 2)       = "abef"
        //      /// StringUtils.overlay("abcdef", "zzzz", 2, 4)   = "abzzzzef"
        //      /// StringUtils.overlay("abcdef", "zzzz", 4, 2)   = "abzzzzef"
        //      /// StringUtils.overlay("abcdef", "zzzz", -1, 4)  = "zzzzef"
        //      /// StringUtils.overlay("abcdef", "zzzz", 2, 8)   = "abzzzz"
        //      /// StringUtils.overlay("abcdef", "zzzz", -2, -3) = "zzzzabcdef"
        //      /// StringUtils.overlay("abcdef", "zzzz", 8, 10)  = "abcdefzzzz"
        //      /// </pre>
        //      /// </summary>
        //      /// <param name="str">  the String to do overlaying in, may be null </param>
        //      /// <param name="overlay">  the String to overlay, may be null </param>
        //      /// <param name="start">  the position to start overlaying at </param>
        //      /// <param name="end">  the position to stop overlaying before </param>
        //      /// <returns> overlayed String, {@code null} if null String input
        //      /// @since 2.0 </returns>
        //      //JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
        //      //ORIGINAL LINE: public static String overlay(final String str, String overlay, int start, int end)
        //      public static string overlay(string str, string overlay, int start, int end)
        //      {
        //          if (string.ReferenceEquals(str, null))
        //          {
        //              return null;
        //          }
        //          if (string.ReferenceEquals(overlay, null))
        //          {
        //              overlay = EMPTY;
        //          }
        //          //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
        //          //ORIGINAL LINE: final int len = str.length();
        //          int len = str.Length;
        //          if (start < 0)
        //          {
        //              start = 0;
        //          }
        //          if (start > len)
        //          {
        //              start = len;
        //          }
        //          if (end < 0)
        //          {
        //              end = 0;
        //          }
        //          if (end > len)
        //          {
        //              end = len;
        //          }
        //          if (start > end)
        //          {
        //              //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
        //              //ORIGINAL LINE: final int temp = start;
        //              int temp = start;
        //              start = end;
        //              end = temp;
        //          }
        //          return (new StringBuilder(len + start - end + overlay.Length + 1)).Append(str.Substring(0, start)).Append(overlay).Append(str.Substring(end)).ToString();
        //      }

        //      // Chomping
        //      //-----------------------------------------------------------------------
        //      /// <summary>
        //      /// <para>Removes one newline from end of a String if it's there,
        //      /// otherwise leave it alone.  A newline is &quot;{@code \n}&quot;,
        //      /// &quot;{@code \r}&quot;, or &quot;{@code \r\n}&quot;.</para>
        //      /// 
        //      /// <para>NOTE: This method changed in 2.0.
        //      /// It now more closely matches Perl chomp.</para>
        //      /// 
        //      /// <pre>
        //      /// StringUtils.chomp(null)          = null
        //      /// StringUtils.chomp("")            = ""
        //      /// StringUtils.chomp("abc \r")      = "abc "
        //      /// StringUtils.chomp("abc\n")       = "abc"
        //      /// StringUtils.chomp("abc\r\n")     = "abc"
        //      /// StringUtils.chomp("abc\r\n\r\n") = "abc\r\n"
        //      /// StringUtils.chomp("abc\n\r")     = "abc\n"
        //      /// StringUtils.chomp("abc\n\rabc")  = "abc\n\rabc"
        //      /// StringUtils.chomp("\r")          = ""
        //      /// StringUtils.chomp("\n")          = ""
        //      /// StringUtils.chomp("\r\n")        = ""
        //      /// </pre>
        //      /// </summary>
        //      /// <param name="str">  the String to chomp a newline from, may be null </param>
        //      /// <returns> String without newline, {@code null} if null String input </returns>
        //      //JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
        //      //ORIGINAL LINE: public static String chomp(final String str)
        //      public static string chomp(string str)
        //      {
        //          if (isEmpty(str))
        //          {
        //              return str;
        //          }

        //          if (str.Length == 1)
        //          {
        //              //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
        //              //ORIGINAL LINE: final char ch = str.charAt(0);
        //              char ch = str[0];
        //              if (ch == CharUtils.CR || ch == CharUtils.LF)
        //              {
        //                  return EMPTY;
        //              }
        //              return str;
        //          }

        //          int lastIdx = str.Length - 1;
        //          //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
        //          //ORIGINAL LINE: final char last = str.charAt(lastIdx);
        //          char last = str[lastIdx];

        //          if (last == CharUtils.LF)
        //          {
        //              if (str[lastIdx - 1] == CharUtils.CR)
        //              {
        //                  lastIdx--;
        //              }
        //          }
        //          else if (last != CharUtils.CR)
        //          {
        //              lastIdx++;
        //          }
        //          return str.Substring(0, lastIdx);
        //      }

        //      /// <summary>
        //      /// <para>Removes {@code separator} from the end of
        //      /// {@code str} if it's there, otherwise leave it alone.</para>
        //      /// 
        //      /// <para>NOTE: This method changed in version 2.0.
        //      /// It now more closely matches Perl chomp.
        //      /// For the previous behavior, use <seealso cref="#substringBeforeLast(String, String)"/>.
        //      /// This method uses <seealso cref="String#endsWith(String)"/>.</para>
        //      /// 
        //      /// <pre>
        //      /// StringUtils.chomp(null, *)         = null
        //      /// StringUtils.chomp("", *)           = ""
        //      /// StringUtils.chomp("foobar", "bar") = "foo"
        //      /// StringUtils.chomp("foobar", "baz") = "foobar"
        //      /// StringUtils.chomp("foo", "foo")    = ""
        //      /// StringUtils.chomp("foo ", "foo")   = "foo "
        //      /// StringUtils.chomp(" foo", "foo")   = " "
        //      /// StringUtils.chomp("foo", "foooo")  = "foo"
        //      /// StringUtils.chomp("foo", "")       = "foo"
        //      /// StringUtils.chomp("foo", null)     = "foo"
        //      /// </pre>
        //      /// </summary>
        //      /// <param name="str">  the String to chomp from, may be null </param>
        //      /// <param name="separator">  separator String, may be null </param>
        //      /// <returns> String without trailing separator, {@code null} if null String input </returns>
        //      /// @deprecated This feature will be removed in Lang 4.0, use <seealso cref="StringUtils#removeEnd(String, String)"/> instead 
        //      //JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
        //      //ORIGINAL LINE: @Deprecated("This feature will be removed in Lang 4.0, use <seealso cref="StringUtils#removeEnd(String, String)"/> instead") public static String chomp(final String str, final String separator)
        //      //[Obsolete("This feature will be removed in Lang 4.0, use <seealso cref="StringUtils#removeEnd(String, String)"/> instead")]
        //public static string chomp(string str, string separator)
        //      {
        //          return removeEnd(str, separator);
        //      }

        //      // Chopping
        //      //-----------------------------------------------------------------------
        //      /// <summary>
        //      /// <para>Remove the last character from a String.</para>
        //      /// 
        //      /// <para>If the String ends in {@code \r\n}, then remove both
        //      /// of them.</para>
        //      /// 
        //      /// <pre>
        //      /// StringUtils.chop(null)          = null
        //      /// StringUtils.chop("")            = ""
        //      /// StringUtils.chop("abc \r")      = "abc "
        //      /// StringUtils.chop("abc\n")       = "abc"
        //      /// StringUtils.chop("abc\r\n")     = "abc"
        //      /// StringUtils.chop("abc")         = "ab"
        //      /// StringUtils.chop("abc\nabc")    = "abc\nab"
        //      /// StringUtils.chop("a")           = ""
        //      /// StringUtils.chop("\r")          = ""
        //      /// StringUtils.chop("\n")          = ""
        //      /// StringUtils.chop("\r\n")        = ""
        //      /// </pre>
        //      /// </summary>
        //      /// <param name="str">  the String to chop last character from, may be null </param>
        //      /// <returns> String without last character, {@code null} if null String input </returns>
        //      //JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
        //      //ORIGINAL LINE: public static String chop(final String str)
        //      public static string chop(string str)
        //      {
        //          if (string.ReferenceEquals(str, null))
        //          {
        //              return null;
        //          }
        //          //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
        //          //ORIGINAL LINE: final int strLen = str.length();
        //          int strLen = str.Length;
        //          if (strLen < 2)
        //          {
        //              return EMPTY;
        //          }
        //          //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
        //          //ORIGINAL LINE: final int lastIdx = strLen - 1;
        //          int lastIdx = strLen - 1;
        //          //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
        //          //ORIGINAL LINE: final String ret = str.substring(0, lastIdx);
        //          string ret = str.Substring(0, lastIdx);
        //          //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
        //          //ORIGINAL LINE: final char last = str.charAt(lastIdx);
        //          char last = str[lastIdx];
        //          if (last == CharUtils.LF && ret[lastIdx - 1] == CharUtils.CR)
        //          {
        //              return ret.Substring(0, lastIdx - 1);
        //          }
        //          return ret;
        //      }

        // Conversion
        //-----------------------------------------------------------------------

        // Padding
        //-----------------------------------------------------------------------
        /// <summary>
        /// <para>Repeat a String {@code repeat} times to form a
        /// new String.</para>
        /// 
        /// <pre>
        /// StringUtils.repeat(null, 2) = null
        /// StringUtils.repeat("", 0)   = ""
        /// StringUtils.repeat("", 2)   = ""
        /// StringUtils.repeat("a", 3)  = "aaa"
        /// StringUtils.repeat("ab", 2) = "abab"
        /// StringUtils.repeat("a", -2) = ""
        /// </pre>
        /// </summary>
        /// <param name="str">  the String to repeat, may be null </param>
        /// <param name="repeat">  number of times to repeat str, negative treated as zero </param>
        /// <returns> a new String consisting of the original String repeated,
        ///  {@code null} if null String input </returns>
        //JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
        //ORIGINAL LINE: public static String repeat(final String str, final int repeat)
        public static string repeat(string str, int repeat)
        {
            // Performance tuned for 2.0 (JDK1.4)

            if (string.ReferenceEquals(str, null))
            {
                return null;
            }
            if (repeat <= 0)
            {
                return EMPTY;
            }
            //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
            //ORIGINAL LINE: final int inputLength = str.length();
            int inputLength = str.Length;
            if (repeat == 1 || inputLength == 0)
            {
                return str;
            }
            if (inputLength == 1 && repeat <= PAD_LIMIT)
            {
                return StringUtils.repeat(str[0], repeat);
            }

            //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
            //ORIGINAL LINE: final int outputLength = inputLength * repeat;
            int outputLength = inputLength * repeat;
            switch (inputLength)
            {
                case 1:
                    return StringUtils.repeat(str[0], repeat);
                case 2:
                    //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
                    //ORIGINAL LINE: final char ch0 = str.charAt(0);
                    char ch0 = str[0];
                    //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
                    //ORIGINAL LINE: final char ch1 = str.charAt(1);
                    char ch1 = str[1];
                    //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
                    //ORIGINAL LINE: final char[] output2 = new char[outputLength];
                    char[] output2 = new char[outputLength];
                    for (int i = repeat * 2 - 2; i >= 0; i--, i--)
                    {
                        output2[i] = ch0;
                        output2[i + 1] = ch1;
                    }
                    return new string(output2);
                default:
                    //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
                    //ORIGINAL LINE: final StringBuilder buf = new StringBuilder(outputLength);
                    StringBuilder buf = new StringBuilder(outputLength);
                    for (int i = 0; i < repeat; i++)
                    {
                        buf.Append(str);
                    }
                    return buf.ToString();
            }
        }

        /// <summary>
        /// <para>Repeat a String {@code repeat} times to form a
        /// new String, with a String separator injected each time. </para>
        /// 
        /// <pre>
        /// StringUtils.repeat(null, null, 2) = null
        /// StringUtils.repeat(null, "x", 2)  = null
        /// StringUtils.repeat("", null, 0)   = ""
        /// StringUtils.repeat("", "", 2)     = ""
        /// StringUtils.repeat("", "x", 3)    = "xxx"
        /// StringUtils.repeat("?", ", ", 3)  = "?, ?, ?"
        /// </pre>
        /// </summary>
        /// <param name="str">        the String to repeat, may be null </param>
        /// <param name="separator">  the String to inject, may be null </param>
        /// <param name="repeat">     number of times to repeat str, negative treated as zero </param>
        /// <returns> a new String consisting of the original String repeated,
        ///  {@code null} if null String input
        /// @since 2.5 </returns>
        //JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
        //ORIGINAL LINE: public static String repeat(final String str, final String separator, final int repeat)
        public static string repeat(string str, string separator, int repeat)
        {
            if (string.ReferenceEquals(str, null) || string.ReferenceEquals(separator, null))
            {
                return StringUtils.repeat(str, repeat);
            }
            // given that repeat(String, int) is quite optimized, better to rely on it than try and splice this into it
            //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
            //ORIGINAL LINE: final String result = repeat(str + separator, repeat);
            string result = StringUtils.repeat(str + separator, repeat);
            return removeEnd(result, separator);
        }

        /// <summary>
        /// <para>Returns padding using the specified delimiter repeated
        /// to a given length.</para>
        /// 
        /// <pre>
        /// StringUtils.repeat('e', 0)  = ""
        /// StringUtils.repeat('e', 3)  = "eee"
        /// StringUtils.repeat('e', -2) = ""
        /// </pre>
        /// 
        /// <para>Note: this method doesn't not support padding with
        /// <a href="http://www.unicode.org/glossary/#supplementary_character">Unicode Supplementary Characters</a>
        /// as they require a pair of {@code char}s to be represented.
        /// If you are needing to support full I18N of your applications
        /// consider using <seealso cref="#repeat(String, int)"/> instead.
        /// </para>
        /// </summary>
        /// <param name="ch">  character to repeat </param>
        /// <param name="repeat">  number of times to repeat char, negative treated as zero </param>
        /// <returns> String with repeated character </returns>
        /// <seealso cref= #repeat(String, int) </seealso>
        //JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
        //ORIGINAL LINE: public static String repeat(final char ch, final int repeat)
        public static string repeat(char ch, int repeat)
        {
            if (repeat <= 0)
            {
                return EMPTY;
            }
            //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
            //ORIGINAL LINE: final char[] buf = new char[repeat];
            char[] buf = new char[repeat];
            for (int i = repeat - 1; i >= 0; i--)
            {
                buf[i] = ch;
            }
            return new string(buf);
        }

        /// <summary>
        /// <para>Right pad a String with spaces (' ').</para>
        /// 
        /// <para>The String is padded to the size of {@code size}.</para>
        /// 
        /// <pre>
        /// StringUtils.rightPad(null, *)   = null
        /// StringUtils.rightPad("", 3)     = "   "
        /// StringUtils.rightPad("bat", 3)  = "bat"
        /// StringUtils.rightPad("bat", 5)  = "bat  "
        /// StringUtils.rightPad("bat", 1)  = "bat"
        /// StringUtils.rightPad("bat", -1) = "bat"
        /// </pre>
        /// </summary>
        /// <param name="str">  the String to pad out, may be null </param>
        /// <param name="size">  the size to pad to </param>
        /// <returns> right padded String or original String if no padding is necessary,
        ///  {@code null} if null String input </returns>
        //JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
        //ORIGINAL LINE: public static String rightPad(final String str, final int size)
        public static string rightPad(string str, int size)
        {
            return rightPad(str, size, ' ');
        }

        /// <summary>
        /// <para>Right pad a String with a specified character.</para>
        /// 
        /// <para>The String is padded to the size of {@code size}.</para>
        /// 
        /// <pre>
        /// StringUtils.rightPad(null, *, *)     = null
        /// StringUtils.rightPad("", 3, 'z')     = "zzz"
        /// StringUtils.rightPad("bat", 3, 'z')  = "bat"
        /// StringUtils.rightPad("bat", 5, 'z')  = "batzz"
        /// StringUtils.rightPad("bat", 1, 'z')  = "bat"
        /// StringUtils.rightPad("bat", -1, 'z') = "bat"
        /// </pre>
        /// </summary>
        /// <param name="str">  the String to pad out, may be null </param>
        /// <param name="size">  the size to pad to </param>
        /// <param name="padChar">  the character to pad with </param>
        /// <returns> right padded String or original String if no padding is necessary,
        ///  {@code null} if null String input
        /// @since 2.0 </returns>
        //JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
        //ORIGINAL LINE: public static String rightPad(final String str, final int size, final char padChar)
        public static string rightPad(string str, int size, char padChar)
        {
            if (string.ReferenceEquals(str, null))
            {
                return null;
            }
            //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
            //ORIGINAL LINE: final int pads = size - str.length();
            int pads = size - str.Length;
            if (pads <= 0)
            {
                return str; // returns original String when possible
            }
            if (pads > PAD_LIMIT)
            {
                return rightPad(str, size, padChar.ToString());
            }
            return str + repeat(padChar, pads);
        }

        /// <summary>
        /// <para>Right pad a String with a specified String.</para>
        /// 
        /// <para>The String is padded to the size of {@code size}.</para>
        /// 
        /// <pre>
        /// StringUtils.rightPad(null, *, *)      = null
        /// StringUtils.rightPad("", 3, "z")      = "zzz"
        /// StringUtils.rightPad("bat", 3, "yz")  = "bat"
        /// StringUtils.rightPad("bat", 5, "yz")  = "batyz"
        /// StringUtils.rightPad("bat", 8, "yz")  = "batyzyzy"
        /// StringUtils.rightPad("bat", 1, "yz")  = "bat"
        /// StringUtils.rightPad("bat", -1, "yz") = "bat"
        /// StringUtils.rightPad("bat", 5, null)  = "bat  "
        /// StringUtils.rightPad("bat", 5, "")    = "bat  "
        /// </pre>
        /// </summary>
        /// <param name="str">  the String to pad out, may be null </param>
        /// <param name="size">  the size to pad to </param>
        /// <param name="padStr">  the String to pad with, null or empty treated as single space </param>
        /// <returns> right padded String or original String if no padding is necessary,
        ///  {@code null} if null String input </returns>
        //JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
        //ORIGINAL LINE: public static String rightPad(final String str, final int size, String padStr)
        public static string rightPad(string str, int size, string padStr)
        {
            if (string.ReferenceEquals(str, null))
            {
                return null;
            }
            if (isEmpty(padStr))
            {
                padStr = SPACE;
            }
            //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
            //ORIGINAL LINE: final int padLen = padStr.length();
            int padLen = padStr.Length;
            //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
            //ORIGINAL LINE: final int strLen = str.length();
            int strLen = str.Length;
            //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
            //ORIGINAL LINE: final int pads = size - strLen;
            int pads = size - strLen;
            if (pads <= 0)
            {
                return str; // returns original String when possible
            }
            if (padLen == 1 && pads <= PAD_LIMIT)
            {
                return rightPad(str, size, padStr[0]);
            }

            if (pads == padLen)
            {
                return str + padStr;
            }
            else if (pads < padLen)
            {
                return str + padStr.Substring(0, pads);
            }
            else
            {
                //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
                //ORIGINAL LINE: final char[] padding = new char[pads];
                char[] padding = new char[pads];
                //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
                //ORIGINAL LINE: final char[] padChars = padStr.toCharArray();
                char[] padChars = padStr.ToCharArray();
                for (int i = 0; i < pads; i++)
                {
                    padding[i] = padChars[i % padLen];
                }
                return str + new string(padding);
            }
        }

        /// <summary>
        /// <para>Left pad a String with spaces (' ').</para>
        /// 
        /// <para>The String is padded to the size of {@code size}.</para>
        /// 
        /// <pre>
        /// StringUtils.leftPad(null, *)   = null
        /// StringUtils.leftPad("", 3)     = "   "
        /// StringUtils.leftPad("bat", 3)  = "bat"
        /// StringUtils.leftPad("bat", 5)  = "  bat"
        /// StringUtils.leftPad("bat", 1)  = "bat"
        /// StringUtils.leftPad("bat", -1) = "bat"
        /// </pre>
        /// </summary>
        /// <param name="str">  the String to pad out, may be null </param>
        /// <param name="size">  the size to pad to </param>
        /// <returns> left padded String or original String if no padding is necessary,
        ///  {@code null} if null String input </returns>
        //JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
        //ORIGINAL LINE: public static String leftPad(final String str, final int size)
        public static string leftPad(string str, int size)
        {
            return leftPad(str, size, ' ');
        }

        /// <summary>
        /// <para>Left pad a String with a specified character.</para>
        /// 
        /// <para>Pad to a size of {@code size}.</para>
        /// 
        /// <pre>
        /// StringUtils.leftPad(null, *, *)     = null
        /// StringUtils.leftPad("", 3, 'z')     = "zzz"
        /// StringUtils.leftPad("bat", 3, 'z')  = "bat"
        /// StringUtils.leftPad("bat", 5, 'z')  = "zzbat"
        /// StringUtils.leftPad("bat", 1, 'z')  = "bat"
        /// StringUtils.leftPad("bat", -1, 'z') = "bat"
        /// </pre>
        /// </summary>
        /// <param name="str">  the String to pad out, may be null </param>
        /// <param name="size">  the size to pad to </param>
        /// <param name="padChar">  the character to pad with </param>
        /// <returns> left padded String or original String if no padding is necessary,
        ///  {@code null} if null String input
        /// @since 2.0 </returns>
        //JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
        //ORIGINAL LINE: public static String leftPad(final String str, final int size, final char padChar)
        public static string leftPad(string str, int size, char padChar)
        {
            if (string.ReferenceEquals(str, null))
            {
                return null;
            }
            //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
            //ORIGINAL LINE: final int pads = size - str.length();
            int pads = size - str.Length;
            if (pads <= 0)
            {
                return str; // returns original String when possible
            }
            if (pads > PAD_LIMIT)
            {
                return leftPad(str, size, padChar.ToString());
            }
            return repeat(padChar, pads) + str;
        }

        /// <summary>
        /// <para>Left pad a String with a specified String.</para>
        /// 
        /// <para>Pad to a size of {@code size}.</para>
        /// 
        /// <pre>
        /// StringUtils.leftPad(null, *, *)      = null
        /// StringUtils.leftPad("", 3, "z")      = "zzz"
        /// StringUtils.leftPad("bat", 3, "yz")  = "bat"
        /// StringUtils.leftPad("bat", 5, "yz")  = "yzbat"
        /// StringUtils.leftPad("bat", 8, "yz")  = "yzyzybat"
        /// StringUtils.leftPad("bat", 1, "yz")  = "bat"
        /// StringUtils.leftPad("bat", -1, "yz") = "bat"
        /// StringUtils.leftPad("bat", 5, null)  = "  bat"
        /// StringUtils.leftPad("bat", 5, "")    = "  bat"
        /// </pre>
        /// </summary>
        /// <param name="str">  the String to pad out, may be null </param>
        /// <param name="size">  the size to pad to </param>
        /// <param name="padStr">  the String to pad with, null or empty treated as single space </param>
        /// <returns> left padded String or original String if no padding is necessary,
        ///  {@code null} if null String input </returns>
        //JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
        //ORIGINAL LINE: public static String leftPad(final String str, final int size, String padStr)
        public static string leftPad(string str, int size, string padStr)
        {
            if (string.ReferenceEquals(str, null))
            {
                return null;
            }
            if (isEmpty(padStr))
            {
                padStr = SPACE;
            }
            //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
            //ORIGINAL LINE: final int padLen = padStr.length();
            int padLen = padStr.Length;
            //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
            //ORIGINAL LINE: final int strLen = str.length();
            int strLen = str.Length;
            //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
            //ORIGINAL LINE: final int pads = size - strLen;
            int pads = size - strLen;
            if (pads <= 0)
            {
                return str; // returns original String when possible
            }
            if (padLen == 1 && pads <= PAD_LIMIT)
            {
                return leftPad(str, size, padStr[0]);
            }

            if (pads == padLen)
            {
                return padStr + str;
            }
            else if (pads < padLen)
            {
                return padStr.Substring(0, pads) + str;
            }
            else
            {
                //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
                //ORIGINAL LINE: final char[] padding = new char[pads];
                char[] padding = new char[pads];
                //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
                //ORIGINAL LINE: final char[] padChars = padStr.toCharArray();
                char[] padChars = padStr.ToCharArray();
                for (int i = 0; i < pads; i++)
                {
                    padding[i] = padChars[i % padLen];
                }
                return (new string(padding)) + str;
            }
        }

        //      /// <summary>
        //      /// Gets a CharSequence length or {@code 0} if the CharSequence is
        //      /// {@code null}.
        //      /// </summary>
        //      /// <param name="cs">
        //      ///            a CharSequence or {@code null} </param>
        //      /// <returns> CharSequence length or {@code 0} if the CharSequence is
        //      ///         {@code null}.
        //      /// @since 2.4
        //      /// @since 3.0 Changed signature from length(String) to length(CharSequence) </returns>
        //      //JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
        //      //ORIGINAL LINE: public static int length(final CharSequence cs)
        //      public static int length(CharSequence cs)
        //      {
        //          return cs == null ? 0 : cs.length();
        //      }

        // Centering
        //-----------------------------------------------------------------------
        /// <summary>
        /// <para>Centers a String in a larger String of size {@code size}
        /// using the space character (' ').</para>
        /// 
        /// <para>If the size is less than the String length, the String is returned.
        /// A {@code null} String returns {@code null}.
        /// A negative size is treated as zero.</para>
        /// 
        /// <para>Equivalent to {@code center(str, size, " ")}.</para>
        /// 
        /// <pre>
        /// StringUtils.center(null, *)   = null
        /// StringUtils.center("", 4)     = "    "
        /// StringUtils.center("ab", -1)  = "ab"
        /// StringUtils.center("ab", 4)   = " ab "
        /// StringUtils.center("abcd", 2) = "abcd"
        /// StringUtils.center("a", 4)    = " a  "
        /// </pre>
        /// </summary>
        /// <param name="str">  the String to center, may be null </param>
        /// <param name="size">  the int size of new String, negative treated as zero </param>
        /// <returns> centered String, {@code null} if null String input </returns>
        //JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
        //ORIGINAL LINE: public static String center(final String str, final int size)
        public static string center(string str, int size)
        {
            return center(str, size, ' ');
        }

        /// <summary>
        /// <para>Centers a String in a larger String of size {@code size}.
        /// Uses a supplied character as the value to pad the String with.</para>
        /// 
        /// <para>If the size is less than the String length, the String is returned.
        /// A {@code null} String returns {@code null}.
        /// A negative size is treated as zero.</para>
        /// 
        /// <pre>
        /// StringUtils.center(null, *, *)     = null
        /// StringUtils.center("", 4, ' ')     = "    "
        /// StringUtils.center("ab", -1, ' ')  = "ab"
        /// StringUtils.center("ab", 4, ' ')   = " ab "
        /// StringUtils.center("abcd", 2, ' ') = "abcd"
        /// StringUtils.center("a", 4, ' ')    = " a  "
        /// StringUtils.center("a", 4, 'y')    = "yayy"
        /// </pre>
        /// </summary>
        /// <param name="str">  the String to center, may be null </param>
        /// <param name="size">  the int size of new String, negative treated as zero </param>
        /// <param name="padChar">  the character to pad the new String with </param>
        /// <returns> centered String, {@code null} if null String input
        /// @since 2.0 </returns>
        //JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
        //ORIGINAL LINE: public static String center(String str, final int size, final char padChar)
        public static string center(string str, int size, char padChar)
        {
            if (string.ReferenceEquals(str, null) || size <= 0)
            {
                return str;
            }
            //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
            //ORIGINAL LINE: final int strLen = str.length();
            int strLen = str.Length;
            //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
            //ORIGINAL LINE: final int pads = size - strLen;
            int pads = size - strLen;
            if (pads <= 0)
            {
                return str;
            }
            str = leftPad(str, strLen + pads / 2, padChar);
            str = rightPad(str, size, padChar);
            return str;
        }

        /// <summary>
        /// <para>Centers a String in a larger String of size {@code size}.
        /// Uses a supplied String as the value to pad the String with.</para>
        /// 
        /// <para>If the size is less than the String length, the String is returned.
        /// A {@code null} String returns {@code null}.
        /// A negative size is treated as zero.</para>
        /// 
        /// <pre>
        /// StringUtils.center(null, *, *)     = null
        /// StringUtils.center("", 4, " ")     = "    "
        /// StringUtils.center("ab", -1, " ")  = "ab"
        /// StringUtils.center("ab", 4, " ")   = " ab "
        /// StringUtils.center("abcd", 2, " ") = "abcd"
        /// StringUtils.center("a", 4, " ")    = " a  "
        /// StringUtils.center("a", 4, "yz")   = "yayz"
        /// StringUtils.center("abc", 7, null) = "  abc  "
        /// StringUtils.center("abc", 7, "")   = "  abc  "
        /// </pre>
        /// </summary>
        /// <param name="str">  the String to center, may be null </param>
        /// <param name="size">  the int size of new String, negative treated as zero </param>
        /// <param name="padStr">  the String to pad the new String with, must not be null or empty </param>
        /// <returns> centered String, {@code null} if null String input </returns>
        /// <exception cref="IllegalArgumentException"> if padStr is {@code null} or empty </exception>
        //JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
        //ORIGINAL LINE: public static String center(String str, final int size, String padStr)
        public static string center(string str, int size, string padStr)
        {
            if (string.ReferenceEquals(str, null) || size <= 0)
            {
                return str;
            }
            if (isEmpty(padStr))
            {
                padStr = SPACE;
            }
            //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
            //ORIGINAL LINE: final int strLen = str.length();
            int strLen = str.Length;
            //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
            //ORIGINAL LINE: final int pads = size - strLen;
            int pads = size - strLen;
            if (pads <= 0)
            {
                return str;
            }
            str = leftPad(str, strLen + pads / 2, padStr);
            str = rightPad(str, size, padStr);
            return str;
        }

        //      // Case conversion
        //      //-----------------------------------------------------------------------
        //      /// <summary>
        //      /// <para>Converts a String to upper case as per <seealso cref="String#toUpperCase()"/>.</para>
        //      /// 
        //      /// <para>A {@code null} input String returns {@code null}.</para>
        //      /// 
        //      /// <pre>
        //      /// StringUtils.upperCase(null)  = null
        //      /// StringUtils.upperCase("")    = ""
        //      /// StringUtils.upperCase("aBc") = "ABC"
        //      /// </pre>
        //      /// 
        //      /// <para><strong>Note:</strong> As described in the documentation for <seealso cref="String#toUpperCase()"/>,
        //      /// the result of this method is affected by the current locale.
        //      /// For platform-independent case transformations, the method <seealso cref="#lowerCase(String, Locale)"/>
        //      /// should be used with a specific locale (e.g. <seealso cref="Locale#ENGLISH"/>).</para>
        //      /// </summary>
        //      /// <param name="str">  the String to upper case, may be null </param>
        //      /// <returns> the upper cased String, {@code null} if null String input </returns>
        //      //JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
        //      //ORIGINAL LINE: public static String upperCase(final String str)
        //      public static string upperCase(string str)
        //      {
        //          if (string.ReferenceEquals(str, null))
        //          {
        //              return null;
        //          }
        //          return str.ToUpper();
        //      }

        //      /// <summary>
        //      /// <para>Converts a String to upper case as per <seealso cref="String#toUpperCase(Locale)"/>.</para>
        //      /// 
        //      /// <para>A {@code null} input String returns {@code null}.</para>
        //      /// 
        //      /// <pre>
        //      /// StringUtils.upperCase(null, Locale.ENGLISH)  = null
        //      /// StringUtils.upperCase("", Locale.ENGLISH)    = ""
        //      /// StringUtils.upperCase("aBc", Locale.ENGLISH) = "ABC"
        //      /// </pre>
        //      /// </summary>
        //      /// <param name="str">  the String to upper case, may be null </param>
        //      /// <param name="locale">  the locale that defines the case transformation rules, must not be null </param>
        //      /// <returns> the upper cased String, {@code null} if null String input
        //      /// @since 2.5 </returns>
        //      //JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
        //      //ORIGINAL LINE: public static String upperCase(final String str, final java.util.Locale locale)
        //      //public static string upperCase(string str, Locale locale)
        //      //{
        //      //    if (string.ReferenceEquals(str, null))
        //      //    {
        //      //        return null;
        //      //    }
        //      //    return str.ToUpper(locale);
        //      //}

        //      /// <summary>
        //      /// <para>Converts a String to lower case as per <seealso cref="String#toLowerCase()"/>.</para>
        //      /// 
        //      /// <para>A {@code null} input String returns {@code null}.</para>
        //      /// 
        //      /// <pre>
        //      /// StringUtils.lowerCase(null)  = null
        //      /// StringUtils.lowerCase("")    = ""
        //      /// StringUtils.lowerCase("aBc") = "abc"
        //      /// </pre>
        //      /// 
        //      /// <para><strong>Note:</strong> As described in the documentation for <seealso cref="String#toLowerCase()"/>,
        //      /// the result of this method is affected by the current locale.
        //      /// For platform-independent case transformations, the method <seealso cref="#lowerCase(String, Locale)"/>
        //      /// should be used with a specific locale (e.g. <seealso cref="Locale#ENGLISH"/>).</para>
        //      /// </summary>
        //      /// <param name="str">  the String to lower case, may be null </param>
        //      /// <returns> the lower cased String, {@code null} if null String input </returns>
        //      //JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
        //      //ORIGINAL LINE: public static String lowerCase(final String str)
        //      public static string lowerCase(string str)
        //      {
        //          if (string.ReferenceEquals(str, null))
        //          {
        //              return null;
        //          }
        //          return str.ToLower();
        //      }

        //      /// <summary>
        //      /// <para>Converts a String to lower case as per <seealso cref="String#toLowerCase(Locale)"/>.</para>
        //      /// 
        //      /// <para>A {@code null} input String returns {@code null}.</para>
        //      /// 
        //      /// <pre>
        //      /// StringUtils.lowerCase(null, Locale.ENGLISH)  = null
        //      /// StringUtils.lowerCase("", Locale.ENGLISH)    = ""
        //      /// StringUtils.lowerCase("aBc", Locale.ENGLISH) = "abc"
        //      /// </pre>
        //      /// </summary>
        //      /// <param name="str">  the String to lower case, may be null </param>
        //      /// <param name="locale">  the locale that defines the case transformation rules, must not be null </param>
        //      /// <returns> the lower cased String, {@code null} if null String input
        //      /// @since 2.5 </returns>
        //      //JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
        //      //ORIGINAL LINE: public static String lowerCase(final String str, final java.util.Locale locale)
        //      //public static string lowerCase(string str, Locale locale)
        //      //{
        //      //    if (string.ReferenceEquals(str, null))
        //      //    {
        //      //        return null;
        //      //    }
        //      //    return str.ToLower(locale);
        //      //}

        //      /// <summary>
        //      /// <para>Capitalizes a String changing the first character to title case as
        //      /// per <seealso cref="Character#toTitleCase(int)"/>. No other characters are changed.</para>
        //      /// 
        //      /// <para>For a word based algorithm, see <seealso cref="org.apache.commons.lang3.text.WordUtils#capitalize(String)"/>.
        //      /// A {@code null} input String returns {@code null}.</para>
        //      /// 
        //      /// <pre>
        //      /// StringUtils.capitalize(null)  = null
        //      /// StringUtils.capitalize("")    = ""
        //      /// StringUtils.capitalize("cat") = "Cat"
        //      /// StringUtils.capitalize("cAt") = "CAt"
        //      /// StringUtils.capitalize("'cat'") = "'cat'"
        //      /// </pre>
        //      /// </summary>
        //      /// <param name="str"> the String to capitalize, may be null </param>
        //      /// <returns> the capitalized String, {@code null} if null String input </returns>
        //      /// <seealso cref= org.apache.commons.lang3.text.WordUtils#capitalize(String) </seealso>
        //      /// <seealso cref= #uncapitalize(String)
        //      /// @since 2.0 </seealso>
        //      //JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
        //      //ORIGINAL LINE: public static String capitalize(final String str)
        //      //public static string capitalize(string str)
        //      //{
        //      //    int strLen;
        //      //    if (string.ReferenceEquals(str, null) || (strLen = str.Length) == 0)
        //      //    {
        //      //        return str;
        //      //    }

        //      //    //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
        //      //    //ORIGINAL LINE: final int firstCodepoint = str.codePointAt(0);
        //      //    int firstCodepoint = char.ConvertToUtf32(str, 0);
        //      //    //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
        //      //    //ORIGINAL LINE: final int newCodePoint = Character.toTitleCase(firstCodepoint);
        //      //    int newCodePoint = Char.ToTitleCase(firstCodepoint);
        //      //    if (firstCodepoint == newCodePoint)
        //      //    {
        //      //        // already capitalized
        //      //        return str;
        //      //    }

        //      //    //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
        //      //    //ORIGINAL LINE: final int newCodePoints[] = new int[strLen];
        //      //    int[] newCodePoints = new int[strLen]; // cannot be longer than the char array
        //      //    int outOffset = 0;
        //      //    newCodePoints[outOffset++] = newCodePoint; // copy the first codepoint
        //      //    for (int inOffset = Character.charCount(firstCodepoint); inOffset < strLen;)
        //      //    {
        //      //        //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
        //      //        //ORIGINAL LINE: final int codepoint = str.codePointAt(inOffset);
        //      //        int codepoint = char.ConvertToUtf32(str, inOffset);
        //      //        newCodePoints[outOffset++] = codepoint; // copy the remaining ones
        //      //        inOffset += Character.charCount(codepoint);
        //      //    }
        //      //    return new string(newCodePoints, 0, outOffset);
        //      //}

        //      /// <summary>
        //      /// <para>Uncapitalizes a String, changing the first character to lower case as
        //      /// per <seealso cref="Character#toLowerCase(int)"/>. No other characters are changed.</para>
        //      /// 
        //      /// <para>For a word based algorithm, see <seealso cref="org.apache.commons.lang3.text.WordUtils#uncapitalize(String)"/>.
        //      /// A {@code null} input String returns {@code null}.</para>
        //      /// 
        //      /// <pre>
        //      /// StringUtils.uncapitalize(null)  = null
        //      /// StringUtils.uncapitalize("")    = ""
        //      /// StringUtils.uncapitalize("cat") = "cat"
        //      /// StringUtils.uncapitalize("Cat") = "cat"
        //      /// StringUtils.uncapitalize("CAT") = "cAT"
        //      /// </pre>
        //      /// </summary>
        //      /// <param name="str"> the String to uncapitalize, may be null </param>
        //      /// <returns> the uncapitalized String, {@code null} if null String input </returns>
        //      /// <seealso cref= org.apache.commons.lang3.text.WordUtils#uncapitalize(String) </seealso>
        //      /// <seealso cref= #capitalize(String)
        //      /// @since 2.0 </seealso>
        //      //JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
        //      //ORIGINAL LINE: public static String uncapitalize(final String str)
        //      //public static string uncapitalize(string str)
        //      //{
        //      //    int strLen;
        //      //    if (string.ReferenceEquals(str, null) || (strLen = str.Length) == 0)
        //      //    {
        //      //        return str;
        //      //    }

        //      //    //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
        //      //    //ORIGINAL LINE: final int firstCodepoint = str.codePointAt(0);
        //      //    int firstCodepoint = char.ConvertToUtf32(str, 0);
        //      //    //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
        //      //    //ORIGINAL LINE: final int newCodePoint = Character.toLowerCase(firstCodepoint);
        //      //    int newCodePoint = char.ToLower(firstCodepoint);
        //      //    if (firstCodepoint == newCodePoint)
        //      //    {
        //      //        // already capitalized
        //      //        return str;
        //      //    }

        //      //    //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
        //      //    //ORIGINAL LINE: final int newCodePoints[] = new int[strLen];
        //      //    int[] newCodePoints = new int[strLen]; // cannot be longer than the char array
        //      //    int outOffset = 0;
        //      //    newCodePoints[outOffset++] = newCodePoint; // copy the first codepoint
        //      //    for (int inOffset = Character.charCount(firstCodepoint); inOffset < strLen;)
        //      //    {
        //      //        //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
        //      //        //ORIGINAL LINE: final int codepoint = str.codePointAt(inOffset);
        //      //        int codepoint = char.ConvertToUtf32(str, inOffset);
        //      //        newCodePoints[outOffset++] = codepoint; // copy the remaining ones
        //      //        inOffset += Character.charCount(codepoint);
        //      //    }
        //      //    return new string(newCodePoints, 0, outOffset);
        //      //}

        //      /// <summary>
        //      /// <para>Swaps the case of a String changing upper and title case to
        //      /// lower case, and lower case to upper case.</para>
        //      /// 
        //      /// <ul>
        //      ///  <li>Upper case character converts to Lower case</li>
        //      ///  <li>Title case character converts to Lower case</li>
        //      ///  <li>Lower case character converts to Upper case</li>
        //      /// </ul>
        //      /// 
        //      /// <para>For a word based algorithm, see <seealso cref="org.apache.commons.lang3.text.WordUtils#swapCase(String)"/>.
        //      /// A {@code null} input String returns {@code null}.</para>
        //      /// 
        //      /// <pre>
        //      /// StringUtils.swapCase(null)                 = null
        //      /// StringUtils.swapCase("")                   = ""
        //      /// StringUtils.swapCase("The dog has a BONE") = "tHE DOG HAS A bone"
        //      /// </pre>
        //      /// 
        //      /// <para>NOTE: This method changed in Lang version 2.0.
        //      /// It no longer performs a word based algorithm.
        //      /// If you only use ASCII, you will notice no change.
        //      /// That functionality is available in org.apache.commons.lang3.text.WordUtils.</para>
        //      /// </summary>
        //      /// <param name="str">  the String to swap case, may be null </param>
        //      /// <returns> the changed String, {@code null} if null String input </returns>
        //      //JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
        //      //ORIGINAL LINE: public static String swapCase(final String str)
        //      public static string swapCase(string str)
        //      {
        //          if (StringUtils.isEmpty(str))
        //          {
        //              return str;
        //          }

        //          //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
        //          //ORIGINAL LINE: final int strLen = str.length();
        //          int strLen = str.Length;
        //          //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
        //          //ORIGINAL LINE: final int newCodePoints[] = new int[strLen];
        //          int[] newCodePoints = new int[strLen]; // cannot be longer than the char array
        //          int outOffset = 0;
        //          for (int i = 0; i < strLen;)
        //          {
        //              //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
        //              //ORIGINAL LINE: final int oldCodepoint = str.codePointAt(i);
        //              int oldCodepoint = char.ConvertToUtf32(str, i);
        //              //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
        //              //ORIGINAL LINE: final int newCodePoint;
        //              int newCodePoint;
        //              if (char.IsUpper(oldCodepoint))
        //              {
        //                  newCodePoint = char.ToLower(oldCodepoint);
        //              }
        //              else if (Character.isTitleCase(oldCodepoint))
        //              {
        //                  newCodePoint = char.ToLower(oldCodepoint);
        //              }
        //              else if (char.IsLower(oldCodepoint))
        //              {
        //                  newCodePoint = char.ToUpper(oldCodepoint);
        //              }
        //              else
        //              {
        //                  newCodePoint = oldCodepoint;
        //              }
        //              newCodePoints[outOffset++] = newCodePoint;
        //              i += Character.charCount(newCodePoint);
        //          }
        //          return new string(newCodePoints, 0, outOffset);
        //      }

        //      // Count matches
        //      //-----------------------------------------------------------------------
        //      /// <summary>
        //      /// <para>Counts how many times the substring appears in the larger string.</para>
        //      /// 
        //      /// <para>A {@code null} or empty ("") String input returns {@code 0}.</para>
        //      /// 
        //      /// <pre>
        //      /// StringUtils.countMatches(null, *)       = 0
        //      /// StringUtils.countMatches("", *)         = 0
        //      /// StringUtils.countMatches("abba", null)  = 0
        //      /// StringUtils.countMatches("abba", "")    = 0
        //      /// StringUtils.countMatches("abba", "a")   = 2
        //      /// StringUtils.countMatches("abba", "ab")  = 1
        //      /// StringUtils.countMatches("abba", "xxx") = 0
        //      /// </pre>
        //      /// </summary>
        //      /// <param name="str">  the CharSequence to check, may be null </param>
        //      /// <param name="sub">  the substring to count, may be null </param>
        //      /// <returns> the number of occurrences, 0 if either CharSequence is {@code null}
        //      /// @since 3.0 Changed signature from countMatches(String, String) to countMatches(CharSequence, CharSequence) </returns>
        //      //JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
        //      //ORIGINAL LINE: public static int countMatches(final CharSequence str, final CharSequence sub)
        //      public static int countMatches(CharSequence str, CharSequence sub)
        //      {
        //          if (isEmpty(str) || isEmpty(sub))
        //          {
        //              return 0;
        //          }
        //          int count = 0;
        //          int idx = 0;
        //          while ((idx = CharSequenceUtils.IndexOf(str, sub, idx)) != INDEX_NOT_FOUND)
        //          {
        //              count++;
        //              idx += sub.length();
        //          }
        //          return count;
        //      }

        //      /// <summary>
        //      /// <para>Counts how many times the char appears in the given string.</para>
        //      /// 
        //      /// <para>A {@code null} or empty ("") String input returns {@code 0}.</para>
        //      /// 
        //      /// <pre>
        //      /// StringUtils.countMatches(null, *)       = 0
        //      /// StringUtils.countMatches("", *)         = 0
        //      /// StringUtils.countMatches("abba", 0)  = 0
        //      /// StringUtils.countMatches("abba", 'a')   = 2
        //      /// StringUtils.countMatches("abba", 'b')  = 2
        //      /// StringUtils.countMatches("abba", 'x') = 0
        //      /// </pre>
        //      /// </summary>
        //      /// <param name="str">  the CharSequence to check, may be null </param>
        //      /// <param name="ch">  the char to count </param>
        //      /// <returns> the number of occurrences, 0 if the CharSequence is {@code null}
        //      /// @since 3.4 </returns>
        //      //JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
        //      //ORIGINAL LINE: public static int countMatches(final CharSequence str, final char ch)
        //      public static int countMatches(CharSequence str, char ch)
        //      {
        //          if (isEmpty(str))
        //          {
        //              return 0;
        //          }
        //          int count = 0;
        //          // We could also call str.toCharArray() for faster look ups but that would generate more garbage.
        //          for (int i = 0; i < str.length(); i++)
        //          {
        //              if (ch == str.charAt(i))
        //              {
        //                  count++;
        //              }
        //          }
        //          return count;
        //      }

        //      // Character Tests
        //      //-----------------------------------------------------------------------
        //      /// <summary>
        //      /// <para>Checks if the CharSequence contains only Unicode letters.</para>
        //      /// 
        //      /// <para>{@code null} will return {@code false}.
        //      /// An empty CharSequence (length()=0) will return {@code false}.</para>
        //      /// 
        //      /// <pre>
        //      /// StringUtils.isAlpha(null)   = false
        //      /// StringUtils.isAlpha("")     = false
        //      /// StringUtils.isAlpha("  ")   = false
        //      /// StringUtils.isAlpha("abc")  = true
        //      /// StringUtils.isAlpha("ab2c") = false
        //      /// StringUtils.isAlpha("ab-c") = false
        //      /// </pre>
        //      /// </summary>
        //      /// <param name="cs">  the CharSequence to check, may be null </param>
        //      /// <returns> {@code true} if only contains letters, and is non-null
        //      /// @since 3.0 Changed signature from isAlpha(String) to isAlpha(CharSequence)
        //      /// @since 3.0 Changed "" to return false and not true </returns>
        //      //JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
        //      //ORIGINAL LINE: public static boolean isAlpha(final CharSequence cs)
        //      public static bool isAlpha(CharSequence cs)
        //      {
        //          if (isEmpty(cs))
        //          {
        //              return false;
        //          }
        //          //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
        //          //ORIGINAL LINE: final int sz = cs.length();
        //          int sz = cs.length();
        //          for (int i = 0; i < sz; i++)
        //          {
        //              if (char.IsLetter(cs.charAt(i)) == false)
        //              {
        //                  return false;
        //              }
        //          }
        //          return true;
        //      }

        //      /// <summary>
        //      /// <para>Checks if the CharSequence contains only Unicode letters and
        //      /// space (' ').</para>
        //      /// 
        //      /// <para>{@code null} will return {@code false}
        //      /// An empty CharSequence (length()=0) will return {@code true}.</para>
        //      /// 
        //      /// <pre>
        //      /// StringUtils.isAlphaSpace(null)   = false
        //      /// StringUtils.isAlphaSpace("")     = true
        //      /// StringUtils.isAlphaSpace("  ")   = true
        //      /// StringUtils.isAlphaSpace("abc")  = true
        //      /// StringUtils.isAlphaSpace("ab c") = true
        //      /// StringUtils.isAlphaSpace("ab2c") = false
        //      /// StringUtils.isAlphaSpace("ab-c") = false
        //      /// </pre>
        //      /// </summary>
        //      /// <param name="cs">  the CharSequence to check, may be null </param>
        //      /// <returns> {@code true} if only contains letters and space,
        //      ///  and is non-null
        //      /// @since 3.0 Changed signature from isAlphaSpace(String) to isAlphaSpace(CharSequence) </returns>
        //      //JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
        //      //ORIGINAL LINE: public static boolean isAlphaSpace(final CharSequence cs)
        //      public static bool isAlphaSpace(CharSequence cs)
        //      {
        //          if (cs == null)
        //          {
        //              return false;
        //          }
        //          //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
        //          //ORIGINAL LINE: final int sz = cs.length();
        //          int sz = cs.length();
        //          for (int i = 0; i < sz; i++)
        //          {
        //              if (char.IsLetter(cs.charAt(i)) == false && cs.charAt(i) != ' ')
        //              {
        //                  return false;
        //              }
        //          }
        //          return true;
        //      }

        //      /// <summary>
        //      /// <para>Checks if the CharSequence contains only Unicode letters or digits.</para>
        //      /// 
        //      /// <para>{@code null} will return {@code false}.
        //      /// An empty CharSequence (length()=0) will return {@code false}.</para>
        //      /// 
        //      /// <pre>
        //      /// StringUtils.isAlphanumeric(null)   = false
        //      /// StringUtils.isAlphanumeric("")     = false
        //      /// StringUtils.isAlphanumeric("  ")   = false
        //      /// StringUtils.isAlphanumeric("abc")  = true
        //      /// StringUtils.isAlphanumeric("ab c") = false
        //      /// StringUtils.isAlphanumeric("ab2c") = true
        //      /// StringUtils.isAlphanumeric("ab-c") = false
        //      /// </pre>
        //      /// </summary>
        //      /// <param name="cs">  the CharSequence to check, may be null </param>
        //      /// <returns> {@code true} if only contains letters or digits,
        //      ///  and is non-null
        //      /// @since 3.0 Changed signature from isAlphanumeric(String) to isAlphanumeric(CharSequence)
        //      /// @since 3.0 Changed "" to return false and not true </returns>
        //      //JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
        //      //ORIGINAL LINE: public static boolean isAlphanumeric(final CharSequence cs)
        //      public static bool isAlphanumeric(CharSequence cs)
        //      {
        //          if (isEmpty(cs))
        //          {
        //              return false;
        //          }
        //          //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
        //          //ORIGINAL LINE: final int sz = cs.length();
        //          int sz = cs.length();
        //          for (int i = 0; i < sz; i++)
        //          {
        //              if (char.IsLetterOrDigit(cs.charAt(i)) == false)
        //              {
        //                  return false;
        //              }
        //          }
        //          return true;
        //      }

        //      /// <summary>
        //      /// <para>Checks if the CharSequence contains only Unicode letters, digits
        //      /// or space ({@code ' '}).</para>
        //      /// 
        //      /// <para>{@code null} will return {@code false}.
        //      /// An empty CharSequence (length()=0) will return {@code true}.</para>
        //      /// 
        //      /// <pre>
        //      /// StringUtils.isAlphanumericSpace(null)   = false
        //      /// StringUtils.isAlphanumericSpace("")     = true
        //      /// StringUtils.isAlphanumericSpace("  ")   = true
        //      /// StringUtils.isAlphanumericSpace("abc")  = true
        //      /// StringUtils.isAlphanumericSpace("ab c") = true
        //      /// StringUtils.isAlphanumericSpace("ab2c") = true
        //      /// StringUtils.isAlphanumericSpace("ab-c") = false
        //      /// </pre>
        //      /// </summary>
        //      /// <param name="cs">  the CharSequence to check, may be null </param>
        //      /// <returns> {@code true} if only contains letters, digits or space,
        //      ///  and is non-null
        //      /// @since 3.0 Changed signature from isAlphanumericSpace(String) to isAlphanumericSpace(CharSequence) </returns>
        //      //JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
        //      //ORIGINAL LINE: public static boolean isAlphanumericSpace(final CharSequence cs)
        //      public static bool isAlphanumericSpace(CharSequence cs)
        //      {
        //          if (cs == null)
        //          {
        //              return false;
        //          }
        //          //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
        //          //ORIGINAL LINE: final int sz = cs.length();
        //          int sz = cs.length();
        //          for (int i = 0; i < sz; i++)
        //          {
        //              if (char.IsLetterOrDigit(cs.charAt(i)) == false && cs.charAt(i) != ' ')
        //              {
        //                  return false;
        //              }
        //          }
        //          return true;
        //      }

        //      /// <summary>
        //      /// <para>Checks if the CharSequence contains only ASCII printable characters.</para>
        //      /// 
        //      /// <para>{@code null} will return {@code false}.
        //      /// An empty CharSequence (length()=0) will return {@code true}.</para>
        //      /// 
        //      /// <pre>
        //      /// StringUtils.isAsciiPrintable(null)     = false
        //      /// StringUtils.isAsciiPrintable("")       = true
        //      /// StringUtils.isAsciiPrintable(" ")      = true
        //      /// StringUtils.isAsciiPrintable("Ceki")   = true
        //      /// StringUtils.isAsciiPrintable("ab2c")   = true
        //      /// StringUtils.isAsciiPrintable("!ab-c~") = true
        //      /// StringUtils.isAsciiPrintable("\u0020") = true
        //      /// StringUtils.isAsciiPrintable("\u0021") = true
        //      /// StringUtils.isAsciiPrintable("\u007e") = true
        //      /// StringUtils.isAsciiPrintable("\u007f") = false
        //      /// StringUtils.isAsciiPrintable("Ceki G\u00fclc\u00fc") = false
        //      /// </pre>
        //      /// </summary>
        //      /// <param name="cs"> the CharSequence to check, may be null </param>
        //      /// <returns> {@code true} if every character is in the range
        //      ///  32 thru 126
        //      /// @since 2.1
        //      /// @since 3.0 Changed signature from isAsciiPrintable(String) to isAsciiPrintable(CharSequence) </returns>
        //      //JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
        //      //ORIGINAL LINE: public static boolean isAsciiPrintable(final CharSequence cs)
        //      public static bool isAsciiPrintable(CharSequence cs)
        //      {
        //          if (cs == null)
        //          {
        //              return false;
        //          }
        //          //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
        //          //ORIGINAL LINE: final int sz = cs.length();
        //          int sz = cs.length();
        //          for (int i = 0; i < sz; i++)
        //          {
        //              if (CharUtils.isAsciiPrintable(cs.charAt(i)) == false)
        //              {
        //                  return false;
        //              }
        //          }
        //          return true;
        //      }

        //      /// <summary>
        //      /// <para>Checks if the CharSequence contains only Unicode digits.
        //      /// A decimal point is not a Unicode digit and returns false.</para>
        //      /// 
        //      /// <para>{@code null} will return {@code false}.
        //      /// An empty CharSequence (length()=0) will return {@code false}.</para>
        //      /// 
        //      /// <para>Note that the method does not allow for a leading sign, either positive or negative.
        //      /// Also, if a String passes the numeric test, it may still generate a NumberFormatException
        //      /// when parsed by Integer.parseInt or Long.parseLong, e.g. if the value is outside the range
        //      /// for int or long respectively.</para>
        //      /// 
        //      /// <pre>
        //      /// StringUtils.isNumeric(null)   = false
        //      /// StringUtils.isNumeric("")     = false
        //      /// StringUtils.isNumeric("  ")   = false
        //      /// StringUtils.isNumeric("123")  = true
        //      /// StringUtils.isNumeric("\u0967\u0968\u0969")  = true
        //      /// StringUtils.isNumeric("12 3") = false
        //      /// StringUtils.isNumeric("ab2c") = false
        //      /// StringUtils.isNumeric("12-3") = false
        //      /// StringUtils.isNumeric("12.3") = false
        //      /// StringUtils.isNumeric("-123") = false
        //      /// StringUtils.isNumeric("+123") = false
        //      /// </pre>
        //      /// </summary>
        //      /// <param name="cs">  the CharSequence to check, may be null </param>
        //      /// <returns> {@code true} if only contains digits, and is non-null
        //      /// @since 3.0 Changed signature from isNumeric(String) to isNumeric(CharSequence)
        //      /// @since 3.0 Changed "" to return false and not true </returns>
        //      //JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
        //      //ORIGINAL LINE: public static boolean isNumeric(final CharSequence cs)
        //      public static bool isNumeric(CharSequence cs)
        //      {
        //          if (isEmpty(cs))
        //          {
        //              return false;
        //          }
        //          //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
        //          //ORIGINAL LINE: final int sz = cs.length();
        //          int sz = cs.length();
        //          for (int i = 0; i < sz; i++)
        //          {
        //              if (!char.IsDigit(cs.charAt(i)))
        //              {
        //                  return false;
        //              }
        //          }
        //          return true;
        //      }

        //      /// <summary>
        //      /// <para>Checks if the CharSequence contains only Unicode digits or space
        //      /// ({@code ' '}).
        //      /// A decimal point is not a Unicode digit and returns false.</para>
        //      /// 
        //      /// <para>{@code null} will return {@code false}.
        //      /// An empty CharSequence (length()=0) will return {@code true}.</para>
        //      /// 
        //      /// <pre>
        //      /// StringUtils.isNumericSpace(null)   = false
        //      /// StringUtils.isNumericSpace("")     = true
        //      /// StringUtils.isNumericSpace("  ")   = true
        //      /// StringUtils.isNumericSpace("123")  = true
        //      /// StringUtils.isNumericSpace("12 3") = true
        //      /// StringUtils.isNumeric("\u0967\u0968\u0969")  = true
        //      /// StringUtils.isNumeric("\u0967\u0968 \u0969")  = true
        //      /// StringUtils.isNumericSpace("ab2c") = false
        //      /// StringUtils.isNumericSpace("12-3") = false
        //      /// StringUtils.isNumericSpace("12.3") = false
        //      /// </pre>
        //      /// </summary>
        //      /// <param name="cs">  the CharSequence to check, may be null </param>
        //      /// <returns> {@code true} if only contains digits or space,
        //      ///  and is non-null
        //      /// @since 3.0 Changed signature from isNumericSpace(String) to isNumericSpace(CharSequence) </returns>
        //      //JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
        //      //ORIGINAL LINE: public static boolean isNumericSpace(final CharSequence cs)
        //      public static bool isNumericSpace(CharSequence cs)
        //      {
        //          if (cs == null)
        //          {
        //              return false;
        //          }
        //          //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
        //          //ORIGINAL LINE: final int sz = cs.length();
        //          int sz = cs.length();
        //          for (int i = 0; i < sz; i++)
        //          {
        //              if (char.IsDigit(cs.charAt(i)) == false && cs.charAt(i) != ' ')
        //              {
        //                  return false;
        //              }
        //          }
        //          return true;
        //      }

        //      /// <summary>
        //      /// <para>Checks if the CharSequence contains only whitespace.</para>
        //      /// 
        //      /// </p>Whitespace is defined by <seealso cref="Character#isWhitespace(char)"/>.</p>
        //      /// 
        //      /// <para>{@code null} will return {@code false}.
        //      /// An empty CharSequence (length()=0) will return {@code true}.</para>
        //      /// 
        //      /// <pre>
        //      /// StringUtils.isWhitespace(null)   = false
        //      /// StringUtils.isWhitespace("")     = true
        //      /// StringUtils.isWhitespace("  ")   = true
        //      /// StringUtils.isWhitespace("abc")  = false
        //      /// StringUtils.isWhitespace("ab2c") = false
        //      /// StringUtils.isWhitespace("ab-c") = false
        //      /// </pre>
        //      /// </summary>
        //      /// <param name="cs">  the CharSequence to check, may be null </param>
        //      /// <returns> {@code true} if only contains whitespace, and is non-null
        //      /// @since 2.0
        //      /// @since 3.0 Changed signature from isWhitespace(String) to isWhitespace(CharSequence) </returns>
        //      //JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
        //      //ORIGINAL LINE: public static boolean isWhitespace(final CharSequence cs)
        //      public static bool isWhitespace(CharSequence cs)
        //      {
        //          if (cs == null)
        //          {
        //              return false;
        //          }
        //          //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
        //          //ORIGINAL LINE: final int sz = cs.length();
        //          int sz = cs.length();
        //          for (int i = 0; i < sz; i++)
        //          {
        //              if (char.IsWhiteSpace(cs.charAt(i)) == false)
        //              {
        //                  return false;
        //              }
        //          }
        //          return true;
        //      }

        //      /// <summary>
        //      /// <para>Checks if the CharSequence contains only lowercase characters.</para>
        //      /// 
        //      /// <para>{@code null} will return {@code false}.
        //      /// An empty CharSequence (length()=0) will return {@code false}.</para>
        //      /// 
        //      /// <pre>
        //      /// StringUtils.isAllLowerCase(null)   = false
        //      /// StringUtils.isAllLowerCase("")     = false
        //      /// StringUtils.isAllLowerCase("  ")   = false
        //      /// StringUtils.isAllLowerCase("abc")  = true
        //      /// StringUtils.isAllLowerCase("abC")  = false
        //      /// StringUtils.isAllLowerCase("ab c") = false
        //      /// StringUtils.isAllLowerCase("ab1c") = false
        //      /// StringUtils.isAllLowerCase("ab/c") = false
        //      /// </pre>
        //      /// </summary>
        //      /// <param name="cs">  the CharSequence to check, may be null </param>
        //      /// <returns> {@code true} if only contains lowercase characters, and is non-null
        //      /// @since 2.5
        //      /// @since 3.0 Changed signature from isAllLowerCase(String) to isAllLowerCase(CharSequence) </returns>
        //      //JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
        //      //ORIGINAL LINE: public static boolean isAllLowerCase(final CharSequence cs)
        //      public static bool isAllLowerCase(CharSequence cs)
        //      {
        //          if (cs == null || isEmpty(cs))
        //          {
        //              return false;
        //          }
        //          //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
        //          //ORIGINAL LINE: final int sz = cs.length();
        //          int sz = cs.length();
        //          for (int i = 0; i < sz; i++)
        //          {
        //              if (char.IsLower(cs.charAt(i)) == false)
        //              {
        //                  return false;
        //              }
        //          }
        //          return true;
        //      }

        //      /// <summary>
        //      /// <para>Checks if the CharSequence contains only uppercase characters.</para>
        //      /// 
        //      /// <para>{@code null} will return {@code false}.
        //      /// An empty String (length()=0) will return {@code false}.</para>
        //      /// 
        //      /// <pre>
        //      /// StringUtils.isAllUpperCase(null)   = false
        //      /// StringUtils.isAllUpperCase("")     = false
        //      /// StringUtils.isAllUpperCase("  ")   = false
        //      /// StringUtils.isAllUpperCase("ABC")  = true
        //      /// StringUtils.isAllUpperCase("aBC")  = false
        //      /// StringUtils.isAllUpperCase("A C")  = false
        //      /// StringUtils.isAllUpperCase("A1C")  = false
        //      /// StringUtils.isAllUpperCase("A/C")  = false
        //      /// </pre>
        //      /// </summary>
        //      /// <param name="cs"> the CharSequence to check, may be null </param>
        //      /// <returns> {@code true} if only contains uppercase characters, and is non-null
        //      /// @since 2.5
        //      /// @since 3.0 Changed signature from isAllUpperCase(String) to isAllUpperCase(CharSequence) </returns>
        //      //JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
        //      //ORIGINAL LINE: public static boolean isAllUpperCase(final CharSequence cs)
        //      public static bool isAllUpperCase(CharSequence cs)
        //      {
        //          if (cs == null || isEmpty(cs))
        //          {
        //              return false;
        //          }
        //          //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
        //          //ORIGINAL LINE: final int sz = cs.length();
        //          int sz = cs.length();
        //          for (int i = 0; i < sz; i++)
        //          {
        //              if (char.IsUpper(cs.charAt(i)) == false)
        //              {
        //                  return false;
        //              }
        //          }
        //          return true;
        //      }

        //      // Defaults
        //      //-----------------------------------------------------------------------
        //      /// <summary>
        //      /// <para>Returns either the passed in String,
        //      /// or if the String is {@code null}, an empty String ("").</para>
        //      /// 
        //      /// <pre>
        //      /// StringUtils.defaultString(null)  = ""
        //      /// StringUtils.defaultString("")    = ""
        //      /// StringUtils.defaultString("bat") = "bat"
        //      /// </pre>
        //      /// </summary>
        //      /// <seealso cref= ObjectUtils#toString(Object) </seealso>
        //      /// <seealso cref= String#valueOf(Object) </seealso>
        //      /// <param name="str">  the String to check, may be null </param>
        //      /// <returns> the passed in String, or the empty String if it
        //      ///  was {@code null} </returns>
        //      //JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
        //      //ORIGINAL LINE: public static String defaultString(final String str)
        //      public static string defaultString(string str)
        //      {
        //          return string.ReferenceEquals(str, null) ? EMPTY : str;
        //      }

        //      /// <summary>
        //      /// <para>Returns either the passed in String, or if the String is
        //      /// {@code null}, the value of {@code defaultStr}.</para>
        //      /// 
        //      /// <pre>
        //      /// StringUtils.defaultString(null, "NULL")  = "NULL"
        //      /// StringUtils.defaultString("", "NULL")    = ""
        //      /// StringUtils.defaultString("bat", "NULL") = "bat"
        //      /// </pre>
        //      /// </summary>
        //      /// <seealso cref= ObjectUtils#toString(Object,String) </seealso>
        //      /// <seealso cref= String#valueOf(Object) </seealso>
        //      /// <param name="str">  the String to check, may be null </param>
        //      /// <param name="defaultStr">  the default String to return
        //      ///  if the input is {@code null}, may be null </param>
        //      /// <returns> the passed in String, or the default if it was {@code null} </returns>
        //      //JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
        //      //ORIGINAL LINE: public static String defaultString(final String str, final String defaultStr)
        //      public static string defaultString(string str, string defaultStr)
        //      {
        //          return string.ReferenceEquals(str, null) ? defaultStr : str;
        //      }

        //      /// <summary>
        //      /// <para>Returns either the passed in CharSequence, or if the CharSequence is
        //      /// whitespace, empty ("") or {@code null}, the value of {@code defaultStr}.</para>
        //      /// 
        //      /// </p>Whitespace is defined by <seealso cref="Character#isWhitespace(char)"/>.</p>
        //      /// 
        //      /// <pre>
        //      /// StringUtils.defaultIfBlank(null, "NULL")  = "NULL"
        //      /// StringUtils.defaultIfBlank("", "NULL")    = "NULL"
        //      /// StringUtils.defaultIfBlank(" ", "NULL")   = "NULL"
        //      /// StringUtils.defaultIfBlank("bat", "NULL") = "bat"
        //      /// StringUtils.defaultIfBlank("", null)      = null
        //      /// </pre> </summary>
        //      /// @param <T> the specific kind of CharSequence </param>
        //      /// <param name="str"> the CharSequence to check, may be null </param>
        //      /// <param name="defaultStr">  the default CharSequence to return
        //      ///  if the input is whitespace, empty ("") or {@code null}, may be null </param>
        //      /// <returns> the passed in CharSequence, or the default </returns>
        //      /// <seealso cref= StringUtils#defaultString(String, String) </seealso>
        //      //JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
        //      //ORIGINAL LINE: public static <T extends CharSequence> T defaultIfBlank(final T str, final T defaultStr)
        //      public static T defaultIfBlank<T>(T str, T defaultStr) where T : CharSequence
        //      {
        //          return isBlank(str) ? defaultStr : str;
        //      }

        //      /// <summary>
        //      /// <para>Returns either the passed in CharSequence, or if the CharSequence is
        //      /// empty or {@code null}, the value of {@code defaultStr}.</para>
        //      /// 
        //      /// <pre>
        //      /// StringUtils.defaultIfEmpty(null, "NULL")  = "NULL"
        //      /// StringUtils.defaultIfEmpty("", "NULL")    = "NULL"
        //      /// StringUtils.defaultIfEmpty(" ", "NULL")   = " "
        //      /// StringUtils.defaultIfEmpty("bat", "NULL") = "bat"
        //      /// StringUtils.defaultIfEmpty("", null)      = null
        //      /// </pre> </summary>
        //      /// @param <T> the specific kind of CharSequence </param>
        //      /// <param name="str">  the CharSequence to check, may be null </param>
        //      /// <param name="defaultStr">  the default CharSequence to return
        //      ///  if the input is empty ("") or {@code null}, may be null </param>
        //      /// <returns> the passed in CharSequence, or the default </returns>
        //      /// <seealso cref= StringUtils#defaultString(String, String) </seealso>
        //      //JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
        //      //ORIGINAL LINE: public static <T extends CharSequence> T defaultIfEmpty(final T str, final T defaultStr)
        //      public static T defaultIfEmpty<T>(T str, T defaultStr) where T : CharSequence
        //      {
        //          return isEmpty(str) ? defaultStr : str;
        //      }

        //      // Rotating (circular shift)
        //      //-----------------------------------------------------------------------
        //      /// <summary>
        //      /// <para>Rotate (circular shift) a String of {@code shift} characters.</para>
        //      /// <ul>
        //      ///  <li>If {@code shift > 0}, right circular shift (ex : ABCDEF =&gt; FABCDE)</li>
        //      ///  <li>If {@code shift < 0}, left circular shift (ex : ABCDEF =&gt; BCDEFA)</li>
        //      /// </ul>
        //      /// 
        //      /// <pre>
        //      /// StringUtils.rotate(null, *)        = null
        //      /// StringUtils.rotate("", *)          = ""
        //      /// StringUtils.rotate("abcdefg", 0)   = "abcdefg"
        //      /// StringUtils.rotate("abcdefg", 2)   = "fgabcde"
        //      /// StringUtils.rotate("abcdefg", -2)  = "cdefgab"
        //      /// StringUtils.rotate("abcdefg", 7)   = "abcdefg"
        //      /// StringUtils.rotate("abcdefg", -7)  = "abcdefg"
        //      /// StringUtils.rotate("abcdefg", 9)   = "fgabcde"
        //      /// StringUtils.rotate("abcdefg", -9)  = "cdefgab"
        //      /// </pre>
        //      /// </summary>
        //      /// <param name="str">  the String to rotate, may be null </param>
        //      /// <param name="shift">  number of time to shift (positive : right shift, negative : left shift) </param>
        //      /// <returns> the rotated String,
        //      ///          or the original String if {@code shift == 0},
        //      ///          or {@code null} if null String input
        //      /// @since 3.5 </returns>
        //      //JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
        //      //ORIGINAL LINE: public static String rotate(final String str, final int shift)
        //      public static string rotate(string str, int shift)
        //      {
        //          if (string.ReferenceEquals(str, null))
        //          {
        //              return null;
        //          }

        //          //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
        //          //ORIGINAL LINE: final int strLen = str.length();
        //          int strLen = str.Length;
        //          if (shift == 0 || strLen == 0 || shift % strLen == 0)
        //          {
        //              return str;
        //          }

        //          //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
        //          //ORIGINAL LINE: final StringBuilder builder = new StringBuilder(strLen);
        //          StringBuilder builder = new StringBuilder(strLen);
        //          //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
        //          //ORIGINAL LINE: final int offset = - (shift % strLen);
        //          int offset = -(shift % strLen);
        //          builder.Append(substring(str, offset));
        //          builder.Append(substring(str, 0, offset));
        //          return builder.ToString();
        //      }

        //      // Reversing
        //      //-----------------------------------------------------------------------
        //      /// <summary>
        //      /// <para>Reverses a String as per <seealso cref="StringBuilder#reverse()"/>.</para>
        //      /// 
        //      /// <para>A {@code null} String returns {@code null}.</para>
        //      /// 
        //      /// <pre>
        //      /// StringUtils.reverse(null)  = null
        //      /// StringUtils.reverse("")    = ""
        //      /// StringUtils.reverse("bat") = "tab"
        //      /// </pre>
        //      /// </summary>
        //      /// <param name="str">  the String to reverse, may be null </param>
        //      /// <returns> the reversed String, {@code null} if null String input </returns>
        //      //JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
        //      //ORIGINAL LINE: public static String reverse(final String str)
        //      public static string reverse(string str)
        //      {
        //          if (string.ReferenceEquals(str, null))
        //          {
        //              return null;
        //          }
        //          //JAVA TO C# CONVERTER TODO TASK: There is no .NET StringBuilder equivalent to the Java 'reverse' method:
        //          return (new StringBuilder(str)).reverse().ToString();
        //      }

        //      /// <summary>
        //      /// <para>Reverses a String that is delimited by a specific character.</para>
        //      /// 
        //      /// <para>The Strings between the delimiters are not reversed.
        //      /// Thus java.lang.String becomes String.lang.java (if the delimiter
        //      /// is {@code '.'}).</para>
        //      /// 
        //      /// <pre>
        //      /// StringUtils.reverseDelimited(null, *)      = null
        //      /// StringUtils.reverseDelimited("", *)        = ""
        //      /// StringUtils.reverseDelimited("a.b.c", 'x') = "a.b.c"
        //      /// StringUtils.reverseDelimited("a.b.c", ".") = "c.b.a"
        //      /// </pre>
        //      /// </summary>
        //      /// <param name="str">  the String to reverse, may be null </param>
        //      /// <param name="separatorChar">  the separator character to use </param>
        //      /// <returns> the reversed String, {@code null} if null String input
        //      /// @since 2.0 </returns>
        //      //JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
        //      //ORIGINAL LINE: public static String reverseDelimited(final String str, final char separatorChar)
        //      public static string reverseDelimited(string str, char separatorChar)
        //      {
        //          if (string.ReferenceEquals(str, null))
        //          {
        //              return null;
        //          }
        //          // could implement manually, but simple way is to reuse other,
        //          // probably slower, methods.
        //          //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
        //          //ORIGINAL LINE: final String[] strs = split(str, separatorChar);
        //          string[] strs = split(str, separatorChar);
        //          ArrayUtils.reverse(strs);
        //          return join(strs, separatorChar);
        //      }

        // Abbreviating
        //-----------------------------------------------------------------------
        /// <summary>
        /// <para>Abbreviates a String using ellipses. This will turn
        /// "Now is the time for all good men" into "Now is the time for..."</para>
        /// 
        /// <para>Specifically:</para>
        /// <ul>
        ///   <li>If the number of characters in {@code str} is less than or equal to 
        ///       {@code maxWidth}, return {@code str}.</li>
        ///   <li>Else abbreviate it to {@code (substring(str, 0, max-3) + "...")}.</li>
        ///   <li>If {@code maxWidth} is less than {@code 4}, throw an
        ///       {@code IllegalArgumentException}.</li>
        ///   <li>In no case will it return a String of length greater than
        ///       {@code maxWidth}.</li>
        /// </ul>
        /// 
        /// <pre>
        /// StringUtils.abbreviate(null, *)      = null
        /// StringUtils.abbreviate("", 4)        = ""
        /// StringUtils.abbreviate("abcdefg", 6) = "abc..."
        /// StringUtils.abbreviate("abcdefg", 7) = "abcdefg"
        /// StringUtils.abbreviate("abcdefg", 8) = "abcdefg"
        /// StringUtils.abbreviate("abcdefg", 4) = "a..."
        /// StringUtils.abbreviate("abcdefg", 3) = IllegalArgumentException
        /// </pre>
        /// </summary>
        /// <param name="str">  the String to check, may be null </param>
        /// <param name="maxWidth">  maximum length of result String, must be at least 4 </param>
        /// <returns> abbreviated String, {@code null} if null String input </returns>
        /// <exception cref="IllegalArgumentException"> if the width is too small
        /// @since 2.0 </exception>
        //JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
        //ORIGINAL LINE: public static String abbreviate(final String str, final int maxWidth)
        public static string abbreviate(string str, int maxWidth)
        {
            const string defaultAbbrevMarker = "...";
            return abbreviate(str, defaultAbbrevMarker, 0, maxWidth);
        }

        /// <summary>
        /// <para>Abbreviates a String using ellipses. This will turn
        /// "Now is the time for all good men" into "...is the time for..."</para>
        /// 
        /// <para>Works like {@code abbreviate(String, int)}, but allows you to specify
        /// a "left edge" offset.  Note that this left edge is not necessarily going to
        /// be the leftmost character in the result, or the first character following the
        /// ellipses, but it will appear somewhere in the result.
        /// 
        /// </para>
        /// <para>In no case will it return a String of length greater than
        /// {@code maxWidth}.</para>
        /// 
        /// <pre>
        /// StringUtils.abbreviate(null, *, *)                = null
        /// StringUtils.abbreviate("", 0, 4)                  = ""
        /// StringUtils.abbreviate("abcdefghijklmno", -1, 10) = "abcdefg..."
        /// StringUtils.abbreviate("abcdefghijklmno", 0, 10)  = "abcdefg..."
        /// StringUtils.abbreviate("abcdefghijklmno", 1, 10)  = "abcdefg..."
        /// StringUtils.abbreviate("abcdefghijklmno", 4, 10)  = "abcdefg..."
        /// StringUtils.abbreviate("abcdefghijklmno", 5, 10)  = "...fghi..."
        /// StringUtils.abbreviate("abcdefghijklmno", 6, 10)  = "...ghij..."
        /// StringUtils.abbreviate("abcdefghijklmno", 8, 10)  = "...ijklmno"
        /// StringUtils.abbreviate("abcdefghijklmno", 10, 10) = "...ijklmno"
        /// StringUtils.abbreviate("abcdefghijklmno", 12, 10) = "...ijklmno"
        /// StringUtils.abbreviate("abcdefghij", 0, 3)        = IllegalArgumentException
        /// StringUtils.abbreviate("abcdefghij", 5, 6)        = IllegalArgumentException
        /// </pre>
        /// </summary>
        /// <param name="str">  the String to check, may be null </param>
        /// <param name="offset">  left edge of source String </param>
        /// <param name="maxWidth">  maximum length of result String, must be at least 4 </param>
        /// <returns> abbreviated String, {@code null} if null String input </returns>
        /// <exception cref="IllegalArgumentException"> if the width is too small
        /// @since 2.0 </exception>
        //JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
        //ORIGINAL LINE: public static String abbreviate(final String str, final int offset, final int maxWidth)
        public static string abbreviate(string str, int offset, int maxWidth)
        {
            const string defaultAbbrevMarker = "...";
            return abbreviate(str, defaultAbbrevMarker, offset, maxWidth);
        }

        /// <summary>
        /// <para>Abbreviates a String using another given String as replacement marker. This will turn
        /// "Now is the time for all good men" into "Now is the time for..." if "..." was defined
        /// as the replacement marker.</para>
        /// 
        /// <para>Specifically:</para>
        /// <ul>
        ///   <li>If the number of characters in {@code str} is less than or equal to 
        ///       {@code maxWidth}, return {@code str}.</li>
        ///   <li>Else abbreviate it to {@code (substring(str, 0, max-abbrevMarker.length) + abbrevMarker)}.</li>
        ///   <li>If {@code maxWidth} is less than {@code abbrevMarker.length + 1}, throw an
        ///       {@code IllegalArgumentException}.</li>
        ///   <li>In no case will it return a String of length greater than
        ///       {@code maxWidth}.</li>
        /// </ul>
        /// 
        /// <pre>
        /// StringUtils.abbreviate(null, "...", *)      = null
        /// StringUtils.abbreviate("abcdefg", null, *)  = "abcdefg"
        /// StringUtils.abbreviate("", "...", 4)        = ""
        /// StringUtils.abbreviate("abcdefg", ".", 5)   = "abcd."
        /// StringUtils.abbreviate("abcdefg", ".", 7)   = "abcdefg"
        /// StringUtils.abbreviate("abcdefg", ".", 8)   = "abcdefg"
        /// StringUtils.abbreviate("abcdefg", "..", 4)  = "ab.."
        /// StringUtils.abbreviate("abcdefg", "..", 3)  = "a.."
        /// StringUtils.abbreviate("abcdefg", "..", 2)  = IllegalArgumentException
        /// StringUtils.abbreviate("abcdefg", "...", 3) = IllegalArgumentException
        /// </pre>
        /// </summary>
        /// <param name="str">  the String to check, may be null </param>
        /// <param name="abbrevMarker">  the String used as replacement marker </param>
        /// <param name="maxWidth">  maximum length of result String, must be at least {@code abbrevMarker.length + 1} </param>
        /// <returns> abbreviated String, {@code null} if null String input </returns>
        /// <exception cref="IllegalArgumentException"> if the width is too small
        /// @since 3.5 </exception>
        //JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
        //ORIGINAL LINE: public static String abbreviate(final String str, final String abbrevMarker, final int maxWidth)
        public static string abbreviate(string str, string abbrevMarker, int maxWidth)
        {
            return abbreviate(str, abbrevMarker, 0, maxWidth);
        }

        /// <summary>
        /// <para>Abbreviates a String using a given replacement marker. This will turn
        /// "Now is the time for all good men" into "...is the time for..." if "..." was defined
        /// as the replacement marker.</para>
        /// 
        /// <para>Works like {@code abbreviate(String, String, int)}, but allows you to specify
        /// a "left edge" offset.  Note that this left edge is not necessarily going to
        /// be the leftmost character in the result, or the first character following the
        /// replacement marker, but it will appear somewhere in the result.
        /// 
        /// </para>
        /// <para>In no case will it return a String of length greater than {@code maxWidth}.</para>
        /// 
        /// <pre>
        /// StringUtils.abbreviate(null, null, *, *)                 = null
        /// StringUtils.abbreviate("abcdefghijklmno", null, *, *)    = "abcdefghijklmno"
        /// StringUtils.abbreviate("", "...", 0, 4)                  = ""
        /// StringUtils.abbreviate("abcdefghijklmno", "---", -1, 10) = "abcdefg---"
        /// StringUtils.abbreviate("abcdefghijklmno", ",", 0, 10)    = "abcdefghi,"
        /// StringUtils.abbreviate("abcdefghijklmno", ",", 1, 10)    = "abcdefghi,"
        /// StringUtils.abbreviate("abcdefghijklmno", ",", 2, 10)    = "abcdefghi,"
        /// StringUtils.abbreviate("abcdefghijklmno", "::", 4, 10)   = "::efghij::"
        /// StringUtils.abbreviate("abcdefghijklmno", "...", 6, 10)  = "...ghij..."
        /// StringUtils.abbreviate("abcdefghijklmno", "*", 9, 10)    = "*ghijklmno"
        /// StringUtils.abbreviate("abcdefghijklmno", "'", 10, 10)   = "'ghijklmno"
        /// StringUtils.abbreviate("abcdefghijklmno", "!", 12, 10)   = "!ghijklmno"
        /// StringUtils.abbreviate("abcdefghij", "abra", 0, 4)       = IllegalArgumentException
        /// StringUtils.abbreviate("abcdefghij", "...", 5, 6)        = IllegalArgumentException
        /// </pre>
        /// </summary>
        /// <param name="str">  the String to check, may be null </param>
        /// <param name="abbrevMarker">  the String used as replacement marker </param>
        /// <param name="offset">  left edge of source String </param>
        /// <param name="maxWidth">  maximum length of result String, must be at least 4 </param>
        /// <returns> abbreviated String, {@code null} if null String input </returns>
        /// <exception cref="IllegalArgumentException"> if the width is too small
        /// @since 3.5 </exception>
        //JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
        //ORIGINAL LINE: public static String abbreviate(final String str, final String abbrevMarker, int offset, final int maxWidth)
        public static string abbreviate(string str, string abbrevMarker, int offset, int maxWidth)
        {
            if (isEmpty(str) || isEmpty(abbrevMarker))
            {
                return str;
            }

            //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
            //ORIGINAL LINE: final int abbrevMarkerLength = abbrevMarker.length();
            int abbrevMarkerLength = abbrevMarker.Length;
            //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
            //ORIGINAL LINE: final int minAbbrevWidth = abbrevMarkerLength + 1;
            int minAbbrevWidth = abbrevMarkerLength + 1;
            //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
            //ORIGINAL LINE: final int minAbbrevWidthOffset = abbrevMarkerLength + abbrevMarkerLength + 1;
            int minAbbrevWidthOffset = abbrevMarkerLength + abbrevMarkerLength + 1;

            if (maxWidth < minAbbrevWidth)
            {
                throw new System.ArgumentException(string.Format("Minimum abbreviation width is {0:D}", minAbbrevWidth));
            }
            if (str.Length <= maxWidth)
            {
                return str;
            }
            if (offset > str.Length)
            {
                offset = str.Length;
            }
            if (str.Length - offset < maxWidth - abbrevMarkerLength)
            {
                offset = str.Length - (maxWidth - abbrevMarkerLength);
            }
            if (offset <= abbrevMarkerLength + 1)
            {
                return str.Substring(0, maxWidth - abbrevMarkerLength) + abbrevMarker;
            }
            if (maxWidth < minAbbrevWidthOffset)
            {
                throw new System.ArgumentException(string.Format("Minimum abbreviation width with offset is {0:D}", minAbbrevWidthOffset));
            }
            if (offset + maxWidth - abbrevMarkerLength < str.Length)
            {
                return abbrevMarker + abbreviate(str.Substring(offset), abbrevMarker, maxWidth - abbrevMarkerLength);
            }
            return abbrevMarker + str.Substring(str.Length - (maxWidth - abbrevMarkerLength));
        }

        /// <summary>
        /// <para>Abbreviates a String to the length passed, replacing the middle characters with the supplied
        /// replacement String.</para>
        /// 
        /// <para>This abbreviation only occurs if the following criteria is met:</para>
        /// <ul>
        /// <li>Neither the String for abbreviation nor the replacement String are null or empty </li>
        /// <li>The length to truncate to is less than the length of the supplied String</li>
        /// <li>The length to truncate to is greater than 0</li>
        /// <li>The abbreviated String will have enough room for the length supplied replacement String
        /// and the first and last characters of the supplied String for abbreviation</li>
        /// </ul>
        /// <para>Otherwise, the returned String will be the same as the supplied String for abbreviation.
        /// </para>
        /// 
        /// <pre>
        /// StringUtils.abbreviateMiddle(null, null, 0)      = null
        /// StringUtils.abbreviateMiddle("abc", null, 0)      = "abc"
        /// StringUtils.abbreviateMiddle("abc", ".", 0)      = "abc"
        /// StringUtils.abbreviateMiddle("abc", ".", 3)      = "abc"
        /// StringUtils.abbreviateMiddle("abcdef", ".", 4)     = "ab.f"
        /// </pre>
        /// </summary>
        /// <param name="str">  the String to abbreviate, may be null </param>
        /// <param name="middle"> the String to replace the middle characters with, may be null </param>
        /// <param name="length"> the length to abbreviate {@code str} to. </param>
        /// <returns> the abbreviated String if the above criteria is met, or the original String supplied for abbreviation.
        /// @since 2.5 </returns>
        //JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
        //ORIGINAL LINE: public static String abbreviateMiddle(final String str, final String middle, final int length)
        public static string abbreviateMiddle(string str, string middle, int length)
        {
            if (isEmpty(str) || isEmpty(middle))
            {
                return str;
            }

            if (length >= str.Length || length < middle.Length + 2)
            {
                return str;
            }

            //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
            //ORIGINAL LINE: final int targetSting = length-middle.length();
            int targetSting = length - middle.Length;
            //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
            //ORIGINAL LINE: final int startOffset = targetSting/2+targetSting%2;
            int startOffset = targetSting / 2 + targetSting % 2;
            //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
            //ORIGINAL LINE: final int endOffset = str.length()-targetSting/2;
            int endOffset = str.Length - targetSting / 2;

            //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
            //ORIGINAL LINE: final StringBuilder builder = new StringBuilder(length);
            StringBuilder builder = new StringBuilder(length);
            builder.Append(str.Substring(0, startOffset));
            builder.Append(middle);
            builder.Append(str.Substring(endOffset));

            return builder.ToString();
        }

        //      // Difference
        //      //-----------------------------------------------------------------------
        //      /// <summary>
        //      /// <para>Compares two Strings, and returns the portion where they differ.
        //      /// More precisely, return the remainder of the second String,
        //      /// starting from where it's different from the first. This means that
        //      /// the difference between "abc" and "ab" is the empty String and not "c". </para>
        //      /// 
        //      /// <para>For example,
        //      /// {@code difference("i am a machine", "i am a robot") -> "robot"}.</para>
        //      /// 
        //      /// <pre>
        //      /// StringUtils.difference(null, null) = null
        //      /// StringUtils.difference("", "") = ""
        //      /// StringUtils.difference("", "abc") = "abc"
        //      /// StringUtils.difference("abc", "") = ""
        //      /// StringUtils.difference("abc", "abc") = ""
        //      /// StringUtils.difference("abc", "ab") = ""
        //      /// StringUtils.difference("ab", "abxyz") = "xyz"
        //      /// StringUtils.difference("abcde", "abxyz") = "xyz"
        //      /// StringUtils.difference("abcde", "xyz") = "xyz"
        //      /// </pre>
        //      /// </summary>
        //      /// <param name="str1">  the first String, may be null </param>
        //      /// <param name="str2">  the second String, may be null </param>
        //      /// <returns> the portion of str2 where it differs from str1; returns the
        //      /// empty String if they are equal </returns>
        //      /// <seealso cref= #indexOfDifference(CharSequence,CharSequence)
        //      /// @since 2.0 </seealso>
        //      //JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
        //      //ORIGINAL LINE: public static String difference(final String str1, final String str2)
        //      public static string difference(string str1, string str2)
        //      {
        //          if (string.ReferenceEquals(str1, null))
        //          {
        //              return str2;
        //          }
        //          if (string.ReferenceEquals(str2, null))
        //          {
        //              return str1;
        //          }
        //          //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
        //          //ORIGINAL LINE: final int at = indexOfDifference(str1, str2);
        //          int at = indexOfDifference(str1, str2);
        //          if (at == INDEX_NOT_FOUND)
        //          {
        //              return EMPTY;
        //          }
        //          return str2.Substring(at);
        //      }

        //      /// <summary>
        //      /// <para>Compares two CharSequences, and returns the index at which the
        //      /// CharSequences begin to differ.</para>
        //      /// 
        //      /// <para>For example,
        //      /// {@code indexOfDifference("i am a machine", "i am a robot") -> 7}</para>
        //      /// 
        //      /// <pre>
        //      /// StringUtils.indexOfDifference(null, null) = -1
        //      /// StringUtils.indexOfDifference("", "") = -1
        //      /// StringUtils.indexOfDifference("", "abc") = 0
        //      /// StringUtils.indexOfDifference("abc", "") = 0
        //      /// StringUtils.indexOfDifference("abc", "abc") = -1
        //      /// StringUtils.indexOfDifference("ab", "abxyz") = 2
        //      /// StringUtils.indexOfDifference("abcde", "abxyz") = 2
        //      /// StringUtils.indexOfDifference("abcde", "xyz") = 0
        //      /// </pre>
        //      /// </summary>
        //      /// <param name="cs1">  the first CharSequence, may be null </param>
        //      /// <param name="cs2">  the second CharSequence, may be null </param>
        //      /// <returns> the index where cs1 and cs2 begin to differ; -1 if they are equal
        //      /// @since 2.0
        //      /// @since 3.0 Changed signature from indexOfDifference(String, String) to
        //      /// indexOfDifference(CharSequence, CharSequence) </returns>
        //      //JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
        //      //ORIGINAL LINE: public static int indexOfDifference(final CharSequence cs1, final CharSequence cs2)
        //      public static int indexOfDifference(CharSequence cs1, CharSequence cs2)
        //      {
        //          if (cs1 == cs2)
        //          {
        //              return INDEX_NOT_FOUND;
        //          }
        //          if (cs1 == null || cs2 == null)
        //          {
        //              return 0;
        //          }
        //          int i;
        //          for (i = 0; i < cs1.length() && i < cs2.length(); ++i)
        //          {
        //              if (cs1.charAt(i) != cs2.charAt(i))
        //              {
        //                  break;
        //              }
        //          }
        //          if (i < cs2.length() || i < cs1.length())
        //          {
        //              return i;
        //          }
        //          return INDEX_NOT_FOUND;
        //      }

        //      /// <summary>
        //      /// <para>Compares all CharSequences in an array and returns the index at which the
        //      /// CharSequences begin to differ.</para>
        //      /// 
        //      /// <para>For example,
        //      /// <code>indexOfDifference(new String[] {"i am a machine", "i am a robot"}) -&gt; 7</code></para>
        //      /// 
        //      /// <pre>
        //      /// StringUtils.indexOfDifference(null) = -1
        //      /// StringUtils.indexOfDifference(new String[] {}) = -1
        //      /// StringUtils.indexOfDifference(new String[] {"abc"}) = -1
        //      /// StringUtils.indexOfDifference(new String[] {null, null}) = -1
        //      /// StringUtils.indexOfDifference(new String[] {"", ""}) = -1
        //      /// StringUtils.indexOfDifference(new String[] {"", null}) = 0
        //      /// StringUtils.indexOfDifference(new String[] {"abc", null, null}) = 0
        //      /// StringUtils.indexOfDifference(new String[] {null, null, "abc"}) = 0
        //      /// StringUtils.indexOfDifference(new String[] {"", "abc"}) = 0
        //      /// StringUtils.indexOfDifference(new String[] {"abc", ""}) = 0
        //      /// StringUtils.indexOfDifference(new String[] {"abc", "abc"}) = -1
        //      /// StringUtils.indexOfDifference(new String[] {"abc", "a"}) = 1
        //      /// StringUtils.indexOfDifference(new String[] {"ab", "abxyz"}) = 2
        //      /// StringUtils.indexOfDifference(new String[] {"abcde", "abxyz"}) = 2
        //      /// StringUtils.indexOfDifference(new String[] {"abcde", "xyz"}) = 0
        //      /// StringUtils.indexOfDifference(new String[] {"xyz", "abcde"}) = 0
        //      /// StringUtils.indexOfDifference(new String[] {"i am a machine", "i am a robot"}) = 7
        //      /// </pre>
        //      /// </summary>
        //      /// <param name="css">  array of CharSequences, entries may be null </param>
        //      /// <returns> the index where the strings begin to differ; -1 if they are all equal
        //      /// @since 2.4
        //      /// @since 3.0 Changed signature from indexOfDifference(String...) to indexOfDifference(CharSequence...) </returns>
        //      //JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
        //      //ORIGINAL LINE: public static int indexOfDifference(final CharSequence... css)
        //      public static int indexOfDifference(params CharSequence[] css)
        //      {
        //          if (css == null || css.Length <= 1)
        //          {
        //              return INDEX_NOT_FOUND;
        //          }
        //          bool anyStringNull = false;
        //          bool allStringsNull = true;
        //          //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
        //          //ORIGINAL LINE: final int arrayLen = css.length;
        //          int arrayLen = css.Length;
        //          int shortestStrLen = int.MaxValue;
        //          int longestStrLen = 0;

        //          // find the min and max string lengths; this avoids checking to make
        //          // sure we are not exceeding the length of the string each time through
        //          // the bottom loop.
        //          for (int i = 0; i < arrayLen; i++)
        //          {
        //              if (css[i] == null)
        //              {
        //                  anyStringNull = true;
        //                  shortestStrLen = 0;
        //              }
        //              else
        //              {
        //                  allStringsNull = false;
        //                  shortestStrLen = Math.Min(css[i].length(), shortestStrLen);
        //                  longestStrLen = Math.Max(css[i].length(), longestStrLen);
        //              }
        //          }

        //          // handle lists containing all nulls or all empty strings
        //          if (allStringsNull || longestStrLen == 0 && !anyStringNull)
        //          {
        //              return INDEX_NOT_FOUND;
        //          }

        //          // handle lists containing some nulls or some empty strings
        //          if (shortestStrLen == 0)
        //          {
        //              return 0;
        //          }

        //          // find the position with the first difference across all strings
        //          int firstDiff = -1;
        //          for (int stringPos = 0; stringPos < shortestStrLen; stringPos++)
        //          {
        //              //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
        //              //ORIGINAL LINE: final char comparisonChar = css[0].charAt(stringPos);
        //              char comparisonChar = css[0].charAt(stringPos);
        //              for (int arrayPos = 1; arrayPos < arrayLen; arrayPos++)
        //              {
        //                  if (css[arrayPos].charAt(stringPos) != comparisonChar)
        //                  {
        //                      firstDiff = stringPos;
        //                      break;
        //                  }
        //              }
        //              if (firstDiff != -1)
        //              {
        //                  break;
        //              }
        //          }

        //          if (firstDiff == -1 && shortestStrLen != longestStrLen)
        //          {
        //              // we compared all of the characters up to the length of the
        //              // shortest string and didn't find a match, but the string lengths
        //              // vary, so return the length of the shortest string.
        //              return shortestStrLen;
        //          }
        //          return firstDiff;
        //      }

        //      /// <summary>
        //      /// <para>Compares all Strings in an array and returns the initial sequence of
        //      /// characters that is common to all of them.</para>
        //      /// 
        //      /// <para>For example,
        //      /// <code>getCommonPrefix(new String[] {"i am a machine", "i am a robot"}) -&gt; "i am a "</code></para>
        //      /// 
        //      /// <pre>
        //      /// StringUtils.getCommonPrefix(null) = ""
        //      /// StringUtils.getCommonPrefix(new String[] {}) = ""
        //      /// StringUtils.getCommonPrefix(new String[] {"abc"}) = "abc"
        //      /// StringUtils.getCommonPrefix(new String[] {null, null}) = ""
        //      /// StringUtils.getCommonPrefix(new String[] {"", ""}) = ""
        //      /// StringUtils.getCommonPrefix(new String[] {"", null}) = ""
        //      /// StringUtils.getCommonPrefix(new String[] {"abc", null, null}) = ""
        //      /// StringUtils.getCommonPrefix(new String[] {null, null, "abc"}) = ""
        //      /// StringUtils.getCommonPrefix(new String[] {"", "abc"}) = ""
        //      /// StringUtils.getCommonPrefix(new String[] {"abc", ""}) = ""
        //      /// StringUtils.getCommonPrefix(new String[] {"abc", "abc"}) = "abc"
        //      /// StringUtils.getCommonPrefix(new String[] {"abc", "a"}) = "a"
        //      /// StringUtils.getCommonPrefix(new String[] {"ab", "abxyz"}) = "ab"
        //      /// StringUtils.getCommonPrefix(new String[] {"abcde", "abxyz"}) = "ab"
        //      /// StringUtils.getCommonPrefix(new String[] {"abcde", "xyz"}) = ""
        //      /// StringUtils.getCommonPrefix(new String[] {"xyz", "abcde"}) = ""
        //      /// StringUtils.getCommonPrefix(new String[] {"i am a machine", "i am a robot"}) = "i am a "
        //      /// </pre>
        //      /// </summary>
        //      /// <param name="strs">  array of String objects, entries may be null </param>
        //      /// <returns> the initial sequence of characters that are common to all Strings
        //      /// in the array; empty String if the array is null, the elements are all null
        //      /// or if there is no common prefix.
        //      /// @since 2.4 </returns>
        //      //JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
        //      //ORIGINAL LINE: public static String getCommonPrefix(final String... strs)
        //      public static string getCommonPrefix(params string[] strs)
        //      {
        //          if (strs == null || strs.Length == 0)
        //          {
        //              return EMPTY;
        //          }
        //          //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
        //          //ORIGINAL LINE: final int smallestIndexOfDiff = indexOfDifference(strs);
        //          int smallestIndexOfDiff = indexOfDifference(strs);
        //          if (smallestIndexOfDiff == INDEX_NOT_FOUND)
        //          {
        //              // all strings were identical
        //              if (string.ReferenceEquals(strs[0], null))
        //              {
        //                  return EMPTY;
        //              }
        //              return strs[0];
        //          }
        //          else if (smallestIndexOfDiff == 0)
        //          {
        //              // there were no common initial characters
        //              return EMPTY;
        //          }
        //          else
        //          {
        //              // we found a common initial character sequence
        //              return strs[0].Substring(0, smallestIndexOfDiff);
        //          }
        //      }

        //      // Misc
        //      //-----------------------------------------------------------------------
        //      /// <summary>
        //      /// <para>Find the Levenshtein distance between two Strings.</para>
        //      /// 
        //      /// <para>This is the number of changes needed to change one String into
        //      /// another, where each change is a single character modification (deletion,
        //      /// insertion or substitution).</para>
        //      /// 
        //      /// <para>The implementation uses a single-dimensional array of length s.length() + 1. See 
        //      /// <a href="http://blog.softwx.net/2014/12/optimizing-levenshtein-algorithm-in-c.html">
        //      /// http://blog.softwx.net/2014/12/optimizing-levenshtein-algorithm-in-c.html</a> for details.</para>
        //      /// 
        //      /// <pre>
        //      /// StringUtils.getLevenshteinDistance(null, *)             = IllegalArgumentException
        //      /// StringUtils.getLevenshteinDistance(*, null)             = IllegalArgumentException
        //      /// StringUtils.getLevenshteinDistance("","")               = 0
        //      /// StringUtils.getLevenshteinDistance("","a")              = 1
        //      /// StringUtils.getLevenshteinDistance("aaapppp", "")       = 7
        //      /// StringUtils.getLevenshteinDistance("frog", "fog")       = 1
        //      /// StringUtils.getLevenshteinDistance("fly", "ant")        = 3
        //      /// StringUtils.getLevenshteinDistance("elephant", "hippo") = 7
        //      /// StringUtils.getLevenshteinDistance("hippo", "elephant") = 7
        //      /// StringUtils.getLevenshteinDistance("hippo", "zzzzzzzz") = 8
        //      /// StringUtils.getLevenshteinDistance("hello", "hallo")    = 1
        //      /// </pre>
        //      /// </summary>
        //      /// <param name="s">  the first String, must not be null </param>
        //      /// <param name="t">  the second String, must not be null </param>
        //      /// <returns> result distance </returns>
        //      /// <exception cref="IllegalArgumentException"> if either String input {@code null}
        //      /// @since 3.0 Changed signature from getLevenshteinDistance(String, String) to
        //      /// getLevenshteinDistance(CharSequence, CharSequence) </exception>
        //      public static int getLevenshteinDistance(CharSequence s, CharSequence t)
        //      {
        //          if (s == null || t == null)
        //          {
        //              throw new System.ArgumentException("Strings must not be null");
        //          }

        //          int n = s.length();
        //          int m = t.length();

        //          if (n == 0)
        //          {
        //              return m;
        //          }
        //          else if (m == 0)
        //          {
        //              return n;
        //          }

        //          if (n > m)
        //          {
        //              // swap the input strings to consume less memory
        //              //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
        //              //ORIGINAL LINE: final CharSequence tmp = s;
        //              CharSequence tmp = s;
        //              s = t;
        //              t = tmp;
        //              n = m;
        //              m = t.length();
        //          }

        //          //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
        //          //ORIGINAL LINE: final int p[] = new int[n + 1];
        //          int[] p = new int[n + 1];
        //          // indexes into strings s and t
        //          int i; // iterates through s
        //          int j; // iterates through t
        //          int upper_left;
        //          int upper;

        //          char t_j; // jth character of t
        //          int cost;

        //          for (i = 0; i <= n; i++)
        //          {
        //              p[i] = i;
        //          }

        //          for (j = 1; j <= m; j++)
        //          {
        //              upper_left = p[0];
        //              t_j = t.charAt(j - 1);
        //              p[0] = j;

        //              for (i = 1; i <= n; i++)
        //              {
        //                  upper = p[i];
        //                  cost = s.charAt(i - 1) == t_j ? 0 : 1;
        //                  // minimum of cell to the left+1, to the top+1, diagonally left and up +cost
        //                  p[i] = Math.Min(Math.Min(p[i - 1] + 1, p[i] + 1), upper_left + cost);
        //                  upper_left = upper;
        //              }
        //          }

        //          return p[n];
        //      }

        //      /// <summary>
        //      /// <para>Find the Levenshtein distance between two Strings if it's less than or equal to a given
        //      /// threshold.</para>
        //      /// 
        //      /// <para>This is the number of changes needed to change one String into
        //      /// another, where each change is a single character modification (deletion,
        //      /// insertion or substitution).</para>
        //      /// 
        //      /// <para>This implementation follows from Algorithms on Strings, Trees and Sequences by Dan Gusfield
        //      /// and Chas Emerick's implementation of the Levenshtein distance algorithm from
        //      /// <a href="http://www.merriampark.com/ld.htm">http://www.merriampark.com/ld.htm</a></para>
        //      /// 
        //      /// <pre>
        //      /// StringUtils.getLevenshteinDistance(null, *, *)             = IllegalArgumentException
        //      /// StringUtils.getLevenshteinDistance(*, null, *)             = IllegalArgumentException
        //      /// StringUtils.getLevenshteinDistance(*, *, -1)               = IllegalArgumentException
        //      /// StringUtils.getLevenshteinDistance("","", 0)               = 0
        //      /// StringUtils.getLevenshteinDistance("aaapppp", "", 8)       = 7
        //      /// StringUtils.getLevenshteinDistance("aaapppp", "", 7)       = 7
        //      /// StringUtils.getLevenshteinDistance("aaapppp", "", 6))      = -1
        //      /// StringUtils.getLevenshteinDistance("elephant", "hippo", 7) = 7
        //      /// StringUtils.getLevenshteinDistance("elephant", "hippo", 6) = -1
        //      /// StringUtils.getLevenshteinDistance("hippo", "elephant", 7) = 7
        //      /// StringUtils.getLevenshteinDistance("hippo", "elephant", 6) = -1
        //      /// </pre>
        //      /// </summary>
        //      /// <param name="s">  the first String, must not be null </param>
        //      /// <param name="t">  the second String, must not be null </param>
        //      /// <param name="threshold"> the target threshold, must not be negative </param>
        //      /// <returns> result distance, or {@code -1} if the distance would be greater than the threshold </returns>
        //      /// <exception cref="IllegalArgumentException"> if either String input {@code null} or negative threshold </exception>
        //      //JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
        //      //ORIGINAL LINE: public static int getLevenshteinDistance(CharSequence s, CharSequence t, final int threshold)
        //      public static int getLevenshteinDistance(CharSequence s, CharSequence t, int threshold)
        //      {
        //          if (s == null || t == null)
        //          {
        //              throw new System.ArgumentException("Strings must not be null");
        //          }
        //          if (threshold < 0)
        //          {
        //              throw new System.ArgumentException("Threshold must not be negative");
        //          }

        //          /*
        //	This implementation only computes the distance if it's less than or equal to the
        //	threshold value, returning -1 if it's greater.  The advantage is performance: unbounded
        //	distance is O(nm), but a bound of k allows us to reduce it to O(km) time by only
        //	computing a diagonal stripe of width 2k + 1 of the cost table.
        //	It is also possible to use this to compute the unbounded Levenshtein distance by starting
        //	the threshold at 1 and doubling each time until the distance is found; this is O(dm), where
        //	d is the distance.

        //	One subtlety comes from needing to ignore entries on the border of our stripe
        //	eg.
        //	p[] = |#|#|#|*
        //	d[] =  *|#|#|#|
        //	We must ignore the entry to the left of the leftmost member
        //	We must ignore the entry above the rightmost member

        //	Another subtlety comes from our stripe running off the matrix if the strings aren't
        //	of the same size.  Since string s is always swapped to be the shorter of the two,
        //	the stripe will always run off to the upper right instead of the lower left of the matrix.

        //	As a concrete example, suppose s is of length 5, t is of length 7, and our threshold is 1.
        //	In this case we're going to walk a stripe of length 3.  The matrix would look like so:

        //	   1 2 3 4 5
        //	1 |#|#| | | |
        //	2 |#|#|#| | |
        //	3 | |#|#|#| |
        //	4 | | |#|#|#|
        //	5 | | | |#|#|
        //	6 | | | | |#|
        //	7 | | | | | |

        //	Note how the stripe leads off the table as there is no possible way to turn a string of length 5
        //	into one of length 7 in edit distance of 1.

        //	Additionally, this implementation decreases memory usage by using two
        //	single-dimensional arrays and swapping them back and forth instead of allocating
        //	an entire n by m matrix.  This requires a few minor changes, such as immediately returning
        //	when it's detected that the stripe has run off the matrix and initially filling the arrays with
        //	large values so that entries we don't compute are ignored.

        //	See Algorithms on Strings, Trees and Sequences by Dan Gusfield for some discussion.
        //	 */

        //          int n = s.length(); // length of s
        //          int m = t.length(); // length of t

        //          // if one string is empty, the edit distance is necessarily the length of the other
        //          if (n == 0)
        //          {
        //              return m <= threshold ? m : -1;
        //          }
        //          else if (m == 0)
        //          {
        //              return n <= threshold ? n : -1;
        //          }
        //          // no need to calculate the distance if the length difference is greater than the threshold
        //          else if (Math.Abs(n - m) > threshold)
        //          {
        //              return -1;
        //          }

        //          if (n > m)
        //          {
        //              // swap the two strings to consume less memory
        //              //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
        //              //ORIGINAL LINE: final CharSequence tmp = s;
        //              CharSequence tmp = s;
        //              s = t;
        //              t = tmp;
        //              n = m;
        //              m = t.length();
        //          }

        //          int[] p = new int[n + 1]; // 'previous' cost array, horizontally
        //          int[] d = new int[n + 1]; // cost array, horizontally
        //          int[] _d; // placeholder to assist in swapping p and d

        //          // fill in starting table values
        //          //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
        //          //ORIGINAL LINE: final int boundary = Math.min(n, threshold) + 1;
        //          int boundary = Math.Min(n, threshold) + 1;
        //          for (int i = 0; i < boundary; i++)
        //          {
        //              p[i] = i;
        //          }
        //          // these fills ensure that the value above the rightmost entry of our
        //          // stripe will be ignored in following loop iterations
        //          Arrays.fill(p, boundary, p.Length, int.MaxValue);
        //          Arrays.fill(d, int.MaxValue);

        //          // iterates through t
        //          for (int j = 1; j <= m; j++)
        //          {
        //              //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
        //              //ORIGINAL LINE: final char t_j = t.charAt(j - 1);
        //              char t_j = t.charAt(j - 1); // jth character of t
        //              d[0] = j;

        //              // compute stripe indices, constrain to array size
        //              //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
        //              //ORIGINAL LINE: final int min = Math.max(1, j - threshold);
        //              int min = Math.Max(1, j - threshold);
        //              //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
        //              //ORIGINAL LINE: final int max = j > Integer.MAX_VALUE - threshold ? n : Math.min(n, j + threshold);
        //              int max = j > int.MaxValue - threshold ? n : Math.Min(n, j + threshold);

        //              // the stripe may lead off of the table if s and t are of different sizes
        //              if (min > max)
        //              {
        //                  return -1;
        //              }

        //              // ignore entry left of leftmost
        //              if (min > 1)
        //              {
        //                  d[min - 1] = int.MaxValue;
        //              }

        //              // iterates through [min, max] in s
        //              for (int i = min; i <= max; i++)
        //              {
        //                  if (s.charAt(i - 1) == t_j)
        //                  {
        //                      // diagonally left and up
        //                      d[i] = p[i - 1];
        //                  }
        //                  else
        //                  {
        //                      // 1 + minimum of cell to the left, to the top, diagonally left and up
        //                      d[i] = 1 + Math.Min(Math.Min(d[i - 1], p[i]), p[i - 1]);
        //                  }
        //              }

        //              // copy current distance counts to 'previous row' distance counts
        //              _d = p;
        //              p = d;
        //              d = _d;
        //          }

        //          // if p[n] is greater than the threshold, there's no guarantee on it being the correct
        //          // distance
        //          if (p[n] <= threshold)
        //          {
        //              return p[n];
        //          }
        //          return -1;
        //      }

        //      /// <summary>
        //      /// <para>Find the Jaro Winkler Distance which indicates the similarity score between two Strings.</para>
        //      /// 
        //      /// <para>The Jaro measure is the weighted sum of percentage of matched characters from each file and transposed characters. 
        //      /// Winkler increased this measure for matching initial characters.</para>
        //      /// 
        //      /// <para>This implementation is based on the Jaro Winkler similarity algorithm
        //      /// from <a href="http://en.wikipedia.org/wiki/Jaro%E2%80%93Winkler_distance">http://en.wikipedia.org/wiki/Jaro%E2%80%93Winkler_distance</a>.</para>
        //      /// 
        //      /// <pre>
        //      /// StringUtils.getJaroWinklerDistance(null, null)          = IllegalArgumentException
        //      /// StringUtils.getJaroWinklerDistance("","")               = 0.0
        //      /// StringUtils.getJaroWinklerDistance("","a")              = 0.0
        //      /// StringUtils.getJaroWinklerDistance("aaapppp", "")       = 0.0
        //      /// StringUtils.getJaroWinklerDistance("frog", "fog")       = 0.93
        //      /// StringUtils.getJaroWinklerDistance("fly", "ant")        = 0.0
        //      /// StringUtils.getJaroWinklerDistance("elephant", "hippo") = 0.44
        //      /// StringUtils.getJaroWinklerDistance("hippo", "elephant") = 0.44
        //      /// StringUtils.getJaroWinklerDistance("hippo", "zzzzzzzz") = 0.0
        //      /// StringUtils.getJaroWinklerDistance("hello", "hallo")    = 0.88
        //      /// StringUtils.getJaroWinklerDistance("ABC Corporation", "ABC Corp") = 0.93
        //      /// StringUtils.getJaroWinklerDistance("D N H Enterprises Inc", "D &amp; H Enterprises, Inc.") = 0.95
        //      /// StringUtils.getJaroWinklerDistance("My Gym Children's Fitness Center", "My Gym. Childrens Fitness") = 0.92
        //      /// StringUtils.getJaroWinklerDistance("PENNSYLVANIA", "PENNCISYLVNIA") = 0.88
        //      /// </pre>
        //      /// </summary>
        //      /// <param name="first"> the first String, must not be null </param>
        //      /// <param name="second"> the second String, must not be null </param>
        //      /// <returns> result distance </returns>
        //      /// <exception cref="IllegalArgumentException"> if either String input {@code null}
        //      /// @since 3.3 </exception>
        //      /// @deprecated as of 3.6, due to a misleading name, use <seealso cref="#getJaroWinklerSimilarity(CharSequence, CharSequence)"/> instead 
        //      //JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
        //      //ORIGINAL LINE: @Deprecated("as of 3.6, due to a misleading name, use <seealso cref="#getJaroWinklerSimilarity(CharSequence, CharSequence)"/> instead") public static double getJaroWinklerDistance(final CharSequence first, final CharSequence second)
        //      //[Obsolete("as of 3.6, due to a misleading name, use <seealso cref="#getJaroWinklerSimilarity(CharSequence, CharSequence)"/> instead")]
        //public static double getJaroWinklerDistance(CharSequence first, CharSequence second)
        //      {
        //          const double DEFAULT_SCALING_FACTOR = 0.1;

        //          if (first == null || second == null)
        //          {
        //              throw new System.ArgumentException("Strings must not be null");
        //          }

        //          //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
        //          //ORIGINAL LINE: final int[] mtp = matches(first, second);
        //          int[] mtp = matches(first, second);
        //          //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
        //          //ORIGINAL LINE: final double m = mtp[0];
        //          double m = mtp[0];
        //          if (m == 0)
        //          {
        //              return 0D;
        //          }
        //          //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
        //          //ORIGINAL LINE: final double j = ((m / first.length() + m / second.length() + (m - mtp[1]) / m)) / 3;
        //          double j = ((m / first.length() + m / second.length() + (m - mtp[1]) / m)) / 3;
        //          //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
        //          //ORIGINAL LINE: final double jw = j < 0.7D ? j : j + Math.min(DEFAULT_SCALING_FACTOR, 1D / mtp[3]) * mtp[2] * (1D - j);
        //          double jw = j < 0.7D ? j : j + Math.Min(DEFAULT_SCALING_FACTOR, 1D / mtp[3]) * mtp[2] * (1D - j);
        //          return Math.Round(jw * 100.0D) / 100.0D;
        //      }

        //      /// <summary>
        //      /// <para>Find the Jaro Winkler Similarity which indicates the similarity score between two Strings.</para>
        //      /// 
        //      /// <para>The Jaro measure is the weighted sum of percentage of matched characters from each file and transposed characters. 
        //      /// Winkler increased this measure for matching initial characters.</para>
        //      /// 
        //      /// <para>This implementation is based on the Jaro Winkler similarity algorithm
        //      /// from <a href="http://en.wikipedia.org/wiki/Jaro%E2%80%93Winkler_distance">http://en.wikipedia.org/wiki/Jaro%E2%80%93Winkler_distance</a>.</para>
        //      /// 
        //      /// <pre>
        //      /// StringUtils.getJaroWinklerSimilarity(null, null)          = IllegalArgumentException
        //      /// StringUtils.getJaroWinklerSimilarity("","")               = 0.0
        //      /// StringUtils.getJaroWinklerSimilarity("","a")              = 0.0
        //      /// StringUtils.getJaroWinklerSimilarity("aaapppp", "")       = 0.0
        //      /// StringUtils.getJaroWinklerSimilarity("frog", "fog")       = 0.93
        //      /// StringUtils.getJaroWinklerSimilarity("fly", "ant")        = 0.0
        //      /// StringUtils.getJaroWinklerSimilarity("elephant", "hippo") = 0.44
        //      /// StringUtils.getJaroWinklerSimilarity("hippo", "elephant") = 0.44
        //      /// StringUtils.getJaroWinklerSimilarity("hippo", "zzzzzzzz") = 0.0
        //      /// StringUtils.getJaroWinklerSimilarity("hello", "hallo")    = 0.88
        //      /// StringUtils.getJaroWinklerSimilarity("ABC Corporation", "ABC Corp") = 0.93
        //      /// StringUtils.getJaroWinklerSimilarity("D N H Enterprises Inc", "D &amp; H Enterprises, Inc.") = 0.95
        //      /// StringUtils.getJaroWinklerSimilarity("My Gym Children's Fitness Center", "My Gym. Childrens Fitness") = 0.92
        //      /// StringUtils.getJaroWinklerSimilarity("PENNSYLVANIA", "PENNCISYLVNIA") = 0.88
        //      /// </pre>
        //      /// </summary>
        //      /// <param name="first"> the first String, must not be null </param>
        //      /// <param name="second"> the second String, must not be null </param>
        //      /// <returns> result similarity </returns>
        //      /// <exception cref="IllegalArgumentException"> if either String input {@code null}
        //      /// @since 3.6 </exception>
        //      //JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
        //      //ORIGINAL LINE: public static double getJaroWinklerSimilarity(final CharSequence first, final CharSequence second)
        //      public static double getJaroWinklerSimilarity(CharSequence first, CharSequence second)
        //      {
        //          const double DEFAULT_SCALING_FACTOR = 0.1;

        //          if (first == null || second == null)
        //          {
        //              throw new System.ArgumentException("Strings must not be null");
        //          }

        //          //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
        //          //ORIGINAL LINE: final int[] mtp = matches(first, second);
        //          int[] mtp = matches(first, second);
        //          //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
        //          //ORIGINAL LINE: final double m = mtp[0];
        //          double m = mtp[0];
        //          if (m == 0)
        //          {
        //              return 0D;
        //          }
        //          //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
        //          //ORIGINAL LINE: final double j = ((m / first.length() + m / second.length() + (m - mtp[1]) / m)) / 3;
        //          double j = ((m / first.length() + m / second.length() + (m - mtp[1]) / m)) / 3;
        //          //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
        //          //ORIGINAL LINE: final double jw = j < 0.7D ? j : j + Math.min(DEFAULT_SCALING_FACTOR, 1D / mtp[3]) * mtp[2] * (1D - j);
        //          double jw = j < 0.7D ? j : j + Math.Min(DEFAULT_SCALING_FACTOR, 1D / mtp[3]) * mtp[2] * (1D - j);
        //          return Math.Round(jw * 100.0D) / 100.0D;
        //      }

        //      //JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
        //      //ORIGINAL LINE: private static int[] matches(final CharSequence first, final CharSequence second)
        //      private static int[] matches(CharSequence first, CharSequence second)
        //      {
        //          CharSequence max, min;
        //          if (first.length() > second.length())
        //          {
        //              max = first;
        //              min = second;
        //          }
        //          else
        //          {
        //              max = second;
        //              min = first;
        //          }
        //          //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
        //          //ORIGINAL LINE: final int range = Math.max(max.length() / 2 - 1, 0);
        //          int range = Math.Max(max.length() / 2 - 1, 0);
        //          //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
        //          //ORIGINAL LINE: final int[] matchIndexes = new int[min.length()];
        //          int[] matchIndexes = new int[min.length()];
        //          Arrays.fill(matchIndexes, -1);
        //          //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
        //          //ORIGINAL LINE: final boolean[] matchFlags = new boolean[max.length()];
        //          bool[] matchFlags = new bool[max.length()];
        //          int matches = 0;
        //          for (int mi = 0; mi < min.length(); mi++)
        //          {
        //              //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
        //              //ORIGINAL LINE: final char c1 = min.charAt(mi);
        //              char c1 = min.charAt(mi);
        //              for (int xi = Math.Max(mi - range, 0), xn = Math.Min(mi + range + 1, max.length()); xi < xn; xi++)
        //              {
        //                  if (!matchFlags[xi] && c1 == max.charAt(xi))
        //                  {
        //                      matchIndexes[mi] = xi;
        //                      matchFlags[xi] = true;
        //                      matches++;
        //                      break;
        //                  }
        //              }
        //          }
        //          //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
        //          //ORIGINAL LINE: final char[] ms1 = new char[matches];
        //          char[] ms1 = new char[matches];
        //          //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
        //          //ORIGINAL LINE: final char[] ms2 = new char[matches];
        //          char[] ms2 = new char[matches];
        //          for (int i = 0, si = 0; i < min.length(); i++)
        //          {
        //              if (matchIndexes[i] != -1)
        //              {
        //                  ms1[si] = min.charAt(i);
        //                  si++;
        //              }
        //          }
        //          for (int i = 0, si = 0; i < max.length(); i++)
        //          {
        //              if (matchFlags[i])
        //              {
        //                  ms2[si] = max.charAt(i);
        //                  si++;
        //              }
        //          }
        //          int transpositions = 0;
        //          for (int mi = 0; mi < ms1.Length; mi++)
        //          {
        //              if (ms1[mi] != ms2[mi])
        //              {
        //                  transpositions++;
        //              }
        //          }
        //          int prefix = 0;
        //          for (int mi = 0; mi < min.length(); mi++)
        //          {
        //              if (first.charAt(mi) == second.charAt(mi))
        //              {
        //                  prefix++;
        //              }
        //              else
        //              {
        //                  break;
        //              }
        //          }
        //          return new int[] { matches, transpositions / 2, prefix, max.length() };
        //      }

        //      /// <summary>
        //      /// <para>Find the Fuzzy Distance which indicates the similarity score between two Strings.</para>
        //      /// 
        //      /// <para>This string matching algorithm is similar to the algorithms of editors such as Sublime Text,
        //      /// TextMate, Atom and others. One point is given for every matched character. Subsequent
        //      /// matches yield two bonus points. A higher score indicates a higher similarity.</para>
        //      /// 
        //      /// <pre>
        //      /// StringUtils.getFuzzyDistance(null, null, null)                                    = IllegalArgumentException
        //      /// StringUtils.getFuzzyDistance("", "", Locale.ENGLISH)                              = 0
        //      /// StringUtils.getFuzzyDistance("Workshop", "b", Locale.ENGLISH)                     = 0
        //      /// StringUtils.getFuzzyDistance("Room", "o", Locale.ENGLISH)                         = 1
        //      /// StringUtils.getFuzzyDistance("Workshop", "w", Locale.ENGLISH)                     = 1
        //      /// StringUtils.getFuzzyDistance("Workshop", "ws", Locale.ENGLISH)                    = 2
        //      /// StringUtils.getFuzzyDistance("Workshop", "wo", Locale.ENGLISH)                    = 4
        //      /// StringUtils.getFuzzyDistance("Apache Software Foundation", "asf", Locale.ENGLISH) = 3
        //      /// </pre>
        //      /// </summary>
        //      /// <param name="term"> a full term that should be matched against, must not be null </param>
        //      /// <param name="query"> the query that will be matched against a term, must not be null </param>
        //      /// <param name="locale"> This string matching logic is case insensitive. A locale is necessary to normalize
        //      ///  both Strings to lower case. </param>
        //      /// <returns> result score </returns>
        //      /// <exception cref="IllegalArgumentException"> if either String input {@code null} or Locale input {@code null}
        //      /// @since 3.4 </exception>
        //      //JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
        //      //ORIGINAL LINE: public static int getFuzzyDistance(final CharSequence term, final CharSequence query, final java.util.Locale locale)
        //      public static int getFuzzyDistance(CharSequence term, CharSequence query, Locale locale)
        //      {
        //          if (term == null || query == null)
        //          {
        //              throw new System.ArgumentException("Strings must not be null");
        //          }
        //          else if (locale == null)
        //          {
        //              throw new System.ArgumentException("Locale must not be null");
        //          }

        //          // fuzzy logic is case insensitive. We normalize the Strings to lower
        //          // case right from the start. Turning characters to lower case
        //          // via Character.toLowerCase(char) is unfortunately insufficient
        //          // as it does not accept a locale.
        //          //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
        //          //ORIGINAL LINE: final String termLowerCase = term.toString().toLowerCase(locale);
        //          string termLowerCase = term.ToString().ToLower(locale);
        //          //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
        //          //ORIGINAL LINE: final String queryLowerCase = query.toString().toLowerCase(locale);
        //          string queryLowerCase = query.ToString().ToLower(locale);

        //          // the resulting score
        //          int score = 0;

        //          // the position in the term which will be scanned next for potential
        //          // query character matches
        //          int termIndex = 0;

        //          // index of the previously matched character in the term
        //          int previousMatchingCharacterIndex = int.MinValue;

        //          for (int queryIndex = 0; queryIndex < queryLowerCase.Length; queryIndex++)
        //          {
        //              //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
        //              //ORIGINAL LINE: final char queryChar = queryLowerCase.charAt(queryIndex);
        //              char queryChar = queryLowerCase[queryIndex];

        //              bool termCharacterMatchFound = false;
        //              for (; termIndex < termLowerCase.Length && !termCharacterMatchFound; termIndex++)
        //              {
        //                  //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
        //                  //ORIGINAL LINE: final char termChar = termLowerCase.charAt(termIndex);
        //                  char termChar = termLowerCase[termIndex];

        //                  if (queryChar == termChar)
        //                  {
        //                      // simple character matches result in one point
        //                      score++;

        //                      // subsequent character matches further improve
        //                      // the score.
        //                      if (previousMatchingCharacterIndex + 1 == termIndex)
        //                      {
        //                          score += 2;
        //                      }

        //                      previousMatchingCharacterIndex = termIndex;

        //                      // we can leave the nested loop. Every character in the
        //                      // query can match at most one character in the term.
        //                      termCharacterMatchFound = true;
        //                  }
        //              }
        //          }

        //          return score;
        //      }

        //      // startsWith
        //      //-----------------------------------------------------------------------

        //      /// <summary>
        //      /// <para>Check if a CharSequence starts with a specified prefix.</para>
        //      /// 
        //      /// <para>{@code null}s are handled without exceptions. Two {@code null}
        //      /// references are considered to be equal. The comparison is case sensitive.</para>
        //      /// 
        //      /// <pre>
        //      /// StringUtils.startsWith(null, null)      = true
        //      /// StringUtils.startsWith(null, "abc")     = false
        //      /// StringUtils.startsWith("abcdef", null)  = false
        //      /// StringUtils.startsWith("abcdef", "abc") = true
        //      /// StringUtils.startsWith("ABCDEF", "abc") = false
        //      /// </pre>
        //      /// </summary>
        //      /// <seealso cref= java.lang.String#startsWith(String) </seealso>
        //      /// <param name="str">  the CharSequence to check, may be null </param>
        //      /// <param name="prefix"> the prefix to find, may be null </param>
        //      /// <returns> {@code true} if the CharSequence starts with the prefix, case sensitive, or
        //      ///  both {@code null}
        //      /// @since 2.4
        //      /// @since 3.0 Changed signature from startsWith(String, String) to startsWith(CharSequence, CharSequence) </returns>
        //      //JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
        //      //ORIGINAL LINE: public static boolean startsWith(final CharSequence str, final CharSequence prefix)
        //      public static bool startsWith(CharSequence str, CharSequence prefix)
        //      {
        //          return startsWith(str, prefix, false);
        //      }

        //      /// <summary>
        //      /// <para>Case insensitive check if a CharSequence starts with a specified prefix.</para>
        //      /// 
        //      /// <para>{@code null}s are handled without exceptions. Two {@code null}
        //      /// references are considered to be equal. The comparison is case insensitive.</para>
        //      /// 
        //      /// <pre>
        //      /// StringUtils.startsWithIgnoreCase(null, null)      = true
        //      /// StringUtils.startsWithIgnoreCase(null, "abc")     = false
        //      /// StringUtils.startsWithIgnoreCase("abcdef", null)  = false
        //      /// StringUtils.startsWithIgnoreCase("abcdef", "abc") = true
        //      /// StringUtils.startsWithIgnoreCase("ABCDEF", "abc") = true
        //      /// </pre>
        //      /// </summary>
        //      /// <seealso cref= java.lang.String#startsWith(String) </seealso>
        //      /// <param name="str">  the CharSequence to check, may be null </param>
        //      /// <param name="prefix"> the prefix to find, may be null </param>
        //      /// <returns> {@code true} if the CharSequence starts with the prefix, case insensitive, or
        //      ///  both {@code null}
        //      /// @since 2.4
        //      /// @since 3.0 Changed signature from startsWithIgnoreCase(String, String) to startsWithIgnoreCase(CharSequence, CharSequence) </returns>
        //      //JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
        //      //ORIGINAL LINE: public static boolean startsWithIgnoreCase(final CharSequence str, final CharSequence prefix)
        //      public static bool startsWithIgnoreCase(CharSequence str, CharSequence prefix)
        //      {
        //          return startsWith(str, prefix, true);
        //      }

        //      /// <summary>
        //      /// <para>Check if a CharSequence starts with a specified prefix (optionally case insensitive).</para>
        //      /// </summary>
        //      /// <seealso cref= java.lang.String#startsWith(String) </seealso>
        //      /// <param name="str">  the CharSequence to check, may be null </param>
        //      /// <param name="prefix"> the prefix to find, may be null </param>
        //      /// <param name="ignoreCase"> indicates whether the compare should ignore case
        //      ///  (case insensitive) or not. </param>
        //      /// <returns> {@code true} if the CharSequence starts with the prefix or
        //      ///  both {@code null} </returns>
        //      //JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
        //      //ORIGINAL LINE: private static boolean startsWith(final CharSequence str, final CharSequence prefix, final boolean ignoreCase)
        //      private static bool startsWith(CharSequence str, CharSequence prefix, bool ignoreCase)
        //      {
        //          if (str == null || prefix == null)
        //          {
        //              return str == null && prefix == null;
        //          }
        //          if (prefix.length() > str.length())
        //          {
        //              return false;
        //          }
        //          return CharSequenceUtils.regionMatches(str, ignoreCase, 0, prefix, 0, prefix.length());
        //      }

        //      /// <summary>
        //      /// <para>Check if a CharSequence starts with any of the provided case-sensitive prefixes.</para>
        //      /// 
        //      /// <pre>
        //      /// StringUtils.startsWithAny(null, null)      = false
        //      /// StringUtils.startsWithAny(null, new String[] {"abc"})  = false
        //      /// StringUtils.startsWithAny("abcxyz", null)     = false
        //      /// StringUtils.startsWithAny("abcxyz", new String[] {""}) = true
        //      /// StringUtils.startsWithAny("abcxyz", new String[] {"abc"}) = true
        //      /// StringUtils.startsWithAny("abcxyz", new String[] {null, "xyz", "abc"}) = true
        //      /// StringUtils.startsWithAny("abcxyz", null, "xyz", "ABCX") = false
        //      /// StringUtils.startsWithAny("ABCXYZ", null, "xyz", "abc") = false
        //      /// </pre>
        //      /// </summary>
        //      /// <param name="sequence"> the CharSequence to check, may be null </param>
        //      /// <param name="searchStrings"> the case-sensitive CharSequence prefixes, may be empty or contain {@code null} </param>
        //      /// <seealso cref= StringUtils#startsWith(CharSequence, CharSequence) </seealso>
        //      /// <returns> {@code true} if the input {@code sequence} is {@code null} AND no {@code searchStrings} are provided, or
        //      ///   the input {@code sequence} begins with any of the provided case-sensitive {@code searchStrings}.
        //      /// @since 2.5
        //      /// @since 3.0 Changed signature from startsWithAny(String, String[]) to startsWithAny(CharSequence, CharSequence...) </returns>
        //      //JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
        //      //ORIGINAL LINE: public static boolean startsWithAny(final CharSequence sequence, final CharSequence... searchStrings)
        //      public static bool startsWithAny(CharSequence sequence, params CharSequence[] searchStrings)
        //      {
        //          if (isEmpty(sequence) || ArrayUtils.isEmpty(searchStrings))
        //          {
        //              return false;
        //          }
        //          foreach (CharSequence searchString in searchStrings)
        //          {
        //              if (startsWith(sequence, searchString))
        //              {
        //                  return true;
        //              }
        //          }
        //          return false;
        //      }

        //      // endsWith
        //      //-----------------------------------------------------------------------

        //      /// <summary>
        //      /// <para>Check if a CharSequence ends with a specified suffix.</para>
        //      /// 
        //      /// <para>{@code null}s are handled without exceptions. Two {@code null}
        //      /// references are considered to be equal. The comparison is case sensitive.</para>
        //      /// 
        //      /// <pre>
        //      /// StringUtils.endsWith(null, null)      = true
        //      /// StringUtils.endsWith(null, "def")     = false
        //      /// StringUtils.endsWith("abcdef", null)  = false
        //      /// StringUtils.endsWith("abcdef", "def") = true
        //      /// StringUtils.endsWith("ABCDEF", "def") = false
        //      /// StringUtils.endsWith("ABCDEF", "cde") = false
        //      /// StringUtils.endsWith("ABCDEF", "")    = true
        //      /// </pre>
        //      /// </summary>
        //      /// <seealso cref= java.lang.String#endsWith(String) </seealso>
        //      /// <param name="str">  the CharSequence to check, may be null </param>
        //      /// <param name="suffix"> the suffix to find, may be null </param>
        //      /// <returns> {@code true} if the CharSequence ends with the suffix, case sensitive, or
        //      ///  both {@code null}
        //      /// @since 2.4
        //      /// @since 3.0 Changed signature from endsWith(String, String) to endsWith(CharSequence, CharSequence) </returns>
        //      //JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
        //      //ORIGINAL LINE: public static boolean endsWith(final CharSequence str, final CharSequence suffix)
        //      public static bool endsWith(CharSequence str, CharSequence suffix)
        //      {
        //          return endsWith(str, suffix, false);
        //      }

        //      /// <summary>
        //      /// <para>Case insensitive check if a CharSequence ends with a specified suffix.</para>
        //      /// 
        //      /// <para>{@code null}s are handled without exceptions. Two {@code null}
        //      /// references are considered to be equal. The comparison is case insensitive.</para>
        //      /// 
        //      /// <pre>
        //      /// StringUtils.endsWithIgnoreCase(null, null)      = true
        //      /// StringUtils.endsWithIgnoreCase(null, "def")     = false
        //      /// StringUtils.endsWithIgnoreCase("abcdef", null)  = false
        //      /// StringUtils.endsWithIgnoreCase("abcdef", "def") = true
        //      /// StringUtils.endsWithIgnoreCase("ABCDEF", "def") = true
        //      /// StringUtils.endsWithIgnoreCase("ABCDEF", "cde") = false
        //      /// </pre>
        //      /// </summary>
        //      /// <seealso cref= java.lang.String#endsWith(String) </seealso>
        //      /// <param name="str">  the CharSequence to check, may be null </param>
        //      /// <param name="suffix"> the suffix to find, may be null </param>
        //      /// <returns> {@code true} if the CharSequence ends with the suffix, case insensitive, or
        //      ///  both {@code null}
        //      /// @since 2.4
        //      /// @since 3.0 Changed signature from endsWithIgnoreCase(String, String) to endsWithIgnoreCase(CharSequence, CharSequence) </returns>
        //      //JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
        //      //ORIGINAL LINE: public static boolean endsWithIgnoreCase(final CharSequence str, final CharSequence suffix)
        //      public static bool endsWithIgnoreCase(CharSequence str, CharSequence suffix)
        //      {
        //          return endsWith(str, suffix, true);
        //      }

        //      /// <summary>
        //      /// <para>Check if a CharSequence ends with a specified suffix (optionally case insensitive).</para>
        //      /// </summary>
        //      /// <seealso cref= java.lang.String#endsWith(String) </seealso>
        //      /// <param name="str">  the CharSequence to check, may be null </param>
        //      /// <param name="suffix"> the suffix to find, may be null </param>
        //      /// <param name="ignoreCase"> indicates whether the compare should ignore case
        //      ///  (case insensitive) or not. </param>
        //      /// <returns> {@code true} if the CharSequence starts with the prefix or
        //      ///  both {@code null} </returns>
        //      //JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
        //      //ORIGINAL LINE: private static boolean endsWith(final CharSequence str, final CharSequence suffix, final boolean ignoreCase)
        //      private static bool endsWith(CharSequence str, CharSequence suffix, bool ignoreCase)
        //      {
        //          if (str == null || suffix == null)
        //          {
        //              return str == null && suffix == null;
        //          }
        //          if (suffix.length() > str.length())
        //          {
        //              return false;
        //          }
        //          //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
        //          //ORIGINAL LINE: final int strOffset = str.length() - suffix.length();
        //          int strOffset = str.length() - suffix.length();
        //          return CharSequenceUtils.regionMatches(str, ignoreCase, strOffset, suffix, 0, suffix.length());
        //      }

        //      /// <summary>
        //      /// <para>
        //      /// Similar to <a
        //      /// href="http://www.w3.org/TR/xpath/#function-normalize-space">http://www.w3.org/TR/xpath/#function-normalize
        //      /// -space</a>
        //      /// </para>
        //      /// <para>
        //      /// The function returns the argument string with whitespace normalized by using
        //      /// <code><seealso cref="#trim(String)"/></code> to remove leading and trailing whitespace
        //      /// and then replacing sequences of whitespace characters by a single space.
        //      /// </para>
        //      /// In XML Whitespace characters are the same as those allowed by the <a
        //      /// href="http://www.w3.org/TR/REC-xml/#NT-S">S</a> production, which is S ::= (#x20 | #x9 | #xD | #xA)+
        //      /// <para>
        //      /// Java's regexp pattern \s defines whitespace as [ \t\n\x0B\f\r]
        //      /// 
        //      /// </para>
        //      /// <para>For reference:</para>
        //      /// <ul>
        //      /// <li>\x0B = vertical tab</li>
        //      /// <li>\f = #xC = form feed</li>
        //      /// <li>#x20 = space</li>
        //      /// <li>#x9 = \t</li>
        //      /// <li>#xA = \n</li>
        //      /// <li>#xD = \r</li>
        //      /// </ul>
        //      /// 
        //      /// <para>
        //      /// The difference is that Java's whitespace includes vertical tab and form feed, which this functional will also
        //      /// normalize. Additionally <code><seealso cref="#trim(String)"/></code> removes control characters (char &lt;= 32) from both
        //      /// ends of this String.
        //      /// </para>
        //      /// </summary>
        //      /// <seealso cref= Pattern </seealso>
        //      /// <seealso cref= #trim(String) </seealso>
        //      /// <seealso cref= <a
        //      ///      href="http://www.w3.org/TR/xpath/#function-normalize-space">http://www.w3.org/TR/xpath/#function-normalize-space</a> </seealso>
        //      /// <param name="str"> the source String to normalize whitespaces from, may be null </param>
        //      /// <returns> the modified string with whitespace normalized, {@code null} if null String input
        //      /// 
        //      /// @since 3.0 </returns>
        //      //JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
        //      //ORIGINAL LINE: public static String normalizeSpace(final String str)
        //      public static string normalizeSpace(string str)
        //      {
        //          // LANG-1020: Improved performance significantly by normalizing manually instead of using regex
        //          // See https://github.com/librucha/commons-lang-normalizespaces-benchmark for performance test
        //          if (isEmpty(str))
        //          {
        //              return str;
        //          }
        //          //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
        //          //ORIGINAL LINE: final int size = str.length();
        //          int size = str.Length;
        //          //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
        //          //ORIGINAL LINE: final char[] newChars = new char[size];
        //          char[] newChars = new char[size];
        //          int count = 0;
        //          int whitespacesCount = 0;
        //          bool startWhitespaces = true;
        //          for (int i = 0; i < size; i++)
        //          {
        //              //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
        //              //ORIGINAL LINE: final char actualChar = str.charAt(i);
        //              char actualChar = str[i];
        //              //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
        //              //ORIGINAL LINE: final boolean isWhitespace = Character.isWhitespace(actualChar);
        //              bool isWhitespace = char.IsWhiteSpace(actualChar);
        //              if (!isWhitespace)
        //              {
        //                  startWhitespaces = false;
        //                  newChars[count++] = (actualChar == 160 ? 32 : actualChar);
        //                  whitespacesCount = 0;
        //              }
        //              else
        //              {
        //                  if (whitespacesCount == 0 && !startWhitespaces)
        //                  {
        //                      newChars[count++] = SPACE[0];
        //                  }
        //                  whitespacesCount++;
        //              }
        //          }
        //          if (startWhitespaces)
        //          {
        //              return EMPTY;
        //          }
        //          return (new string(newChars, 0, count - (whitespacesCount > 0 ? 1 : 0))).Trim();
        //      }

        //      /// <summary>
        //      /// <para>Check if a CharSequence ends with any of the provided case-sensitive suffixes.</para>
        //      /// 
        //      /// <pre>
        //      /// StringUtils.endsWithAny(null, null)      = false
        //      /// StringUtils.endsWithAny(null, new String[] {"abc"})  = false
        //      /// StringUtils.endsWithAny("abcxyz", null)     = false
        //      /// StringUtils.endsWithAny("abcxyz", new String[] {""}) = true
        //      /// StringUtils.endsWithAny("abcxyz", new String[] {"xyz"}) = true
        //      /// StringUtils.endsWithAny("abcxyz", new String[] {null, "xyz", "abc"}) = true
        //      /// StringUtils.endsWithAny("abcXYZ", "def", "XYZ") = true
        //      /// StringUtils.endsWithAny("abcXYZ", "def", "xyz") = false
        //      /// </pre>
        //      /// </summary>
        //      /// <param name="sequence">  the CharSequence to check, may be null </param>
        //      /// <param name="searchStrings"> the case-sensitive CharSequences to find, may be empty or contain {@code null} </param>
        //      /// <seealso cref= StringUtils#endsWith(CharSequence, CharSequence) </seealso>
        //      /// <returns> {@code true} if the input {@code sequence} is {@code null} AND no {@code searchStrings} are provided, or
        //      ///   the input {@code sequence} ends in any of the provided case-sensitive {@code searchStrings}.
        //      /// @since 3.0 </returns>
        //      //JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
        //      //ORIGINAL LINE: public static boolean endsWithAny(final CharSequence sequence, final CharSequence... searchStrings)
        //      public static bool endsWithAny(CharSequence sequence, params CharSequence[] searchStrings)
        //      {
        //          if (isEmpty(sequence) || ArrayUtils.isEmpty(searchStrings))
        //          {
        //              return false;
        //          }
        //          foreach (CharSequence searchString in searchStrings)
        //          {
        //              if (endsWith(sequence, searchString))
        //              {
        //                  return true;
        //              }
        //          }
        //          return false;
        //      }

        //      /// <summary>
        //      /// Appends the suffix to the end of the string if the string does not
        //      /// already end with the suffix.
        //      /// </summary>
        //      /// <param name="str"> The string. </param>
        //      /// <param name="suffix"> The suffix to append to the end of the string. </param>
        //      /// <param name="ignoreCase"> Indicates whether the compare should ignore case. </param>
        //      /// <param name="suffixes"> Additional suffixes that are valid terminators (optional).
        //      /// </param>
        //      /// <returns> A new String if suffix was appended, the same string otherwise. </returns>
        //      //JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
        //      //ORIGINAL LINE: private static String appendIfMissing(final String str, final CharSequence suffix, final boolean ignoreCase, final CharSequence... suffixes)
        //      private static string appendIfMissing(string str, CharSequence suffix, bool ignoreCase, params CharSequence[] suffixes)
        //      {
        //          if (string.ReferenceEquals(str, null) || isEmpty(suffix) || endsWith(str, suffix, ignoreCase))
        //          {
        //              return str;
        //          }
        //          if (suffixes != null && suffixes.Length > 0)
        //          {
        //              foreach (CharSequence s in suffixes)
        //              {
        //                  if (endsWith(str, s, ignoreCase))
        //                  {
        //                      return str;
        //                  }
        //              }
        //          }
        //          return str + suffix.ToString();
        //      }

        //      /// <summary>
        //      /// Appends the suffix to the end of the string if the string does not
        //      /// already end with any of the suffixes.
        //      /// 
        //      /// <pre>
        //      /// StringUtils.appendIfMissing(null, null) = null
        //      /// StringUtils.appendIfMissing("abc", null) = "abc"
        //      /// StringUtils.appendIfMissing("", "xyz") = "xyz"
        //      /// StringUtils.appendIfMissing("abc", "xyz") = "abcxyz"
        //      /// StringUtils.appendIfMissing("abcxyz", "xyz") = "abcxyz"
        //      /// StringUtils.appendIfMissing("abcXYZ", "xyz") = "abcXYZxyz"
        //      /// </pre>
        //      /// <para>With additional suffixes,</para>
        //      /// <pre>
        //      /// StringUtils.appendIfMissing(null, null, null) = null
        //      /// StringUtils.appendIfMissing("abc", null, null) = "abc"
        //      /// StringUtils.appendIfMissing("", "xyz", null) = "xyz"
        //      /// StringUtils.appendIfMissing("abc", "xyz", new CharSequence[]{null}) = "abcxyz"
        //      /// StringUtils.appendIfMissing("abc", "xyz", "") = "abc"
        //      /// StringUtils.appendIfMissing("abc", "xyz", "mno") = "abcxyz"
        //      /// StringUtils.appendIfMissing("abcxyz", "xyz", "mno") = "abcxyz"
        //      /// StringUtils.appendIfMissing("abcmno", "xyz", "mno") = "abcmno"
        //      /// StringUtils.appendIfMissing("abcXYZ", "xyz", "mno") = "abcXYZxyz"
        //      /// StringUtils.appendIfMissing("abcMNO", "xyz", "mno") = "abcMNOxyz"
        //      /// </pre>
        //      /// </summary>
        //      /// <param name="str"> The string. </param>
        //      /// <param name="suffix"> The suffix to append to the end of the string. </param>
        //      /// <param name="suffixes"> Additional suffixes that are valid terminators.
        //      /// </param>
        //      /// <returns> A new String if suffix was appended, the same string otherwise.
        //      /// 
        //      /// @since 3.2 </returns>
        //      //JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
        //      //ORIGINAL LINE: public static String appendIfMissing(final String str, final CharSequence suffix, final CharSequence... suffixes)
        //      public static string appendIfMissing(string str, CharSequence suffix, params CharSequence[] suffixes)
        //      {
        //          return appendIfMissing(str, suffix, false, suffixes);
        //      }

        //      /// <summary>
        //      /// Appends the suffix to the end of the string if the string does not
        //      /// already end, case insensitive, with any of the suffixes.
        //      /// 
        //      /// <pre>
        //      /// StringUtils.appendIfMissingIgnoreCase(null, null) = null
        //      /// StringUtils.appendIfMissingIgnoreCase("abc", null) = "abc"
        //      /// StringUtils.appendIfMissingIgnoreCase("", "xyz") = "xyz"
        //      /// StringUtils.appendIfMissingIgnoreCase("abc", "xyz") = "abcxyz"
        //      /// StringUtils.appendIfMissingIgnoreCase("abcxyz", "xyz") = "abcxyz"
        //      /// StringUtils.appendIfMissingIgnoreCase("abcXYZ", "xyz") = "abcXYZ"
        //      /// </pre>
        //      /// <para>With additional suffixes,</para>
        //      /// <pre>
        //      /// StringUtils.appendIfMissingIgnoreCase(null, null, null) = null
        //      /// StringUtils.appendIfMissingIgnoreCase("abc", null, null) = "abc"
        //      /// StringUtils.appendIfMissingIgnoreCase("", "xyz", null) = "xyz"
        //      /// StringUtils.appendIfMissingIgnoreCase("abc", "xyz", new CharSequence[]{null}) = "abcxyz"
        //      /// StringUtils.appendIfMissingIgnoreCase("abc", "xyz", "") = "abc"
        //      /// StringUtils.appendIfMissingIgnoreCase("abc", "xyz", "mno") = "axyz"
        //      /// StringUtils.appendIfMissingIgnoreCase("abcxyz", "xyz", "mno") = "abcxyz"
        //      /// StringUtils.appendIfMissingIgnoreCase("abcmno", "xyz", "mno") = "abcmno"
        //      /// StringUtils.appendIfMissingIgnoreCase("abcXYZ", "xyz", "mno") = "abcXYZ"
        //      /// StringUtils.appendIfMissingIgnoreCase("abcMNO", "xyz", "mno") = "abcMNO"
        //      /// </pre>
        //      /// </summary>
        //      /// <param name="str"> The string. </param>
        //      /// <param name="suffix"> The suffix to append to the end of the string. </param>
        //      /// <param name="suffixes"> Additional suffixes that are valid terminators.
        //      /// </param>
        //      /// <returns> A new String if suffix was appended, the same string otherwise.
        //      /// 
        //      /// @since 3.2 </returns>
        //      //JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
        //      //ORIGINAL LINE: public static String appendIfMissingIgnoreCase(final String str, final CharSequence suffix, final CharSequence... suffixes)
        //      public static string appendIfMissingIgnoreCase(string str, CharSequence suffix, params CharSequence[] suffixes)
        //      {
        //          return appendIfMissing(str, suffix, true, suffixes);
        //      }

        //      /// <summary>
        //      /// Prepends the prefix to the start of the string if the string does not
        //      /// already start with any of the prefixes.
        //      /// </summary>
        //      /// <param name="str"> The string. </param>
        //      /// <param name="prefix"> The prefix to prepend to the start of the string. </param>
        //      /// <param name="ignoreCase"> Indicates whether the compare should ignore case. </param>
        //      /// <param name="prefixes"> Additional prefixes that are valid (optional).
        //      /// </param>
        //      /// <returns> A new String if prefix was prepended, the same string otherwise. </returns>
        //      //JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
        //      //ORIGINAL LINE: private static String prependIfMissing(final String str, final CharSequence prefix, final boolean ignoreCase, final CharSequence... prefixes)
        //      private static string prependIfMissing(string str, CharSequence prefix, bool ignoreCase, params CharSequence[] prefixes)
        //      {
        //          if (string.ReferenceEquals(str, null) || isEmpty(prefix) || startsWith(str, prefix, ignoreCase))
        //          {
        //              return str;
        //          }
        //          if (prefixes != null && prefixes.Length > 0)
        //          {
        //              foreach (CharSequence p in prefixes)
        //              {
        //                  if (startsWith(str, p, ignoreCase))
        //                  {
        //                      return str;
        //                  }
        //              }
        //          }
        //          return prefix.ToString() + str;
        //      }

        //      /// <summary>
        //      /// Prepends the prefix to the start of the string if the string does not
        //      /// already start with any of the prefixes.
        //      /// 
        //      /// <pre>
        //      /// StringUtils.prependIfMissing(null, null) = null
        //      /// StringUtils.prependIfMissing("abc", null) = "abc"
        //      /// StringUtils.prependIfMissing("", "xyz") = "xyz"
        //      /// StringUtils.prependIfMissing("abc", "xyz") = "xyzabc"
        //      /// StringUtils.prependIfMissing("xyzabc", "xyz") = "xyzabc"
        //      /// StringUtils.prependIfMissing("XYZabc", "xyz") = "xyzXYZabc"
        //      /// </pre>
        //      /// <para>With additional prefixes,</para>
        //      /// <pre>
        //      /// StringUtils.prependIfMissing(null, null, null) = null
        //      /// StringUtils.prependIfMissing("abc", null, null) = "abc"
        //      /// StringUtils.prependIfMissing("", "xyz", null) = "xyz"
        //      /// StringUtils.prependIfMissing("abc", "xyz", new CharSequence[]{null}) = "xyzabc"
        //      /// StringUtils.prependIfMissing("abc", "xyz", "") = "abc"
        //      /// StringUtils.prependIfMissing("abc", "xyz", "mno") = "xyzabc"
        //      /// StringUtils.prependIfMissing("xyzabc", "xyz", "mno") = "xyzabc"
        //      /// StringUtils.prependIfMissing("mnoabc", "xyz", "mno") = "mnoabc"
        //      /// StringUtils.prependIfMissing("XYZabc", "xyz", "mno") = "xyzXYZabc"
        //      /// StringUtils.prependIfMissing("MNOabc", "xyz", "mno") = "xyzMNOabc"
        //      /// </pre>
        //      /// </summary>
        //      /// <param name="str"> The string. </param>
        //      /// <param name="prefix"> The prefix to prepend to the start of the string. </param>
        //      /// <param name="prefixes"> Additional prefixes that are valid.
        //      /// </param>
        //      /// <returns> A new String if prefix was prepended, the same string otherwise.
        //      /// 
        //      /// @since 3.2 </returns>
        //      //JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
        //      //ORIGINAL LINE: public static String prependIfMissing(final String str, final CharSequence prefix, final CharSequence... prefixes)
        //      public static string prependIfMissing(string str, CharSequence prefix, params CharSequence[] prefixes)
        //      {
        //          return prependIfMissing(str, prefix, false, prefixes);
        //      }

        //      /// <summary>
        //      /// Prepends the prefix to the start of the string if the string does not
        //      /// already start, case insensitive, with any of the prefixes.
        //      /// 
        //      /// <pre>
        //      /// StringUtils.prependIfMissingIgnoreCase(null, null) = null
        //      /// StringUtils.prependIfMissingIgnoreCase("abc", null) = "abc"
        //      /// StringUtils.prependIfMissingIgnoreCase("", "xyz") = "xyz"
        //      /// StringUtils.prependIfMissingIgnoreCase("abc", "xyz") = "xyzabc"
        //      /// StringUtils.prependIfMissingIgnoreCase("xyzabc", "xyz") = "xyzabc"
        //      /// StringUtils.prependIfMissingIgnoreCase("XYZabc", "xyz") = "XYZabc"
        //      /// </pre>
        //      /// <para>With additional prefixes,</para>
        //      /// <pre>
        //      /// StringUtils.prependIfMissingIgnoreCase(null, null, null) = null
        //      /// StringUtils.prependIfMissingIgnoreCase("abc", null, null) = "abc"
        //      /// StringUtils.prependIfMissingIgnoreCase("", "xyz", null) = "xyz"
        //      /// StringUtils.prependIfMissingIgnoreCase("abc", "xyz", new CharSequence[]{null}) = "xyzabc"
        //      /// StringUtils.prependIfMissingIgnoreCase("abc", "xyz", "") = "abc"
        //      /// StringUtils.prependIfMissingIgnoreCase("abc", "xyz", "mno") = "xyzabc"
        //      /// StringUtils.prependIfMissingIgnoreCase("xyzabc", "xyz", "mno") = "xyzabc"
        //      /// StringUtils.prependIfMissingIgnoreCase("mnoabc", "xyz", "mno") = "mnoabc"
        //      /// StringUtils.prependIfMissingIgnoreCase("XYZabc", "xyz", "mno") = "XYZabc"
        //      /// StringUtils.prependIfMissingIgnoreCase("MNOabc", "xyz", "mno") = "MNOabc"
        //      /// </pre>
        //      /// </summary>
        //      /// <param name="str"> The string. </param>
        //      /// <param name="prefix"> The prefix to prepend to the start of the string. </param>
        //      /// <param name="prefixes"> Additional prefixes that are valid (optional).
        //      /// </param>
        //      /// <returns> A new String if prefix was prepended, the same string otherwise.
        //      /// 
        //      /// @since 3.2 </returns>
        //      //JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
        //      //ORIGINAL LINE: public static String prependIfMissingIgnoreCase(final String str, final CharSequence prefix, final CharSequence... prefixes)
        //      public static string prependIfMissingIgnoreCase(string str, CharSequence prefix, params CharSequence[] prefixes)
        //      {
        //          return prependIfMissing(str, prefix, true, prefixes);
        //      }

        //      /// <summary>
        //      /// Converts a <code>byte[]</code> to a String using the specified character encoding.
        //      /// </summary>
        //      /// <param name="bytes">
        //      ///            the byte array to read from </param>
        //      /// <param name="charsetName">
        //      ///            the encoding to use, if null then use the platform default </param>
        //      /// <returns> a new String </returns>
        //      /// <exception cref="UnsupportedEncodingException">
        //      ///             If the named charset is not supported </exception>
        //      /// <exception cref="NullPointerException">
        //      ///             if the input is null </exception>
        //      /// @deprecated use <seealso cref="StringUtils#toEncodedString(byte[], Charset)"/> instead of String constants in your code
        //      /// @since 3.1 
        //      //JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
        //      //ORIGINAL LINE: @Deprecated("use <seealso cref="StringUtils#toEncodedString(byte[], java.nio.charset.Charset)"/> instead of String constants in your code") public static String toString(final byte[] bytes, final String charsetName) throws java.io.UnsupportedEncodingException
        //      //JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
        //      [Obsolete("use <seealso cref="StringUtils#toEncodedString(byte[], java.nio.charset.Charset)"/> instead of String constants in your code")]
        //public static string ToString(sbyte[] bytes, string charsetName)
        //      {
        //          return !string.ReferenceEquals(charsetName, null) ? StringHelperClass.NewString(bytes, charsetName) : StringHelperClass.NewString(bytes, Charset.defaultCharset());
        //      }

        //      /// <summary>
        //      /// Converts a <code>byte[]</code> to a String using the specified character encoding.
        //      /// </summary>
        //      /// <param name="bytes">
        //      ///            the byte array to read from </param>
        //      /// <param name="charset">
        //      ///            the encoding to use, if null then use the platform default </param>
        //      /// <returns> a new String </returns>
        //      /// <exception cref="NullPointerException">
        //      ///             if {@code bytes} is null
        //      /// @since 3.2
        //      /// @since 3.3 No longer throws <seealso cref="UnsupportedEncodingException"/>. </exception>
        //      //JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
        //      //ORIGINAL LINE: public static String toEncodedString(final byte[] bytes, final java.nio.charset.Charset charset)
        //      public static string toEncodedString(sbyte[] bytes, Charset charset)
        //      {
        //          return StringHelperClass.NewString(bytes, charset != null ? charset : Charset.defaultCharset());
        //      }

        //      /// <summary>
        //      /// <para>
        //      /// Wraps a string with a char.
        //      /// </para>
        //      /// 
        //      /// <pre>
        //      /// StringUtils.wrap(null, *)        = null
        //      /// StringUtils.wrap("", *)          = ""
        //      /// StringUtils.wrap("ab", '\0')     = "ab"
        //      /// StringUtils.wrap("ab", 'x')      = "xabx"
        //      /// StringUtils.wrap("ab", '\'')     = "'ab'"
        //      /// StringUtils.wrap("\"ab\"", '\"') = "\"\"ab\"\""
        //      /// </pre>
        //      /// </summary>
        //      /// <param name="str">
        //      ///            the string to be wrapped, may be {@code null} </param>
        //      /// <param name="wrapWith">
        //      ///            the char that will wrap {@code str} </param>
        //      /// <returns> the wrapped string, or {@code null} if {@code str==null}
        //      /// @since 3.4 </returns>
        //      //JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
        //      //ORIGINAL LINE: public static String wrap(final String str, final char wrapWith)
        //      public static string wrap(string str, char wrapWith)
        //      {

        //          if (isEmpty(str) || wrapWith == '\0')
        //          {
        //              return str;
        //          }

        //          return wrapWith + str + wrapWith;
        //      }

        //      /// <summary>
        //      /// <para>
        //      /// Wraps a String with another String.
        //      /// </para>
        //      /// 
        //      /// <para>
        //      /// A {@code null} input String returns {@code null}.
        //      /// </para>
        //      /// 
        //      /// <pre>
        //      /// StringUtils.wrap(null, *)         = null
        //      /// StringUtils.wrap("", *)           = ""
        //      /// StringUtils.wrap("ab", null)      = "ab"
        //      /// StringUtils.wrap("ab", "x")       = "xabx"
        //      /// StringUtils.wrap("ab", "\"")      = "\"ab\""
        //      /// StringUtils.wrap("\"ab\"", "\"")  = "\"\"ab\"\""
        //      /// StringUtils.wrap("ab", "'")       = "'ab'"
        //      /// StringUtils.wrap("'abcd'", "'")   = "''abcd''"
        //      /// StringUtils.wrap("\"abcd\"", "'") = "'\"abcd\"'"
        //      /// StringUtils.wrap("'abcd'", "\"")  = "\"'abcd'\""
        //      /// </pre>
        //      /// </summary>
        //      /// <param name="str">
        //      ///            the String to be wrapper, may be null </param>
        //      /// <param name="wrapWith">
        //      ///            the String that will wrap str </param>
        //      /// <returns> wrapped String, {@code null} if null String input
        //      /// @since 3.4 </returns>
        //      //JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
        //      //ORIGINAL LINE: public static String wrap(final String str, final String wrapWith)
        //      public static string wrap(string str, string wrapWith)
        //      {

        //          if (isEmpty(str) || isEmpty(wrapWith))
        //          {
        //              return str;
        //          }

        //          return wrapWith + str + wrapWith;
        //      }

        //      /// <summary>
        //      /// <para>
        //      /// Wraps a string with a char if that char is missing from the start or end of the given string.
        //      /// </para>
        //      /// 
        //      /// <pre>
        //      /// StringUtils.wrap(null, *)        = null
        //      /// StringUtils.wrap("", *)          = ""
        //      /// StringUtils.wrap("ab", '\0')     = "ab"
        //      /// StringUtils.wrap("ab", 'x')      = "xabx"
        //      /// StringUtils.wrap("ab", '\'')     = "'ab'"
        //      /// StringUtils.wrap("\"ab\"", '\"') = "\"ab\""
        //      /// StringUtils.wrap("/", '/')  = "/"
        //      /// StringUtils.wrap("a/b/c", '/')  = "/a/b/c/"
        //      /// StringUtils.wrap("/a/b/c", '/')  = "/a/b/c/"
        //      /// StringUtils.wrap("a/b/c/", '/')  = "/a/b/c/"
        //      /// </pre>
        //      /// </summary>
        //      /// <param name="str">
        //      ///            the string to be wrapped, may be {@code null} </param>
        //      /// <param name="wrapWith">
        //      ///            the char that will wrap {@code str} </param>
        //      /// <returns> the wrapped string, or {@code null} if {@code str==null}
        //      /// @since 3.5 </returns>
        //      //JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
        //      //ORIGINAL LINE: public static String wrapIfMissing(final String str, final char wrapWith)
        //      public static string wrapIfMissing(string str, char wrapWith)
        //      {
        //          if (isEmpty(str) || wrapWith == '\0')
        //          {
        //              return str;
        //          }
        //          //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
        //          //ORIGINAL LINE: final StringBuilder builder = new StringBuilder(str.length() + 2);
        //          StringBuilder builder = new StringBuilder(str.Length + 2);
        //          if (str[0] != wrapWith)
        //          {
        //              builder.Append(wrapWith);
        //          }
        //          builder.Append(str);
        //          if (str[str.Length - 1] != wrapWith)
        //          {
        //              builder.Append(wrapWith);
        //          }
        //          return builder.ToString();
        //      }

        //      /// <summary>
        //      /// <para>
        //      /// Wraps a string with a string if that string is missing from the start or end of the given string.
        //      /// </para>
        //      /// 
        //      /// <pre>
        //      /// StringUtils.wrap(null, *)         = null
        //      /// StringUtils.wrap("", *)           = ""
        //      /// StringUtils.wrap("ab", null)      = "ab"
        //      /// StringUtils.wrap("ab", "x")       = "xabx"
        //      /// StringUtils.wrap("ab", "\"")      = "\"ab\""
        //      /// StringUtils.wrap("\"ab\"", "\"")  = "\"ab\""
        //      /// StringUtils.wrap("ab", "'")       = "'ab'"
        //      /// StringUtils.wrap("'abcd'", "'")   = "'abcd'"
        //      /// StringUtils.wrap("\"abcd\"", "'") = "'\"abcd\"'"
        //      /// StringUtils.wrap("'abcd'", "\"")  = "\"'abcd'\""
        //      /// StringUtils.wrap("/", "/")  = "/"
        //      /// StringUtils.wrap("a/b/c", "/")  = "/a/b/c/"
        //      /// StringUtils.wrap("/a/b/c", "/")  = "/a/b/c/"
        //      /// StringUtils.wrap("a/b/c/", "/")  = "/a/b/c/"
        //      /// </pre>
        //      /// </summary>
        //      /// <param name="str">
        //      ///            the string to be wrapped, may be {@code null} </param>
        //      /// <param name="wrapWith">
        //      ///            the char that will wrap {@code str} </param>
        //      /// <returns> the wrapped string, or {@code null} if {@code str==null}
        //      /// @since 3.5 </returns>
        //      //JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
        //      //ORIGINAL LINE: public static String wrapIfMissing(final String str, final String wrapWith)
        //      public static string wrapIfMissing(string str, string wrapWith)
        //      {
        //          if (isEmpty(str) || isEmpty(wrapWith))
        //          {
        //              return str;
        //          }
        //          //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
        //          //ORIGINAL LINE: final StringBuilder builder = new StringBuilder(str.length() + wrapWith.length() + wrapWith.length());
        //          StringBuilder builder = new StringBuilder(str.Length + wrapWith.Length + wrapWith.Length);
        //          if (!str.StartsWith(wrapWith, StringComparison.Ordinal))
        //          {
        //              builder.Append(wrapWith);
        //          }
        //          builder.Append(str);
        //          if (!str.EndsWith(wrapWith, StringComparison.Ordinal))
        //          {
        //              builder.Append(wrapWith);
        //          }
        //          return builder.ToString();
        //      }

        //      /// <summary>
        //      /// <para>
        //      /// Unwraps a given string from anther string.
        //      /// </para>
        //      /// 
        //      /// <pre>
        //      /// StringUtils.unwrap(null, null)         = null
        //      /// StringUtils.unwrap(null, "")           = null
        //      /// StringUtils.unwrap(null, "1")          = null
        //      /// StringUtils.unwrap("\'abc\'", "\'")    = "abc"
        //      /// StringUtils.unwrap("\"abc\"", "\"")    = "abc"
        //      /// StringUtils.unwrap("AABabcBAA", "AA")  = "BabcB"
        //      /// StringUtils.unwrap("A", "#")           = "A"
        //      /// StringUtils.unwrap("#A", "#")          = "#A"
        //      /// StringUtils.unwrap("A#", "#")          = "A#"
        //      /// </pre>
        //      /// </summary>
        //      /// <param name="str">
        //      ///          the String to be unwrapped, can be null </param>
        //      /// <param name="wrapToken">
        //      ///          the String used to unwrap </param>
        //      /// <returns> unwrapped String or the original string 
        //      ///          if it is not quoted properly with the wrapToken
        //      /// @since 3.6 </returns>
        //      //JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
        //      //ORIGINAL LINE: public static String unwrap(final String str, final String wrapToken)
        //      public static string unwrap(string str, string wrapToken)
        //      {
        //          if (isEmpty(str) || isEmpty(wrapToken))
        //          {
        //              return str;
        //          }

        //          if (startsWith(str, wrapToken) && endsWith(str, wrapToken))
        //          {
        //              //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
        //              //ORIGINAL LINE: final int startIndex = str.indexOf(wrapToken);
        //              int startIndex = str.IndexOf(wrapToken, StringComparison.Ordinal);
        //              //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
        //              //ORIGINAL LINE: final int endIndex = str.lastIndexOf(wrapToken);
        //              int endIndex = str.LastIndexOf(wrapToken, StringComparison.Ordinal);
        //              //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
        //              //ORIGINAL LINE: final int wrapLength = wrapToken.length();
        //              int wrapLength = wrapToken.Length;
        //              if (startIndex != -1 && endIndex != -1)
        //              {
        //                  return str.Substring(startIndex + wrapLength, endIndex - (startIndex + wrapLength));
        //              }
        //          }

        //          return str;
        //      }

        //      /// <summary>
        //      /// <para>
        //      /// Unwraps a given string from a character.
        //      /// </para>
        //      /// 
        //      /// <pre>
        //      /// StringUtils.unwrap(null, null)         = null
        //      /// StringUtils.unwrap(null, '\0')         = null
        //      /// StringUtils.unwrap(null, '1')          = null
        //      /// StringUtils.unwrap("\'abc\'", '\'')    = "abc"
        //      /// StringUtils.unwrap("AABabcBAA", 'A')  = "ABabcBA"
        //      /// StringUtils.unwrap("A", '#')           = "A"
        //      /// StringUtils.unwrap("#A", '#')          = "#A"
        //      /// StringUtils.unwrap("A#", '#')          = "A#"
        //      /// </pre>
        //      /// </summary>
        //      /// <param name="str">
        //      ///          the String to be unwrapped, can be null </param>
        //      /// <param name="wrapChar">
        //      ///          the character used to unwrap </param>
        //      /// <returns> unwrapped String or the original string 
        //      ///          if it is not quoted properly with the wrapChar
        //      /// @since 3.6 </returns>
        //      //JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
        //      //ORIGINAL LINE: public static String unwrap(final String str, final char wrapChar)
        //      public static string unwrap(string str, char wrapChar)
        //      {
        //          if (isEmpty(str) || wrapChar == '\0')
        //          {
        //              return str;
        //          }

        //          if (str[0] == wrapChar && str[str.Length - 1] == wrapChar)
        //          {
        //              const int startIndex = 0;
        //              //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
        //              //ORIGINAL LINE: final int endIndex = str.length() - 1;
        //              int endIndex = str.Length - 1;
        //              if (startIndex != -1 && endIndex != -1)
        //              {
        //                  return str.Substring(startIndex + 1, endIndex - (startIndex + 1));
        //              }
        //          }

        //          return str;
        //      }


        //      /// <summary>
        //      /// <para>Converts a {@code CharSequence} into an array of code points.</para>
        //      /// 
        //      /// <para>Valid pairs of surrogate code units will be converted into a single supplementary
        //      /// code point. Isolated surrogate code units (i.e. a high surrogate not followed by a low surrogate or
        //      /// a low surrogate not preceeded by a high surrogate) will be returned as-is.</para>
        //      /// 
        //      /// <pre>
        //      /// StringUtils.toCodePoints(null)   =  null
        //      /// StringUtils.toCodePoints("")     =  []  // empty array
        //      /// </pre>
        //      /// </summary>
        //      /// <param name="str"> the character sequence to convert </param>
        //      /// <returns> an array of code points
        //      /// @since 3.6 </returns>
        //      public static int[] toCodePoints(CharSequence str)
        //      {
        //          if (str == null)
        //          {
        //              return null;
        //          }
        //          if (str.length() == 0)
        //          {
        //              return ArrayUtils.EMPTY_INT_ARRAY;
        //          }

        //          string s = str.ToString();
        //          int[] result = new int[s.codePointCount(0, s.Length)];
        //          int index = 0;
        //          for (int i = 0; i < result.Length; i++)
        //          {
        //              result[i] = char.ConvertToUtf32(s, index);
        //              index += Character.charCount(result[i]);
        //          }
        //          return result;
        //      }
    }
}

//-------------------------------------------------------------------------------------------
//	Copyright © 2007 - 2017 Tangible Software Solutions Inc.
//	This class can be used by anyone provided that the copyright notice remains intact.
//
//	This class is used to convert some aspects of the Java String class.
//-------------------------------------------------------------------------------------------
internal static class StringHelperClass
{
    //----------------------------------------------------------------------------------
    //	This method replaces the Java String.substring method when 'start' is a
    //	method call or calculated value to ensure that 'start' is obtained just once.
    //----------------------------------------------------------------------------------
    internal static string SubstringSpecial(this string self, int start, int end)
    {
        return self.Substring(start, end - start);
    }

    //------------------------------------------------------------------------------------
    //	This method is used to replace calls to the 2-arg Java String.startsWith method.
    //------------------------------------------------------------------------------------
    internal static bool StartsWith(this string self, string prefix, int toffset)
    {
        return self.IndexOf(prefix, toffset, System.StringComparison.Ordinal) == toffset;
    }

    //------------------------------------------------------------------------------
    //	This method is used to replace most calls to the Java String.split method.
    //------------------------------------------------------------------------------
    internal static string[] Split(this string self, string regexDelimiter, bool trimTrailingEmptyStrings)
    {
        string[] splitArray = System.Text.RegularExpressions.Regex.Split(self, regexDelimiter);

        if (trimTrailingEmptyStrings)
        {
            if (splitArray.Length > 1)
            {
                for (int i = splitArray.Length; i > 0; i--)
                {
                    if (splitArray[i - 1].Length > 0)
                    {
                        if (i < splitArray.Length)
                            System.Array.Resize(ref splitArray, i);

                        break;
                    }
                }
            }
        }

        return splitArray;
    }

    //-----------------------------------------------------------------------------
    //	These methods are used to replace calls to some Java String constructors.
    //-----------------------------------------------------------------------------
    internal static string NewString(sbyte[] bytes)
    {
        return NewString(bytes, 0, bytes.Length);
    }
    internal static string NewString(sbyte[] bytes, int index, int count)
    {
        return System.Text.Encoding.UTF8.GetString((byte[])(object)bytes, index, count);
    }
    internal static string NewString(sbyte[] bytes, string encoding)
    {
        return NewString(bytes, 0, bytes.Length, encoding);
    }
    internal static string NewString(sbyte[] bytes, int index, int count, string encoding)
    {
        return System.Text.Encoding.GetEncoding(encoding).GetString((byte[])(object)bytes, index, count);
    }

    //--------------------------------------------------------------------------------
    //	These methods are used to replace calls to the Java String.getBytes methods.
    //--------------------------------------------------------------------------------
    internal static sbyte[] GetBytes(this string self)
    {
        return GetSBytesForEncoding(System.Text.Encoding.UTF8, self);
    }
    internal static sbyte[] GetBytes(this string self, System.Text.Encoding encoding)
    {
        return GetSBytesForEncoding(encoding, self);
    }
    internal static sbyte[] GetBytes(this string self, string encoding)
    {
        return GetSBytesForEncoding(System.Text.Encoding.GetEncoding(encoding), self);
    }
    private static sbyte[] GetSBytesForEncoding(System.Text.Encoding encoding, string s)
    {
        sbyte[] sbytes = new sbyte[encoding.GetByteCount(s)];
        encoding.GetBytes(s, 0, s.Length, (byte[])(object)sbytes, 0);
        return sbytes;
    }
}
