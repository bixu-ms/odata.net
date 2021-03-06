﻿//---------------------------------------------------------------------
// <copyright file="SelectTreeNormalizer.cs" company="Microsoft">
//      Copyright (C) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.
// </copyright>
//---------------------------------------------------------------------

namespace Microsoft.OData.Core.UriParser.Parsers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.OData.Core.UriParser.Syntactic;
    using Microsoft.OData.Edm;
    using Microsoft.OData.Core.UriParser.Semantic;
    using ODataErrorStrings = Microsoft.OData.Core.Strings;

    /// <summary>
    /// Translate a select tree into the right format to be used with an expand tree.
    /// </summary>
    internal sealed class SelectTreeNormalizer
    {
        /// <summary>
        /// Normalize a SelectToken into something that can be used to trim an expand tree.
        /// </summary>
        /// <param name="treeToNormalize">The select token to normalize</param>
        /// <returns>Normalized SelectToken</returns>
        public SelectToken NormalizeSelectTree(SelectToken treeToNormalize)
        {
            PathReverser pathReverser = new PathReverser();
            List<PathSegmentToken> invertedPaths = (from property in treeToNormalize.Properties 
                                                    select property.Accept(pathReverser)).ToList();

            // to normalize a select token we just need to invert its paths, so that 
            // we match the ordering on an ExpandToken.
            return new SelectToken(invertedPaths);
        }
    }
}