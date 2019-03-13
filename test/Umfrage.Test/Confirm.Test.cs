using System.IO;
using Umfrage.Implementations;
using Xunit;

namespace Umfrage.Test {

	public class ConfirmTest {

		[Fact]
		public void Should_Possible_Answers_Be_Filled() {

			string[ ] options = new string[ ] { "option 1", "option 2" };

			Confirm confirm = new Confirm( "some questions" , possibleAnswers: options );

			Assert.Same(confirm.PossibleAnswers, options);
		}

		[Fact]
		public void Should_Answer_Be_Taken() {

			using (StringWriter writer = new StringWriter()) {
				using (StringReader reader = new StringReader("Answer")) {

				


				}

			}
		}
	}

}