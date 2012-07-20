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

namespace VDS.RDF.Query.Expressions.Functions.Sparql.Constructor
{
    /// <summary>
    /// Class representing the SPARQL IRI() function
    /// </summary>
    public class IriFunction
        : BaseUnaryExpression
    {
        /// <summary>
        /// Creates a new IRI() function expression
        /// </summary>
        /// <param name="expr">Expression to apply the function to</param>
        public IriFunction(ISparqlExpression expr)
            : base(expr) { }

        /// <summary>
        /// Returns the value of the Expression as evaluated for a given Binding as a Literal Node
        /// </summary>
        /// <param name="context">Evaluation Context</param>
        /// <param name="bindingID">Binding ID</param>
        /// <returns></returns>
        public override IValuedNode Evaluate(SparqlEvaluationContext context, int bindingID)
        {
            IValuedNode result = this._expr.Evaluate(context, bindingID);
            if (result == null)
            {
                throw new RdfQueryException("Cannot create an IRI from a null");
            }
            else
            {
                switch (result.NodeType)
                {
                    case NodeType.Literal:
                        ILiteralNode lit = (ILiteralNode)result;
                        string baseUri = string.Empty;
                        if (context.Query != null) baseUri = context.Query.BaseUri.ToSafeString();
                        string uri;
                        if (lit.DataType == null)
                        {
                            uri = Tools.ResolveUri(lit.Value, baseUri);
                            return new UriNode(null, UriFactory.Create(uri));
                        }
                        else
                        {
                            string dt = lit.DataType.AbsoluteUri;
                            if (dt.Equals(XmlSpecsHelper.XmlSchemaDataTypeString, StringComparison.Ordinal))
                            {
                                uri = Tools.ResolveUri(lit.Value, baseUri);
                                return new UriNode(null, UriFactory.Create(uri));
                            }
                            else
                            {
                                throw new RdfQueryException("Cannot create an IRI from a non-string typed literal");
                            }
                        }

                    case NodeType.Uri:
                        //Already a URI so nothing to do
                        return result;
                    default:
                        throw new RdfQueryException("Cannot create an IRI from a non-URI/String literal");
                }
            }
        }

        /// <summary>
        /// Gets the String representation of the function
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "IRI(" + this._expr.ToString() + ")";
        }

        /// <summary>
        /// Gets the Type of the Expression
        /// </summary>
        public override SparqlExpressionType Type
        {
            get
            {
                return SparqlExpressionType.Function;
            }
        }

        /// <summary>
        /// Gets the Functor of the Expression
        /// </summary>
        public override string Functor
        {
            get
            {
                return SparqlSpecsHelper.SparqlKeywordIri;
            }
        }

        /// <summary>
        /// Transforms the Expression using the given Transformer
        /// </summary>
        /// <param name="transformer">Expression Transformer</param>
        /// <returns></returns>
        public override ISparqlExpression Transform(IExpressionTransformer transformer)
        {
            return new IriFunction(transformer.Transform(this._expr));
        }
    }
}
