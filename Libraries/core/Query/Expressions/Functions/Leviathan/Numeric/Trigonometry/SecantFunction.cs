/*

Copyright dotNetRDF Project 2009-12
dotnetrdf-develop@lists.sf.net

------------------------------------------------------------------------

This file is part of dotNetRDF.

dotNetRDF is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

dotNetRDF is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with dotNetRDF.  If not, see <http://www.gnu.org/licenses/>.

------------------------------------------------------------------------

dotNetRDF may alternatively be used under the LGPL or MIT License

http://www.gnu.org/licenses/lgpl.html
http://www.opensource.org/licenses/mit-license.php

If these licenses are not suitable for your intended use please contact
us at the above stated email address to discuss alternative
terms.

*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VDS.RDF.Query.Expressions.Functions.Leviathan.Numeric.Trigonometry
{
    /// <summary>
    /// Represents the Leviathan lfn:sec() or lfn:sec-1 function
    /// </summary>
    public class SecantFunction
        : BaseTrigonometricFunction
    {
        private bool _inverse = false;
        private static Func<double, double> _secant = (d => (1 / Math.Cos(d)));
        private static Func<double, double> _arcsecant = (d => Math.Acos(1 / d));

        /// <summary>
        /// Creates a new Leviathan Secant Function
        /// </summary>
        /// <param name="expr">Expression</param>
        public SecantFunction(ISparqlExpression expr)
            : base(expr, _secant) { }

        /// <summary>
        /// Creates a new Leviathan Secant Function
        /// </summary>
        /// <param name="expr">Expression</param>
        /// <param name="inverse">Whether this should be the inverse function</param>
        public SecantFunction(ISparqlExpression expr, bool inverse)
            : base(expr)
        {
            this._inverse = inverse;
            if (this._inverse)
            {
                this._func = _arcsecant;
            }
            else
            {
                this._func = _secant;
            }
        }

        /// <summary>
        /// Gets the String representation of the function
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            if (this._inverse)
            {
                return "<" + LeviathanFunctionFactory.LeviathanFunctionsNamespace + LeviathanFunctionFactory.TrigSecInv + ">(" + this._expr.ToString() + ")";
            }
            else
            {
                return "<" + LeviathanFunctionFactory.LeviathanFunctionsNamespace + LeviathanFunctionFactory.TrigSec + ">(" + this._expr.ToString() + ")";
            }
        }

        /// <summary>
        /// Gets the Functor of the Expression
        /// </summary>
        public override string Functor
        {
            get
            {
                if (this._inverse)
                {
                    return LeviathanFunctionFactory.LeviathanFunctionsNamespace + LeviathanFunctionFactory.TrigSecInv;
                }
                else
                {
                    return LeviathanFunctionFactory.LeviathanFunctionsNamespace + LeviathanFunctionFactory.TrigSec;
                }
            }
        }

        /// <summary>
        /// Transforms the Expression using the given Transformer
        /// </summary>
        /// <param name="transformer">Expression Transformer</param>
        /// <returns></returns>
        public override ISparqlExpression Transform(IExpressionTransformer transformer)
        {
            return new SecantFunction(transformer.Transform(this._expr), this._inverse);
        }
    }
}
