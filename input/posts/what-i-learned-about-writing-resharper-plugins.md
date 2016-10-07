Title: What I Learned About Writing ReSharper Plugins
Published: 9/4/2013
Tags:
  - ReSharper
---

Recently I wrote a plug-in for ReSharper, [You Can't Spell](https://bitbucket.org/aarondandy/youcantspell/).
It was just a simple spell check tool that I wanted to write as a weekend project.
Yeah right! Nearly two months later my weekend project was finished and I released it.
This post contains some tips and strategies I learned from my experience.

## What You Need

* [WiX Toolset](http://wixtoolset.org/)
* Visual Studio Pro+ *
* [ReSharper](https://www.jetbrains.com/resharper/buy) *
* [ReSharper SDK](https://www.jetbrains.com/resharper/download/index.html#related&section=resharper-sdk) (for each version you will target)
* [ReSharper SDK Documentation](https://confluence.jetbrains.com/display/NETCOM/ReSharper+7+Plugin+Development) &larr; read this

\* Be aware that some of these things cost lots money which many of us have little of. There are some solutions for you!

* You are a college student.
  - Check with your school to see if they offer MSDNAA. Probably the CS/IS/IT department.
  - JetBrains offers cheaper academic pricing. If you blew the leftovers of your loan check on other stuff then get into open source for the experience and maybe a free ReSharper license.
* You own a small/tiny software "business."
  - Look into [BizSpark](http://www.microsoft.com/bizspark/)
* You do open source stuff.
  - Start a "business" then see above.
  - E-mail JetBrains and ask for free stuff; you should reach a human. Also mention the plug-in you would like to write that may enhance their revenue.

## Solution and Project Organization

Note: You may want to grab a copy of my project from [BitBucket](https://bitbucket.org/aarondandy/youcantspell) using [Mercurial](http://tortoisehg.bitbucket.org/download/index.html).

### Projects and Code Organization (my way)

It helps to decide first what versions of Visual Studio and ReSharper you will be targeting.
Each ReSharper version that you will target should have its own project as it will reference
a different ReSharper SDK and there may be breaking code changes between each version.

For each project you should define Conditional compilation symbols that describe the version of ReSharper the project is targeting
([example](https://bitbucket.org/aarondandy/youcantspell/src/d2de83a7c3344270803bd55eabe4c0cba8d8d7f2/src/YouCantSpell.ReSharper.v71/YouCantSpell.ReSharper.v71.csproj?at=default#cl-22)).
I define two for each project; One for the major version and another or the major and minor version.
For example, MyPlugin.ReSharper.v71 would have RSHARP7 and RSHARP71 defined.
This lets me use the preprocessor to conditionally compile different sections of code depending on what version of the API I am compiling against
([example](https://bitbucket.org/aarondandy/youcantspell/src/d2de83a7c334/src/YouCantSpell.ReSharper.Shared/SpellingQuickFixBase.cs?at=default#cl-60)).

Each project also shares the same root namespace but will have a different assembly name corresponding to the ReSharper version
([example](https://bitbucket.org/aarondandy/youcantspell/src/d2de83a7c334/src/YouCantSpell.ReSharper.v70/YouCantSpell.ReSharper.v70.csproj?at=default#cl-12)).
This helps prevent any confusion over which assemblies reference which SDK version.

Each of my plug-in projects has very little code within their directories.
Instead I put all of the code into a
[shared folder](https://bitbucket.org/aarondandy/youcantspell/src/d2de83a7c334/src/YouCantSpell.ReSharper.Shared?at=default)
(_not_ a common project or assembly) and each project references the same code using links
([example](https://bitbucket.org/aarondandy/youcantspell/src/d2de83a7c334/src/YouCantSpell.ReSharper.v71/YouCantSpell.ReSharper.v71.csproj?at=default#cl-50)).
I then use the preprocessor to handle different versions of the API
([example](https://bitbucket.org/aarondandy/youcantspell/src/d2de83a7c334/src/YouCantSpell.ReSharper.Shared/SpellingQuickFixBase.cs?at=default#cl-60)).
A nice time saver with linked files is that you can make links that go to entire folders using wild-cards but eventually Visual Studio will flatten them out into individual items.

If you go with this project setup you will find that it is extremely important to keep as much code as is possible in a shared assembly to
simplify the creation of new plug-in projects and ease maintenance. Maintaining the linked files can be error prone and annoying.

### Assembly Versions

Assembly versions and installer versions are somewhat important.
For any given release of your plug-in you may want all of your files to share a common version number.
I am lazy and use the `N.M.*` format but if you want to Do It Right use source control branching and a CI system to generate version numbers.
You can have a common file with
[shared assembly information](https://bitbucket.org/aarondandy/youcantspell/src/d2de83a7c3344270803bd55eabe4c0cba8d8d7f2/src/YouCantSpellAssemblyInfo.cs?at=default)
and [link it](https://bitbucket.org/aarondandy/youcantspell/src/d2de83a7c3344270803bd55eabe4c0cba8d8d7f2/src/YouCantSpell.ReSharper.v71/YouCantSpell.ReSharper.v71.csproj?at=default#cl-147)
within your projects. Then your
[installer version can be derived from the assembly version](https://bitbucket.org/aarondandy/youcantspell/src/d2de83a7c334/build/YouCantSpell.Installer.v71/YouCantSpell.Installer.v71.wixproj?at=default#cl-23)
so everything matches up nicely.
Keep in mind that because of the way the MSI system works *only the first 3 parts of a version are significant* for upgrades.

### Installer

I use the [WiX Toolset](http://www.wixtoolset.org/) to generate MSI packages.
I don't really know what I am doing but hopefully my
[example wxs](https://bitbucket.org/aarondandy/youcantspell/src/d2de83a7c334/build/YouCantSpell.Installer.v71/Product.wxs?at=default)
can help you out. Something important to note is that I install to the user's local "AppData" folder instead of the Program Files folder.
This is because Visual Studio is sometimes run under a restricted account and may not have needed access
to the Program Files directory to run the plug-in.

### Visual Studio 2010 Solution Issues

There is an issue with Visual Studio 2010 where it will only properly debug a ReSharper plug-in that is targeting .NET 4.0.
If you wish to release the plug-in against .NET 3.5 you will need to have two projects, one for debugging and another for release.
I have an [example](https://bitbucket.org/aarondandy/youcantspell/src/d2de83a7c3344270803bd55eabe4c0cba8d8d7f2/src?at=default)
on my project site of this structure.

## Debugging

### Debug Performance

Debugging ReSharper and your plug-in is brew-a-cup-of-coffee slow! If you want to ever get anything done, never start with the debugger attached unless you need to.

1. Start with `Ctrl+F5` or "Debug &rarr; Start Without Debugging"
2. Open your ConsoleApplication42 that you want to debug your plug-in against
3. Then right before you trigger an action that would reach your breakpoint or throw an exception attach the debugger: "Debug &rarr; Attach to Process..."
4. Detach when done: "Debug &rarr; Detach all"

### Project Issues

Your plug-in project should be configured to launch a new Visual Studio instance with the debugger attached and with your plug-in loaded.
Normally it works but I ran into issues with this.

* If your plug-in is installed on the development system ReSharper will get confused (two of the same plug-in). Make sure to un-install your plug-in if you installed it before debugging it.
* If you set the project up wrong or didn't copy the debug launch settings to another computer maybe, you will need to reconfigure that.
  1. Right click your project then select "Properties"
  2. Click the "Debug" tab on the left
  3. Select "Start external program:" and enter the path to your `devenv.exe` (Visual Studio)
  4. Set the "Command line arguments" to: `/ReSharper.Plugin <your-plugin>.dll`, obviously substituting your plug-in DLL file.
  5. Set the "Working directory" to: `<the-folder-containing-your-plugin-assembly>\`

### Visual Studio 2010 Debug Issue

If you are using Visual Studio 2010 and the debugger is behaving as if it is not attached see the above section: Visual Studio 2010 Solution Issues.
You may need to target .NET 4.0 for debugging to work.

## Testing

### Unit Testing

If you are going to write tests for your plug-in I recommend you do as much unit testing as possible on your shared assemblies.
The integration testing is in my opinion very difficult and tedious to get right.

### Integration Testing

JetBrains provides a bit of framework to help you test your plug-in but I still found it difficult and very tedious when working with multiple versions of ReSharper.
I highly recommend using the ReSharper Plug-In Tests project wizard to create your plug-in test projects instead of starting from scratch.
The testing works by
[running your plug-in](https://bitbucket.org/aarondandy/youcantspell/src/d2de83a7c334/src/YouCantSpell.ReSharper.v61.Test/SpellingHighlightingTest.cs?at=default)
against
[input files](https://bitbucket.org/aarondandy/youcantspell/src/d2de83a7c334/test/data/Simple/Highlight/Mispeled.cs?at=default)
and comparing the result against
[gold files](https://bitbucket.org/aarondandy/youcantspell/src/d2de83a7c334/test/data/Simple/Highlight/Mispeled.cs.gold?at=default)
which you can either write or rename after a test is first run.

### Assembly Reference Issues

One thing I found out the hard way is that your test assemblies _may not_ share the same output folder as your plug-in.
The ReSharper Plug-In Tests project wizard adds a bunch of assemblies that are required to run the tests but mess with your plug-in during debug.
Because of this I have a
[special output folder for my test](https://bitbucket.org/aarondandy/youcantspell/src/d2de83a7c3344270803bd55eabe4c0cba8d8d7f2/src/YouCantSpell.ReSharper.v61.Test/YouCantSpell.ReSharper.v61.Test.csproj?at=default#cl-21)
projects.

## Dependency injection and Inversion of Control

When you are working on a plug-in and need an instance of a ReSharper class you can probably get one from their IoC containers or have it injected into your classes.
Most of your classes that implement ReSharper interfaces seem to receive their parameters through dependency injection.
You can sometimes find ways to access needed instances from parameters or members you already have but may need to call directly to one of their IoC containers.
You can even use their IoC containers for your own classes.

```csharp
[ShellComponent] // Tell ReSharper to manage the lifetime of this class.
public class MyStuff {
	public MyStuff([NotNull] ISettingsStore someRequiredStuff){
		// 'someRequiredStuff' will be injected by ReSharper.
	}
	public string ImportantStuff { get; private set; }
}
// ...
public class DoesStuff {
	public void ProcessThings(IEnumerable things){
		var myStuff = Shell.Instance.GetComponent&lt;MyStuff&gt;();
		// do stuff...
	}
}
```

### IoC Issues

I ran into some issues getting instances from the component system.
Specifically in my settings form I could not figure out how to access an instance of my own class marked as a `ShellComponent`.
I had to resort to having a `WeakReference` that pointed to the most recent instance.
I couldn't just create a singleton as ReSharper is supposed to manage the lifetime of the object.
It feels dirty but that was the best I could come up with at the time.

```csharp
[ShellComponent] // Tell ReSharper to manage the lifetime of this class.
public class MyStuff {
	private static WeakReference _mostRecentInstance;
	public static MyStuff Instance {
		get {
			var instance = Shell.Instance.GetComponent&lt;MyStuff&gt;();
			return (null == instance && null != _mostRecentInstance)
				? _mostRecentInstance.Target as MyStuff // dirty hack
				: instance;
		}
	}
	public MyStuff([NotNull] ISettingsStore someRequiredStuff){
		_mostRecentInstance = new WeakReference(this);
	}
}
```

## Processing Nodes

ReSharper parses the code files into a tree for you, its pretty handy.
ReSharper also allows you to manipulate that tree and have the changes reflected in the editor.
This way you can edit and process code files in an object oriented way.
It works out pretty well once you get the hang of their way of doing things.

### Declarations

There is a class of node called a declaration that can be identified by the IDeclaration interface.
I often ran into situations where the node I had was an ITreeNode when I needed an IDeclaration but the node I had did not implement that interface.
It turns out that if the node you have does not implement the IDeclaration interface its parent or grandparent may.
A variable identifier for example may be represented by both a parent and child node; one of which may be the IDeclaration you are looking for.

```csharp
[CanBeNull]
public static IDeclaration GetDeclaration(ITreeNode node) {
	while (null != node) {
		var declaration = node as IDeclaration;
		if (null != declaration)
			return declaration;
		node = node.Parent;
	}
	return null;
}
```

### Declared Elements

After you get a declaration for a tree node you can get the declared element which can give you even more information or instances that you need.

```csharp
[CanBeNull]
public static IDeclaredElement GetDeclaredElement(ITreeNode node) {
	var declaration = GetDeclaration(node); // see above
	return null == declaration ? null : declaration.DeclaredElement;
}
```

### Node Locations

A tree node has a location within a document.
For simple documents like a C# code file this is pretty straight forward.
For an MVC razor document however things get tricky.
Consider that a cshtml file can contain HTML, JavaScript, and C#.
ReSharper handles documents like this by treating them as multiple documents.
Now not only do you need to worry about where in a document your node is but which document.

Nodes can offer what is called a `TreeTextRange` that has information about the location of the node.
You can get an instance of this from the `GetTreeTextRange()` method.
Additionally some more specific interfaces have different ranges they can offer. 
For example the `IComment` interface provides a `GetCommentRange()` method.

Typically though you will need to work with a `DocumentRange` instance.
As I understand it this offers more context regarding the text range.
Because of the complexity involved with multiple documents you have to go through some extra steps to make sure you get the right instance.

The first thing you need is an instance of `IFile`. This example is for an `ICSharpFile`.

```csharp
public static ICSharpFile GetCSharpFile(IPsiSourceFile sourceFile) {
#if RSHARP6
	return sourceFile.GetPsiFile(CSharpLanguage.Instance) as ICSharpFile;
#else
	return sourceFile.GetPsiFiles&lt;CSharpLanguage&gt;()
		.OfType&lt;ICSharpFile&gt;().SingleOrDefault();
#endif
}
```

Then using that `IFile` instance you can get the correct document range.

```csharp
var codeFile = GetCSharpFile(PsiSourceFile);
var documentRange = codeFile.GetIntersectingRanges(range)
	.First(x => x.Document == PsiSourceFile.Document);
```

This document range can be used to create a ReSharper highlighting in the correct spot within the code file you are processing.

## Manipulating Nodes and Trees

Manipulating the node tree can be somewhat tricky.
Each language seems to have a different way of doing things.

### CRUD for Nodes

A handy utility class for mutating the document tree is the `ModificationUtil`.
It allows you to add, delete, and replace nodes in the document tree.

```csharp
ModificationUtil.ReplaceChild(oldStringLiteralNode, newStringLiteralNode);
```

### Creating C# Comment Nodes

```csharp
var elementFactory = CSharpElementFactory.GetInstance(node.GetPsiModule());
var newComment = elementFactory.CreateComment(newText);
```

### Creating C# String Literal Nodes

```csharp
var elementFactory = CSharpElementFactory.GetInstance(node.GetPsiModule());
// NOTE: newText needs quotes "" or literal quotes @@""
var newStringLiteral = elementFactory.CreateExpression("$0", newText);
```

### Creating HTML Nodes

```csharp
var elementFactory = HtmlElementFactory.GetInstance(node.Language);
// NOTE: may return multiple nodes, all of them should be used together:
var newElements = elementFactory.CompileText(someNewHtmlText, node).ToList();
// ...
// EXAMPLE: replace one node with the result of CompileText:
if(newElements.Count > 0){
	var recentNode = ModificationUtil.ReplaceChild(oldNode, newElements[0]);
	for(int i = 1; i < newElements.Count; i++) {
		recentNode = ModificationUtil.AddChildAfter(
			recentNode.Parent, recentNode, newElements[i]);
	}
}
```

### Creating JavaScript Nodes

```csharp
[CanBeNull]
private static ITreeNode CreateNode(
	[NotNull] IPsiSourceFile file,
	[NotNull] IPsiModule module,
	[NotNull] LanguageService languageService,
	string javaScriptText
) {
	// parse the JavaScript text into new node instances
	var lexer = languageService.GetPrimaryLexerFactory()
		.CreateLexer(new StringBuffer(javaScriptText))
	var parser = languageService.CreateParser(
		lexer,
		module,
		file
	) as IJavaScriptParser;
	if (null != parser) {
		var newNodes = parser.ParseFile();
		if (null != newNodes)
			return newNodes.LastChild;
	}
	return null;
}
```

## Identifier Names

You can use ReSharper's built in naming utilities.
First you have to get access to the naming manager and provider.

```csharp
public class MyDaemonStageProcess : IDaemonStageProcess{
	public MyDaemonStageProcess(
		[NotNull] IDaemonProcess process,
		[NotNull] IContextBoundSettingsStore settingsStore,
		[NotNull] PsiLanguageType languageType
	) {
		PsiSourceFile = process.SourceFile;
		var psiServices = PsiSourceFile.PsiModule.GetPsiServices();
		// get the naming manager
		NamingManager = psiServices.Naming;
		// get the naming policy provider
		NamingPolicyProvider = psiServices.Naming.Policy
			.GetPolicyProvider(languageType, process.SourceFile, settingsStore);
	}
	// ...
}
```

After you have an instance of the naming manager and provider you can then parse names using ReSharper.

```csharp
public Name ParseName(string name){
	return NamingManager.Parsing.Parse(
		name, NamingRule.Default, NamingPolicyProvider);
}

public Name ParseName(IIdentifier identifier) {
	var declared = GetDeclaredElement(identifier); // see above
	var namingRule = null == declared
		? NamingRule.Default
		: NamingPolicyProvider.GetPolicy(declared).NamingRule
	return NamingManager.Parsing.Parse(
		identifier.Name, namingRule, NamingPolicyProvider);
}
```

## ReSharper Settings

ReSharper lets you use their settings system for your own plug-in.
There are plenty of advantages to this so consider it before you just go dropping XML files in some folder.

### List

Saving primitive variables to the settings is simple enough but saving collections of strings for example is not so simple.
ReSharper does not offer any kind of simple collection setting type but they do offer an index entry type.
The index entry type works like a generic dictionary so if I need a list of strings I can use an index entry
of type `IIndexEntry<string,byte>` where the key contains the string I wish to store and the byte will contain garbage.

You define an index entry like in the following example.

```csharp
[SettingsKey(typeof(EnvironmentSettings), "My Settings")]
public class MySettings {
	[SettingsIndexedEntryAttribute("Some Names")]
	public IIndexedEntry&lt;string, byte&gt; SomeNames { get; set; }
}
```

You can then enumerate and modify the settings collection like the next example.

```csharp
// read all
var someWords = settings
	.EnumEntryIndices&lt;SpellCheckSettings, string, byte&gt;(x => x.SomeNames)
// set an item
settings.SetIndexedValue((MySettings x) => x.SomeNames, name, default(byte));
// remove an item
settings.RemoveIndexedValue((MySettings x) => x.SomeNames, name);
```

This method ends up behaving more like a `HashSet<string>`.
If you need something like a true list you can do that to too.
You still need to use key value pairs but you can use the list index as the key and the list item as the value: `IIndexEntry<int,string>` .

## Refactoring

### Name Changes

If you dig around in the sample project you can find a utility class that triggers ReSharper renames for you.
I don't know of any documentation for it or what the magic values mean but so far it is the best way I know of to do it.
You can find this magical beast within your ReSharper SDK folder at "\SDK\Samples\SamplePlugin\SamplePlugin\src\CallRename\CallRenameUtil.cs" .

## Icons

In ReSharper 6 you could just give your options pane an embedded PNG file, that was nice.
In ReSharper 7 it was changed to something a bit more complex.
You have to jump through some code generation hoops the first time but it should be smooth after that.
If for some reason you have trouble getting icons to work, try the following steps:

1. Right click your project and select "Unload"
2. Right click your project again and select "Edit ..."
3. Scroll all the way down to the bottom. You will want something that looks like this:
   ```csharp
   <Import Project="$(MSBuildToolsPath)\Microsoft.CSharp.targets" />
   <Import Project="$(ReSharperSdkTargets)\Plugin.Targets" />
   <Import Project="$(ReSharperSdkTools)\MSBuild\JetBrains.Build.Platform.ThemedIconsConverter.Targets" />
   ```
4. Right click your project a 3rd time and select "Reload"
5. You should have some folder in your project that you can put resources in, I called mine "resources"
6. Make sure your PNG icon is in that folder
7. Right click the PNG and select Properties
8. In the properties pane make sure the BuildAction is "ThemedIconPng"
9. Build your project again. When you build the project the "ThemedIconPng" action will generate some new files for you
10. Right click your resources folder and select "Add Existing Item..."
11. Add the generated cs and xaml files. You may need to set the filter to All Files
12. You should be good to go now

You can attach the icon to a settings form like this:

```csharp
[OptionsPage(
	Pid,
	"My Settings Form",
	typeof(UnnamedThemedIcons.MyIcon),
	ParentId = EnvironmentPage.Pid,
	Sequence = 100
)]
public partial class MyOptionsControl : UserControl, IOptionsPage {
	// ...
}
```

You can use the icon in a bulb item like this:
```csharp
public class MyQuickFix : IQuickFix {
	// ...
	public void CreateBulbItems(BulbMenu menu, Severity severity)
	{
		var defaultGroup = menu.GetOrCreateGroup(Anchor.DefaultAnchor);
		var subMenu = defaultGroup.GetOrCreateSubmenu(new BulbMenuItemViewDescription(
			new Anchor("MyStuff", new AnchorRelation[0]),
			UnnamedThemedIcons.MyIcon.Id,
			"Stuff"
		));
		subMenu.Submenu.ArrangeQuickFixes(Items
			.Select(x => new Pair&lt;IBulbAction, Severity&gt;(x,severity)));
	}
	// ...
}
```