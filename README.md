csharp-yourls - Yourls API for csharp
====================

This module provides hooks into the [Yourls](http://yourls.org/) API for C#.
It is a quick, idiomatic port of [gabrielpreston](https://github.com/gabrielpreston)'s [node-yourls](https://github.com/gabrielpreston/node-yourls).
For more information about Yourls and what it can do, visit their [API docs](http://yourls.org/#API).

Note
----
YOURLS is a small set of PHP scripts that will allow you to run your own URL shortening service (a la TinyURL). You can make it private or public, you can pick custom keyword URLs.  This means you will need the URL of your, or someone else's, YOURLS installation as well as an API Signature token for that install.

Installation
------------
Using NuGet, `Install-Package yourls`

Using git, `git clone https://github.com/smithrobs/csharp-yourls.git /path/to/yourls`

Usage
-----

	var client = new Yourls.YourlsClient("ph.ly", "1a40d1e654");
	var result = client.Expand("dzg-v");
	Console.WriteLine(result.shorturl);

Tests
-----
Tests make use of [xUnit.net](http://xunit.codeplex.com/)

To run:
	xunit.console.clr4.x86.exe (path to compiled Yourls.Test.dll)
			
Yourls Features
---------------
* shorten
* vanity
* expand
* urlstats
