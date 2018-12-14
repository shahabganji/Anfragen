using System;
using System.IO;
using Umfrage.Implementations;
using Xunit;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace Umfrage.Test {

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

                var cw = new UserConsole( );

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

                var cw = new UserConsole( );

                //When
                var data = cw.Scanner.ReadLine( );

                //Then
                Assert.Equal( "Hello", data );
            }
        }
    }
}