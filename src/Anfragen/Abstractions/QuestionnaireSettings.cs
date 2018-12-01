using System;
using System.Collections.Generic;
using System.Text;

namespace Anfragen.Abstractions
{
	public class QuestionnaireSetting
	{
		public string WelcomeMessage { get; set; } = "";
		public string QuestionIcon { get; set; } = "?";
		public ConsoleColor HintColor { get; set; } = ConsoleColor.Gray;
		public ConsoleColor AnswerColor { get; set; } = ConsoleColor.DarkCyan;
		public ConsoleColor QuestionColor { get; set; } = ConsoleColor.DarkGray;
		public ConsoleColor QuestionIconColor { get; set; } = ConsoleColor.DarkGreen;

		public string ValidationIcon { get; set; } = ">>";
		public ConsoleColor ValidationIconColor { get; set; } = ConsoleColor.DarkRed;
	}
}
