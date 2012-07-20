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
using VDS.RDF.Parsing;
using VDS.RDF.Nodes;

namespace VDS.RDF.Query.Expressions
{
    /// <summary>
    /// Abstract base class for Unary Expressions
    /// </summary>
    public abstract class BaseUnaryExpression 
        : ISparqlExpression
    {
        /// <summary>
        /// The sub-expression of this Expression
        /// </summary>
        protected ISparqlExpression _expr;

        /// <summary>
        /// Creates a new Base Unary Expression
        /// </summary>
        /// <param name="expr">Expression</param>
        public BaseUnaryExpression(ISparqlExpression expr)
        {
            this._expr = expr;
        }

        /// <summary>
        /// Evaluates the expression
        /// </summary>
        /// <param name="context">Evaluuation Context</param>
        /// <param name="bindingID">Binding ID</param>
        /// <returns></returns>
        public abstract IValuedNode Evaluate(SparqlEvaluationContext context, int bindingID);

        /// <summary>
        /// Gets the String representation of the Expression
        /// </summary>
        /// <returns></returns>
        public abstract override string ToString();

        /// <summary>
        /// Gets an enumeration of all the Variables used in this expression
        /// </summary>
        public virtual IEnumerable<String> Variables
        {
            get
            {
                return this._expr.Variables;
            }
        }

        /// <summary>
        /// Gets the Type of the Expression
        /// </summary>
        public abstract SparqlExpressionType Type
        {
            get;
        }

        /// <summary>
        /// Gets the Functor of the Expression
        /// </summary>
        public abstract String Functor
        {
            get;
        }

        /// <summary>
        /// Gets the Arguments of the Expression
        /// </summary>
        public virtual IEnumerable<ISparqlExpression> Arguments
        {
            get
            {
                return this._expr.AsEnumerable();
            }
        }

        /// <summary>
        /// Gets whether an expression can safely be evaluated in parallel
        /// </summary>
        public virtual bool CanParallelise
        {
            get
            {
                return this._expr.CanParallelise;
            }
        }

        /// <summary>
        /// Transforms the arguments of the expression using the given transformer
        /// </summary>
        /// <param name="transformer">Expression Transformer</param>
        /// <returns></returns>
        public abstract ISparqlExpression Transform(IExpressionTransformer transformer);
    }

    /// <summary>
    /// Abstract base class for Binary Expressions
    /// </summary>
    public abstract class BaseBinaryExpression
        : ISparqlExpression
    {
        /// <summary>
        /// The sub-expressions of this Expression
        /// </summary>
        protected ISparqlExpression _leftExpr, _rightExpr;

        /// <summary>
        /// Creates a new Base Binary Expression
        /// </summary>
        /// <param name="leftExpr">Left Expression</param>
        /// <param name="rightExpr">Right Expression</param>
        public BaseBinaryExpression(ISparqlExpression leftExpr, ISparqlExpression rightExpr)
        {
            this._leftExpr = leftExpr;
            this._rightExpr = rightExpr;
        }

        /// <summary>
        /// Evaluates the expression
        /// </summary>
        /// <param name="context">Evaluation Context</param>
        /// <param name="bindingID">Binding ID</param>
        /// <returns></returns>
        public abstract IValuedNode Evaluate(SparqlEvaluationContext context, int bindingID);

        /// <summary>
        /// Gets the String representation of the Expression
        /// </summary>
        /// <returns></returns>
        public abstract override string ToString();

        /// <summary>
        /// Gets an enumeration of all the Variables used in this expression
        /// </summary>
        public virtual IEnumerable<String> Variables
        {
            get
            {
                return this._leftExpr.Variables.Concat(this._rightExpr.Variables);
            }
        }

        /// <summary>
        /// Gets the Type of the Expression
        /// </summary>
        public abstract SparqlExpressionType Type
        {
            get;
        }

        /// <summary>
        /// Gets the Functor of the Expression
        /// </summary>
        public abstract String Functor
        {
            get;
        }

        /// <summary>
        /// Gets the Arguments of the Expression
        /// </summary>
        public IEnumerable<ISparqlExpression> Arguments
        {
            get
            {
                return new ISparqlExpression[] { this._leftExpr, this._rightExpr };
            }
        }

        /// <summary>
        /// Gets whether an expression can safely be evaluated in parallel
        /// </summary>
        public virtual bool CanParallelise
        {
            get
            {
                return this._leftExpr.CanParallelise && this._rightExpr.CanParallelise;
            }
        }

        /// <summary>
        /// Transforms the arguments of the expression using the given transformer
        /// </summary>
        /// <param name="transformer">Expression Transformer</param>
        /// <returns></returns>
        public abstract ISparqlExpression Transform(IExpressionTransformer transformer);
    }
}