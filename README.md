
# AntlrVSIX

AntlrVSIX is a Visual Studio 2019 extension for programming language support,
for languages that are based in Antlr v4 grammars.
Each language is described by the grammar along with mappings of syntax
to support tagging, go to def, find all refs,
replace, go to visitor/listener, command completion, options,
reformat. Programming languages that are currently supported to one degree or another
are: Antlr, Java, Python, and Rust.

This extension also includes project templates for compiling and running Antlr programs.

The source code for the extension is open source, free of charge, and free of ads.

# Installation of Prerequisites

* Install Java tool chain, either [OpenJDK](https://openjdk.java.net/) or [Oracle JDK SE](https://www.oracle.com/technetwork/java/javase/downloads/index.html).

* Downloaded the Java-based Antlr tool chain. [Complete ANTLR 4.7.2 Java binaries jar](https://www.antlr.org/download/antlr-4.7.2-complete.jar).

* Set the environment variable "JAVA_HOME" to the directory of the java installation. See [this](https://confluence.atlassian.com/doc/setting-the-java_home-variable-in-windows-8895.html) for some instructions on how to do this
on Windows.

* Set the environment variable "Antlr4ToolPath" to the path of the downloaded Antlr jar file.

* Do not include the generated .cs Antlr parser files in the CSPROJ file for your program. The generated parser code is placed in the build temp output directory and automatically included.

* Make sure you do not have a version skew between the Java Antlr tool and the runtime versions.

# Verify Prerequisites

Please verify that you have these variables set up as expected. Try
*"$JAVA_HOME/bin/java.exe" -jar "$Antlr4ToolPath"*
from a Git Bash or
*"%JAVA_HOME%\bin\java.exe" -jar "%Antlr4ToolPath%"*
from a Cmd.exe.
That should execute the Antlr tool and print out the options expected
for the command. If it doesn't
work, adjust JAVA_HOME and Antlr4ToolPath. JAVA_HOME should be the full
path of the JDK; Antlr4ToolPath should be the full path of the Antlr
tool jar file. If you look at the generated .csproj file for the Antlr
Console program generated, you should see what it defaults if they
aren't set.

# Documentation

For information on how to use AntlrVSIX, see the [User Guide](doc/readme.md), which is geared specifically for Antlr programs.

# Caveats:

* Support for VS2015 and older editions has been removed.
If you are interested in those, you can try using an older version of the extension.

* The grammar used is the standard Antlr4 grammar in the examples: 
https://github.com/antlr/grammars-v4/tree/master/antlr4.

* If you want to make modifications for yourself, you should [reset your
Experimental Hive for Visual Studio](https://docs.microsoft.com/en-us/visualstudio/extensibility/the-experimental-instance?view=vs-2017). To do that,
Microsoft recommends using CreateExpInstance.exe.
Unfortunately, I've found CreateExpInstance doesn't always work because it copies from
previous hives stored under the AppData directory. It is often easier to
just recursively delete all directories ...\AppData\Local\Microsoft\VisualStudio\16.0_*.

* Use Visual Studio 2019 to build the extension.

## New in v4.0:

* Symbol table implementation added. Workspace implementation added. Symbol definitions added to Intellisense pop-ups.

## New in v3.0:

* Supports Java, Python, Rust, Antlr in various stages. Description of languages abstracted into a "Grammar Description".

## New in v2.0:

* The extension will support VS 2019 and VS 2017.

* A menu for the extension will be added to a submenu under Extensions. The functionality provided will
duplicate that in context menus.

* The source code build files will be updated and migrated to the most recent version
of .csproj format that is compatible with VS extensions. Unfortunately, updating to the latest
(version 16) is not possible.

* With my [Antlr4BuildTasks](https://www.nuget.org/packages/Antlr4BuildTasks/) NuGet package,
.g4 files can be automatically compiled to .cs input within
VS 2019 without having to manually run the Antlr4 Java tool on the command line.
Building of the extension itself will be upgraded to use the Antlr4BuildTasks package.

* Listener and Visitor classes are generated for a grammar with a right-click
menu operation. For Listeners, there are two methods associated with a nonterminal.
Depress the control key to select the Exit method, otherwise it will
select the Enter method.

* An options menu is provided to turn on incremental parsing. By default, incremental
* parsing is off because it is very slow.

## New in v1.2.4:

* The extension is now both VS 2017 and 2015 compatible.

* The results windows of Antlr Find All References is now "Antlr Find Results".

## New in v1.2.3:

* Color selection through VS Options/Environment/Fonts and Colors. Look for "Antlr ..." named items.

* Bug fixes with Context Menu entries for AntlrVSIX. AntlrVSIX commands are now only visible when cursor positioned at an Antlr symbol in the grammar. This fixes the segv's when selecting AntlrVSIX commands in non-Antlr files.

Any questions, email me at ken.domino <at> gmail.com
