//---------------------------------------------------------------------
// <copyright file="CsdlSemanticsValueTerm.cs" company="Microsoft">
//      Copyright (C) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.
// </copyright>
//---------------------------------------------------------------------

using System;
using System.Collections.Generic;
using Microsoft.OData.Edm.Annotations;
using Microsoft.OData.Edm.Csdl.Parsing.Ast;

namespace Microsoft.OData.Edm.Csdl.CsdlSemantics
{
    /// <summary>
    /// Provides semantics for a CsdlTerm.
    /// </summary>
    [System.Diagnostics.DebuggerDisplay("CsdlSemanticsValueTerm({Name})")]
    internal class CsdlSemanticsValueTerm : CsdlSemanticsElement, IEdmValueTerm
    {
        protected readonly CsdlSemanticsSchema Context;
        protected CsdlTerm valueTerm;

        private readonly Cache<CsdlSemanticsValueTerm, IEdmTypeReference> typeCache = new Cache<CsdlSemanticsValueTerm, IEdmTypeReference>();
        private static readonly Func<CsdlSemanticsValueTerm, IEdmTypeReference> ComputeTypeFunc = (me) => me.ComputeType();

        public CsdlSemanticsValueTerm(CsdlSemanticsSchema context, CsdlTerm valueTerm)
            : base(valueTerm)
        {
            this.Context = context;
            this.valueTerm = valueTerm;
        }

        public string Name
        {
            get { return this.valueTerm.Name; }
        }

        public string Namespace
        {
            get { return this.Context.Namespace; }
        }

        public EdmSchemaElementKind SchemaElementKind
        {
            get { return EdmSchemaElementKind.ValueTerm; }
        }

        public EdmTermKind TermKind
        {
            get { return EdmTermKind.Value; }
        }

        public IEdmTypeReference Type
        {
            get { return this.typeCache.GetValue(this, ComputeTypeFunc, null); }
        }

        public string AppliesTo
        {
            get { return this.valueTerm.AppliesTo; }
        }

        public string DefaultValue
        {
            get { return this.valueTerm.DefaultValue; }
        }

        public override CsdlSemanticsModel Model
        {
            get { return this.Context.Model; }
        }

        public override CsdlElement Element
        {
            get { return this.valueTerm; }
        }

        protected override IEnumerable<IEdmVocabularyAnnotation> ComputeInlineVocabularyAnnotations()
        {
            return this.Model.WrapInlineVocabularyAnnotations(this, this.Context);
        }

        private IEdmTypeReference ComputeType()
        {
            return CsdlSemanticsModel.WrapTypeReference(this.Context, this.valueTerm.Type);
        }
    }
}
