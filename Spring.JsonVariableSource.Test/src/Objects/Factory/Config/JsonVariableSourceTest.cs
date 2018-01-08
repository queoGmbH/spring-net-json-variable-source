using System;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Spring.Context.Support;
using Spring.Core.IO;
using Spring.JsonVariableSource.Test.Sample;
using Spring.Objects.Factory;
using Spring.Util;

namespace Spring.JsonVariableSource.Test.Objects.Factory.Config {
    [TestClass]
    public class JsonVariableSourceTest {
        [TestMethod]
        public void Test_Init_Spring_Context_With_JsonVariableSource_Should_Correctly_Resolve_Bean_When_JSon_File_Is_Valid() {
            /* Given: Spring-context with JsonVariableSource */
            ContextRegistry.RegisterContext(new XmlApplicationContext("..\\..\\Sample\\Valid\\sample.spring.config.xml"));

            /* When: Spring context is started */
            SampleSpringBean sampleSpringBean = ContextRegistry.GetContext().GetObject<SampleSpringBean>();

            /* Then: Bean should be resolved including properties */
            sampleSpringBean.Should().NotBeNull();
            sampleSpringBean.ApplicationName.Should().Be("Sample Application");
            sampleSpringBean.Login.Should().Be("username");
            sampleSpringBean.Password.Should().Be("top-secret123!");
        }

        [TestMethod]
        public void Test_Init_Spring_Context_With_JsonVariableSource_Should_Throw_Exception_When_JSon_File_Is_Invalid() {
            /* Given: Spring-config with invalid json-file as JsonVariableSource */
            string springConfigReferencingInvalidJsonVariableSource = "..\\..\\Sample\\Invalid\\sample.spring.config.xml";

            /* When: Spring context is started */
            Action action = () => ContextRegistry.RegisterContext(new XmlApplicationContext(springConfigReferencingInvalidJsonVariableSource));

            /* Then: exception should be thrown */
            action.ShouldThrow<ObjectCreationException>();
        }

        [TestMethod]
        public void Test_ResolveVariable_Should_Correctly_Resolve_Child_Property() {
            /* Given: the valid json file */
            IResource json = new FileSystemResource("..\\..\\Sample\\Valid\\Config\\sample.json");

            /* When: child variable should be resolved */
            string applicationName = new Spring.Objects.Factory.Config.JsonVariableSource(json).ResolveVariable("credentials.login");

            /* Then: login-name of credentials should be resolved correctly */
            applicationName.Should().Be("username");
        }

        [TestMethod]
        public void Test_ResolveVariable_Should_Correctly_Resolve_Parent_Property() {
            /* Given: the valid json file */
            IResource json = new FileSystemResource("..\\..\\Sample\\Valid\\Config\\sample.json");

            /* When: variable should be resolved */
            string applicationName = new Spring.Objects.Factory.Config.JsonVariableSource(json).ResolveVariable("applicationName");

            /* Then: applicationName should be resolved correctly */
            applicationName.Should().Be("Sample Application");
        }

        [TestMethod]
        public void Test_ResolveVariable_Should_Throw_Exception_When_JSon_Is_Invalid() {
            /* Given: the invalid json file */
            IResource json = new FileSystemResource("..\\..\\Sample\\Invalid\\Config\\sample.json");

            /* When: variable should be resolved */
            Action action = () => new Spring.Objects.Factory.Config.JsonVariableSource(json).ResolveVariable("applicationName");

            /* Then: an exception should be thrown */
            action.ShouldThrow<FormatException>();
        }
    }
}