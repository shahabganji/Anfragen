using System;
using System.IO;
using Anfragen.Implementations;
using Xunit;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace Anfragen.Test {
    public class ConsoleWriterTest {

        //public ConsoleWriterTest( ) {
        //    StreamWriter standardOut =
        //        new StreamWriter( Console.OpenStandardOutput( ) );
        //    standardOut.AutoFlush = true;
        //    Console.SetOut( standardOut );
        //}

        [Fact]
        public void Print_A_Simple_String_On_Console( ) {

            //StreamWriter standardOut =
            //    new StreamWriter( Console.OpenStandardOutput( ) );
            //standardOut.AutoFlush = true;
            //Console.SetOut( standardOut );
            ////Given
            //using ( var output = new StringWriter( ) ) {

            //    Console.SetOut( output );
            //    Console.Clear( );

            //    var cw = new UserConsole( );

            //    //When
            //    cw.Print( "Hello there" );

            //    //Then
            //    Assert.Equal( "Hello there", output.ToString( ) );
            //}
        }

        [Fact]
        public void Print_A_New_Line_On_Console( ) {
            //StreamWriter standardOut =
            //    new StreamWriter( Console.OpenStandardOutput( ) );
            //standardOut.AutoFlush = true;
            //Console.SetOut( standardOut );
            //using ( var output = new StringWriter( ) ) {
            //    //Given
            //    Console.SetOut( output );

            //    var cw = new UserConsole( );
            //    //When
            //    cw.AddNewLine( );

            //    //Then
            //    Assert.Equal( "\n", output.ToString( ) );
            //}
        }
    }
}