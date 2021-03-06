﻿//---------------------------------------------------------------------
// <copyright file="ODataJsonLightOutputContextTests.cs" company="Microsoft">
//      Copyright (C) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.
// </copyright>
//---------------------------------------------------------------------

namespace Microsoft.Test.OData.TDD.Tests.Writer.JsonLight
{
    using System;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Text;
    using FluentAssertions;
    using Microsoft.OData.Edm;
    using Microsoft.OData.Edm.Library;
    using Microsoft.OData.Core;
    using Microsoft.OData.Core.JsonLight;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class ODataJsonLightOutputContextTests
    {
        #region WriteProperty
        [TestMethod]
        public void ShouldBeAbleToWritePropertyRequestWithoutModel()
        {
            ODataProperty property = new ODataProperty {Name = "Prop", Value = Guid.Empty};
            WriteAndValidate(outputContext => outputContext.WriteProperty(property), "{\"@odata.context\":\"http://odata.org/test/$metadata#Edm.Guid\",\"@odata.type\":\"#Guid\",\"value\":\"00000000-0000-0000-0000-000000000000\"}", writingResponse: false);
        }

        [TestMethod]
        public void ShouldBeAbleToWritePropertyResponseWithoutModel()
        {
            ODataProperty property = new ODataProperty { Name = "Prop", Value = Guid.Empty };
            WriteAndValidate(outputContext => outputContext.WriteProperty(property), "{\"@odata.context\":\"http://odata.org/test/$metadata#Edm.Guid\",\"value\":\"00000000-0000-0000-0000-000000000000\"}", writingResponse: true);
        }

        [TestMethod]
        public void ShouldBeAbleToWritePropertyRequestWithoutModelAsync()
        {
            ODataProperty property = new ODataProperty { Name = "Prop", Value = Guid.Empty };
            WriteAndValidate(outputContext => outputContext.WritePropertyAsync(property).Wait(), "{\"@odata.context\":\"http://odata.org/test/$metadata#Edm.Guid\",\"@odata.type\":\"#Guid\",\"value\":\"00000000-0000-0000-0000-000000000000\"}", writingResponse: false, synchronous: false);
        }

        [TestMethod]
        public void ShouldBeAbleToWritePropertyResponseWithoutModelAsync()
        {
            ODataProperty property = new ODataProperty { Name = "Prop", Value = Guid.Empty };
            WriteAndValidate(outputContext => outputContext.WritePropertyAsync(property).Wait(), "{\"@odata.context\":\"http://odata.org/test/$metadata#Edm.Guid\",\"@odata.type\":\"#Guid\",\"value\":\"00000000-0000-0000-0000-000000000000\"}", writingResponse: false, synchronous: false);
        }

        [TestMethod]
        public void ShouldBeAbleToWriteInstanceAnnotationsInRequest()
        {
            ODataProperty property = new ODataProperty()
            {
                Name = "Prop",
                Value = Guid.Empty,
                InstanceAnnotations = new Collection<ODataInstanceAnnotation>
                {
                    new ODataInstanceAnnotation("Annotation.1", new ODataPrimitiveValue(true)),
                    new ODataInstanceAnnotation("Annotation.2", new ODataPrimitiveValue(123))
                }
            };
            WriteAndValidate(outputContext => outputContext.WriteProperty(property), "{\"@odata.context\":\"http://odata.org/test/$metadata#Edm.Guid\",\"@Annotation.1\":true,\"@Annotation.2\":123,\"@odata.type\":\"#Guid\",\"value\":\"00000000-0000-0000-0000-000000000000\"}", writingResponse: false);
        }

        [TestMethod]
        public void ShouldBeAbleToWriteInstanceAnnotationsInResponse()
        {
            ODataProperty property = new ODataProperty()
            {
                Name = "Prop",
                Value = Guid.Empty,
                InstanceAnnotations = new Collection<ODataInstanceAnnotation>
                {
                    new ODataInstanceAnnotation("Annotation.1", new ODataPrimitiveValue(true)),
                    new ODataInstanceAnnotation("Annotation.2", new ODataPrimitiveValue(123))
                }
            };
            WriteAndValidate(outputContext => outputContext.WriteProperty(property), "{\"@odata.context\":\"http://odata.org/test/$metadata#Edm.Guid\",\"@Annotation.1\":true,\"@Annotation.2\":123,\"value\":\"00000000-0000-0000-0000-000000000000\"}");
        }

        #endregion WriteProperty

        #region CreateFeedWriter
        [TestMethod]
        public void ShouldBeAbleToCreateFeedWriterForRequestWithoutModelAndWithoutSet()
        {
            WriteAndValidate(outputContext => outputContext.CreateODataFeedWriter(entitySet:null, entityType:null), "", writingResponse: false);
        }

        [TestMethod]
        public void ShouldBeAbleToCreateFeedWriterForResponseWithoutModelAndWithoutSet()
        {
            WriteAndValidate(outputContext => outputContext.CreateODataFeedWriter(entitySet: null, entityType: null), "", writingResponse: true);
        }

        [TestMethod]
        public void ShouldBeAbleToCreateFeedWriterAsyncForRequestWithoutModelAndWithoutSet()
        {
            WriteAndValidate(outputContext => outputContext.CreateODataFeedWriterAsync(entitySet: null, entityType: null).Result.Should().NotBeNull(), "", writingResponse: false, synchronous: false);
        }

        [TestMethod]
        public void ShouldBeAbleToCreateFeedWriterAsyncForResponseWithoutModelAndWithoutSet()
        {
            WriteAndValidate(outputContext => outputContext.CreateODataFeedWriterAsync(entitySet: null, entityType: null).Result.Should().NotBeNull(), "", writingResponse: true, synchronous: false);
        }
        #endregion CreateFeedWriter

        #region CreateEntryWriter
        [TestMethod]
        public void ShouldBeAbleToCreateEntryWriterForRequestWithoutModelAndWithoutSet()
        {
            WriteAndValidate(outputContext => outputContext.CreateODataEntryWriter(navigationSource: null, entityType: null), "", writingResponse: false);
        }

        [TestMethod]
        public void ShouldBeAbleToCreateEntryWriterForResponseWithoutModelAndWithoutSet()
        {
            WriteAndValidate(outputContext => outputContext.CreateODataEntryWriter(navigationSource: null, entityType: null), "", writingResponse: true);
        }

        [TestMethod]
        public void ShouldBeAbleToCreateEntryWriterAsyncForRequestWithoutModelAndWithoutSet()
        {
            WriteAndValidate(outputContext => outputContext.CreateODataEntryWriterAsync(navigationSource: null, entityType: null).Result.Should().NotBeNull(), "", writingResponse: false, synchronous: false);
        }

        [TestMethod]
        public void ShouldBeAbleToCreateEntryWriterAsyncForResponseWithoutModelAndWithoutSet()
        {
            WriteAndValidate(outputContext => outputContext.CreateODataEntryWriterAsync(navigationSource: null, entityType: null).Result.Should().NotBeNull(), "", writingResponse: true, synchronous: false);
        }
        #endregion CreateEntryWriter

        #region CreateCollectionWriter
        [TestMethod]
        public void ShouldBeAbleToCreateCollectionWriterForRequestWithoutModelAndWithoutItemType()
        {
            WriteAndValidate(outputContext => outputContext.CreateODataCollectionWriter(itemTypeReference: null), "", writingResponse: false);
        }

        [TestMethod]
        public void ShouldBeAbleToCreateCollectionWriterForResponseWithoutModelAndWithoutItemType()
        {
            WriteAndValidate(outputContext => outputContext.CreateODataCollectionWriter(itemTypeReference: null), "", writingResponse: true);
        }

        [TestMethod]
        public void ShouldBeAbleToCreateCollectionWriterAsyncForRequestWithoutModelAndWithoutItemType()
        {
            WriteAndValidate(outputContext => outputContext.CreateODataCollectionWriterAsync(itemTypeReference: null).Result.Should().NotBeNull(), "", writingResponse: false, synchronous: false);
        }

        [TestMethod]
        public void ShouldBeAbleToCreateCollectionWriterAsyncForResponseWithoutModelAndWithoutItemType()
        {
            WriteAndValidate(outputContext => outputContext.CreateODataCollectionWriterAsync(itemTypeReference: null).Result.Should().NotBeNull(), "", writingResponse: true, synchronous: false);
        }
        #endregion CreateCollectionWriter

        #region CreateParameterWriter
        [TestMethod]
        public void ShouldBeAbleToCreateParameterWriterForRequestWithoutModelAndWithoutFunction()
        {
            WriteAndValidate(outputContext => outputContext.CreateODataParameterWriter(operation: null), "", writingResponse: false);
        }

        [TestMethod]
        public void ShouldBeAbleToCreateParameterWriterForResponseWithoutModelAndWithoutFunction()
        {
            WriteAndValidate(outputContext => outputContext.CreateODataParameterWriter(operation: null), "", writingResponse: true);
        }

        [TestMethod]
        public void ShouldBeAbleToCreateParameterWriterAsyncForRequestWithoutModelAndWithoutFunction()
        {
            WriteAndValidate(outputContext => outputContext.CreateODataParameterWriterAsync(operation: null).Result.Should().NotBeNull(), "", writingResponse: false, synchronous: false);
        }

        [TestMethod]
        public void ShouldBeAbleToCreateParameterWriterAsyncForResponseWithoutModelAndWithoutFunction()
        {
            WriteAndValidate(outputContext => outputContext.CreateODataParameterWriterAsync(operation: null).Result.Should().NotBeNull(), "", writingResponse: true, synchronous: false);
        }
        #endregion CreateParameterWriter

        #region WriteServiceDocument
        [TestMethod]
        public void ShouldWriteServiceDocumentWithoutModel()
        {
            ODataServiceDocument serviceDocument = new ODataServiceDocument();
            serviceDocument.EntitySets = new ODataEntitySetInfo[] {new ODataEntitySetInfo {Name = "Customers", Url = new Uri("http://host/Customers")}};
            WriteAndValidate(outputContext => outputContext.WriteServiceDocument(serviceDocument), "{\"@odata.context\":\"http://odata.org/test/$metadata\",\"value\":[{\"name\":\"Customers\",\"kind\":\"EntitySet\",\"url\":\"http://host/Customers\"}]}");
        }

        [TestMethod]
        public void ShouldWriteServiceDocumentAsyncWithoutModel()
        {
            ODataServiceDocument serviceDocument = new ODataServiceDocument();
            serviceDocument.EntitySets = new ODataEntitySetInfo[] { new ODataEntitySetInfo { Name = "Customers", Url = new Uri("http://host/Customers") } };
            WriteAndValidate(outputContext => outputContext.WriteServiceDocumentAsync(serviceDocument).Wait(), "{\"@odata.context\":\"http://odata.org/test/$metadata\",\"value\":[{\"name\":\"Customers\",\"kind\":\"EntitySet\",\"url\":\"http://host/Customers\"}]}", writingResponse: true, synchronous: false);
        }
        #endregion WriteServiceDocument

        #region WriteEntityReferenceLink
        #region sync
        [TestMethod]
        public void ShouldWriteContextUriForEntityReferenceLinkRequest()
        {
            ODataEntityReferenceLink referenceLink = new ODataEntityReferenceLink { Url = new Uri("http://host/Customers(1)") };
            WriteAndValidate(outputContext => outputContext.WriteEntityReferenceLink(referenceLink), "{\"@odata.context\":\"http://odata.org/test/$metadata#$ref\",\"@odata.id\":\"http://host/Customers(1)\"}", writingResponse: false);
            WriteAndValidate(outputContext => outputContext.WriteEntityReferenceLink(referenceLink), "{\"@odata.context\":\"http://odata.org/test/$metadata#$ref\",\"@odata.id\":\"http://host/Customers(1)\"}", writingResponse: true);
        }
        #endregion sync

        #region async
        [TestMethod]
        public void AsyncShouldWriteContextUriForEntityReferenceLinkRequest()
        {
            ODataEntityReferenceLink referenceLink = new ODataEntityReferenceLink { Url = new Uri("http://host/Customers(1)") };
            WriteAndValidate(outputContext => outputContext.WriteEntityReferenceLinkAsync(referenceLink).Wait(), "{\"@odata.context\":\"http://odata.org/test/$metadata#$ref\",\"@odata.id\":\"http://host/Customers(1)\"}", writingResponse: false, synchronous: false);
            WriteAndValidate(outputContext => outputContext.WriteEntityReferenceLinkAsync(referenceLink).Wait(), "{\"@odata.context\":\"http://odata.org/test/$metadata#$ref\",\"@odata.id\":\"http://host/Customers(1)\"}", writingResponse: true, synchronous: false);
        }
        #endregion async
        #endregion WriteEntityReferenceLink

        #region WriteEntityReferenceLinks
        #region sync
        [TestMethod]
        public void ShouldWriteContextUriForEntityReferenceLinksRequest()
        {
            ODataEntityReferenceLink referenceLink = new ODataEntityReferenceLink { Url = new Uri("http://host/Orders(1)") };
            ODataEntityReferenceLinks referenceLinks = new ODataEntityReferenceLinks { Links = new[] { referenceLink } };
            WriteAndValidate(outputContext => outputContext.WriteEntityReferenceLinks(referenceLinks), "{\"@odata.context\":\"http://odata.org/test/$metadata#Collection($ref)\",\"value\":[{\"@odata.id\":\"http://host/Orders(1)\"}]}", writingResponse: false);
            WriteAndValidate(outputContext => outputContext.WriteEntityReferenceLinks(referenceLinks), "{\"@odata.context\":\"http://odata.org/test/$metadata#Collection($ref)\",\"value\":[{\"@odata.id\":\"http://host/Orders(1)\"}]}", writingResponse: true);
        }
        #endregion sync

        #region async
        [TestMethod]
        public void AsyncShouldWriteContextUriForEntityReferenceLinksRequest()
        {
            ODataEntityReferenceLink referenceLink = new ODataEntityReferenceLink { Url = new Uri("http://host/Orders(1)") };
            ODataEntityReferenceLinks referenceLinks = new ODataEntityReferenceLinks { Links = new[] { referenceLink } };
            WriteAndValidate(outputContext => outputContext.WriteEntityReferenceLinksAsync(referenceLinks).Wait(), "{\"@odata.context\":\"http://odata.org/test/$metadata#Collection($ref)\",\"value\":[{\"@odata.id\":\"http://host/Orders(1)\"}]}", writingResponse: false, synchronous: false);
            WriteAndValidate(outputContext => outputContext.WriteEntityReferenceLinksAsync(referenceLinks).Wait(), "{\"@odata.context\":\"http://odata.org/test/$metadata#Collection($ref)\",\"value\":[{\"@odata.id\":\"http://host/Orders(1)\"}]}", writingResponse: true, synchronous: false);
        }
        #endregion async
        #endregion WriteEntityReferenceLinks

        private static void WriteAndValidate(Action<ODataJsonLightOutputContext> test, string expectedPayload, bool writingResponse = true, bool synchronous = true)
        {
            MemoryStream stream = new MemoryStream();
            var outputContext = CreateJsonLightOutputContext(stream, writingResponse, synchronous);
            test(outputContext);
            ValidateWrittenPayload(stream, expectedPayload);
        }

        private static void ValidateWrittenPayload(MemoryStream stream, string expectedPayload)
        {
            stream.Seek(0, SeekOrigin.Begin);
            string payload = (new StreamReader(stream)).ReadToEnd();
            payload.Should().Be(expectedPayload);
        }

        private static ODataJsonLightOutputContext CreateJsonLightOutputContext(MemoryStream stream, bool writingResponse = true, bool synchronous = true)
        {
            ODataMessageWriterSettings settings = new ODataMessageWriterSettings { Version = ODataVersion.V4 };
            settings.SetServiceDocumentUri(new Uri("http://odata.org/test"));
            settings.ShouldIncludeAnnotation = ODataUtils.CreateAnnotationFilter("*");

            return new ODataJsonLightOutputContext(
                ODataFormat.Json,
                new NonDisposingStream(stream),
                new ODataMediaType("application", "json"),
                Encoding.UTF8,
                settings,
                writingResponse,
                synchronous,
                EdmCoreModel.Instance,
                /*urlResolver*/ null);
        }
    }
}
