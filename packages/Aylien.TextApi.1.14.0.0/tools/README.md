About
=====

This is the C# client library for AYLIEN's APIs. If you haven't already done so, you will need to [sign up](https://developer.aylien.com/signup).

Frameworks supported
====================

- .NET 4.0 or later

Installation
============

To install, use nuget package manager:

```PowerShell
PM> Install-Package Aylien.TextApi
```

See the [Developers Guide](https://developer.aylien.com/docs) for additional documentation.

Example
=======

```cs
using Aylien.TextApi;
using System;

namespace ConsoleApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            Client client = new Client("YourApplicationID", "YourApplicationKey");
            Sentiment sentiment = client.Sentiment(text: "John is a very good football player!");
            Language language = client.Language(text: "John is a very good football player!");

            Console.WriteLine("Sentiment: {0} ({1})", 
                sentiment.Polarity, sentiment.PolarityConfidence);
            Console.WriteLine("Language: {0} ({1})",
                language.Lang, language.Confidence);
        }
    }
}
```
