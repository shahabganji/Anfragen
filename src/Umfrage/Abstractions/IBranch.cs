using System;
using System.Collections.Generic;
using System.Text;

namespace Umfrage.Abstractions
{
	public interface IBranch
	{
		string Name { get; } // the name of the branch
		IEnumerable<IQuestion> Questions { get; }
		IBranch Add(IQuestion question, int? position = null); // adds a question ton the end of the list 
	}

}
