namespace Time_Table_Arranging_Program.Interfaces {
    /// <summary>
    ///     Classes that implement this interface should implement a method that
    ///     Return a string that represent the construction of an instance
    ///     E.g. I have a class named Person, its constructor is defined so : Person(string name, int age).
    ///     So if I have a Person object with name of "John", and age of 10,
    ///     then its ConstructionString should return @"new Person("John", 10)"
    /// </summary>
    public interface IToConstructionString {
        /// <summary>
        ///     Return a string that represent the construction of an instance
        /// </summary>
        /// <param name="terminationSymbol">The symbol of termination for the construction string, by default is semicolon</param>
        /// <returns>Example of return is : @"new Person("John",10)"</returns>
        string ToConstructionString();
    }
}