using System;
using Umfrage.Abstractions;
using Umfrage.Builders;
using Umfrage.Builders.Abstractions;
using Umfrage.Extensions;
using Umfrage.Implementations;

namespace Umfrage.Demo
{
    internal static class Program
    {
        public static void Main(string[] args)
        {
            var terminal = new UserConsole();
            IQuestionnaire questionnaire = new Questionnaire(terminal);
            questionnaire.Settings.WelcomeMessage = "Welcome to my questionnaire";

            bool Validator(IQuestion x) => x.Answer.Length > 0;
            var errorMessage = "Please Provide a value";

            IQuestionBuilder builder = new QuestionBuilder();

            var askName = builder.Simple().New("What's your name?").Build();

            var askFamily = builder.Simple().New("What's your family?").AddValidation(Validator, errorMessage).Build();

            builder.List()
                .New("What's your favorite language?")
                .AddOption(new QuestionOption("Persian"))
                .AddOptions(new[]
                {
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
                .AddToQuestionnaire(questionnaire)
                .WithErrorMessage("You must select an option")
                .Build();

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
                        .New("Are you older than 18?")
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
                            .New("How old are you?")
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
