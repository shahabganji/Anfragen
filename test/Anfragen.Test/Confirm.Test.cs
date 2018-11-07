using System;
using System.IO;
using Anfragen.Implementations;
using Anfragen.Interfaces;
using Xunit;

namespace Anfragen.Test {

    public class ConfirmTest {

        //public ConfirmTest( ) {
        //    StreamWriter standardOut =
        //        new StreamWriter( Console.OpenStandardOutput( ) );
        //    standardOut.AutoFlush = true;
        //    Console.SetOut( standardOut );
        //}

        [Fact]
        public void Confrim_Prints_Question( ) {

            StreamWriter standardOut =
                new StreamWriter( Console.OpenStandardOutput( ) );
            standardOut.AutoFlush = true;
            Console.SetOut( standardOut );

            // Given
            using ( var output = new StringWriter( ) ) {
                Console.SetOut( output );

                IPrinter printer = new ConsoleWriter( );

                //When
                var question = "Are you OK?";
                IQuestion confirm_question = new Confirm( question, new[ ] { "Saeed", "Shahab" } );

                confirm_question.Ask( printer );
                //Then
                Assert.Equal( question, output.ToString( ) );
            }

        }
    }
}