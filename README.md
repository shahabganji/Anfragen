# Umfrage ( Questonnaire )

This is a .Net Core plugin enabling you to have questionnaires or console-like wizards in your applications. 

Umfrage inspired by [Yeomon](https://yeoman.io/) and [Inquirer.js](https://github.com/SBoudrias/Inquirer.js/), yet it is implemented and looks different.

### Install

Add `Umfrage` package from [Nuget]() by running the following command

```shell
dotnet add package Umfrage --version 1.0.0
```

### Usage

```csharp
var questionnaire = new Questionnaire( );
var builder = new QuestionBuilder( );

var question = builder.Simple( ).New( "What's your name?" ).Build( );

questionnaire.Add(question);

questionnaire.Start();

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
			new QuestionOption("Persian")
			new QuestionOption("English"),
			new QuestionOption("Italian"),
			new QuestionOption("Spanish"),
			new QuestionOption("French"),
			new QuestionOption("German")
		})
		.AsCheckList()
		.Build();
```