using System;

using Umfrage.Abstractions;

namespace Umfrage.Extensions
{

	public static class IQuestionExtensions {
		public static void ClearLine(this IQuestion question, int? line) {

			if (!line.HasValue) { line = Console.CursorTop; }

			Console.SetCursorPosition(0, line.Value);
			Console.Write(new string(' ', Console.WindowWidth));
			Console.SetCursorPosition(0, line.Value);
		}

		public static void ClearLines(this IQuestion question, int from, int to) {
			if (to < from) {
				throw new InvalidOperationException($"{nameof(to)} cannot be less than {nameof(from)}");
			}

			for (int i = from; i <= to; i++) {
				question.ClearLine(i);
			}
		}

		public static void ClearAnswer(this IQuestion question, int line, int? col = null ) {
			int left =   col ??
							question.Questionnaire.Settings.QuestionIcon.Length
						+   1
						+   question.Text.Length
						+   1
						+   ( question.Hint.Length > 0 ? ( question.Hint.Length +   4 ) : 0 );
			Console.SetCursorPosition(left, line);
			Console.Write(new string(' ', question.Answer?.Length ?? Console.WindowWidth - left ));
			Console.SetCursorPosition(left, line);

		}

	}
}
