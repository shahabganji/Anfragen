using Anfragen.Abstractions;
using Moq;
using System;
using System.IO;
using Xunit;

namespace Anfragen.Test {

	public class QuestionTests {

		[Fact]
		public void Print_the_result_of_a_question() {

			string fake_answer = "Shahab";
			using (StringWriter output = new StringWriter()) {
				using (StringReader input = new StringReader(fake_answer)) {

					// arrange
					Mock<IUserTerminal> mock_user_terminal = new Mock<IUserTerminal>();
					mock_user_terminal.SetupGet(x => x.Printer).Returns(output);
					mock_user_terminal.SetupGet(x => x.Scanner).Returns(input);


					Mock<IQuestionnaire> mock = new Mock<IQuestionnaire>();
					mock.SetupGet(x => x.Terminal).Returns(mock_user_terminal.Object);
					mock.SetupGet(x => x.Settings).Returns(new QuestionnaireSetting());

					string text = "What's your name?";
					MockQuestion question = new MockQuestion( text, mock.Object );

					question.Ask();
					output.GetStringBuilder().Clear();

					// act
					question.PrintResult();

					// assert
					string result = output.ToString( );
					string expected = $"{text}: {fake_answer}{Environment.NewLine}";

					Assert.Equal(expected, result);

				}
			}
		}

		[Fact]
		public void Ask_a_question_should_print_icon_question_and_answer_without_hint() {

			string fake_answer = "Shahab";
			using (StringWriter output = new StringWriter()) {
				using (StringReader input = new StringReader(fake_answer)) {

					// arrange
					Mock<IUserTerminal> mock_user_terminal = new Mock<IUserTerminal>();
					mock_user_terminal.SetupGet(x => x.Printer).Returns(output);
					mock_user_terminal.SetupGet(x => x.Scanner).Returns(input);


					Mock<IQuestionnaire> mock_questionnaire = new Mock<IQuestionnaire>();
					mock_questionnaire.SetupGet(x => x.Terminal).Returns(mock_user_terminal.Object);
					mock_questionnaire.SetupGet(x => x.Settings).Returns(new QuestionnaireSetting());

					string text = "What's your name?";
					MockQuestion question = new MockQuestion( text, mock_questionnaire.Object );

					question.Ask();

					// assert
					string result = output.ToString( );
					string expected = $"{question.Questionnaire.Settings.QuestionIcon} {text} {fake_answer}";

					Assert.Equal(expected, result);

				}
			}
		}

		[Fact]
		public void Calling_Finshish_method_should_change_state() {

			using (StringWriter output = new StringWriter()) {
				// arrange
				Mock<IUserTerminal> mock_user_terminal = new Mock<IUserTerminal>();
				mock_user_terminal.SetupGet(x => x.Printer).Returns(output);

				string text = "What's your name?";
				MockQuestion question = new MockQuestion( text );

				QuestionStates state_before_call = question.State;

				// act
				question.Finish();

				// assert
				Assert.Equal(QuestionStates.Initilaized, state_before_call);
				Assert.Equal(QuestionStates.Finished, question.State);

			}
		}

		[Fact]
		public void Calling_Finshish_method_should_change_state_and_calls_lambda() {

			using (StringWriter output = new StringWriter()) {
				// arrange
				Mock<IUserTerminal> mock_user_terminal = new Mock<IUserTerminal>();
				mock_user_terminal.SetupGet(x => x.Printer).Returns(output);

				string text = "What's your name?";
				MockQuestion question = new MockQuestion( text );

				string test_variable = "";

				// act
				question.Finish(x => {
					test_variable = "done called from Finish method";
				});


				// assert
				Assert.Equal("done called from Finish method", test_variable);
				Assert.Equal(QuestionStates.Finished, question.State);

			}
		}

		[Fact]
		public void Constructing_a_Question_without_a_text_should_throw_exception() {

			Assert.Throws<ArgumentNullException>(() => {
				MockQuestion question = new MockQuestion(null);
			});
		}
		
		[Fact]
		public void A_Null_validator_should_fail() {

			// arrange
			Mock<IQuestionnaire> mock_questionnaire = new Mock<IQuestionnaire>();

			string text = "What's your name?";
			MockQuestion question = new MockQuestion( text, mock_questionnaire.Object );


			// assert
			Assert.Throws<ArgumentNullException>(() => {

				// act
				question.Validator(null, "Invalid");
			});
		}

		[Fact]
		public void A_Null_error_message_should_fail() {

			// arrange
			Mock<IQuestionnaire> mock_questionnaire = new Mock<IQuestionnaire>();

			string text = "What's your name?";
			MockQuestion question = new MockQuestion( text, mock_questionnaire.Object );


			// assert
			Assert.Throws<ArgumentNullException>(() => {

				// act
				question.Validator(x => true, null);
			});
		}

	}
}
