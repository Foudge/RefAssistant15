# Reference Assistant 15

## Project Description
Reference Assistant is free, open source tool to remove unused references from C#, F#, VB.NET or VC++/CLI projects in the Visual Studio 2010/11. It's developed in C#.

The [original version](https://github.com/vchistov/ref-assistant-legacy) targeted Visual Studio 2010-2013.

## Quick Information
Often a .NET project has some references that are not used by any types of its project. When creates new project, the Visual Studio adds several assemblies by default. Types from these assemblies can be used in the future. 

How to detect what assembly reference is useful and what is not? Simplest way checking the used types. But what if a project is so big? Well, it is possible to check an assembly manifest. Yes, assemblies references which are required for runtime will be included in the manifest. But there is one more problem. The assembly can be required indirectly. For example, if you use some type then all assemblies containing definitions of its base types (classes and interfaces from type's hierarchy) should be in a project's references. It is just one example. 

The Reference Assistant checks several criterions to decide to remove assembly reference or not:

* checks a project assembly's manifest (for VC++/CLI doesn't check);
* checks the hierarchy of used classes;
* checks the hierarchy of used interfaces;
* checks the used attributes and their parameters types;
* checks the [forwarded types](https://msdn.microsoft.com/en-us/library/system.runtime.compilerservices.typeforwardedtoattribute.aspx) (some type can be moved from one assembly to another);
* checks the imported types (e.g. COM types);
* checks the used members of types (methods, properties, fields, events) and them parameters types;
* checks the types declared in XAML (Silverlight, WPF, Workflow 4.0, Windows Phone)

The Reference Assistant uses the [Mono.Cecil](https://www.nuget.org/packages/Mono.Cecil/) package for reading assembly metadata.

## Visual Studio Gallery
The Reference Assistant is available for downloading under the repository's [Releases](https://github.com/devio-at/RefAssistant15/releases)

Original versions of the Reference Assistant can be found on the Visual Studio Gallery:
* [Reference Assistant for Visual Studio 2010](http://visualstudiogallery.msdn.microsoft.com/fc504cc6-5808-4da8-ae86-8d3f9ed81606)
* [Reference Assistant for Visual Studio 11](http://visualstudiogallery.msdn.microsoft.com/ff717422-f6a7-4f18-b972-f7540eaf371e)

## Prerequisites for Developers
If you want to change the Reference Assistant's source code for yourself and to compile it after that, you need to install the [Visual Studio extension development workload](https://docs.microsoft.com/en-us/visualstudio/extensibility/installing-the-visual-studio-sdk?view=vs-2017) in Visual Studio 2017 under `Tools` > `Get Tools and Features`.
