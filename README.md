# Umfrage ( Questonnaire )    

[![Build status](https://ci.appveyor.com/api/projects/status/xfq7qatie27311t0/branch/master?svg=true)](https://ci.appveyor.com/project/shahabganji/umfrage/branch/master)


This is a .Net Core plugin enabling you to have questionnaires or console-like wizards in your applications. 

Umfrage inspired by [Yeomon](https://yeoman.io/) and [Inquirer.js](https://github.com/SBoudrias/Inquirer.js/), yet it is implemented differently and looks different.

### Install

Add `Umfrage` package from [Nuget]() by running the following command

```shell
dotnet add package Umfrage --version 1.0.0
```

Also another package is avaialable which provides some extension methods, not required necessarily:

```shell
dotnet add package Umfrage.Extensions --version 1.0.0
```

### Usage

```csharp
var builder = new QuestionBuilder( );
var questionnaire = new Questionnaire( );

builder
    .Simple( )
    .New( "What's your name?" )
    .AddToQuestionnaire( questionnaire );

var another = builder.Simple( ).New( "What's your lastname?" ).Build( );
questionnaire.Add(another);

questionnaire.Start();

While( questionnaire.CanProceed ){
    questionnaire.Next();
}

questionnaire.End();

```

Then you can access the values of `processed` questions via `ProcessedQuestions` property of your questionnaire, and decide what you want to do afterwards on your app.

### Question Types

Currently there are four types of questions supported, `prompt`, `confirm`, `single-select-lists`, `multi-select-lists`
 , categorized into `Simple` and `List`.

* **Prompt**

```csharp
var prompt = builder.Simple( )
		.New( "Are you older than 18?" )
		.Build( );
```

* **Confirm**

```csharp
var confirm = builder.Simple( )
		.New( "Are you older than 18?" )
		.AsConfirm( )
		.Build( );
```


* **Single Select List**

```csharp
var list = builder.List()
		.New("What's your favorite language?")
		.AddOptions(new[ ] {
			new QuestionOption("Persian")
			new QuestionOption("English"),
			new QuestionOption("Italian"),
			new QuestionOption("Spanish"),
			new QuestionOption("French"),
			new QuestionOption("German")
		})
		.Build();
```

* **Multi Select List / CheckList**

```csharp
var list = builder.List()
		.New("What's your favorite language?")
		.AddOptions(new[ ] {
			"Persian"
			"English",
			"Italian",
			"Spanish",
			"French",
			"German"
		})
		.AsCheckList()
		.Build();
```

* You can use `constructor` method to create these four types of questions, yet I suggest to use the builder for easier usee

```csharp
var agePrompt = new Prompt("How old are you?");

agePrompt.Validator(x =>
{

	int.TryParse(x.Answer, out int age);

	return age >= 18;

}, "You must be older than 18");

// Use Extension methods in Umfrage.Extensions package
questionnaire.Prompt(agePrompt);    // questionnaire.Add(agePrompt);

```

And at the end you have aceess to the list of asked/processed questions:

```csharp
 // Print Processed questions
foreach ( var q in questionnaire.ProcessedQuestions ) {
    questionnaire.Terminal.Printer.Write( $"{q.Text} : {q.Answer}" );
    questionnaire.Terminal.Printer.WriteLine( );
}
```

