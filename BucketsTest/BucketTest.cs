using System;
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
            Container systemUnderTest = new Container();

            //Act

            //Assert
            Assert.Equal(defaultCapacity, systemUnderTest.Capacity);
        }
        [Fact]
        public void SetSizeTest()
        {
            //Arrange
            int setToCapacity = 14;
            Container systemUnderTest = new Container(setToCapacity);

            //Act

            //Assert
            Assert.Equal(setToCapacity, systemUnderTest.Capacity);
        }
        [Fact]
        public void MinimalSizeTest()
        {
            //Arrange
            int tooSmallCapacity = 9;
            Action tooSmallCapacityTestAction = () => { new Container(tooSmallCapacity); };
            //Act

            //Assert
            Assert.Throws<Exception>(tooSmallCapacityTestAction);
        }
        [Fact]
        public void SetContentTest()
        {
            //Arrange
            int capacity = 12;
            int contentAmount = 4;
            Container systemUnderTest = new Container(capacity, contentAmount);
            //Act

            //Assert
            Assert.Equal(contentAmount, systemUnderTest.Content);
        }
    }
}
