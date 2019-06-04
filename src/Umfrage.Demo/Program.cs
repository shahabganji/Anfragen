using Umfrage.Abstractions;
using Umfrage.Builders;
using Umfrage.Builders.Abstractions;
using Umfrage.Implementations;
using System;
using Umfrage.Extensions;

// Test
namespace Umfrage.Demo
{
    internal static class Program
    {
        public static void Main(string[] args)
        {
            var terminal = new UserTerminal();
            IQuestionnaire questionnaire = new Questionnaire(terminal);
            questionnaire.Settings.WelcomeMessage = "Welcome to my questionnaire";

            bool Validator(IQuestion x) => x.Answer.Length > 0;
            var errorMessage = "Please Provide a value";

            IQuestionBuilder builder = new QuestionBuilder();

            var askName = builder.Simple().Text("What's your name?").Build();

            var askFamily = builder.Simple().Text("What's your family?").AddValidation(Validator, errorMessage).Build();

            builder.List()
                .Text("What's your favorite language?")
                .AddOptions(new []
                {
                    new QuestionOption("Persian"),
                    new QuestionOption("English"),
                    new QuestionOption("Italian"),
                    new QuestionOption("Spanish"),
                    new QuestionOption("French"),
                    new QuestionOption("German")
                })
                .WithHint("Persian")
                .WithDefaultAnswer("Persian")
                .AddValidation(x => x.Answer != null)
                //.AsCheckList( )
                .WithErrorMessage("You must select an option")
                .AddToQuestionnaire(questionnaire)
                ;

            questionnaire
                .Add(askName)
                .Add(askFamily)
                ;

            questionnaire.Start();

            var add = true;

            // loop until there is a question to ask
            while (questionnaire.CanProceed)
            {
                if (add)
                {
                    var confirm = builder.Simple()
                        .Text("Are you older than 18?")
                        .AsConfirm()
                        .WithHint("Y/n")
                        .WithDefaultAnswer("y")
                        .AddValidation(x =>
                        {
                            var q = (Confirm) x;
                            return q.PossibleAnswers.Contains(x.Answer);
                        }, "Your value should be either 'y' or 'n'")
                        .Build();

                    confirm.Finish(q =>
                    {
                        if (q.Answer != "y") return;

                        var agePrompt = builder.Simple()
                            .Text("How old are you?")
                            .AddValidation(x =>
                            {
                                int.TryParse(x.Answer, out var age);

                                return age >= 18;
                            }, "Your must be older than 18")
                            .Build();

                        questionnaire.Prompt(agePrompt as Prompt);
                    });

                    questionnaire.Confirm(confirm as Confirm);

                    add = false;
                }

                questionnaire.Next();
            }

            questionnaire.End();

            // Print Processed questions
            foreach (var q in questionnaire.ProcessedQuestions)
            {
                terminal.Printer.Write($"{q.Text} : {q.Answer}");
                terminal.Printer.WriteLine();
            }

            Console.ReadLine();
        }
    }
}
