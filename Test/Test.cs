namespace Dansnom.Test
{
    public class Test1 
    {
        public int Id { get; set; }
        protected string Name { get; set; }
        public string Email { get; set; }
    }


    public class Test2 : Test1
    {
       public static void MyTest()
       {
            Test2 test1 = new Test2
            {
                Id = 2,
                Name = "zaf",
                Email = "hyuewe"
            };
       }
    }   

} 