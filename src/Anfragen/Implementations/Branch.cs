using System.Collections.Generic;
using Anfragen.Interfaces;
using System;
namespace Anfragen.Implementations {

    public class Branch : IBranch {

        public string Name { get; }

        public Branch( string name ) {
            this.Name = name;
            this._questions = new List<Question>( );
        }

        private List<Question> _questions;
        public IEnumerable<Question> Questions => this._questions;

        public IBranch Add( Question question, int? position = null ) {

            if ( position < 0 ) {
                throw new InvalidOperationException( $"The {nameof( position )} is invalid" );
            }

            if ( position.HasValue ) {
                this._questions.Insert( position.Value, question );
            } else {
                this._questions.Add( question );
            }

            return this;
        }
    }
}
