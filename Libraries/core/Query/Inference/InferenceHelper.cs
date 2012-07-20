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

namespace VDS.RDF.Query.Inference
{
    /*public static class InferenceHelper
    {
    }*/

    /// <summary>
    /// Helper class containing constants and methods for use in implementing OWL support
    /// </summary>
    public static class OwlHelper
    {
        /// <summary>
        /// Class containing Extraction Mode constants
        /// </summary>
        public static class OwlExtractMode
        {
            /// <summary>
            /// OWL Extraction Mode constants
            /// </summary>
            public const String DefaultStatements = "DefaultStatements",
                                AllClass = "AllClass",
                                AllIndividual = "AllIndividual",
                                AllProperty = "AllProperty",
                                AllStatements = "AllStatements",
                                AllStatementsIncludingJena = "AllStatementsIncludingJena",
                                ClassAssertion = "ClassAssertion",
                                ComplementOf = "ComplementOf",
                                DataPropertyAssertion = "DataPropertyAssertion",
                                DifferentIndividuals = "DifferentIndividuals",
                                DirectClassAssertion = "DirectClassAssertion",
                                DirectSubClassOf = "DirectSubClassOf",
                                DirectSubPropertyOf = "DirectSubPropertyOf",
                                DisjointClasses = "DisjointClasses",
                                DisjointProperties = "DisjointProperties",
                                EquivalentClasses = "EquivalentClasses",
                                EquivalentProperties = "EquivalentProperties",
                                InverseProperties = "InverserProperties",
                                ObjectPropertyAssertion = "ObjectPropertyAssertion",
                                PropertyAssertion = "PropertyAssertion",
                                SameIndividual = "SameIndividual",
                                SubClassOf = "SubClassOf",
                                SubPropertyOf = "SubPropertyOf";
        }

        /// <summary>
        /// OWL Class and Property Constants
        /// </summary>
        public const String OwlNothing = "http://www.w3.org/2002/07/owl#Nothing";
    }

}
