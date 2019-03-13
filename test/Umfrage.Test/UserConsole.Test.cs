using System;
using System.IO;

using Umfrage.Implementations;

using Xunit;

namespace Umfrage.Test
{

	public class UserConsoleTests {

        //public ConsoleWriterTest( ) {
        //    StreamWriter standardOut =
        //        new StreamWriter( Console.OpenStandardOutput( ) );
        //    standardOut.AutoFlush = true;
        //    Console.SetOut( standardOut );
        //}

        [Fact]
        public void Print_A_Simple_String_On_Console( ) {

            using ( var output = new StringWriter( ) ) {

                //Given
                Console.SetOut( output );
                Console.Clear( );

                var cw = new UserTerminal( );

                //When
                cw.Printer.Write( "Hello there" );

                //Then
                Assert.Equal( "Hello there", output.ToString( ) );
            }
        }

        [Fact]
        public void Read_A_Value_From_Console( ) {

            using ( var input = new StringReader( "Hello" ) ) {

                //Given
                Console.SetIn( input );

                var cw = new UserTerminal( );

                //When
                var data = cw.Scanner.ReadLine( );

                //Then
                Assert.Equal( "Hello", data );
            }
        }
    }
}