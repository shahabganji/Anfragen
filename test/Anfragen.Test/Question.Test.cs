using System;
using System.IO;
using System.Text;
using Anfragen.Implementations;
using Anfragen.Interfaces;
using Moq;
using Xunit;

namespace Anfragen.Test {

    public class QuestionTests {

        [Fact]
        public void Print_The_Result_Of_A_Question( ) {

            string fake_answer = "Shahab";
            using ( var output = new StringWriter( ) ) {
                using ( var input = new StringReader( fake_answer ) ) {

                    // arrange
                    var mock_user_terminal = new Mock<IUserTerminal>( );
                    mock_user_terminal.SetupGet( x => x.Printer ).Returns( output );
                    mock_user_terminal.SetupGet( x => x.Scanner ).Returns( input );


                    var mock = new Mock<IQuestionnaire>( );
                    mock.SetupGet( x => x.Terminal ).Returns( mock_user_terminal.Object );
                    mock.SetupGet( x => x.Settings ).Returns( new QuestionnaireSetting( ) );

                    var text = "What's your name?";
                    var question = new MockQuestion( text, mock.Object );

                    question.Ask( );
                    output.GetStringBuilder( ).Clear( );

                    // act
                    question.PrintResult( );

                    // assert
                    var result = output.ToString( );
                    var expected = $"{text}: {fake_answer}\n";

                    Assert.Equal( expected, result );

                }
            }
        }
    }
}