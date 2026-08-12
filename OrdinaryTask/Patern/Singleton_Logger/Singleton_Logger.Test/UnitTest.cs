namespace Singleton_Logger.Test
{
    public class Test
    {
        [Fact]
        public void EqualInstanceTest()
        {
            Logger first = Logger.Instance;
            Logger second = Logger.Instance;
            Assert.Equal(first, second);
        }


        [Fact]
        public void MessageRecieved()
        {
            Logger log = Logger.Instance;
            log.ClearLogs();
            log.Log("test");
            var str = log.GetLogs();
            Assert.NotEmpty(str);
            Assert.Contains("test", str);



        }

        [Fact]
        public void NullList()
        {
            Logger log = Logger.Instance;
            log.ClearLogs();
            var exception = Record.Exception(() => log.PrintAllLogs());
            Assert.Null(exception);
        }
    }
}
