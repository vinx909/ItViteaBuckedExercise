using Buckets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace BucketsTest
{
    public class OilBarrelTest
    {
        [Fact]
        public void DefaultSizeTest()
        {
            //Arrange
            int defaultCapacity = 159;
            Container systemUnderTest = new OilBarrel();

            //Act

            //Assert
            Assert.Equal(defaultCapacity, systemUnderTest.Capacity);
        }
        [Fact]
        public void SetContentTest()
        {
            //Arrange
            int contentAmount = 4;
            Container systemUnderTest = new OilBarrel(contentAmount);
            //Act

            //Assert
            Assert.Equal(contentAmount, systemUnderTest.Content);
        }
        [Fact]
        public void SetNegativeContentTest()
        {
            //Arrange
            int contentAmount = -4;
            Action negativeContentTestAction = () => { new OilBarrel(contentAmount); };
            //Act

            //Assert
            Assert.ThrowsAny<Exception>(negativeContentTestAction);
        }

        [Fact]
        public void FillTest()
        {
            //Arrange
            int fillAmount = 4;
            Container systemUnderTest = new OilBarrel();

            //Act
            systemUnderTest.Fill(fillAmount);

            //Assert
            Assert.Equal(fillAmount, systemUnderTest.Content);
        }
        [Fact]
        public void FillNegativeNumberTest()
        {
            //Arrange
            int fillAmount = -4;
            Container systemUnderTest = new OilBarrel();
            Action negativeFillAmountTestAction = () => { systemUnderTest.Fill(fillAmount); };

            //Act

            //Assert
            Assert.ThrowsAny<Exception>(negativeFillAmountTestAction);
        }
        [Fact]
        public void FillFilledCalledTest()
        {
            //Arrange
            bool filledCalled = false;
            FilledDelagate filledDelagate = (container, eventArgs) => { filledCalled = true; };

            Container systemUnderTest = new OilBarrel();
            systemUnderTest.Filled += filledDelagate;

            int fillAmount = systemUnderTest.Capacity;

            //Act
            systemUnderTest.Fill(fillAmount);

            //Assert
            Assert.True(filledCalled);
        }
        [Fact]
        public void FillOverflowingNotCalledTest()
        {
            //Arrange
            bool OverflowingCalled = false;
            OverflowingDelagate overflowingDelagate = (container, actions, eventArgs) => { OverflowingCalled = true; };

            Container systemUnderTest = new OilBarrel();
            systemUnderTest.Overflowing += overflowingDelagate;

            int fillAmount = systemUnderTest.Capacity;

            //Act
            systemUnderTest.Fill(fillAmount);

            //Assert
            Assert.False(OverflowingCalled);
        }
        [Fact]
        public void FillOverflowedNotCalledTest()
        {
            //Arrange
            bool OverflowedCalled = false;
            OverflowedDelagate overflowedDelagate = (Container, actions, EventArgs) => { OverflowedCalled = true; };

            Container systemUnderTest = new OilBarrel();
            systemUnderTest.Overflowed += overflowedDelagate;

            int fillAmount = systemUnderTest.Capacity;

            //Act
            systemUnderTest.Fill(fillAmount);

            //Assert
            Assert.False(OverflowedCalled);
        }

        //the next three can easily fail as it's determained by the default overflow method
        [Fact]
        public void FillOverflowedFilledCalledTest()
        {
            //Arrange
            bool filledCalled = false;
            FilledDelagate filledDelagate = (container, eventArgs) => { filledCalled = true; };

            Container systemUnderTest = new OilBarrel();
            systemUnderTest.Filled += filledDelagate;

            int fillAmount = systemUnderTest.Capacity + 1;

            //Act
            systemUnderTest.Fill(fillAmount);

            //Assert
            Assert.True(filledCalled);
        }
        [Fact]
        public void FillOverflowedOverflowingCalledTest()
        {
            //Arrange
            bool OverflowingCalled = false;
            OverflowingDelagate overflowingDelagate = (container, actions, eventArgs) => { OverflowingCalled = true; };

            Container systemUnderTest = new OilBarrel();
            systemUnderTest.Overflowing += overflowingDelagate;

            int fillAmount = systemUnderTest.Capacity + 1;

            //Act
            systemUnderTest.Fill(fillAmount);

            //Assert
            Assert.True(OverflowingCalled);
        }
        [Fact]
        public void FillOverflowedOverflowedCalledTest()
        {
            //Arrange
            bool OverflowedCalled = false;
            OverflowedDelagate overflowedDelagate = (Container, actions, EventArgs) => { OverflowedCalled = true; };

            Container systemUnderTest = new OilBarrel();
            systemUnderTest.Overflowed += overflowedDelagate;

            int fillAmount = systemUnderTest.Capacity + 1;

            //Act
            systemUnderTest.Fill(fillAmount);

            //Assert
            Assert.True(OverflowedCalled);
        }

        [Fact]
        public void FillOverflowedOverflowingActionsChangeFilledCalledTest()
        {
            //Arrange
            List<bool> filledCalled = new();
            OverflowingDelagate overflowingDelagateSetup = (container, actions, eventArgs) =>
            {
                foreach (KeyValuePair<string, Action> action in actions)
                {
                    filledCalled.Add(false);
                }
            };

            Container systemUnderTest = new OilBarrel();
            systemUnderTest.Overflowing += overflowingDelagateSetup;
            systemUnderTest.Fill(systemUnderTest.Capacity + 1);

            //Act
            for (int i = 0; i < filledCalled.Count; i++)
            {
                OverflowingDelagate overflowingDelagate = (container, actions, eventArgs) =>
                {
                    int index = 0;
                    foreach (KeyValuePair<string, Action> action in actions)
                    {
                        if (index == i)
                        {
                            action.Value();
                            break;
                        }
                        index++;
                    }
                };
                FilledDelagate filledDelagate = (container, eventArgs) => { filledCalled[i] = true; };
                systemUnderTest = new OilBarrel();
                systemUnderTest.Filled += filledDelagate;
                systemUnderTest.Overflowing += overflowingDelagate;
                systemUnderTest.Fill(systemUnderTest.Capacity + 1);
            }

            //Assert
            bool sameResult = true;
            for (int i = 1; i < filledCalled.Count; i++)
            {
                sameResult = sameResult && filledCalled[i - 1] == filledCalled[i];
            }
            Assert.False(sameResult);
        }
        [Fact]
        public void FillOverflowedOverflowingActionsChangeOverflowedCalledTest()
        {
            //Arrange
            List<bool> overflowedCalled = new();
            OverflowingDelagate overflowingDelagateSetup = (container, actions, eventArgs) =>
            {
                foreach (KeyValuePair<string, Action> action in actions)
                {
                    overflowedCalled.Add(false);
                }
            };

            Container systemUnderTest = new OilBarrel();
            systemUnderTest.Overflowing += overflowingDelagateSetup;
            systemUnderTest.Fill(systemUnderTest.Capacity + 1);

            //Act
            for (int i = 0; i < overflowedCalled.Count; i++)
            {
                OverflowingDelagate overflowingDelagate = (container, actions, eventArgs) =>
                {
                    int index = 0;
                    foreach (KeyValuePair<string, Action> action in actions)
                    {
                        if (index == i)
                        {
                            action.Value();
                            break;
                        }
                        index++;
                    }
                };
                OverflowedDelagate filledDelagate = (container, amount, eventArgs) => { overflowedCalled[i] = true; };
                systemUnderTest = new OilBarrel();
                systemUnderTest.Overflowed += filledDelagate;
                systemUnderTest.Overflowing += overflowingDelagate;
                systemUnderTest.Fill(systemUnderTest.Capacity + 1);
            }

            //Assert
            bool sameResult = true;
            for (int i = 1; i < overflowedCalled.Count; i++)
            {
                sameResult = sameResult && overflowedCalled[i - 1] == overflowedCalled[i];
            }
            Assert.False(sameResult);
        }

        [Fact]
        public void EmptyTest()
        {
            //Arrange
            int content = 10;
            Container systemUnderTest = new OilBarrel(content);

            int emptiedContent = 0;

            //Act
            systemUnderTest.Empty();

            //Assert
            Assert.Equal(emptiedContent, systemUnderTest.Content);
        }
        [Fact]
        public void EmptyAmountTest()
        {
            //Arrange
            int content = 10;
            int emptyAmount = 8;
            Container systemUnderTest = new OilBarrel(content);

            int emptiedContent = content - emptyAmount;

            //Act
            systemUnderTest.Empty(emptyAmount);

            //Assert
            Assert.Equal(emptiedContent, systemUnderTest.Content);
        }
        [Fact]
        public void EmptyNegativeAmountTest()
        {
            //Arrange
            int content = 10;
            int emptyAmount = -8;
            Container systemUnderTest = new OilBarrel(content);
            Action negativeFillAmountTestAction = () => { systemUnderTest.Fill(emptyAmount); };

            //Act

            //Assert
            Assert.ThrowsAny<Exception>(negativeFillAmountTestAction);
        }
        [Fact]
        public void EmptyAmountMoreThenContentTest()
        {
            //Arrange
            int content = 10;
            int emptyAmount = 14;
            Container systemUnderTest = new OilBarrel(content);

            int emptiedContent = 0;

            //Act
            systemUnderTest.Empty(emptyAmount);

            //Assert
            Assert.Equal(emptiedContent, systemUnderTest.Content);
        }
    }
}
