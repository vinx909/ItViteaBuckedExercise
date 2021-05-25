using System;
using System.Collections.Generic;
using Buckets;
using Xunit;

namespace BucketsTest
{
    public class BucketTest
    {
        [Fact]
        public void DefaultSizeTest()
        {
            //Arrange
            int defaultCapacity = 12;
            Container systemUnderTest = new Bucket();

            //Act

            //Assert
            Assert.Equal(defaultCapacity, systemUnderTest.Capacity);
        }
        [Fact]
        public void SetSizeTest()
        {
            //Arrange
            int setToCapacity = 14;
            Container systemUnderTest = new Bucket(setToCapacity);

            //Act

            //Assert
            Assert.Equal(setToCapacity, systemUnderTest.Capacity);
        }
        [Fact]
        public void MinimalSizeTest()
        {
            //Arrange
            int tooSmallCapacity = 9;
            Action tooSmallCapacityTestAction = () => { new Bucket(tooSmallCapacity); };
            //Act

            //Assert
            Assert.ThrowsAny<Exception>(tooSmallCapacityTestAction);
        }
        [Fact]
        public void SetContentTest()
        {
            //Arrange
            int capacity = 12;
            int contentAmount = 4;
            Container systemUnderTest = new Bucket(capacity, contentAmount);
            //Act

            //Assert
            Assert.Equal(contentAmount, systemUnderTest.Content);
        }
        [Fact]
        public void SetNegativeContentTest()
        {
            //Arrange
            int capacity = 12;
            int contentAmount = -4;
            Action negativeContentTestAction = () => { new Bucket(capacity, contentAmount); };
            //Act

            //Assert
            Assert.ThrowsAny<Exception>(negativeContentTestAction);
        }

        [Fact]
        public void FillTest()
        {
            //Arrange
            int fillAmount = 4;
            Container systemUnderTest = new Bucket();

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
            Container systemUnderTest = new Bucket();
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
            
            Container systemUnderTest = new Bucket();
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

            Container systemUnderTest = new Bucket();
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

            Container systemUnderTest = new Bucket();
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

            Container systemUnderTest = new Bucket();
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

            Container systemUnderTest = new Bucket();
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

            Container systemUnderTest = new Bucket();
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

            Container systemUnderTest = new Bucket();
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
                        if(index == i)
                        {
                            action.Value();
                            break;
                        }
                        index++;
                    }
                };
                FilledDelagate filledDelagate = (container, eventArgs) => { filledCalled[i] = true; };
                systemUnderTest = new Bucket();
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

            Container systemUnderTest = new Bucket();
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
                systemUnderTest = new Bucket();
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
        public void FillWithBucketTest()
        {
            //Arrange
            int fillWithBucketCapacity = 20;
            int fillWithBucketContent = 4;
            Bucket fillWithBucket = new Bucket(fillWithBucketCapacity, fillWithBucketContent);

            int systemUnderTestCapacity = 10;
            Bucket systemUnderTest = new Bucket(systemUnderTestCapacity);

            //Act
            systemUnderTest.Fill(fillWithBucket);

            //Assert
            Assert.Equal(fillWithBucketContent, systemUnderTest.Content);
        }
        [Fact]
        public void FillWithBucketEmptiesOtherBucketTest()
        {
            //Arrange
            int fillWithBucketCapacity = 20;
            int fillWithBucketContent = 4;
            Bucket systemUnderTest = new Bucket(fillWithBucketCapacity, fillWithBucketContent);

            int systemUnderTestCapacity = 10;
            Bucket toFillBucket = new Bucket(systemUnderTestCapacity);

            int systemUnderTestExpectedContent = 0;

            //Act
            toFillBucket.Fill(systemUnderTest);

            //Assert
            Assert.Equal(systemUnderTestExpectedContent, systemUnderTest.Content);
        }
        [Fact]
        public void FillWithBucketWithItselfTest()
        {
            //Arrange
            Bucket systemUnderTest = new();
            Action fillWithBucketSelfTestAction = () => { systemUnderTest.Fill(systemUnderTest); };

            //Act

            //Assert
            Assert.ThrowsAny<Exception>(fillWithBucketSelfTestAction);
        }
        [Fact]
        public void FillWithBucketFilledCalledTest()
        {
            //Arrange
            bool filledCalled = false;
            FilledDelagate filledDelagate = (container, eventArgs) => { filledCalled = true; };

            int fillWithBucketCapacity = 20;
            int fillWithBucketContent = 15;
            Bucket fillWithBucket = new Bucket(fillWithBucketCapacity, fillWithBucketContent);

            int systemUnderTestCapacity = fillWithBucketContent;
            Bucket systemUnderTest = new Bucket(systemUnderTestCapacity);

            systemUnderTest.Filled += filledDelagate;

            //Act
            systemUnderTest.Fill(fillWithBucket);

            //Assert
            Assert.True(filledCalled);
        }
        [Fact]
        public void FillWithBucketOverflowingNotCalledTest()
        {
            //Arrange
            bool OverflowingCalled = false;
            OverflowingDelagate overflowingDelagate = (container, actions, eventArgs) => { OverflowingCalled = true; };

            int fillWithBucketCapacity = 20;
            int fillWithBucketContent = 15;
            Bucket fillWithBucket = new Bucket(fillWithBucketCapacity, fillWithBucketContent);

            int systemUnderTestCapacity = fillWithBucketContent;
            Bucket systemUnderTest = new Bucket(systemUnderTestCapacity);

            systemUnderTest.Overflowing += overflowingDelagate;

            //Act
            systemUnderTest.Fill(fillWithBucket);

            //Assert
            Assert.False(OverflowingCalled);
        }
        [Fact]
        public void FillWithBucketOverflowedNotCalledTest()
        {
            //Arrange
            bool OverflowedCalled = false;
            OverflowedDelagate overflowedDelagate = (Container, actions, EventArgs) => { OverflowedCalled = true; };

            int fillWithBucketCapacity = 20;
            int fillWithBucketContent = 15;
            Bucket fillWithBucket = new Bucket(fillWithBucketCapacity, fillWithBucketContent);

            int systemUnderTestCapacity = fillWithBucketContent;
            Bucket systemUnderTest = new Bucket(systemUnderTestCapacity);

            systemUnderTest.Overflowed += overflowedDelagate;

            //Act
            systemUnderTest.Fill(fillWithBucket);

            //Assert
            Assert.False(OverflowedCalled);
        }

        //the next three can easily fail as it's determained by the default overflow method
        [Fact]
        public void FillWithBucketOverflowedFilledCalledTest()
        {
            //Arrange
            bool filledCalled = false;
            FilledDelagate filledDelagate = (container, eventArgs) => { filledCalled = true; };

            int fillWithBucketCapacity = 20;
            int fillWithBucketContent = 15;
            Bucket fillWithBucket = new Bucket(fillWithBucketCapacity, fillWithBucketContent);

            int systemUnderTestCapacity = fillWithBucketContent - 1;
            Bucket systemUnderTest = new Bucket(systemUnderTestCapacity);

            systemUnderTest.Filled += filledDelagate;

            //Act
            systemUnderTest.Fill(fillWithBucket);

            //Assert
            Assert.True(filledCalled);
        }
        [Fact]
        public void FillWithBucketOverflowedOverflowingCalledTest()
        {
            //Arrange
            bool OverflowingCalled = false;
            OverflowingDelagate overflowingDelagate = (container, actions, eventArgs) => { OverflowingCalled = true; };

            int fillWithBucketCapacity = 20;
            int fillWithBucketContent = 15;
            Bucket fillWithBucket = new Bucket(fillWithBucketCapacity, fillWithBucketContent);

            int systemUnderTestCapacity = fillWithBucketContent - 1;
            Bucket systemUnderTest = new Bucket(systemUnderTestCapacity);

            systemUnderTest.Overflowing += overflowingDelagate;

            //Act
            systemUnderTest.Fill(fillWithBucket);

            //Assert
            Assert.True(OverflowingCalled);
        }
        [Fact]
        public void FillWithBucketOverflowedOverflowedCalledTest()
        {
            //Arrange
            bool OverflowedCalled = false;
            OverflowedDelagate overflowedDelagate = (Container, actions, EventArgs) => { OverflowedCalled = true; };

            int fillWithBucketCapacity = 20;
            int fillWithBucketContent = 15;
            Bucket fillWithBucket = new Bucket(fillWithBucketCapacity, fillWithBucketContent);

            int systemUnderTestCapacity = fillWithBucketContent - 1;
            Bucket systemUnderTest = new Bucket(systemUnderTestCapacity);
            systemUnderTest.Overflowed += overflowedDelagate;

            //Act
            systemUnderTest.Fill(fillWithBucket);

            //Assert
            Assert.True(OverflowedCalled);
        }

        [Fact]
        public void FillWithBucketOverflowedOverflowingActionsChangeFilledCalledTest()
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

            int fillWithBucketCapacity = 20;
            int fillWithBucketContent = 15;
            Bucket fillWithBucket = new Bucket(fillWithBucketCapacity, fillWithBucketContent);

            int systemUnderTestCapacity = fillWithBucketContent - 1;
            Bucket systemUnderTest = new Bucket(systemUnderTestCapacity);

            systemUnderTest.Overflowing += overflowingDelagateSetup;
            systemUnderTest.Fill(fillWithBucket);

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
                
                fillWithBucket = new Bucket(fillWithBucketCapacity, fillWithBucketContent);
                systemUnderTest = new Bucket(systemUnderTestCapacity);
                
                systemUnderTest.Filled += filledDelagate;
                systemUnderTest.Overflowing += overflowingDelagate;
                systemUnderTest.Fill(fillWithBucket);
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
        public void FillWithBucketOverflowedOverflowingActionsChangeOverflowedCalledTest()
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

            int fillWithBucketCapacity = 20;
            int fillWithBucketContent = 15;
            Bucket fillWithBucket = new Bucket(fillWithBucketCapacity, fillWithBucketContent);

            int systemUnderTestCapacity = fillWithBucketContent - 1;
            Bucket systemUnderTest = new Bucket(systemUnderTestCapacity);

            systemUnderTest.Overflowing += overflowingDelagateSetup;
            systemUnderTest.Fill(fillWithBucket);

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
                
                fillWithBucket = new Bucket(fillWithBucketCapacity, fillWithBucketContent);
                systemUnderTest = new Bucket(systemUnderTestCapacity);

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
            int capacity = 12;
            int content = 10;
            Container systemUnderTest = new Bucket(capacity, content);

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
            int capacity = 12;
            int content = 10;
            int emptyAmount = 8;
            Container systemUnderTest = new Bucket(capacity, content);

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
            int capacity = 12;
            int content = 10;
            int emptyAmount = -8;
            Container systemUnderTest = new Bucket(capacity, content);
            Action negativeFillAmountTestAction = () => { systemUnderTest.Fill(emptyAmount); };

            //Act

            //Assert
            Assert.ThrowsAny<Exception>(negativeFillAmountTestAction);
        }
        [Fact]
        public void EmptyAmountMoreThenContentTest()
        {
            //Arrange
            int capacity = 12;
            int content = 10;
            int emptyAmount = 14;
            Container systemUnderTest = new Bucket(capacity, content);

            int emptiedContent = 0;

            //Act
            systemUnderTest.Empty(emptyAmount);

            //Assert
            Assert.Equal(emptiedContent, systemUnderTest.Content);
        }
    }
}
