using System;

namespace Umfrage.Extensions
{
	static  class FuncExtensions {

		internal static Func<A,C> Compose<A,B,C>( this Func<A,B> f1 , Func<B,C> f2) {
			return (a) => f2(f1(a));
		}

	}
}
