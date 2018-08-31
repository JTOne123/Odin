using System;
using Odin.Conventions;
using Odin.Tests.Lib;
using Shouldly;
using Xunit;

namespace Odin.Tests.Conventions
{
    public class SlashColonConventionTests
    {
        public SlashColonConventionTests()
        {
            this.Logger = new StringBuilderLogger();
            this.SubCommand = new SubCommand();
            this.Subject = new DefaultCommand(this.SubCommand);
            this.Subject
                .Use(this.Logger)
                .Use(new SlashColonConvention())
                ;
        }

        private StringBuilderLogger Logger { get; }

        private SubCommand SubCommand { get; }

        public DefaultCommand Subject { get; set; }

        #region ActionExecution

        [Fact]
        public void OnlyActionMethodsAreInterpretedAsActions()
        {
            // Given
            var args = new[] { "NotAnAction" };

            // When
            var result = this.Subject.GetAction(args);

            var exitCode = this.Subject.Execute(args);

            // Then
            result.ShouldNotBeNull();
            result.Name.ShouldBe("DoSomething");
            result.Parameters[0].Value.ShouldBe("NotAnAction");
            result.Parameters[1].Value.ShouldBe(Type.Missing);
            result.Parameters[2].Value.ShouldBe(Type.Missing);
        }

        [Fact]
        public void CanExecuteAMethodThatIsAnAction()
        {
            // Given
            var args = new[] { "DoSomething" };

            // When
            var result = this.Subject.GetAction(args);

            // Then
            result.ShouldNotBeNull();
            result.Parameters.Count.ShouldBe(3);
            result.Parameters.ShouldAllBe(pv=> pv.Value == Type.Missing);
        }

        [Fact]
        public void ReturnsResultFromAction()
        {
            // Given
            var args = new[] { "AlwaysReturnsMinus2" };

            // When
            var result = this.Subject.GetAction(args);

            // Then
            result.ShouldNotBeNull();
            result.Parameters.Count.ShouldBe(0);
        }

        #endregion

        #region Required arguments

        [Fact]
        public void WithRequiredStringArg()
        {
            var args = new[] { "WithRequiredStringArg", "/argument:value" };


            var result = this.Subject.GetAction(args);

            result.ShouldNotBeNull();
            result.Parameters.Count.ShouldBe(1);
            result.Parameters[0].Name.ShouldBe("argument");
            result.Parameters[0].Value.ShouldBe("value");
        }

        [Fact]
        public void WithMultipleRequiredStringArgs()
        {
            var args = new[] { "WithRequiredStringArgs", "/argument1:value1", "/argument2:value2"};

            var result = this.Subject.GetAction(args);

            result.ShouldNotBeNull();
            result.Parameters.Count.ShouldBe(2);
            result.Parameters[0].Name.ShouldBe("argument1");
            result.Parameters[0].Value.ShouldBe("value1");
            result.Parameters[1].Name.ShouldBe("argument2");
            result.Parameters[1].Value.ShouldBe("value2");
        }

        [Fact]
        public void CanMatchArgsByParameterOrder()
        {
            var args = new[] { "WithRequiredStringArgs", "value1", "value2" };

            var result = this.Subject.GetAction(args);

            result.ShouldNotBeNull();
            result.Parameters.Count.ShouldBe(2);
            result.Parameters[0].Name.ShouldBe("argument1");
            result.Parameters[0].Value.ShouldBe("value1");
            result.Parameters[1].Name.ShouldBe("argument2");
            result.Parameters[1].Value.ShouldBe("value2");
        }

        #endregion

        #region Switches

        [Fact]
        public void SwitchWithValue()
        {
            var args = new[] { "WithSwitch", "/argument:true"};

            var result = this.Subject.GetAction(args);

            result.ShouldNotBeNull();
            result.Parameters.Count.ShouldBe(1);
            result.Parameters[0].Name.ShouldBe("argument");
            result.Parameters[0].Value.ShouldBe(true);
        }

        [Fact]
        public void SwitchWithoutValue()
        {
            var args = new[] { "WithSwitch", "/argument" };

            var result = this.Subject.GetAction(args);

            result.ShouldNotBeNull();
            result.Parameters.Count.ShouldBe(1);
            result.Parameters[0].Name.ShouldBe("argument");
            result.Parameters[0].Value.ShouldBe(true);
        }

        [Fact]
        public void NegativeSwitch()
        {
            var args = new[] { "WithSwitch", "/no-argument" };

            var result = this.Subject.GetAction(args);

            result.ShouldNotBeNull();
            result.Parameters.Count.ShouldBe(1);
            result.Parameters[0].Name.ShouldBe("argument");
            result.Parameters[0].Value.ShouldBe(false);
        }

        [Fact]
        public void SwitchNotGiven()
        {
            var args = new[] { "WithSwitch" };

            var result = this.Subject.GetAction(args);

            result.ShouldNotBeNull();
            result.Parameters.Count.ShouldBe(1);
            result.Parameters[0].Name.ShouldBe("argument");
            result.Parameters[0].Value.ShouldBe(false);
        }

        #endregion

        #region Optional arguments

        [Fact]
        public void WithOptionalStringArg_DoNotPassIt()
        {
            var args = new[] { "WithOptionalStringArg" };

            var result = this.Subject.GetAction(args);

            result.ShouldNotBeNull();
            result.Parameters.Count.ShouldBe(1);
            result.Parameters[0].Name.ShouldBe("argument");
            result.Parameters[0].Value.ShouldBe(Type.Missing);
        }

        [Fact]
        public void WithOptionalStringArg_PassIt()
        {
            var args = new[] { "WithOptionalStringArg", "/argument:value1"};

            var result = this.Subject.GetAction(args);

            result.ShouldNotBeNull();
            result.Parameters.Count.ShouldBe(1);
            result.Parameters[0].Name.ShouldBe("argument");
            result.Parameters[0].Value.ShouldBe("value1");
        }

        [Fact]
        public void WithOptionalStringArgs_PassThemAll()
        {
            var args = new[] { "WithOptionalStringArgs", "/argument1:value1", "/argument2:value2", "/argument3:value3"};

            this.Subject.Execute(args);

            var result = this.Subject.GetAction(args);

            result.ShouldNotBeNull();
            result.Parameters.Count.ShouldBe(3);
            result.Parameters[0].Name.ShouldBe("argument1");
            result.Parameters[0].Value.ShouldBe("value1");
            result.Parameters[1].Name.ShouldBe("argument2");
            result.Parameters[1].Value.ShouldBe("value2");
            result.Parameters[2].Name.ShouldBe("argument3");
            result.Parameters[2].Value.ShouldBe("value3");
        }

        [Fact]
        public void WithOptionalStringArgs_PassHead()
        {
            var args = new[] { "WithOptionalStringArgs", "/argument1:value1"};

            var result = this.Subject.GetAction(args);

            result.ShouldNotBeNull();
            result.Parameters.Count.ShouldBe(3);
            result.Parameters[0].Name.ShouldBe("argument1");
            result.Parameters[0].Value.ShouldBe("value1");
            result.Parameters[1].Name.ShouldBe("argument2");
            result.Parameters[1].Value.ShouldBe(Type.Missing);
            result.Parameters[2].Name.ShouldBe("argument3");
            result.Parameters[2].Value.ShouldBe(Type.Missing);
        }

        [Fact]
        public void WithOptionalStringArgs_PassBody()
        {
            var args = new[] { "WithOptionalStringArgs", "/argument2:value2"};

            var result = this.Subject.GetAction(args);

            result.ShouldNotBeNull();
            result.Parameters.Count.ShouldBe(3);
            result.Parameters[0].Name.ShouldBe("argument1");
            result.Parameters[0].Value.ShouldBe(Type.Missing);
            result.Parameters[1].Name.ShouldBe("argument2");
            result.Parameters[1].Value.ShouldBe("value2");
            result.Parameters[2].Name.ShouldBe("argument3");
            result.Parameters[2].Value.ShouldBe(Type.Missing);
        }

        [Fact]
        public void WithOptionalStringArgs_PassNone()
        {
            var args = new[] { "WithOptionalStringArgs" };

            var result = this.Subject.GetAction(args);

            result.ShouldNotBeNull();
            result.Parameters.Count.ShouldBe(3);
            result.Parameters[0].Name.ShouldBe("argument1");
            result.Parameters[0].Value.ShouldBe(Type.Missing);
            result.Parameters[1].Name.ShouldBe("argument2");
            result.Parameters[1].Value.ShouldBe(Type.Missing);
            result.Parameters[2].Name.ShouldBe("argument3");
            result.Parameters[2].Value.ShouldBe(Type.Missing);
        }

        [Fact]
        public void WithOptionalStringArgs_PassTail()
        {
            var args = new[] { "WithOptionalStringArgs", "/argument3:value3"};

            var result = this.Subject.GetAction(args);

            result.ShouldNotBeNull();
            result.Parameters.Count.ShouldBe(3);
            result.Parameters[0].Name.ShouldBe("argument1");
            result.Parameters[0].Value.ShouldBe(Type.Missing);
            result.Parameters[1].Name.ShouldBe("argument2");
            result.Parameters[1].Value.ShouldBe(Type.Missing);
            result.Parameters[2].Name.ShouldBe("argument3");
            result.Parameters[2].Value.ShouldBe("value3");
        }

        #endregion

        #region SubCommands

        [Fact]
        public void ExecuteSubCommand()
        {
            var args = new[] { "Sub" };

            var result = this.Subject.GetAction(args);

            result.ShouldNotBeNull();
            result.Command.ShouldBe(this.SubCommand);
            result.Parameters.Count.ShouldBe(0);
        }

        #endregion
    }
}