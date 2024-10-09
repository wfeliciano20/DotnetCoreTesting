namespace CRUDTests;

public class UnitTest1
{
    [Fact]
    public void Test1()
    {
        //Arrange
        int a = 0;
        int b = 1;
        int expected = 1;
        MyMath m = new MyMath();

        //Act
        int actual = m.Add(a, b);

        //Assert
        Assert.Equal(expected, actual);
    }
}